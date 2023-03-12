
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OTelUseCase;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
// Creating the logger factory to create the logger used in the application
using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddOpenTelemetry(options =>
    {
        // Creating a console exporter for the LoggerProvider
        options.AddConsoleExporter();
    });
});

var logger = loggerFactory.CreateLogger<Program>();

// shared Resource to use for both OTel metrics AND tracing
var resource = ResourceBuilder.CreateDefault().AddService("OTelUseCase");

// Add services to the container.
builder.Services.AddRazorPages();

builder.Configuration.AddEnvironmentVariables();
// Add tracing auto instrumentation
builder.Services.AddOpenTelemetryTracing(b =>
{
    Environment.GetEnvironmentVariable("JAEGER_SERVICE_NAME");
    // uses the default Jaeger settings
    //b.AddJaegerExporter();
    // get Jaeger configuration parameters from environment variables
    b.AddJaegerExporter(o =>
     {
         o.AgentHost = builder.Configuration.GetValue<string>("Jaeger:Host");
         o.AgentPort = builder.Configuration.GetValue<int>("Jaeger:Port");
     });
    // receive traces from our own custom sources
    b.AddSource(TelemetryConstants.MyAppTraceSource);

    // decorate our service name so we can find it when we look inside Jaeger
    b.SetResourceBuilder(ResourceBuilder.CreateDefault()
        .AddService("OTelUseCase"));

    // receive traces from built-in sources
    b.AddHttpClientInstrumentation();
    b.AddAspNetCoreInstrumentation();
});


// Add metrics auto instrumentation
builder.Services.AddOpenTelemetryMetrics(b =>
{
    // add prometheus exporter if needed
    //b.AddPrometheusExporter();

    // receive metrics from our own custom sources
    b.AddMeter(TelemetryConstants.MyAppTraceSource);

    // decorate our service name so we can find it when we look inside Prometheus
    b.SetResourceBuilder(resource);

    // receive metrics from built-in sources
    b.AddHttpClientInstrumentation();
    b.AddAspNetCoreInstrumentation();
});

var app = builder.Build();

// dynamic page for logger testing
app.MapGet("/Testing", async context =>
{
    logger.LogInformation("Testing logging in Program.cs");
    await context.Response.WriteAsync("Testing, check log info.");
});

logger.LogInformation($"Welcome to {Assembly.GetEntryAssembly().GetName().Name}.");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

logger.LogInformation("Starting the app");
app.Run();
