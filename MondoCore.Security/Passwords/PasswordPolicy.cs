/***************************************************************************
 *                                                                          
 *    The MondoCore Libraries  							                    
 *                                                                          
 *        Namespace: MondoCore.Security.Passwords				            
 *             File: PasswordPolicy.cs			 		    		    
 *        Class(es): PasswordPolicy				           		        
 *          Purpose: A policy to determine password requirements and validate passwords 
 *                     against that policy                          
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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MondoCore.Security.Passwords
{
    /****************************************************************************/
    /****************************************************************************/
    /// <summary>
    /// A policy to determine password requirements
    /// </summary>
    public class PasswordPolicy
    {
        public int           MinLength      { get; } = 10;
        public int           MinUniqueChars { get; } = 8;
        public int           MinLetters     { get; } = 2;
        public int           MinUppercase   { get; } = 1;
        public int           MinDigits      { get; } = 1;
        public int           MinPunctuation { get; } = 1;

        public IList<string> InvalidWords   { get; } = new List<string> { "password", "passwrd", "admin", "admln", "qwert", "google", "computer", "login", "logln", "username", "iloveyou", "lloveyou", "princess", "prlncess" };
        public IList<char>   InvalidChars   { get; } = new List<char>   {};

        /****************************************************************************/
        /// <summary>
        /// Determines is the given (user entered) password meets the policy
        /// </summary>
        /// <param name="password">User entered password to checj</param>
        /// <param name="additionalInvalidWords">Additional invalid words to check, e.g. this might include your org name, product name, words in a different language, etc</param>
        /// <returns></returns>
        public bool IsValid(string password, params string[] additionalInvalidWords)
        {
            if(string.IsNullOrWhiteSpace(password))
                return false;

            if(password.Length < this.MinLength)
                return false;

            var check          = new Dictionary<char,char>();
            var invalid        = this.InvalidChars.ToDictionary( (ch)=> { return ch; } );
            var numLetters     = 0;
            var numUppercase   = 0;
            var numDigits      = 0;
            var numPunctuation = 0;

            // Check for number of unique characters
            foreach(char ch in password)
            {
                if(ch == ' ')
                   return false;

                if(invalid.ContainsKey(ch))
                    return false;

                if(char.IsLetter(ch))
                { 
                    ++numLetters;

                    if(char.IsUpper(ch))
                        ++numUppercase;
                }
                else if(char.IsDigit(ch))
                    ++numDigits;
                else  
                    ++numPunctuation;

                if(!check.ContainsKey(ch))
                    check.Add(ch, ch);
            }

            if(check.Count < this.MinUniqueChars)
                return false;

            if(numLetters < this.MinLetters)
                return false;

            if(numUppercase < this.MinUppercase)
                return false;

            if(numDigits < this.MinDigits)
                return false;

            if(numPunctuation < this.MinPunctuation)
                return false;

            foreach(var invalidSequence in _invalidSequences)
                if(password.Contains("0123"))
                    return false;

            var normalized    = password.ToLower().Normalize(NormalizationForm.FormD); // Separate out diacriticals into separate chars
            var chList        = new List<char>(normalized.ToCharArray().Where( ch=> (int)ch < 256 )); // Remove diacriticals
            var checkPassword = new string(chList.ToArray());

            foreach(var sub in _charSubstitutions)
                checkPassword = checkPassword.Replace(sub.Item1, sub.Item2);

            foreach(var word in this.InvalidWords)
                if(checkPassword.Contains(word))
                    return false;

            // Additional invalid words for just this pass, e.g. user name or user's first and last name
            if(additionalInvalidWords != null)
                foreach(var word in additionalInvalidWords)
                    if(!string.IsNullOrWhiteSpace(word) && checkPassword.Contains(word))
                        return false;

            return true;
        }

        #region Private

        /****************************************************************************/
        private static readonly List<(string, string)> _charSubstitutions = new List<(string, string)>
        {
            ("$", "s"),
            ("0", "o"),
            ("3", "e"),
            ("5", "s"),
            ("6", "g"),
            ("7", "t"),
            ("@", "a"),
            ("1", "l"),
            ("|", "l"),
            ("!", "i")
        };

        /****************************************************************************/
        private static readonly List<string> _invalidSequences = new List<string>
        {
            "0123",
            "1234",
            "2345",
            "3456",
            "4567",
            "5678",
            "6789",
            "9876",
            "8765",
            "7654",
            "6543",
            "5432",
            "4321",
            "3210"
        };

        #endregion
    }
}
