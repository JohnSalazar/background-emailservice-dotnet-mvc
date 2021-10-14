using BackgroundEmailService.Business.Models;
using System;
using System.Collections.Generic;

namespace BackgroundEmailService.Data.Repository
{
    public interface IClientRepository
    {
        List<Client> List();
        Client Get(Guid id);
    }
}
