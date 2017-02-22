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
        private Business.PropertyBLL PropBLL = new Business.PropertyBLL();
        private Business.DoorBLL DoorBLL = new Business.DoorBLL();

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
            if (!Enum.IsDefined(typeof(Entities.Property.PropertyType), type))
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
                PropBLL.Property_Create(prop, sender.dimension);
                API.sendChatMessageToPlayer(sender, "Você criou uma propriedade com sucesso!");
            }
            //}
        }

        [Command("deletarpropriedade", GreedyArg = true)]
        public void DeletePropertyCommand(Client sender, int id)
        {
            //if (sender.IsAdmin()){
            if (id < 1)
            {
                API.sendChatMessageToPlayer(sender, "Não existem propriedades com ID menor que 1!");
            }
            else
            {
                Entities.Property.Property prop = PropBLL.FindPropertyById(id);
                
                if(prop == null)
                {
                    API.sendChatMessageToPlayer(sender, "Esta propriedade não existe!");
                }
                else
                {
                    PropBLL.Property_Delete(prop);
                    API.sendChatMessageToPlayer(sender, "Você deletou esta propriedade com sucesso!");
                }                             
            }
            //}
        }

        [Command("criarporta", GreedyArg = true)]
        public void CreateDoorCommand(Client sender, int propid, long model)
        {
            //if (sender.IsAdmin()){
            if(propid < 1)
            {
                API.sendChatMessageToPlayer(sender, "Não existem propriedades com ID menor que 1!");
            }
            //else if(model is invalid && != 0)
            //{
                //API.sendChatMessageToPlayer(sender, "Este modelo de porta é inválido!");
            //}
            else
            {
                Entities.Property.Property prop = PropBLL.FindPropertyById(propid);

                if (prop == null)
                {
                    API.sendChatMessageToPlayer(sender, "Esta propriedade não existe!");
                }
                else
                {
                    
                    DoorBLL.Door_Create(prop, model, true, new Vector3(sender.position.X, sender.position.Y, sender.position.Z), sender.dimension, new Vector3(-773.8976, 342.1525, 196.6863), prop.Id);
                    API.sendChatMessageToPlayer(sender, "Você criou uma porta com sucesso!");
                }
            }
            //}
        }
    }
}
