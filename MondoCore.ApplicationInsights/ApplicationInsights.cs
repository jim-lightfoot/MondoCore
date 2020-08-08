/***************************************************************************
 *                                                                          
 *    The MondoCore Libraries  	                                            
 *                                                                          
 *      Namespace: MondoCore.ApplicationInsights	                                        
 *           File: ApplicationInsights.cs                                       
 *      Class(es): ApplicationInsights                                          
 *        Purpose: Implementation of ILog interface for Azure Application Insights                            
 *                                                                          
 *  Original Author: Jim Lightfoot                                          
 *    Creation Date: 29 Nov 2015                                            
 *                                                                          
 *   Copyright (c) 2015-2020 - Jim Lightfoot, All rights reserved           
 *                                                                          
 *  Licensed under the MIT license:                                         
 *    http://www.opensource.org/licenses/mit-license.php                    
 *                                                                          
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

using MondoCore.Common;
using MondoCore.Log;

namespace MondoCore.ApplicationInsights
{
    public class ApplicationInsights : ILog
    {
        private readonly TelemetryClient _client;

        public ApplicationInsights(TelemetryConfiguration telemetryConfiguration)
        {
            _client = new TelemetryClient(telemetryConfiguration);
        }

        public async Task WriteTelemetry(Telemetry telemetry)
        {
            await Task.Yield();

            var props = telemetry.Properties?.ToStringDictionary();
            
            switch(telemetry.Type)
            {
                case Telemetry.TelemetryType.Error:
                    _client.TrackException(telemetry.Exception, properties: props);
                    break;

                case Telemetry.TelemetryType.Event:
                    _client.TrackEvent(telemetry.Message, properties: props);
                    break;

                case Telemetry.TelemetryType.Metric:
                    _client.TrackMetric(telemetry.Message, telemetry.Value, properties: props);
                    break;

                case Telemetry.TelemetryType.Trace:
                    _client.TrackTrace(telemetry.Message, (SeverityLevel)((int)telemetry.Severity), properties: props);
                    break;

                case Telemetry.TelemetryType.Request:
                    _client.TrackRequest(telemetry.Message, 
                                            telemetry.Request.StartTime, 
                                            telemetry.Request.Duration, 
                                            telemetry.Request.ResponseCode,
                                            telemetry.Request.Success);
                    break;
            }

            _client.Flush();

            return;
        }
    }
}
