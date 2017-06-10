using System;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared.Math;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetoRP.Business.Career;
using ProjetoRP.Business.Player;
using ProjetoRP.Entities;
using ProjetoRP.Types;

namespace ProjetoRP.Modules.Career
{
    class Career : Script
    {
        private CareerBLL CareerBLL = new CareerBLL();

        public Career()
        {
            API.onResourceStart += OnResourceStart;
            API.onResourceStop += OnResourceStop;
            API.onClientEventTrigger += OnClientEventTrigger;
        }

        public void OnResourceStart()
        {
            API.consoleOutput(Messages.console_startup);
            CareerBLL.LoadCareers();
            CareerBLL.DrawCareersPickups();
        }
        public void OnResourceStop()
        {

        }

        public void OnClientEventTrigger(Client player, string eventName, object[] args)
        {            
            switch (eventName)
            {
                case "CS_LEAVE_CAREER_CONFIRMATION":                    
                    if (ActivePlayer.GetSpawned(player) == null) return;
                    Character c = ActivePlayer.GetSpawned(player).Character;
                    int resp = (int)args[0];

                    if (c.Career == null)
                    {
                        Career_KickForInvalidTrigger(player);
                    }
                    else
                    {
                        switch (resp)
                        {
                            case 0: //Yes
                                c.Career = null;
                                c.CareerExperience = null;
                                c.CareerRank = null;
                                c.Career_Id = null;

                                API.sendChatMessageToPlayer(player, "Você saiu do seu emprego!");
                                break;

                            case 1: //No
                                API.sendChatMessageToPlayer(player, "Você cancelou e não saiu do seu emprego!");
                                break;

                            default:
                                Career_KickForInvalidTrigger(player);
                                break;
                        }                        
                    }

                    API.triggerClientEvent(player, "SC_CLOSE_LEAVE_CAREER_CONFIRM_MENU");
                    break;
            }
        }

        private void Career_KickForInvalidTrigger(Client player)
        {
            player.kick(Messages.player_kicked_inconsistency);
        }

        // Commands
        [Command("emprego")]
        public void CareerCommand(Client player)
        {
            ActivePlayer ac = ActivePlayer.GetSpawned(player);
            if (ac == null) return;

            Character c = ac.Character;

            Entities.Career.Career career = CareerBLL.Career_GetNearestInRange(player, 5.0);
            if (career == null)
            {
                API.sendChatMessageToPlayer(player, "Você não está próximo a nenhum emprego!");
            }
            else if (null != c.Career)
            {
                API.sendChatMessageToPlayer(player, "Você já possui um emprego! (/meuemprego)");
            }
            else
            {
                c.Career = career;
                c.Career_Id = career.Id;
                c.CareerRank = 1;
                c.CareerExperience = 0;

                string career_msg = string.Format("Você agora é um {0}!", career.Name);
                API.sendChatMessageToPlayer(player, career_msg);

                switch (career.Type)
                {
                    case Entities.Career.CareerType.Trucker:
                        API.sendPictureNotificationToPlayer(player, "Entre em uma van para começar a trabalhar", "CHAR_MP_ARMY_CONTACT", 1, 0, "John Doe", "Você agora é um caminhoneiro");
                        break;
                    default:
                        break;
                }
            }
        }

        [Command("meuemprego")]
        public void MyCareerCommand(Client player)
        {
            ActivePlayer ac = ActivePlayer.GetSpawned(player);
            if (ac == null) return;

            Character c = ac.Character;

            if (c.Career == null)
            { 
                API.sendChatMessageToPlayer(player, "Você não possui um emprego!");
            }
            else
            {
                switch (c.Career.Type)
                {
                    case Entities.Career.CareerType.Trucker:
                        string rank, nextrank, lastrank;
                        TruckerRank last = Enum.GetValues(typeof(TruckerRank)).Cast<TruckerRank>().Max();                                                
                        TruckerRankDictionary.TruckerRankNames.TryGetValue((TruckerRank)Enum.ToObject(typeof(TruckerRank), c.CareerRank), out rank);
                        TruckerRankDictionary.TruckerRankNames.TryGetValue((TruckerRank)Enum.ToObject(typeof(TruckerRank), c.CareerRank+1), out nextrank);
                        TruckerRankDictionary.TruckerRankNames.TryGetValue(last, out lastrank);
                        API.sendChatMessageToPlayer(player, "~y~______________[" + c.Career.Name + "]______________");
                        API.sendChatMessageToPlayer(player, "Meu cargo: ~c~" + rank);
                        API.sendChatMessageToPlayer(player, "Experiência Atual: ~c~" + c.CareerExperience);
                        if(rank != lastrank)
                        {
                            API.sendChatMessageToPlayer(player, "Próximo cargo: ~c~" + nextrank);
                            API.sendChatMessageToPlayer(player, "Experiência necessária para o próximo cargo: ~c~" + (c.CareerRank + 10));
                        }
                        else
                        {
                            API.sendChatMessageToPlayer(player, "Você já atingiu o maior cargo deste emprego!");
                        }
                        API.sendChatMessageToPlayer(player, "Para abandonar o emprego atual, digite ~r~/sairemprego");
                        break;
                }
            }
        }

        [Command("sairemprego")]
        public void LeaveCareerCommand(Client player)
        {
            ActivePlayer ac = ActivePlayer.GetSpawned(player);
            if (ac == null) return;

            Character c = ac.Character;

            if (c.Career == null)
            {
                API.sendChatMessageToPlayer(player, "Você não possui um emprego!");
            }
            else
            {
                API.shared.triggerClientEvent(player, "SC_SHOW_LEAVE_CAREER_CONFIRM_MENU");                
            }
        }
    }
}