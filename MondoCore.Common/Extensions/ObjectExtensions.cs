/*************************************************************************** 
 *                                                                           
 *    The MondoCore Libraries  							                     
 *                                                                           
 *        Namespace: MondoCore.Common							             
 *             File: ObjectExtensions.cs					    		         
 *        Class(es): ObjectExtensions				         		             
 *          Purpose: Extensions for objects                  
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Newtonsoft.Json;

namespace MondoCore.Common
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Translates an object into a dictionary
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IDictionary<string, object> ToDictionary(this object obj)
        {
            if (obj == null)
                return null;

            if (obj is IDictionary<string, object> dict)
                return dict;

            var result = new Dictionary<string, object>();

            if(obj is IDictionary dict2)
            {
                foreach(var key in dict2.Keys)
                    result.Add(key.ToString(), dict2[key]);
            }
            else if(obj is IEnumerable list)
            {
                foreach(var val in list)
                    result.Add(val.ToString(), val);
            }
            else
            {
                // Get public and instance properties only
                var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where( p=> p.CanRead );

                foreach(var property in properties)
                {
                    try
                    { 
                        var value = property.GetGetMethod().Invoke(obj, null);

                        // Add property name and value to dictionary
                        if (value != null)
                            result.Add(property.Name, value);
                    }
                    catch
                    {
                        // Just ignore it
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Translates an object into a string dictionary
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IDictionary<string, string> ToStringDictionary(this object obj, bool childrenAsJson = true)
        {
            if (obj == null)
                return null;

            if (obj is IDictionary<string, string> dict)
                return dict;

            var result  = new Dictionary<string, string>();

            AppendValue(result, "", obj, childrenAsJson);

            return result;
        }

        /// <summary>
        /// Set the value of a named property
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propName"></param>
        /// <param name="value"></param>
        /// <returns>True if successfully set</returns>
        public static bool SetValue(this object obj, string propName, object value)
        {
            if (obj == null)
                return false;

            var property = obj.GetType().GetProperty(propName);

            if(property == null)
                return false;

            var currentVal = property.GetValue(obj);

            if(currentVal.Equals(value))
                return false;

            property.SetValue(obj, value);

            return true;
        }

        /// <summary>
        /// Get the value of a named property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static T GetValue<T>(this object obj, string propertyName)
        {
            var type    = obj.GetType();
            var property = type.GetProperty(propertyName);

            if(property == null)
                return default(T);

            var val = property.GetValue(obj);

            if(!val.GetType().IsEquivalentTo(typeof(T)))
                return (T)Convert.ChangeType(val, typeof(T));

            return (T)val;
        }

        /// <summary>
        /// Get the value of a named property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static T GetValue<T, U>(this U obj, string propertyName)
        {
            var type    = obj.GetType();
            var property = type.GetProperty(propertyName);

            if(property == null)
                return default(T);

            var val = property.GetValue(obj);

            if(!val.GetType().IsEquivalentTo(typeof(T)))
                return (T)Convert.ChangeType(val, typeof(T));

            return (T)val;
        }

        /// <summary>
        /// Set the value of multiple properties
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="properties">A POCO, anon object or dictionary that contains a set of key value pairs</param>
        /// <returns></returns>
        public static bool SetValues(this object obj, object properties)
        {
            if (obj == null)
                return false;

            var dProps  = properties.ToDictionary();
            var type    = obj.GetType();
            var changed = false;

            foreach(var kv in dProps)
            { 
                var property = type.GetProperty(kv.Key);

                if(property == null)
                    continue;

                var currentVal = property.GetValue(obj);
                object newVal = kv.Value;

                if(!currentVal.GetType().IsEquivalentTo(newVal.GetType()))
                    newVal = Convert.ChangeType(newVal, currentVal.GetType());

                if(currentVal.Equals(newVal))
                    continue;

                try
                { 
                    property.SetValue(obj, newVal);

                    changed = true;
                }
                catch
                {
                }
            }

            return changed;
        }

        private static void AppendValue(IDictionary<string, string> dict, string prefix, object obj, bool childrenAsJson)
        {
            if (obj == null)
                return;

            var objDict = obj.ToDictionary();

            foreach(var key in objDict.Keys)
            { 
                var val = objDict[key];

                if(val is DateTime dtVal)
                {
                    dict.Add(prefix + key, dtVal.ToString("s"));
                    continue;
                }

                if(val is DateTimeOffset dtoVal)
                {
                    dict.Add(prefix + key, dtoVal.ToString("s"));
                    continue;
                }

                if(val.GetType().IsPrimitive || val is string)
                {
                    dict.Add(prefix + key, val.ToString());
                    continue;
                }

                if(childrenAsJson)
                { 
                    var json = JsonConvert.SerializeObject(val);

                    dict.Add(prefix + key, json);

                    continue;
                }

                AppendValue(dict, prefix + key + ".", val, false);
            }

            return;
        }
    }
}
