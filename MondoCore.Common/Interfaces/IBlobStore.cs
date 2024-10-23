/*************************************************************************** 
 *                                                                           
 *    The MondoCore Libraries  	                                             
 *                                                                           
 *      Namespace: MondoCore.Common	                                         
 *           File: IBlobStore.cs                                             
 *      Class(es): IBlobStore                                                
 *        Purpose: Generic interface for storing blobs                       
 *                                                                           
 *  Original Author: Jim Lightfoot                                           
 *    Creation Date: 29 Nov 2015                                             
 *                                                                           
 *   Copyright (c) 2015-2024 - Jim Lightfoot, All rights reserved            
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
    /// <summary>
    /// Interface for blob storage
    /// </summary>
    public interface IBlobStore
    {
        /// <summary>
        /// Gets a blob with the given id/path
        /// </summary>
        /// <param name="id">An identifier for the blob. This could be a path in file storage for instance</param>
        /// <param name="encoding">A text encode to use to encode the text</param>
        /// <returns>A string that is the blob</returns>
        Task<string> Get(string id, Encoding encoding = null);

        /// <summary>
        /// Gets a blob with the given id/path
        /// </summary>
        /// <param name="id">An identifier for the blob. This could be a path in file storage for instance</param>
        /// <returns>The blob as an array of bytes</returns>
        Task<byte[]> GetBytes(string id);

        /// <summary>
        /// Writes a blob with the given id/path to the given stream
        /// </summary>
        /// <param name="id">An identifier for the blob. This could be a path.</param>
        /// <param name="destination">Destination stream to write blob to</param>
        Task Get(string id, Stream destination);

        /// <summary>
        /// Opens a readonly stream to a blob with the given id/path 
        /// </summary>
        /// <param name="id">An identifier for the blob. This could be a path.</param>
        /// <returns>A readonly stream to read the blob from</returns>
        Task<Stream> OpenRead(string id);

        /// <summary>
        /// Opens a writable stream to a blob with the given id/path 
        /// </summary>
        /// <param name="id">An identifier for the blob. This could be a path.</param>
        /// <returns>A writable stream to write to the blob</returns>
        Task<Stream> OpenWrite(string id);

        /// <summary>
        /// Puts the string into the blob storage
        /// </summary>
        /// <param name="id">An identifier for the blob. This could be a path in file storage for instance</param>
        /// <param name="content">The string to store</param>
        Task Put(string id, string content, Encoding encoding = null);

        /// <summary>
        /// Puts the stream into the blob storage
        /// </summary>
        /// <param name="id">An identifier for the blob. This could be a path in file storage for instance</param>
        /// <param name="content">The content to store</param>
        Task Put(string id, Stream content);

        /// <summary>
        /// Deletes the blob from storage
        /// </summary>
        /// <param name="id">An identifier for the blob. This could be a path in file storage for instance</param>
        Task Delete(string id);

        /// <summary>
        /// Finds all blobs that meet the filter 
        /// </summary>
        /// <param name="filter">A file path type filter, e.g. "Policies*.*"</param>
        /// <returns>A collection of the blob ids/paths</returns>
        Task<IEnumerable<string>> Find(string filter);

        /// <summary>
        /// Enumerates on each blob and calls the given function for each
        /// </summary>
        /// <param name="filter">A file path type filter, e.g. "Policies*.*"</param>
        /// <param name="fnEach">A function to call with each blob</param>
        /// <returns></returns>
        Task Enumerate(string filter, Func<IBlob, Task> fnEach, bool asynchronous = true);
    }

    public interface IBlob
    {
        string                      Name        { get; }
        bool                        Deleted     { get; }
        IDictionary<string, string> Metadata    { get; }
        IDictionary<string, string> Tags        { get; }
    }
}
