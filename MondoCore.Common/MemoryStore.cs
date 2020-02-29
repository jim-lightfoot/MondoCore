/****************************************************************************/
/*                                                                          */
/*    The MondoCore Libraries  	                                            */
/*                                                                          */
/*      Namespace: MondoCore.Common	                                        */
/*           File: MemoryStore.cs                                           */
/*      Class(es): MemoryStore                                              */
/*        Purpose: In memory implementation of IBlobStore                   */
/*                                                                          */
/*  Original Author: Jim Lightfoot                                          */
/*    Creation Date: 29 Jan 2020                                            */
/*                                                                          */
/*   Copyright (c) 2020 - Jim Lightfoot, All rights reserved               */
/*                                                                          */
/*  Licensed under the MIT license:                                         */
/*    http://www.opensource.org/licenses/mit-license.php                    */
/*                                                                          */
/****************************************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MondoCore.Common
{
    /****************************************************************************/
    /****************************************************************************/
    public class MemoryStore : IBlobStore
    {
        private readonly ConcurrentDictionary<string, object> _store = new ConcurrentDictionary<string, object>();

        /****************************************************************************/
        public void Clear()
        {
            _store.Clear();
        }

        /****************************************************************************/
        public Task<string> Get(string id, Encoding encoding = null)
        {
             return Task.FromResult(_store[id].ToString());
        }

        /****************************************************************************/
        public Task<byte[]> GetBytes(string id)
        {
             return Task.FromResult(_store[id] as byte[]);
        }

        /****************************************************************************/
        public Task Put(string id, string blob)
        {
            _store[id] = blob;

            return Task.CompletedTask;
        }

        /****************************************************************************/
        public Task Delete(string id)
        {
            _store.TryRemove(id, out object val);

            return Task.CompletedTask;
        }

        /****************************************************************************/
        public Task<IEnumerable<string>> Find(string filter)
        {
            var keys = _store.Keys as IEnumerable<string>;

            return Task.FromResult(keys);
        }
    }
}
