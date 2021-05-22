namespace QuakeLogSummarizer.Core.GameEvents
{
    public sealed class KillEvent : IGameEvent
    {
        public int KillerId { get; set; }

        public int VictimId { get; set; }
    }
}
