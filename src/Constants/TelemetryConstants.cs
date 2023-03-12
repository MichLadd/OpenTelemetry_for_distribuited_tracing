using System.Diagnostics.Metrics;

namespace OTelUseCase
{    
    /// <summary>
     /// The name of the <see cref="ActivitySource"/> that is going to produce our traces.
     /// </summary>
    public static class TelemetryConstants
    {
        public const string MyAppTraceSource = "OTelUseCase";

        public static readonly Meter DemoMeter = new Meter(MyAppTraceSource);

        public static readonly Counter<long> HitsCounter =
            DemoMeter.CreateCounter<long>("IndexHits", "hits", "number of hits to homepage");
    }
}
