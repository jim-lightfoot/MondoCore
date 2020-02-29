/****************************************************************************/
/*                                                                          */
/*    The MondoCore Libraries  						                        */
/*                                                                          */
/*        Namespace: MondoCore.Security.Encryption				            */
/*             File: SymmetricEncryptorFactory.cs							*/
/*        Class(es): SymmetricEncryptorFactory							    */
/*          Purpose: A factory to produce SymmetricEncryptors               */
/*                                                                          */
/*  Original Author: Jim Lightfoot                                          */
/*    Creation Date: 14 Jan 2020                                            */
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

using MondoCore.Common;

namespace MondoCore.Security.Encryption
{
    /****************************************************************************/
    /****************************************************************************/
    public class SymmetricEncryptorFactory : IEncryptorFactory
    {
        private readonly ICache      _cache;
        private readonly IKeyFactory _keyFactory;

        /****************************************************************************/
        public SymmetricEncryptorFactory(ICache cache, IKeyFactory keyFactory)
        {
            _cache      = cache;
            _keyFactory = keyFactory;
        }

        #region IEncryptorFactory

        /****************************************************************************/
        public async Task<IEncryptor> GetValidForDecryption(Guid keyId)
        {
            // See if there's one in the cache
            return await _cache.Get<IEncryptor>("Decrypt/" + keyId.ToId(), async ()=>
            {
                // Create a new encryptor with an existing policy and key
                var key = await _keyFactory.GetDecryptionKey(keyId);

                return new SymmetricEncryptor(key);
            },
            tsExpires: new TimeSpan(0, 30, 0) // Expire after 30 minutes
            );
        }

        /****************************************************************************/
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

            encryptor = new SymmetricEncryptor(key);

            // Add it to the cache
            await _cache.Add("Encrypt", encryptor, new TimeSpan(0, 30, 0));

            return encryptor;
        }

        #endregion
    }
}
