using System;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared.Math;
using System.Collections.Generic;
using ProjetoRP.Business.Player;
using ProjetoRP.Business.Faction;
using ProjetoRP.Business.Property;

namespace ProjetoRP.Modules.Admin
{
    class Admin : Script
    {
        private DiscordBot _discordBot = new DiscordBot();
        private PropertyBLL PropBLL = new PropertyBLL();
        private DoorBLL DoorBLL = new DoorBLL();
        private FactionBLL FacBLL = new FactionBLL();

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
            switch (eventName)
            {
                case "CS_CREATE_FACTION_SUBMIT":
                    var ac = ActivePlayer.Get(player);
                    if (ac == null || ac.Status != Types.PlayerStatus.AdminDuty) return;

                    var datafc = API.fromJson((string)args[0]);

                    string name = (string)datafc.factionname;
                    string acro = (string)datafc.factionacro;
                    int type = (int)datafc.factiontype;
                    string bank = (string)datafc.factionbank;

                    string msgfc;

                    if (FacBLL.Faction_Validate(name, acro, type, bank, out msgfc))
                    {
                        int bankVal = 0;
                        int.TryParse(bank, out bankVal);

                        API.call("Ui", "evalUi", player, "factioncreate_app.display=false;factioncreate_app.blocked=false");
                        API.call("Ui", "fixCursor", player, false);
                        FacBLL.Faction_Create(name, acro, (Entities.Faction.FactionType)type, bankVal);
                        API.sendChatMessageToPlayer(player, msgfc);
                    }
                    else
                    {
                        API.call("Ui", "evalUi", player, "factioncreate_app.blocked=false;factioncreate_app.error='" + msgfc + "';");
                    }
                    break;

                case "CS_CREATE_FACTION_CANCEL":
                    API.call("Ui", "evalUi", player, "factioncreate_app.display=false;factioncreate_app.blocked=false");
                    API.call("Ui", "fixCursor", player, false);
                    API.sendChatMessageToPlayer(player, "Você cancelou a criação da facção!");
                    break;

                case "CS_CREATE_PROPERTY_SUBMIT":
                    var datapc = API.fromJson((string)args[0]);

                    string address = (string)datapc.propertyaddress;
                    int typepc = (int)datapc.propertytype;
                    string price = (string)datapc.propertyprice;

                    string msgpc;

                    if (PropBLL.Property_Validate(address, typepc, price, out msgpc))
                    {
                        int priceVal = 0;
                        int.TryParse(price, out priceVal);

                        API.call("Ui", "evalUi", player, "propertycreate_app.display=false;propertycreate_app.blocked=false");
                        API.call("Ui", "fixCursor", player, false);

                        Entities.Property.Property prop = null;

                        switch (typepc)
                        {
                            case (int)Entities.Property.PropertyType.PROPERTY_TYPE_HOUSE:
                                prop = new Entities.Property.House();
                                prop.Type = Entities.Property.PropertyType.PROPERTY_TYPE_HOUSE;
                                prop.Address = address;
                                prop.X = player.position.X;
                                prop.Y = player.position.Y;
                                prop.Z = player.position.Z;
                                prop.Price = priceVal;
                                prop.Dimension = player.dimension;
                                break;

                            case (int)Entities.Property.PropertyType.PROPERTY_TYPE_BUSINESS:
                                prop = new Entities.Property.Business();
                                prop.Type = Entities.Property.PropertyType.PROPERTY_TYPE_BUSINESS;
                                prop.Address = address;
                                prop.X = player.position.X;
                                prop.Y = player.position.Y;
                                prop.Z = player.position.Z;
                                prop.Price = priceVal;
                                prop.Dimension = player.dimension;
                                ((Entities.Property.Business)prop).Name = "Nova Empresa";
                                ((Entities.Property.Business)prop).BusinessType = Entities.Property.BusinessType.BUSINESS_TYPE_NULL;
                                break;

                            case (int)Entities.Property.PropertyType.PROPERTY_TYPE_ENTRANCE:
                                ///TODO                        
                                break;

                            case (int)Entities.Property.PropertyType.PROPERTY_TYPE_OFFICE:
                                //TODO
                                break;
                        }

                        PropBLL.Property_Create(prop, player.dimension);
                        API.sendChatMessageToPlayer(player, msgpc);
                    }
                    else
                    {
                        API.call("Ui", "evalUi", player, "propertycreate_app.blocked=false;propertycreate_app.error='" + msgpc + "';");
                    }
                    break;

                case "CS_CREATE_PROPERTY_CANCEL":
                    API.call("Ui", "evalUi", player, "propertycreate_app.display=false;propertycreate_app.blocked=false");
                    API.call("Ui", "fixCursor", player, false);
                    API.sendChatMessageToPlayer(player, "Você cancelou a criação da propriedade!");
                    break;
            }
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
            var ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

            var player = ac.Player;
            //if sender.IsAdmin(){
            SendAdminChatMessage(player.Name, text);
            _discordBot.SendAdminChatMessageToDiscord(player.Name, text);
            //}
        }

        [Command("criarpropriedade", GreedyArg = true)]
        public void CreatePropertyCommand(Client sender)
        {
            var ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

            //if (sender.IsAdmin()){
            API.call("Ui", "fixCursor", sender, true);
            API.call("Ui", "evalUi", sender, "propertycreate_app.display=true;");
            //}
        }

        [Command("deletarpropriedade")]
        public void DeletePropertyCommand(Client sender, int id)
        {
            var ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

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
            var ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

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
                            if (value.Equals("default"))
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
                                    if (price < 1)
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

                        case "nome":
                            if (value.Equals("default"))
                            {
                                API.sendChatMessageToPlayer(sender, "Escolha um nome para a propriedade!");
                                API.sendChatMessageToPlayer(sender, "~y~[EXEMPLO] ~w~/editarpropriedade " + id + " nome Posto de Gasolina X!");
                            }
                            else
                            {
                                if (prop is Entities.Property.Business)
                                {
                                    ((Entities.Property.Business)prop).Name = value;

                                    PropBLL.Property_Save(prop);
                                    PropBLL.RedrawPickup(prop);

                                    API.sendChatMessageToPlayer(sender, "Você alterou o nome da propriedade ID " + id + " para " + value);
                                }
                                else
                                {
                                    API.sendChatMessageToPlayer(sender, "Você só pode alterar o nome de propriedades do tipo Empresa!");
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
            var ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

            //if (sender.IsAdmin()){
            if (propid < 1)
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
            var ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

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
            var ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

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

                                        if (prop == null)
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

                                        if (locked == 0)
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

        [Command("criarfaccao", GreedyArg = true)]
        public void CreateFactionCommand(Client sender)
        {
            var ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

            //if (sender.IsAdmin()){            
            API.call("Ui", "fixCursor", sender, true);
            API.call("Ui", "evalUi", sender, "factioncreate_app.display=true;");
            //}
        }

        [Command("deletarfaccao")]
        public void DeleteFactionCommand(Client sender, int factionid)
        {
            var ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

            //if (sender.IsAdmin()){
            if (factionid < 1)
            {
                API.sendChatMessageToPlayer(sender, "Não existem facções com ID menor que 1!");
            }
            else
            {
                Entities.Faction.Faction faction = FacBLL.FindFactionById(factionid);

                if (faction == null)
                {
                    API.sendChatMessageToPlayer(sender, "Esta facção não existe!");
                }
                else
                {
                    FacBLL.Faction_Delete(faction);
                    API.sendChatMessageToPlayer(sender, "Você deletou esta facção com sucesso!");
                }
            }
            //}
        }

        [Command("editarfaccao", GreedyArg = true)]
        public void EditFactionCommand(Client sender, int id, string option, string value)
        {
            var ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

            //if (sender.IsAdmin()){
            if (id < 1)
            {
                API.sendChatMessageToPlayer(sender, "Não existem facções com ID menor que 1!");
            }
            else
            {
                Entities.Faction.Faction faction = FacBLL.FindFactionById(id);

                if (faction == null)
                {
                    API.sendChatMessageToPlayer(sender, "Esta facção não existe!");
                }
                else
                {
                    switch (option)
                    {
                        case "nome":
                            if (Business.GlobalVariables.Instance.ServerFactions.Find(x => x.Name == value) != null)
                            {
                                API.sendChatMessageToPlayer(sender, "Já existe uma facção com este nome!");
                            }
                            else
                            {
                                faction.Name = value;

                                FacBLL.Faction_Save(faction);

                                API.sendChatMessageToPlayer(sender, "Você alterou o nome da facção ID " + id + " para " + value);
                            }
                            break;

                        case "acro":
                            if (Business.GlobalVariables.Instance.ServerFactions.Find(x => x.Acro == value) != null)
                            {
                                API.sendChatMessageToPlayer(sender, "Já existe uma facção com este acrônimo!");
                            }
                            else
                            {
                                faction.Acro = value;

                                FacBLL.Faction_Save(faction);

                                API.sendChatMessageToPlayer(sender, "Você alterou o acrônimo da facção ID " + id + " para " + value);
                            }
                            break;

                        case "tipo":
                            int type = 0;

                            if (Int32.TryParse(value, out type))
                            {
                                if (!Enum.IsDefined(typeof(Entities.Faction.FactionType), type))
                                {
                                    API.sendChatMessageToPlayer(sender, "Este tipo é inválido!");
                                    API.sendChatMessageToPlayer(sender, "~y~[EXEMPLO] ~w~/editarfaccao " + id + " tipo 1");
                                }
                                else
                                {
                                    faction.Type = (Entities.Faction.FactionType)type;

                                    FacBLL.Faction_Save(faction);

                                    API.sendChatMessageToPlayer(sender, "Você alterou o tipo da facção ID " + id + " para " + type);
                                }
                            }
                            else
                            {
                                API.sendChatMessageToPlayer(sender, "Digite apenas números no tipo da facção!");
                                API.sendChatMessageToPlayer(sender, "~y~[EXEMPLO] ~w~/editarfaccao " + id + " tipo 1");
                            }
                            break;

                        case "cofre":
                            int bank = 0;

                            if (Int32.TryParse(value, out bank))
                            {
                                if (bank < 1)
                                {
                                    API.sendChatMessageToPlayer(sender, "Escolha um valor para o cofre maior do que 0!");
                                    API.sendChatMessageToPlayer(sender, "~y~[EXEMPLO] ~w~/editarfaccao " + id + " cofre 15000");
                                }
                                else
                                {
                                    faction.Bank = bank;

                                    FacBLL.Faction_Save(faction);

                                    API.sendChatMessageToPlayer(sender, "Você alterou o cofre da facção ID " + id + " para " + bank);
                                }
                            }
                            else
                            {
                                API.sendChatMessageToPlayer(sender, "Digite apenas números no cofre da facção!");
                                API.sendChatMessageToPlayer(sender, "~y~[EXEMPLO] ~w~/editarfaccao " + id + " cofre 150000");
                            }
                            break;

                        default:
                            API.sendChatMessageToPlayer(sender, "Escolha uma ação válida!");
                            API.sendChatMessageToPlayer(sender, "~y~[AÇÕES] ~w~nome, acro, tipo, cofre.");
                            break;
                    }
                }
            }
            //}
        }

        [Command("darlider")]
        public void GiveFactionleaderCommand(Client sender, int playerid, int factionid)
        {
            var ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

            var targetAc = ActivePlayer.GetSpawned(playerid);
            if (null == targetAc)
            {
                API.sendChatMessageToPlayer(sender, "Escolha um playerid válido!");
            }
            else if (factionid < 0)
            {
                API.sendChatMessageToPlayer(sender, "Não existem facções com o ID menor que 0!");
            }
            else if (factionid == 0)
            {
                Client target = targetAc.Client;

                Entities.Character c = targetAc.Character;
                c.Faction = null;
                c.Faction_Id = null;
                c.Rank = null;
                c.Rank_Id = null;

                API.sendChatMessageToPlayer(sender, "Você retirou o líder de facção do jogador " + c.Name);
            }
            else
            {
                Client target = targetAc.Client;
                Entities.Faction.Faction faction = FacBLL.FindFactionById(factionid);
                if (faction == null)
                {
                    API.sendChatMessageToPlayer(sender, "Esta facção não existe!");
                }
                else //Player is Connected and Faction Exists
                {
                    Entities.Character c = targetAc.Character;
                    c.Faction = faction;
                    c.Faction_Id = faction.Id;
                    Entities.Faction.Rank rank = FacBLL.Faction_GetLeaderRank(faction);
                    c.Rank = rank;
                    c.Rank_Id = rank.Id;

                    API.sendChatMessageToPlayer(sender, "Você setou o jogador " + c.Name + " como líder da facção " + faction.Name);
                    API.sendChatMessageToPlayer(target, "Você foi setado como líder da facção " + faction.Name + " pelo administrador " + ac.Character.Name);
                }
            }
        }

        [Command("local")]
        public void AreaNameCommand(Client sender)
        {
            var ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

            API.triggerClientEvent(sender, "SC_PRINT_AREA_NAME");
        }
    }     
}