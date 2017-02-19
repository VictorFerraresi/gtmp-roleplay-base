using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Entities.ItemPlacement
{
    [Table("TrunkItems")]
    public class Trunk : Placement
    {
        // public Vehicle ParentVehicle { get; set; }
    }
}
