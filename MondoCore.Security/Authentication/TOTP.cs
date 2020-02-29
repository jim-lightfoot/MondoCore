using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using MondoCore.Common;
using MondoCore.Security.Encryption;
using MondoCore.Security.Passwords;

namespace MondoCore.Security.Authentication
{
    /****************************************************************************/
    /****************************************************************************/
    /// <summary>
    /// For generating and validating a time-based one-time password (TOTP)
    /// </summary>
    public static class TOTP
    {
        /*****************************************************************************/
        /// <summary>
        /// Generates a time-based one-time password (TOTP)
        /// </summary>
        /// <returns>Returns a tuple with generated secret and code  (TOTP). Secret should be persisted (encrypted) for later validation and code should be sent to user's device, e.g. text message</returns>
        public static (byte[] Secret, int Code) GenerateCode()
        {            
            var secret = PasswordHasher.CreateSalt(10);  // Create a random set of bytes for the secret     
            var code   = TOTPGenerator.GenerateTOTP(secret, DateTime.UtcNow);

            return (secret, code);
        }

        /*****************************************************************************/
        /// <summary>
        /// Determine if verification code is valid. Allows for an extra step (30 sec) future grace period for clocks out of sync and an expiration time
        /// </summary>
        /// <param name="verificationCode">Code sent to user's device and received from user</param>
        /// <param name="secret">Secret generated for code. Should come from storage</param>
        /// <param name="expires">How long code should last</param>
        /// <returns>Returns true if code is valid</returns>
        public static bool IsValid(int iVerificationCode, byte[] secret, int expires = TOTPGenerator.Step)
        {            
            return IsValid(iVerificationCode, secret, DateTime.UtcNow, expires);
        }

        /*****************************************************************************/
        /// <summary>
        /// Determine if verification code is valid. Allows for an extra step (30 sec) future grace period for clocks out of sync and a past grace period, i.e. expiration time
        ///   This version of the function is for unit testing only. Use the version with a datetime.
        /// </summary>
        /// <param name="verificationCode">Code sent to user's device and received from user</param>
        /// <param name="secret">Secret generated for code. Should come from storage</param>
        /// <param name="dtNow">Current data in utc, e.g. DateTime.UtcNow </param>
        /// <param name="gracePeriod">How long code should last (expires)</param>
        /// <returns>Returns true if code is valid</returns>
        public static bool IsValid(int verificationCode, byte[] secret, DateTime dtNow, int expires = TOTPGenerator.Step)
        {            
            if(verificationCode == TOTPGenerator.GenerateTOTP(secret, dtNow))
                return true;            

            // One step grace period in future to account for clock's out of sync
            if(verificationCode == TOTPGenerator.GenerateTOTP(secret, dtNow.AddSeconds(TOTPGenerator.Step)))
                return true;
            
            // Check up to expiration time
            var periods = expires / TOTPGenerator.Step;

            for(var i = 1; i <= periods; ++i)
                if(verificationCode == TOTPGenerator.GenerateTOTP(secret, dtNow.AddSeconds(-(TOTPGenerator.Step * i))))
                    return true;
            
            return false;
        }

        #region TOTP

        /*************************************************************************/
        /*************************************************************************/
        private static class TOTPGenerator
        {
            private static int[] _digitsPower = { 1, 10, 100, 1000, 10000, 100000, 1000000, 10000000, 100000000 };
                                               // 0  1   2    3     4      5       6        7         8

            internal const int Step = 30;

            /*************************************************************************/
            internal static int GenerateTOTP(byte[] k, DateTime dtNow) 
            {
                DateTime dtEpoch = new DateTime(1970, 1, 1);
                TimeSpan tsSteps = dtNow - dtEpoch;
                long     iSteps  = (long)Math.Floor(tsSteps.TotalSeconds / Step);
                byte[]   aTime   = BitConverter.GetBytes(iSteps).Reverse().ToArray();

                return GenerateTOTP(k, aTime, 6);
            }

            /*************************************************************************/
            internal static int GenerateTOTP(byte[] k, byte[] aTime, int codeDigits) 
            {
               byte[] hash = new HMACSHA1(k).ComputeHash(aTime); 

               int offset = hash[hash.Length - 1] & 0xf;
               int binary = ((hash[offset]     & 0x7f) << 24) |
                            ((hash[offset + 1] & 0xff) << 16) |
                            ((hash[offset + 2] & 0xff) << 8 | 
                             (hash[offset + 3] & 0xff));

               var otp = binary % _digitsPower[codeDigits];

               string result = otp.ToString();

               while (result.Length < codeDigits) 
                   result = "0" + result;

               return int.Parse(result);
            }
        }

        #endregion
    }

}
