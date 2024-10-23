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

using MondoCore.Collections.Concurrent;

namespace MondoCore.Common
{
    /****************************************************************************/
    /****************************************************************************/
    /// <summary>
    /// In memory implementation of an ICache
    /// </summary>
    public class MemoryCache : ICache
    {
        private readonly ConcurrentKeyedQueue<string, CacheEntry> _cache = new();
        private readonly int _maxCount;
        private readonly int _flushThreshold = 100;

        /****************************************************************************/
        public MemoryCache(int maxCount = 10000, int flushThreshold = 100)
        {
            _maxCount = maxCount;
            _flushThreshold = flushThreshold;
        }

        /****************************************************************************/
        /// <summary>
        /// Get the value form the cache
        /// </summary>
        public Task<object> Get(string key)
        {
            if(_cache.ContainsKey(key))
            { 
                var entry = _cache[key];

                if(!entry.IsExpired)
                { 
                    entry.SetLastAccessed();
                    return Task.FromResult(entry.Item);
                }

                Remove(key);
            }

            return Task.FromResult((object)null);
        }
                 
        /****************************************************************************/
        public Task Add(string key, object objToAdd)
        {
            return Add( new CacheEntry { Key = key, Item = objToAdd });
        }

        /****************************************************************************/
        public Task Add(string key, object objToAdd, DateTime dtExpires, ICacheDependency dependency = null)
        {
            return Add( new CacheEntry { Key = key, Item = objToAdd, AbsoluteExpiration = dtExpires });
        }

        /****************************************************************************/
        public Task Add(string key, object objToAdd, TimeSpan tsExpires, ICacheDependency dependency = null)
        {
            return Add( new CacheEntry { Key = key, Item = objToAdd, SlidingExpiration = tsExpires });
        }
                          
        /****************************************************************************/
        public Task Remove(string key)
        {
            _cache.Remove(key);

            return Task.CompletedTask;
        }

        #region Private 

        /****************************************************************************/
        private Task Add(CacheEntry entry)
        {
            if(_cache.Count >= _maxCount)
            {
                // Too many items in cache. Remove the oldest ones
                _cache.Dequeue(_flushThreshold);
            }
        
            _cache.Enqueue(entry.Key, entry);

            return Task.CompletedTask;
        }

        /****************************************************************************/        
        private class CacheEntry
        {
            internal string    Key                { get; set; }
            internal object    Item               { get; set; }
            internal DateTime? AbsoluteExpiration { get; set; }
            internal TimeSpan? SlidingExpiration  { get; set; }

            private readonly object _lock = new object();
            private DateTimeOffset _lastAccessed = DateTimeOffset.UtcNow;

            internal bool IsExpired
            {
                get
                {
                    if(AbsoluteExpiration != null)
                    {
                        if(AbsoluteExpiration.Value.Kind == DateTimeKind.Utc)
                            return DateTime.UtcNow > AbsoluteExpiration.Value;

                        return DateTime.Now > AbsoluteExpiration.Value;
                    }

                    if(SlidingExpiration != null)
                    {
                        lock(_lock)
                        {
                            return DateTimeOffset.UtcNow > this.LastAccessed.Add(SlidingExpiration.Value);
                        }
                    }

                    return false;
                }
            }
            
            internal void SetLastAccessed()
            {
                lock(_lock)
                {
                    _lastAccessed = DateTimeOffset.UtcNow;
                }
            }
            
            private DateTimeOffset LastAccessed 
            {
                get
                { 
                    lock(_lock)
                    {
                        return _lastAccessed;
                    }
                }
            }
        }

        #endregion
    }
}