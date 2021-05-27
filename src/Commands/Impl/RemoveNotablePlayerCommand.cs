using System.Threading.Tasks;
using SimpleTwitchBot.Bots;
using SimpleTwitchBot.Database;

namespace SimpleTwitchBot.Commands {

    public class RemoveNotablePlayerCommand : Command {

        public override string command => "remove-np";

        public override async Task run(TwitchBot bot, TwitchBot.TwitchChatMessage message, string[] args) {
            // TODO: Impl
        }
    }

}