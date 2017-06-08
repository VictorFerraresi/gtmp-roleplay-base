using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Entities.Property
{
    [Table("Houses")]
    public class House : Property
    {
        public int? Owner_Id { get; set; }

        [ForeignKey("Owner_Id")]
        public Character Owner { get; set; }
    }
}