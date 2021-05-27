using System.Threading.Tasks;
using SimpleTwitchBot.Bots;
using SimpleTwitchBot.Database;

namespace SimpleTwitchBot.Commands {

    public class AddAccountCommand : Command {

        public override string command => "add-account";

        public override async Task run(TwitchBot bot, TwitchBot.TwitchChatMessage message, string[] args) {
        }
    }

}