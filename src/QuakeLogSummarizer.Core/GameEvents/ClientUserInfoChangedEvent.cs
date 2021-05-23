namespace QuakeLogSummarizer.Core.GameEvents
{
    public sealed class ClientUserInfoChangedEvent : IGameEvent
    {
        public int PlayerId { get; init; }

        public string PlayerName { get; set; }
    }
}
