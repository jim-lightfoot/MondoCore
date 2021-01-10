using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MondoCore.Common;

using Moq;

namespace MondoCore.Common.UnitTests
{
    [TestClass]
    [TestCategory("Unit Tests")]
    public class SettingsExtensionsTests
    {
        [TestMethod]
        public void SettingsExtensions_GetInt()
        {
            var settings = new Mock<ISettings>();

            settings.Setup( s=> s.Get("bob")).Returns("5");
            settings.Setup( s=> s.Get("fred")).Returns("36.17");
            settings.Setup( s=> s.Get("wilma")).Returns("true");

            var val1 = settings.Object.Get<int>("bob");
            var val2 = settings.Object.Get<decimal>("fred");
            var val3 = settings.Object.Get<bool>("wilma");

            Assert.AreEqual(5,      val1);
            Assert.AreEqual(36.17m, val2);
            Assert.IsTrue(val3);
        }

        [TestMethod]
        public void SettingsExtensions_GetObject()
        {
            var settings = new Mock<ISettings>();

            settings.Setup( s=> s.Get("bob.Url")).Returns("http://hello");
            settings.Setup( s=> s.Get("bob.Max")).Returns("35");
            settings.Setup( s=> s.Get("bob.Cost")).Returns("65.45");
            settings.Setup( s=> s.Get("bob.IsLeft")).Returns("true");

            var val = settings.Object.GetObject<Preferences>("bob");

            Assert.AreEqual("http://hello", val.Url);
            Assert.AreEqual(35,             val.Max);
            Assert.AreEqual(65.45m,         val.Cost);
            Assert.IsTrue(val.IsLeft);
        }


        [TestMethod]
        public void SettingsExtensions_AsDictionary()
        {
            var settings = new Mock<ISettings>();

            settings.Setup( s=> s.Get("Url")).Returns("http://hello");
            settings.Setup( s=> s.Get("Max")).Returns("35");
            settings.Setup( s=> s.Get("Cost")).Returns("65.45");
            settings.Setup( s=> s.Get("IsLeft")).Returns("true");

            var dict = settings.Object.AsDictionary();

            Assert.AreEqual("http://hello",  dict["Url"]);
            Assert.AreEqual("35",            dict["Max"]);
            Assert.AreEqual("65.45",         dict["Cost"]);
            Assert.AreEqual("true",          dict["IsLeft"]);
        }

        private class Preferences
        {
            public string  Url      { get; set; }
            public int     Max      { get; set; }
            public decimal Cost     { get; set; }
            public bool    IsLeft   { get; set; } = false;
        }
    }
}
