using System;
using System.Collections.Generic;
using System.Text;

using MondoCore.Common;

namespace MondoCore.Security.Passwords
{
    /****************************************************************************/
    /****************************************************************************/
    public static class PasswordHasherExtensions
    {
        /****************************************************************************/
        /// <summary>
        ///  Hash a password using a salt and an authenticator (usually a user id)
        /// </summary>
        /// <param name="password">Password to hash</param>
        /// <param name="salt">A salt which should be unique for every password</param>
        /// <param name="authenticator">An authenticator (usually a user id)</param>
        /// <returns>The salted/hashed password</returns>
        public static byte[] Hash(this IPasswordHasher passwordHasher, byte[] password, byte[] salt, byte[] authenticator)
        {
            var unhashed = password;
            
            if(authenticator != null)
                unhashed = password.Prepend(authenticator);

            return passwordHasher.Hash(unhashed, salt);
        }
    }
}
