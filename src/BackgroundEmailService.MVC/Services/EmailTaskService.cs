using Microsoft.Extensions.Hosting;
using BackgroundEmailService.MVC.Hubs;
using BackgroundEmailService.MVC.Models;
using BackgroundEmailService.Service.EmailService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundEmailService.MVC.Services
{
    public class EmailTaskService : BackgroundService
    {
        private readonly EmailHub _emailHub;
        private readonly IEmailService _emailService;

        private List<string> _emailWaitingList = new List<string>();
        private List<string> _emailProcessingList = new List<string>();
        private List<EmailViewModel> _emailStatusList = new List<EmailViewModel>();
        private const int _maxThrottler = 3;

        public EmailTaskService(EmailHub emailHub, IEmailService emailService)
        {
            _emailHub = emailHub;
            _emailService = emailService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                ProcessQueue();

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        public async Task<bool> AddEmailTask(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;
            if (_emailWaitingList?.Find(e => e.ToString().Equals(email.Trim().ToString())) == null)
            {
                _emailWaitingList.Add(email.Trim());                
            }
            return true;
        }

        private async void ProcessQueue()
        {

            if (CreateProcessingQueue())
            {
                _emailProcessingList.ForEach(async delegate (string email)
                {
                    var emailStatusListSending = CreateEmailState(email, SendState.Sending);
                    UpdateStatusList(emailStatusListSending);
                    emailStatusListSending = null;

                    var send = await Send(email);
                    if (send)
                    {
                        var emailStatusListSent = CreateEmailState(email, SendState.Sent);
                        UpdateStatusList(emailStatusListSent);
                        emailStatusListSent = null;
                        
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{email} sent!");
                        Console.ResetColor();
                    }
                    else
                    {
                        var emailStatusListFail = CreateEmailState(email, SendState.Fail);
                        UpdateStatusList(emailStatusListFail);
                        emailStatusListFail = null;

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"{email} send failed!");
                        Console.ResetColor();
                    }
                });

                _emailWaitingList.RemoveAll(new HashSet<string>(_emailProcessingList.ToArray()).Contains);

                _emailProcessingList.Clear();
            }
        }

        private bool CreateProcessingQueue()
        {

            bool queueState = false;

            if (_emailWaitingList?.Count > 0)
            {
                var list = _emailWaitingList.Take(_maxThrottler).ToList();
                list.ForEach(email =>
                {
                    _emailProcessingList.Add(email);
                    var emailViewModel = new EmailViewModel() { EmailAddress = email, State = SendState.Sending };
                    UpdateStatusList(emailViewModel);
                    emailViewModel = null;
                });
                list = null;
                queueState = true;
            }

            return queueState;
        }

        private bool UpdateStatusList(EmailViewModel email)
        {
            var index = _emailStatusList.FindIndex(e => e.EmailAddress.Equals(email.EmailAddress));
            if (index >= 0)
            {
                _emailStatusList[index].State = email.State;
            }
            else
            {
                _emailStatusList.Add(email);
            }

            Task.Run(async () => await _emailHub.SendMessage(email.EmailAddress, email));

            return true;
        }

        public bool CancelTask(string email)
        {
            var emailStatusListProcessing = CreateEmailState(email, SendState.Canceling);
            UpdateStatusList(emailStatusListProcessing);
            emailStatusListProcessing = null;

            _emailProcessingList.RemoveAll(x => x.Equals(email));
            _emailWaitingList.RemoveAll(x => x.Equals(email));

            var emailStatusListProcessed = CreateEmailState(email, SendState.Canceled);
            UpdateStatusList(emailStatusListProcessed);
            emailStatusListProcessed = null;

            return true;
        }

        private EmailViewModel CreateEmailState(string email, SendState state)
        {
            return new EmailViewModel() { EmailAddress = email, State = state };
        }

        private async Task<bool> Send(string email)
        {
            var send = await _emailService.SendEmailAsync(email);
            return send;
        }
    }
}
