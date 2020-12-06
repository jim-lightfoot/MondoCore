/***************************************************************************
 *                                                                          
 *    The MondoCore Libraries  						                        
 *                                                                          
 *        Namespace: MondoCore.Log 			            
 *             File: IRequestLog.cs							
 *        Class(es): IRequestLog							    
 *          Purpose: Logging while in a single request              
 *                                                                          
 *  Original Author: Jim Lightfoot                                          
 *    Creation Date: 8 Aug 2020                                             
 *                                                                          
 *   Copyright (c) 2020 - Jim Lightfoot, All rights reserved                
 *                                                                          
 *  Licensed under the MIT license:                                         
 *    http://www.opensource.org/licenses/mit-license.php                    
 *                                                                          
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MondoCore.Log
{
    /*************************************************************************/
    /// <summary>
    /// Logging while in a single request
    /// </summary>
    /*************************************************************************/
    public interface IRequestLog : ILog, IDisposable
    {
        void SetProperty(string name, string value);
    }
}