/***************************************************************************
 *                                                                          
 *    The MondoCore Libraries  						                        
 *                                                                          
 *        Namespace: MondoCore.Security.Encryption				            
 *             File: SymmetricEncryptorFactory.cs							
 *        Class(es): SymmetricEncryptorFactory							    
 *          Purpose: A factory to produce SymmetricEncryptors               
 *                                                                          
 *  Original Author: Jim Lightfoot                                          
 *    Creation Date: 14 Jan 2020                                            
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
using System.Threading.Tasks;

using MondoCore.Common;

namespace MondoCore.Security.Encryption
{
    /****************************************************************************/
    /****************************************************************************/
    public class SymmetricEncryptorFactory : IEncryptorFactory
    {
        /****************************************************************************/
        public SymmetricEncryptorFactory()
        {
        }

        #region IEncryptorFactory

        /****************************************************************************/
        public IEncryptor Create(IKey key)
        {
            return new SymmetricEncryptor(key);
        }

        #endregion
    }
}
