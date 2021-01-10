/***************************************************************************
 *                                                                          
 *    The MondoCore Libraries  							                    
 *                                                                          
 *        Namespace: MondoCore.Security.Encryption  					    
 *             File: SymmetricEncryptor.cs					    		    
 *        Class(es): SymmetricEncryptor				         		        
 *          Purpose: Provides data encryption and decryption                
 *                                                                          
 *  Original Author: Jim Lightfoot                                          
 *    Creation Date: 1 Jan 2020                                             
 *                                                                          
 *   Copyright (c) 2020 - Jim Lightfoot, All rights reserved                
 *                                                                          
 *  Licensed under the MIT license:                                         
 *    http://www.opensource.org/licenses/mit-license.php                    
 *                                                                          
 ****************************************************************************/

using System;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using MondoCore.Common;

namespace MondoCore.Security.Encryption
{
    /****************************************************************************/
    /****************************************************************************/
	public class SymmetricEncryptor : IEncryptor, IDisposable
	{
        public const int DefaultKeySize = 0; // The default key size for the algorithm will be used
        private readonly EncryptionPolicy _policy;

		/****************************************************************************/
        public SymmetricEncryptor(IKey key)
		{
            this.Key = key;
            _policy  = this.Key.Policy;
		}

		/****************************************************************************/
        /// <summary>
        /// Creates a "keyless" encryptor. Can only be used to create a new key 
        /// </summary>
        private SymmetricEncryptor(EncryptionPolicy policy)
		{
            _policy = policy;
		}

        public IKey             Key     { get; } = null;
        public EncryptionPolicy Policy  => _policy;

        /****************************************************************************/
        protected virtual SymmetricAlgorithm CreateAlgorithm(EncryptionPolicy policy)
        {
            if(string.IsNullOrWhiteSpace(policy.Algorithm))
                return Aes.Create(); 

            return Aes.Create(policy.Algorithm);
        }

        #region GenerateKey

        /****************************************************************************/
        internal static byte[] GenerateKey(EncryptionPolicy policy)
		{
            using(var encryptor = new SymmetricEncryptor(policy))
            { 
                using(var algorithm = encryptor.GetAlgorithm(policy))
                {
                    SetKeySize(algorithm, policy.KeySize);

                    policy.IsReadOnly = false;
                    policy.KeySize    = algorithm.KeySize;
                    policy.Algorithm  = algorithm.GetType().ToString();
                    policy.IsReadOnly = true;

                    algorithm.GenerateKey();

                    return algorithm.Key.DeepClone();
                }
            }
        }

        #endregion

        /*************************************************************************/
        public void Dispose()
        {
            this.Key?.Dispose();
        }

        #region IEncryptor

        /****************************************************************************/
        public async Task<byte[]> Encrypt(byte[] aData)
        {
            if(this.Policy.IsExpired)
                throw new ExpiredPolicyException();

            // Prevent policy from being modified
            this.Policy.IsReadOnly = true;

            using(var algorithm = GetAlgorithm())
            {
                return await EncryptBytes(algorithm, aData).ConfigureAwait(false);
            }
        }

        /****************************************************************************/
        public async Task Encrypt(Stream input, Stream output)
        {
            if(this.Policy.IsExpired)
                throw new ExpiredPolicyException();

            // Prevent policy from being modified
            this.Policy.IsReadOnly = true;

            using(var algorithm = GetAlgorithm())
            {
                await EncryptStream(algorithm, input, output).ConfigureAwait(false);
            }

            if(output.CanSeek)
                output.Seek(0, SeekOrigin.Begin);
        }

        /****************************************************************************/
        public async Task<byte[]> Decrypt(byte[] aEncrypted, int offset = 0)
        {
            using(var algorithm = GetAlgorithm())
            {
                return await DecryptBytes(algorithm, aEncrypted, offset).ConfigureAwait(false);
            }
        }

        /****************************************************************************/
        public async Task Decrypt(Stream input, Stream output)
        {
            using(var algorithm = GetAlgorithm())
            {
                await DecryptStream(algorithm, input, output).ConfigureAwait(false);
            }

            if(output.CanSeek)
                output.Seek(0, SeekOrigin.Begin);
        }

        #endregion

        #region Private Methods

        /****************************************************************************/
        private SymmetricAlgorithm GetAlgorithm(EncryptionPolicy policy)
        {
            var algorithm = CreateAlgorithm(policy);

            if(this.Key != null)
                algorithm.Key = this.Key.ToArray();

            policy.IsReadOnly = false;

            if(policy.BlockSize.HasValue)
                algorithm.BlockSize = policy.BlockSize.Value;
            else
                policy.BlockSize = algorithm.BlockSize;

            if(!string.IsNullOrWhiteSpace(policy.Mode) && Enum.TryParse(policy.Mode, out CipherMode mode))
                algorithm.Mode = mode;
            else
                policy.Mode = algorithm.Mode.ToString();

            if(!string.IsNullOrWhiteSpace(policy.Padding) && Enum.TryParse(policy.Padding, out PaddingMode padding))
                algorithm.Padding = padding;
            else
                policy.Padding = algorithm.Padding.ToString();

            policy.IsReadOnly = true;

            return algorithm;
        }
                
        /****************************************************************************/
        private SymmetricAlgorithm GetAlgorithm()
        {
            if (this.Key == null)
                throw new Exception("This is a keyless encryptor and can only be used to create a new keyed encryptor (using GenerateNew)");

            return GetAlgorithm(_policy);
        }
        
        /****************************************************************************/
        private static void SetKeySize(SymmetricAlgorithm algorithm, int keySize)
		{
            if(keySize == int.MaxValue)
                algorithm.KeySize = algorithm.LegalKeySizes.Last().MaxSize;
            else if(keySize != DefaultKeySize)
                algorithm.KeySize = keySize;
        }

        /****************************************************************************/
        private async Task EncryptStream(SymmetricAlgorithm algorithm, Stream input, Stream output)
        {
            // Write IV out to beginning of output stream
            await output.WriteAsync(algorithm.IV, 0, algorithm.IV.Length).ConfigureAwait(false);

            // Get an encryptor
            using(ICryptoTransform encryptor = algorithm.CreateEncryptor(this.Key.ToArray(), algorithm.IV))
            {
                // Encrypt the data.
                await Transform(encryptor, input, output).ConfigureAwait(false);
            }
        }  

        /****************************************************************************/
        private async Task<byte[]> EncryptBytes(SymmetricAlgorithm algorithm, byte[] data)
        {
            // Get an encryptor
            using(ICryptoTransform encryptor = algorithm.CreateEncryptor(this.Key.ToArray(), algorithm.IV))
            {
                // Encrypt the data.
                var encrypted = await TransformToBytes(encryptor, data, 0, data.Length).ConfigureAwait(false);

                // Prepend the IV to the encrypted data
                return encrypted.Prepend(algorithm.IV);
            }
        }   
                            
        /****************************************************************************/
        private async Task<byte[]> DecryptBytes(SymmetricAlgorithm algorithm, byte[] encrypted, int offset)
        {
            var ivSize = algorithm.BlockSize / 8;
            var iv     = new byte[ivSize];

            // Get the iv from the front of the data
            Buffer.BlockCopy(encrypted, offset, iv, 0, ivSize); 

            // Now decrypt the encrypted data using the decryptor
            using(ICryptoTransform decryptor = algorithm.CreateDecryptor(this.Key.ToArray(), iv))
            {
                return await TransformToBytes(decryptor, encrypted, offset + ivSize, encrypted.Length - ivSize - offset).ConfigureAwait(false);
            }
        }

        /****************************************************************************/
        private async Task DecryptStream(SymmetricAlgorithm algorithm, Stream input, Stream output)
        {
            // Read IV from beginning of input stream
            var ivSize = algorithm.BlockSize / 8;
            var iv     = new byte[ivSize];

            await input.ReadAsync(iv, 0, ivSize).ConfigureAwait(false);

            // Get a decryptor
            using(ICryptoTransform encryptor = algorithm.CreateDecryptor(this.Key.ToArray(), iv))
            {
                // Decrypt the data.
                await Transform(encryptor, input, output).ConfigureAwait(false);
            }
        }  

        /****************************************************************************/
        private async Task Transform(ICryptoTransform transform, Stream input, Stream output)
        {
            // Create an intermediate memory stream so as not to close the output stream
            using(var memStream = new MemoryStream())
            { 
                using(var cryptoStream = new CryptoStream(memStream, transform, CryptoStreamMode.Write))
                {
                    await input.CopyToAsync(cryptoStream).ConfigureAwait(false);

                    cryptoStream.FlushFinalBlock();
                    await cryptoStream.FlushAsync().ConfigureAwait(false);

                    memStream.Seek(0, SeekOrigin.Begin);
                    await memStream.CopyToAsync(output).ConfigureAwait(false);
                }
            }
        }

        /****************************************************************************/
        private async Task<MemoryStream> Transform(ICryptoTransform transform, byte[] data, int offset, int length)
        {
            var memory = new MemoryStream();
            
            try
            { 
                using(var cryptoStream = new CryptoStream(memory, transform, CryptoStreamMode.Write))
                {
                    await cryptoStream.WriteAsync(data, offset, length).ConfigureAwait(false);
                    cryptoStream.FlushFinalBlock();
                    await cryptoStream.FlushAsync().ConfigureAwait(false);

                    return memory;
                }
            }
            catch
            {
                memory.Dispose();
                throw;
            }
        }

        /****************************************************************************/
        private async Task<byte[]> TransformToBytes(ICryptoTransform transform, byte[] data, int offset, int length)
        {
            using(var memStream = await Transform(transform, data, offset, length).ConfigureAwait(false))
            {
                return memStream.ToArray();
            }
        }

        #endregion
    }

    /****************************************************************************/
    /****************************************************************************/
    public class ExpiredPolicyException : Exception 
    { 
        /****************************************************************************/
        public ExpiredPolicyException() : base("The policy for this encryptor has expired and cannot be used to encrypt")
        {
        }
    }
}
