using System.Collections.Generic;

namespace StatusTracker.Models.ViewModels
{
    public class MyTopTicketsViewModel
    {
        public Ticket Ticket { get; set; }

        public TicketPriority TicketPriority { get; set; }

        public TicketStatus TicketStatus { get; set; }

        public TicketType TicketType { get; set; }

        public ICollection<Ticket> Tickets { get; set; }

        public Project Project { get; set; }

        public ICollection<Project> Projects { get; set; }

        public string OwnerUserId { get; set; }

        public string DeveloperUserId { get; set; }

        public STUser OwnerUser { get; set; }

        public STUser DeveloperUser { get; set; }

    }
}
