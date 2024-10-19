using System.ComponentModel.DataAnnotations;

namespace Masterpiece.DTO
{
    public class ChangeStatusRequest
    {
        [Required(ErrorMessage = "The Status field is required.")]
        public string Status { get; set; }

        public string RejectionReason { get; set; } // يمكن أن يكون فارغًا في حالة الموافقة
    }

}
