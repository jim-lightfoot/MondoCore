/***************************************************************************
 *                                                                          
 *    The MondoCore Libraries  							                    
 *                                                                          
 *        Namespace: MondoCore.Data							            
 *             File: IEnumerableExtensions.cs					    		        
 *        Class(es): IEnumerableExtensions				         		            
 *          Purpose: Extensions for IEnumerable           
 *                                                                          
 *  Original Author: Jim Lightfoot                                          
 *    Creation Date: 15 Jun 2021                                             
 *                                                                          
 *   Copyright (c) 2021 - Jim Lightfoot, All rights reserved           
 *                                                                          
 *  Licensed under the MIT license:                                         
 *    http://www.opensource.org/licenses/mit-license.php                    
 *                                                                          
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MondoCore.Data
{
    /****************************************************************************/
    /****************************************************************************/
    public static class IEnumerableExtensions
    {
        /****************************************************************************/
        public static async IAsyncEnumerable<T> ToListAsync<T>(this IEnumerable<T> list)
        {
            foreach(var item in list)
            {
                yield return await Task.FromResult(item);
            }
        }
    }
}
