using System;
using System.Collections.Generic;

namespace Masterpiece.Models;

public partial class Service
{
    public int ServiceId { get; set; }

    public string? ServiceName { get; set; }

    public string? ServiceDescription { get; set; }

    public string? ServiceImage { get; set; }
}
