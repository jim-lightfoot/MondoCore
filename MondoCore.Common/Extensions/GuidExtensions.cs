/*************************************************************************** 
 *                                                                           
 *    The MondoCore Libraries  							                     
 *                                                                           
 *        Namespace: MondoCore.Common							             
 *             File: GuidExtensions.cs					    		         
 *        Class(es): GuidExtensions				         		             
 *          Purpose: Extensions for guids                  
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

namespace MondoCore.Common
{
    /****************************************************************************/
    /****************************************************************************/
    /// <summary>
    /// Extensions for guids 
    /// </summary>
    public static class GuidExtensions
    {
        /// <summary>
        /// Normalizes the guid into a string
        /// </summary>
        public static string ToId(this Guid guid)
        {
            return guid.ToString().ToLower();
        }
    }
}
