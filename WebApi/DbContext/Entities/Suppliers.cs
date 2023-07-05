using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Data.Entities
{
    [Index("CompanyName", Name = "CompanyName")]
    [Index("PostalCode", Name = "PostalCode")]
    public partial class Suppliers
    {
        public Suppliers()
        {
            Products = new HashSet<Products>();
        }

        [Key]
        [Column("SupplierID")]
        public int SupplierId { get; set; }
        [StringLength(40)]
        public string CompanyName { get; set; } = null!;
        [StringLength(30)]
        public string ContactName { get; set; } = null!;
        [StringLength(30)]
        public string ContactTitle { get; set; } = null!;
        [StringLength(60)]
        public string Address { get; set; } = null!;
        [StringLength(15)]
        public string City { get; set; } = null!;
        [StringLength(15)]
        public string? Region { get; set; }
        [StringLength(10)]
        public string PostalCode { get; set; } = null!;
        [StringLength(15)]
        public string Country { get; set; } = null!;
        [StringLength(24)]
        public string Phone { get; set; } = null!;
        [StringLength(24)]
        public string? Fax { get; set; }
        [Column(TypeName = "ntext")]
        public string? HomePage { get; set; }
        public bool IsDeleted { get; set; }

        [InverseProperty("Supplier")]
        public virtual ICollection<Products> Products { get; set; }
    }
}
