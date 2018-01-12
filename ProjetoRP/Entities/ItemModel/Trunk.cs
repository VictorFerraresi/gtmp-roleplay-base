using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Entities.ItemModel
{
    [Table("Items_TrunkContainers")]
    public class TrunkContainer : Item
    {
        [MaxLength(64)]
        public string VehicleModel { get; set; }

        public int Slots { get; set; }        
    }
}
