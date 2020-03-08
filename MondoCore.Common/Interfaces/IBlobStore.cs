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
 *   Copyright (c) 2015-2020 - Jim Lightfoot, All rights reserved            
 *                                                                           
 *  Licensed under the MIT license:                                          
 *    http://www.opensource.org/licenses/mit-license.php                     
 *                                                                           
 ****************************************************************************/

using System;
using System.Collections.Generic;
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
        /// Puts the string into the blob storage
        /// </summary>
        /// <param name="id">An identifier for the blob. This could be a path in file storage for instance</param>
        /// <param name="blob">The string to store</param>
        Task Put(string id, string blob);

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
    }
}
