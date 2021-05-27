
using System.Collections.Generic;

namespace RiotAPI.DTOs {

    public class CurrentGameInfo {

        public long gameId { get; set; }
        public string gameType { get; set; }
        public long gameStartTime { get; set; }
        public long mapId { get; set; }
        public long gameLength { get; set; }
        public string platformId { get; set; }
        public string gameMode { get; set; }
        public List<BannedChampion> bannedChampions { get; set; }
        public long gameQueueConfigId { get; set; }
        public List<CurrentGameParticipant> participants { get; set; }
        
    }
}