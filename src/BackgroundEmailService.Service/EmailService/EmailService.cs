using BackgroundEmailService.Service.EmailService.SendEmailAdapter;
using System.Threading.Tasks;

namespace BackgroundEmailService.Service.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly ISendEmail _sendEmail;

        public EmailService(ISendEmail sendEmail)
        {
            _sendEmail = sendEmail;
        }

        public async Task<bool> SendEmailAsync(string email)
        {            
            return await _sendEmail.SendEmailAsync(email);
        }
    }
}
