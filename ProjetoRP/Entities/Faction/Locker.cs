using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GrandTheftMultiplayer.Server.Elements;

namespace ProjetoRP.Entities.Faction
{
    public class Locker
    {
        [Key]
        public int Id { get; set; }

        [MinLength(3), MaxLength(32)]
        public string Name { get; set; }

        public int Faction_Id { get; set; }

        [Required]
        [ForeignKey("Faction_Id")]
        public Faction Faction { get; set; }

        [Required]
        public double X { get; set; }
        [Required]
        public double Y { get; set; }
        [Required]
        public double Z { get; set; }
        [Required]
        public int Dimension { get; set; }

        [NotMapped]
        public Marker Pickup { get; set; }
        [NotMapped]
        public TextLabel TextLabel { get; set; }
    }
}
