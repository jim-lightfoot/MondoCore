using Microsoft.VisualStudio.TestTools.UnitTesting;

using MondoCore.Common;
using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using System.Linq;

namespace MondoCore.Common.UnitTests
{
    [TestClass]
    [TestCategory("Unit Tests")]
    public class ObjectExtensionsTests
    {
        #region ToDictionary 

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

        #endregion

        #region ToReadOnlyDictionary 

        [TestMethod]
        public void ObjectExtensions_ToReadOnlyDictionary_null()
        {
            object src  = null;
            var dict = src.ToReadOnlyDictionary();

            Assert.IsNull(dict);
        }

        [TestMethod]
        public void ObjectExtensions_ToReadOnlyDictionary_sametype()
        {
            var src  = new Dictionary<string, object> { {"Color", "red" }, { "Make", "Chevy" } };
            var dict = src.ToReadOnlyDictionary();

            Assert.AreEqual(2, dict.Count);
            Assert.AreEqual("red", dict["Color"]);
            Assert.AreEqual("Chevy", dict["Make"]);
        }

        [TestMethod]
        public void ObjectExtensions_ToReadOnlyDictionary_diff_dict_type()
        {
            var src  = new Dictionary<string, string> { {"Color", "red" }, { "Make", "Chevy" } };
            var dict = src.ToReadOnlyDictionary();

            Assert.AreEqual(2, dict.Count);
            Assert.AreEqual("red", dict["Color"]);
            Assert.AreEqual("Chevy", dict["Make"]);
        }

        [TestMethod]
        public void ObjectExtensions_ToReadOnlyDictionary_diff_dict_type2()
        {
            var src  = new Dictionary<string, Automobile> { {"Chevy", new Automobile { Model = "Camaro" }  }, { "Pontiac", new Automobile { Model = "Firebird" } } };
            var dict = src.ToReadOnlyDictionary();

            Assert.AreEqual(2, dict.Count);
            Assert.AreEqual("Camaro", (dict["Chevy"] as Automobile).Model);
            Assert.AreEqual("Firebird", (dict["Pontiac"] as Automobile).Model);
        }

        [TestMethod]
        public void ObjectExtensions_ToReadOnlyDictionary_object()
        {
            var src  = new { Make = "Chevy", Model = "Camaro" , Year = 1969 };
            var dict = src.ToReadOnlyDictionary();

            Assert.AreEqual(3, dict.Count);
            Assert.AreEqual("Chevy", dict["Make"]);
            Assert.AreEqual("Camaro", dict["Model"]);
            Assert.AreEqual(1969, dict["Year"]);
        }

        [TestMethod]
        public void ObjectExtensions_ToReadOnlyDictionary_types()
        {
            var src  = new Automobile { Make = "Chevy", Model = "Camaro" , Year = 1969 };
            var dict = src.ToReadOnlyDictionary();

            Assert.AreEqual(3, dict.Count);
            Assert.AreEqual("Chevy", dict["Make"]);
            Assert.AreEqual("Camaro", dict["Model"]);
            Assert.AreEqual(1969, dict["Year"]);
        }

        #endregion

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

        [TestMethod]
        public void ObjectExtensions_SetValue_success()
        {
            var src  = new Automobile { Make = "Chevy", Model = "Camaro" , Color = "Blue", Year = 1969 };

            Assert.IsTrue(src.SetValue("Model", "Corvette"));

            Assert.AreEqual("Corvette", src.Model);
        }

        [TestMethod]
        public void ObjectExtensions_SetValue_fails()
        {
            var src  = new Automobile { Make = "Chevy", Model = "Camaro" , Color = "Blue", Year = 1969 };

            Assert.ThrowsException<ArgumentException>( ()=> src.SetValue("Year", "bob"));
        }

        [TestMethod]
        public void ObjectExtensions_SetValue_fails2()
        {
            var src  = new Automobile { Make = "Chevy", Model = "Camaro" , Color = "Blue", Year = 1969 };

            Assert.IsFalse(src.SetValue("Frank", "bob"));
        }

        [TestMethod]
        public void ObjectExtensions_SetProperties_success()
        {
            var car1 = new Automobile { Make = "Chevy", Model = "Camaro", Color = "Blue", Year = 1969 };
            var car2 = new Automobile { Make = "Chevy", Model = "Corvette", Color = "Green", Year = 1964 };

            Assert.IsTrue(car1.SetValues( new { Color = "Black", Year = 1970 }));
            Assert.IsTrue(car2.SetValues( new { Color = "Purple", Year = "1965" }));

            Assert.AreEqual("Black", car1.Color);
            Assert.AreEqual(1970, car1.Year);

           Assert.AreEqual("Purple", car2.Color);
           Assert.AreEqual(1965, car2.Year);
        }

        [TestMethod]
        public void ObjectExtensions_GetProperties_success()
        {
            var car1  = new Automobile { Make = "Chevy", Model = "Camaro", Color = "Blue", Year = 1969 };
            var props = car1.GetProperties().ToList();

            Assert.AreEqual(4, props.Count);

            Assert.AreEqual("Make",  props[0].Name);
            Assert.AreEqual("Model",  props[1].Name);
            Assert.AreEqual("Color",  props[2].Name);
            Assert.AreEqual("Year",   props[3].Name);
                                      
            Assert.AreEqual("Chevy",  props[0].Value);
            Assert.AreEqual("Camaro", props[1].Value);
            Assert.AreEqual("Blue",   props[2].Value);
            Assert.AreEqual(1969,     props[3].Value);
        }

        [TestMethod]
        public void ObjectExtensions_Map_success()
        {
            var car1 = new Automobile { Make = "Chevy", Model = "Camaro", Color = "Blue", Year = 1969 };
            var car2 = car1.Map<Automobile, Car>();

            Assert.AreEqual("Chevy", car2.Make);
            Assert.AreEqual("Camaro", car2.Model);
            Assert.AreEqual("Blue", car2.Color);
            Assert.AreEqual(1969, car2.Year);
        }

        [TestMethod]
        public void ObjectExtensions_Map2_success()
        {
            var car1 = new Automobile { Make = "Chevy", Model = "Camaro", Color = "Blue", Year = 1969 };
            var car2 = car1.Map<Automobile, Car2>();

            Assert.AreEqual("Chevy", car2.Make);
            Assert.AreEqual("Camaro", car2.Model);
            Assert.AreEqual("Blue", car2.Color);
            Assert.AreEqual(1969, car2.Year);
            Assert.AreEqual(null, car2.Engine);
        }

        [TestMethod]
        public void ObjectExtensions_Map3_success()
        {
            var car1 = new Automobile { Make = "Chevy", Model = "Camaro", Color = "Blue", Year = 1969 };
            var car2 = car1.Map<Automobile, Car3>();

            Assert.AreEqual("Chevy", car2.Make);
            Assert.AreEqual("Camaro", car2.Model);
            Assert.AreEqual("Blue", car2.Color);
            Assert.AreEqual("1969", car2.Year);
            Assert.AreEqual(null, car2.Engine);
        }

        public class Automobile
        {
            public string Make  {get; set;}
            public string Model {get; set;}
            public string Color {get; set;}
            public int    Year  {get; set;}

            public override string ToString()
            {
                return Model;
            }
        }

        public class Car
        {
            public string Make  {get; set;}
            public string Model {get; set;}
            public string Color {get; set;}
            public int    Year  {get; set;}
        }

        public class Car2
        {
            public string Make  {get; set;}
            public string Model {get; set;}
            public string Color {get; set;}
            public int    Year  {get; set;}
            public string Engine  {get; set;}
        }

        public class Car3
        {
            public string Make  {get; set;}
            public string Model {get; set;}
            public string Color {get; set;}
            public string Year  {get; set;}
            public string Engine  {get; set;}
        }
    }
}
