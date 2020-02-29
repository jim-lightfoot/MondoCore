/****************************************************************************/
/*                                                                          */
/*    The MondoCore Libraries  						                        */
/*                                                                          */
/*        Namespace: MondoCore.Security.Encryption					        */
/*             File: RotatingKeyEncryptor.cs								*/
/*        Class(es): RotatingKeyEncryptor								    */
/*          Purpose: An encryptor that encrypts rotates keys                */
/*                                                                          */
/*  Original Author: Jim Lightfoot                                          */
/*    Creation Date: 11 Jan 2020                                            */
/*                                                                          */
/*   Copyright (c) 2020 - Jim Lightfoot, All rights reserved                */
/*                                                                          */
/*  Licensed under the MIT license:                                         */
/*    http://www.opensource.org/licenses/mit-license.php                    */
/*                                                                          */
/****************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using MondoCore.Common;

namespace MondoCore.Security.Encryption
{
    /****************************************************************************/
    /****************************************************************************/
    public class RotatingKeyEncryptor : IEncryptor
    {
        private readonly IEncryptorFactory _factory;
        private const int GuidSize = 16;

        /****************************************************************************/
        public RotatingKeyEncryptor(IEncryptorFactory factory)
        {
            _factory = factory;
        }

        // Since this is a multiple policy encryptor there is no one policy to return
        public EncryptionPolicy Policy => throw new NotSupportedException();

        #region IEncryptor

        /****************************************************************************/
        public async Task<byte[]> Decrypt(byte[] aEncrypted, int offset = 0)
        {
            var policyBytes = new byte[GuidSize];  
           
            Buffer.BlockCopy(aEncrypted, offset, policyBytes, 0, GuidSize); 
            
            var policyId  = new Guid(policyBytes);
            var encryptor = await _factory.GetValidForDecryption(policyId);

            return await encryptor.Decrypt(aEncrypted, offset + GuidSize);
        }

        /****************************************************************************/
        public async Task Decrypt(Stream input, Stream output)
        {
            var policyBytes = new byte[GuidSize];  

            await input.ReadAsync(policyBytes, 0, GuidSize);

            var policyId  = new Guid(policyBytes);
            var encryptor = await _factory.GetValidForDecryption(policyId);

            await encryptor.Decrypt(input, output);
        }

        /****************************************************************************/
        public async Task<byte[]> Encrypt(byte[] aData)
        {
            var encryptor   = await _factory.GetValidForEncryption();
            var cipherData  = await encryptor.Encrypt(aData);
            var policy      = encryptor.Policy;
            var policyBytes = policy.Id.ToByteArray();

            // Prepend the policy id to the cipher data for retrieval when decrypting
            return cipherData.Prepend(policyBytes);
        }

        /****************************************************************************/
        public async Task Encrypt(Stream input, Stream output)
        {
            var encryptor   = await _factory.GetValidForEncryption();
            var policy      = encryptor.Policy;
            var policyBytes = policy.Id.ToByteArray();
            
            // Prepend the policy id to the cipher data for retrieval when decrypting
            await output.WriteAsync(policyBytes, 0, policyBytes.Length);

            await encryptor.Encrypt(input, output);
        }

        #endregion
    }
}
