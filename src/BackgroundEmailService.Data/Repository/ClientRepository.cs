using Bogus;
using BackgroundEmailService.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BackgroundEmailService.Data.Repository
{
    public class ClientRepository : IClientRepository
    {
        public List<Client> ClientStore { get; set; }

        public ClientRepository()
        {
            var testClient = new Faker<Client>("pt_BR")
                .RuleFor(c => c.Id, f => Guid.NewGuid())
                .RuleFor(c => c.Nome, f => f.Name.FullName())
                .RuleFor(c => c.Email, f => f.Internet.Email());
            ClientStore = testClient.Generate(10);
        }

        public List<Client> List()
        {
            return ClientStore.ToList();
        }

        public Client Get(Guid id)
        {
            var client = ClientStore.FirstOrDefault(c => c.Id == id);
            return client;
        }

        
    }
}
