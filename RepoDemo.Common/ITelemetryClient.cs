namespace RepoDemo.Common
{
    public interface ITelemetryClient
    {
        void TrackException(Exception exception);
        void TrackEvent(string eventName);
        void TrackTrace(string message);
        // Add other methods as needed
    }
}
