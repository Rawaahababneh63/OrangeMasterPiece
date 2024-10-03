using System;
using System.Collections.Generic;

namespace Masterpiece.Models;

public partial class BeneficiaryTransaction
{
    public int TransactionId { get; set; }

    public int? OrganizationId { get; set; }

    public decimal? SaleAmount { get; set; }

    public DateOnly? TransactionDate { get; set; }

    public string? Status { get; set; }

    public virtual Organization? Organization { get; set; }
}
