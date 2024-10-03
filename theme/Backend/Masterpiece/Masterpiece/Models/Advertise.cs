using System;
using System.Collections.Generic;

namespace Masterpiece.Models;

public partial class Advertise
{
    public int AdvertiseId { get; set; }

    public string? AdvertiserName { get; set; }

    public string? AdTitle { get; set; }

    public string? AdDescription { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public decimal? Budget { get; set; }

    public string? Status { get; set; }

    public string? AdvertiserEmail { get; set; }
}
