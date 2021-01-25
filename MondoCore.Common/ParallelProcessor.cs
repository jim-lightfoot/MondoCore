/***************************************************************************
 *                                                                          
 *    The MondoCore Libraries  							                    
 *                                                                          
 *        Namespace: MondoCore.Common				            
 *             File: ParallelStreamWriter.cs			 		    		    
 *        Class(es): ParallelStreamWriter				           		        
 *          Purpose: Writes to a stream in parallel by creating substreams for each parallel thread                      
 *                                                                          
 *  Original Author: Jim Lightfoot                                          
 *    Creation Date: 18 Jan 2021                                             
 *                                                                          
 *   Copyright (c) 2021 - Jim Lightfoot, All rights reserved                
 *                                                                          
 *  Licensed under the MIT license:                                         
 *    http://www.opensource.org/licenses/mit-license.php                    
 *                                                                          
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.IO;
using System.Threading;
using System.Text;
using System.Threading.Tasks;

namespace MondoCore.Common
{
    /****************************************************************************/
    /****************************************************************************/
    /// <summary>
    /// Writes to a stream in parallel by creating substreams for each parallel thread
    /// </summary>
    public class ParallelProcessor<T>
    {
        private readonly ConcurrentDictionary<int, T> _tasks = new ConcurrentDictionary<int, T>();

        private long                       _numTasks   = 0;
        private Task                       _scanner       = null;
        private volatile int               _lastCompleted = -1;
        private volatile bool              _finished      = false;
        private Exception                  _exError       = null;
        private readonly CancellationToken _cancel;
        private readonly Func<ParallelProcessor<T>, long, T, Task> _onEachItem;

        public ParallelProcessor(Func<ParallelProcessor<T>, long, T, Task> onEachItem, CancellationToken cancel = default)
        {
            _cancel     = cancel;
            _onEachItem = onEachItem;

            _scanner = Task.Run( async ()=>
            {
                try
                {
                    while(!_cancel.IsCancellationRequested && (!_finished || _tasks.Any() || Interlocked.Read(ref _numTasks) > (_lastCompleted+1)))
                    {
                        var next = _lastCompleted + 1;

                        if(!_tasks.ContainsKey(next))
                        {
                            await Task.Delay(10).ConfigureAwait(false);
                            continue;
                        }

                        var subItem = _tasks[next];

                        await _onEachItem(this, next, subItem);

                        _tasks.TryRemove(next, out T val);

                        ++_lastCompleted;
                    }
                }
                catch(Exception ex)
                {
                    _exError = ex;
                }
            });
        }

        public ParallelTask<T> CreateParallelTask(T data)
        {
            return new ParallelTask<T>(this, data, (int)Interlocked.Increment(ref _numTasks) - 1);
        }

        public ParallelTask<T> CreateParallelTask(int index, T data)
        {
            return new ParallelTask<T>(this, data, index);
        }

        public async virtual Task WaitComplete()
        {
            if(_numTasks == 0)
                return;

            _finished = true;

            await _scanner;

            if(_exError != null)
                throw _exError;
        }

        internal void SetReady(int index, T item)
        {
            _tasks.TryAdd(index, item);
        }
    }

    public class ParallelTask<T> : IDisposable
    {
        private readonly int                  _index;
        private readonly ParallelProcessor<T> _processor;

        public ParallelTask(ParallelProcessor<T> processor, T item, int index)
        {
            _processor = processor;
            _index     = index;
            this.Object    = item;
        }

        public T Object { get; }

        public void Dispose()
        {
            _processor.SetReady(_index, this.Object);
        }
    }
}
