﻿using FinalProjectMVC.Areas.AdminPanel.Models;
using FinalProjectMVC.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProjectMVC.Areas.SellerPanel.Models
{
    public class Product
    {
        public int Id { get; set; }

        public int SerialNumber { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public byte[]? ProductImage { get; set; }

        [Required]
        [ForeignKey(nameof(SubCategory))]
        [DisplayName("Sub Category")]
        public int SubCategoryId { get; set; }

        // Navigation property for Category
        // Marked as virtual to enable lazy loading
        public virtual SubCategory? SubCategory { get; set; }

        [Required]
        [ForeignKey("Brand")]
        [DisplayName("Brand")]
        public int BrandId { get; set; }

        // Navigation property for Brand
        // Marked as virtual to enable lazy loading
        public virtual Brand? Brand { get; set; }

        public virtual ICollection<Review>? Reviews { get; set; }
        public virtual ICollection<SellerProduct>? SellerProducts { get; set; }
    }
}