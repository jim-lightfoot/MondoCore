/***************************************************************************
 *                                                                          
 *    The MondoCore Libraries  						                        
 *                                                                          
 *        Namespace: MondoCore.Log 			            
 *             File: LogExtensions.cs							
 *        Class(es): LogExtensions							    
 *          Purpose: Extensions methods for ILog inteface            
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
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;

using MondoCore.Common;

namespace MondoCore.Log
{
    /*************************************************************************/
    /*************************************************************************/
    public static class LogExtensions
    {
        /// <summary>
        /// Write an exception to the log
        /// </summary>
        /// <example>
        ///     Use anonymous objects to pass properties:
        ///     log.WriteEvent("Message received", new { Category = "Blue", Level = 4 });
        /// </example>        
        /// <example>
        ///   Use dictionary to pass properties:
        ///     log.WriteEvent("Message received", new Dictionary<string, object> { {"Category", "Blue"}, {"Level", 4"} });
        /// </example>
        /// <example>
        ///   Use xml to pass properties (only logs elements under root):
        ///     log.WriteEvent("Message received", XmlDoc.LoadXml("<Root><Category>Blue</Category><Level>4</Level></Root>") );
        /// </example>
        /// <param name="log">Log to write to</param>
        /// <param name="ex">Exception to log</param>
        /// <param name="properties">See examples</param>
        /// <param name="correlationId">A value to correlate actions across calls and processes</param>
        public static Task WriteError(this ILog log, Exception ex, Telemetry.LogSeverity severity = Telemetry.LogSeverity.Error, object properties = null, string correlationId = null)
        {
            return log.WriteTelemetry(new Telemetry { 
                                                        Type          = Telemetry.TelemetryType.Error, 
                                                        Exception     = ex,
                                                        Severity      = severity,
                                                        CorrelationId = correlationId,
                                                        Properties    = properties == null ? (object)ex.Data : (properties.ToDictionary().Merge(ex.Data.ToDictionary()))
                                                    });
        }

        /// <summary>
        /// Write an event to the log
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="eventName">Name of event to write</param>
        /// <param name="properties">See examples in WriteError</param>
        /// <param name="metrics">An optional dictionary of metrics to write</param>
        /// <param name="correlationId">A value to correlate actions across calls and processes</param>
        public static Task WriteEvent(this ILog log, string eventName, object properties = null, Dictionary<string, double> metrics = null, string correlationId = null)
        {
            return log.WriteTelemetry(new Telemetry { 
                                                        Type          = Telemetry.TelemetryType.Event, 
                                                        Message       = eventName,
                                                        CorrelationId = correlationId,
                                                        Properties    = properties,
                                                        Metrics       = metrics
                                                    });
        }

        /// <summary>
        /// Write a metric to the log
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="metricName">Name of metric to write</param>
        /// <param name="value">Value of metric</param>
        /// <param name="properties">See examples in WriteError</param>
        /// <param name="correlationId">A value to correlate actions across calls and processes</param>
        public static Task WriteMetric(this ILog log, string metricName, double value, object properties = null, string correlationId = null)
        {
            return log.WriteTelemetry(new Telemetry { 
                                                        Type          = Telemetry.TelemetryType.Metric, 
                                                        Message       = metricName,
                                                        Value         = value,
                                                        CorrelationId = correlationId,
                                                        Properties    = properties
                                                    });
        }

        /// <summary>
        /// Write a trace to the log
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="message">Message to write</param>
        /// <param name="severity">Severity of trace</param>
        /// <param name="properties">See examples in WriteError</param>
        /// <param name="correlationId">A value to correlate actions across calls and processes</param>
        public static Task WriteTrace(this ILog log, string message, Telemetry.LogSeverity severity, object properties = null, string correlationId = null)
        {
            return log.WriteTelemetry(new Telemetry { 
                                                        Type          = Telemetry.TelemetryType.Trace, 
                                                        Message       = message,
                                                        Severity      = severity,
                                                        CorrelationId = correlationId,
                                                        Properties    = properties
                                                    });
        }

        /// <summary>
        /// Write a request to the log
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="name">Name of request</param>
        /// <param name="startTime">Time request started</param>
        /// <param name="duration">Duration of request</param>
        /// <param name="responseCode">Response code returned from request</param>
        /// <param name="success">True if request was successful</param>
        /// <param name="correlationId">A value to correlate actions across calls and processes</param>
        public static Task WriteRequest(this ILog log, string name, DateTime startTime, TimeSpan duration, string responseCode, bool success, object properties = null, string correlationId = null)
        {
            return log.WriteTelemetry(new Telemetry { 
                                                        Type          = Telemetry.TelemetryType.Request, 
                                                        Message       = name,
                                                        CorrelationId = correlationId,
                                                        Properties    = properties,
                                                        Request       = new Telemetry.RequestParams
                                                        {
                                                            StartTime    = startTime,
                                                            Duration     = duration,
                                                            ResponseCode = responseCode,
                                                            Success      = success
                                                        }
                                                    });
        }
    }
}