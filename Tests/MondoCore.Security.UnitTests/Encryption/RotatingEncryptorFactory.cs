using System;
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
    public class RotatingEncryptorFactoryTests
    {
        [TestMethod]
        public async Task RotatingEncryptorFactory_GetValidForDecryption()
        {
            var keyFactory    = new Mock<IKeyFactory>();
            var cache         = new Mock<ICache>();
            var encrFactory   = new RotatingEncryptorFactory(new SymmetricEncryptorFactory(), cache.Object, keyFactory.Object);
            var policy        = new EncryptionPolicy();
            var idPolicy      = policy.Id;
            var key           = new Key(policy);

            keyFactory.Setup( f=> f.GetDecryptionKey(idPolicy)).ReturnsAsync( key );

            var result = await encrFactory.GetValidForDecryption(idPolicy);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task RotatingEncryptorFactory_GetValidForEncryption()
        {
            var keyFactory    = new Mock<IKeyFactory>();
            var cache         = new Mock<ICache>();
            var encrFactory   = new RotatingEncryptorFactory(new SymmetricEncryptorFactory(), cache.Object, keyFactory.Object);
            var policy        = new EncryptionPolicy();
            var idPolicy      = policy.Id;
            var key           = new Key(policy);

            keyFactory.Setup( f=> f.GetEncryptionKey()).ReturnsAsync( key );

            var result = await encrFactory.GetValidForEncryption();

            Assert.IsNotNull(result);
            Assert.AreEqual(key.Id, (result as IEncryptor).Policy.Id);
        }
    }
}
