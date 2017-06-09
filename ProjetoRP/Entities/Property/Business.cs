using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoRP.Entities.Property
{
    [Table("Businesses")]
    public class Business : Property
    {
        public int? Owner_Id { get; set; }

        [ForeignKey("Owner_Id")]
        public Character Owner { get; set; }
    }
}