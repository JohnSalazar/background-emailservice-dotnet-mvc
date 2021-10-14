namespace BackgroundEmailService.Service.EmailService 
{ 
    public enum SendState
    {
        Send = 0,
        Sending = 1,
        Sent = 2,
        Cancel = 3,
        Canceling = 4,
        Canceled = 5,
        Fail = 6
    }
}
