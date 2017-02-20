using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTANetworkServer;

namespace ProjetoRP.Entities.Property
{
    [Table("Businesses")]
    public class Business : Property
    {
        [Key]
        public int Id { get; set; }

        public virtual Character Owner { get; set; }
    }
}