using Masterpiece.DTO;
using Masterpiece.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Masterpiece.Controllers
{
    public class ContactController:ControllerBase
    {

        private readonly MyDbContext _db;
        private readonly IEmailService _emailService;
        public ContactController(MyDbContext db, IEmailService emailService)
        {
            _db = db;
            _emailService = emailService;
        }

        [HttpPost("ADD")]
        public IActionResult Contact([FromForm] ContactRequestDTO DTO)
        {
            var contact = new Contact
            {
                Name = DTO.Name,
                Email = DTO.Email,
                Message = DTO.Message,
                Subject = DTO.Subject,
                SentDate = DateOnly.FromDateTime(DateTime.Now)
            };
            _db.Contacts.Add(contact);
            _db.SaveChanges();


            return Ok(contact);
        }

        [HttpGet("contact")]
        public IActionResult GetContact()
        {
            var contact = _db.Contacts.ToList();
            return Ok(contact);
        }
        [HttpPost("reply/{id}")]
        public async Task<IActionResult> ReplyToContactForm(int id, [FromForm] ContactUsReplyDto replyDto)
        {
            var contactForm = await _db.Contacts.FindAsync(id);

            if (contactForm == null)
            {
                return NotFound();
            }

            // تحديث الرد من الإدارة
            contactForm.AdminResponse = replyDto.AdminResponse;
            contactForm.Status = "replied"; // تغيير الحالة إلى "تم الرد"

            _db.Entry(contactForm).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            // استخدام الخدمة لإرسال البريد الإلكتروني
            await _emailService.SendEmailAsync(replyDto.Email, replyDto.Subject, contactForm.AdminResponse);

            return Ok(replyDto);
        }


        private static void SendEmailAsync(string? email, string subject, string? adminResponse)
        {
            throw new NotImplementedException();
        }
    }
}
