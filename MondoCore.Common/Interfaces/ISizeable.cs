/*************************************************************************** 
 *                                                                           
 *    The MondoCore Libraries  	                                             
 *                                                                           
 *      Namespace: MondoCore.Common	                                         
 *           File: ISizeable.cs                                             
 *      Class(es): ISizeable                                                
 *        Purpose: Generic interface for things that can be resized                  
 *                                                                           
 *  Original Author: Jim Lightfoot                                           
 *    Creation Date: 7 Jan 2021                                             
 *                                                                           
 *   Copyright (c) 2021 - Jim Lightfoot, All rights reserved            
 *                                                                           
 *  Licensed under the MIT license:                                          
 *    http://www.opensource.org/licenses/mit-license.php                     
 *                                                                           
 ****************************************************************************/

using System;
using System.Threading.Tasks;

namespace MondoCore.Common
{
    public interface ISizeable
    {
        long Size { get; }
        Task ResizeAsync(long newSize);
        void Resize(long newSize);
    }
}
