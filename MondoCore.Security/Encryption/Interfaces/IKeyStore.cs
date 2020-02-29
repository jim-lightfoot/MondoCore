/****************************************************************************/
/*                                                                          */
/*    The MondoCore Libraries  							                    */
/*                                                                          */
/*        Namespace: MondoCore.Security.Encryption				            */
/*             File: IKeyStore.cs					    		            */
/*        Class(es): IKeyStore				         		                */
/*          Purpose: Interface for persisting encryption keys               */
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
using System.Threading.Tasks;

namespace MondoCore.Security.Encryption
{
    /****************************************************************************/
    /****************************************************************************/
    public interface IKeyStore
    {   
        Task<IKey>              Get(Guid id);
        Task<IEnumerable<IKey>> GetAll();
        Task                    Add(IKey key);
        Task                    Remove(Guid id);
    }
}
