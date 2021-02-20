/**************************************************************************
 *                                                                         
 *    The MondoCore Libraries  	                                           
 *                                                                         
 *      Namespace: MondoCore.Common	                                       
 *           File: MemoryStore.cs                                          
 *      Class(es): MemoryStore                                             
 *        Purpose: In memory implementation of IBlobStore                  
 *                                                                         
 *  Original Author: Jim Lightfoot                                         
 *    Creation Date: 29 Jan 2020                                           
 *                                                                         
 *   Copyright (c) 2020 - Jim Lightfoot, All rights reserved               
 *                                                                         
 *  Licensed under the MIT license:                                        
 *    http://www.opensource.org/licenses/mit-license.php                   
 *                                                                         
 ****************************************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MondoCore.Common
{
    /****************************************************************************/
    /****************************************************************************/
    /// <summary>
    /// In memory implementation of IBlobStore
    /// </summary>
    public class MemoryStore : IBlobStore
    {
        private readonly ConcurrentDictionary<string, byte[]> _store = new ConcurrentDictionary<string, byte[]>();

        /****************************************************************************/
        public void Clear()
        {
            _store.Clear();
        }

        public byte[] this[string key] => _store[key];
        public byte[] this[int key]    => _store.ToList()[key].Value;
        public int    Count            => _store.Count;

        #region IBlobStore

        /****************************************************************************/
        public async Task<string> Get(string id, Encoding encoding = null)
        {
            encoding = encoding ?? UTF8Encoding.UTF8;

            try
            { 
                var result = encoding.GetString(_store[id]);

                return await Task.FromResult(result);
            }
            catch
            {
                throw new FileNotFoundException();
            }
        }

        /****************************************************************************/
        public async Task<byte[]> GetBytes(string id)
        {
            try
            { 
                var result = _store[id] as byte[];

                return await Task.FromResult(result);
            }
            catch
            {
                throw new FileNotFoundException();
            }
        }

        /****************************************************************************/
        public async Task Get(string id, Stream destination)
        {
            if(!_store.ContainsKey(id))
                throw new FileNotFoundException();
                
            var result = _store[id] as byte[];

            await destination.WriteAsync(result, 0, result.Length);
        }
          
        /****************************************************************************/
        /// <inheritdoc/>
        public Task<Stream> OpenRead(string id)
        {
            if(!_store.ContainsKey(id))
                throw new FileNotFoundException();
                
            var blob = _store[id] as byte[];

            return Task.FromResult((Stream)new MemoryStream(blob, 0, blob.Length));
        }        

        /****************************************************************************/
        public Task Put(string id, string content, Encoding encoding = null)
        {
            encoding = encoding ?? UTF8Encoding.UTF8;

            _store[id] = encoding.GetBytes(content);

            return Task.CompletedTask;
        }

        /****************************************************************************/
        public Task Put(string id, Stream content)
        {
            _store[id] = content.ToArray();

            return Task.CompletedTask;
        }

        /****************************************************************************/
        public Task Delete(string id)
        {
            _store.TryRemove(id, out byte[] _);

            return Task.CompletedTask;
        }

        /****************************************************************************/
        public Task<IEnumerable<string>> Find(string filter)
        {
            var keys = _store.Keys as IEnumerable<string>;

            return Task.FromResult(keys.Where( k=> k.MatchesWildcard(filter) ));
        }


        /****************************************************************************/
        /// <inheritdoc/>
        public async Task Enumerate(string filter, Func<IBlob, Task> fnEach, bool asynchronous = true)
        {
            var list = await this.Find(filter);
            List<Task> tasks = asynchronous ? new List<Task>() : null;

            foreach(var file in list)
            {
                var task = fnEach(new FileStore.FileBlob(file));

                if(asynchronous)
                    tasks.Add(task);
                else
                    await task;
            }

            if(asynchronous)
                await Task.WhenAll(tasks);
        }

        public Task<Stream> OpenWrite(string id)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
