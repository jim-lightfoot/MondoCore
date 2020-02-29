/****************************************************************************/
/*                                                                          */
/*    The MondoCore Libraries  							                    */
/*                                                                          */
/*        Namespace: MondoCore.Security.Password  					        */
/*             File: Password.cs					    		            */
/*        Class(es): Password				         		                */
/*          Purpose: Provides a password                                    */
/*                                                                          */
/*  Original Author: Jim Lightfoot                                          */
/*    Creation Date: 1 Jan 2020                                             */
/*                                                                          */
/*   Copyright (c) 2020 - Jim Lightfoot, All rights reserved                */
/*                                                                          */
/*  Licensed under the MIT license:                                         */
/*    http://www.opensource.org/licenses/mit-license.php                    */
/*                                                                          */
/****************************************************************************/

using System;
using System.Text;
using System.Threading.Tasks;

using MondoCore.Common;

namespace MondoCore.Security.Passwords
{
    /****************************************************************************/
    /****************************************************************************/
    public class Password : IDisposable
    {
        private readonly byte[] _hash;
        private readonly byte[] _salt;
        private readonly IPasswordOwner _owner;

        /****************************************************************************/
        public Password(byte[] hash, byte[] salt, IPasswordOwner owner)
        {
            _hash  = hash.DeepClone();
            _salt  = salt.DeepClone();
            _owner = owner;
        }

        /****************************************************************************/
        public byte[]         Salt        => _salt;
        public string         SaltString  => Convert.ToBase64String(_salt);
        public IPasswordOwner Owner       => _owner;

        /****************************************************************************/
        public override string ToString()
        {
            return Convert.ToBase64String(_hash);
        }

        /****************************************************************************/
        public byte[] ToArray()
        {
            return _hash;
        }

        /****************************************************************************/
        public bool IsEqual(Password password)
        {
            return _hash.IsEqual(password._hash);
        }

        /****************************************************************************/
        public void Dispose()
        {
            Array.Clear(_hash, 0, _hash.Length);
            Array.Clear(_salt, 0, _salt.Length);
        }
    }
}
