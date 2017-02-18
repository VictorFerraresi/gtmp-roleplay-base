using System;
using GTANetworkServer;
using GTANetworkShared;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Modules.Admin
{
    class Admin : Script
    {
        private DiscordBot _discordBot = new DiscordBot();        

        public Admin()
        {
            API.onResourceStart += OnResourceStart;
            API.onResourceStop += OnResourceStop;
            API.onClientEventTrigger += OnClientEventTrigger;
        }

        public void OnResourceStart()
        {
            API.consoleOutput(Messages.console_startup);
        }
        public void OnResourceStop()
        {

        }

        public void OnClientEventTrigger(Client player, string eventName, object[] args)
        {

        }

        public void SendAdminChatMessage(string name, string text)
        {
            List<Client> players = API.getAllPlayers();
            foreach (Client c in players)
            {
                API.sendChatMessageToPlayer(c, Colors.COLOR_ADMINCHAT, "@ " + name + ": " + text);
            }
        }

        // Commands        
        [Command("a", GreedyArg = true)]
        public void AdminChatCommand(Client sender, string text)
        {
            Entities.Character character = sender.getData("CHARACTER_DATA");
            //if sender.IsAdmin(){
                SendAdminChatMessage(character.Name, text);                
                _discordBot.SendAdminChatMessageToDiscord(character.Name, text);                
            //}
        }
    }
}
