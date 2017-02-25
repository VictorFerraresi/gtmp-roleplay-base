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
        public void CreatePropertyCommand(Client sender, int type, int price, string address)
        {
            //if (sender.IsAdmin()){
            if (!Enum.IsDefined(typeof(Entities.Property.PropertyType), type))
            {
                API.sendChatMessageToPlayer(sender, "Este tipo é inválido!");
            }
            else if(price < 1)
            {
                API.sendChatMessageToPlayer(sender, "Escolha um preço maior do que 0!");
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
                        prop.Price = price;
                        break;

                    case (int)Entities.Property.PropertyType.PROPERTY_TYPE_BUSINESS:
                        prop = new Entities.Property.Business();
                        prop.Type = Entities.Property.PropertyType.PROPERTY_TYPE_BUSINESS;
                        prop.Address = address;
                        prop.X = sender.position.X;
                        prop.Y = sender.position.Y;
                        prop.Z = sender.position.Z;
                        prop.Price = price;
                        break;

                    case (int)Entities.Property.PropertyType.PROPERTY_TYPE_ENTRANCE:
                        ///TODO                        
                        break;

                    case (int)Entities.Property.PropertyType.PROPERTY_TYPE_ENTRANCE:
                        //TODO
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

        [Command("deletarpropriedade")]
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

                if (prop == null)
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

        [Command("criarporta")]
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
                    DoorBLL.Door_Create(prop, model, true, new Vector3(sender.position.X, sender.position.Y, sender.position.Z), sender.dimension, new Vector3(-18.77586, -581.755, 90.11491), prop.Id);
                    API.sendChatMessageToPlayer(sender, "Você criou uma porta com sucesso!");
                }
            }
            //}
        }

        [Command("deletarporta")]
        public void DeleteDoorCommand(Client sender, int doorid)
        {
            //if (sender.IsAdmin()){
            if (doorid < 1)
            {
                API.sendChatMessageToPlayer(sender, "Não existem portas com ID menor que 1!");
            }
            else
            {
                Entities.Property.Door door = DoorBLL.FindDoorById(doorid);

                if (door == null)
                {
                    API.sendChatMessageToPlayer(sender, "Esta porta não existe!");
                }
                else
                {
                    DoorBLL.Door_Delete(door);
                    API.sendChatMessageToPlayer(sender, "Você deletou esta porta com sucesso!");
                }
            }
            //}
        }
    }
}