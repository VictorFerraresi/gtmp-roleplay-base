using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System.Collections.Generic;
using ProjetoRP.Business.Career;
using ProjetoRP.Business.Player;
using ProjetoRP.Entities;
using ProjetoRP.Business.Vehicle;
using ProjetoRP.Business.Industry;
using ProjetoRP.Types;
using System.Linq;
using System;
using Newtonsoft.Json.Linq;

namespace ProjetoRP.Modules.TruckerCareer
{
    class TruckerCareer : Script
    {
        private TruckerCareerBLL TruckerBLL = new TruckerCareerBLL();
        private IndustryBLL IndustryBLL = new IndustryBLL();
        private VehicleBLL VehBLL = new VehicleBLL();
        private PlayerBLL PlayerBLL = new PlayerBLL();

        public TruckerCareer()
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
                case "CS_TRUCKER_PDA_CLOSE":
                    API.call("Ui", "evalUi", player, "truckerpda_app.display=false;truckerpda_app.blocked=false");
                    API.call("Ui", "fixCursor", player, false);
                    break;

                case "CS_TRUCKER_PDA_TRACKGPS":
                    int loadpointid = (int)args[0];

                    Entities.Industry.LoadPoint lp = IndustryBLL.FindLoadPointById(loadpointid);

                    if(lp == null)
                    {
                        TruckerCareer_KickForInvalidTrigger(player);
                    }
                    else
                    {
                        API.triggerClientEvent(player, "SC_SET_WAYPOINT", lp.X, lp.Y);
                    }
                    break;

                case "CS_TRUCK_CARGO_CLOSE":
                    API.call("Ui", "evalUi", player, "truckcargo_app.display=false;truckcargo_app.blocked=false");
                    API.call("Ui", "fixCursor", player, false);
                    break;

                case "CS_TAKE_CARGO_FROM_TRUCK":
                    if (API.isPlayerInAnyVehicle(player))
                    {
                        API.sendChatMessageToPlayer(player, "Você só pode retirar as caixas do caminhão se estiver a pé!");
                    }
                    else if (player.hasData("CRATE_HOLDING"))
                    {
                        API.sendChatMessageToPlayer(player, "Você só pode segurar uma caixa por vez!");
                    }
                    else
                    {
                        JToken data = JArray.Parse((string)args[0]);
                        int cargoid = (int)data[0];
                        int truckid = (int)data[1];

                        Entities.Vehicle.Vehicle veh = ActiveVehicle.GetSpawned(truckid).Vehicle;

                        if (veh == null)
                        {
                            TruckerCareer_KickForInvalidTrigger(player);
                        }
                        else
                        {
                            if(!VehBLL.Vehicle_IsNearPlayer(veh, player))
                            {
                                API.sendChatMessageToPlayer(player, "Este caminhão não está mais próximo a você!");
                            }
                            else
                            {
                                ProductType? prodType = veh.Cargo.ElementAtOrDefault(cargoid);

                                if (prodType == null)
                                {
                                    API.sendChatMessageToPlayer(player, "Esta caixa não está mais no caminhão!");
                                }
                                else
                                {
                                    if(player.hasData("BIZ_UNLOADING") || player.hasData("CRATE_UNLOADING"))
                                    {
                                        if(TruckerBLL.CountCargoOfType(veh, (ProductType)prodType) == 1) //Is the last crate of type
                                        {
                                            player.setData("LAST_CRATE", true);
                                        }
                                    }
                                    NetHandle cargoCrate = API.createObject(1102352397, player.position, new Vector3(0, 0, 0), player.dimension);
                                    API.attachEntityToEntity(cargoCrate, player, "SKEL_R_Hand", new Vector3(0, 0, 0), new Vector3(0, 0, 0));

                                    player.setData("CRATE_HOLDING", prodType);
                                    player.setData("CRATE_HOLDING_OBJ", cargoCrate);

                                    veh.Cargo.RemoveAt(cargoid);

                                    string unloadMsg = string.Format("Caixa de {0} descarregada", IndustryBLL.LoadPoint_GetProductName((ProductType)prodType));
                                    API.sendNotificationToPlayer(player, unloadMsg);
                                }                                                                
                            }
                        }
                    }
                    break;

                case "CS_TRUCK_UNLOAD_CARGO_CLOSE":
                    API.call("Ui", "evalUi", player, "truckunloadcargo_app.display=false;truckunloadcargo_app.blocked=false");
                    API.call("Ui", "fixCursor", player, false);
                    break;

                case "CS_TRUCK_UNLOAD_CARGO_START":
                    if (!API.isPlayerInAnyVehicle(player))
                    {
                        API.sendChatMessageToPlayer(player, "Você não está mais dentro do caminhão!");
                    }
                    else
                    {
                        NetHandle serverVeh = API.getPlayerVehicle(player);
                        Entities.Vehicle.Vehicle veh = ActiveVehicle.GetSpawned(serverVeh).Vehicle;

                        if (!veh.Cargo.Any())
                        {
                            API.sendChatMessageToPlayer(player, "Não existe mais carga neste veículo!");
                        }
                        else
                        {
                            IEnumerable<ProductType> cargoDistinct = veh.Cargo.Distinct();
                            int index = (int)args[0];

                            ProductType? prodType = cargoDistinct.ElementAtOrDefault(index);

                            if(prodType == null)
                            {
                                API.sendChatMessageToPlayer(player, "Esta carga não está mais no caminhão!");
                            }
                            else
                            {
                                Entities.Property.Business b = TruckerBLL.GetRandomBusiness((ProductType)prodType);
                                string deliverMsg = string.Format("Siga o GPS para a empresa {0}.", b.Name);
                                API.sendChatMessageToPlayer(player, deliverMsg);
                                API.sendChatMessageToPlayer(player, "Ao chegar, utilize /entregarcarga com a carga em mãos na porta da empresa.");
                                API.triggerClientEvent(player, "SC_SET_WAYPOINT", b.X, b.Y);

                                player.setData("BIZ_UNLOADING", b);
                                player.setData("TYPE_UNLOADING", prodType);
                            }
                        }                        
                    }                    
                    break;
            }            
        }

        private void TruckerCareer_KickForInvalidTrigger(Client player)
        {
            player.kick(Player.Messages.player_kicked_inconsistency);
        }

        // Commands        

        [Command("caminhoneiro")]
        public void TruckerCommand(Client player)
        {
            ActivePlayer ac = ActivePlayer.GetSpawned(player);
            if (ac == null) return;

            Character c = ac.Character;

            if (!API.isPlayerInAnyVehicle(player))
            {
                API.sendChatMessageToPlayer(player, "Você não está dentro de um veículo!");
            }
            else
            {
                NetHandle serverVeh = API.getPlayerVehicle(player);
                Entities.Vehicle.Vehicle veh = ActiveVehicle.GetSpawned(serverVeh).Vehicle;

                if (c.Career == null || c.Career.Type != Entities.Career.CareerType.Trucker)
                {
                    API.sendChatMessageToPlayer(player, "Você não é um caminhoneiro!");
                }
                else
                {
                    if (!TruckerBLL.IsValidTruck(veh))
                    {
                        API.sendChatMessageToPlayer(player, "Este veículo não é apropriado para o trabalho!");
                    }
                    else if (!TruckerBLL.CanDriveTruck(c, veh))
                    {
                        API.sendChatMessageToPlayer(player, "Você não possui o cargo necessário para trabalhar com este caminhão!");
                    }
                    else //Vehicle is a valid truck and trucker has enough rank to drive it
                    {
                        dynamic data = new System.Dynamic.ExpandoObject();

                        dynamic loadpoints = new List<System.Dynamic.ExpandoObject>();

                        foreach (Entities.Industry.Industry industry in Business.GlobalVariables.Instance.ServerIndustries)
                        {                            
                            foreach (Entities.Industry.LoadPoint lp in industry.LoadPoints)
                            {
                                dynamic dyn = new System.Dynamic.ExpandoObject();

                                dyn.id = lp.Id;
                                dyn.industryname = lp.Industry.Name;
                                dyn.product = IndustryBLL.LoadPoint_GetProductName(lp.ProductType);

                                loadpoints.Add(dyn);
                            }
                        }

                        data.loadpoints = loadpoints;

                        string _in = API.shared.toJson(data);                        
                        API.call("Ui", "fixCursor", player, true);
                        API.call("Ui", "evalUi", player, "truckerpda_app.in = " + _in + ";truckerpda_app.display=true;");
                    }
                }
            }            
        }

        [Command("pegarcarga")]
        public void TakeCargoCommand(Client sender)
        {
            ActivePlayer ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

            Character c = ac.Character;

            if (c.Career == null || c.Career.Type != Entities.Career.CareerType.Trucker)
            {
                API.sendChatMessageToPlayer(sender, "Você não é um caminhoneiro!");
            }
            else
            {
                Entities.Industry.LoadPoint lp = IndustryBLL.LoadPoint_GetNearestInRange(sender, 5.0);

                if (lp == null)
                {
                    API.sendChatMessageToPlayer(sender, "Você não está próximo a nenhum ponto de carga!");
                }
                else if (API.isPlayerInAnyVehicle(sender))
                {
                    API.sendChatMessageToPlayer(sender, "Saia do veículo primeiro para utilizar este comando!");
                }
                else
                {
                    ProductClass prodClass;
                    ProductTypeDictionary.ProductTypeClasses.TryGetValue(lp.ProductType, out prodClass);

                    switch (prodClass)
                    {
                        case ProductClass.Crate:
                        case ProductClass.SafeBox:
                            if (sender.hasData("CRATE_HOLDING"))
                            {
                                API.sendChatMessageToPlayer(sender, "Você já está segurando uma caixa!");
                            }
                            else
                            {
                                API.playPlayerAnimation(sender, (int)(AnimationFlags.AllowPlayerControl), "pickup_object", "pickup_low");
                                NetHandle cargoCrate = API.createObject(1102352397, sender.position, new Vector3(0, 0, 0), sender.dimension);
                                API.attachEntityToEntity(cargoCrate, sender, "SKEL_R_Hand", new Vector3(0, 0, 0), new Vector3(0, 0, 0));

                                sender.setData("CRATE_HOLDING_OBJ", cargoCrate);
                                sender.setData("CRATE_HOLDING", lp.ProductType);
                                string crateMsg = string.Format("Você pegou uma caixa de {0}. Digite /guardarcarga em um caminhão compatível.", IndustryBLL.LoadPoint_GetProductName(lp.ProductType));
                                API.sendChatMessageToPlayer(sender, crateMsg);
                            }                                                        
                            break;
                        case ProductClass.Liquid:
                        case ProductClass.Log:
                        case ProductClass.Loose:
                        case ProductClass.Vehicle:
                            API.sendChatMessageToPlayer(sender, "Este tipo de carga não pode ser carregado a pé! (/carregarcaminhao)");
                            break;
                    }
                }
            }
        }

        [Command("guardarcarga")]
        public void StoreCargoCommand(Client sender)
        {
            ActivePlayer ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

            Character c = ac.Character;

            if (c.Career == null || c.Career.Type != Entities.Career.CareerType.Trucker)
            {
                API.sendChatMessageToPlayer(sender, "Você não é um caminhoneiro!");
            }
            else if (!sender.hasData("CRATE_HOLDING"))
            {
                API.sendChatMessageToPlayer(sender, "Você não está segurando nenhuma carga!");
            }
            else
            {
                NetHandle serverVeh = VehBLL.Vehicle_GetNearestInRange(sender, 5.0);
                if (serverVeh == null)
                {
                    API.sendChatMessageToPlayer(sender, "Você não está próximo a nenhum veículo!");
                }
                else
                {
                    Entities.Vehicle.Vehicle veh = ActiveVehicle.GetSpawned(serverVeh).Vehicle;
                    if (!TruckerBLL.IsValidTruck(veh))
                    {
                        API.sendChatMessageToPlayer(sender, "Este veículo não é um caminhão!");
                    }
                    else
                    {
                        ProductType prodType = sender.getData("CRATE_HOLDING");
                        NetHandle crateObj = sender.getData("CRATE_HOLDING_OBJ");
                        ProductClass prodClass = IndustryBLL.GetProductClassFromType(prodType);

                        if (!TruckerBLL.CanCarryClass(veh, prodClass))
                        {
                            API.sendChatMessageToPlayer(sender, "Este caminhão não pode carregar este tipo de carga!");
                        }
                        else if (veh.Cargo.Count >= TruckerBLL.GetTruckCapacity(veh))
                        {
                            API.sendChatMessageToPlayer(sender, "O armazenamento deste caminhão está cheio. Descarregue-o utilizando /entregar!");
                            sender.resetData("CRATE_HOLDING");
                            sender.resetData("CRATE_HOLDING_OBJ");
                            API.deleteEntity(crateObj);
                        }
                        else
                        {
                            if(TruckerBLL.CountCargoOfType(veh, prodType) >= 5)
                            {
                                API.sendChatMessageToPlayer(sender, "Você só pode carregar 5 caixas do mesmo tipo!");
                                sender.resetData("CRATE_HOLDING");
                                sender.resetData("CRATE_HOLDING_OBJ");
                                API.deleteEntity(crateObj);
                            }
                            else
                            {
                                veh.Cargo.Add(prodType);
                                sender.resetData("CRATE_HOLDING");
                                sender.resetData("CRATE_HOLDING_OBJ");
                                API.deleteEntity(crateObj);
                                string loadMsg = string.Format("Caixa de {0} carregada", IndustryBLL.LoadPoint_GetProductName(prodType));
                                API.sendNotificationToPlayer(sender, loadMsg);
                            }                            
                        }                        
                    }
                }
            }
        }        

        [Command("carga")]
        public void CheckCargoCommand(Client sender)
        {
            ActivePlayer ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

            Character c = ac.Character;

            if (c.Career == null || c.Career.Type != Entities.Career.CareerType.Trucker)
            {
                API.sendChatMessageToPlayer(sender, "Você não é um caminhoneiro!");
            }
            else
            {
                NetHandle serverVeh;
                if (sender.vehicle != null)
                {
                    serverVeh = sender.vehicle;
                }
                else
                {
                    serverVeh = VehBLL.Vehicle_GetNearestInRange(sender, 5.0);
                }

                if (serverVeh == null)
                {
                    API.sendChatMessageToPlayer(sender, "Você não está próximo a nenhum veículo!");
                }
                else
                {
                    Entities.Vehicle.Vehicle veh = ActiveVehicle.GetSpawned(serverVeh).Vehicle;
                    if (!TruckerBLL.IsValidTruck(veh))
                    {
                        API.sendChatMessageToPlayer(sender, "Este veículo não é um caminhão!");
                    }
                    else
                    {
                        dynamic data = new System.Dynamic.ExpandoObject();

                        dynamic cargoes = new List<System.Dynamic.ExpandoObject>();

                        int id = 0;                        

                        foreach (ProductType product in veh.Cargo)
                        {
                            ProductClass prodClass = IndustryBLL.GetProductClassFromType(product);

                            dynamic dyn = new System.Dynamic.ExpandoObject();

                            dyn.id = id;
                            dyn.type = product;
                            dyn.name = IndustryBLL.LoadPoint_GetProductName(product);
                            dyn.className = IndustryBLL.GetProductClassName(prodClass);
                            dyn.truckid = ActiveVehicle.GetSpawned(serverVeh).Id;

                            cargoes.Add(dyn);

                            id++;
                        }

                        data.cargoes = cargoes;

                        string _in = API.shared.toJson(data);
                        API.call("Ui", "fixCursor", sender, true);
                        API.call("Ui", "evalUi", sender, "truckcargo_app.in = " + _in + ";truckcargo_app.display=true;");
                    }
                }              
            }
        }

        [Command("destruircarga")]
        public void DestructCargoCommand(Client sender)
        {
            ActivePlayer ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

            Character c = ac.Character;

            if (c.Career == null || c.Career.Type != Entities.Career.CareerType.Trucker)
            {
                API.sendChatMessageToPlayer(sender, "Você não é um caminhoneiro!");
            }
            else if (!sender.hasData("CRATE_HOLDING"))
            {
                API.sendChatMessageToPlayer(sender, "Você não está segurando nenhuma carga!");
            }
            else
            {                
                NetHandle crateObj = sender.getData("CRATE_HOLDING_OBJ");                        
                sender.resetData("CRATE_HOLDING");
                sender.resetData("CRATE_HOLDING_OBJ");
                API.deleteEntity(crateObj);
                API.sendNotificationToPlayer(sender, "Você destruiu esta carga");

                if (sender.hasData("LAST_CRATE") && (sender.getData("LAST_CRATE") == true))
                {
                    ProductType prodType = sender.getData("TYPE_UNLOADING");
                    sender.resetData("LAST_CRATE");
                    sender.resetData("BIZ_UNLOADING");
                    sender.resetData("TYPE_UNLOADING");
                    string finishTypeMsg = string.Format("Você terminou a entrega de carga do tipo {0}.", IndustryBLL.LoadPoint_GetProductName(prodType));
                    API.sendChatMessageToPlayer(sender, finishTypeMsg);
                }
            }
        }

        [Command("descarregar")]
        public void UnloadCargoCommand(Client sender)
        {
            ActivePlayer ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

            Character c = ac.Character;

            if (c.Career == null || c.Career.Type != Entities.Career.CareerType.Trucker)
            {
                API.sendChatMessageToPlayer(sender, "Você não é um caminhoneiro!");
            }
            else if (!API.isPlayerInAnyVehicle(sender))
            {
                API.sendChatMessageToPlayer(sender, "Você não está dentro de um veículo!");
            }
            else
            {
                NetHandle serverVeh = sender.vehicle;               
                Entities.Vehicle.Vehicle veh = ActiveVehicle.GetSpawned(serverVeh).Vehicle;
                if (!TruckerBLL.IsValidTruck(veh))
                {
                    API.sendChatMessageToPlayer(sender, "Este veículo não é um caminhão!");
                }
                else
                {
                    IEnumerable<ProductType> cargoDistinct = veh.Cargo.Distinct();

                    if (sender.hasData("BIZ_UNLOADING") || sender.hasData("TYPE_UNLOADING"))
                    {
                        ProductType prodType = sender.getData("TYPE_UNLOADING");

                        if (cargoDistinct.Contains(prodType))
                        {
                            string unloadMsg = string.Format("Descarregue toda a carga do tipo {0} antes de descarregar outro tipo!", IndustryBLL.LoadPoint_GetProductName(prodType));
                            API.sendChatMessageToPlayer(sender, unloadMsg);
                            return;
                        }
                    }

                    dynamic data = new System.Dynamic.ExpandoObject();

                    dynamic cargoes = new List<System.Dynamic.ExpandoObject>();

                    List<Tuple<ProductType, int>> prodCount = new List<Tuple<ProductType, int>>();                    

                    foreach(ProductType product in cargoDistinct)
                    {
                        int count = TruckerBLL.CountCargoOfType(veh, product);
                        prodCount.Add(new Tuple<ProductType, int>(product, count));
                    }

                    int id = 0;

                    foreach (Tuple<ProductType, int> quantityTuple in prodCount)
                    {
                        ProductType tupleType = quantityTuple.Item1;
                        int tupleCount = quantityTuple.Item2;

                        ProductClass prodClass = IndustryBLL.GetProductClassFromType(tupleType);

                        dynamic dyn = new System.Dynamic.ExpandoObject();

                        dyn.id = id;
                        dyn.type = tupleType;
                        dyn.name = IndustryBLL.LoadPoint_GetProductName(tupleType);
                        dyn.className = IndustryBLL.GetProductClassName(prodClass);
                        dyn.count = tupleCount;                                            

                        cargoes.Add(dyn);

                        id++;
                    }

                    data.cargoes = cargoes;

                    string _in = API.shared.toJson(data);
                    API.call("Ui", "fixCursor", sender, true);
                    API.call("Ui", "evalUi", sender, "truckunloadcargo_app.in = " + _in + ";truckunloadcargo_app.display=true;");
                }                
            }
        }

        [Command("entregarcarga")]
        public void DeliverCargoCommand(Client sender)
        {
            ActivePlayer ac = ActivePlayer.GetSpawned(sender);
            if (ac == null) return;

            Character c = ac.Character;

            if (c.Career == null || c.Career.Type != Entities.Career.CareerType.Trucker)
            {
                API.sendChatMessageToPlayer(sender, "Você não é um caminhoneiro!");
            }
            else if (API.isPlayerInAnyVehicle(sender))
            {
                API.sendChatMessageToPlayer(sender, "Você deve entregar a carga a pé!");
            }
            else if(!sender.hasData("BIZ_UNLOADING") || !sender.hasData("TYPE_UNLOADING"))
            {
                API.sendChatMessageToPlayer(sender, "Você não está entregando nenhuma carga. Digite /descarregar para iniciar o processo.");                
            }
            else
            {
                Entities.Property.Business b = sender.getData("BIZ_UNLOADING");
                ProductType prodType = sender.getData("TYPE_UNLOADING");
                
                if(!PlayerBLL.Player_IsInRangeOfPoint(sender, new Vector3(b.X, b.Y, b.Z)))
                {
                    API.sendChatMessageToPlayer(sender, "Você não está próximo a empresa designada!");
                    API.triggerClientEvent(sender, "SC_SET_WAYPOINT", b.X, b.Y);
                }
                else if (!sender.hasData("CRATE_HOLDING"))
                {
                    API.sendChatMessageToPlayer(sender, "Você não está segurando nenhuma caixa!");
                }
                else
                {
                    ProductType holdingType = sender.getData("CRATE_HOLDING");

                    if(holdingType != prodType)
                    {
                        string wrongTypeMsg = string.Format("Este tipo de carga não coincide com o tipo que você deve entregar aqui! ({0})", IndustryBLL.LoadPoint_GetProductName(prodType));
                        API.sendChatMessageToPlayer(sender, wrongTypeMsg);
                    }
                    else
                    {
                        NetHandle crateObj = sender.getData("CRATE_HOLDING_OBJ");

                        sender.resetData("CRATE_HOLDING");
                        sender.resetData("CRATE_HOLDING_OBJ");
                        API.deleteEntity(crateObj);

                        int payment = 25; //TO DO Payment system based on distances maybe with random multiplier

                        string salaryMsg = string.Format("~g~+${0} ~w~adicionados ao pagamento", payment);
                        API.sendNotificationToPlayer(sender, salaryMsg);

                        if(sender.hasData("LAST_CRATE") && (sender.getData("LAST_CRATE") == true))
                        {
                            sender.resetData("LAST_CRATE");
                            sender.resetData("BIZ_UNLOADING");
                            sender.resetData("TYPE_UNLOADING");
                            string finishTypeMsg = string.Format("Você terminou a entrega de carga do tipo {0}.", IndustryBLL.LoadPoint_GetProductName(prodType));
                            API.sendChatMessageToPlayer(sender, finishTypeMsg);
                        }
                    }
                }
            }
        }

        [Command("pos")]
        public void PosCommand(Client sender, int id)
        {
            API.consoleOutput(sender.position.ToString());
            API.consoleOutput(API.getEntityRotation(sender.vehicle).ToString());
        }
    }
}