﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRP.Entities.Property
{
    interface IProperty<Property>
    {
        void DrawPickup(Property p);
    }
}