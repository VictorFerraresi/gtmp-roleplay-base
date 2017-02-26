using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Entities.ItemModel
{
    [Table("Items_Pistols")]
    public class Pistol : Item
    {
        [Required]
        public int Bullets { get; set; }
    }
}
