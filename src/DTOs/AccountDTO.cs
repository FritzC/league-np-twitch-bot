using System.Collections.Generic;

namespace SimpleTwitchBot.DTOs {

    public class AccountDTO {

        public int id { get; set; }
        public string encryptedAccountId { get; set; }
        public int regions_id { get; set; }
        public int notable_players_id { get; set; }
        
    }
}