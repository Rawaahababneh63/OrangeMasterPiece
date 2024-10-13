using Masterpiece.DTO;
using Masterpiece.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Masterpiece.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
          private readonly MyDbContext _db;
            private readonly IEmailService _emailService;

            public VoucherController(MyDbContext db, IEmailService emailService)
            {
                _db = db;
                _emailService = emailService;
            }
        //هاي مشات


        //  ارسال البريد الالكتروني بشكل يدوي من قبل الادمن بالقسيمة يلي بدي ابعتها 
        [HttpPost("send-voucher/{userEmail}")]
        public async Task<IActionResult> SendVoucherToUser(string userEmail, string voucherCode)
        {
            // تحقق من وجود القسيمة
            var existingVoucher = _db.Vouchers.FirstOrDefault(v => v.Code == voucherCode);
            if (existingVoucher == null)
            {
                return NotFound("Voucher not found.");
            }

            // إعداد رسالة البريد الإلكتروني
            var subject = "Your Voucher Code";
            var body = $"Here is your voucher code: {existingVoucher.Code} with a discount of {existingVoucher.DiscountAmount}. " +
                       $"The voucher expires on {existingVoucher.ExpiryDate}.";

            // إرسال البريد الإلكتروني
            _emailService.SendEmail(userEmail, subject, body);

            return Ok("Voucher sent to user.");
        }


        [HttpPut("UpdateVoucher/{id}")]
        public IActionResult UpdateVoucher(int id, [FromForm] VoucherRequestDto request)
        {
            var voucher = _db.Vouchers.Find(id);
            if (voucher == null)
            {
                return NotFound("Voucher not found.");
            }

            voucher.Code = request.Code;
            voucher.DiscountAmount = request.DiscountAmount;
            voucher.ExpiryDate = request.ExpiryDate;

            _db.Vouchers.Update(voucher);
            _db.SaveChanges();

            return Ok("Voucher updated successfully.");
        }



        [HttpDelete("{id}")]
        public IActionResult DeleteVoucher(int id)
        {
            var voucher = _db.Vouchers.Find(id);
            if (voucher == null)
            {
                return NotFound("Voucher not found.");
            }

            _db.Vouchers.Remove(voucher);
            _db.SaveChanges();

            return Ok("Voucher deleted successfully.");
        }


        //////////////////////////////////////////
        ///ارسال البريد الالكتروني لجميع اليوزر بمناسبة معينة بس كان القسيمة مش داينمك
        [HttpPost("SendEmailALLUser")]
        public async Task<IActionResult> SendBulkEmail(string subject, string body)
        {
            var users = _db.Users.ToList(); // الحصول على قائمة جميع المستخدمين

            // قائمة للمهام غير المتزامنة
            var tasks = new List<Task>();

            foreach (var user in users)
            {
                // صياغة الرسالة المناسبة
                var personalizedBody = $@"
        مرحبًا {user.Email}،

        بمناسبة الانتخابات الوطنية، نود أن نقدم لك قسيمة خصم خاصة كعربون تقدير لمساندتك لنا.

        استخدم رمز القسيمة التالي للحصول على خصم قيمته 10% على أي عملية شراء:

        **رمز القسيمة: ELECTION10**

        القسيمة صالحة لمدة أسبوع بدءًا من تاريخ إرسال هذه الرسالة.

        شكرًا لك على دعمك، ونتمنى لك يوماً سعيداً!

        مع تحياتنا،
        فريق [اسم شركتك]
        ";

                // إضافة مهمة غير متزامنة لإرسال البريد لكل مستخدم
                tasks.Add(Task.Run(() => _emailService.SendEmail(user.Email, subject, personalizedBody)));
            }

            // انتظار كل المهام حتى تكتمل
            await Task.WhenAll(tasks);

            return Ok("Emails sent to all users.");
        }


        ////////////////////////////////////////////
        ///هاد الكود بقوم بانشاء القسيمة تلقائيا وارسالها لجميع اليوزر بمناسبة معينة لازم اكوم نعرفة ميثود بتنشألأي القسيمة تلقائيا

        // توليد رمز القسيمة الفريد


        // إرسال البريد الإلكتروني لجميع المستخدمين  كان الخطأ انه مش حاطة الريويسك بوست
        [HttpPost("SendEmailALLUserRememberMe")]
        public async Task<IActionResult> SendALLUSEREmail([FromForm] string subject, [FromForm] string body)
        {
            string GenerateVoucherCode()
            {
                var guid = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10); // توليد رمز عشوائي مكون من 10 أحرف
                return $"VOUCHER-{guid}";
            }
            try
            {
                var users = await _db.Users.ToListAsync(); // الحصول على قائمة جميع المستخدمين بشكل غير متزامن

                // قائمة للمهام غير المتزامنة
                var tasks = new List<Task>();

                foreach (var user in users)
                {
                    // توليد رمز قسيمة فريد
                    var voucherCode = GenerateVoucherCode();

                    // تخزين القسيمة في قاعدة البيانات
                    var voucher = new Voucher
                    {
                        Code = voucherCode,
                        DiscountAmount = 10, // قيمة الخصم
                        ExpiryDate = DateTime.Now.AddDays(7), // فترة صلاحية القسيمة
                        CreatedAt = DateTime.Now
                    };

                    _db.Vouchers.Add(voucher);
                    await _db.SaveChangesAsync(); // حفظ التغييرات بشكل غير متزامن

                    // صياغة الرسالة المناسبة
                    var personalizedBody = $@"
          مرحبًا {user.Email}،

          بمناسبة الانتخابات الوطنية، نود أن نقدم لك قسيمة خصم خاصة كعربون تقدير لمساندتك لنا.

          استخدم رمز القسيمة التالي للحصول على خصم قيمته 10% على أي عملية شراء:D

          **رمز القسيمة: {voucherCode}**

          القسيمة صالحة لمدة أسبوع بدءًا من تاريخ إرسال هذه الرسالة.

          شكرًا لك على دعمك، ونتمنى لك يوماً سعيداً!

          مع تحياتنا،
          فريق [DORNO]
      ";

                    tasks.Add(Task.Run(() => _emailService.SendEmail(user.Email, subject, personalizedBody)));
                }

                await Task.WhenAll(tasks);

                return Ok("Emails sent to all users.");
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ وإرجاع استجابة داخلية للمشكلة
                //_logger.LogError(ex, "Error occurred while sending emails.");
                return StatusCode(500, "Internal server error.");
            }
        }




        [HttpGet("ApplyVoucher")]
        public IActionResult ApplyVoucher(string code)
        {

            var voucher = _db.Vouchers.FirstOrDefault(v => v.Code == code);


            if (voucher == null && voucher.IsUsed)
            {
                return BadRequest(new { message = "Invalid voucher code." });
            }


            voucher.IsUsed = true;
            _db.SaveChanges();


            return Ok(new { Discount = voucher.DiscountAmount });
        }
        [HttpGet("GetVoucher")]
        public IActionResult GetVoucher()
        {

            var voucher = _db.Vouchers.ToList();



            _db.SaveChanges();


            return Ok(voucher);
        }




        [HttpPost("AddVoucheraa")]
        public IActionResult CreateVoucher([FromForm] VoucherRequestDto voucher)
        {

            var vouchers = new Voucher
            {
                Code = voucher.Code,
                DiscountAmount = voucher.DiscountAmount,
                ExpiryDate = voucher.ExpiryDate,
                IsUsed = false,
                CreatedAt = DateTime.Now
            };

            _db.Add(vouchers);
            _db.SaveChanges();
            return Ok("Voucher created successfully.");
        }








        //[HttpPut("{id}/changeRwquestOrder-status")]
        //public IActionResult ChangeStatus(int id, [FromBody] string status)
        //{
        //    var saleRequest = _db.SaleRequests.Include(sr => sr.User).FirstOrDefault(sr => sr.RequestId == id);
        //    if (saleRequest == null)
        //    {
        //        return NotFound(new { message = "طلب البيع غير موجود." });
        //    }

        //    if (status.Equals("approve", StringComparison.OrdinalIgnoreCase))
        //    {
        //        saleRequest.Status = "Approved";
        //        SendEmailNotification(saleRequest.User.Email, saleRequest.User.UserName, true);
        //    }
        //    else if (status.Equals("reject", StringComparison.OrdinalIgnoreCase))
        //    {
        //        saleRequest.Status = "Rejected";
        //        SendEmailNotification(saleRequest.User.Email, saleRequest.User.UserName, false);
        //    }
        //    else
        //    {
        //        return BadRequest(new { message = "حالة غير صالحة. استخدم 'approve' أو 'reject'." });
        //    }

        //    _db.SaveChanges();
        //    return NoContent();
        //}

        //private void SendEmailNotification(string email, string userName, bool isApproved)
        //{
        //    var subject = isApproved ? "إشعار بالموافقة" : "إشعار بالرفض";
        //    var message = isApproved
        //        ? $"عزيزي/عزيزتي {userName},\n\nنحن سعيدون أن نخبرك بأنه تم الموافقة على طلبك لبيع المنتج! 🎉 يمكنك الآن مشاهدة منتجك على الموقع، وعند بيعه، ستتلقى إشعارًا بذلك. شكرًا لكونك جزءًا من مجتمعنا!"
        //        : $"عزيزي/عزيزتي {userName},\n\nنأسف لإخبارك بأن طلبك لبيع المنتج لم يتم قبوله. لا تتردد في تقديم طلب آخر في المستقبل. نحن هنا لدعمك!";

        //    _emailService.SendEmail(email, subject, message);
        //}

        [HttpPut("change-status/{id}")]
        public IActionResult ChangeStatus(int id, [FromBody] StatusChangeRequest request)
        {
            var saleRequest = _db.SaleRequests.Include(sr => sr.User).FirstOrDefault(sr => sr.RequestId == id);
            if (saleRequest == null)
            {
                return NotFound(new { message = "طلب البيع غير موجود." });
            }

            if (string.IsNullOrWhiteSpace(request.Status))
            {
                return BadRequest(new { message = "حقل الحالة مطلوب." });
            }

            if (request.Status.Equals("approve", StringComparison.OrdinalIgnoreCase))
            {
                saleRequest.Status = "Approved";
                // لا تحتاج إلى RejectionReason هنا
                SendEmailNotification(saleRequest.User.Email, saleRequest.User.UserName, true, null);
            }
            else if (request.Status.Equals("reject", StringComparison.OrdinalIgnoreCase))
            {
                // تحقق من أن RejectionReason ليس فارغًا
                if (string.IsNullOrWhiteSpace(request.RejectionReason))
                {
                    return BadRequest(new { message = "حقل سبب الرفض مطلوب عند رفض الطلب." });
                }

                saleRequest.Status = "Rejected";
                SendEmailNotification(saleRequest.User.Email, saleRequest.User.UserName, false, request.RejectionReason);
            }
            else
            {
                return BadRequest(new { message = "حالة غير صالحة. استخدم 'approve' أو 'reject'." });
            }

            _db.SaveChanges();
            return NoContent();
        }




        private void SendEmailNotification(string email, string userName, bool isApproved, string rejectionReason)
        {
            var subject = isApproved ? "إشعار بالموافقة" : "إشعار بالرفض";
            var message = isApproved
                ? $"عزيزي/عزيزتي {userName},\n\nنحن سعيدون أن نخبرك بأنه تم الموافقة على طلبك لبيع المنتج! 🎉 يمكنك الآن مشاهدة منتجك على الموقع، وعند بيعه، ستتلقى إشعارًا بذلك. شكرًا لكونك جزءًا من مجتمعنا!"
                : $"عزيزي/عزيزتي {userName},\n\nنأسف لإخبارك بأن طلبك لبيع المنتج لم يتم قبوله.\n\nسبب الرفض: {rejectionReason ?? "لم يتم تحديد سبب."}\n\nلا تتردد في تقديم طلب آخر في المستقبل. نحن هنا لدعمك!";

            _emailService.SendEmail(email, subject, message);
        }







    }
}
