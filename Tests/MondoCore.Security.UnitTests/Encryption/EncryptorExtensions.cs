using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MondoCore.Common;
using MondoCore.Security.Encryption;

namespace MondoCore.Security.Encryption.UnitTests
{
    [TestClass]
    [TestCategory("Unit Tests")]
    public class EncryptorExtensionsTests
    {
        private const string DataToEncrypt1 = "Bob's your uncle";
        private const string DataToEncrypt2 = "普通话/普通話";
        private const string DataToEncrypt3 = "أحب القراءة كثيرا";
        private const string DataToEncrypt4 = "К нам в око́шко застучи́т";

        [TestMethod]
        public async Task EncryptorExtensions_EncryptDecrypt()
        {
            var encr = new SymmetricEncryptor(new Key(new EncryptionPolicy()));

            await TestEncryptDecrypt(encr, DataToEncrypt1);
            await TestEncryptDecrypt(encr, DataToEncrypt2);
            await TestEncryptDecrypt(encr, DataToEncrypt3);
            await TestEncryptDecrypt(encr, DataToEncrypt4);
        }


        [TestMethod]
        public async Task EncryptorExtensions_EncryptDecrypt_expired()
        {
            var encr = new SymmetricEncryptor(new Key(new EncryptionPolicy { Expires = DateTime.UtcNow.AddDays(-4) } ));

            await Assert.ThrowsExceptionAsync<ExpiredPolicyException>( async ()=>
            { 
                await TestEncryptDecrypt(encr, DataToEncrypt1);
            },
            "Policy has expired and should throw an exception");
            
        }

        private async Task TestEncryptDecrypt(IEncryptor encr, string data)
        {
            var result = await encr.Encrypt(data);

            Assert.IsFalse(string.IsNullOrEmpty(result));
            Assert.AreNotEqual(data, result);

            var original = await encr.Decrypt(result);

            Assert.AreEqual(data, original);
        }
    }
}
