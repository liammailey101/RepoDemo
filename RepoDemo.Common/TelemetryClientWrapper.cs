using Microsoft.ApplicationInsights;

namespace RepoDemo.Common
{
    public class TelemetryClientWrapper : ITelemetryClient
    {
        private readonly TelemetryClient _telemetryClient;

        public TelemetryClientWrapper(TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient;
        }

        public void TrackException(Exception exception)
        {
            _telemetryClient.TrackException(exception);
        }

        public void TrackEvent(string eventName)
        {
            _telemetryClient.TrackEvent(eventName);
        }

        public void TrackTrace(string message)
        {
            _telemetryClient.TrackTrace(message);
        }

        // Implement other methods as needed
    }
}
