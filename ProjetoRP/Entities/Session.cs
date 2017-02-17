using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Entities
{
    public class Session
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public virtual Player Player { get; set; }
        public virtual Character Character { get; set; }

        [Required]
        public bool Failed { get; set; }

        [Required]
        [MaxLength(45)]
        public string Ip { get; set; }

        [Required]
        public DateTime LoginAt { get; set; }
        public DateTime LogoutAt { get; set; }

        public virtual Session ParentSession { get; set; }

        [MaxLength(64)]
        public string Rgsc { get; set; }
    }
}
