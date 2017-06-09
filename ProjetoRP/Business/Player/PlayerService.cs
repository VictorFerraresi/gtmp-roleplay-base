using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Business.Player
{
    public class PlayerService
    {
        private ActivePlayer ActivePlayer;

        public PlayerService(ActivePlayer ActivePlayer)
        {
            this.ActivePlayer = ActivePlayer;
        }

        /// <summary>
        /// Returns a list of reasons why a certain user should not be able 
        /// to log in. Bans are not mapped, but the absence of AttributeType.Activate 
        /// includes a value to the returned list.</summary>
        public List<Entities.PlayerAttribute> GetConflictingAttributesForLogin()
        {
            var conflicts = new List<Entities.PlayerAttribute>();
            var has_activated = false;

            foreach(var attribute in ActivePlayer.Player.PlayerAttributes)
            {
                if (attribute.Attribute == Entities.PlayerAttribute.AttributeType.Banned && (attribute.ExpiresAt == null || DateTime.Now < attribute.ExpiresAt.Value))
                {
                    conflicts.Add(attribute);
                }

                if(attribute.Attribute == Entities.PlayerAttribute.AttributeType.Activated)
                {
                    has_activated = true;
                }
            }

            if(has_activated == false)
            {
                conflicts.Add(new Entities.PlayerAttribute { Attribute = Entities.PlayerAttribute.AttributeType.Activated, ExpiresAt = DateTime.MinValue });
            }

            return conflicts;
        }
    }
}
