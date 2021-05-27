using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleTwitchBot.Bots;
using SimpleTwitchBot.Database;
using SimpleTwitchBot.DTOs;
using AsyncAwaitBestPractices;

namespace SimpleTwitchBot {

    public static class Program {

        public static Dictionary<string, UserChannelBot> runningBots = new Dictionary<string, UserChannelBot>();
        public static DatabaseManager databaseManager;

        
        public static async Task Main(string[] args) {
            databaseManager = new DatabaseManager();

            // Start the main bot on the bot's channel
            HubBot hubBot = new HubBot();
            hubBot.start().SafeFireAndForget();

            // Load twitch accounts from the DB and start channel bots for each of them
            List<TwitchChannelDTO> twitchAccounts = await databaseManager.getAllTwitchAccounts();
            foreach (TwitchChannelDTO account in twitchAccounts) {
                Logger.log("Starting channel for : " + account.channel_name);
                startUserChannelBot(account);
            }

            await Task.Delay(-1);
        }

        public static void startUserChannelBot(TwitchChannelDTO account) {
            if (runningBots.ContainsKey(account.name.ToLower())) {
                return;
            }
            UserChannelBot newBot = new UserChannelBot(account);
            runningBots[account.name.ToLower()] = newBot;
            newBot.start().SafeFireAndForget();
        }

        public static void removeUserChannelBot(string channel) {
            channel = channel.ToLower();

            if (runningBots.ContainsKey(channel)) {
                runningBots[channel].stop();
                runningBots.Remove(channel);
                Logger.log($"Successfully removed bot from channel {channel}");
            } else {
                Logger.log($"Failed to remove bot from channel {channel} as is not running there!");
            }
        }

    }
}