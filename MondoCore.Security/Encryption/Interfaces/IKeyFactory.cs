/****************************************************************************/
/*                                                                          */
/*    The MondoCore Libraries  							                    */
/*                                                                          */
/*        Namespace: MondoCore.Security.Encryption				            */
/*             File: IKeyFactory.cs					    		            */
/*        Class(es): IKeyFactory				         		            */
/*          Purpose: Interface for creating encryption keys                 */
/*                                                                          */
/*  Original Author: Jim Lightfoot                                          */
/*    Creation Date: 19 Jan 2020                                            */
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
using System.Threading.Tasks;

namespace MondoCore.Security.Encryption
{
    /****************************************************************************/
    /****************************************************************************/
    public interface IKeyFactory
    {   
        /// <summary>
        /// Get an existing key for decryption
        /// </summary>
        /// <param name="keyId"></param>
        Task<IKey> GetDecryptionKey(Guid keyId);

        /// <summary>
        /// Get a valid key for encryption
        /// </summary>
        Task<IKey> GetEncryptionKey();
    }
}
