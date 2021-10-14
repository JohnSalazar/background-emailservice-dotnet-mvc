using System;

namespace BackgroundEmailService.MVC.Models
{
    public class ClientViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    }
}
