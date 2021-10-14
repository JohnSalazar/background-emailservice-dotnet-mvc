namespace BackgroundEmailService.Service.EmailService
{
    public class EmailStatusList
    {
        public string EmailAddress { get; set; }
        public SendState State { get; set; }
    }
}