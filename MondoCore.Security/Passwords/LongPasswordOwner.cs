/****************************************************************************/
/*                                                                          */
/*    The MondoCore Libraries  							                    */
/*                                                                          */
/*        Namespace: MondoCore.Security.Passwords				            */
/*             File: LongPasswordOwner.cs			 		    		    */
/*        Class(es): LongPasswordOwner				           		        */
/*          Purpose: Class for owner/user ids that are longs (or ints)      */
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
using System.Text;

namespace MondoCore.Security.Passwords
{
    /****************************************************************************/
    /****************************************************************************/
    public class LongPasswordOwner : IPasswordOwner
    {
        private readonly long _owner;

        /****************************************************************************/
        public LongPasswordOwner(long owner)
        { 
            _owner = owner;
        }

        /****************************************************************************/
        public long   Id      => _owner;
        public byte[] ToArray => BitConverter.GetBytes(_owner);
    }
}
