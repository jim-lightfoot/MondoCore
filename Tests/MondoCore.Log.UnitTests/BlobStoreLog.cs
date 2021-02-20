
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using MondoCore.Log;
using MondoCore.Common;

using Newtonsoft.Json;

namespace MondoCore.Log.UnitTests
{
    [TestClass]
    [TestCategory("Unit Tests")]
    public class BlobStoreLogTest
    {
        private ILog _log;
        private MemoryStore _store = new MemoryStore();

        public BlobStoreLogTest()
        {
            _log = new BlobStoreLog(_store);
        }

        [TestMethod]
        public async Task BlobStoreLog_WriteError()
        {
            await _log.WriteError(new Exception("Bob's hair is on fire"));

            Assert.AreEqual(1, _store.Count);

            var telemetry = JsonConvert.DeserializeObject<Telemetry>(Encoding.UTF8.GetString(_store[0]));

            Assert.AreEqual(Telemetry.TelemetryType.Error, telemetry.Type);
            Assert.AreEqual("Bob's hair is on fire", telemetry.Exception.Message);
        }

        [TestMethod]
        public async Task BlobStoreLog_WriteError_wData()
        {
            var ex = new Exception("Bob's hair is on fire");

            ex.Data["Model"] = "Chevy";

            await _log.WriteError(ex);

            var json = Encoding.UTF8.GetString(_store[0]);
            var telemetry = JsonConvert.DeserializeObject<Telemetry>(Encoding.UTF8.GetString(_store[0]));
            var props = JsonConvert.DeserializeObject<Dictionary<string, string>>(telemetry.Properties.ToString());

            Assert.AreEqual(1, _store.Count);
            Assert.AreEqual(Telemetry.TelemetryType.Error, telemetry.Type);
            Assert.AreEqual("Bob's hair is on fire", telemetry.Exception.Message);
            Assert.AreEqual("Chevy", props["Model"]);
        }

        [TestMethod]
        public async Task BlobStoreLog_WriteEvent()
        {
            await _log.WriteEvent("Race", new { Model = "Chevy" });

            var telemetry = JsonConvert.DeserializeObject<Telemetry>(Encoding.UTF8.GetString(_store[0]));
            var props = JsonConvert.DeserializeObject<Dictionary<string, string>>(telemetry.Properties.ToString());

            Assert.AreEqual(1, _store.Count);
            Assert.AreEqual(Telemetry.TelemetryType.Event, telemetry.Type);
            Assert.AreEqual("Race", telemetry.Message);
            Assert.AreEqual("Chevy", props["Model"]);
        }

        [TestMethod]
        public async Task BlobStoreLog_WriteTrace()
        {
            await _log.WriteTrace("Bob's hair is on fire", Telemetry.LogSeverity.Critical, new { Model = "Chevy" });

            var telemetry = JsonConvert.DeserializeObject<Telemetry>(Encoding.UTF8.GetString(_store[0]));

            Assert.AreEqual(1, _store.Count);
            Assert.AreEqual(Telemetry.TelemetryType.Trace, telemetry.Type);
            Assert.AreEqual(Telemetry.LogSeverity.Critical, telemetry.Severity);
            Assert.AreEqual("Bob's hair is on fire", telemetry.Message);
        }
    }
}
