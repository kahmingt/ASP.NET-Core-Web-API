using System.ComponentModel.DataAnnotations;

namespace WebApi.Area.Product.Model
{
    public class ProductDetailUpdateModel
    {
        [Required]
        [RegularExpression("^[0-9]*", ErrorMessage = "Category Id must be numeric.")]
        public int CategoryID { get; set; }

        [Required]
        [RegularExpression("^[0-9]*", ErrorMessage = "Product Id must be numeric.")]
        public int ProductID { get; set; }

        [Display(Name = "Product")]
        [RegularExpression("^[A-Za-z]", ErrorMessage = "Product name can contain only alphabets (A-Za-z).")]
        public string ProductName { get; set; }

        [Required]
        [RegularExpression("^[0-9]*", ErrorMessage = "Supplier Id must be numeric.")]
        public int SupplierID { get; set; }

        [Required]
        [RegularExpression("^\\d{0,}?$", ErrorMessage = "Unit in stock format is invalid.")]
        public short UnitsInStock { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        [RegularExpression("^\\d{0,}(\\.\\d{0,2})?$", ErrorMessage = "Unit price is invalid format.")]
        public decimal UnitPrice { get; set; }

    }
}