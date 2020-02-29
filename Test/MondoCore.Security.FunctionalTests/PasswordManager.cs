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
    public class PasswordManagerTest
    {
        [TestMethod]
        public void PasswordManager_FromOwner()
        {
            var manager  = CreateManager();
            var password = manager.FromOwner("bob", new LongPasswordOwner(1000));

            Assert.IsNotNull(password);
            Assert.IsNotNull(password.Salt);
            Assert.AreNotEqual(0, password.Salt.Length);
            Assert.IsNotNull(password.ToArray());
            Assert.AreNotEqual(0, password.ToArray().Length);
            Assert.AreEqual(1000, (password.Owner as LongPasswordOwner).Id);
        }

        [TestMethod]
        public void PasswordManager_GenerateNew()
        {
            var manager  = CreateManager();
            var password1 = manager.GenerateNew(10);
            var password2 = manager.GenerateNew(10);
            var password3 = manager.GenerateNew(10);

            Assert.IsFalse(string.IsNullOrWhiteSpace(password1));
            Assert.IsFalse(string.IsNullOrWhiteSpace(password2));
            Assert.IsFalse(string.IsNullOrWhiteSpace(password3));
            Assert.AreEqual(10, password1.Length);
            Assert.AreEqual(10, password2.Length);
            Assert.AreEqual(10, password3.Length);

            Assert.AreNotEqual(password1, password2);
            Assert.AreNotEqual(password2, password3);
            Assert.AreNotEqual(password1, password3);
        }

        #region Private

        private IPasswordManager CreateManager()
        {
            var passwordStore = new MockPasswordStore();
            var encryptor     = new PassThruEncryptor();

            return new PasswordManager(new PasswordHasher(11528), passwordStore, encryptor);
        }

        private class MockPasswordStore : IPasswordStore
        {   
            public Task<byte[]> Get(IPasswordOwner owner, out byte[] salt)
            {
                salt = new byte[] { 1, 2, 3, 4 };

                return Task.FromResult(new byte[] { 5, 6, 7, 8 });

            }

            public Task Add(byte[] password, byte[] salt, IPasswordOwner owner)
            { 
                return Task.CompletedTask;
            }
        }

        #endregion
    }
}
