/****************************************************************************/
/*                                                                          */
/*    The MondoCore Libraries  	                                            */
/*                                                                          */
/*      Namespace: MondoCore.Common	                                        */
/*           File: IBlobStore.cs                                            */
/*      Class(es): IBlobStore                                               */
/*        Purpose: Generic interface for storing blobs                      */
/*                                                                          */
/*  Original Author: Jim Lightfoot                                          */
/*    Creation Date: 29 Nov 2015                                            */
/*                                                                          */
/*   Copyright (c) 2015-2020 - Jim Lightfoot, All rights reserved           */
/*                                                                          */
/*  Licensed under the MIT license:                                         */
/*    http://www.opensource.org/licenses/mit-license.php                    */
/*                                                                          */
/****************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MondoCore.Common
{
    /****************************************************************************/
    /****************************************************************************/
    public interface IBlobStore
    {
        Task<string>              Get(string id, Encoding encoding = null);
        Task<byte[]>              GetBytes(string id);
        Task                      Put(string id, string blob);
        Task                      Delete(string id);
        Task<IEnumerable<string>> Find(string filter);
    }
}
