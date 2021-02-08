# MondoCore.Log
  Classes for logging

<br>

### Dependency Injection

In your dependency injection code create a singleton instance of Log

    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Register the main Log class as a singleton.
            builder.Services.AddSingleton<ILog>( (p)=> 
            {
                var log = new Log();

                // Use GetService to get the TelemetryConfiguration to share with the host
                log.Register( new ApplicationInsights( p.GetService<TelemetryConfiguration>() ),
                              new Splunk("???"), types: new List<Telemetry.TelemetryType> { Telemetry.TelemetryType.Event } ); // Log only WriteEvent telemetry

                return log;
            );

            // Register IRequestLog as scoped. That same instance will be returned within the scope of a function invocation
            builder.Services.AddScoped<IRequestLog, RequestLog>();
        }
    }

<br>

### Logging with ILog

Inject the ILog interface
 
    using MondoCore.Log;

    public class CoolClass
    {
        private readonly ILog _log;

        public CoolClass(ILog log)
        {
            _log = log;
        }

        public async Task DoSomething(string name)
        {
            try
            {
                _log.WriteEvent("Something cool", new {Make = "Chevy", Model = "Corvette", Name = name } );

                if(blah)
                    await _log.WriteError(new Exception("Data error"), Telemetry.LogSeverity.Warning, new { Class = "CoolClass", Function = "DoSomething" } )
            }
            catch(Exception ex)
            {
                // The anonymous object will be logged as two properties: "Class" and "Function"
                await _log.WriteError(ex, Telemetry.LogSeverity.Critical, new { Class = "CoolClass", Function = "DoSomething" } )
            }
        }
    }

<br>


### Logging with IRequestLog

By creating a scoped RequestLog you can set properties that will exist for every log within that scope. IRequestLog is derived from ILog so you just treat it as an ILog
 
    using MondoCore.Log;

    public class CoolClass
    {
        private readonly IRequestLog _log;

        public CoolClass(IRequestLog log)
        {
            _log = log;
        }

        public async Task DoSomething(string name)
        {
            // This property will be added to all subsequent log calls during the lifetime of IRequestLog
            _log.SetProperty("Name", name);

            try
            {
                // Do something...
                // ...

                _log.WriteEvent("Something cool", new {Make = "Chevy", Model = "Corvette"} );

                if(blah)
                    await _log.WriteError(new Exception("Data error"), Telemetry.LogSeverity.Warning, new { Class = "CoolClass", Function = "DoSomething" } )
            }
            catch(Exception ex)
            {
                // The anonymous object will be logged as two properties: "Class" and "Function"
                await _log.WriteError(ex, new { Class = "CoolClass", Function = "DoSomething" } )
            }
        }

        // Nested IRequestLog
        public async Task DoSomethingElse(string name)
        {
            using(var requestLog = log.NewRequest("CoolClass.DoSomethingElse))
            {
                // These properties will be added to all log calls within this using block (you must use the local log var)
                requestLog.SetProperty("Name", name);
                requestLog.SetProperty("Class", nameof(CoolClass));
                requestLog.SetProperty("Method", nameof(DoSomethingElse));

                try
                {
                    // Do something...
                    // ...

                    requestLog.WriteEvent("Something cool", new {Make = "Chevy", Model = "Corvette"} );

                    if(blah)
                        await requestLog.WriteError(new Exception("Data error"), Telemetry.LogSeverity.Warning)
                }
                catch(Exception ex)
                {
                    await requestLog.WriteError(ex)
                }
            }
        }
    }

<br>

License
----

MIT
