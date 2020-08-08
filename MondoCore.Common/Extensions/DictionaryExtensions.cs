/***************************************************************************
 *                                                                          
 *    The MondoCore Libraries  							                    
 *                                                                          
 *        Namespace: MondoCore.Common							            
 *             File: DictionaryExtensions.cs					    		        
 *        Class(es): DictionaryExtensions				         		            
 *          Purpose: Extensions for dictionaries                  
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
using System.IO;

namespace MondoCore.Common
{
    /****************************************************************************/
    /****************************************************************************/
    public static class DictionaryExtensions
    {
        /****************************************************************************/
        public static IDictionary<K, V> Merge<K, V>(this IDictionary<K, V> dict1, IDictionary<K, V> dict2)
        {
            if(dict2 == null)
                return dict1;
       
            if(dict1 == null)
                return dict2;
       
            foreach(var kv in dict2)
                dict1[kv.Key] = kv.Value;

            return dict1;
        }
    }
}
