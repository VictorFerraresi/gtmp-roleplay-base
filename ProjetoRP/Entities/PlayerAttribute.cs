using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Entities
{
    public class PlayerAttribute
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public virtual Player Player { get; set; }

        [Required]
        public AttributeType Attribute { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
       
        public DateTime? ExpiresAt { get; set; }

        public enum AttributeType
        {
            Activated,
            Trusted,
            Tester,
            Admin,
            Owner,
            Banned
        }
    }
}
