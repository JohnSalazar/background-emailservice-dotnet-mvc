using System.Threading.Tasks;

namespace BackgroundEmailService.Service.EmailService.SendEmailAdapter
{
    public interface ISendEmail
    {
        Task<bool> SendEmailAsync(string email);
    }
}
