using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Entities
{
    [Table("players")]
    public class Player
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        [MaxLength(255)]
        public string Email { get; set; }
    }
}
