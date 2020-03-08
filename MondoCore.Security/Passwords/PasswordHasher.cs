/*************************************************************************** 
 *                                                                           
 *    The MondoCore Libraries  							                     
 *                                                                           
 *        Namespace: MondoCore.Security.Passwords				             
 *             File: PasswordHasher.cs			 		    		         
 *        Class(es): PasswordHasher			           		                 
 *          Purpose: Class for hashing passwords                             
 *                                                                           
 *  Original Author: Jim Lightfoot                                           
 *    Creation Date: 2 Feb 2020                                              
 *                                                                           
 *   Copyright (c) 2020 - Jim Lightfoot, All rights reserved                 
 *                                                                           
 *  Licensed under the MIT license:                                          
 *    http://www.opensource.org/licenses/mit-license.php                     
 *                                                                           
 ****************************************************************************/

using System;
using System.Security.Cryptography;

namespace MondoCore.Security.Passwords
{
    /****************************************************************************/
    /****************************************************************************/
    /// <summary>
    /// Uses the PBKDF2 algorithm to generate a password hash
    /// </summary>
    public class PasswordHasher : IPasswordHasher
    {
        private readonly int _numIterations;
        private readonly int _hashSize;

        /****************************************************************************/
        /// <summary>
        /// Initialized a new PBKDF2 based password hasher
        /// </summary>
        /// <param name="numIterations">Number of iterations to hash. Minimum is 5000. See https://en.wikipedia.org/wiki/PBKDF2 for hash iteration recommendations</param>
        /// <param name="hashSize">The size of the hash. Default is 128</param>
        public PasswordHasher(int numIterations, int hashSize = 128)
        {
            if(numIterations < 5000)
                throw new ArgumentException("Hash iterations must be greater than 5000");

            _numIterations = numIterations;
            _hashSize = hashSize;
        }

        /****************************************************************************/
        /// <summary>
        /// Hash the password using the given salt
        /// </summary>
        /// <param name="password">Password to hash</param>
        /// <param name="salt">The salt must be unique to each password</param>
        /// <returns>The hash of the password</returns>
        public byte[] Hash(byte[] password, byte[] salt)
        {
            using(Rfc2898DeriveBytes hasher = new Rfc2898DeriveBytes(password, salt, _numIterations))
            {
                return hasher.GetBytes(_hashSize);
            }        
        }

        /****************************************************************************/
        /// <summary>
        /// Generates a new salt
        /// </summary>
        /// <param name="size">The size of the salt to generate</param>
        /// <returns>A new salt</returns>
        public byte[] GenerateSalt(int size)
        {
            return CreateSalt(size);
        }

        /****************************************************************************/
        /// <summary>
        /// Generates a new salt
        /// </summary>
        /// <param name="size">The size of the salt to generate</param>
        /// <returns>A new salt</returns>
        public static byte[] CreateSalt(int size)
        {
            using(var crypto = new RNGCryptoServiceProvider())
            {
                byte[] buffer = new byte[size];

                crypto.GetBytes(buffer);

                return buffer;
            }
        }
    }
}
