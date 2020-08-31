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
        [TestMethod]
        public async Task ApplicationInsights_WriteException()
        {
            var instrumentationKey = "";
            var log = new ApplicationInsights(new TelemetryConfiguration(instrumentationKey));
             
            await log.WriteError(new System.Exception("Test Exception"), properties: new { Make = "Chevy", Model = "Camaro", Year = 1969 });
        }

        [TestMethod]
        public async Task ApplicationInsights_WriteEvent()
        {
            var instrumentationKey = "";
            var log = new ApplicationInsights(new TelemetryConfiguration(instrumentationKey));
             
            await log.WriteEvent("Test Event", properties: new { Make = "Chevy", Model = "Camaro", Year = 1969 });
        }
    }
}
