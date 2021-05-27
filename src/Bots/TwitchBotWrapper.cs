using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncAwaitBestPractices;
using SimpleTwitchBot.Commands;

namespace SimpleTwitchBot.Bots {

    public abstract class TwitchBotWrapper : TwitchBot {

        protected List<Command> commands;

        public TwitchBotWrapper(string channel) : base(GlobalConstants.BOT_NAME, GlobalConstants.BOT_OAUTH, channel) {
            this.commands = new List<Command>();
        }

        public async Task start() {
            connectAndBeginListening().SafeFireAndForget();
            //We could .SafeFireAndForget() these two calls if we want to
            await JoinChannel(channel);
            await SendMessage("#Joined channel");

            OnMessage += async (sender, twitchChatMessage) => {
                System.Console.WriteLine(twitchChatMessage?.Message);
                // Check if a command is entered first
                if (!twitchChatMessage.Message.StartsWith(Command.PREFIX)) {
                    return;
                }
                // Chop off the prefix
                twitchChatMessage.Message = twitchChatMessage.Message.Substring(Command.PREFIX.Length);

                // Try to run the command
                await findAndRunCommand(twitchChatMessage);
            };
            
            await Task.Delay(-1);
        }

        private async Task findAndRunCommand(TwitchChatMessage message) {
            foreach (Command command in commands) {
                if (command.matches(message.Message)) {
                    await command.run(this, message, message.Message.Split(' '));
                    break;
                }
            }
        }
    }
}