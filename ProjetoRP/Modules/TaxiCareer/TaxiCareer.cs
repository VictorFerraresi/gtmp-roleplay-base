using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using GrandTheftMultiplayer.Server.Managers;
using ProjetoRP.Business.Player;
using ProjetoRP.Business.Vehicle;
using ProjetoRP.Entities;

namespace ProjetoRP.Modules.TaxiCareer
{
    class TaxiCareer : Script
    {

        Business.Career.TaxiCareerBLL TaxiBLL = new Business.Career.TaxiCareerBLL();

        public TaxiCareer()
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
                case "CS_REQUEST_TAXI":
                    string streetName = (string)args[0];
                    string areaName = (string)args[1];

                    ActivePlayer ac = ActivePlayer.GetSpawned(player);
                    if (ac == null) return;

                    Character c = ac.Character;
                    TaxiBLL.SendMessageToOnDuty("~y~____________________[Chamado de Taxi]____________________");
                    string taxiMsg = string.Format("{0} solicitou um taxi em ~b~{1}, {2}~w~. Digite ~b~/aceitartaxi {3} ~w~para aceitar o chamado.", c.Name, streetName, areaName, ac.Id);
                    TaxiBLL.SendMessageToOnDuty(taxiMsg);
                    break;

                case "CS_CANCEL_FARE_CONFIRMATION":
                    if (ActivePlayer.GetSpawned(player) == null) return;
                    c = ActivePlayer.GetSpawned(player).Character;

                    int resp = (int)args[0];

                    if (!player.hasData("TAXI_CUSTOMER"))
                    {
                        TaxiCareer_KickForInvalidTrigger(player);
                    }
                    else
                    {
                        switch (resp)
                        {
                            case 0: //Yes
                                API.sendChatMessageToPlayer(player, "Você cancelou a sua corrida atual e não recebeu nenhum pagamento!");

                                Character taxiCustomer = player.getData("TAXI_CUSTOMER");

                                ActivePlayer targetAc = ActivePlayer.Get(taxiCustomer);
                                if (targetAc != null)
                                {
                                    API.sendChatMessageToPlayer(targetAc.Client, "O taxista cancelou a sua corrida e você não pagou nada!");
                                }

                                targetAc.Client.resetData("TAXI_DRIVER");
                                player.resetData("TAXI_CUSTOMER");

                                if (targetAc.Client.hasData("TAXI_TIMER"))
                                {
                                    TaxiBLL.CancelFare(c, taxiCustomer);
                                }                                

                                API.triggerClientEvent(player, "SC_CLOSE_CANCEL_FARE_CONFIRM_MENU");
                                break;

                            case 1: //No
                                API.sendChatMessageToPlayer(player, "Você desistiu e não cancelou a corrida!");
                                break;

                            default:
                                TaxiCareer_KickForInvalidTrigger(player);
                                break;
                        }
                    }

                    API.triggerClientEvent(player, "SC_CLOSE_CANCEL_FARE_CONFIRM_MENU");                    
                    break;
            }
        }


        private void TaxiCareer_KickForInvalidTrigger(Client player)
        {
            player.kick(Player.Messages.player_kicked_inconsistency);
        }

        // Commands        
        [Command("taxista")]
        public void TaxiDriverCommand(Client player)
        {
            ActivePlayer ac = ActivePlayer.GetSpawned(player);
            if (ac == null) return;

            Character c = ac.Character;

            if (c.Career == null || c.Career.Type != Entities.Career.CareerType.Taxi)
            {
                API.sendChatMessageToPlayer(player, "Você não é um taxista!");
            }
            else
            {
                if (player.hasData("TAXI_DUTY"))
                {
                    if (player.hasData("TAXI_CUSTOMER"))
                    {
                        API.sendChatMessageToPlayer(player, "Você possui um cliente ativo! Digite /cancelarcorrida para cancelar a corrida!");
                    }
                    else
                    {
                        player.resetData("TAXI_DUTY");
                        API.sendChatMessageToPlayer(player, "Você saiu do trabalho e não receberá mais chamados de taxista!");
                    }                    
                }
                else
                {
                    if (!API.isPlayerInAnyVehicle(player))
                    {
                        API.sendChatMessageToPlayer(player, "Você não está dentro de um veículo!");
                    }
                    else
                    {
                        NetHandle serverVeh = API.getPlayerVehicle(player);
                        Entities.Vehicle.Vehicle veh = ActiveVehicle.GetSpawned(serverVeh).Vehicle;

                        if (!TaxiBLL.IsValidTaxi(veh))
                        {
                            API.sendChatMessageToPlayer(player, "Este veículo não é apropriado para o trabalho!");
                        }
                        else
                        {
                            player.setData("TAXI_DUTY", true);
                            API.sendChatMessageToPlayer(player, "Você entrou em trabalho e agora receberá chamados de taxista!");
                        }
                    }                    
                }
            }            
        }

        [Command("taxi")]
        public void CallTaxiCommand(Client player)
        {
            ActivePlayer ac = ActivePlayer.GetSpawned(player);
            if (ac == null) return;

            Character c = ac.Character;

            if (player.hasData("TAXI_REQUEST"))
            {
                API.sendChatMessageToPlayer(player, "Você já possui um chamado de taxi ativo! Digite /cancelartaxi para cancelar.");
            }
            else if (player.hasData("TAXI_DRIVER"))
            {
                API.sendChatMessageToPlayer(player, "Você já está em uma corrida! Caso deseje cancelá-la, entre em contato com o taxista.");
            }
            else
            {
                if(TaxiBLL.GetOnDutyCount() == 0)
                {
                    API.sendChatMessageToPlayer(player, "Infelizmente não existem taxistas em trabalho no momento!");
                }
                else
                {
                    API.sendChatMessageToPlayer(player, "Você chamou um taxi. Aguarde até que um taxista aceite o seu chamado!");
                    API.sendChatMessageToPlayer(player, "~r~Não saia de perto do local de chamada!");
                    API.triggerClientEvent(player, "SC_REQUEST_TAXI");

                    player.setData("TAXI_REQUEST", true);
                }                
            }
        }

        [Command("cancelartaxi")]
        public void CancelTaxiCommand(Client player)
        {
            ActivePlayer ac = ActivePlayer.GetSpawned(player);
            if (ac == null) return;

            Character c = ac.Character;

            if (!player.hasData("TAXI_REQUEST"))
            {
                API.sendChatMessageToPlayer(player, "Você não possui nenhum chamado de taxi ativo!");
            }
            else
            {
                API.sendChatMessageToPlayer(player, "Você cancelou o seu chamado de taxi!");
                player.resetData("TAXI_REQUEST");
            }
        }

        [Command("aceitartaxi")]
        public void AcceptTaxiCommand(Client player, int targetid)
        {
            ActivePlayer ac = ActivePlayer.GetSpawned(player);
            if (ac == null) return;

            Character c = ac.Character;

            if (!player.hasData("TAXI_DUTY"))
            {
                API.sendChatMessageToPlayer(player, "Você não está em trabalho como um taxista! (/taxista)");
            }
            else
            {
                ActivePlayer acTarget = ActivePlayer.GetSpawned(targetid);
                if (acTarget == null)
                {
                    API.sendChatMessageToPlayer(player, "Este jogador não está conectado!");
                }
                else
                {
                    Character cTarget = acTarget.Character;

                    if (!acTarget.Client.hasData("TAXI_REQUEST"))
                    {
                        API.sendChatMessageToPlayer(player, "Este jogador não solicitou um taxi ou sua solicitação já foi aceita!");
                    }
                    else if (player.hasData("TAXI_CUSTOMER"))
                    {
                        API.sendChatMessageToPlayer(player, "Você já possui um cliente ativo!");
                    }
                    else
                    {
                        API.sendChatMessageToPlayer(player, "Você aceitou a solicitação. Siga a marca em seu GPS!");
                        string taxiMsg = string.Format("O taxista {0} aceitou a sua solicitação! Aguarde no local.", c.Name);
                        API.sendChatMessageToPlayer(acTarget.Client, taxiMsg);

                        acTarget.Client.resetData("TAXI_REQUEST");

                        acTarget.Client.setData("TAXI_DRIVER", c);
                        player.setData("TAXI_CUSTOMER", cTarget);

                        Vector3 targetPos = acTarget.Client.position;
                        API.triggerClientEvent(player, "SC_SET_WAYPOINT", targetPos.X, targetPos.Y);
                    }
                }
            }                        
        }

        [Command("cancelarcorrida")]
        public void CancelFareCommand(Client player)
        {
            ActivePlayer ac = ActivePlayer.GetSpawned(player);
            if (ac == null) return;

            Character c = ac.Character;

            if (!player.hasData("TAXI_DUTY"))
            {
                API.sendChatMessageToPlayer(player, "Você não está em trabalho como um taxista! (/taxista)");
            }
            else if (!player.hasData("TAXI_CUSTOMER"))
            {
                API.sendChatMessageToPlayer(player, "Você não está em uma corrida!");
            }
            else
            {
                API.triggerClientEvent(player, "SC_SHOW_CANCEL_FARE_CONFIRM_MENU");                
            }
        }
    }
}