/****************************************************************************/
/*                                                                          */
/*    The MondoCore Libraries  							                    */
/*                                                                          */
/*        Namespace: MondoCore.Security.Encryption				            */
/*             File: IKey.cs					    		                */
/*        Class(es): IKey				         		                    */
/*          Purpose: Interface for encryption keys                          */
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

namespace MondoCore.Security.Encryption
{
    public interface IKey : IDisposable
    {   
        Guid             Id     { get; }
        EncryptionPolicy Policy { get; }

        byte[] ToArray();
    }
}
