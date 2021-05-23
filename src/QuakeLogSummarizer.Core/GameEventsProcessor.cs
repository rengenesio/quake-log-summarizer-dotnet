using System.Collections.Generic;
using System.Linq;
using QuakeLogSummarizer.Core.GameEvents;
using QuakeLogSummarizer.Core.Model;

namespace QuakeLogSummarizer.Core
{
    public sealed class GameEventsProcessor
    {
        /// <summary>
        /// Player identifier assigned to the 'world' player. Extracted from 'ENTITYNUM_WORLD' constant from Quake III source code.
        /// </summary>
        /// <remarks>
        /// Extracted from original source code:
        /// #define	GENTITYNUM_BITS		10
        /// #define MAX_GENTITIES		(1<<GENTITYNUM_BITS)
        /// #define	ENTITYNUM_WORLD		(MAX_GENTITIES-2)
        /// </remarks>
        private const int WorldPlayerId = 1022;
    
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
                        Index = ++(this._gameIndex)
                    });
                    break;

                case ClientConnectEvent clientConnectEvent:
                    this.GameList.Last().PlayerMap.TryAdd(clientConnectEvent.PlayerId, new PlayerData());
                    break;

                case ClientUserInfoChangedEvent clientUserInfoChangedEvent:
                    this.GameList.Last().PlayerMap[clientUserInfoChangedEvent.PlayerId].PlayerNameList.Add(clientUserInfoChangedEvent.PlayerName);
                    break;

                case KillEvent killEvent:
                    this.GameList.Last().KillCount++;

                    // Suicides doesn't change players' score.
                    if (killEvent.KillerId == killEvent.VictimId)
                    {
                        return;
                    }

                    int scoreIncrement = 1;
                    int playerId = killEvent.KillerId;
                    if (killEvent.KillerId == WorldPlayerId)
                    {
                        playerId = killEvent.VictimId;
                        scoreIncrement *= -1;
                    }

                    this.GameList.Last().PlayerMap[playerId].Score += scoreIncrement;
                    break;
            }
        }
    }
}
