
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using MondoCore.Log;
using MondoCore.Common;

namespace MondoCore.Log.UnitTests
{
    [TestClass]
    [TestCategory("Unit Tests")]
    public class LogTest
    {
        private ILog _log;
        private List<Telemetry> _errors = new List<Telemetry>();

        public LogTest()
        {
            var log = new Log();

            log.Register(new TestLog(_errors));

            _log = log;
        }

        [TestMethod]
        public async Task Log_WriteError()
        {
            await _log.WriteError(new Exception("Bob's hair is on fire"));

            Assert.AreEqual(1, _errors.Count);
            Assert.AreEqual(Telemetry.TelemetryType.Error, _errors[0].Type);
            Assert.AreEqual("Bob's hair is on fire", _errors[0].Exception.Message);
        }

        [TestMethod]
        public async Task Log_WriteError_wData()
        {
            var ex = new Exception("Bob's hair is on fire");

            ex.Data["Model"] = "Chevy";

            await _log.WriteError(ex);

            Assert.AreEqual(1, _errors.Count);
            Assert.AreEqual(Telemetry.TelemetryType.Error, _errors[0].Type);
            Assert.AreEqual("Bob's hair is on fire", _errors[0].Exception.Message);
            Assert.AreEqual("Chevy", _errors[0].Properties.ToDictionary()["Model"]);
        }

        [TestMethod]
        public async Task Log_WriteEvent()
        {
            await _log.WriteEvent("Race", new { Model = "Chevy" });

            Assert.AreEqual(1, _errors.Count);
            Assert.AreEqual(Telemetry.TelemetryType.Event, _errors[0].Type);
            Assert.AreEqual("Race", _errors[0].Message);
        }

        [TestMethod]
        public async Task Log_WriteTrace()
        {
            await _log.WriteTrace("Bob's hair is on fire", Telemetry.LogSeverity.Critical, new { Model = "Chevy" });

            Assert.AreEqual(1, _errors.Count);
            Assert.AreEqual(Telemetry.TelemetryType.Trace, _errors[0].Type);
            Assert.AreEqual("Bob's hair is on fire", _errors[0].Message);
        }

        [TestMethod]
        public async Task Log_WriteError_Fallback()
        {
            var log = new Log();
            var errors = new List<Telemetry>();
            var failLog = new Mock<ILog>();

            failLog.Setup( f=> f.WriteTelemetry(It.IsAny<Telemetry>())).Throws(new Exception("Whatever"));

            log.Register(failLog.Object);
            log.Register(new TestLog(errors), true);

            await log.WriteError(new Exception("Bob's hair is on fire"));

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(Telemetry.TelemetryType.Error, errors[0].Type);
            Assert.AreEqual("Bob's hair is on fire", errors[0].Exception.Message);
        }

        [TestMethod]
        public async Task Log_WriteError_MultipleLoggers()
        {
            var log        = new Log();
            var errors1    = new List<Telemetry>();
            var log1       = new TestLog(errors1);
            var errors2    = new List<Telemetry>();
            var log2       = new TestLog(errors2);
            var errors3    = new List<Telemetry>();
            var log3       = new TestLog(errors3);
            var failErrors = new List<Telemetry>();
            var failLog    = new TestLog(failErrors);

            log.Register(log1);
            log.Register(failLog, true);
            log.Register(log2);
            log.Register(log3);

            await log.WriteError(new Exception("Bob's hair is on fire"));

            Assert.AreEqual(1, errors1.Count);
            Assert.AreEqual(1, errors2.Count);
            Assert.AreEqual(1, errors3.Count);
            Assert.AreEqual(0, failErrors.Count);

            Assert.AreEqual(Telemetry.TelemetryType.Error, errors1[0].Type);
            Assert.AreEqual("Bob's hair is on fire", errors1[0].Exception.Message);

            Assert.AreEqual(Telemetry.TelemetryType.Error, errors2[0].Type);
            Assert.AreEqual("Bob's hair is on fire", errors2[0].Exception.Message);

            Assert.AreEqual(Telemetry.TelemetryType.Error, errors3[0].Type);
            Assert.AreEqual("Bob's hair is on fire", errors3[0].Exception.Message);
        }


        [TestMethod]
        public async Task Log_WriteError_MultipleLoggers_filtered()
        {
            var log        = new Log();
            var errors1    = new List<Telemetry>();
            var log1       = new TestLog(errors1);
            var errors2    = new List<Telemetry>();
            var log2       = new TestLog(errors2);
            var errors3    = new List<Telemetry>();
            var log3       = new TestLog(errors3);
            var errors4    = new List<Telemetry>();
            var log4       = new TestLog(errors4);
            var failErrors = new List<Telemetry>();
            var failLog    = new TestLog(failErrors);

            log.Register(log1);
            log.Register(failLog, true);
            log.Register(log2, types: new List<Telemetry.TelemetryType> { Telemetry.TelemetryType.Trace,   Telemetry.TelemetryType.Error } );
            log.Register(log3, types: new List<Telemetry.TelemetryType> { Telemetry.TelemetryType.Request, Telemetry.TelemetryType.Event } );
            log.Register(log4, types: new List<Telemetry.TelemetryType> { Telemetry.TelemetryType.Trace } );

            await log.WriteError(new Exception("Bob's hair is on fire"));
            await log.WriteEvent("No it's not");

            Assert.AreEqual(2, errors1.Count);
            Assert.AreEqual(1, errors2.Count);
            Assert.AreEqual(1, errors3.Count);
            Assert.AreEqual(0, errors4.Count);
            Assert.AreEqual(0, failErrors.Count);

            Assert.AreEqual(Telemetry.TelemetryType.Error, errors1[0].Type);
            Assert.AreEqual("Bob's hair is on fire", errors1[0].Exception.Message);
            Assert.AreEqual(Telemetry.TelemetryType.Event, errors1[1].Type);
            Assert.AreEqual("No it's not", errors1[1].Message);

            Assert.AreEqual(Telemetry.TelemetryType.Error, errors2[0].Type);
            Assert.AreEqual("Bob's hair is on fire", errors2[0].Exception.Message);

            Assert.AreEqual(Telemetry.TelemetryType.Event, errors3[0].Type);
            Assert.AreEqual("No it's not", errors3[0].Message);
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

            public IDisposable StartOperation(string operationName)
            {
                return null;
            }
        }
    }
}
