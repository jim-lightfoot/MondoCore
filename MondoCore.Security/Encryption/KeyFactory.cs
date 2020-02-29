/****************************************************************************/
/*                                                                          */
/*    The MondoCore Libraries  							                    */
/*                                                                          */
/*        Namespace: MondoCore.Security.Encryption				            */
/*             File: KeyFactory.cs			 		    		            */
/*        Class(es): KeyFactory				           		                */
/*          Purpose: Class for producing encryption keys                    */
/*                                                                          */
/*  Original Author: Jim Lightfoot                                          */
/*    Creation Date: 19 Jan 2020                                            */
/*                                                                          */
/*   Copyright (c) 2020 - Jim Lightfoot, All rights reserved                */
/*                                                                          */
/*  Licensed under the MIT license:                                         */
/*    http://www.opensource.org/licenses/mit-license.php                    */
/*                                                                          */
/****************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MondoCore.Security.Encryption
{
    /****************************************************************************/
    /****************************************************************************/
    public class KeyFactory : IKeyFactory
    {   
        private readonly IKeyStore _decryptStore;
        private readonly IKeyStore _encryptStore;
        private readonly EncryptionPolicy _policyTemplate;
        private readonly TimeSpan _expires;

        /****************************************************************************/
        public KeyFactory(IKeyStore decryptStore, IKeyStore encryptStore, EncryptionPolicy policyTemplate, TimeSpan expires)
        {
            _decryptStore   = decryptStore;
            _encryptStore   = encryptStore;
            _policyTemplate = policyTemplate;
            _expires        = expires;
        }

        #region IKeyFactory

        /****************************************************************************/
        public async Task<IKey> GetDecryptionKey(Guid keyId)
        {
            var key = await _decryptStore.Get(keyId);

            if(key == null)
                throw new KeyNotFoundException();

            return key;
        }

        /****************************************************************************/
        public async Task<IKey> GetEncryptionKey()
        {
            try
            {
                // Get all keys in encryption store
                var keys = await _encryptStore.GetAll();

                if(keys != null)
                {
                    var expiredKeys = new List<Guid>();

                    try
                    { 
                        // Find one that isn't expired
                        foreach(var key in keys)
                        {
                            if(!key.Policy.IsExpired)
                                return key;

                            expiredKeys.Add(key.Id);
                        }
                    }
                    finally
                    {
                        // If we found expired keys then remove them
                        if(expiredKeys.Count > 0)
                        {
                            var tasks = new List<Task>();

                            foreach(var id in expiredKeys)
                                tasks.Add(RemoveKey(id));

                            await Task.WhenAll(tasks);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                _ = ex;
            }

            // Create a brand new key
            var newKey = new Key(_policyTemplate.Clone(_expires));

            newKey.Policy.IsReadOnly = true;

            // Save to both stores
            await _decryptStore.Add(newKey); // Make sure it's saved here successfully before putting it in encrypt store
            await _encryptStore.Add(newKey);

            return newKey;
        }

        #endregion

        #region Private

        private async Task RemoveKey(Guid id)
        {
            try
            {
                await _encryptStore.Remove(id);
            }
            catch
            {
                // Perhaps already removed by another thread/process
            }
        }
        #endregion
    }
}
