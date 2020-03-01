using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MondoCore.Common;
using MondoCore.Security.Passwords;
using MondoCore.Security.Encryption;

namespace MondoCore.Security.FunctionalTests
{
    [TestClass]
    [TestCategory("Functional Tests")]
    public class PasswordHasherTest
    {
        [TestMethod]
        public void PasswordHasher_Hash_Succeeds()
        {
            var hasher   = new PasswordHasher(11528);
            var salt     = hasher.GenerateSalt(32);
            var password = hasher.GenerateSalt(32);
            var hash     = hasher.Hash(password, salt);

            Assert.IsNotNull(hash);
            Assert.IsFalse(hash.IsEqual(password));

            // Do it a 2nd time and make sure they're equal
            var hash2 = hasher.Hash(password, salt);

            Assert.IsTrue(hash2.IsEqual(hash));
        }
    }
}
