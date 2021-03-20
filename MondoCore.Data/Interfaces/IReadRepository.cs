/***************************************************************************
 *                                                                          
 *    The MondoCore Libraries  	                                            
 *                                                                          
 *      Namespace: MondoCore.Data	                                        
 *           File: IReadRepository.cs                                                
 *      Class(es): IReadRepository                                                   
 *        Purpose: Interface to query a repository                               
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
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MondoCore.Data
{
    public interface IIdentifiable<TID> 
    {
        TID Id { get; }
    }

    public interface IPartitionable<TID> : IIdentifiable<TID> 
    {
        string GetPartitionKey();
    }

    /// <summary>
    /// Provides an interface for all read operations to a repository
    /// </summary>
    /// <typeparam name="TID">The type of the indentifier</typeparam>
    /// <typeparam name="TValue">The type of the object stored in the repository</typeparam>
    public interface IReadRepository<TID, TValue> : IQueryable<TValue> where TValue : IIdentifiable<TID>
    {
        Task<TValue>                Get(TID id);
        IAsyncEnumerable<TValue>    Get(IEnumerable<TID> ids);
        IAsyncEnumerable<TValue>    Get(Expression<Func<TValue, bool>> query);
    }
}
