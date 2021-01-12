using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StatusTracker.Data;
using StatusTracker.Models;
using System.Linq;
using System.Threading.Tasks;

namespace StatusTracker.Controllers
{
    [Authorize]
    public class TicketHistoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketHistoriesController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: TicketHistories
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TicketHistories
                .Include(t => t.Ticket)
                .Include(t => t.User);
            return View(await applicationDbContext.ToListAsync());
        }


        // GET: TicketHistories/Details/
        [Authorize(Roles = "Admin, ProjectManager")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketHistory = await _context.TicketHistories
                .Include(t => t.Ticket)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketHistory == null)
            {
                return NotFound();
            }

            return View(ticketHistory);
        }


        // GET: TicketHistories/Create
        [Authorize(Roles = "Admin, ProjectManager")]
        public IActionResult Create()
        {
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName");
            return View();
        }

        // POST: TicketHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, ProjectManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TicketId,Property,OldValue,NewValue,Created,UserId")] TicketHistory ticketHistory)
        {
            if (!User.IsInRole("Demo"))
            {
                if (ModelState.IsValid)
                {
                     _context.Add(ticketHistory);
                     await _context.SaveChangesAsync();
                     return RedirectToAction(nameof(Index));
                }

                ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", ticketHistory.TicketId);
                ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", ticketHistory.UserId);
                return View(ticketHistory);
            }
            else
            {
                TempData["DemoLockout"] = "Changes made from a Demo role are not saved.";
                return RedirectToAction(nameof(Index));
            }

        }

        // GET: TicketHistories/Edit/5
        [Authorize(Roles = "Admin, ProjectManager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketHistory = await _context.TicketHistories.FindAsync(id);
            if (ticketHistory == null)
            {
                return NotFound();
            }
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", ticketHistory.TicketId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", ticketHistory.UserId);
            return View(ticketHistory);
        }

        // POST: TicketHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, ProjectManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TicketId,Property,OldValue,NewValue,Created,UserId")] TicketHistory ticketHistory)
        {
            if (!User.IsInRole("Demo"))
            {
                if (id != ticketHistory.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(ticketHistory);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TicketHistoryExists(ticketHistory.Id))
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
                ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", ticketHistory.TicketId);
                ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", ticketHistory.UserId);
                return View(ticketHistory);
            }
            else
            {
                TempData["DemoLockout"] = "Changes made from a Demo role are not saved.";
                return RedirectToAction(nameof(Index));
            }

        }

        // GET: TicketHistories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketHistory = await _context.TicketHistories
                .Include(t => t.Ticket)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketHistory == null)
            {
                return NotFound();
            }

            return View(ticketHistory);
        }

        // POST: TicketHistories/Delete/5
        [Authorize(Roles = "Admin, ProjectManager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        { 
            if (!User.IsInRole("Demo"))
            {
                    var ticketHistory = await _context.TicketHistories.FindAsync(id);
                    _context.TicketHistories.Remove(ticketHistory);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["DemoLockout"] = "Changes made from a Demo role are not saved.";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool TicketHistoryExists(int id)
        {
            return _context.TicketHistories.Any(e => e.Id == id);
        }
    }
}
