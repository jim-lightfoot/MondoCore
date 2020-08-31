using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MondoCore.Common;
using MondoCore.Security.Encryption;

namespace MondoCore.Security.Encryption.UnitTests
{
    [TestClass]
    [TestCategory("Unit Tests")]
    public class SymmetricEncryptorTests
    {
        private const string data = "This once recognized, our attention naturally turns to the question of how small the payment can be. We know that by means of a wheel and axle we can raise 1,000 kilogrammes through 1 metre by allowing 100 kilogrammes to fall 10 metres.";

        [TestMethod]
        public async Task SymmetricEncryptor_EncryptDecrypt_stream()
        {
            var encr = new SymmetricEncryptor(new Key(new EncryptionPolicy()));

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
    
        private async Task TestEncryptDecrypt(IEncryptor encr, Stream input, string inputData)
        {
            using(var encrOutput = new MemoryStream())
            { 
                await encr.Encrypt(input, encrOutput);

                var outputData = await encrOutput.ReadStringAsync();

                Assert.IsFalse(string.IsNullOrEmpty(outputData));
                Assert.AreNotEqual(inputData, outputData);
                Assert.IsTrue(encr.Policy.IsReadOnly);

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
