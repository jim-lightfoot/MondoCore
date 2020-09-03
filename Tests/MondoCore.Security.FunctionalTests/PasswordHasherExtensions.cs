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
    public class PasswordHasherExtensionsTest
    {
        [TestMethod]
        public void PasswordHasher_Hash_Succeeds()
        {
            var hasher   = new PasswordHasher(11528);
            var owner    = new LongPasswordOwner(1000);
            var salt     = hasher.GenerateSalt(32);
            var password = hasher.GenerateSalt(32);
            var hash     = hasher.Hash(password, salt, owner.ToArray);

            Assert.IsNotNull(hash);
            Assert.IsFalse(hash.IsEqual(password));

            // Do it a 2nd time and make sure they're equal
            var hash2 = hasher.Hash(password, salt, owner.ToArray);

            Assert.IsTrue(hash2.IsEqual(hash));
        }

        [TestMethod]
        public void PasswordHasher_Hash_Fails_WrongOwner()
        {
            var hasher   = new PasswordHasher(11528);
            var owner1   = new LongPasswordOwner(1000);
            var owner2   = new LongPasswordOwner(1001);
            var salt     = hasher.GenerateSalt(32);
            var password = hasher.GenerateSalt(32);
            var hash     = hasher.Hash(password, salt, owner1.ToArray);

            // Do it a 2nd time with wrong owner and make sure they're not equal
            var hash2 = hasher.Hash(password, salt, owner2.ToArray);

            Assert.IsFalse(hash2.IsEqual(hash));
        }
    }
}
