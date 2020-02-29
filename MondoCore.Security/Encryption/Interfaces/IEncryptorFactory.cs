/****************************************************************************/
/*                                                                          */
/*    The MondoCore Libraries  							                    */
/*                                                                          */
/*        Namespace: MondoCore.Security.Encryption				            */
/*             File: IEncryptorFactory.cs					    		    */
/*        Class(es): IEncryptorFactory				         		        */
/*          Purpose: Interface for creating encryptors                      */
/*                                                                          */
/*  Original Author: Jim Lightfoot                                          */
/*    Creation Date: 1 Jan 2020                                             */
/*                                                                          */
/*   Copyright (c) 2005-2020 - Jim Lightfoot, All rights reserved           */
/*                                                                          */
/*  Licensed under the MIT license:                                         */
/*    http://www.opensource.org/licenses/mit-license.php                    */
/*                                                                          */
/****************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MondoCore.Security.Encryption
{
    /****************************************************************************/
    /****************************************************************************/
	public interface IEncryptorFactory
    {   
        /****************************************************************************/
	    Task<IEncryptor> GetValidForEncryption();
        Task<IEncryptor> GetValidForDecryption(Guid policyId);
    }
}
