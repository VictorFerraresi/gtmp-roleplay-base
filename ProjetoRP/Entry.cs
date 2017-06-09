using GrandTheftMultiplayer.Server.API;

namespace ProjetoRP
{
    public class Entry : Script
    {
        public Entry()
        {
            API.onResourceStart += OnResourceStart;
        }

        public void OnResourceStart()
        {
            API.consoleOutput("=== PROJETO RP ===");
            API.consoleOutput("Luís Gustavo Miki");
            API.consoleOutput("Victor Ferraresi");
        }
    }
}
