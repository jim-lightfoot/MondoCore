/***************************************************************************
 *                                                                          
 *    The MondoCore Libraries  	                                            
 *                                                                          
 *      Namespace: MondoCore.Common	                                        
 *           File: Base32.cs                                                
 *      Class(es): Base32                                                   
 *        Purpose: Encode or decode as base32                               
 *                                                                          
 *  Licensed under the MIT license:                                         
 *    http://www.opensource.org/licenses/mit-license.php                    
 *                                                                          
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MondoCore.Common
{
    /*********************************************************************/
    /*********************************************************************/
    // Adapted from this dude's code (but mostly his): 
    //   https://blogs.technet.microsoft.com/cloudpfe/2018/04/18/base32-encoding-and-decoding-in-c/
    /// <summary>
    /// Encode/decode a string as base32
    /// </summary>
    public static class Base32
    {
        private const string Base32AllowedCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        /*********************************************************************/
        /// <summary>
        /// Encode a string base32
        /// </summary>
        /// <param name="input">Non-encoded string</param>
        /// <returns>base32 string</returns>
        public static string Encode(this string input)
        {
            if(string.IsNullOrWhiteSpace(input))
                return "";

            var bytes = Encoding.UTF8.GetBytes(input);

            return bytes.Encode();
        }

        /*********************************************************************/
        /// <summary>
        /// Decode a base32 string
        /// </summary>
        /// <param name="input">A base32 encoded string</param>
        /// <returns>Orignal non-encoded string</returns>
        public static string Decode(this string input)
        {
            if(string.IsNullOrWhiteSpace(input))
                return "";

            var bytes = input.DecodeAsBytes();

            return Encoding.UTF8.GetString(bytes);
        }

        /*********************************************************************/
        /// <summary>
        /// Base32 encode an array of byte
        /// </summary>
        /// <param name="input">Bytes to encode</param>
        /// <returns>base32 string</returns>
        public static string Encode(this byte[] input)
        {
            var bits = input.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')).Aggregate((a, b) => a + b).PadRight((int)(Math.Ceiling((input.Length * 8) / 5d) * 5), '0');

            return Enumerable.Range(0, bits.Length / 5).Select(i => Base32AllowedCharacters.Substring(Convert.ToInt32(bits.Substring(i * 5, 5), 2), 1)).Aggregate((a, b) => a + b);
        }

        /*********************************************************************/
        /// <summary>
        /// Decode a base32 string into the original byte array
        /// </summary>
        /// <param name="input">A base32 encoded string</param>
        /// <returns>Original byte array</returns>
        public static byte[] DecodeAsBytes(this string input)
        {
            var bits = input.TrimEnd('=').ToUpper().ToCharArray().Select(c => Convert.ToString(Base32AllowedCharacters.IndexOf(c), 2).PadLeft(5, '0')).Aggregate((a, b) => a + b);

            return Enumerable.Range(0, bits.Length / 8).Select(i => Convert.ToByte(bits.Substring(i * 8, 8), 2)).ToArray();
        }
    }
}
