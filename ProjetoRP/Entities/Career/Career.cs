using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandTheftMultiplayer.Server.Elements;

namespace ProjetoRP.Entities.Career
{
    public enum CareerType
    {
        Abstract = 0,
        Trucker,
        Taxi,
        Mechanic,
        Fisherman,
        Sweeper
    }

    public class Career
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public CareerType Type { get; set; }

        [Required]
        public double X { get; set; }
        [Required]
        public double Y { get; set; }
        [Required]
        public double Z { get; set; }
        [Required]
        public int Dimension { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public bool Public { get; set; }

        [NotMapped]
        public Marker Pickup { get; set; }
        [NotMapped]
        public TextLabel TextLabel { get; set; }
    }
}