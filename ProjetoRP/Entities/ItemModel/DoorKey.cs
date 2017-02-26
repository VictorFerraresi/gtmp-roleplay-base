using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Entities.ItemModel
{
    [Table("Items_DoorKeys")]
    public class DoorKey : Item
    {
        [Required]
        public int Door_Id { get; set; }
    }
}
