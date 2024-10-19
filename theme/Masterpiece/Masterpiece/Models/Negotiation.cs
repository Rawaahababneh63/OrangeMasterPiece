using System;
using System.Collections.Generic;

namespace Masterpiece.Models;

public partial class Negotiation
{
    public int NegotiationId { get; set; }

    public int? ProductId { get; set; }

    public int? UserId { get; set; }

    public decimal? InitialOffer { get; set; }

    public decimal? FinalOffer { get; set; }

    public DateOnly? NegotiationDate { get; set; }

    public string? Status { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}
