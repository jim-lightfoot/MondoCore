/***************************************************************************
 *                                                                          
 *    The MondoCore Libraries  	                                            
 *                                                                          
 *      Namespace: MondoCore.Data	                                        
 *           File: IWriteRepository.cs                                                
 *      Class(es): IWriteRepository                                                   
 *        Purpose: Interface to write to a repository                               
 *                                                                          
 *  Original Author: Jim Lightfoot                                         
 *    Creation Date: 24 Feb 2021                                           
 *                                                                          
 *   Copyright (c) 2021 - Jim Lightfoot, All rights reserved                
 *                                                                          
 *  Licensed under the MIT license:                                         
 *    http://www.opensource.org/licenses/mit-license.php                    
 *                                                                          
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MondoCore.Data
{
    /// <summary>
    /// Provides an interface for all modification operations to a repository
    /// </summary>
    /// <typeparam name="TID">The type of the indentifier</typeparam>
    /// <typeparam name="TValue">The type of the object stored in the repository</typeparam>
    public interface IWriteRepository<TID, TValue> where TValue : IIdentifiable<TID>
    {
        Task<TValue> Insert(TValue item);
        Task         Insert(IEnumerable<TValue> items);
        
        Task<bool>   Update(TValue item, Expression<Func<TValue, bool>> guard = null);
        Task<long>   Update(object properties, Expression<Func<TValue, bool>> query);
        
        Task<bool>   Delete(TID id);
        Task<long>   Delete(Expression<Func<TValue, bool>> guard);
     }
}
