using System.ComponentModel.DataAnnotations;
using WebApi.Area.Product.Utility;

namespace WebApi.Area.Product.Model
{
    public class ProductDetailCreateModel
    {
        [Required]
        [RegularExpression("^[0-9]*", ErrorMessage = "Category Id must be numeric.")]
        public int CategoryID { get; set; }

        [ValidateProductName]
        [RegularExpression("^[A-Za-z]", ErrorMessage = "Product name can contain only alphabets (A-Za-z).")]
        public string ProductName { get; set; }

        [Required]
        [RegularExpression("^[0-9]*", ErrorMessage = "Category Id must be numeric.")]
        public int SupplierID { get; set; }

        [Required]
        [RegularExpression("^\\d{0,}?$", ErrorMessage = "Unit in stock is invalid format.")]
        public short UnitsInStock { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        [RegularExpression("^\\d{0,}(\\.\\d{0,2})?$", ErrorMessage = "Unit price is invalid format.")]
        public decimal UnitPrice { get; set; }

    }
}