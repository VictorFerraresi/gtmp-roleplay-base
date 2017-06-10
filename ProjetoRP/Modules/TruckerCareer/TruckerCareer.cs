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

namespace ProjetoRP.Modules.TruckerCareer
{
    class TruckerCareer : Script
    {
        private CareerBLL CareerBLL = new CareerBLL();

        public TruckerCareer()
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
        }

        // Commands        

        [Command("caminhoneiro")]
        public void TruckerCommand(Client player)
        {
            ActivePlayer ac = ActivePlayer.GetSpawned(player);
            if (ac == null) return;

            Character c = ac.Character;

            if (c.Career == null || c.Career.Type != Entities.Career.CareerType.Trucker)
            {
                API.sendChatMessageToPlayer(player, "Você não é um caminhoneiro!");
            }
            else
            {
                //Aqui começa a brincadeira
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