using Microsoft.VisualStudio.TestTools.UnitTesting;

using MondoCore.Common;

namespace MondoCore.Common.UnitTests
{
    [TestClass]
    [TestCategory("Unit Tests")]
    public class Base32Tests
    {
        [TestMethod]
        public void Base32_Encode()
        {
            Assert.AreEqual("",                      Base32.Encode(""));
            Assert.AreEqual("HA4TAMZRGIYDS",         Base32.Encode("89031209"));
            Assert.AreEqual("MJXWE43ZN52XE5LOMNWGK", Base32.Encode("bobsyouruncle"));
            Assert.AreEqual("GU",                    Base32.Encode("5"));
        }

        [TestMethod]
        public void Base32_Decode()
        {
            Assert.AreEqual("",              Base32.Decode(""));
            Assert.AreEqual("89031209",      Base32.Decode("HA4TAMZRGIYDS"));
            Assert.AreEqual("bobsyouruncle", Base32.Decode("MJXWE43ZN52XE5LOMNWGK"));
            Assert.AreEqual("5",             Base32.Decode("GU"));
        }
    }
}
