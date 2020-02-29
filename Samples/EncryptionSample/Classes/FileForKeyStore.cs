/****************************************************************************/
/*                                                                          */
/*    The MondoCore Libraries  							                    */
/*                                                                          */
/*        Namespace: MondoCore.Common							            */
/*             File: FileStore.cs					    		            */
/*        Class(es): FileStore				         		                */
/*          Purpose: IBlobStore wrapper for file system                     */
/*                                                                          */
/*  Original Author: Jim Lightfoot                                          */
/*    Creation Date: 20 Jan 2020                                            */
/*                                                                          */
/*   Copyright (c) 2020 - Jim Lightfoot, All rights reserved                */
/*                                                                          */
/*  Licensed under the MIT license:                                         */
/*    http://www.opensource.org/licenses/mit-license.php                    */
/*                                                                          */
/****************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MondoCore.Common;

namespace EncryptionSample
{
    /****************************************************************************/
    /****************************************************************************/
    public class FileForKeyStore : FileStore
    {
        /****************************************************************************/
        public FileForKeyStore(string pathRoot) : base(pathRoot)
        {
        }

        #region IBlobStore 

        /****************************************************************************/
        public override async Task<IEnumerable<string>> Find(string filter)
        {
            var files = await base.Find(filter);

            return await Task.FromResult(files.Select( (fi)=> Path.GetFileNameWithoutExtension(fi) ));
        }

        #endregion
    }
}
