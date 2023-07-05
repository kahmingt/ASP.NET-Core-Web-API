using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Data.Entities
{
    [Index("OrderId", Name = "OrderID")]
    [Index("OrderId", Name = "OrdersOrder_Details")]
    [Index("ProductId", Name = "ProductID")]
    [Index("ProductId", Name = "ProductsOrder_Details")]
    public partial class OrderDetails
    {
        [Key]
        [Column("OrderID")]
        public int OrderId { get; set; }
        [Key]
        [Column("ProductID")]
        public int ProductId { get; set; }
        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float? Discount { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("OrderId")]
        [InverseProperty("OrderDetails")]
        public virtual Orders Order { get; set; } = null!;
        [ForeignKey("ProductId")]
        [InverseProperty("OrderDetails")]
        public virtual Products Product { get; set; } = null!;
    }
}
