﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace PetSiteAPI.Models.EFModels
{
    public partial class PurchaseItem
    {
        public int PurchasesItemId { get; set; }
        public int PurchaseId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public decimal Total { get; set; }

        public virtual Product Product { get; set; }
        public virtual Purchase Purchase { get; set; }
    }
}