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

        public int? Placement_Id { get; set; }

        [ForeignKey("Placement_Id")]
        public ItemPlacement.Placement Placement { get; set; }
    }
}
