using GrandTheftMultiplayer.Server.API;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
