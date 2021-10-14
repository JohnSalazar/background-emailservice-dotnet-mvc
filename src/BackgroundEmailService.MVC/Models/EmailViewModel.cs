using BackgroundEmailService.Service.EmailService;

namespace BackgroundEmailService.MVC.Models
{
    public class EmailViewModel
    {
        public string EmailAddress { get; set; }
        public SendState State { get; set; }
    }
}
