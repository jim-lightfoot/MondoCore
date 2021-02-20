/*************************************************************************** 
 *                                                                           
 *    The MondoCore Libraries  							                     
 *                                                                           
 *        Namespace: MondoCore.Common							             
 *             File: UriExtensions.cs					    		         
 *        Class(es): UriExtensions				         		             
 *          Purpose: Extensions for Uri                  
 *                                                                           
 *  Original Author: Jim Lightfoot                                           
 *    Creation Date: 15 Feb 2021                                              
 *                                                                           
 *   Copyright (c) 2021 - Jim Lightfoot, All rights reserved            
 *                                                                           
 *  Licensed under the MIT license:                                          
 *    http://www.opensource.org/licenses/mit-license.php                     
 *                                                                           
 ****************************************************************************/

using System;
using System.Collections.Generic;

namespace MondoCore.Common
{
    public static class UriExtensions
    {
        public static Uri Combine(this Uri uri, params string[] paths)
        {
            var parts = new List<string>();

            parts.Add(uri.AbsoluteUri);

            foreach(var path in paths)
            {
                if(!string.IsNullOrWhiteSpace(path))
                    parts.Add(path.Replace("/", "").Replace("\\", ""));
            }

            return new Uri(string.Join("/", parts));
        }
    }
}
