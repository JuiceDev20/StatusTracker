using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StatusTracker.Data;
using StatusTracker.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StatusTracker.Controllers
{
    [Authorize]
    public class TicketCommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketCommentsController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        // GET: TicketComments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TicketComments
                .Include(t => t.Ticket)
                .Include(t => t.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TicketComments/Details/5
        [Authorize(Roles = "Admin, ProjectManager, Developer")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketComment = await _context.TicketComments
                .Include(t => t.Ticket)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketComment == null)
            {
                return NotFound();
            }

            return View(ticketComment);
        }

        // GET: TicketComments/Create
       [Authorize(Roles = "Admin, ProjectManager, Developer")]
        public IActionResult Create()
        {
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName");
            return View();
        }

        // POST: TicketComments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, ProjectManager, Developer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Comment,TicketId,UserId")] TicketComment ticketComment)
        {
            if (!User.IsInRole("Demo"))
            {
                ticketComment.Created = DateTime.Now;
                if (ModelState.IsValid)
                {
                    _context.Add(ticketComment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", ticketComment.TicketId);
                ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", ticketComment.UserId);
                return View(ticketComment);
            }
            else
            {
                TempData["DemoLockout"] = "Changes made from a Demo role are not saved.";
                return RedirectToAction(nameof(Index));
            }

        }

        // GET: TicketComments/Edit/5
        [Authorize(Roles = "Admin, ProjectManager, Developer")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketComment = await _context.TicketComments.FindAsync(id);
            if (ticketComment == null)
            {
                return NotFound();
            }
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", ticketComment.TicketId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", ticketComment.UserId);
            return View(ticketComment);
        }

        // POST: TicketComments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, ProjectManager, Developer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Comment,Created,TicketId,UserId")] TicketComment ticketComment)
        {
            if (!User.IsInRole("Demo"))
            {
                if (id != ticketComment.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(ticketComment);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TicketCommentExists(ticketComment.Id))
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
                ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", ticketComment.TicketId);
                ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", ticketComment.UserId);
                return View(ticketComment); 
            }
            else
            {
                TempData["DemoLockout"] = "Changes made from a Demo role are not saved.";
                return RedirectToAction(nameof(Index));
            }

        }

        // GET: TicketComments/Delete/5
        [Authorize(Roles = "Admin, ProjectManager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketComment = await _context.TicketComments
                .Include(t => t.Ticket)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketComment == null)
            {
                return NotFound();
            }

            return View(ticketComment);
        }

        // POST: TicketComments/Delete/5
        [Authorize(Roles = "Admin, ProjectManager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!User.IsInRole("Demo")) 
            {
                 var ticketComment = await _context.TicketComments.FindAsync(id);
                 _context.TicketComments.Remove(ticketComment);
                 await _context.SaveChangesAsync();
                 return RedirectToAction(nameof(Index));      
            
            }
            else
            {
                TempData["DemoLockout"] = "Changes made from a Demo role are not saved.";
                return RedirectToAction(nameof(Index));
            }

        }

        private bool TicketCommentExists(int id)
        {
            return _context.TicketComments.Any(e => e.Id == id);
        }
    }
}
