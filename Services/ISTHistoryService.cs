using StatusTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatusTracker.Services
{
    public interface ISTHistoryService
    {
        Task AddHistory(Ticket oldTicket, Ticket newTicket, string userId);
    }
}
