﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace PetSiteAPI.Models.EFModels
{
    public partial class ProductName
    {
        public ProductName()
        {
            ProductImages = new HashSet<ProductImage>();
            Products = new HashSet<Product>();
        }

        public int ProductNameId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}