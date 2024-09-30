﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int BrandId { get; set; } //Foreign key for => ProductBrand
        public int CategoryId { get; set; } //Foreign key for => ProductCategory


        public ProductBrand Brand { get; set; } // Navigational Property [ONE] Between [Product , ProductBrand]
        public ProductCategory Category { get; set; } // Navigational Property [ONE] Between [Product , ProductCategory]
    }
}
