/****************************************************************************/
/*                                                                          */
/*    The MondoCore Libraries  							                    */
/*                                                                          */
/*        Namespace: MondoCore.Security.Encryption   			            */
/*             File: IEncryptor.cs					    		            */
/*        Class(es): IEncryptor				         		                */
/*          Purpose: Interface for encryption and decryption                */
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
using System.IO;
using System.Threading.Tasks;

namespace MondoCore.Security.Encryption
{
    /****************************************************************************/
    /****************************************************************************/
	public interface IEncryptor 
	{
        Task<byte[]>     Encrypt(byte[] aData);
        Task<byte[]>     Decrypt(byte[] aEncrypted, int offset = 0);
                         
        Task             Encrypt(Stream input, Stream output);
        Task             Decrypt(Stream input, Stream output);

        EncryptionPolicy Policy { get; }
    }
}
