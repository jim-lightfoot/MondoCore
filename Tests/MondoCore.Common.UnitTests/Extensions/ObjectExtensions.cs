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
