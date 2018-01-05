using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GrandTheftMultiplayer.Server.Elements;

namespace ProjetoRP.Entities.Industry
{
    public class Industry
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3), MaxLength(32)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

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

        virtual public ICollection<LoadPoint> LoadPoints { get; set; }
    }
}