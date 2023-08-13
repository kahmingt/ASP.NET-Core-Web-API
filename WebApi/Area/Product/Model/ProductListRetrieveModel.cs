﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebApi.Area.Product.Model
{
    public class ProductListRetrieveModel
    {
        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        public int ProductID { get; set; }

        [Display(Name = "Product")]
        public string ProductName { get; set; }

        [Display(Name = "Units In Stock")]
        public short UnitsInStock { get; set; }

        [Display(Name = "Unit Price")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal UnitPrice { get; set; }
    }
}