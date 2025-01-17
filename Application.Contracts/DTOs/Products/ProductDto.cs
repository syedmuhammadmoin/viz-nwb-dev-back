﻿using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public ProductType ProductType { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CostAccountId { get; set; }
        public string InventoryAccountId { get; set; }
        public string RevenueAccountId { get; set; }
        public int UnitOfMeasurementId { get; set; }
        public string UnitOfMeasurementName { get; set; }
        public decimal SalesPrice { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalesTax { get; set; }
        public string Barcode { get; set; }
        public bool IsFixedAsset { get; set; }
    }
}
