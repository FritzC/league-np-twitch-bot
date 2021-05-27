
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SimpleTwitchBot.Bots {

    public class TwitchBot {

        protected const string ip = "irc.chat.twitch.tv";
        protected const int port = 6667;

        protected string nick;
        protected string password;
        public string channel;

        private StreamReader streamReader;
        private StreamWriter streamWriter;
        private TaskCompletionSource<int> connected = new TaskCompletionSource<int>();

        public event TwitchChatEventHandler OnMessage = delegate { };
        public delegate void TwitchChatEventHandler(object sender, TwitchChatMessage e);
        public bool running;
        public bool finished;

        public class TwitchChatMessage : EventArgs {
            public string Sender { get; set; }
            public string Message { get; set; }
            public string Channel { get; set; }
        }

        public TwitchBot(string nick, string password, string channel) {
            this.nick = nick;
            this.password = password;
            this.channel = channel;

            running = true;
        }

        protected async Task connectAndBeginListening() {
            var tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(ip, port);
            streamReader = new StreamReader(tcpClient.GetStream());
            streamWriter = new StreamWriter(tcpClient.GetStream()) { NewLine = "\r\n", AutoFlush = true};

            await streamWriter.WriteLineAsync($"PASS {password}");
            await streamWriter.WriteLineAsync($"NICK {nick}");
            connected.SetResult(0);

            while (running) {
                string line = await streamReader.ReadLineAsync();
                Console.WriteLine(line);
                
                string[] split = line.Split(" ");
                //PING :tmi.twitch.tv
                //Respond with PONG :tmi.twitch.tv
                if (line.StartsWith("PING")) {
                    Console.WriteLine("PONG");
                    await streamWriter.WriteLineAsync($"PONG {split[1]}");
                }

                if (split.Length > 2 && split[1] == "PRIVMSG") {
                    //:mytwitchchannel!mytwitchchannel@mytwitchchannel.tmi.twitch.tv 
                    // ^^^^^^^^
                    //Grab this name here
                    int exclamationPointPosition = split[0].IndexOf("!");
                    string username = split[0].Substring(1, exclamationPointPosition - 1);
                    //Skip the first character, the first colon, then find the next colon
                    int secondColonPosition = line.IndexOf(':', 1);//the 1 here is what skips the first character
                    string message = line.Substring(secondColonPosition + 1);//Everything past the second colon
                    string channel = split[2].TrimStart('#');
                    
                    OnMessage(this, new TwitchChatMessage {
                        Message = message,
                        Sender = username,
                        Channel = channel
                    });
                }
            }

            await SendMessage("#Left channel");

            streamReader.Close();
            streamWriter.Close();
            tcpClient.Close();
            finished = true;
        }

        public void stop() {
            running = false;
        }

        public async Task SendMessage(string message) {
            await connected.Task;
            await streamWriter.WriteLineAsync($"PRIVMSG #{channel} :{message}");
        }
        
        public async Task JoinChannel(string channel) {
            await connected.Task;
            await streamWriter.WriteLineAsync($"JOIN #{channel}");
        }
    }
}