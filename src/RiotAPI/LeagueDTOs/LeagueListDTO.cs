using System.Collections.Generic;

namespace RiotAPI.DTOs {

    public class LeagueListDTO {

        public string leagueId { get; set; }
        public List<LeagueItemDTO> entries { get; set; }
        public string tier { get; set; }
        public string queue { get; set; }
        
    }
}