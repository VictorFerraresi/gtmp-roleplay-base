using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Entities.ItemModel
{
    [Table("Items_CarKeys")]
    public class CarKey : Item
    {
        [Required]
        public int Vehicle_Id { get; set; }

        [ForeignKey("Vehicle_Id")]
        public Vehicle.Vehicle Vehicle { get; set; }
    }
}
