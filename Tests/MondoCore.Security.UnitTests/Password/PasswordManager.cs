using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MondoCore.Common;
using MondoCore.Security.Passwords;
using MondoCore.Security.Encryption;

using Moq;

namespace MondoCore.Security.UnitTests
{
    [TestClass]
    [TestCategory("Unit Tests")]
    public class PasswordManagerTest
    {
        private IPasswordManager     _passwordManager;
        private Mock<IPasswordStore> _passwordStore;

        [TestInitialize]
        public void Init()
        {
            _passwordStore = new Mock<IPasswordStore>();

            var encryptor = new PassThruEncryptor();

            _passwordManager = new PasswordManager(new PasswordHasher(11528), _passwordStore.Object, encryptor);
        }

        [TestMethod]
        public void PasswordManager_FromOwner_Succeeds()
        {
            var password = _passwordManager.FromOwner("bob", new LongPasswordOwner(1000));

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
            var password1 = _passwordManager.GenerateNew(10);
            var password2 = _passwordManager.GenerateNew(10);
            var password3 = _passwordManager.GenerateNew(10);

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

        [TestMethod]
        public async Task PasswordManager_Load()
        {
            var owner = new LongPasswordOwner(1000);
            var password = _passwordManager.FromOwner("bobsyouruncle", owner);

            var passwordManager = CreateManager(password);

            var password2 = await passwordManager.Load(owner);

            Assert.IsTrue(password.IsEqual(password2));
        }
       
        [TestMethod]
        public async Task PasswordManager_Save()
        {
            var owner = new LongPasswordOwner(1000);
            var password = _passwordManager.FromOwner("bobsyouruncle", owner);

            var passwordManager = CreateManager(null);

            await passwordManager.Save(password);

            var password2 = await passwordManager.Load(owner);

            Assert.IsTrue(password.IsEqual(password2));
        }
       
        #region Private

        private IPasswordManager CreateManager(Password password)
        {
            var passwordStore = new MockPasswordStore(password);
            var encryptor     = new PassThruEncryptor();

            // See https://en.wikipedia.org/wiki/PBKDF2 for hash iteration recommendations
            return new PasswordManager(new PasswordHasher(11528), passwordStore, encryptor);
        }

        private class MockPasswordStore : IPasswordStore
        {   
            private readonly List<Password> _passwords = new List<Password>();

            internal MockPasswordStore(Password password)
            {
                if(password != null)
                    _passwords.Add(password);
            }

            public Task<byte[]> Get(IPasswordOwner owner, out byte[] salt)
            {
                salt = _passwords[0].Salt;

                return Task.FromResult(_passwords[0].ToArray());
            }

            public Task Add(byte[] password, byte[] salt, IPasswordOwner owner)
            { 
                _passwords.Add(new Password(password, salt, owner));

                return Task.CompletedTask;
            }
        }

        #endregion

    }
}
