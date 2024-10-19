
namespace Masterpiece.DTO
{
    public interface IEmailService
    {
        void SendEmail(string to, string subject, string body);
        Task SendEmailAsync(string? email, string? subject, string adminResponse);
    }
}
