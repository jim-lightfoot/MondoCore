
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using MondoCore.Common;
using MondoCore.Log;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

namespace MondoCore.Log.UnitTests
{
    [TestClass]
    [TestCategory("Unit Tests")]
    public class RequestLogTest
    {
        private IRequestLog _log;
        private List<Telemetry> _errors = new List<Telemetry>();

        public RequestLogTest()
        {
            var log = new Log();

            log.Register(new TestLog(_errors));

            _log = new RequestLog(log);
        }

        [TestMethod]
        public async Task Log_WriteError()
        {
            _log.SetProperty("Model", "Corvette");

            await _log.WriteError(new Exception("Bob's hair is on fire"), properties: new {Make = "Chevy" } );

            Assert.AreEqual(1, _errors.Count);
            Assert.AreEqual(Telemetry.TelemetryType.Error, _errors[0].Type);
            Assert.AreEqual("Bob's hair is on fire", _errors[0].Exception.Message);

            var props = _errors[0].Properties.ToDictionary();

            Assert.AreEqual("Chevy", props["Make"]);
            Assert.AreEqual("Corvette", props["Model"]);
        }

        [TestMethod]
        public async Task Log_WriteError2()
        {
            _log.SetProperty("Make", "Chevy");
            _log.SetProperty("Model", "Corvette");

            await _log.WriteError(new Exception("Bob's hair is on fire"));

            Assert.AreEqual(1, _errors.Count);
            Assert.AreEqual(Telemetry.TelemetryType.Error, _errors[0].Type);
            Assert.AreEqual("Bob's hair is on fire", _errors[0].Exception.Message);

            var props = _errors[0].Properties.ToDictionary();

            Assert.AreEqual("Chevy", props["Make"]);
            Assert.AreEqual("Corvette", props["Model"]);
        }

        [TestMethod]
        public async Task Log_WriteError3()
        {
            await _log.WriteError(new Exception("Bob's hair is on fire"), properties: new {Make = "Chevy", Model = "Corvette" } );

            Assert.AreEqual(1, _errors.Count);
            Assert.AreEqual(Telemetry.TelemetryType.Error, _errors[0].Type);
            Assert.AreEqual("Bob's hair is on fire", _errors[0].Exception.Message);

            var props = _errors[0].Properties.ToDictionary();

            Assert.AreEqual("Chevy", props["Make"]);
            Assert.AreEqual("Corvette", props["Model"]);
        }

        /*************************************************************************/
        /*************************************************************************/
        internal class TestLog : ILog
        {
            private readonly List<Telemetry> _entries;

            /*************************************************************************/
            internal TestLog(List<Telemetry> entries)
            {
                _entries = entries;
            }

            /*************************************************************************/
            public Task WriteTelemetry(Telemetry telemetry)
            {
                _entries.Add(telemetry);

                return Task.CompletedTask;
            }

            public void SetProperty(string name, string value)
            {
                throw new NotImplementedException();
            }
        }
    }
}
