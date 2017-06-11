using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GrandTheftMultiplayer.Server.Elements;
using ProjetoRP.Types;

namespace ProjetoRP.Entities.Industry
{
    public class LoadPoint
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public ProductType ProductType { get; set; }

        public int? Industry_Id { get; set; }
        
        [ForeignKey("Industry_Id")]
        public Industry Industry { get; set; }

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