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

        [Command("criarpropriedade", GreedyArg = true)]
        public void CreatePropertyCommand(Client sender, int type, string address)
        {
            //if (sender.IsAdmin()){
            if(!Enum.IsDefined(typeof(Entities.Property.PropertyType), type))
            {
                API.sendChatMessageToPlayer(sender, "Este tipo é inválido!");
            }
            else
            {
                Entities.Property.Property prop = null;

                switch (type)
                {
                    case (int)Entities.Property.PropertyType.PROPERTY_TYPE_HOUSE:
                        prop = new Entities.Property.House();
                        prop.Type = Entities.Property.PropertyType.PROPERTY_TYPE_HOUSE;
                        prop.Address = address;
                        prop.X = sender.position.X;
                        prop.Y = sender.position.Y;
                        prop.Z = sender.position.Z;
                        break;

                    case (int)Entities.Property.PropertyType.PROPERTY_TYPE_BUSINESS:
                        prop = new Entities.Property.Business();
                        prop.Type = Entities.Property.PropertyType.PROPERTY_TYPE_BUSINESS;
                        prop.Address = address;
                        prop.X = sender.position.X;
                        prop.Y = sender.position.Y;
                        prop.Z = sender.position.Z;
                        break;

                    case (int)Entities.Property.PropertyType.PROPERTY_TYPE_ENTRANCE:
                        ///TODO
                        break;

                    case (int)Entities.Property.PropertyType.PROPERTY_TYPE_OFFICE:
                        //TODO
                        break;
                }

                Business.PropertyBLL bll = new Business.PropertyBLL();
                bll.Property_Create(prop);

                bll.SQL_FetchPropertyData();
            }
            //}
        }
    }
}
