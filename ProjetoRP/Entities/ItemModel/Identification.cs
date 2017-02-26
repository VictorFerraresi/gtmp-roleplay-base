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
    [Table("Items_Identifications")]
    public class Identification : Item
    {
        [MaxLength(64)]
        public string Serial { get; set; }
        [MaxLength(64)]
        public string FirstName { get; set; }
        [MaxLength(64)]
        public string LastName { get; set; }
        [MaxLength(128)]
        public string Address { get; set; }
        [MaxLength(12)] // "01/JAN/2017"
        public string DOB { get; set; }
        [MaxLength(12)]
        public string EXP { get; set; }
        public bool Donor { get; set; }

        public Gender Gender { get; set; }
        public HairColor Hair { get; set; }
        public EyeColor Eyes { get; set; }

        public float Height { get; set; }
        public float Weight { get; set; }
    }
}
