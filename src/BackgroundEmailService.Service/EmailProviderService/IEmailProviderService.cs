using System.Threading.Tasks;

namespace BackgroundEmailService.Service.EmailProviderService
{
    public interface IEmailProviderService
    {
        Task<bool> SendEmailAsync (string email);
    }
}
