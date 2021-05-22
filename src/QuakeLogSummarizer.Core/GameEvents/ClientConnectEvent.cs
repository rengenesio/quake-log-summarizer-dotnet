namespace QuakeLogSummarizer.Core.GameEvents
{
    public sealed class ClientConnectEvent : IGameEvent
    {
        public int PlayerId { get; init; }
    }
}
