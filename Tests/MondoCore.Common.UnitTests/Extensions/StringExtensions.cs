using Microsoft.VisualStudio.TestTools.UnitTesting;

using MondoCore.Common;

namespace MondoCore.Common.UnitTests
{
    [TestClass]
    [TestCategory("Unit Tests")]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void StringExtensions_EnsureEndsWith()
        {
            Assert.AreEqual("",                 ((string)null).EnsureEndsWith(""));
            Assert.AreEqual("bob",              ((string)null).EnsureEndsWith("bob"));
            Assert.AreEqual("Fred Flintstone",  "Fred".EnsureEndsWith(" Flintstone"));
            Assert.AreEqual("Fred Flintstone",  "Fred Flintstone".EnsureEndsWith(" Flintstone"));
        }

        [TestMethod]
        public void StringExtensions_MatchesWildcard()
        {
            Assert.IsTrue(((string)null).MatchesWildcard("*.*"));
            Assert.IsTrue("".MatchesWildcard("*.*"));
            Assert.IsTrue("fred".MatchesWildcard("*.*"));

            Assert.IsTrue("fred.img".MatchesWildcard("*.img"));
            Assert.IsTrue("fred.img".MatchesWildcard("f*.img"));
            Assert.IsTrue("Fred.img".MatchesWildcard("f*.img"));
            Assert.IsTrue("fred.img".MatchesWildcard("?red.img"));
            Assert.IsTrue("FreD.img".MatchesWildcard("?red.img"));

            Assert.IsFalse("fred.bmp".MatchesWildcard("*.img"));
            Assert.IsFalse("george.img".MatchesWildcard("f*.img"));
            Assert.IsFalse("freb.img".MatchesWildcard("?red.img"));

        }
    }
}
