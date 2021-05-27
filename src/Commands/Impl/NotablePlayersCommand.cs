using System.Text;
using System.Threading.Tasks;
using RiotAPI;
using RiotAPI.DTOs;
using SimpleTwitchBot.Bots;
using SimpleTwitchBot.DTOs;

namespace SimpleTwitchBot.Commands {

    public class NotablePlayersCommand : Command {

        public override string command => "np";

        public override async Task run(TwitchBot bot, TwitchBot.TwitchChatMessage message, string[] args) {
            UserChannelBot userChannelBot = (UserChannelBot) bot;
            if (userChannelBot.accounts.Count == 0) {
                await bot.SendMessage("You need to add accounts using '!add-account [summoner]' first!");
                return;
            }

            bool found = false;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < userChannelBot.accounts.Count; i++) {
                AccountDTO account = userChannelBot.accounts[i];
                // TODO: Load region table and cache it then pass into this
                CurrentGameInfo currentGameInfo = await RiotAPIHandler.lookupGame("NA1", account.encryptedAccountId);

                if (currentGameInfo != null) {
                    found = true;
                    if (i == 0) {
                        // TODO: If it's our active account, check the cache to see if this game already has been found
                    }
                    
                    // TODO: Find NPs from the participants of the game then build the message
                    sb.Append(currentGameInfo.gameType);
                    sb.Append(": ");


                    // Move active user to front of list
                    userChannelBot.accounts.Remove(account);
                    userChannelBot.accounts.Add(account);
                    break;
                }
            }

            if (!found) {
                await bot.SendMessage($"{bot.channel} is not in in a game yet!");
            }
        }

        private string getAverageRank(CurrentGameInfo info) {
            return "";
        }

    }

    
        /**
         * Steps:
         *  - Streamer (or mod) inputs summoner name
         *  - Make Riot API request to get the accountId for that summoner Id
         *  - Add to list of streamer accounts
         *  - Add to database 
         *  - Send chat message verifying account was added
         */
        // public async Task registerAccount() {

        // }

        /**
         * Steps:
         *  - Search through registers accounts for streamer to find which is ingame
         *  - When an account is found to be ingame, move it to top of the account list and mark as active
         *  - Make Riot API request to get streamer's current match data
         *  - Search database for "notable players" from the account IDs found in the match data
         *  - Make follow up API requests to get found "notable player"s' current ranks (probably need to cache this)
         *  - Format message to display in chat
         *  - Send chat message
         */
        // public static async Task lookupGame(TwitchBot bot, TwitchChatMessage message) {
        //     string puid = "NrQYPHZPuN-VvqVUPGVgUOc7WvOM7MEEv99NNvRsODgL9jnoZiWDoyGU4H5jXyhMi4e7pTRQlc-WRw";
        //     string encryptedSumId = "J-MsmgZZTxs5y2uPbjiZRpCJa6GLWLDBrv8J7AHdldjO6Kxl";
        //     string matchRequestAPI = "https://na1.api.riotgames.com/lol/spectator/v4/active-games/by-summoner/{0}";
        //     string apiKey = "RGAPI-acc4641f-6313-4016-86fc-11c45e023991";

        //     var client = new RestClient(string.Format(matchRequestAPI, encryptedSumId)); 
        //     client.AddDefaultHeader("X-Riot-Token", apiKey);
        //     IRestResponse response = await client.ExecuteAsync(new RestRequest()); 
        //     CurrentGameInfo currentGame = JsonSerializer.Deserialize<CurrentGameInfo>(response.Content);

        //     StringBuilder sb = new StringBuilder();
        //     sb.Append("Sum ids: ");
        //     foreach (CurrentGameParticipant participant in currentGame.participants) {
        //         sb.Append($"{participant.summonerName}, ");
        //     }
        //     await bot.SendMessage(message.Channel, sb.ToString());

        // }

}