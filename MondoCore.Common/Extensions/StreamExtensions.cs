/*************************************************************************** 
 *                                                                           
 *    The MondoCore Libraries  							                     
 *                                                                           
 *        Namespace: MondoCore.Common							             
 *             File: StreamExtensions.cs					    		         
 *        Class(es): StreamExtensions				         		             
 *          Purpose: Extensions for Streams                  
 *                                                                           
 *  Original Author: Jim Lightfoot                                           
 *    Creation Date: 1 Jan 2020                                              
 *                                                                           
 *   Copyright (c) 2005-2020 - Jim Lightfoot, All rights reserved            
 *                                                                           
 *  Licensed under the MIT license:                                          
 *    http://www.opensource.org/licenses/mit-license.php                     
 *                                                                           
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MondoCore.Common
{
    /****************************************************************************/
    /****************************************************************************/
    public static class StreamExtensions
    {
        /****************************************************************************/
        /// <summary>
        /// Reads in from string and converts into a string
        /// </summary>
        /// <param name="stream">Stream to read from</param>
        /// <param name="encoder">Text encoder. Will default to UTFEncoding</param>
        /// <returns>The resulting string</returns>
        public static async Task<string> ReadStringAsync(this Stream stream, Encoding encoder = null)
        {
            if(encoder == null)
                encoder = UTF8Encoding.UTF8;

            if(stream.CanSeek)
                stream.Seek(0, SeekOrigin.Begin);

            try
            { 
                using(var mem = new MemoryStream())
                { 
                    await mem.WriteAsync(stream);

                    return encoder.GetString(mem.ToArray());
                }
            }
            finally
            { 
                if(stream.CanSeek)
                    stream.Seek(0, SeekOrigin.Begin);
            }
        }

        /****************************************************************************/
        /// <summary>
        /// Write a string to the stream
        /// </summary>
        /// <param name="stream">The stream to write to</param>
        /// <param name="data"></param>
        /// <param name="encoder">Text encoder. Will default to UTFEncoding</param>
        /// <returns></returns>
        public static Task WriteAsync(this Stream stream, string data, Encoding encoder = null)
        {
            if(encoder == null)
                encoder = UTF8Encoding.UTF8;

            byte[] bytes = encoder.GetBytes(data);
       
            return stream.WriteAsync(bytes, 0, bytes.Length);
        }

        /****************************************************************************/
        /// <summary>
        /// Write one stream to another
        /// </summary>
        /// <param name="dest">The stream to write to</param>
        /// <param name="src">The stream to read from</param>
        public static async Task WriteAsync(this Stream dest, Stream src)
        {
            const int BufferSize  = 65536;
            int       numRead     = 0;
            byte[]    buffer      = new byte[BufferSize];
            
            numRead = await src.ReadAsync(buffer, 0, BufferSize);
                
            while(numRead > 0)
            {
                await dest.WriteAsync(buffer, 0, numRead);
                numRead = await src.ReadAsync(buffer, 0, BufferSize);
            }
        }
    }
}
