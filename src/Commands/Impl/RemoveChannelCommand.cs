using System.Net.Mail;
using System.Threading.Tasks;
using SimpleTwitchBot.Bots;
using SimpleTwitchBot.Database;

namespace SimpleTwitchBot.Commands {

    public class RemoveChannelCommand : Command {

        public override string command => "remove";

        public override async Task run(TwitchBot bot, TwitchBot.TwitchChatMessage message, string[] args) {
            if (await Program.databaseManager.removeTwitchAccount(bot, message.Sender)) {
                await bot.SendMessage($"Bot has been removed from {message.Sender}'s channel!");
                Program.removeUserChannelBot(message.Sender);
            } else {
                await bot.SendMessage($"Error removing bot from {message.Sender}'s channel!");
            }
        }
    }

}