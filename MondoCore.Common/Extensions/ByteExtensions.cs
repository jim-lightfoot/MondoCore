/***************************************************************************
 *                                                                          
 *    The MondoCore Libraries  							                    
 *                                                                          
 *        Namespace: MondoCore.Common							            
 *             File: ByteExtensions.cs					    		        
 *        Class(es): ByteExtensions				         		            
 *          Purpose: Extensions for bytes and byte arrays                   
 *                                                                          
 *  Original Author: Jim Lightfoot                                          
 *    Creation Date: 1 Jan 2020                                             
 *                                                                          
 *   Copyright (c) 2005-2020 - Jim Lightfoot, All rights reserved           
 *                                                                          
 *  Licensed under the MIT license:                                         
 *    http://www.opensource.org/licenses/mit-license.php                    
 *                                                                          
 ****************************************************************************/

using System;
using System.IO;

namespace MondoCore.Common
{
    /****************************************************************************/
    /****************************************************************************/
    public static class ByteExtensions
    {
        /****************************************************************************/
        public static byte[] DeepClone(this byte[] aBytes)
        {
            byte[] aNew = new byte[aBytes.Length];
       
            aBytes.CopyTo(aNew, 0);

            return aNew;
        }

        /****************************************************************************/
        public static byte[] DeepClone(this byte[] aBytes, int srcOffset, int iLength)
        {
            if(iLength == -1)
                iLength = aBytes.Length - srcOffset;

            byte[] aNew = new byte[iLength];
       
            Buffer.BlockCopy(aBytes, srcOffset, aNew, 0, iLength);

            return aNew;
        }
                
        /****************************************************************************/
        public static byte[] Prepend(this byte[] aBytes, byte[] bytesToPrepend)
        {
            using(var output = new MemoryStream())
            {
                // Write the bytes to prepend
                output.Write(bytesToPrepend, 0, bytesToPrepend.Length);

                // Write the original bytes
                output.Write(aBytes, 0, aBytes.Length);

                return output.ToArray();
            }
        }   

        /****************************************************************************/
        /// <summary>
        ///  Determine of two byte are equivalent
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        public static bool IsEqual(this byte[] b1, byte[] b2)
        {
            if(b1 == null || b2 == null)
                throw new ArgumentNullException();

            if(b1.Length != b2.Length)
                return false;

            for(var i = 0; i < b1.Length; ++i)
                if(b1[i] != b2[i])
                    return false;

            return true;
        }

        /****************************************************************************/
        /// <summary>
        /// xor each byte in two byte arrays
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns>A new byte array with the XORd values<returns>
        public static byte[] XOR(this byte[] a1, byte[] a2)
        {
            var iLength  = a1.Length;
            var aResults = new byte[a1.Length];

            for(int i = 0; i < iLength; ++i)
                aResults[i] = (byte)(a1[i] ^ a2[i]);

            return aResults;
        }

        /****************************************************************************/
        /// <summary>
        /// XOR 4 byte arrays
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns>A new byte array with the XORd values<returns>
        public static byte[] XOR(this byte[] a1, byte[] a2, byte[] a3, byte[] a4)
        {
            byte[] p1 = XOR(a1, a2);
            byte[] p2 = XOR(a3, a4);
            byte[] p3 = XOR(p1, p2);

            Array.Clear(p1, 0, p1.Length);
            Array.Clear(p2, 0, p2.Length);

            return p3;
        }
    }
}
