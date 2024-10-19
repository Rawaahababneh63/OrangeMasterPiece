using System;
using System.Collections.Generic;

namespace Masterpiece.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public decimal? Price { get; set; }

    public string? Color { get; set; }

    public int? ClothColorId { get; set; }

    public string? TypeProduct { get; set; }

    public string? Condition { get; set; }

    public string? Brand { get; set; }

    public decimal? PriceWithDiscount { get; set; }

    public DateOnly? Date { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDonation { get; set; }

    public int? SubcategoryId { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Color? ClothColor { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Negotiation> Negotiations { get; set; } = new List<Negotiation>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<SaleRequest> SaleRequests { get; set; } = new List<SaleRequest>();

    public virtual Subcategory? Subcategory { get; set; }
}
