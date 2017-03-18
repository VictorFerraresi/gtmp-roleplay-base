using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Entities
{
    public abstract class Item
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(32), Required]
        public string Variation { get; set; }
        public virtual ItemPlacement.Placement Placement { get; set; }

        public Item()
        {
            Variation = "default";
        }
    }
}
