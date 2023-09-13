using System.ComponentModel.DataAnnotations;
using WebApi.Area.Product.Utility;

namespace WebApi.Area.Product.Model
{
    public class ProductDetailCreateModel
    {
        [Required]
        [ValidateUniqueIdentifier]
        public int CategoryID { get; set; }

        [Required]
        [ValidateProductName]
        public string ProductName { get; set; }

        [Required]
        [ValidateUniqueIdentifier]
        public int SupplierID { get; set; }

        [Required]
        [ValidateUnitInStock]
        public short UnitsInStock { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        [ValidateUnitPrice]
        public decimal UnitPrice { get; set; }

    }
}