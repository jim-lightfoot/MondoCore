using System;
using System.Collections.Generic;

using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MondoCore.Common;
using MondoCore.Security.Passwords;

using Moq;

namespace MondoCore.Security.Passwords.UnitTests
{
    [TestClass]
    public class PasswordTests
    {
        [TestMethod]
        public void Password_IsEqual()
        {
            var pwd1 = new Password(new byte[] { 1, 2, 3, 4}, new byte[] { 1, 2, 3, 4}, new LongPasswordOwner(1000) );
            var pwd2 = new Password(new byte[] { 1, 2, 3, 4}, new byte[] { 1, 2, 3, 4}, new LongPasswordOwner(1000)  );

            Assert.IsTrue(pwd1.IsEqual(pwd2));
        }
    }
}
