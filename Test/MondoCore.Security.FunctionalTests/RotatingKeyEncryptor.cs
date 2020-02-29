using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MondoCore.Common;
using MondoCore.Security.Encryption;

namespace MondoCore.Security.FunctionalTests
{
    [TestClass]
    [TestCategory("Functional Tests")]
    public class RotatingKeyEncryptorTest
    {
        private MemoryStore _decryptStore = new MemoryStore();
        private MemoryStore _encryptStore = new MemoryStore();

        [TestMethod]
        public async Task RotatingKeyEncryptor_EncryptDecrypt()
        {
            _decryptStore.Clear();
            _encryptStore.Clear();

            var encryptor  = CreateEncryptor();
            var cipherText = await encryptor.Encrypt("bob");

            Assert.IsFalse(string.IsNullOrWhiteSpace(cipherText));
            Assert.AreNotEqual("bob", cipherText);

            var decrypted = await encryptor.Decrypt(cipherText);

            Assert.AreEqual("bob", decrypted);

            Assert.AreEqual(1, (await _decryptStore.Find("")).Count());
            Assert.AreEqual(1, (await _encryptStore.Find("")).Count());
        }

        [TestMethod]
        public async Task RotatingKeyEncryptor_EncryptDecrypt_2times()
        {
            _decryptStore.Clear();
            _encryptStore.Clear();

            var encryptor  = CreateEncryptor();
            var cipherText = await encryptor.Encrypt("bob");
            var cipherText2 = await encryptor.Encrypt("fred");

            Assert.IsFalse(string.IsNullOrWhiteSpace(cipherText));
            Assert.AreNotEqual("bob", cipherText);
            Assert.IsFalse(string.IsNullOrWhiteSpace(cipherText2));
            Assert.AreNotEqual("fred", cipherText2);

            var decrypted = await encryptor.Decrypt(cipherText);
            var decrypted2 = await encryptor.Decrypt(cipherText2);

            Assert.AreEqual("bob", decrypted);
            Assert.AreEqual("fred", decrypted2);

            Assert.AreEqual(1, (await _decryptStore.Find("")).Count());
            Assert.AreEqual(1, (await _encryptStore.Find("")).Count());
        }

        [TestMethod]
        public async Task RotatingKeyEncryptor_EncryptDecrypt_differentcontexts()
        {
            _decryptStore.Clear();
            _encryptStore.Clear();

            var encryptor  = CreateEncryptor();
            var cipherText = await encryptor.Encrypt("bob");

            Assert.IsFalse(string.IsNullOrWhiteSpace(cipherText));
            Assert.AreNotEqual("bob", cipherText);

            var encryptor2  = CreateEncryptor();
            var decrypted = await encryptor2.Decrypt(cipherText);

            Assert.AreEqual("bob", decrypted);

            Assert.AreEqual(1, (await _decryptStore.Find("")).Count());
            Assert.AreEqual(1, (await _encryptStore.Find("")).Count());
        }

        private IEncryptor CreateEncryptor()
        {
            var encryptorCache = new MemoryCache();
            var keyFactory     = new KeyFactory(new KeyStore(_decryptStore, new PassThruEncryptor()),
                                                new KeyStore(_encryptStore, new PassThruEncryptor()),
                                                new EncryptionPolicy(), 
                                                new TimeSpan(1, 0, 0));

            return new RotatingKeyEncryptor(new SymmetricEncryptorFactory(encryptorCache, keyFactory));
        }
    }
}
