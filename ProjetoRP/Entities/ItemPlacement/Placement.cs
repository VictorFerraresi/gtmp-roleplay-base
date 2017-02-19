using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Entities.ItemPlacement
{
    public abstract class Placement
    {
        [Key]
        public int Id { get; set; }
    }
}
