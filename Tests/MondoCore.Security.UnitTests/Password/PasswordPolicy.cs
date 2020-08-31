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
    [TestCategory("Unit Tests")]
    public class PasswordPolicyTests
    {
        [TestMethod]
        public void PasswordPolicy_IsValid_succeeds()
        {
            var policy = new PasswordPolicy();

            Assert.IsTrue(policy.IsValid("geoR;ge123bbbbb"));
            Assert.IsTrue(policy.IsValid("Fred234()_"));
            Assert.IsTrue(policy.IsValid("Goingonahike123!@#"));
        }

        [TestMethod]
        public void PasswordPolicy_IsValid_fails()
        {
            var policy = new PasswordPolicy();

            Assert.IsFalse(policy.IsValid(null));
            Assert.IsFalse(policy.IsValid(""));
            Assert.IsFalse(policy.IsValid("  "));
            Assert.IsFalse(policy.IsValid("bob"));
            Assert.IsFalse(policy.IsValid("george123"));
            Assert.IsFalse(policy.IsValid("george123bbbbb"));
            Assert.IsFalse(policy.IsValid("1234"));
            Assert.IsFalse(policy.IsValid("4567"));
            Assert.IsFalse(policy.IsValid("9876"));
            Assert.IsFalse(policy.IsValid("8765"));
            Assert.IsFalse(policy.IsValid("7654"));
            Assert.IsFalse(policy.IsValid("6543"));
            Assert.IsFalse(policy.IsValid("5432"));
            Assert.IsFalse(policy.IsValid("4321"));
            Assert.IsFalse(policy.IsValid("Frank Bob 123 !!*("));
        }

        [TestMethod]
        public void PasswordPolicy_IsValid_fails_invalidchars()
        {
            var policy = new PasswordPolicy();

            policy.InvalidChars.Add('@');

            Assert.IsFalse(policy.IsValid("geoR;ge12@3bbbbb"));
        }

        [TestMethod]
        public void PasswordPolicy_IsValid_fails_invalidwords()
        {
            var policy = new PasswordPolicy();

            policy.InvalidWords.Add("jim");

            Assert.IsFalse(policy.IsValid("Password123"));
            Assert.IsFalse(policy.IsValid("Pa$$word123"));
            Assert.IsFalse(policy.IsValid("Pa$$w0rd123"));
            Assert.IsFalse(policy.IsValid("Passw0rd123"));
            Assert.IsFalse(policy.IsValid("P@ssword123"));

            Assert.IsFalse(policy.IsValid("AdMin123!&*("));
            Assert.IsFalse(policy.IsValid("adMin123!&*("));
            Assert.IsFalse(policy.IsValid("@dMin123!&*("));
            Assert.IsFalse(policy.IsValid("AdM1n123!&*("));
            Assert.IsFalse(policy.IsValid("AdM!n123!&*("));
            Assert.IsFalse(policy.IsValid("AdM!n123!&*("));
            Assert.IsFalse(policy.IsValid("AdM!n123!&*("));
            Assert.IsFalse(policy.IsValid("AdM!n123!&*("));
            Assert.IsFalse(policy.IsValid("ådmin123!&*(M"));
            Assert.IsFalse(policy.IsValid("àdmin123!&*(M"));
            Assert.IsFalse(policy.IsValid("ádmin123!&*(M"));
            Assert.IsFalse(policy.IsValid("ãdmin123!&*(M"));
            Assert.IsFalse(policy.IsValid("ädmin123!&*(M"));
            Assert.IsFalse(policy.IsValid("ạdmin123!&*(M"));
            
            Assert.IsFalse(policy.IsValid("Jimbo456"));
            Assert.IsFalse(policy.IsValid("J!mbo456"));
            Assert.IsFalse(policy.IsValid("Jimb0456"));
            Assert.IsFalse(policy.IsValid("jimbo456"));

            Assert.IsFalse(policy.IsValid("qwerty345B;"));
            Assert.IsFalse(policy.IsValid("qw3rty345B;"));
            Assert.IsFalse(policy.IsValid("qwer7y345B;"));
            Assert.IsFalse(policy.IsValid("qwerty345B;"));

            Assert.IsFalse(policy.IsValid("princess"));
            Assert.IsFalse(policy.IsValid("qw3rty345B;"));
            Assert.IsFalse(policy.IsValid("qwer7y345B;"));
            Assert.IsFalse(policy.IsValid("qwerty345B;"));

            Assert.IsFalse(policy.IsValid("Fr@nk789!!", "frank"));
            Assert.IsFalse(policy.IsValid("G3orge789!!", "frank", "george"));
            Assert.IsFalse(policy.IsValid("acme789!!Bob", "frank", "george", "acme"));

            Assert.IsFalse(policy.IsValid("cute!!BobFrank;;", "cute"));
            Assert.IsFalse(policy.IsValid("çute!!BobFrank;;", "cute"));
            Assert.IsFalse(policy.IsValid("ćute!!BobFrank;;", "cute"));
        }
    }
}
