using System;
using System.Collections.Generic;

using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MondoCore.Common;
using MondoCore.Security.Encryption;
using MondoCore.Security.Passwords;

using Moq;

namespace MondoCore.Security.Authentication.UnitTests
{
    [TestClass]
    [TestCategory("Unit Tests")]
    public class TOTPTests
    {
        [TestMethod]
        public void TOTP_GenerateCode()
        {
            var result = TOTP.GenerateCode();

            Assert.IsNotNull(result.Secret);
            Assert.AreNotEqual(0, result.Code);
        }

        [TestMethod]
        public void TOTP_IsValid()
        {
            var result = TOTP.GenerateCode();

            Assert.IsTrue(TOTP.IsValid(result.Code, result.Secret, DateTime.UtcNow.AddSeconds(30), 90));
        }

        [TestMethod]
        public void TOTP_IsValid_fails()
        {
            var result = TOTP.GenerateCode();

            Assert.IsFalse(TOTP.IsValid(result.Code, result.Secret, DateTime.UtcNow.AddSeconds(300), 90));
        }

        [TestMethod]
        public void TOTP_IsValid_out_of_sync_clocks()
        {
            var result = TOTP.GenerateCode();

            Assert.IsTrue(TOTP.IsValid(result.Code, result.Secret, DateTime.UtcNow.AddSeconds(-32), 90));
        }
    }
}
