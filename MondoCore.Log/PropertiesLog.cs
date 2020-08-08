/***************************************************************************
 *                                                                          
 *    The MondoCore Libraries  						                        
 *                                                                          
 *        Namespace: MondoCore.Log 			            
 *             File: PropertiesLog.cs							
 *        Class(es): PropertiesLog							    
 *          Purpose: A transient log capturing properties during a single request              
 *                                                                          
 *  Original Author: Jim Lightfoot                                          
 *    Creation Date: 1 Aug 2020                                            
 *                                                                          
 *   Copyright (c) 2020 - Jim Lightfoot, All rights reserved                
 *                                                                          
 *  Licensed under the MIT license:                                         
 *    http://www.opensource.org/licenses/mit-license.php                    
 *                                                                          
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MondoCore.Log
{
    /*************************************************************************/
    /*************************************************************************/
    public class PropertiesLog : ILog
    {
        private readonly Dictionary<string, string> _properties = new Dictionary<string, string>();
        private readonly ILog _parentLog;

        /*************************************************************************/
        public PropertiesLog(ILog parentLog)
        {
            _parentLog = parentLog;
        }

        /*************************************************************************/
        public void SetProperty(string name, string value)
        {
            _properties[name] = value;
        }

        /*************************************************************************/
        public Task WriteTelemetry(Telemetry telemetry)
        {
            return _parentLog.WriteTelemetry(telemetry);
        }
    }
}