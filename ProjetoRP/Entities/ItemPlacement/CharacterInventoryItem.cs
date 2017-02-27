using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Entities.ItemPlacement
{
    [Table("Placements_CharacterInventoryItems")]
    public class CharacterInventoryItem : Placement
    {
        [Required]
        public int? Character_Id { get; set; }

        [ForeignKey("Character_Id")]
        public Character Character { get; set; }

        [Required]
        public Types.EquipSlot Slot { get; set; }
    }
}
