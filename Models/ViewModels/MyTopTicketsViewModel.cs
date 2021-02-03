using System;
using System.Linq;
using System.Collections.Generic;

namespace StatusTracker.Models.ViewModels
{
    public class MyTopTicketsViewModel
    {
        public MyTopTicketsViewModel()
        {
            Tickets = new List<Ticket>();
            TicketPriorities = new List<TicketPriority>();
            TicketStatuses = new List<TicketStatus>();
            TicketTypes = new List<TicketType>();
        }

        public Ticket Ticket { get; set; }

        public List<Ticket> Tickets { get; set; }

        public TicketPriority TicketPriority { get; set; }

        public List<TicketPriority> TicketPriorities { get; set; }

        public TicketStatus TicketStatus { get; set; }

        public List<TicketStatus> TicketStatuses { get; set; }

        public TicketType TicketType { get; set; }

        public List<TicketType> TicketTypes { get; set; }


        public Project Project { get; set; }

        public ICollection<Project> Projects { get; set; }

        public string OwnerUserId { get; set; }

        public string DeveloperUserId { get; set; }

        public STUser OwnerUser { get; set; }

        public STUser DeveloperUser { get; set; }

    }
}
