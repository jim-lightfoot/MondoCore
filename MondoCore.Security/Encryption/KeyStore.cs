/****************************************************************************/
/*                                                                          */
/*    The MondoCore Libraries  							                    */
/*                                                                          */
/*        Namespace: MondoCore.Security.Encryption				            */
/*             File: KeyStore.cs					    	                */
/*        Class(es): KeyStore				         		                */
/*          Purpose: Encodes and encrypts a key and it's policy             */
/*                                                                          */
/*  Original Author: Jim Lightfoot                                          */
/*    Creation Date: 23 Jan 2020                                            */
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MondoCore.Common;

using Newtonsoft.Json;

namespace MondoCore.Security.Encryption
{
    /****************************************************************************/
    /****************************************************************************/
    public class KeyStore : IKeyStore
    {   
        private readonly IBlobStore _blobStore;
        private readonly IEncryptor _keyEncryptor;

        /****************************************************************************/
        public KeyStore(IBlobStore blobStore, IEncryptor keyEncryptor)
        {
            _blobStore = blobStore;
            _keyEncryptor = keyEncryptor;
        }

        #region IKeyStore

        /****************************************************************************/
        public async Task<IKey> Get(Guid id)
        {
            var    encoded    = await _blobStore.Get(id.ToId());  
            var    encrypted  = Convert.FromBase64String(encoded);
            var    decrypted  = await _keyEncryptor.Decrypt(encrypted);
            var    policy     = new EncryptionPolicy { Id = id };
            byte[] keyBytes   = null;
            long   expires    = 0L;
            string policyData = null;

            using(var memStream = new MemoryStream(decrypted))
            {
                using(var reader = new BinaryReader(memStream))
                {
                    policy.KeySize   = reader.ReadInt32();
                    policy.BlockSize = reader.ReadInt32();

                    keyBytes   = reader.ReadBytes(policy.KeySize / 8);
                    expires    = reader.ReadInt64();
                    policyData = reader.ReadString();
                }
            }

            var policyStr = policyData.Split(new string[] { ";" }, StringSplitOptions.None);

            policy.Expires   = new DateTime(expires);
            policy.Padding   = policyStr[0];
            policy.Mode      = policyStr[1];
            policy.Algorithm = policyStr[2];

            var key = new Key(policy, keyBytes);

            Array.Clear(keyBytes, 0, keyBytes.Length);

            return key;
        }

        public class KeyNotFoundException : Exception { }

        /****************************************************************************/
        public async Task Add(IKey key)
        {
            var list = new List<string>();

            list.Add(key.Policy.Padding);
            list.Add(key.Policy.Mode);
            list.Add(key.Policy.Algorithm);

            var policyData = string.Join(";", list);
            byte[] output = null;

            using(var memStream = new MemoryStream())
            {
                using(var writer = new BinaryWriter(memStream))
                {
                    writer.Write(key.Policy.KeySize);
                    writer.Write(key.Policy.BlockSize.Value);
                    writer.Write(key.ToArray());
                    writer.Write(key.Policy.Expires.Ticks);
                    writer.Write(policyData);

                    writer.Flush();
                }

                output = memStream.ToArray();
            }

            // Encrypt the entire package
            var cipherData = await _keyEncryptor.Encrypt(output);

            Array.Clear(output, 0, output.Length);

            try
            { 
                // Store package
                await _blobStore.Put(key.Id.ToId(), Convert.ToBase64String(cipherData));
            }
            finally
            { 
                Array.Clear(cipherData, 0, cipherData.Length);
            }
        }

        /****************************************************************************/
        public async Task<IEnumerable<IKey>> GetAll()
        {
            var ids  = await _blobStore.Find("*");
            var keys = new List<IKey>();

            if(ids != null && ids.Count() > 0)
            {
                foreach(var id in ids)
                {
                    keys.Add(await Get(Guid.Parse(id)));
                }
            }

            return keys;
        }

        /****************************************************************************/
        public async Task Remove(Guid id)
        {
            await _blobStore.Delete(id.ToId());
        }

        #endregion
    }
}
