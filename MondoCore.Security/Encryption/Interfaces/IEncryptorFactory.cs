/****************************************************************************
 *                                                                         
 *  The MondoCore Libraries 							                   
 *                                                                         
 *    Namespace: MondoCore.Security.Encryption				               
 *         File: IEncryptorFactory.cs					  		               
 *    Class(es): IEncryptorFactory				     		               
 *      Purpose: Interface for creating encryptors                  
 *                                      
 * Original Author: Jim Lightfoot                                          
 *  Creation Date: 7 Feb 2020                       
 *                                     
 *  Copyright (c) 2020 - Jim Lightfoot, All rights reserved        
 *                                     
 * Licensed under the MIT license:                     
 *  http://www.opensource.org/licenses/mit-license.php           
 *                                      
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MondoCore.Security.Encryption
{
    /****************************************************************************/
    /****************************************************************************/
    /// <summary>
    /// Interface for creating encryptors  
    /// </summary>
    public interface IEncryptorFactory
    {  
        /// <summary>
        /// Creates a new encryptor using the given key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IEncryptor Create(IKey key);
  }
}
