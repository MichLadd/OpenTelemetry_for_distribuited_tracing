using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace OTelUseCase.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly Tracer _tracer;

        public IndexModel(ILogger<IndexModel> logger, TracerProvider provider)
        {
            _logger = logger;
            _tracer = provider.GetTracer(TelemetryConstants.MyAppTraceSource);
            _logger.LogInformation("Log: IndexModel");
        }

        public void OnGet()
        {
            _logger.LogInformation("Log: Index OnGet");

            var tags = new TagList();
            tags.Add("user-agent", Request.Headers.UserAgent);
            TelemetryConstants.HitsCounter.Add(1, tags);
            using var mySpan = _tracer.StartActiveSpan("MyOp").SetAttribute("httpTracer", HttpContext.TraceIdentifier);
            mySpan.AddEvent($"Received HTTP request from {Request.Headers.UserAgent}");
        }
    }
}