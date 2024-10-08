using Masterpiece.DTO;
using Masterpiece.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Masterpiece.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {

        private readonly MyDbContext _db;

        public CommentController(MyDbContext db)
        {
            _db = db;
        }


        [HttpGet("GetProductRating/{productId:int}")]
        public async Task<IActionResult> GetProductRating(int productId)
        {
            // جلب التقييمات الخاصة بالمنتج
            var ratings = await _db.Comments
                                    .Where(c => c.ProductId == productId && c.Status == "approved")
                                    .Select(c => c.Rating)
                                    .ToListAsync();

            // حساب متوسط التقييم
            if (ratings.Count == 0)
            {
                return Ok(0); // إذا لم يكن هناك تقييمات
            }

            var averageRating = ratings.Average();

            return Ok(averageRating);
        }



        [HttpGet("GetComments/{productId:int}")]
        public async Task<IActionResult> GetCommentsUser(int productId)
        {
            var comments = await _db.Comments
                                           .Where(c => c.ProductId == productId && c.Status == "approved")
                                           .Include(c => c.User)
                                           .ToListAsync();

            var commentDTOs = comments.Select(c => new CommentDTO
            {
                CommentId = c.CommentId,
                Comment1 = c.Comment1,
                Rating = c.Rating ?? 0,
                Date = c.Date ?? DateOnly.FromDateTime(DateTime.Now),
                UserName = c.User?.UserName?? "Anonymous"
            }).ToList();

            return Ok(commentDTOs);
        }
        [HttpPost("AddComment")]
        public async Task<IActionResult> AddComment([FromBody] Comment comment)
        {
            // التحقق من وجود تقييم سابق
            var existingComment = await _db.Comments
                .Where(c => c.ProductId == comment.ProductId && c.UserId == comment.UserId)
                .FirstOrDefaultAsync();

            if (existingComment != null)
            {
                return BadRequest("لقد قمت بالفعل بتقييم هذا المنتج.");
            }

            comment.Status = "pending";
            comment.Date = DateOnly.FromDateTime(DateTime.Now);

            _db.Comments.Add(comment);
            await _db.SaveChangesAsync();

            return Ok("تم إرسال التقييم بنجاح. سيتم نشره بمجرد الموافقة عليه.");
        }


        // API للموافقة أو الرفض
        [HttpPost("UpdateCommentStatus")]
        public async Task<IActionResult> UpdateCommentStatus([FromBody] UpdateCommentStatusDTO updateComment)
        {
            var comment = await _db.Comments.FindAsync(updateComment.CommentId);
            if (comment == null)
            {
                return NotFound("التعليق غير موجود.");
            }

            comment.Status = updateComment.NewStatus;
            await _db.SaveChangesAsync();

            return Ok($"تم تحديث حالة التعليق إلى {updateComment.NewStatus}.");
        }

        ///جلب التعليقات المعلقة
        [HttpGet("GetPendingComments")]
        public async Task<IActionResult> GetPendingComments()
        {
            var pendingComments = await _db.Comments
                                           .Where(c => c.Status == "pending")
                                           .Include(c => c.User)
                                           .ToListAsync();

            var commentDTOs = pendingComments.Select(c => new CommentDTO
            {
                CommentId = c.CommentId,
                Comment1 = c.Comment1,
                Rating = c.Rating ?? 0,
                Date = c.Date ?? DateOnly.FromDateTime(DateTime.Now),
                UserName = c.User?.UserName ?? "Anonymous"
            }).ToList();

            return Ok(commentDTOs);
        }

    


    }
}

