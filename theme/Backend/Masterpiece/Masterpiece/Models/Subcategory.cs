using System;
using System.Collections.Generic;

namespace Masterpiece.Models;

public partial class Subcategory
{
    public int SubcategoryId { get; set; }

    public string? SubcategoryName { get; set; }

    public int? CategoryId { get; set; }

    public string? Image { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
