using System.Threading.Tasks;
using SimpleTwitchBot.Bots;
using static SimpleTwitchBot.Bots.TwitchBot;

namespace SimpleTwitchBot.Commands {

    public abstract class Command {

        public const string PREFIX = "!";

        public abstract string command { get; }

        public bool matches(string message) {
            return message.ToLower().Split(' ')[0].Equals(command.ToLower());
        }

        public virtual Task run(TwitchBot bot, TwitchChatMessage message, params string[] args) {
            return Task.FromResult(default(object));
        }

    }

}