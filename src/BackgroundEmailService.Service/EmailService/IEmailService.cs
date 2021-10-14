using System.Threading.Tasks;

namespace BackgroundEmailService.Service.EmailService
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string email);
    }
}
