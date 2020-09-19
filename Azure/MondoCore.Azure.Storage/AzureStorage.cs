/***************************************************************************
 *                                                                          
 *    The MondoCore Libraries  							                    
 *                                                                          
 *        Namespace: MondoCore.AzurStorage				            
 *             File: AzureStorage.cs			 		    		    
 *        Class(es): AzureStorage				           		        
 *          Purpose: Class for blob storage in Azure Storge                           
 *                                                                          
 *  Original Author: Jim Lightfoot                                          
 *    Creation Date: 15 Aug 2020                                             
 *                                                                          
 *   Copyright (c) 2020 - Jim Lightfoot, All rights reserved                
 *                                                                          
 *  Licensed under the MIT license:                                         
 *    http://www.opensource.org/licenses/mit-license.php                    
 *                                                                          
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureBlobs = Azure.Storage.Blobs;

using MondoCore.Common;

namespace MondoCore.Azure.Storage
{
    /****************************************************************************/
    /****************************************************************************/
    /// <summary>
    /// Azure blob storage
    /// </summary>
    public class AzureStorage : IBlobStore
    {
        private readonly BlobContainerClient _container;
        private readonly string _folder;

        public AzureStorage(string connectionString, string blobContainerName)
        {
            var folderParts = blobContainerName.Split('/');

            _container = new BlobContainerClient(connectionString, folderParts[0]);

            if(folderParts.Length > 1)
                _folder = string.Join("/", folderParts.Skip(1)).EnsureEndsWith("/");
            else
                _folder = "";
        }

        #region IBlobStore

        /// <summary>
        /// Gets a blob with the given id/path
        /// </summary>
        /// <param name="id">An identifier for the blob. This could be a path.</param>
        /// <param name="encoding">A text encode to use to encode the text</param>
        /// <returns>A string that is the blob</returns>
        public async Task<string> Get(string id, Encoding encoding = null)
        {
            try
            { 
                var blob     = _container.GetBlobClient(_folder + id);
                var response = await blob.DownloadAsync();

                return await response.Value.Content.ReadStringAsync(encoding);
            }
            catch(RequestFailedException ex)
            {
                if(ex.Status == 404)
                    throw new FileNotFoundException("Blob not found", ex);

                throw;
            }
        }

        /// <summary>
        /// Gets a blob with the given id/path
        /// </summary>
        /// <param name="id">An identifier for the blob. This could be a path.</param>
        /// <returns>The blob as an array of bytes</returns>
        public async Task<byte[]> GetBytes(string id)
        {
            try
            { 
                var blob = _container.GetBlobClient(_folder + id);

                using(var mem = new MemoryStream())
                { 
                    await blob.DownloadToAsync(mem);

                    return mem.ToArray();
                }
            }
            catch(RequestFailedException ex)
            {
                if(ex.Status == 404)
                    throw new FileNotFoundException("Blob not found", ex);

                throw;
            }
        }

        /// <summary>
        /// Puts the string into the blob storage
        /// </summary>
        /// <param name="id">The identifier for the blob. This could be a path.</param>
        /// <param name="blob">The string to store</param>
        public async Task Put(string id, string contents, Encoding encoding = null)
        {
            encoding = encoding ?? UTF8Encoding.UTF8;

            using(var mem = new MemoryStream(encoding.GetBytes(contents)))
            { 
                await Put(id, mem);
            }
        }

        /// <summary>
        /// Puts the string into the blob storage
        /// </summary>
        /// <param name="id">An identifier for the blob. This could be a path.</param>
        /// <param name="contents">The contents to store</param>
        public Task Put(string id, Stream contents)
        {
            var blob = _container.GetBlobClient(_folder + id);

            return blob.UploadAsync(contents);
        }

        /// <summary>
        /// Deletes the blob from storage
        /// </summary>
        /// <param name="id">An identifier for the blob. This could be a path.</param>
        public async Task Delete(string id)
        {
            try
            { 
                await _container.DeleteBlobIfExistsAsync(_folder + id);
            }
            catch(RequestFailedException ex)
            {
                if(ex.Status == 404)
                    return;

                throw;
            }
        }

        /// <summary>
        /// Finds all blobs that meet the filter 
        /// </summary>
        /// <param name="filter">A file path type filter, e.g. "Policies*.*"</param>
        /// <returns>A collection of the blob ids/paths</returns>
        public async Task<IEnumerable<string>> Find(string filter)
        {
            var result = new List<string>();

            await this.Enumerate(filter, async (blob)=>
            {
                result.Add(blob.Name);

                await Task.CompletedTask;
            },
            
            false);

            return result;
        }

        /// <summary>
        /// Finds all blobs that meet the filter 
        /// </summary>
        /// <param name="filter">A file path type filter, e.g. "Policies*.*"</param>
        /// <returns>A collection of the blob ids/paths</returns>
        public async Task Enumerate(string filter, Func<IBlob, Task> fnEach, bool asynchronous = true)
        {
            var pages = _container.GetBlobs(AzureBlobs.Models.BlobTraits.Metadata).AsPages();
            List<Task> tasks = asynchronous ? new List<Task>() : null;

            foreach(var page in pages)
            {
                var blobs = page.Values;

                foreach(var blob in blobs)
                {
                    if(blob.Name.MatchesWildcard(filter) && (string.IsNullOrWhiteSpace(_folder) || blob.Name.StartsWith(_folder, StringComparison.InvariantCultureIgnoreCase)))
                    { 
                        var ablob = new AzureBlob(blob);

                        ablob.Name = ablob.Name.Substring(_folder.Length);

                        var task = fnEach(ablob);

                        if(asynchronous)
                            tasks.Add(task);
                        else
                            await task;
                    }
                }
            }

            if(asynchronous)
                await Task.WhenAll(tasks);

            return;
        }

        #endregion

        #region Private

        private class AzureBlob : IBlob
        { 
            private readonly BlobItem _blob;

            internal AzureBlob(BlobItem blob)
            {
                _blob = blob;
                this.Name = blob.Name;
            }

            public string                      Name     { get; set; }
            public bool                        Deleted  => _blob.Deleted;
            public IDictionary<string, string> Metadata => _blob.Metadata;
            public IDictionary<string, string> Tags     => _blob.Tags;
        }

        #endregion
    }
}
