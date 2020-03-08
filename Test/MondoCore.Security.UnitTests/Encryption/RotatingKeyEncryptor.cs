using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MondoCore.Common;
using MondoCore.Security.Encryption;

using Moq;

namespace MondoCore.Security.Encryption.UnitTests
{
    [TestClass]
    [TestCategory("Unit Tests")]
    public class RotatingKeyEncryptorTests
    {
        private const string DataToEncrypt1 = "Bob's your uncle";
        private const string DataToEncrypt2 = "普通话/普通話";
        private const string DataToEncrypt3 = "أحب القراءة كثيرا";
        private const string DataToEncrypt4 = "К нам в око́шко застучи́т";
       
        private const string data = "This once recognized, our attention naturally turns to the question of how small the payment can be. We know that by means of a wheel and axle we can raise 1,000 kilogrammes through 1 metre by allowing 100 kilogrammes to fall 10 metres.";

        [TestMethod]
        public async Task RotatingKeyEncryptor_EncryptDecrypt_bytes()
        {
            var encr1   = new SymmetricEncryptor(new Key(new EncryptionPolicy()));
            var factory = new Mock<IRotatingEncryptorFactory>();
            var encr    = new RotatingKeyEncryptor(factory.Object);

            factory.Setup( (f)=> f.GetValidForEncryption() ).ReturnsAsync(encr1);
            factory.Setup( (f)=> f.GetValidForDecryption( encr1.Policy.Id ) ).ReturnsAsync(encr1);

            await TestEncryptDecrypt(encr, DataToEncrypt1);
            await TestEncryptDecrypt(encr, DataToEncrypt2);
            await TestEncryptDecrypt(encr, DataToEncrypt3);
            await TestEncryptDecrypt(encr, DataToEncrypt4);
        }
    
        [TestMethod]
        public async Task RotatingKeyEncryptor_EncryptDecrypt_stream()
        {
            var encr1   = new SymmetricEncryptor(new Key(new EncryptionPolicy()));
            var factory = new Mock<IRotatingEncryptorFactory>();
            var encr    = new RotatingKeyEncryptor(factory.Object);

            factory.Setup( (f)=> f.GetValidForEncryption() ).ReturnsAsync(encr1);
            factory.Setup( (f)=> f.GetValidForDecryption( encr1.Policy.Id ) ).ReturnsAsync(encr1);

            var input = new MemoryStream();
            var sb = new StringBuilder();

            // Initialize input with some data (> buffer size which is 64k)
            for(var i = 0; i < 600; ++i)
            { 
                await input.WriteAsync(data);
                sb.Append(data);
            }

            input.Seek(0, SeekOrigin.Begin);

            await TestEncryptDecrypt(encr, input, sb.ToString());
        }
    
        private async Task TestEncryptDecrypt(IEncryptor encr, string data)
        {
            var result = await encr.Encrypt(data);

            Assert.IsFalse(string.IsNullOrEmpty(result));
            Assert.AreNotEqual(data, result);

            var original = await encr.Decrypt(result);

            Assert.AreEqual(data, original);
        }
            
        private async Task TestEncryptDecrypt(IEncryptor encr, Stream input, string inputData)
        {
            using(var encrOutput = new MemoryStream())
            { 
                await encr.Encrypt(input, encrOutput);

                var outputData = await encrOutput.ReadStringAsync();

                Assert.IsFalse(string.IsNullOrEmpty(outputData));
                Assert.AreNotEqual(inputData, outputData);

                using(var decrOutput = new MemoryStream())
                {
                    await encr.Decrypt(encrOutput, decrOutput);

                    var decrResult = await decrOutput.ReadStringAsync();

                    Assert.AreEqual(inputData, decrResult);
                }
            }
        }

    }
}
