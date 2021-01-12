using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatusTracker.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public string SenderId { get; set; }
        public string RecipientId { get; set; }
        public bool Viewed { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual STUser Sender { get; set; }
        public virtual STUser Recipient { get; set; }

    
    }
}
