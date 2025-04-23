namespace Shared.EventBridge.Enums
{
    public enum LogStatus
    {
        Success,    // Indicates successful processing
        Failure,    // Represents a processing failure
        Retry       // Processing retried due to an issue
    }
}