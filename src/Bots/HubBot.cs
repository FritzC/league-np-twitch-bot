using SimpleTwitchBot.Commands;

namespace SimpleTwitchBot.Bots {

    public class HubBot : TwitchBotWrapper {

        public HubBot() : base(GlobalConstants.BOT_NAME) {
            commands.Add(new AddChannelCommand());
            commands.Add(new RemoveChannelCommand());
            commands.Add(new AddNotablePlayerCommand());
            commands.Add(new RemoveNotablePlayerCommand());
        }

    }

}