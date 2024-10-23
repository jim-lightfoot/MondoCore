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
 *   Copyright (c) 2005-2024 - Jim Lightfoot, All rights reserved            
 *                                                                           
 *  Licensed under the MIT license:                                          
 *    http://www.opensource.org/licenses/mit-license.php                     
 *                                                                           
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;
using MondoCore.Collections;

namespace MondoCore.Common
{
    public static class ObjectExtensions
    {
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
                return default;

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
                return default;

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

                if(currentVal == null  && newVal == null)
                    continue;

                if(newVal != null && newVal.GetType() != property.PropertyType)
                    newVal = Convert.ChangeType(newVal, property.PropertyType);

                if(currentVal != null && currentVal.Equals(newVal))
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

        public static T1 Map<T2, T1>(this T2 obj) where T1 : class, new()
                                                  where T2 : class
        {
            if (obj == null)
                return null;

            var props  = obj.ToDictionary();
            var result = new T1();

            result.SetValues(props);

            return result;
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
