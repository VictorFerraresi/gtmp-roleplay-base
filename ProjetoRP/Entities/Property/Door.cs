using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Entities.Property
{
    public class Door
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Property Property { get; set; }

        public long Model { get; set; }

        [Required]
        public bool Locked { get; set; }

        [Required]
        public double ExteriorX { get; set; }
        [Required]
        public double ExteriorY { get; set; }
        [Required]
        public double ExteriorZ { get; set; }
        [Required]
        public int ExteriorDimension { get; set; }

        [Required]
        public double InteriorX { get; set; }
        [Required]
        public double InteriorY { get; set; }
        [Required]
        public double InteriorZ { get; set; }
        [Required]
        public int InteriorDimension { get; set; }
    }
}
