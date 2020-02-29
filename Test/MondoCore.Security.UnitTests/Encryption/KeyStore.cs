using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MondoCore.Common;
using MondoCore.Security.Encryption;

using Newtonsoft.Json;

using Moq;

namespace MondoCore.Security.Encryption.UnitTests
{
    [TestClass]
    public class KeyStoreTests
    {
        [TestMethod]
        public async Task KeyStore_Put()
        {
            var store       = new Mock<IBlobStore>();
            var policy      = new EncryptionPolicy();
            var idPolicy    = policy.Id;
            var keyStore    = new KeyStore(store.Object, new PassThruEncryptor());
            var key         = new Key(policy);
            var cbResult    = "";

            store.Setup( s=> s.Put(It.IsAny<string>(), It.IsAny<string>())).Callback<string, string>( (id, data)=> { cbResult = data; } );

            await keyStore.Add(key);

            Assert.IsFalse(string.IsNullOrWhiteSpace(cbResult));
        }     
        
        [TestMethod]
        public async Task KeyStore_Get()
        {
            var store       = new Mock<IBlobStore>();
            var policy      = new EncryptionPolicy();
            var idPolicy    = policy.Id;
            var keyStore    = new KeyStore(store.Object, new PassThruEncryptor());
            var key         = new Key(policy);
            var cbResult    = "";

            store.Setup( s=> s.Put(It.IsAny<string>(), It.IsAny<string>())).Callback<string, string>( (id, data)=> { cbResult = data; } );

            await keyStore.Add(key);

            var store2 = new Mock<IBlobStore>();
            keyStore    = new KeyStore(store2.Object, new PassThruEncryptor());

            store2.Setup( s=> s.Get(It.IsAny<string>(), It.IsAny<Encoding>())).ReturnsAsync(cbResult);

            var result = await keyStore.Get(idPolicy);

            Assert.AreEqual(policy.Id,        result.Id);
            Assert.AreEqual(policy.Id,        result.Policy.Id);
            Assert.AreEqual(policy.KeySize,   result.Policy.KeySize);
            Assert.AreEqual(policy.BlockSize, result.Policy.BlockSize);
            Assert.AreEqual(policy.Padding,   result.Policy.Padding);
            Assert.AreEqual(policy.Mode,      result.Policy.Mode);
            Assert.AreEqual(policy.Algorithm, result.Policy.Algorithm);
            Assert.AreEqual(policy.Expires,   result.Policy.Expires);
        }
    }
}
