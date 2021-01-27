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
    /// Log requests contain request specific data like a 
    ///    correlation id and custom properties that will be logged on all log entries
    /// </summary>
    /*************************************************************************/
    public interface IRequestLog : ILog, IDisposable
    {
        /// <summary>
        /// Sets a property for this request. This property will be logged with every subsequent logging entry
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void SetProperty(string name, object value);
    }
}