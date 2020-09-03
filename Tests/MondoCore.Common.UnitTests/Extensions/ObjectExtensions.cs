using Microsoft.VisualStudio.TestTools.UnitTesting;

using MondoCore.Common;
using System.Collections.Generic;

namespace MondoCore.Common.UnitTests
{
    [TestClass]
    [TestCategory("Unit Tests")]
    public class ObjectExtensionsTests
    {
        [TestMethod]
        public void ObjectExtensions_ToDictionary_null()
        {
            object src  = null;
            var dict = src.ToDictionary();

            Assert.IsNull(dict);
        }

        [TestMethod]
        public void ObjectExtensions_ToDictionary_sametype()
        {
            var src  = new Dictionary<string, object> { {"Color", "red" }, { "Make", "Chevy" } };
            var dict = src.ToDictionary();

            Assert.AreEqual(2, dict.Count);
            Assert.AreEqual("red", dict["Color"]);
            Assert.AreEqual("Chevy", dict["Make"]);
        }

        [TestMethod]
        public void ObjectExtensions_ToDictionary_diff_dict_type()
        {
            var src  = new Dictionary<string, string> { {"Color", "red" }, { "Make", "Chevy" } };
            var dict = src.ToDictionary();

            Assert.AreEqual(2, dict.Count);
            Assert.AreEqual("red", dict["Color"]);
            Assert.AreEqual("Chevy", dict["Make"]);
        }

        [TestMethod]
        public void ObjectExtensions_ToDictionary_diff_dict_type2()
        {
            var src  = new Dictionary<string, Automobile> { {"Chevy", new Automobile { Model = "Camaro" }  }, { "Pontiac", new Automobile { Model = "Firebird" } } };
            var dict = src.ToDictionary();

            Assert.AreEqual(2, dict.Count);
            Assert.AreEqual("Camaro", (dict["Chevy"] as Automobile).Model);
            Assert.AreEqual("Firebird", (dict["Pontiac"] as Automobile).Model);
        }

        [TestMethod]
        public void ObjectExtensions_ToDictionary_object()
        {
            var src  = new { Make = "Chevy", Model = "Camaro" , Year = 1969 };
            var dict = src.ToDictionary();

            Assert.AreEqual(3, dict.Count);
            Assert.AreEqual("Chevy", dict["Make"]);
            Assert.AreEqual("Camaro", dict["Model"]);
            Assert.AreEqual(1969, dict["Year"]);
        }


        [TestMethod]
        public void ObjectExtensions_ToDictionary_types()
        {
            var src  = new Automobile { Make = "Chevy", Model = "Camaro" , Year = 1969 };
            var dict = src.ToDictionary();

            Assert.AreEqual(3, dict.Count);
            Assert.AreEqual("Chevy", dict["Make"]);
            Assert.AreEqual("Camaro", dict["Model"]);
            Assert.AreEqual(1969, dict["Year"]);
        }

       [TestMethod]
        public void ObjectExtensions_ToStringDictionary()
        {
            var src  = new Dictionary<string, Automobile> { {"Chevy", new Automobile { Model = "Camaro" }  }, { "Pontiac", new Automobile { Model = "Firebird" } } };
            var dict = src.ToStringDictionary();

            Assert.AreEqual(2, dict.Count);
            Assert.AreEqual("Camaro", dict["Chevy"]);
            Assert.AreEqual("Firebird", dict["Pontiac"]);
        }

        public class Automobile
        {
            public string Make {get; set;}
            public string Model {get; set;}
            public int    Year {get; set;}

            public override string ToString()
            {
                return Model;
            }
        }
    }
}
