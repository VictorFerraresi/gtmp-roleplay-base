﻿using System;
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

        [Command("editarpropriedade", GreedyArg = true)]
        public void EditPropertyCommand(Client sender, int id, string option, string value = "default")
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
                    switch (option)
                    {
                        case "pos":
                            prop.X = sender.position.X;
                            prop.Y = sender.position.Y;
                            prop.Z = sender.position.Z;
                            prop.Dimension = sender.dimension;

                            PropBLL.Property_Save(prop);
                            PropBLL.RedrawPickup(prop);                            

                            API.sendChatMessageToPlayer(sender, "Você alterou a posição da propriedade ID " + id + " para a sua localização!");
                            break;

                        case "endereco":
                            if(value.Equals("default"))
                            {
                                API.sendChatMessageToPlayer(sender, "Escolha um endereço para a propriedade!");
                                API.sendChatMessageToPlayer(sender, "~y~[EXEMPLO] ~w~/editarpropriedade " + id + " endereco Rua Lorem Ipsum, 340!");
                            }
                            else
                            {
                                prop.Address = value;

                                PropBLL.Property_Save(prop);
                                PropBLL.RedrawPickup(prop);

                                API.sendChatMessageToPlayer(sender, "Você alterou o endereço da propriedade ID " + id + " para " + value);
                            }                            
                            break;

                        case "preco":
                            if (value.Equals("default"))
                            {
                                API.sendChatMessageToPlayer(sender, "Escolha um preço para a propriedade!");
                                API.sendChatMessageToPlayer(sender, "~y~[EXEMPLO] ~w~/editarpropriedade " + id + " preco 25000");
                            }
                            else
                            {
                                int price = 0;

                                if (Int32.TryParse(value, out price))
                                {
                                    if(price < 1)
                                    {
                                        API.sendChatMessageToPlayer(sender, "Escolha um preço maior do que 0!");
                                        API.sendChatMessageToPlayer(sender, "~y~[EXEMPLO] ~w~/editarpropriedade " + id + " preco 25000");
                                    }
                                    else
                                    {
                                        prop.Price = price;

                                        PropBLL.Property_Save(prop);
                                        PropBLL.RedrawPickup(prop);

                                        API.sendChatMessageToPlayer(sender, "Você alterou o preço da propriedade ID " + id + " para $" + price.ToString("N0"));
                                    }                                    
                                }
                                else
                                {
                                    API.sendChatMessageToPlayer(sender, "Digite apenas números no valor!");
                                    API.sendChatMessageToPlayer(sender, "~y~[EXEMPLO] ~w~/editarpropriedade " + id + " preco 25000");
                                }                    
                            }
                            break;

                        default:
                            API.sendChatMessageToPlayer(sender, "Escolha uma ação válida!");
                            API.sendChatMessageToPlayer(sender, "~y~[AÇÕES] ~w~pos, endereco, preco.");
                            break;                        
                    }
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

        [Command("editarporta", GreedyArg = true)]
        public void EditDoorCommand(Client sender, int id, string option, string value = "default")
        {
            //if (sender.IsAdmin()){
            if (id < 1)
            {
                API.sendChatMessageToPlayer(sender, "Não existem portas com ID menor que 1!");
            }
            else
            {
                Entities.Property.Door door = DoorBLL.FindDoorById(id);
                
                if (door == null)
                {
                    API.sendChatMessageToPlayer(sender, "Esta porta não existe!");
                }
                else
                {
                    switch (option)
                    {
                        case "pos":
                            door.ExteriorX = sender.position.X;
                            door.ExteriorY = sender.position.Y;
                            door.ExteriorZ = sender.position.Z;
                            door.ExteriorDimension = sender.dimension;

                            DoorBLL.Door_Save(door);                            

                            API.sendChatMessageToPlayer(sender, "Você alterou a posição da porta ID " + id + " para a sua localização!");
                            break;

                        case "interior":
                            door.InteriorX = sender.position.X;
                            door.InteriorY = sender.position.Y;
                            door.InteriorZ = sender.position.Z;
                            door.InteriorDimension = sender.dimension;

                            DoorBLL.Door_Save(door);

                            API.sendChatMessageToPlayer(sender, "Você alterou o interior da porta ID " + id + " para a sua localização!");
                            break;

                        case "propriedade":
                            if (value.Equals("default"))
                            {
                                API.sendChatMessageToPlayer(sender, "Escolha um ID de propriedade!");
                                API.sendChatMessageToPlayer(sender, "~y~[EXEMPLO] ~w~/editarporta " + id + " propriedade 245");
                            }
                            else
                            {
                                int propid = 0;

                                if (Int32.TryParse(value, out propid))
                                {
                                    if (propid < 1)
                                    {
                                        API.sendChatMessageToPlayer(sender, "Escolha um ID de propriedade maior do que 0!");
                                        API.sendChatMessageToPlayer(sender, "~y~[EXEMPLO] ~w~/editarporta " + id + " propriedade 245");
                                    }
                                    else
                                    {
                                        Entities.Property.Property prop = PropBLL.FindPropertyById(propid);

                                        if(prop == null)
                                        {
                                            API.sendChatMessageToPlayer(sender, "Esta propriedade não existe!");
                                        }
                                        else
                                        {
                                            door.Property = prop;
                                            door.Property_Id = prop.Id;

                                            DoorBLL.Door_Save(door);                                            

                                            API.sendChatMessageToPlayer(sender, "Você alterou a propriedade da porta ID " + id + " para " + prop.Id);
                                        }                                        
                                    }
                                }
                                else
                                {
                                    API.sendChatMessageToPlayer(sender, "Digite apenas números no ID da propriedade!");
                                    API.sendChatMessageToPlayer(sender, "~y~[EXEMPLO] ~w~/editarporta " + id + " propriedade 245");
                                }
                            }
                            break;

                        case "modelo":
                            if (value.Equals("default"))
                            {
                                API.sendChatMessageToPlayer(sender, "Escolha um modelo!");
                                API.sendChatMessageToPlayer(sender, "~y~[EXEMPLO] ~w~/editarporta " + id + " modelo 113");
                            }
                            else
                            {
                                int model = 0;

                                if (Int32.TryParse(value, out model))
                                {
                                    if (model < 0) //Change to Is Valid Door Model
                                    {
                                        API.sendChatMessageToPlayer(sender, "Escolha um modelo maior do que 0!");
                                        API.sendChatMessageToPlayer(sender, "~y~[EXEMPLO] ~w~/editarporta " + id + " modelo 113");
                                    }
                                    else
                                    {
    
                                        door.Model = model;

                                        DoorBLL.Door_Save(door);                                            

                                        API.sendChatMessageToPlayer(sender, "Você alterou o modelo da porta ID " + id + " para " + model);
                                    }
                                }
                                else
                                {
                                    API.sendChatMessageToPlayer(sender, "Digite apenas números no modelo!");
                                    API.sendChatMessageToPlayer(sender, "~y~[EXEMPLO] ~w~/editarporta " + id + " modelo 113");
                                }
                            }
                            break;

                        case "trancado":
                            if (value.Equals("default"))
                            {
                                API.sendChatMessageToPlayer(sender, "Escolha um valor (0 ou 1, destrancado ou trancado)!");
                                API.sendChatMessageToPlayer(sender, "~y~[EXEMPLO] ~w~/editarporta " + id + " trancado 0");
                            }
                            else
                            {
                                int locked = 0;

                                if (Int32.TryParse(value, out locked))
                                {
                                    if (locked < 0 || locked > 1)
                                    {
                                        API.sendChatMessageToPlayer(sender, "Escolha um valor entre 0 e 1!");
                                        API.sendChatMessageToPlayer(sender, "~y~[EXEMPLO] ~w~/editarporta " + id + " trancado 0");
                                    }
                                    else
                                    {

                                        if(locked == 0)
                                        {
                                            door.Locked = false;
                                            API.sendChatMessageToPlayer(sender, "Você destrancou a porta ID " + id);

                                        }
                                        else
                                        {
                                            door.Locked = true;
                                            API.sendChatMessageToPlayer(sender, "Você trancou a porta ID " + id);
                                        }

                                        DoorBLL.Door_Save(door);
                                    }
                                }
                                else
                                {
                                    API.sendChatMessageToPlayer(sender, "Digite apenas números no trancado!");
                                    API.sendChatMessageToPlayer(sender, "~y~[EXEMPLO] ~w~/editarporta " + id + " modelo 113");
                                }
                            }
                            break;

                        default:
                            API.sendChatMessageToPlayer(sender, "Escolha uma ação válida!");
                            API.sendChatMessageToPlayer(sender, "~y~[AÇÕES] ~w~pos, interior, propriedade, modelo, trancado.");
                            break;
                    }
                }
            }
            //}
        }
    }
}