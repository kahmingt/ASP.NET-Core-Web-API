using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Database.Entities
{
    [Index("CategoryId", Name = "CategoriesProducts")]
    [Index("CategoryId", Name = "CategoryID")]
    [Index("ProductName", Name = "ProductName")]
    [Index("SupplierId", Name = "SupplierID")]
    [Index("SupplierId", Name = "SuppliersProducts")]
    public partial class Products
    {
        public Products()
        {
            OrderDetails = new HashSet<OrderDetails>();
        }

        [Key]
        [Column("ProductID")]
        public int ProductId { get; set; }
        [StringLength(40)]
        public string ProductName { get; set; } = null!;
        [Column("SupplierID")]
        public int? SupplierId { get; set; }
        [Column("CategoryID")]
        public int CategoryId { get; set; }
        [StringLength(20)]
        public string? QuantityPerUnit { get; set; }
        [Column(TypeName = "money")]
        public decimal? UnitPrice { get; set; }
        public short UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }
        public bool? Discontinued { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("CategoryId")]
        [InverseProperty("Products")]
        public virtual Categories Category { get; set; } = null!;
        [ForeignKey("SupplierId")]
        [InverseProperty("Products")]
        public virtual Suppliers? Supplier { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
