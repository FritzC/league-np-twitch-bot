using System.Collections.Generic;
using SimpleTwitchBot.Commands;
using SimpleTwitchBot.DTOs;

namespace SimpleTwitchBot.Bots {

    public class UserChannelBot : TwitchBotWrapper {

        public TwitchChannelDTO twitchAccount;
        public List<AccountDTO> accounts;

        public UserChannelBot(TwitchChannelDTO twitchAccount) : base(twitchAccount.name) {
            this.twitchAccount = twitchAccount;
            this.accounts = new List<AccountDTO>();
            // TODO: Load account IDs from DB here
            
            commands.Add(new NotablePlayersCommand());
        }

    }

}