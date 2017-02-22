﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTANetworkServer;
using GTANetworkShared;

namespace ProjetoRP.Entities.Property
{
    public enum PropertyType
    {
        PROPERTY_TYPE_NULL = 0,
        PROPERTY_TYPE_HOUSE = 1,
        PROPERTY_TYPE_BUSINESS = 2,
        PROPERTY_TYPE_OFFICE = 3,
        PROPERTY_TYPE_ENTRANCE = 4
    }

    public abstract class Property
    {
        //[Key]
        //public virtual Property Prop { get; set; }

        [Key]
        public int Id { get; set; }

        [Required]
        public PropertyType Type { get; set; }

        [Required]
        public double X { get; set; }
        [Required]
        public double Y { get; set; }
        [Required]
        public double Z { get; set; }
        [Required]
        public int Dimension { get; set; }

        public string Address { get; set; }

        [NotMapped]
        public Marker Pickup { get; set; }
        [NotMapped]    
        public TextLabel TextLabel { get; set; }
    }
}