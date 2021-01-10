/*************************************************************************** 
 *                                                                           
 *    The MondoCore Libraries  	                                             
 *                                                                           
 *      Namespace: MondoCore.Common	                                         
 *           File: ISettings.cs                                                 
 *      Class(es): ISettings                  
 *        Purpose: Generic settings interface                                   
 *                                                                           
 *  Original Author: Jim Lightfoot                                           
 *    Creation Date: 12 Dec 2020                                             
 *                                                                           
 *   Copyright (c) 2020 - Jim Lightfoot, All rights reserved            
 *                                                                           
 *  Licensed under the MIT license:                                          
 *    http://www.opensource.org/licenses/mit-license.php                     
 *                                                                           
 ****************************************************************************/

using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MondoCore.Common
{
    /****************************************************************************/
    /****************************************************************************/
    public interface ISettings
    {
        string Get(string key);
    }
}
