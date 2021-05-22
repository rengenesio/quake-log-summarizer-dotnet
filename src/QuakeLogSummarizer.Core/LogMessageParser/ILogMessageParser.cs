using QuakeLogSummarizer.Core.GameEvents;

namespace QuakeLogSummarizer.Core.LogMessageParser
{
    public interface ILogMessageParser
    {
        IGameEvent Parse(string logMessage);
    }
}