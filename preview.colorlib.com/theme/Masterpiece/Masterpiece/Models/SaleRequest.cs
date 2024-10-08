using System;
using System.Collections.Generic;

namespace Masterpiece.Models;

public partial class SaleRequest
{
    public int RequestId { get; set; }

    public int? UserId { get; set; }

    public int? ProductId { get; set; }

    public decimal? RequestedPrice { get; set; }

    public DateOnly? RequestDate { get; set; }

    public string? Status { get; set; }

    public int? AdminId { get; set; }

    public virtual Admin? Admin { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}
