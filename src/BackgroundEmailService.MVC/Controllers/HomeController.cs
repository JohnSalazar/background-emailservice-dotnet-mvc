using Microsoft.AspNetCore.Mvc;
using BackgroundEmailService.Data.Repository;
using BackgroundEmailService.MVC.Models;
using BackgroundEmailService.MVC.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BackgroundEmailService.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IClientRepository _clientRepository;
        private readonly EmailTaskService _emailTaskService;

        public HomeController(IClientRepository clientRepository,
                              EmailTaskService emailTaskService)
        {
            _clientRepository = clientRepository;
            _emailTaskService = emailTaskService;
        }

        public async Task<IActionResult> Index()
        {
            IList<EmailViewModel> clients = _clientRepository.List().Select(x => new EmailViewModel
            {
                EmailAddress = x.Email,
                State = Service.EmailService.SendState.Send
            }).ToList();

            return View(clients);
        }

        [HttpGet]
        [Route("clients")]
        public async Task<IActionResult> List()
        {
            return Ok(_clientRepository.List());
        }

        [HttpGet]
        [Route("clients/{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(_clientRepository.Get(id));
        }
        
        [HttpPost]
        [Route("clients/send-email")]
        public async Task<IActionResult> SendEmail([FromBody] List<EmailViewModel> emails)
        {
            if (emails?.Count > 0)
            {
                emails.ForEach(e =>
                {
                    var result = _emailTaskService.AddEmailTask(e.EmailAddress).Result;
                    if (!result) Console.WriteLine($"{e.EmailAddress} not add!");
                });
            };

            return Ok(emails.Count());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
