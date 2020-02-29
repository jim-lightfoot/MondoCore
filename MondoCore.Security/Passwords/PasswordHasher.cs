/****************************************************************************/
/*                                                                          */
/*    The MondoCore Libraries  							                    */
/*                                                                          */
/*        Namespace: MondoCore.Security.Passwords				            */
/*             File: PasswordHasher.cs			 		    		        */
/*        Class(es): PasswordHasher			           		                */
/*          Purpose: Class for hashing passwords                            */
/*                                                                          */
/*  Original Author: Jim Lightfoot                                          */
/*    Creation Date: 2 Feb 2020                                             */
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
using System.Security.Cryptography;

namespace MondoCore.Security.Passwords
{
    /****************************************************************************/
    /****************************************************************************/
    public class PasswordHasher : IPasswordHasher
    {
        private readonly int _numIterations;
        private readonly int _hashSize;

        /****************************************************************************/
        public PasswordHasher(int numIterations, int hashSize = 128)
        {
            if(numIterations < 5000)
                throw new ArgumentException("Hash iterations must be greater than 5000");

            _numIterations = numIterations;
            _hashSize = hashSize;
        }

        /****************************************************************************/
        public byte[] Hash(byte[] password, byte[] salt, byte[] authenticator)
        {
            using(Rfc2898DeriveBytes hasher = new Rfc2898DeriveBytes(password, salt, _numIterations))
            {
                return hasher.GetBytes(_hashSize);
            }        
        }

        /****************************************************************************/
        public byte[] GenerateSalt(int size)
        {
            return CreateSalt(size);
        }

        /****************************************************************************/
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
