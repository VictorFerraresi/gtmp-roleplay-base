using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoRP.Entities.Property
{

    public enum BusinessType
    {
        BUSINESS_TYPE_NULL = 0,
        BUSINESS_TYPE_STORE,
        BUSINESS_TYPE_FOOD,        
        BUSINESS_TYPE_GAS,
        BUSINESS_TYPE_BAR,
        BUSINESS_TYPE_NIGHTCLUB,
        BUSINESS_TYPE_BARBER,
        BUSINESS_TYPE_WORKSHOP,
        BUSINESS_TYPE_ELECTRONICS,
        BUSINESS_TYPE_HARDWARE,
        BUSINESS_TYPE_CLOTHING,
        BUSINESS_TYPE_PHARMACY,
        BUSINESS_TYPE_GUNSHOP,
        BUSINESS_TYPE_DEALERSHIP,
        BUSINESS_TYPE_TATTOO
    }


    [Table("Businesses")]
    public class Business : Property
    {
        public int? Owner_Id { get; set; }

        [ForeignKey("Owner_Id")]
        public Character Owner { get; set; }

        [Required]
        [MinLength(3), MaxLength(40)]
        public string Name { get; set; }

        [Required]        
        public BusinessType BusinessType { get; set; }
    }
}