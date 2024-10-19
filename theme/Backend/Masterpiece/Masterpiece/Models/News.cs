using System;
using System.Collections.Generic;

namespace Masterpiece.Models;

public partial class News
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? ShortDescription { get; set; }

    public string? FullDescription { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime? PublishedDate { get; set; }

    public string? Link { get; set; }
}
