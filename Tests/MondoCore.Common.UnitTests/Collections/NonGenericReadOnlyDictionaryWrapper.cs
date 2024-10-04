using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections;
using System.Collections.Generic;

namespace MondoCore.Common.UnitTests
{
    [TestClass]
    [TestCategory("Unit Tests")]
    public class NonGenericReadOnlyDictionaryWrapperTests
    {
        [TestMethod]
        public void NonGenericReadOnlyDictionaryWrapper_this_op()
        {
            var dict = new StringDictionary { { "Make", "Chevy" }, { "Model", "Camaro" }, { "Color", "Blue" } };
            var wrapper = new NonGenericReadOnlyDictionaryWrapper(dict);

            Assert.AreEqual("Chevy", wrapper["Make"]);
            Assert.AreEqual("Camaro", wrapper["Model"]);
            Assert.AreEqual("Blue", wrapper["Color"]);
        }

        [TestMethod]
        public void NonGenericReadOnlyDictionaryWrapper_enumerator()
        {
            var dict = new StringDictionary { { "Make", "Chevy" }, { "Model", "Camaro" }, { "Color", "Blue" } };
            var wrapper = new NonGenericReadOnlyDictionaryWrapper(dict);

            foreach (var item in wrapper)
            {
                if(item.Key == "Make")
                    Assert.AreEqual("Chevy", item.Value);
                else if(item.Key == "Model")
                    Assert.AreEqual("Camaro", item.Value);
                else if(item.Key == "Color")
                    Assert.AreEqual("Blue", item.Value);
            }
        }
    }

    public class StringDictionary : DictionaryBase
    {

        public string this[string key]
        {
            get
            {
                return ((string)Dictionary[key]);
            }
            set
            {
                Dictionary[key] = value;
            }
        }

        public ICollection Keys
        {
            get
            {
                return (Dictionary.Keys);
            }
        }

        public ICollection Values
        {
            get
            {
                return (Dictionary.Values);
            }
        }

        public void Add(string key, string value)
        {
            Dictionary.Add(key, value);
        }

        public bool Contains(string key)
        {
            return (Dictionary.Contains(key));
        }

        public void Remove(string key)
        {
            Dictionary.Remove(key);
        }

        protected override void OnRemove(object key, object value)
        {
        }

        protected override void OnSet(object key, object oldValue, object newValue)
        {
        }

        protected override void OnValidate(object key, object value)
        {
        }
    }


}
