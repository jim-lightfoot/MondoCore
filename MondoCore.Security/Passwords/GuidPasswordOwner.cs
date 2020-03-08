/*************************************************************************** 
 *                                                                           
 *    The MondoCore Libraries  							                     
 *                                                                           
 *        Namespace: MondoCore.Security.Passwords				             
 *             File: GuidPasswordOwner.cs			 		    		     
 *        Class(es): GuidPasswordOwner				           		         
 *          Purpose: Class for owner/user ids that are guids                 
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
    public class GuidPasswordOwner : IPasswordOwner
    {
        private readonly Guid _owner;

        /****************************************************************************/
        public GuidPasswordOwner(Guid owner)
        { 
            _owner = owner;
        }

        /****************************************************************************/
        public Guid   Id      => _owner;
        public byte[] ToArray => _owner.ToByteArray();
    }
}
