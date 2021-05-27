using System;
using System.Threading.Tasks;
using RiotAPI;
using RiotAPI.DTOs;
using SimpleTwitchBot.Bots;
using SimpleTwitchBot.Database;

namespace SimpleTwitchBot.Commands {

    public class AddNotablePlayerCommand : Command {

        public override string command => "add-np";

        public override async Task run(TwitchBot bot, TwitchBot.TwitchChatMessage message, string[] args) {
            if (args.Length != 3) {
                await bot.SendMessage("Invalid arguments - Usage: '!add-np [region] [summoner]'");
                return;
            }

            try {
                SummonerDTO summoner = await RiotAPIHandler.lookupAccountBySummonerId(args[1], args[2]);
                // TODO: Attempt to write to DB (make sure to include region)

            } catch (Exception e) {
                
            }
        }
    }

}