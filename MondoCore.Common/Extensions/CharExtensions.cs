/*************************************************************************** 
 *                                                                           
 *    The MondoCore Libraries  							                     
 *                                                                           
 *        Namespace: MondoCore.Common							             
 *             File: CharExtensions.cs					    		         
 *        Class(es): CharExtensions				         		             
 *          Purpose: Extensions for chars and char arrays                    
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
using System.Collections.Generic;
using System.Text;

namespace MondoCore.Common
{
    /****************************************************************************/
    /****************************************************************************/
    public static class CharExtensions
    {
        /****************************************************************************/
        public static void Clear(this char[] aChars)
        {
            int nChars = aChars.Length;
       
            for(int i = 0; i < nChars; ++i)
                aChars[i] = '\0';

            return;
        }    
    }
}
