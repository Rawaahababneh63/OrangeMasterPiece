using System;
using System.Collections.Generic;

namespace Masterpiece.Models;

public partial class ChatMessage
{
    public int Id { get; set; }

    public string? Sender { get; set; }

    public string? Recipient { get; set; }

    public string? MessageContent { get; set; }

    public DateTime? SentAt { get; set; }
}
