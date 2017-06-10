using System.ComponentModel.DataAnnotations.Schema;

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