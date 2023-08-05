using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Database.Entities
{
    [Index("City", Name = "City")]
    [Index("CompanyName", Name = "CompanyName")]
    [Index("PostalCode", Name = "PostalCode")]
    [Index("Region", Name = "Region")]
    public partial class Customers
    {
        public Customers()
        {
            Orders = new HashSet<Orders>();
        }

        [Key]
        [Column("CustomerID")]
        [StringLength(5)]
        public string CustomerId { get; set; } = null!;
        [StringLength(40)]
        public string CompanyName { get; set; } = null!;
        [StringLength(30)]
        public string? ContactName { get; set; }
        [StringLength(30)]
        public string? ContactTitle { get; set; }
        [StringLength(60)]
        public string? Address { get; set; }
        [StringLength(15)]
        public string? City { get; set; }
        [StringLength(15)]
        public string? Region { get; set; }
        [StringLength(10)]
        public string? PostalCode { get; set; }
        [StringLength(15)]
        public string? Country { get; set; }
        [StringLength(24)]
        public string? Phone { get; set; }
        [StringLength(24)]
        public string? Fax { get; set; }
        public bool IsDeleted { get; set; }
        [StringLength(450)]
        public string? AspNetUsersId { get; set; }

        [InverseProperty("Customer")]
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
