using System;
using System.Collections.Generic;
using System.IO;
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

        public EncryptionViewModel()
        {
            _encryptorCache = new MemoryCache();

            var docFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var keyFactory  = new KeyFactory(new KeyStore(new FileForKeyStore(Path.Combine(docFolder, @"MondoCore\EncryptionSample\Keys\Decrypt")), new PassThruEncryptor()), 
                                             new KeyStore(new FileForKeyStore(Path.Combine(docFolder, @"MondoCore\EncryptionSample\Keys\Encrypt")), new PassThruEncryptor()), 
                                             new EncryptionPolicy(), 
                                             new TimeSpan(0, 5, 0));

            this.Encryptor  = new RotatingKeyEncryptor(new SymmetricEncryptorFactory(_encryptorCache, keyFactory));
        }

        public IEncryptor Encryptor { get; }
    }
}
