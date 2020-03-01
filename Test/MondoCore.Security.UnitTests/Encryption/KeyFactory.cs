using System;
using System.Collections.Generic;

using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MondoCore.Common;
using MondoCore.Security.Encryption;

using Moq;

namespace MondoCore.Security.Encryption.UnitTests
{
    [TestClass]
    [TestCategory("Unit Tests")]
    public class KeyFactoryTests
    {
        [TestMethod]
        public async Task KeyFactory_GetDecryptionKey_notinstore()
        {
            var eStore     = new Mock<IKeyStore>();
            var dStore     = new Mock<IKeyStore>();
            var policy     = new EncryptionPolicy();
            var idPolicy   = policy.Id;
            var keyFactory = new KeyFactory(dStore.Object, eStore.Object, policy, new TimeSpan(0, 5, 0));

            await Assert.ThrowsExceptionAsync<KeyNotFoundException>( async ()=> { await keyFactory.GetDecryptionKey(idPolicy); });
        }

        [TestMethod]
        public async Task KeyFactory_GetDecryptionKey_instore()
        {
            var eStore     = new Mock<IKeyStore>();
            var dStore     = new Mock<IKeyStore>();
            var policy     = new EncryptionPolicy();
            var keyFactory = new KeyFactory(dStore.Object, eStore.Object, policy, new TimeSpan(0, 5, 0));
            var key        = new Key(policy);
            var idPolicy   = policy.Id;

            dStore.Setup( s=> s.Get(idPolicy)).ReturnsAsync( key );

            var result = await keyFactory.GetDecryptionKey(idPolicy);

            dStore.Verify( s=> s.Get(idPolicy), Times.Once );
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Id, idPolicy);
        }
    }
}
