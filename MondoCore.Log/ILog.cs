/***************************************************************************
 *                                                                          
 *    The MondoCore Libraries  						                        
 *                                                                          
 *        Namespace: MondoCore.Log 			            
 *             File: ILog.cs							
 *        Class(es): ILog							    
 *          Purpose: Generic interface for logging              
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
using System.Threading;
using System.Threading.Tasks;

namespace MondoCore.Log
{
    /*************************************************************************/
    /// <summary>
    /// Generic interface for logging
    /// </summary>
    /*************************************************************************/
    public interface ILog
    {
        Task WriteTelemetry(Telemetry telemetry);
    }
        
    /*************************************************************************/
    /*************************************************************************/
    public class Telemetry
    {
        public TelemetryType    Type       { get; set; }
        public string           Message    { get; set; }
        public Exception        Exception  { get; set; }
        public object           Properties { get; set; }
        public double           Value      { get; set; }
        public LogSeverity      Severity   { get; set; }

        public IDictionary<string, double> Metrics { get; set; }
        public RequestParams Request     { get; set; }

        public enum TelemetryType
        {
            Error,
            Event,
            Metric,
            Trace,
            Request
        }

        public class RequestParams
        {
            public DateTime StartTime    { get; set; } 
            public TimeSpan Duration     { get; set; } 
            public string   ResponseCode { get; set; } 
            public bool     Success      { get; set; }
        }

        public enum LogSeverity
        {
            Verbose     = 0,
            Information = 1,
            Warning     = 2,
            Error       = 3,
            Critical    = 4
        }
    }       
}