using System;
using System.Collections.Generic;

namespace Masterpiece.Models;

public partial class Organization
{
    public int OrganizationId { get; set; }

    public string? OrganizationName { get; set; }

    public string? ContactInfo { get; set; }

    public string? BankAccount { get; set; }

    public virtual ICollection<BeneficiaryTransaction> BeneficiaryTransactions { get; set; } = new List<BeneficiaryTransaction>();
}
