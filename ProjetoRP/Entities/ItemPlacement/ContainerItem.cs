using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Entities.ItemPlacement
{
    [Table("ContainerItems")]
    public class ContainerItem : Placement
    {
        public Item ParentItem { get; set; }
    }
}
