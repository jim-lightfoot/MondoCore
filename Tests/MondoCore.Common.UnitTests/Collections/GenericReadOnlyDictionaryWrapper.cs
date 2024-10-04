using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MondoCore.Common.UnitTests
{
    [TestClass]
    [TestCategory("Unit Tests")]
    public class GenericReadOnlyDictionaryWrapperTests
    {
        [TestMethod]
        public void GenericReadOnlyDictionaryWrapper_this_op()
        {
            var dict = new Dictionary<string, string> { {"Make", "Chevy"}, {"Model", "Camaro"}, {"Color", "Blue"} };
            var wrapper = new GenericReadOnlyDictionaryWrapper<string>(dict);

            Assert.AreEqual("Chevy",  wrapper["Make"]);
            Assert.AreEqual("Camaro", wrapper["Model"]);
            Assert.AreEqual("Blue",   wrapper["Color"]);
        }

        [TestMethod]
        public void GenericReadOnlyDictionaryWrapper_enumerator()
        {
            var dict = new Dictionary<string, string> { {"Make", "Chevy"}, {"Model", "Camaro"}, {"Color", "Blue"} };
            var wrapper = new GenericReadOnlyDictionaryWrapper<string>(dict);

            foreach(var item in wrapper) 
            {   
                Assert.AreEqual("Make",  item.Key);
                Assert.AreEqual("Chevy", item.Value);

                break;
            }
        }
    }
}
 