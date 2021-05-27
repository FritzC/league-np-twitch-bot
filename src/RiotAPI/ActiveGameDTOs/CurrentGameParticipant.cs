using System.Collections.Generic;

namespace RiotAPI.DTOs {

    public class CurrentGameParticipant {

        public long championId { get; set; }
        public Perks perks { get; set; }
        public long profileIconId { get; set; }
        public bool bot { get; set; }
        public long teamId { get; set; }
        public string summonerName { get; set; }
        public string summonerId { get; set; }
        public long spell1Id { get; set; }
        public long spell2Id { get; set; }
        public List<GameCustomizationObject> gameCustomizationObjects { get; set; }
        
    }
}