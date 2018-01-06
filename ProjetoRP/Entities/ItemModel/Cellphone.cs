using ProjetoRP.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Entities.ItemModel
{
    [Table("Cellphone")]
    public class Cellphone : Item
    {
        [MaxLength(64)]
        public string Model { get; set; }

        public int Number { get; set; }
        public bool TurnedOn { get; set; }
    }
}
