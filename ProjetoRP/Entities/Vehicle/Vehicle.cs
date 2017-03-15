using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoRP.Entities.Vehicle
{
    public enum OwnerType
    {
        OWNER_TYPE_NULL = 0,
        OWNER_TYPE_CHARACTER = 1,
        OWNER_TYPE_FACTION = 2
    }

    public class Vehicle
    {
        public Vehicle()
        {
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(5), MaxLength(32)]        
        public string Name { get; set; }
        
        public int? Owner_Id { get; set; }

        [Required]
        public OwnerType Owner_Type { get; set; }

        [Required]
        public double X { get; set; }    
        [Required]
        public double Y { get; set; }
        [Required]
        public double Z { get; set; }
        [Required]
        public double rX { get; set; }
        [Required]
        public double rY { get; set; }
        [Required]
        public double rZ { get; set; }

        [Required]
        public int Dimension { get; set; }

        [NotMapped]
        public bool Engine { get; set; }
        [NotMapped]
        public bool Locked { get; set; }

        [Required]
        public float Health { get; set; }

        [Required]
        public int Color1 { get; set; }
        [Required]
        public int Color2 { get; set; }

        [Required]
        public string LicensePlate { get; set; }
    }
}
