using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("MondoCore.Common.UnitTests")]

namespace MondoCore.Common
{
    /****************************************************************************/
    /****************************************************************************/
    /// <summary>
    /// A readonly dictionary that wraps an IDictionary
    /// </summary>
    internal class NonGenericReadOnlyDictionaryWrapper : IReadOnlyDictionary<string, object>
    {
        private readonly IDictionary _dict;

        /****************************************************************************/
        public NonGenericReadOnlyDictionaryWrapper(IDictionary dict)
        {
            _dict = dict;
        }

        /****************************************************************************/
        public object this[string key] => _dict[key];

        /****************************************************************************/
        public IEnumerable<string> Keys       
        {
            get
            {
                if(_dict.Count == 0)
                    return Enumerable.Empty<string>();  

                var keys = new List<string>();

                foreach(var key in _dict.Keys) 
                    keys.Add(key.ToString());

                return keys;
            }
        }

        /****************************************************************************/
        public IEnumerable<object> Values       
        {
            get
            {
                if(_dict.Count == 0)
                    return Enumerable.Empty<object>();  

                var values = new List<object>();

                foreach(var val in _dict.Values) 
                    values.Add(val);

                return values;
            }
        }

        /****************************************************************************/
        public int Count => _dict.Count;

        /****************************************************************************/
        public bool ContainsKey(string key)
        {
            return _dict.Contains(key);
        }

        /****************************************************************************/
        public bool TryGetValue(string key, out object value)
        {
            try
            {
                value = _dict[key];
                return true;    
            }
            catch
            {
                value = null;
                return false;
            }
        }

        /****************************************************************************/
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()        
        {
            return new MyEnumerator(_dict);
        }

        /****************************************************************************/
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        /****************************************************************************/
        /****************************************************************************/
        private class MyEnumerator(IDictionary dict) : IEnumerator<KeyValuePair<string, object>>
        {
            private readonly IEnumerator _enum = dict.GetEnumerator();

            /****************************************************************************/
            public KeyValuePair<string, object> Current 
            {
                get
                { 
                    var current = _enum.Current;

                    if(current is DictionaryEntry entry)
                        return new KeyValuePair<string, object>(entry.Key.ToString(), entry.Value);

                    if(current is KeyValuePair<string, object> kv)
                        return new KeyValuePair<string, object>(kv.Key, kv.Value);

                    return new KeyValuePair<string, object>();
                }
            }

            /****************************************************************************/
            object IEnumerator.Current => _enum.Current;

            /****************************************************************************/
            public void Dispose()
            {
            }

            /****************************************************************************/
            public bool MoveNext()
            {
                return _enum.MoveNext();
            }

            /****************************************************************************/
            public void Reset()
            {
                _enum.Reset();
            }
        }
    }
}
