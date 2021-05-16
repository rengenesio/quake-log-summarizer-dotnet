namespace QuakeLogSummarizer.Core
{
    public interface ILogMessageExtractor
    {
        string Extract(string logRecord);
    }
}