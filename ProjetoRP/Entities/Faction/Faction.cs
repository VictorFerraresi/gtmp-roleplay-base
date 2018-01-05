using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Entities.Faction
{
    public enum FactionType
    { 
        FACTION_TYPE_NULL = 0,
        FACTION_TYPE_CIVILIAN = 1,
        FACTION_TYPE_POLICE = 2,
        FACTION_TYPE_EMS = 3,
        FACTION_TYPE_GANG = 4,
        FACTION_TYPE_MAFIA = 5
    }

    public class Faction
    {
        public Faction()
        {
            Ranks = new List<Rank>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3), MaxLength(40)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        [Required]
        [MinLength(2), MaxLength(7)]
        [Index(IsUnique = true)]
        public string Acro { get; set; }

        [Required]
        public FactionType Type { get; set; }

        [Required]
        public int Bank { get; set; }


        virtual public ICollection<Rank> Ranks { get; set; }

        virtual public ICollection<Locker> Lockers { get; set; }
    }
}