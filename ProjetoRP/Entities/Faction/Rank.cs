using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Entities.Faction
{
    public class Rank
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3), MaxLength(32)]        
        public string Name { get; set; }

        [Required]        
        public int Level { get; set; }

        public int Faction_Id { get; set; }

        [Required]
        [ForeignKey("Faction_Id")]
        public Faction Faction { get; set; }

        [Required]        
        public bool Leader { get; set; }        
    }
}
