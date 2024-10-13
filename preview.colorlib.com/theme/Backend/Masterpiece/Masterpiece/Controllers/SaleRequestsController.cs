using Masterpiece.DTO;
using Masterpiece.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Masterpiece.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleRequestsController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly MyDbContext _db;

        public SaleRequestsController(MyDbContext db, IEmailService emailService)
        {
            _db = db;
            _emailService = emailService;
        }



        [HttpPost("submit-sale-request")]
        public async Task<IActionResult> SubmitSaleRequest([FromForm] SaleRequestDto requestDto)
        {
            // التحقق من صحة النموذج
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "بيانات غير صالحة", errors = ModelState });
            }

            // تحقق من وجود الفئة الفرعية
            var subcategory = await _db.Subcategories
                .Include(sc => sc.Category)  // تأكد من تضمين الفئة الرئيسية
                .FirstOrDefaultAsync(sc => sc.SubcategoryId == requestDto.SubcategoryId);

            if (subcategory == null || subcategory.CategoryId != requestDto.MainCategoryId)
            {
                return BadRequest(new { message = "الفئة الفرعية المختارة لا تتبع الفئة الرئيسية المحددة." });
            }

            // إنشاء كائن المنتج
            var product = new Product
            {
                Name = requestDto.ProductName,
                Description = requestDto.Description,
                Price = requestDto.ExpectedPrice,
                Color = requestDto.Color,
                SubcategoryId = requestDto.SubcategoryId,
                Condition = requestDto.Condition,
                IsDonation = requestDto.ActionType == "donation",
            };

            // تحديد مسار تحميل الصور
            var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }

            // معالجة الصورة
            if (requestDto.ProductImage != null && requestDto.ProductImage.Length > 0)
            {
                var fileExtension = Path.GetExtension(requestDto.ProductImage.FileName);
                var uniqueFileName = Guid.NewGuid().ToString() + fileExtension; // اسم فريد للصورة
                var imageFilePath = Path.Combine(imagesFolder, uniqueFileName);

                using (var stream = new FileStream(imageFilePath, FileMode.Create))
                {
                    await requestDto.ProductImage.CopyToAsync(stream);  // استخدام await هنا
                }

                product.Image = uniqueFileName; // حفظ اسم الملف الفريد في كائن المنتج
            }

            // إنشاء طلب البيع
            var saleRequest = new SaleRequest
            {
                UserId = requestDto.UserId,
                Product = product,
                RequestedPrice = requestDto.ExpectedPrice,
                RequestDate = DateOnly.FromDateTime(DateTime.Now),
                Status = "Pending"
            };

            _db.SaleRequests.Add(saleRequest);
            await _db.SaveChangesAsync();

            return Ok(new { message = "تم تقديم الطلب بنجاح! ستتلقى بريدًا إلكترونيًا بمجرد الموافقة أو الرفض.", saleRequestId = saleRequest.RequestId });
        }




        // GET: api/salerequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAllSaleRequests()
        {
            var saleRequests = await _db.SaleRequests
                .Include(s => s.User)  // تضمين بيانات المستخدم
                .Select(s => new
                {
                    s.RequestId,  // رقم الطلب
                    s.RequestedPrice,  // السعر المطلوب
                    s.RequestDate,  // تاريخ الطلب
                    s.Status,  // حالة الطلب
                    UserName = s.User.UserName,  // اسم المستخدم
                    UserEmail = s.User.Email,  // البريد الإلكتروني للمستخدم
                    UserPhone = s.User.PhoneNumber,  // رقم الهاتف
                    UserAddress = s.User.Address,  // العنوان
                    ProductName = s.Product.Name,  // اسم المنتج
                    ProductDescription = s.Product.Description  // وصف المنتج
                })
                .ToListAsync();

            if (saleRequests == null || !saleRequests.Any())
            {
                return NotFound(new { message = "لا توجد طلبات بيع." });
            }

            return Ok(saleRequests);
        }



        [HttpPut("{id}/change-status")]
        public IActionResult ChangeStatus(int id, [FromBody] ChangeStatusRequest request)
        {
            var saleRequest = _db.SaleRequests.Include(sr => sr.User).FirstOrDefault(sr => sr.RequestId == id);
            if (saleRequest == null)
            {
                return NotFound(new { message = "طلب البيع غير موجود." });
            }

            if (request.Status.Equals("approve", StringComparison.OrdinalIgnoreCase))
            {
                saleRequest.Status = "Approved";
                SendEmailNotification(saleRequest.User.Email, saleRequest.User.UserName, true);
            }
            else if (request.Status.Equals("reject", StringComparison.OrdinalIgnoreCase))
            {
                saleRequest.Status = "Rejected";
                // يمكنك استخدام request.RejectionReason هنا إذا لزم الأمر
                SendEmailNotification(saleRequest.User.Email, saleRequest.User.UserName, false);
            }
            else
            {
                return BadRequest(new { message = "حالة غير صالحة. استخدم 'approve' أو 'reject'." });
            }

            _db.SaveChanges();
            return NoContent();
        }

    

        private void SendEmailNotification(string email, string userName, bool isApproved)
        {
            var subject = isApproved ? "إشعار بالموافقة" : "إشعار بالرفض";
            var message = isApproved
                ? $"عزيزي/عزيزتي {userName},\n\nنحن سعيدون أن نخبرك بأنه تم الموافقة على طلبك لبيع المنتج! 🎉 يمكنك الآن مشاهدة منتجك على الموقع، وعند بيعه، ستتلقى إشعارًا بذلك. شكرًا لكونك جزءًا من مجتمعنا!"
                : $"عزيزي/عزيزتي {userName},\n\nنأسف لإخبارك بأن طلبك لبيع المنتج لم يتم قبوله. لا تتردد في تقديم طلب آخر في المستقبل. نحن هنا لدعمك!";

            _emailService.SendEmail(email, subject, message);
        }








        //[HttpPut("{id}")]
        //public IActionResult ChangeStatus(int id, [FromForm] ChangeStatusRequest request)
        //{
        //    var saleRequest = _db.SaleRequests.Include(sr => sr.User).FirstOrDefault(sr => sr.RequestId == id);
        //    if (saleRequest == null)
        //    {
        //        return NotFound(new { message = "طلب البيع غير موجود." });
        //    }

        //    if (request.Status.Equals("approve", StringComparison.OrdinalIgnoreCase))
        //    {
        //        saleRequest.Status = "Approved";
        //        SendEmailNotification(saleRequest.User.Email, saleRequest.User.UserName, true, null);
        //    }
        //    else if (request.Status.Equals("reject", StringComparison.OrdinalIgnoreCase))
        //    {
        //        saleRequest.Status = "Rejected";
        //        SendEmailNotification(saleRequest.User.Email, saleRequest.User.UserName, false, request.RejectionReason);
        //    }
        //    else
        //    {
        //        return BadRequest(new { message = "حالة غير صالحة. استخدم 'approve' أو 'reject'." });
        //    }

        //    _db.SaveChanges();
        //    return NoContent();
        //}

        //private void SendEmailNotification(string email, string userName, bool isApproved, string rejectionReason)
        //{
        //    var subject = isApproved ? "إشعار بالموافقة" : "إشعار بالرفض";
        //    var message = isApproved
        //        ? $"عزيزي/عزيزتي {userName},\n\nنحن سعيدون أن نخبرك بأنه تم الموافقة على طلبك لبيع المنتج! 🎉 يمكنك الآن مشاهدة منتجك على الموقع، وعند بيعه، ستتلقى إشعارًا بذلك. شكرًا لكونك جزءًا من مجتمعنا!"
        //        : $"عزيزي/عزيزتي {userName},\n\nنأسف لإخبارك بأن طلبك لبيع المنتج لم يتم قبوله. السبب: {rejectionReason}.\nلا تتردد في تقديم طلب آخر في المستقبل. نحن هنا لدعمك!";

        //    _emailService.SendEmail(email, subject, message);
        //}








        // DELETE: api/salerequests/5
        [HttpDelete("{id}")]
        public IActionResult DeleteSaleRequest(int id)
        {
            var saleRequest = _db.SaleRequests.Find(id);
            if (saleRequest == null)
            {
                return NotFound(); // إذا لم يتم العثور على الطلب، أرجع حالة 404
            }

            _db.SaleRequests.Remove(saleRequest); // إزالة الطلب من قاعدة البيانات
            _db.SaveChanges(); // حفظ التغييرات

            return NoContent(); // أرجع حالة 204 (لا يوجد محتوى) بعد الحذف
        }

    }
}
