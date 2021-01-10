/*************************************************************************** 
 *                                                                           
 *    The MondoCore Libraries  							                     
 *                                                                           
 *        Namespace: MondoCore.Common							             
 *             File: StringExtensions.cs					    		         
 *        Class(es): StringExtensions				         		             
 *          Purpose: Extensions for String                  
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
using System.Text.RegularExpressions;

namespace MondoCore.Common
{
    public static class StringExtensions
    {
        /****************************************************************************/
        /// <summary>
        /// Ensure a string starts with a given char by prepending it if it does not
        /// </summary>
        public static string EnsureStartsWith(this string val, char chFirst)
        {
            return EnsureStartsWith(val, chFirst.ToString());
        }

        /****************************************************************************/
        /// <summary>
        /// Ensure a string starts with another string by prepending it if it does not
        /// </summary>
        public static string EnsureStartsWith(this string val, string first)
        {
            if(val.IndexOf(first) == 0)
                return val;

            return first + val;
        }

        /****************************************************************************/
        /// <summary>
        /// Ensures a string does not start with another string by removing it if it does
        /// </summary>
        /// <param name="val"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static string EnsureNotStartsWith(this string val, string start)
        {
            if(!val.StartsWith(start))
                return(val);

            return val.Substring(start.Length).Trim().EnsureNotStartsWith(start);
        }
                
        /****************************************************************************/
        /// <summary>
        /// Ensure a string ends with another string by appending it if it does not
        /// </summary>
        public static string EnsureEndsWith(this string val, string endsWith)
        {
            if(val == null)
                val = "";

            if(string.IsNullOrEmpty(endsWith))
                return val;

            if(val.EndsWith(endsWith))
                return val;

            return val + endsWith;
        }
        
        /****************************************************************************/
        /// <summary>
        /// Ensure a string ends with a given char by appending it if it does not
        /// </summary>
        public static string EnsureEndsWith(this string val, char endsWith)
        {
            return val.EnsureEndsWith(endsWith.ToString());
        }

        /****************************************************************************/
        /// <summary>
        /// Ensure a string ends with a given char by appending it if it does not
        /// </summary>
        public static string SubstringBefore(this string val, string before)
        {
            var index = val.IndexOf(before);

            if(index == -1)
                return val;

            return val.Substring(0, index);
        }

        /****************************************************************************/
        /// <summary>
        /// Match value to a file wildcard, e.g. "*.*", "*.pdf", etc
        /// </summary>
        public static bool MatchesWildcard(this string val, string wildcard)
        {
            wildcard = wildcard ?? throw new ArgumentNullException();

            if(wildcard == "*.*")
                return true;

            if(string.IsNullOrEmpty(val))
                return false;

            wildcard = Regex.Escape(wildcard).Replace("\\*", ".*");
            wildcard = string.Concat("^", wildcard.Replace("\\?", "."), "$");

            var regex = new Regex(wildcard, RegexOptions.IgnoreCase);

            return regex.IsMatch(val);
        }      
    }
}
