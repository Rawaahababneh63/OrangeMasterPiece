using System.ComponentModel.DataAnnotations;

namespace Masterpiece.DTO
{
    public class StatusChangeRequest
    {
        [Required]
        public string Status { get; set; }

        public string? RejectionReason { get; set; } // Allow null values
    }




}
