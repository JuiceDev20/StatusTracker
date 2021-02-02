using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StatusTracker.Data;
using StatusTracker.Models;
using StatusTracker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatusTracker.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ISTHistoryService _historyService;
        private readonly UserManager<STUser> _userManager;
        private readonly ISTRolesService _sTRolesService;
        private readonly ISTProjectService _sTProjectService;
        private readonly ISTFileService _sTFileService;

        public TicketsController(ApplicationDbContext context, ISTHistoryService historyService, UserManager<STUser> userManager, ISTRolesService sTRolesService, ISTProjectService sTProjectService, ISTFileService sTFileService)
        {
            _context = context;
            _historyService = historyService;
            _userManager = userManager;
            _sTRolesService = sTRolesService;
            _sTProjectService = sTProjectService;
            _sTFileService = sTFileService;

        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Tickets
                .Include(t => t.DeveloperUser)
                .Include(t => t.OwnerUser)
                .Include(t => t.Project)
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> MyTickets()
        {
            var userId = _userManager.GetUserId(User); // Get the currently logged in user.
            var roleList = await _sTRolesService.ListUserRoles(_context.Users.Find(userId));
            var role = roleList.FirstOrDefault();
            List<Ticket> model;
            switch (role)
            {
                case "Admin":
                    model = _context.Tickets
                        .Include(t => t.OwnerUser)
                        .Include(t => t.TicketPriority)
                        .Include(t => t.TicketStatus)
                        .Include(t => t.TicketType)
                        .Include(t => t.Project)
                        .Include(t => t.DeveloperUser)
                        .ToList();
                    break;
                //   Snippet to get ticket for project manager -special case for roles
                case "ProjectManager":
                    var projectIds = new List<int>();
                    model = new List<Ticket>();
                    var userProjects = _context.ProjectUsers.Where(pu => pu.UserId == userId).ToList();
                    foreach (var record in userProjects)
                    {
                        projectIds.Add(_context.Projects.Find(record.ProjectId).Id);
                    }
                    foreach (var id in projectIds)
                    {
                        var tickets = _context.Tickets.Where(t => t.ProjectId == id)
                            .Include(t => t.OwnerUser)
                            .Include(t => t.TicketPriority)
                            .Include(t => t.TicketStatus)
                            .Include(t => t.TicketType)
                            .Include(t => t.Project)
                            .Include(t => t.DeveloperUser)
                            .ToList();
                        model.AddRange(tickets);
                    }
                    break;
                case "Developer":
                    model = _context.Tickets.Where(t => t.DeveloperUserId == userId).ToList();
                    break;
                case "Submitter":
                case "NewUser":
                    model = _context.Tickets.Where(t => t.OwnerUserId == userId).ToList();
                    break;
                default:
                    return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> MyTopTickets()
        {
            var userId = _userManager.GetUserId(User); // Get the currently logged in user.
            var roleList = await _sTRolesService.ListUserRoles(_context.Users.Find(userId));
            var role = roleList.FirstOrDefault();
            List<Ticket> model;
            switch (role)
            {
                case "Admin":
                    model = _context.Tickets.Where(t => t.TicketPriority.Name == "Urgent")
                        .Include(t => t.OwnerUser)
                        .Include(t => t.TicketStatus)
                        .Include(t => t.TicketType)
                        .Include(t => t.Project)
                        .Include(t => t.DeveloperUser)
                        .ToList();
                    break;
                //   Snippet to get ticket for project manager -special case for roles
                case "ProjectManager":
                    var projectIds = new List<int>();
                    model = new List<Ticket>();
                    var userProjects = _context.ProjectUsers.Where(pu => pu.UserId == userId).ToList();
                    foreach (var record in userProjects)
                    {
                        projectIds.Add(_context.Projects.Find(record.ProjectId).Id);
                    }
                    foreach (var id in projectIds)
                    {
                        var tickets = _context.Tickets.Where(t => t.ProjectId == id && t.TicketPriority.Name == "Urgent")
                            .Include(t => t.OwnerUser)
                            .Include(t => t.TicketPriority)
                            .Include(t => t.TicketStatus)
                            .Include(t => t.TicketType)
                            .Include(t => t.Project)
                            .Include(t => t.DeveloperUser)
                            .ToList();
                        model.AddRange(tickets);
                    }
                    break;
                default:
                    return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var ticket = await _context.Tickets
                .Include(t => t.DeveloperUser)
                .Include(t => t.OwnerUser)
                .Include(t => t.Project)
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType)
                .Include(t => t.Comments).ThenInclude(tc => tc.User)
                .Include(t => t.Attachments)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Admin, ProjectManager, Developer, Submitter, Demo")]
        public IActionResult Create(int? id)
        {
            ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "FullName");
            ViewData["OwnerUserId"] = new SelectList(_context.Users, "Id", "FullName");
            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name");
            ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Name");
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name");

            if (User.IsInRole("Admin"))
            {
                ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name");
                return View();

            }
            else
            {
                var userId = _userManager.GetUserId(User);
                var records = _context.ProjectUsers.Where(pu => pu.UserId == userId).Select(pu => pu.Project).ToList();
                ViewData["ProjectId"] = new SelectList(records, "Id", "Name");
            }
            if (!User.IsInRole("Admin") || !User.IsInRole("ProjectManager") || !User.IsInRole("Demo"))
            {
                var ticket = new Ticket
                {
                    ProjectId = id.Value,
                    TicketPriorityId = _context.TicketPriorities.Where(tp => tp.Name == "Low").FirstOrDefault().Id,
                    TicketStatusId = _context.TicketStatuses.Where(tp => tp.Name == "Open").FirstOrDefault().Id
                };
                return View(ticket);
            }
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, ProjectManager, Developer, Submitter")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,TicketTypeId,ProjectId,TicketPriorityId,TicketStatusId,DeveloperUserId")] Ticket ticket, IFormFile attachment)
        {
            if (!User.IsInRole("Demo"))
            {
                if (ModelState.IsValid)
                {
                    ticket.Created = DateTime.Now;
                    ticket.OwnerUserId = _userManager.GetUserId(User);
                    _context.Add(ticket);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "FullName", ticket.DeveloperUserId);
                ViewData["OwnerUserId"] = new SelectList(_context.Users, "Id", "FullName", ticket.OwnerUserId);
                ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);
                ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
                ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
                ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name", ticket.TicketTypeId);

                return View(ticket);
            }
            else
            {
                TempData["DemoLockout"] = "Changes made from a Demo role are not saved.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            var users = _context.Users.ToList();
            var developers = new List<STUser>();
            foreach(var user in users)
            {
                if(await _sTRolesService.IsUserInRole(user, "Developer"))
                {
                    developers.Add(user);
                }
            }
            ViewData["DeveloperUserId"] = new SelectList(developers, "Id", "FullName", ticket.DeveloperUserId);
            ViewData["OwnerUserId"] = new SelectList(_context.Users, "Id", "FullName", ticket.OwnerUserId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);
            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, ProjectManager, Developer, Submitter")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Created,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,DeveloperUserId")] Ticket ticket)
        {
            if (!User.IsInRole("Demo"))
            {
                if (id != ticket.Id)
                {
                    return NotFound();
                }
                Ticket oldTicket = await _context.Tickets
                    .Include(t => t.TicketPriority)
                    .Include(t => t.TicketStatus)
                    .Include(t => t.TicketType)
                    .Include(t => t.DeveloperUser)
                    .Include(t => t.Project)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == ticket.Id);

                if (ModelState.IsValid)
                {

                    try
                    {
                        ticket.Updated = DateTime.Now;
                        _context.Update(ticket);
                        await _context.SaveChangesAsync();

                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TicketExists(ticket.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    Ticket newTicket = await _context.Tickets
                           .Include(t => t.TicketPriority)
                           .Include(t => t.TicketStatus)
                           .Include(t => t.TicketType)
                           .Include(t => t.DeveloperUser)
                           .Include(t => t.Project)
                           .AsNoTracking().FirstOrDefaultAsync(t => t.Id == ticket.Id);
                    var userId = _userManager.GetUserId(User);
                    await _historyService.AddHistory(oldTicket, newTicket, userId);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "FullName", ticket.DeveloperUserId);
                ViewData["OwnerUserId"] = new SelectList(_context.Users, "Id", "FullName", ticket.OwnerUserId);
                ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);
                ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
                ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
                ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name", ticket.TicketTypeId);

                return View(ticket);

            }
            else
            {
                TempData["DemoLockout"] = "Changes made from a Demo role are not saved.";
                return RedirectToAction(nameof(Index));

            }

        }

        // GET: Tickets/Delete/5
        [Authorize(Roles = "Admin, ProjectManager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var ticket = await _context.Tickets
                .Include(t => t.DeveloperUser)
                .Include(t => t.OwnerUser)
                .Include(t => t.Project)
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [Authorize(Roles = "Admin, ProjectManager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!User.IsInRole("Demo"))
            {
                var ticket = await _context.Tickets.FindAsync(id);
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            else
            {
                TempData["DemoLockout"] = "Changes made from a Demo role are not saved.";
                return RedirectToAction(nameof(Index));
            }
            
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }

        public async Task<FileResult> DownloadFile(int? id)  //Added to allow downloading
        { 
            if(id == null)
            {
                return null;
            }
            TicketAttachment attachment = await _context.TicketAttachments.FirstOrDefaultAsync(t => t.Id ==id);
            if(attachment == null)
            {
                return null;
            }
            return File(attachment.FileData, attachment.ContentType);
        }

    }

}
