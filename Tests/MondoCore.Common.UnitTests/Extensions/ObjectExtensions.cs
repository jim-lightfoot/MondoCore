using Microsoft.VisualStudio.TestTools.UnitTesting;

using MondoCore.Common;
using System.Collections.Generic;

using Newtonsoft.Json;

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
        public void ObjectExtensions_ToStringDictionary_json()
        {
            var src  = new Dictionary<string, Automobile> { {"Chevy", new Automobile { Model = "Camaro" }  }, { "Pontiac", new Automobile { Model = "Firebird" } } };
            var dict = src.ToStringDictionary();

            Assert.AreEqual(2, dict.Count);

            var chevy = JsonConvert.DeserializeObject<Automobile>(dict["Chevy"]);
            var pontiac = JsonConvert.DeserializeObject<Automobile>(dict["Pontiac"]);

            Assert.IsNotNull(chevy);
            Assert.IsNotNull(pontiac);

            Assert.AreEqual("Camaro", chevy.Model);
            Assert.AreEqual("Firebird", pontiac.Model);
        }

        [TestMethod]
        public void ObjectExtensions_ToStringDictionary_dotted()
        {
            var src  = new { Chevy = new { Model = "Camaro" }, Pontiac = new { Model = "Firebird" } };
            var dict = src.ToStringDictionary(false);

            Assert.AreEqual(2, dict.Count);

            Assert.AreEqual("Camaro", dict["Chevy.Model"]);
            Assert.AreEqual("Firebird", dict["Pontiac.Model"]);
        }

        [TestMethod]
        public void ObjectExtensions_ToStringDictionary_dotted2()
        {
            var src  = new { Chevy = new { Model = "Camaro" }, Pontiac = new { Model = "Firebird", Engine = new { Cylinders = 8, Displacement = 350 } } };
            var dict = src.ToStringDictionary(false);

            Assert.AreEqual(4, dict.Count);

            Assert.AreEqual("Camaro", dict["Chevy.Model"]);
            Assert.AreEqual("Firebird", dict["Pontiac.Model"]);
            Assert.AreEqual("8", dict["Pontiac.Engine.Cylinders"]);
            Assert.AreEqual("350", dict["Pontiac.Engine.Displacement"]);
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
