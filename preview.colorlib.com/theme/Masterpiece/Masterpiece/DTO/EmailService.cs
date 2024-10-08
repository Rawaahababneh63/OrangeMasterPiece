using MimeKit;
using MailKit.Net.Smtp; // التأكد من استيراد مكتبة MailKit الصحيحة
using System;

namespace Masterpiece.DTO
{
    public class EmailService : IEmailService
    {
        public void SendEmail(string to, string subject, string body)
        {
            // إعداد رسالة البريد الإلكتروني
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Your Name", "teamorange077@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", to));
            emailMessage.Subject = subject;

            // إعداد محتوى الرسالة
            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            // استخدام SMTP لإرسال البريد
            using (var client = new MailKit.Net.Smtp.SmtpClient()) // تأكد من استخدام MailKit.Net.Smtp.SmtpClient
            {
                try
                {
                    // الاتصال بخادم SMTP (مثل Gmail)
                    client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

                    // المصادقة باستخدام اسم المستخدم وكلمة مرور التطبيق
                    client.Authenticate("teamorange077@gmail.com", "ugbm vrkj kjwk cvoz");

                    // إرسال البريد
                    client.Send(emailMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email: {ex.Message}");
                }
                finally
                {
                    // التأكد من قطع الاتصال بالخادم
                    client.Disconnect(true);
                }
            }
        }
    }
}
