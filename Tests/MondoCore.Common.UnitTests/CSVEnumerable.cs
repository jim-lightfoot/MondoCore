using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using MondoCore.Common;
using System.Reflection;

namespace MondoCore.Common.UnitTests
{
    [TestClass]
    [TestCategory("Unit Tests")]
    public class CSVEnumerableTests
    {
        [TestMethod]
        public async Task CSVEnumerable_enumerate()
        {
            var list = new List<Automobile>();

            using(var data = await CreateCSV())
            {
                var enumr = new CSVEnumerable<Automobile>(data);
                
                foreach(var item in enumr)
                    list.Add(item);
            }

            Assert.AreEqual(4, list.Count);

            Assert.AreEqual("Chevrolet", list[0].Make);
            Assert.AreEqual("Corvette",  list[0].Model);
            Assert.AreEqual(1956,        list[0].Year);
            Assert.AreEqual("Black",     list[0].Color);

            Assert.AreEqual("Pontiac",   list[1].Make);
            Assert.AreEqual("Firebird",  list[1].Model);
            Assert.AreEqual(1969,        list[1].Year);
            Assert.AreEqual("Green",     list[1].Color);
              
            Assert.AreEqual("Audi",      list[2].Make);
            Assert.AreEqual("A4 Avante", list[2].Model);
            Assert.AreEqual(2021,        list[2].Year);
            Assert.AreEqual("Blue",      list[2].Color);

            Assert.AreEqual("Chevrolet", list[3].Make);
            Assert.AreEqual("Camaro",    list[3].Model);
            Assert.AreEqual(1970,        list[3].Year);
            Assert.AreEqual("Red",       list[3].Color);
        }

        private async Task<Stream> CreateCSV()
        {
            var output = new MemoryStream();

            await output.WriteAsync("Make,Model,Year,Color\r\n");
            await output.WriteAsync("Chevrolet,Corvette,1956,Black\r\n");
            await output.WriteAsync("Pontiac,Firebird,1969,Green\r\n");
            await output.WriteAsync("Audi,A4 Avante,2021,Blue\r\n");
            await output.WriteAsync("Chevrolet,Camaro,1970,Red\r\n");

            return output;
        }

        internal class Automobile
        {
            public string        Make     { get; set; }
            public string        Model    { get; set; }
            public int           Year     { get; set; }
            public string        Color    { get; set; }
        }
    }
}
