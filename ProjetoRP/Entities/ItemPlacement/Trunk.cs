using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Entities.ItemPlacement
{
    [Table("Placements_TrunkItems")]
    public class Trunk : Placement
    {
        [Required]
        public int? ParentVehicle_Id { get; set; }
        [ForeignKey("ParentVehicle_Id")]
        public Entities.Vehicle.Vehicle ParentVehicle { get; set; }
    }
}
