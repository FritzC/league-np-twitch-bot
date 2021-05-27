using System.Collections.Generic;

namespace RiotAPI.DTOs {

    public class SummonerDTO {

        public string accountId { get; set; } // Encrypted ID
        public int profileIconId { get; set; }
        public long revionDate { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public string puuid { get; set; }
        public long summonerLevel { get; set; }
    }

}