/****************************************************************************/
/*                                                                          */
/*    The MondoCore Libraries  							                    */
/*                                                                          */
/*        Namespace: MondoCore.Security.Passwords				            */
/*             File: IPasswordManager.cs			 		    		    */
/*        Class(es): IPasswordManager				           		        */
/*          Purpose: Interface for managing passwords                       */
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
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace MondoCore.Security.Passwords
{
    public interface IPasswordManager
    {
        Password       FromOwner(string password, IPasswordOwner owner);
        string         GenerateNew(int length);
        Task<Password> Load(IPasswordOwner owner);
        Task           Save(Password password);
    }
}
 