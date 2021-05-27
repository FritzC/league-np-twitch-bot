using System;
using System.Threading.Tasks;
using SimpleTwitchBot.Bots;
using SimpleTwitchBot.Database;
using SimpleTwitchBot.DTOs;

namespace SimpleTwitchBot.Commands {

    public class AddChannelCommand : Command {

        public override string command => "add";

        public override async Task run(TwitchBot bot, TwitchBot.TwitchChatMessage message, string[] args) {
            try {
                TwitchChannelDTO dto = await Program.databaseManager.addTwitchAccount(bot, message.Sender);
                await bot.SendMessage($"Bot has been added to {message.Sender}'s channel!");
                Program.startUserChannelBot(dto);
            } catch (Exception e) {
                await bot.SendMessage($"Error adding bot to {message.Sender}'s channel. Has it has already been added?");
            }
        }
    }

}