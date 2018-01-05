using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using ProjetoRP.Business.Industry;


namespace ProjetoRP.Modules.Industry
{
    public class Industry : Script
    {
        private IndustryBLL IndBLL = new IndustryBLL();

        public Industry()
        {
            API.onResourceStart += OnResourceStart;
            API.onClientEventTrigger += OnClientEventTrigger;
        }

        public void OnResourceStart()
        {
            API.consoleOutput(Messages.console_startup);
            IndBLL.LoadIndustries();
            IndBLL.DrawIndustriesPickups();
            IndBLL.DrawLoadPointsPickups();
        }

        public void OnClientEventTrigger(Client player, string eventName, object[] args)
        {
            
        }

        //Commands               
    }
}