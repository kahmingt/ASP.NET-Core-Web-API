using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Shared.Database.Entity
{
    public partial class Shippers
    {
        public Shippers()
        {
            Orders = new HashSet<Orders>();
        }

        [Key]
        [Column("ShipperID")]
        public int ShipperId { get; set; }
        [StringLength(40)]
        public string CompanyName { get; set; } = null!;
        [StringLength(24)]
        public string Phone { get; set; } = null!;
        public bool IsDeleted { get; set; }

        [InverseProperty("ShipViaNavigation")]
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
