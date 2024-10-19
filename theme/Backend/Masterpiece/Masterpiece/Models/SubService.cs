using System;
using System.Collections.Generic;

namespace Masterpiece.Models;

public partial class SubService
{
    public int SubServiceId { get; set; }

    public string SubServiceName { get; set; } = null!;

    public string? SubServiceDescription { get; set; }

    public string? SubServiceImage { get; set; }

    public int ServiceId { get; set; }
}
