using GTANetworkServer;
using ProjetoRP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Business.Character
{
    public class ActiveCharacter
    {
        private const int MaxPlayers = 1000;
        private static List<ActiveCharacter> ActiveCharacters = new List<ActiveCharacter>();

        public Client Client { get; private set; }
        public Entities.Character Character { get; private set; }
        public int Id { get; private set; }

        public ActiveCharacter(Client client, Entities.Character character)
        {
            if (Get(client) != null || Get(character) != null)
            {
                throw new Exceptions.Character.ActiveCharacterAlreadyExistsException();
            }

            Client = client;
            Character = character;
            Id = GetVacantId();

            ActiveCharacters.Add(this);
        }

        public void Dispose()
        {
            ActiveCharacters.Remove(this);
        }

        public static ActiveCharacter Create(Client client, Entities.Character character)
        {
            return new ActiveCharacter(client, character);
        }

        public static ActiveCharacter Get(int id)
        {
            foreach (var ac in ActiveCharacters)
            {
                if (ac.Id == id)
                {
                    return ac;
                }
            }
            return null;
        }

        public static ActiveCharacter Get(Entities.Character character)
        {
            foreach (var ac in ActiveCharacters)
            {
                if (ac.Character == character)
                {
                    return ac;
                }
            }
            return null;
        }

        public static ActiveCharacter Get(Client client)
        {
            foreach (var ac in ActiveCharacters)
            {
                if (ac.Client == client)
                {
                    return ac;
                }
            }
            return null;
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
            throw new Exceptions.Character.NoRemainingCharacterSlotsException();
        }
    }
}
