using System;
using System.Collections.Generic;
using System.Text;

namespace MondoCore.Common
{
    public static class StringExtensions
    {
        /****************************************************************************/
        public static string EnsureStartsWith(this string val, char chFirst)
        {
            return EnsureStartsWith(val, chFirst.ToString());
        }

        /****************************************************************************/
        public static string EnsureStartsWith(this string val, string first)
        {
            if(val.IndexOf(first) == 0)
                return val;

            return first + val;
        }

        /****************************************************************************/
        public static string EnsureNotStartsWith(this string val, string start)
        {
            if(!val.StartsWith(start))
                return(val);

            return val.Substring(start.Length).Trim().EnsureNotStartsWith(start);
        }
                
        /****************************************************************************/
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
        public static string EnsureEndsWith(this string val, char endsWith)
        {
            return val.EnsureEndsWith(endsWith.ToString());
        }
    }
}
