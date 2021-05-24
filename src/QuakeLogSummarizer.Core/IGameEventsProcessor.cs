using System.Collections.Generic;
using QuakeLogSummarizer.Core.GameEvents;
using QuakeLogSummarizer.Core.Model;

namespace QuakeLogSummarizer.Core
{
    public interface IGameEventsProcessor
    {
        IList<Game> GameList { get; }

        void Process(IGameEvent gameEvent);
    }
}