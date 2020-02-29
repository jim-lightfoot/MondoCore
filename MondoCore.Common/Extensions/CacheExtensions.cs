/****************************************************************************/
/*                                                                          */
/*    The MondoCore Libraries  	                                            */
/*                                                                          */
/*      Namespace: MondoCore.Common	                                        */
/*           File: CacheExtensions.cs                                       */
/*      Class(es): CacheExtensions                                          */
/*        Purpose: Extension methods for ICache                             */
/*                                                                          */
/*  Original Author: Jim Lightfoot                                          */
/*    Creation Date: 29 Nov 2015                                            */
/*                                                                          */
/*   Copyright (c) 2015-2020 - Jim Lightfoot, All rights reserved           */
/*                                                                          */
/*  Licensed under the MIT license:                                         */
/*    http://www.opensource.org/licenses/mit-license.php                    */
/*                                                                          */
/****************************************************************************/

using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MondoCore.Common
{
    /****************************************************************************/
    /****************************************************************************/
    public static class CacheExtensions 
    {
        /****************************************************************************/
        public static async Task<string> GetString(this ICache cache, string key)
        {
            return (await cache.Get(key))?.ToString();
        }

        /****************************************************************************/
        public static async Task<T> Get<T>(this ICache cache, string key, Func<Task<T>> fnCreate, Func<Exception, Task> onError = null, DateTime? dtExpires = null, TimeSpan? tsExpires = null, ICacheDependency dependency = null)
        {
            object obj = null;
            
            try
            {
                obj = await cache.Get(key);
            }
            catch
            {
                // Not in cache or other retrieval error
            }

            T tobj = (T)obj;

            if(obj == null)
            { 
                tobj = await fnCreate();

                // We can just fire and forget adding it to the cache
                _ = Task.Run( async ()=>
                {
                    try
                    { 
                        if(dtExpires != null)
                            await cache.Add(key, tobj, dtExpires.Value, dependency);
                        else if(tsExpires != null)
                            await cache.Add(key, tobj, tsExpires.Value, dependency);
                        else
                            await cache.Add(key, tobj);
                    }
                    catch(Exception ex)
                    {
                        if(onError != null)
                            await onError(ex);
                    }

                }).ConfigureAwait(false);
            }

            return tobj;
        }
    }
}
