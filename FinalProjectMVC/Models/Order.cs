﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProjectMVC.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        [ForeignKey("Customer")]
        public required string CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal TotalPrice { get; set; }

        [Required, DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; }

        [ForeignKey("Address")]
        public int AddressId { get; set; }

        public virtual Address? Address { get; set; }
        public virtual ICollection<OrderItem>? OrderItems { get; set; }
    }
}