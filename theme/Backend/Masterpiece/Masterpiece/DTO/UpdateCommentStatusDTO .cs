namespace Masterpiece.DTO
{
    public class UpdateCommentStatusDTO
    {
        public int CommentId { get; set; }
        public string NewStatus { get; set; } // "approved" أو "rejected"
    }

}
