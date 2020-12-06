﻿/***************************************************************************
 *                                                                          
 *    The MondoCore Libraries  						                        
 *                                                                          
 *        Namespace: MondoCore.Log 			            
 *             File: RequestLog.cs							
 *        Class(es): RequestLog							    
 *          Purpose: Logging during a single request            
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
using System.Threading.Tasks;

using MondoCore.Common;

namespace MondoCore.Log
{
    /*************************************************************************/
    /*************************************************************************/
    public class RequestLog : IRequestLog
    {
        private readonly ILog           _log;
        private readonly IDisposable    _operation;
        private readonly IDictionary<string, string> _properties = new Dictionary<string, string>();
        private readonly string         _correlationId;
        private readonly string         _operationName;

        /*************************************************************************/
        public RequestLog(ILog log, string operationName = null, string correlationId = null)
        {
            _log = log;
            _correlationId = correlationId ?? Guid.NewGuid().ToString().ToLower();
            _operationName = operationName;

            if(!string.IsNullOrWhiteSpace(operationName))
                _operation = log.StartOperation(operationName);
            else
                _operation = null;
        }

        /*************************************************************************/
        public void SetProperty(string name, string value)
        {
            _properties[name] = value;
        }

        /*************************************************************************/
        public Task WriteTelemetry(Telemetry telemetry)
        {
            if(_properties.Count > 0)
                telemetry.Properties = telemetry.Properties.ToStringDictionary().Merge(_properties);

            telemetry.CorrelationId = _correlationId;
            telemetry.OperationName = _operationName;

            return _log.WriteTelemetry(telemetry);
        }

        /*************************************************************************/
        public IDisposable StartOperation(string operationName)
        {
           return _log.StartOperation(operationName);
        }
        
        #region IDisposable

        public void Dispose()
        {
            _operation?.Dispose();
        }

        #endregion

    }
}