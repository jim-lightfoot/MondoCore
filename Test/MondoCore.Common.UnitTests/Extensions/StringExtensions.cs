using Microsoft.VisualStudio.TestTools.UnitTesting;

using MondoCore.Common;

namespace MondoCore.Common.UnitTests
{
    [TestClass]
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
    }
}
