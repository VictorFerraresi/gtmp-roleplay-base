using System;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace ProjetoRP.Modules.Admin
{    
    class DiscordBot
    {
        public DiscordClient _client = new DiscordClient();

        public DiscordBot()
        {
            Initialize();
        }

        private async void Initialize()
        {
            _client.MessageReceived += (s, e) =>
            {
                if (!e.Message.IsAuthor)
                    if (e.Channel.Id == 280034280908783616)
                        SendAdminChatMessage(e.Message.User.Name, e.Message.Text);
            };

            await _client.Connect("Mjc5OTc1NzYwNzY3MjIxNzcw.C4ld0w.8LqgRGZx4Z2EGL9mhcqq0jyOzhI", TokenType.Bot);
        }

        public async void SendAdminChatMessageToDiscord(string name, string text)
        {            
            await _client.GetChannel(280034280908783616).SendMessage("**" + name + "**: `" + text + "`");
        }

        public void SendAdminChatMessage(string name, string text)
        {
            List<Client> players = API.shared.getAllPlayers();
            foreach (Client c in players)
            {
                API.shared.sendChatMessageToPlayer(c, Colors.COLOR_ADMINCHAT, "@ " + name + ": " + text);
            }
        }
    }    
}
