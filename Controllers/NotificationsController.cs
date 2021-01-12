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
    public class NotificationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotificationsController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Notifications
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Notifications.Include(n => n.Ticket).Include(n => n.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Notifications/Details/5
        [Authorize(Roles = "Admin, ProjectManager")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _context.Notifications
                .Include(n => n.Ticket)
                .Include(n => n.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notification == null)
            {
                return NotFound();
            }

            return View(notification);
        }

        // GET: Notifications/Create
        [Authorize(Roles = "Admin, ProjectManager")]
        public IActionResult Create()
        {
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Notifications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, ProjectManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TicketId,Description,Created,UserId")] Notification notification)
        {
            if (!User.IsInRole("Demo"))
            {
                if (ModelState.IsValid)
                {
                    _context.Add(notification);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                 ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", notification.TicketId);
                 ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", notification.UserId);
                 return View(notification);
            }
            else
            {
                TempData["DemoLockout"] = "Changes made from a Demo role are not saved.";
                return RedirectToAction(nameof(Index));
            }

        }

        // GET: Notifications/Edit/5
        [Authorize(Roles = "Admin, ProjectManager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", notification.TicketId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", notification.UserId);
            return View(notification);
        }

        // POST: Notifications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, ProjectManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TicketId,Description,Created,UserId")] Notification notification)
        {
            if (!User.IsInRole("Demo"))
            {
               if (id != notification.Id)
               {
                return NotFound();
               }

                if (ModelState.IsValid)
                {
                try
                {
                    _context.Update(notification);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificationExists(notification.Id))
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
                ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", notification.TicketId);
                ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", notification.UserId);
                return View(notification);
            }
            else
            {
                TempData["DemoLockout"] = "Changes made from a Demo role are not saved.";
                return RedirectToAction(nameof(Index));
            }

        }

        // GET: Notifications/Delete/5
        [Authorize(Roles = "Admin, ProjectManager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _context.Notifications
                .Include(n => n.Ticket)
                .Include(n => n.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notification == null)
            {
                return NotFound();
            }

            return View(notification);
        }

        // POST: Notifications/Delete/5
        [Authorize(Roles = "Admin, ProjectManager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!User.IsInRole("Demo"))
            {

                var notification = await _context.Notifications.FindAsync(id);
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            else
            {
                TempData["DemoLockout"] = "Changes made from a Demo role are not saved.";
                return RedirectToAction(nameof(Index));
            }

        }
        private bool NotificationExists(int id)
        {
            return _context.Notifications.Any(e => e.Id == id);
        }


    }
}
