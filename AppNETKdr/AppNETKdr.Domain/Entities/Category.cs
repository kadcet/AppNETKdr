﻿using AppNETKdr.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNETKdr.Domain.Entities
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }
       // public List<Product> Products { get; set; }
    }
}
