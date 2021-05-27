using System.Collections.Generic;

namespace RiotAPI.DTOs {

    public class LeagueItemDTO {

        public bool freshBlood { get; set; }
        public int wins { get; set; }
        public string summonerName { get; set; }
        public MiniSeriesDTO miniSeries { get; set; }
        public bool inactive { get; set; }
        public bool veteran { get; set; }
        public bool hotStreak { get; set; }
        public string rank { get; set; }
        public int leaguePoints { get; set; }
        public int losses { get; set; }
        public string summonerId { get; set; } // Encrypted summoner Id
        
    }

}