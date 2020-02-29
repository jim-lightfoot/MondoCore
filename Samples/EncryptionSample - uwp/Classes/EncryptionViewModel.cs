using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MondoCore.Common;
using MondoCore.Security.Encryption;
    
namespace EncryptionSample
{
    public class EncryptionViewModel
    {
        private readonly ICache _encryptorCache;
        private readonly ICache _policyCache;

        public EncryptionViewModel()
        {
            _encryptorCache = new MemoryCache();
            _policyCache    = new MemoryCache();

            var policyFactory = new PolicyFactory(_policyCache, new PolicyStore(new FileStore(@"c:\\Documents\Testing\EncryptionSample\Policies")), new EncryptionPolicy(), new TimeSpan(0, 5, 0));
            var keyFactory    = new KeyFactory(new BlobKeyStore(new FileStore(@"c:\\Documents\Testing\EncryptionSample\Keys")));

            this.Encryptor  = new MultiPolicyEncryptor(new SymmetricEncryptorFactory(_encryptorCache, policyFactory, keyFactory));
        }

        public IEncryptor Encryptor { get; private set; }
    }

    /// <summary>
    /// DO NOT USE THIS IN PRODUCTION. This is not secure!!!
    /// </summary>
    internal class BlobKeyStore : IKeyStore
    {
        private readonly IBlobStore _blobStore;

        internal BlobKeyStore(IBlobStore blobStore)
        {
            _blobStore = blobStore;
        }

        public async Task Add(IKey key)
        {
            await _blobStore.Put(key.Id.ToId() + ".key", Convert.ToBase64String(key.ToArray()));
        }

        public async Task<IKey> Get(Guid id)
        {
            var encoded = await _blobStore.Get(id.ToId() + ".key");
            var bytes   = Convert.FromBase64String(encoded);
            var key     = new Key(new EncryptionPolicy(), bytes);

            Array.Clear(bytes, 0, bytes.Length);

            return key;
        }
    }
}
