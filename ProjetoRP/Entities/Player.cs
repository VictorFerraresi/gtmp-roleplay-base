using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Entities
{
    public class Player
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3), MaxLength(32)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        [Required]
        [MaxLength(60)]
        public string Password { get; set; }

        [Required]
        [MaxLength(255)]
        [Index(IsUnique = true)]
        public string Email { get; set; }

        virtual public ICollection<Character> Characters { get; set; }
        virtual public ICollection<PlayerAttribute> PlayerAttributes { get; set; }
    }
}