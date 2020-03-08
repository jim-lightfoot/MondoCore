/***************************************************************************
 *                                                                          
 *    The MondoCore Libraries  						                        
 *                                                                          
 *        Namespace: MondoCore.Security.Encryption				            
 *             File: RotatingEncryptorFactory.cs							
 *        Class(es): RotatingEncryptorFactory							    
 *          Purpose: A factory to produce encryptors that rotate (expire)               
 *                                                                          
 *  Original Author: Jim Lightfoot                                          
 *    Creation Date: 7 Feb 2020                                            
 *                                                                          
 *   Copyright (c) 2020 - Jim Lightfoot, All rights reserved                
 *                                                                          
 *  Licensed under the MIT license:                                         
 *    http://www.opensource.org/licenses/mit-license.php                    
 *                                                                          
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using MondoCore.Common;

namespace MondoCore.Security.Encryption
{
    /****************************************************************************/
    /****************************************************************************/
    /// <summary>
    /// A factory to produce encryptors that rotate (expire) 
    /// </summary>
    public class RotatingEncryptorFactory : IRotatingEncryptorFactory
    {
        private readonly ICache      _cache;
        private readonly IEncryptorFactory _encryptorFactory;
        private readonly IKeyFactory _keyFactory;

        /****************************************************************************/
        public RotatingEncryptorFactory(IEncryptorFactory encryptorFactory, ICache cache, IKeyFactory keyFactory)
        {
            _encryptorFactory = encryptorFactory;
            _cache            = cache;
            _keyFactory       = keyFactory;
        }

        #region IRotatingEncryptorFactory

        /****************************************************************************/
        /// <summary>
        /// Retrieves a the encryptor specified by the given id
        /// </summary>
        /// <param name="keyId"></param>
        /// <returns></returns>
        public async Task<IEncryptor> GetValidForDecryption(Guid keyId)
        {
            // See if there's one in the cache
            return await _cache.Get<IEncryptor>("Decrypt/" + keyId.ToId(), async ()=>
            {
                // Create a new encryptor with an existing policy and key
                var key = await _keyFactory.GetDecryptionKey(keyId);

                return _encryptorFactory.Create(key);
            },
            tsExpires: new TimeSpan(0, 30, 0) // Expire after 30 minutes
            );
        }

        /****************************************************************************/
        /// <summary>
        /// Returns an encryptor that is valid for encryption (has not expired)
        /// </summary>
        /// <returns></returns>
        public async Task<IEncryptor> GetValidForEncryption()
        {
            IEncryptor encryptor = null;

            try
            { 
                encryptor = await _cache.Get("Encrypt") as IEncryptor;
            }
            catch
            {
                // Not in cache
            }

            // If we have valid encryptor in cache then use it
            if(encryptor != null && !encryptor.Policy.IsExpired)
                return encryptor;

            // Encryptor is expired remove from cache
            if(encryptor != null)
                await _cache.Remove("Encrypt");

            // Get a new (or existing) encryption key from the factory
            var key = await _keyFactory.GetEncryptionKey();

            encryptor = _encryptorFactory.Create(key);

            // Add it to the cache
            await _cache.Add("Encrypt", encryptor, new TimeSpan(0, 30, 0));

            return encryptor;
        }

        #endregion
    }
}
