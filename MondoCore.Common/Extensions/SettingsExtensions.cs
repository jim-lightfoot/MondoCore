/*************************************************************************** 
 *                                                                           
 *    The MondoCore Libraries  							                     
 *                                                                           
 *        Namespace: MondoCore.Common							             
 *             File: SettingsExtensions.cs					    		         
 *        Class(es): SettingsExtensions				         		             
 *          Purpose: Extensions for ISettings                  
 *                                                                           
 *  Original Author: Jim Lightfoot                                           
 *    Creation Date: 1 Jan 2020                                              
 *                                                                           
 *   Copyright (c) 2005-2020 - Jim Lightfoot, All rights reserved            
 *                                                                           
 *  Licensed under the MIT license:                                          
 *    http://www.opensource.org/licenses/mit-license.php                     
 *                                                                           
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MondoCore.Common
{
    public static class SettingsExtensions
    {
        /// <summary>
        /// Returns a typed value
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Get<T>(this ISettings settings, string name) where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
        {
            var val = settings.Get(name);

            return (T)Convert.ChangeType(val, typeof(T));
        }

        /// <summary>
        /// Returns an object. Each property is filled with a settings value that is the prefix + "." plus the property name
        /// </summary>
        /// <param name="prefix">A prefix</param>
        /// <returns>An object whose member property values come from the settings</returns>
        public static T GetObject<T>(this ISettings settings, string prefix) where T: class, new()
        {
            var val    = settings.Get(prefix);
            var result = new T();
            var props  = result.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where( p=> p.CanRead );

            foreach(var prop in props)
            {
                try
                { 
                    var sval = settings.Get($"{prefix}.{prop.Name}");

                    prop.SetValue(result, Convert.ChangeType(sval, prop.PropertyType));
                }
                catch
                {

                }
            }

            return result;
        }

        /// <summary>
        /// Returns a dictionary that gets it's values from the settings
        /// </summary>
        /// <returns>Dictionary</returns>
        public static IDictionary<string, object> AsDictionary(this ISettings settings)
        {
            return new SettingsDictionary(settings);
        }

        #region SettingsDictionary

        private class SettingsDictionary : IDictionary<string, object>
        {
            private readonly ISettings _settings;

            internal SettingsDictionary(ISettings settings)
            {
                _settings = settings;
            }

            #region IDictionary<string, object>

            public object this[string key] { get => _settings.Get(key); set => throw new NotSupportedException(); }

            public bool IsReadOnly => true;

            #region Not Supported 

            public ICollection<string> Keys => throw new NotSupportedException();

            public ICollection<object> Values => throw new NotSupportedException();

            public int Count => throw new NotSupportedException();

            public void Add(string key, object value)
            {
                throw new NotSupportedException();
            }

            public void Add(KeyValuePair<string, object> item)
            {
                throw new NotSupportedException();
            }

            public void Clear()
            {
                throw new NotSupportedException();
            }

            public bool Contains(KeyValuePair<string, object> item)
            {
                throw new NotSupportedException();
            }

            public bool ContainsKey(string key)
            {
                throw new NotSupportedException();
            }

            public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
            {
                throw new NotSupportedException();
            }

            public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
            {
                throw new NotSupportedException();
            }

            public bool Remove(string key)
            {
                throw new NotSupportedException();
            }

            public bool Remove(KeyValuePair<string, object> item)
            {
                throw new NotSupportedException();
            }

            public bool TryGetValue(string key, out object value)
            {
                throw new NotSupportedException();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                throw new NotSupportedException();
            }

            #endregion

            #endregion
        }

        #endregion
    }
}
