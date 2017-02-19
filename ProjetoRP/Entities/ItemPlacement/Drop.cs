using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Entities.ItemPlacement
{
    [Table("DropItems")]
    public class Drop : Placement
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public int Dimension { get; set; }
    }
}
