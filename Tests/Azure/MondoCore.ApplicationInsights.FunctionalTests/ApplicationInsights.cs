using System;
using System.Threading.Tasks;

using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MondoCore.ApplicationInsights;
using MondoCore.Log;

namespace MondoCore.ApplicationInsights.FunctionalTests
{
    [TestClass]
    [TestCategory("Functional Tests")]
    public class ApplicationInsightsTests
    {
        private string _instrumentationKey = "9f9114ad-1137-41e6-a35b-babd54abbe49";
        private string _correlationId = Guid.NewGuid().ToString().ToLower();

        [TestMethod]
        public async Task ApplicationInsights_WriteException()
        {
            var log = new ApplicationInsights(new TelemetryConfiguration(_instrumentationKey));
             
            await log.WriteError(new System.Exception("Test Exception"), properties: new { Make = "Chevy", Model = "Camaro", Year = 1969 });
        }

        [TestMethod]
        public async Task ApplicationInsights_WriteEvent()
        {
            var log = new ApplicationInsights(new TelemetryConfiguration(_instrumentationKey));
             
            await log.WriteEvent("Test Event", properties: new { Make = "Chevy", Model = "Camaro", Year = 1969 });
        }

        private IRequestLog SetupRequest(string operationName)
        {
            var baseLog = new MondoCore.Log.Log();

            baseLog.Register(new ApplicationInsights(new TelemetryConfiguration(_instrumentationKey)));

            return new RequestLog(baseLog, operationName, _correlationId);
        }

        [TestMethod]
        public async Task ApplicationInsights_WriteError()
        {
            using(var log = SetupRequest("WriteError"))
            { 
                log.SetProperty("Model", "Corvette");

                await log.WriteError(new Exception("Bob's hair is on fire"), properties: new {Make = "Chevy" } );
                await log.WriteError(new Exception("Fred's hair is on fire"), properties: new {Make = "Chevy" } );
                await log.WriteError(new Exception("Wilma's hair is on fire"), properties: new {Make = "Chevy" } );
            }
        }

        [TestMethod]
        public async Task ApplicationInsights_WriteError2()
        {
            using(var log = SetupRequest("WriteError2"))
            { 
                log.SetProperty("Make", "Chevy");
                log.SetProperty("Model", "Corvette");

                await log.WriteEvent("John's feet are wet");
                await log.WriteError(new Exception("George's hair is on fire"));
                await log.WriteError(new Exception("Franks's hair is on fire"));
                await log.WriteError(new Exception("Naomi's hair is on fire"), Telemetry.LogSeverity.Critical);
            }
        }

        [TestMethod]
        public async Task ApplicationInsights_WriteError3()
        {
            using(var log = SetupRequest("WriteError3"))
            { 
                 await log.WriteEvent("John's hair is on fire", properties: new {Make = "Chevy", Model = "Corvette" } );
                 await log.WriteError(new Exception("Linda's hair is on fire"), properties: new {Make = "Chevy", Model = "Corvette" } );
            }
        }
    }
}
