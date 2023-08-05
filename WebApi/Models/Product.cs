using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class ProductListing
    {
        public string CategoryName { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public short UnitsInStock { get; set; }
    }

    public class ProductDetails
    {
        [Required]
        public int CategoryID { get; set; }

        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        [Required]
        public int ProductID { get; set; }

        [Display(Name = "Product")]
        public string ProductName { get; set; }

        [Required]
        public int SupplierID { get; set; }

        [Display(Name = "Supplier")]
        public string SupplierName { get; set; }

        [Required]
        [Display(Name = "Units In Stock")]
        public short UnitsInStock { get; set; }

        [Required]
        [Display(Name = "Unit Price")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal UnitPrice { get; set; }

    }
}
