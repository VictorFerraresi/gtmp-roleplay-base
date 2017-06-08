using System;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Business
{
    class Utils
    {
        public static void ProxDetector(float radius, Client player, string message, string col1, string col2, string col3, string col4, string col5)
        {
            var players = API.shared.getPlayersInRadiusOfPlayer(radius, player);
            foreach (Client c in players)
            {
                if (player.position.DistanceTo(c.position) <= radius / 16)
                {
                    API.shared.sendChatMessageToPlayer(c, col1, message);
                }
                else if (player.position.DistanceTo(c.position) <= radius / 8)
                {
                    API.shared.sendChatMessageToPlayer(c, col2, message);
                }
                else if (player.position.DistanceTo(c.position) <= radius / 4)
                {
                    API.shared.sendChatMessageToPlayer(c, col3, message);
                }
                else if (player.position.DistanceTo(c.position) <= radius / 2)
                {
                    API.shared.sendChatMessageToPlayer(c, col4, message);
                }
                else if (player.position.DistanceTo(c.position) <= radius)
                {
                    API.shared.sendChatMessageToPlayer(c, col5, message);
                }
            }
        }

        public static void ExclusiveProxDetector(float radius, Client player, string message, string col1, string col2, string col3, string col4, string col5)
        {
            var players = API.shared.getPlayersInRadiusOfPlayer(radius, player);
            foreach (Client c in players)
            {
                if (c == player) continue;

                if (player.position.DistanceTo(c.position) <= radius / 16)
                {
                    API.shared.sendChatMessageToPlayer(c, col1, message);
                }
                else if (player.position.DistanceTo(c.position) <= radius / 8)
                {
                    API.shared.sendChatMessageToPlayer(c, col2, message);
                }
                else if (player.position.DistanceTo(c.position) <= radius / 4)
                {
                    API.shared.sendChatMessageToPlayer(c, col3, message);
                }
                else if (player.position.DistanceTo(c.position) <= radius / 2)
                {
                    API.shared.sendChatMessageToPlayer(c, col4, message);
                }
                else if (player.position.DistanceTo(c.position) <= radius)
                {
                    API.shared.sendChatMessageToPlayer(c, col5, message);
                }
            }
        }        
    }
}
