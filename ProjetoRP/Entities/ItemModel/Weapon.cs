using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Entities.ItemModel
{
    [Table("Weapons")]
    public class Weapon : Item
    {
        public int Bullets { get; set; }
    }
}
