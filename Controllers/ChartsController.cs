using Microsoft.AspNetCore.Mvc;
using StatusTracker.Data;
using StatusTracker.Models;
using System.Collections.Generic;
using System.Linq;

namespace StatusTracker.Controllers
{
    public class ChartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChartsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public JsonResult PriorityChart()
        {
            List<DemoData> result = new List<DemoData>();

            var ticketPriorities = _context.TicketPriorities.ToList();

            foreach(var priority in ticketPriorities)
            {
                result.Add(new DemoData
                {
                    Name = priority.Name,
                    Count = _context.Tickets.Where(t => t.TicketPriorityId == priority.Id).Count()

                });
            }
            return Json(result);
        }

        public JsonResult StatusChart()
        {
            List<DemoData> result = new List<DemoData>();

            var ticketStatuses = _context.TicketStatuses.ToList();

            foreach (var status in ticketStatuses)
            {
                result.Add(new DemoData
                {
                    Name = status.Name,
                    Count = _context.Tickets.Where(t => t.TicketStatusId == status.Id).Count()

                });
            }
            return Json(result);
        }

        public JsonResult TypeChart()
        {
            List<DemoData> result = new List<DemoData>();

            var ticketTypes = _context.TicketTypes.ToList();

            foreach (var type in ticketTypes)
            {
                result.Add(new DemoData
                {
                    Name = type.Name,
                    Count = _context.Tickets.Where(t => t.TicketTypeId == type.Id).Count()

                });
            }
            return Json(result);
        }

    }

}
    

