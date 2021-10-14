using System;

namespace BackgroundEmailService.Business.Models
{
    public class Client
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    }
}
