using GTANetworkServer;
using ProjetoRP.Entities;
using ProjetoRP.Types;
using ProjetoRP.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Business.Player
{
    public class ActivePlayer
    {
        private const int MaxPlayers = 1000;
        private static List<ActivePlayer> PlayerServices = new List<ActivePlayer>();

        public Client Client { get; private set; }
        public Entities.Player Player { get; set; }
        public Entities.Character Character { get; set; }
        public PlayerStatus Status { get; set; }
        public int? Id { get; private set; }

        public ActivePlayer(Client client)
        {
            if (Get(client) != null)
            {
                throw new Exceptions.Player.ActiveCharacterAlreadyExistsException();
            }

            Client = client;
            Status = 0;

            PushAndAssignId();
        }

        public void Dispose()
        {
            PlayerServices.Remove(this);
        }

        public static ActivePlayer Create(Client client)
        {
            return new ActivePlayer(client);
        }

        public static ActivePlayer Get(int id)
        {
            foreach (var ac in PlayerServices)
            {
                if (ac.Id == id)
                {
                    return ac;
                }
            }
            return null;
        }

        public static ActivePlayer GetSpawned(int id)
        {
            var ac = Get(id);
            if (null != ac && ac.Status == PlayerStatus.Spawned)
            {
                return ac;
            }
            else
            {
                return null;
            }
        }

        public static ActivePlayer GetSpawned(Client client)
        {
            var ac = Get(client);
            if (ac.Status == PlayerStatus.Spawned)
            {
                return ac;
            }
            else
            {
                return null;
            }
        }

        public static ActivePlayer GetSpawned(Entities.Player player)
        {
            var ac = Get(player);
            if (ac.Status == PlayerStatus.Spawned)
            {
                return ac;
            }
            else
            {
                return null;
            }
        }

        public static ActivePlayer Get(Entities.Character character)
        {
            foreach (var ac in PlayerServices)
            {
                if (ac.Character == character && ac.Status == PlayerStatus.Spawned)
                {
                    return ac;
                }
            }
            return null;
        }

        public static ActivePlayer Get(Entities.Player player)
        {
            foreach (var ac in PlayerServices)
            {
                if (ac.Player == player && ac.Status > PlayerStatus.Login)
                {
                    return ac;
                }
            }
            return null;
        }

        public static ActivePlayer Get(Client client)
        {
            foreach (var ac in PlayerServices)
            {
                if (ac.Client == client)
                {
                    return ac;
                }
            }
            return null;
        }

        private void PushAndAssignId()
        {
            if (Id == null)
            {
                Id = GetVacantId();
                PlayerServices.Add(this);
            }
        }

        private static int GetVacantId()
        {
            for(var i = 0; i < MaxPlayers; i++)
            {
                if(Get(i) == null)
                {
                    return i;
                }
            }
            throw new Exceptions.Player.NoRemainingCharacterSlotsException();
        }
    }
}
