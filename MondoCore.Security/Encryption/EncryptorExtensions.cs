/****************************************************************************/
/*                                                                          */
/*    The MondoCore Libraries  							                    */
/*                                                                          */
/*        Namespace: MondoCore.Security.Encryption				            */
/*             File: EncryptorExtensions.cs					                */
/*        Class(es): EncryptorExtensions				                    */
/*          Purpose: Extension methods for IEncryptor                       */
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
using System.Security;
using System.Text;
using System.Threading.Tasks;

using MondoCore.Common;

namespace MondoCore.Security.Encryption
{
    /****************************************************************************/
    /****************************************************************************/
    public static class EncryptorExtensions
    {
        /****************************************************************************/
        /// <summary>
        /// Encrypt a string
        /// </summary>
        /// <param name="data">Data to encrypt</param>
        /// <returns>A base64 encoded string of the encrypted data</returns>
        public static async Task<string> Encrypt(this IEncryptor encryptor, string data)
        {
            if(string.IsNullOrEmpty(data))
                return data;

            // Get encrypted array of bytes.
            var toEncrypt = Encoding.UTF8.GetBytes(data); 

            try
            {
                var encrypted = await encryptor.Encrypt(toEncrypt);

                return Convert.ToBase64String(encrypted);
            }
            finally
            {
                Array.Clear(toEncrypt, 0, toEncrypt.Length);
            }
        }

        /****************************************************************************/
        /// <summary>
        /// Decrypt a string that was previously encrypted using the above method
        /// </summary>
        /// <param name="data">Data to decrypt</param>
        /// <returns>The original unencrypted string</returns>
        public static async Task<string> Decrypt(this IEncryptor encryptor, string encrypted)
        {
            if(string.IsNullOrEmpty(encrypted))
                return "";

            try 
            { 
                var    decrypted    = await encryptor.DecryptChars(encrypted);
                char[] chDecrypted  = decrypted.Item1;
                string sDecrypted   = new String(chDecrypted, 0, decrypted.Item2);

                chDecrypted.Clear();

                return sDecrypted.Replace(((char)6).ToString(), ""); // Was getting weird char problems
            }
            catch(Exception ex)
            {
                _ = ex;

                return encrypted;
            }
        }

        /****************************************************************************/
        /// <summary>
        /// Descrypts an encrypted string into an array of chars
        /// </summary>
        /// <param name="encrypted">Encrypted string</param>
        /// <returns></returns>
        public static async Task<(char[], int)> DecryptChars(this IEncryptor encryptor, string encrypted)
        {
            byte[] aDecrypted  = await encryptor.Decrypt(Convert.FromBase64String(encrypted));
            char[] chDecrypted = Encoding.UTF8.GetChars(aDecrypted);

            Array.Clear(aDecrypted, 0, aDecrypted.Length);

            int iLength = 0;

            while(iLength < chDecrypted.Length && chDecrypted[iLength] != '\0')
                ++iLength;

            return (chDecrypted, iLength);
        }
    }
}
