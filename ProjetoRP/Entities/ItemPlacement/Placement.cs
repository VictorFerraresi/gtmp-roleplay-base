using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Entities.ItemPlacement
{
    public abstract class Placement
    {
        [Key]
        public int Item_Id { get; set; }

        [ForeignKey("Item_Id")]
        [Required]
        public virtual Item Item { get; set; }
    }
}
