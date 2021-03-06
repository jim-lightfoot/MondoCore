﻿/****************************************************************************
 *                                                                         
 *  The MondoCore Libraries 							                   
 *                                                                         
 *    Namespace: MondoCore.Security.Encryption				               
 *         File: PassThruEncryptor.cs					    		  		               
 *    Class(es): PassThruEncryptor				         		  	               
 *      Purpose: A fake encryptor                       
 *                                      
 * Original Author: Jim Lightfoot                                          
 *  Creation Date: 19 Jan 2020                       
 *                                     
 *  Copyright (c) 2020 - Jim Lightfoot, All rights reserved        
 *                                     
 * Licensed under the MIT license:                     
 *  http://www.opensource.org/licenses/mit-license.php           
 *                                      
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using MondoCore.Common;

namespace MondoCore.Security.Encryption
{
    /****************************************************************************/
    /****************************************************************************/
    public class PassThruEncryptor : IEncryptor
    {
        public EncryptionPolicy Policy => throw new NotImplementedException();

        /****************************************************************************/
        public Task<byte[]> Decrypt(byte[] aEncrypted, int offset = 0)
        {
            return Task.FromResult(aEncrypted.DeepClone());
        }

        /****************************************************************************/
        public async Task Decrypt(Stream input, Stream output)
        {
            await input.CopyToAsync(output);
        }

        /****************************************************************************/
        public Task<byte[]> Encrypt(byte[] aData)
        {
            return Task.FromResult(aData.DeepClone());
        }

        /****************************************************************************/
        public async Task Encrypt(Stream input, Stream output)
        {
            await input.CopyToAsync(output);
        }
    }
}
