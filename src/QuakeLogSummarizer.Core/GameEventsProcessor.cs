using System.Collections.Generic;
using System.Linq;
using QuakeLogSummarizer.Core.GameEvents;
using QuakeLogSummarizer.Core.Model;

namespace QuakeLogSummarizer.Core
{
    public sealed class GameEventsProcessor
    {
        private int _gameIndex = 0;

        public IList<Game> GameList { get; }

        public GameEventsProcessor()
        {
            this.GameList = new List<Game>();
        }

        public void Process(IGameEvent gameEvent)
        {
            switch(gameEvent)
            {
                case InitGameEvent:
                    this.GameList.Add(new Game()
                    {
                        Index = ++_gameIndex
                    });
                    break;

                case KillEvent:
                    this.GameList.Last().KillCount++;
                    break;
            }
        }
    }
}
