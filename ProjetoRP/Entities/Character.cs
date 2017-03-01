using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoRP.Entities
{
    public class Character
    {
        public Character()
        {
            Cash = Int32.Parse(Configurations.Character.Default_Cash);
            Bank = Int32.Parse(Configurations.Character.Default_Bank);
            Savings = Int32.Parse(Configurations.Character.Default_Savings);

            X = Double.Parse(Configurations.Character.Default_X);
            Y = Double.Parse(Configurations.Character.Default_Y);
            Z = Double.Parse(Configurations.Character.Default_Z);
            Dimension = Int32.Parse(Configurations.Character.Default_Dimension);

            Xp = Double.Parse(Configurations.Character.Default_Xp);
            Level = Int32.Parse(Configurations.Character.Default_Level);
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(5), MaxLength(32)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        public int PlayerId { get; set; }
        [ForeignKey("PlayerId")]
        [Required]
        public Player Player { get; set; }

        [Required]
        public int Cash { get; set; }
        [Required]
        public int Bank { get; set; }
        [Required]
        public int Savings { get; set; }

        [Required]
        [MaxLength(32)]
        public string Skin { get; set; }

        [Required]
        public double X { get; set; }
        [Required]
        public double Y { get; set; }
        [Required]
        public double Z { get; set; }
        [Required]
        public int Dimension { get; set; }

        [Required]
        public double Xp { get; set; }
        [Required]
        public int Level { get; set; }

        public int? Faction_Id { get; set; }
        [ForeignKey("Faction_Id")]        
        public Faction.Faction Faction { get; set; }

        public int? Rank_Id { get; set; }
        [ForeignKey("Rank_Id")]
        public Faction.Rank Rank { get; set; }
    }
}
