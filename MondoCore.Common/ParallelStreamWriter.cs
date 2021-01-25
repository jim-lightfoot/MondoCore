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
    public class ParallelStreamWriter : ParallelProcessor<Stream>
    {
        private readonly Stream _output;

        public ParallelStreamWriter(Stream output, CancellationToken cancel = default)
        : base(WriteStream, cancel)
        {
            _output = output;
        }

        private static async Task WriteStream(ParallelProcessor<Stream> self, long index, Stream strm)
        {
            strm.Seek(0, SeekOrigin.Begin);

            await strm.CopyToAsync((self as ParallelStreamWriter)._output).ConfigureAwait(false);
        }

        public ParallelTask<Stream> CreateSubStream()
        {
            return CreateParallelTask(new MemoryStream());
        }

        public async override Task WaitComplete()
        {
            await base.WaitComplete();

            if(_output.CanSeek)
                _output.Seek(0, SeekOrigin.Begin);
        }
    }
}
