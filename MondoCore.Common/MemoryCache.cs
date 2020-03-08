/****************************************************************************
 *                                                                           
 *    The MondoCore Libraries  	                                             
 *                                                                           
 *      Namespace: MondoCore.Common	                                         
 *           File: ICache.cs                                                 
 *      Class(es): ICache, ICacheDependency, FileDependency                  
 *        Purpose: Generic cache interface                                   
 *                                                                           
 *  Original Author: Jim Lightfoot                                           
 *    Creation Date: 29 Nov 2015                                             
 *                                                                           
 *   Copyright (c) 2015-2020 - Jim Lightfoot, All rights reserved            
 *                                                                           
 *  Licensed under the MIT license:                                          
 *    http://www.opensource.org/licenses/mit-license.php                     
 *                                                                           
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace MondoCore.Common
{
    /****************************************************************************/
    /****************************************************************************/
    /// <summary>
    /// In memory implementation of an ICache
    /// </summary>
    public class MemoryCache : ICache
    {
        private readonly ConcurrentDictionary<string, object> _cache = new ConcurrentDictionary<string, object>();

        /****************************************************************************/
        public MemoryCache()
        {
        }

        /****************************************************************************/
        /// <summary>
        /// Get the value form the cache
        /// </summary>
        public Task<object> Get(string key)
        {
            return Task.FromResult(_cache[key]);
        }
                 
        /****************************************************************************/
        public Task Add(string key, object objToAdd)
        {
            _cache[key] = objToAdd;

            return Task.CompletedTask;
        }

        /****************************************************************************/
        public Task Add(string key, object objToAdd, DateTime dtExpires, ICacheDependency dependency = null)
        {
            _cache[key] = objToAdd;

            return Task.CompletedTask;
        }

        /****************************************************************************/
        public Task Add(string key, object objToAdd, TimeSpan tsExpires, ICacheDependency dependency = null)
        {
            _cache[key] = objToAdd;

            return Task.CompletedTask;
        }
                          
        /****************************************************************************/
        public Task Remove(string key)
        {
            _cache.TryRemove(key, out object _);

            return Task.CompletedTask;
        }
    }
}