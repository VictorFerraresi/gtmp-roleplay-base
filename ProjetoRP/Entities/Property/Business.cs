using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoRP.Entities.Property
{
    public enum BusinessType
    {
        BUSINESS_TYPE_GENERIC = 0,
        BUSINESS_TYPE_GENERALSTORE = 1
    }

    [Table("Businesses")]
    public class Business : Property
    {
        public int? Owner_Id { get; set; }

        [ForeignKey("Owner_Id")]
        public Character Owner { get; set; }

        public BusinessType BizType { get; set; }
    }
}