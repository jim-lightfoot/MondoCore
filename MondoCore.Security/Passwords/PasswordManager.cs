/****************************************************************************/
/*                                                                          */
/*    The MondoCore Libraries  							                    */
/*                                                                          */
/*        Namespace: MondoCore.Security.Passwords				            */
/*             File: IPasswordManager.cs			 		    		    */
/*        Class(es): IPasswordManager				           		        */
/*          Purpose: Class for managing passwords                           */
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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MondoCore.Security.Encryption;

namespace MondoCore.Security.Passwords
{
    /****************************************************************************/
    /****************************************************************************/
    public class PasswordManager : IPasswordManager
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IPasswordStore  _passwordStore;
        private readonly IEncryptor      _encryptor;
        private readonly int             _saltLength;

        /****************************************************************************/
        public PasswordManager(IPasswordHasher passwordHasher, 
                               IPasswordStore  passwordStore, 
                               IEncryptor      saltEncryptor,
                               int             saltLength = 32)
        {
            _passwordHasher = passwordHasher;
            _passwordStore  = passwordStore;
            _encryptor      = saltEncryptor;
            _saltLength     = saltLength;
        }

        /****************************************************************************/
        public Password FromOwner(string password, IPasswordOwner owner)
        {
            var unhashed = Encoding.UTF8.GetBytes(password);
            var salt     = _passwordHasher.GenerateSalt(_saltLength);
            var hash     = _passwordHasher.Hash(unhashed, salt, owner.ToArray);

            Array.Clear(unhashed, 0, unhashed.Length);

            return new Password(hash, salt, owner);
        }

        /****************************************************************************/
        public string GenerateNew(int length)
        {
            const string validChars     = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+-={}[]:<>?,./~";
            char[]       characterArray = validChars.ToArray();
            byte[]       bytes          = _passwordHasher.GenerateSalt(length * 8);
            char[]       result         = new char[length];

            for(int j = 0; j < length; ++j)
            {
                ulong value = BitConverter.ToUInt64(bytes, j * 8);
                result[j] = characterArray[value % (uint)characterArray.Length];
            }

            return new string(result);
        }        

        /****************************************************************************/
        public async Task<Password> Load(IPasswordOwner owner)
        {
            var pwd             = await _passwordStore.Get(owner, out byte[] salt);
            var unencryptedSalt = await _encryptor.Decrypt(salt);
            var password        = new Password(pwd, unencryptedSalt, owner);

            Array.Clear(salt, 0, salt.Length);
            Array.Clear(unencryptedSalt, 0, unencryptedSalt.Length);

            return password;
        }

        /****************************************************************************/
        public async Task Save(Password password)
        {
            var encryptedSalt = await _encryptor.Encrypt(password.Salt);

            await _passwordStore.Add(password.ToArray(), encryptedSalt, password.Owner);
           
            Array.Clear(encryptedSalt, 0, encryptedSalt.Length);
       }
    }
}
