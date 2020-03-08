/***************************************************************************
 *                                                                          
 *    The MondoCore Libraries  							                    
 *                                                                          
 *        Namespace: MondoCore.Security.Passwords				            
 *             File: IPasswordHasher.cs			 		    		        
 *        Class(es): IPasswordHasher				           	            
 *          Purpose: Interface for hashing passwords                        
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
using System.Text;

namespace MondoCore.Security.Passwords
{
    /****************************************************************************/
    /****************************************************************************/
    public interface IPasswordHasher
    {
        byte[] Hash(byte[] password, byte[] salt);
        byte[] GenerateSalt(int size);
    }
}
