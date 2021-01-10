/***************************************************************************
 *                                                                          
 *    The MondoCore Libraries  						                        
 *                                                                          
 *        Namespace: MondoCore.Log 			            
 *             File: Log.cs							
 *        Class(es): Log							    
 *          Purpose: Main class for logging              
 *                                                                          
 *  Original Author: Jim Lightfoot                                          
 *    Creation Date: 11 Apr 2014                                             
 *                                                                          
 *   Copyright (c) 2014-2020 - Jim Lightfoot, All rights reserved                
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
    public sealed class Log : ILog
    {
        private readonly List<LogEntry> _logs = new List<LogEntry>();

        /*************************************************************************/
        public void Register(ILog log, bool fallbackOnly = false, bool fallbackAsError = false)
        {
            _logs.Add(new LogEntry {Log = log, FallbackOnly = fallbackOnly, FallbackAsError = fallbackAsError});
        }

        /*************************************************************************/
        public void ClearRegistered()
        {
            _logs.Clear();
        }

        #region ILog

        /*************************************************************************/
        public async Task WriteTelemetry(Telemetry telemetry)
        {
            // Don't need to log these
            if(telemetry.Type == Telemetry.TelemetryType.Error && telemetry.Exception is ThreadAbortException)
            { 
                return;
            }

            var nLoggers = _logs.Count;
            var tasks    = new List<Task>();

            for (var i = 0; i < nLoggers; ++i)
            {
                var logger = _logs[i];

                // Only write primary telemetry to non-fallback loggers
                if (!logger.FallbackOnly)
                {
                    tasks.Add(WriteTelemetry(logger.Log, telemetry, i));
                }
            }
            
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        /*************************************************************************/
        public IDisposable StartOperation(string operationName)
        {
            var ops = new OperationList();

            foreach(var logger in _logs)
            {
                var op = logger.Log.StartOperation(operationName);

                if(op != null)
                    ops.Add(op);
            }

            return ops;
        }

        /*************************************************************************/
        private class OperationList : List<IDisposable>, IDisposable
        {
            internal OperationList()
            {
            }

            public void Dispose()
            {
                foreach(var op in this)
                    op.Dispose();
            }
        }

        #endregion

        #region Private Methods

        /*************************************************************************/
        private async Task WriteTelemetry(ILog log, Telemetry telemetry, int index)
        {
            try
            {
                var retries = 5;
                Exception exLast = null;

                while(retries-- > 0)
                {
                    try
                    { 
                        await log.WriteTelemetry(telemetry).ConfigureAwait(false);

                        break;
                    }
                    catch(Exception ex)
                    {
                        exLast = ex;

                        if(retries > 0)
                            await Task.Delay(50).ConfigureAwait(false);
                    }
                }

                if(exLast != null)
                    throw exLast;
            }
            catch (Exception ex2)
            {
                // Ok that didn't work, write to a fallback log
                await FallbackTelemetry(telemetry, index + 1, ex2).ConfigureAwait(false);
            }
       }

        /*************************************************************************/
        private async Task FallbackTelemetry(Telemetry telemetry, int start, Exception excep)
        {
            var nLoggers = _logs.Count;

            // Go through all the loggers after the last one
            for (var j = start; j < nLoggers; ++j)
            {
                var fallBackLogger = _logs[j];

                // Find the first fallback logger
                if (fallBackLogger.FallbackOnly)
                {
                    try
                    {
                        // Log the exception from the previously failed log
                        if (fallBackLogger.FallbackAsError)
                        {
                            try
                            {
                                await fallBackLogger.Log.WriteError(excep).ConfigureAwait(false);
                            }
                            catch
                            {
                                // This is bad
                            }
                        }
                        else // or write the original telemetry
                            await fallBackLogger.Log.WriteTelemetry(telemetry).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        // The fallback logger failed, fallback to the next
                        await FallbackTelemetry(telemetry, start + 1, ex).ConfigureAwait(false);
                    }

                    break;
                }
            }
        }

        /*************************************************************************/
        private struct LogEntry
        {
            internal bool FallbackAsError;
            internal bool FallbackOnly;
            internal ILog Log;
        }

        #endregion
    }
}