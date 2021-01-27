using System;
using System.Collections.Generic;

using Microsoft.ApplicationInsights.DataContracts;

using MondoCore.Common;
using MondoCore.Log;

using Newtonsoft.Json;

namespace MondoCore.ApplicationInsights
{
    internal static class ISupportPropertiesExtensions
    {
        internal static void MergeProperties(this ISupportProperties aiTelemetry, Telemetry telemetry)
        {
            var props = telemetry.Properties?.ToDictionary();

            if(props == null || props.Count == 0)
                return;

            foreach(var kv in props)
            {
                var val = kv.Value;

                if(val == null)
                    continue;

                if(val.GetType().IsPrimitive || val is string)
                {
                    aiTelemetry.Properties[kv.Key] = val.ToString();
                    continue;
                }

                var json = JsonConvert.SerializeObject(val);
                    
                aiTelemetry.Properties[kv.Key] = json;
            }
        }
    }
}
