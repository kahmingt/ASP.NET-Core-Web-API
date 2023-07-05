﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Data.Entities
{
    [Index("CustomerId", Name = "CustomerID")]
    [Index("CustomerId", Name = "CustomersOrders")]
    [Index("EmployeeId", Name = "EmployeeID")]
    [Index("EmployeeId", Name = "EmployeesOrders")]
    [Index("OrderDate", Name = "OrderDate")]
    [Index("ShipPostalCode", Name = "ShipPostalCode")]
    [Index("ShippedDate", Name = "ShippedDate")]
    [Index("ShipVia", Name = "ShippersOrders")]
    public partial class Orders
    {
        public Orders()
        {
            OrderDetails = new HashSet<OrderDetails>();
        }

        [Key]
        [Column("OrderID")]
        public int OrderId { get; set; }
        [Column("CustomerID")]
        [StringLength(5)]
        public string CustomerId { get; set; } = null!;
        [Column("EmployeeID")]
        public int EmployeeId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime OrderDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? RequiredDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ShippedDate { get; set; }
        public int? ShipVia { get; set; }
        [Column(TypeName = "money")]
        public decimal? Freight { get; set; }
        [StringLength(40)]
        public string? ShipName { get; set; }
        [StringLength(60)]
        public string? ShipAddress { get; set; }
        [StringLength(15)]
        public string? ShipCity { get; set; }
        [StringLength(15)]
        public string? ShipRegion { get; set; }
        [StringLength(10)]
        public string? ShipPostalCode { get; set; }
        [StringLength(15)]
        public string? ShipCountry { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("Orders")]
        public virtual Customers Customer { get; set; } = null!;
        [ForeignKey("EmployeeId")]
        [InverseProperty("Orders")]
        public virtual Employees Employee { get; set; } = null!;
        [ForeignKey("ShipVia")]
        [InverseProperty("Orders")]
        public virtual Shippers? ShipViaNavigation { get; set; }
        [InverseProperty("Order")]
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
