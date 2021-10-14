using System;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundEmailService.Service.EmailProviderService
{
    public class EmailProviderService : IEmailProviderService
    {
        private int Port = 556;
        private bool SecuritySSL = false;
        public async Task<bool> SendEmailAsync(string email)
        {
            try
            {
                Thread.Sleep(TimeSpan.FromSeconds(1.5));
            }
            catch (Exception e)
            {
                return false;
            }

            Random rnd = new Random();
            bool success = Convert.ToBoolean(rnd.Next(0, 2));
            return success;
        }
    }
}
