using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Entities.ItemPlacement
{
    [Table("Placements_ContainerItems")]
    public class ContainerItem : Placement
    {
        [Required]
        public int? ParentItem_Id { get; set; }
        [ForeignKey("ParentItem_Id")]
        public Item ParentItem { get; set; }

        [Required]
        public int Slot { get; set; }
    }
}
