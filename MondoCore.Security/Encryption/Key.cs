/***************************************************************************
 *                                                                          
 *    The MondoCore Libraries  						                        
 *                                                                          
 *        Namespace: MondoCore.Security.Encryption			                
 *        Class(es): Key										            
 *          Purpose: An encrption key                                       
 *                                                                          
 *  Original Author: Jim Lightfoot                                          
 *    Creation Date: 11 Jan 2020                                            
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

using MondoCore.Common;

namespace MondoCore.Security.Encryption
{
    /****************************************************************************/
    /****************************************************************************/
    public class Key : IKey 
    {
        private readonly byte[] _keyBytes;
        private readonly EncryptionPolicy _policy;

        /****************************************************************************/
        /// <summary>
        /// Initialize a Key with existing data
        /// </summary>
        /// <param name="policy">Matching policy of existing key</param>
        /// <param name="keyBytes">Data containing key</param>
        public Key(EncryptionPolicy policy, byte[] keyBytes) 
        {
            if(keyBytes.Length != (policy.KeySize/8))
                throw new ArgumentException("Key size does not match policy");

            _policy   = policy;
            _keyBytes = keyBytes.DeepClone();
        }

        /****************************************************************************/
        /// <summary>
        /// Generate a new key using the given policy
        /// </summary>
        /// <param name="policy">Policy to create key with</param>
        public Key(EncryptionPolicy policy)
        {
            _policy = policy;

             // Reset expiration date to original
             _policy.IsReadOnly = false;
             _policy.Expires    = policy.Expires;
             _policy.IsReadOnly = true;

            _keyBytes = SymmetricEncryptor.GenerateKey(_policy);
        }

        /****************************************************************************/
        public Guid             Id     => this.Policy.Id; // Key id and policy id are the same
        public EncryptionPolicy Policy => _policy;

        /****************************************************************************/
        public byte[] ToArray()
        {
            return _keyBytes;
        }

        /****************************************************************************/
        public void Dispose()
        {
            Array.Clear(_keyBytes, 0, _keyBytes.Length);
        }
    }
}
