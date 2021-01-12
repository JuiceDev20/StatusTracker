using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StatusTracker.Data;
using StatusTracker.Models;
using System.Linq;
using System.Threading.Tasks;

namespace StatusTracker.Controllers
{
    [Authorize]
    public class TicketStatusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketStatusController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        // GET: TicketStatus
        public async Task<IActionResult> Index()
        {
            return View(await _context.TicketStatuses.ToListAsync());
        }

        // GET: TicketStatus/Details/5
        [Authorize(Roles = "Admin, ProjectManager, Developer")]
        public async Task<IActionResult> Details(int? id)
        { 
                if (id == null)
                {
                    return NotFound();
                }

                var ticketStatus = await _context.TicketStatuses
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (ticketStatus == null)
                {
                    return NotFound();
                }

                return View(ticketStatus);
        }

        // GET: TicketStatus/Create
        [Authorize(Roles = "Admin, ProjectManager, Developer")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: TicketStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, ProjectManager, Developer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] TicketStatus ticketStatus)
        { 
            if (!User.IsInRole("Demo"))
            {
                if (ModelState.IsValid)
                {
                    _context.Add(ticketStatus);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(ticketStatus);
            }
            else
            {
                TempData["DemoLockout"] = "Changes made from a Demo role are not saved.";
                return RedirectToAction(nameof(Index));
            }

        }

        // GET: TicketStatus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketStatus = await _context.TicketStatuses.FindAsync(id);
            if (ticketStatus == null)
            {
                return NotFound();
            }
            return View(ticketStatus);
        }

        // POST: TicketStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, ProjectManager, Developer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] TicketStatus ticketStatus)
        {
            if (!User.IsInRole("Demo"))
            {

                if (id != ticketStatus.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(ticketStatus);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TicketStatusExists(ticketStatus.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(ticketStatus);

            }
            else
            {
                TempData["DemoLockout"] = "Changes made from a Demo role are not saved.";
                return RedirectToAction(nameof(Index));
            }

        }

        // GET: TicketStatus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketStatus = await _context.TicketStatuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketStatus == null)
            {
                return NotFound();
            }

            return View(ticketStatus);
        }

        // POST: TicketStatus/Delete/5
        [Authorize(Roles = "Admin, ProjectManager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!User.IsInRole("Demo"))
            {
                 var ticketStatus = await _context.TicketStatuses.FindAsync(id);
                 _context.TicketStatuses.Remove(ticketStatus);
                 await _context.SaveChangesAsync();
                 return RedirectToAction(nameof(Index));

            }
            else
            {
                TempData["DemoLockout"] = "Changes made from a Demo role are not saved.";
                return RedirectToAction(nameof(Index));
            }

        }

        private bool TicketStatusExists(int id)
        {
            return _context.TicketStatuses.Any(e => e.Id == id);
        }
    }
}
