using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using MondoCore.Common;

namespace MondoCore.Common.UnitTests
{
    [TestClass]
    [TestCategory("Unit Tests")]
    public class DictionaryExtensionsTests
    {
        [TestMethod]
        public void DictionaryExtensions_Merge()
        {
            var dict1  = new Dictionary<string, string> { { "Make", "Chevy" }, { "Model", "Camaro" } };
            var dict2  = new Dictionary<string, string> { { "Year", "1969" },  { "Color", "Blue" } };
            var result = dict1.Merge(dict2);
            
            Assert.AreEqual(4,        result.Count);
            Assert.AreEqual("Chevy",  result["Make"]);
            Assert.AreEqual("Camaro", result["Model"]);
            Assert.AreEqual("1969",   result["Year"]);
            Assert.AreEqual("Blue",   result["Color"]);
        }

        [TestMethod]
        public void DictionaryExtensions_Merge_overlap()
        {
            var dict1  = new Dictionary<string, string> { { "Make", "Chevy" }, { "Model", "Camaro" }, { "Year", "1970" } };
            var dict2  = new Dictionary<string, string> { { "Year", "1969" },  { "Color", "Blue" } };
            var result = dict1.Merge(dict2);
            
            Assert.AreEqual(4,        result.Count);
            Assert.AreEqual("Chevy",  result["Make"]);
            Assert.AreEqual("Camaro", result["Model"]);
            Assert.AreEqual("1969",   result["Year"]);
            Assert.AreEqual("Blue",   result["Color"]);
        }
    }
}
