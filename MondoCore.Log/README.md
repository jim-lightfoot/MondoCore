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
                log.Register( new ApplicationInsights( p.GetService<TelemetryConfiguration>() ) );

                return log;
            );

            // Register IRequestLog as scoped. That same instance will be returned within the scope of a function invocation
            builder.Services.AddScoped<IRequestLog, RequestLog>();
        }
    }

<br>

### Logging with IRequestLog

By creating a scoped RequestLog you can set properties that will exist for every log within that scope
 
    using MondoCore.Log;

    public class CoolClass
    {
        public async Task DoSomething(string name, IRequestLog log)
        {
            // This property will be added to all subsequent log calls during the lifetime of IRequestLog
            log.SetProperty("Name", name);

            try
            {
                ...
            }
            catch(Exception ex)
            {
                // The anonymous object will be logged as two properties: "Class" and "Function"
                await log.WriteError(ex, new { Class = "CoolClass", Function = "DoSomething" } )
            }
        }
    }

<br>

License
----

MIT
