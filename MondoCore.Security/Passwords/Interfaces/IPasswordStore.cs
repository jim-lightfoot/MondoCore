/*************************************************************************** 
 *                                                                           
 *    The MondoCore Libraries  							                     
 *                                                                           
 *        Namespace: MondoCore.Security.Passwords				             
 *             File: IPasswordStore.cs					    	             
 *        Class(es): IPasswordStore				         		             
 *          Purpose: Interface for persisting passwords                      
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
using System.Threading.Tasks;

namespace MondoCore.Security.Passwords
{
    /****************************************************************************/
    /****************************************************************************/
    public interface IPasswordStore
    {   
        Task<byte[]> Get(IPasswordOwner owner, out byte[] salt);
        Task         Add(byte[] password, byte[] salt, IPasswordOwner owner);
    }
}
