using BackgroundEmailService.Service.EmailProviderService;
using System.Threading.Tasks;

namespace BackgroundEmailService.Service.EmailService.SendEmailAdapter
{
    public class SendEmail : ISendEmail
    {
        private readonly IEmailProviderService _provider;

        public SendEmail(IEmailProviderService provider)
        {
            _provider = provider;
        }

        public async Task<bool> SendEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;

            bool result = await _provider.SendEmailAsync(email);

            return result;
        }
    }
}
