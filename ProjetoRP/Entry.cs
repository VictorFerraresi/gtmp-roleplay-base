using GTANetworkServer;
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

            using (var context = new DatabaseContext())
            {
                var players = context.Players.ToList();
                foreach (var p in players)
                {
                    Console.WriteLine(p.Id + " " + p.Name + " " + p.Email);
                }

            }
        }
    }
}
