using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StatusTracker.Data;
using StatusTracker.Models;
using StatusTracker.Models.ViewModels;
using StatusTracker.Services;

namespace StatusTracker.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ISTProjectService _sTProjectService;
        private readonly UserManager<STUser> _userManager;

        public ProjectsController(ApplicationDbContext context, ISTProjectService sTProjectService, UserManager<STUser> userManager)
        {
            _context = context;
            _sTProjectService = sTProjectService;
            _userManager = userManager;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            return View(await _context.Projects.ToListAsync());
        }

        //Create a list of type Project, then filter the list according to user Ids.
        public async Task<IActionResult> MyProjects()
        {
            var model = new MyProjectsViewModel();
            var userId = _userManager.GetUserId(User);
            var myProjects = await _sTProjectService.ListUserProjects(userId);
            var user = _context.Users.Find(userId);
            model.Projects = myProjects;
            model.User = user;

            return View(model);
        }

        // GET: Projects/Details/5
        [Authorize(Roles = "Admin, ProjectManager, Developer")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var project = await _context.Projects
                .Include(m => m.ProjectUsers)
                .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin, ProjectManager")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, ProjectManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ImagePath,ImageData")] Project project)
        {
            if (!User.IsInRole("Demo"))
            {
                if (ModelState.IsValid)
                {
                    _context.Add(project);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(project);
            }
            else
            {
                TempData["DemoLockout"] = "Changes made from a Demo role are not saved.";
                return RedirectToAction(nameof(Index));
            }

        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin, ProjectManager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, ProjectManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ImagePath,ImageData")] Project project)
        {
            if (!User.IsInRole("Demo"))
            {
                if (id != project.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(project);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProjectExists(project.Id))
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
                return View(project);
            }
            else
            {
                TempData["DemoLockout"] = "Changes made from a Demo role are not saved.";
                return RedirectToAction(nameof(Index));
            }

        }

        // GET: Projects/Delete/5
        [Authorize(Roles = "Admin, ProjectManager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [Authorize(Roles = "Admin, ProjectManager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!User.IsInRole("Demo"))
            {
                var project = await _context.Projects.FindAsync(id);
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["DemoLockout"] = "Changes made from a Demo role are not saved.";
                return RedirectToAction(nameof(Index));
            }

        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }

        //Assign Users Action
        [Authorize(Roles = "Admin, ProjectManager")]
        [HttpGet]
        public async Task<IActionResult> AssignUsers(int id)
        {
            var model = new ManageProjectUsersViewModel();
            var project = _context.Projects.Find(id);

            model.Project = project;
            List<STUser> users = await _context.Users.ToListAsync();
            List<STUser> members = (List<STUser>)await _sTProjectService.UsersOnProject(id);
            model.Users = new MultiSelectList(users, "Id", "FullName", members);
            return View(model);
        }

        //Post Assign Users
        [Authorize(Roles = "Admin, ProjectManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignUsers(ManageProjectUsersViewModel model)
        {
            if (!User.IsInRole("Demo"))
            {
                if (ModelState.IsValid)
                {
                    //var project = db.Projects.Find(model.projectId);
                    //project.Users.Clear();
                    if (model.SelectedUsers != null)
                    {
                        var currentMembers = await _context.Projects
                        .Include(p => p.ProjectUsers)
                        .FirstOrDefaultAsync(p => p.Id == model.Project.Id);
                        List<string> memberIds = currentMembers.ProjectUsers.Select(u => u.UserId).ToList();
                        foreach (string id in memberIds)
                        {
                            await _sTProjectService.RemoveUserFromProject(id, model.Project.Id);
                        }

                        foreach (string id in model.SelectedUsers)
                        {
                            await _sTProjectService.AddUserToProject(id, model.Project.Id);
                        }
                        return RedirectToAction("Index", "Projects");
                    }
                    else
                    {
                        //Send error message back
                    }
                }
                return View(model);
            }
            else
            {
                TempData["DemoLockout"] = "Changes made from a Demo role are not saved.";
                return RedirectToAction(nameof(Index));
            }

        }

        //Remove Users Action
        [Authorize(Roles = "Admin, ProjectManager")]
        [HttpGet]
        public async Task<IActionResult> RemoveUsers(int id)
        {
            var model = new ManageProjectUsersViewModel();
            var project = _context.Projects.Find(id);

            model.Project = project;
            List<STUser> members = (List<STUser>)await _sTProjectService.UsersOnProject(id);
            model.Users = new MultiSelectList(members, "Id", "FullName");
            return View(model);
        }

        //Post Remove Users
        [Authorize(Roles = "Admin, ProjectManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveUsers(ManageProjectUsersViewModel model)
        {
            if (!User.IsInRole("Demo"))
            {

                if (ModelState.IsValid)
                {
                    //var project = db.Projects.Find(model.projectId);
                    //project.Users.Clear();
                    if (model.SelectedUsers != null)
                    {
                        foreach (string id in model.SelectedUsers)
                        {
                            await _sTProjectService.RemoveUserFromProject(id, model.Project.Id);
                        }
                        return RedirectToAction("Index", "Projects");
                    }
                    else
                    {
                        //Send error message back
                    }
                }
                return View(model);

            }
            else
            {
                TempData["DemoLockout"] = "Changes made from a Demo role are not saved.";
                return RedirectToAction(nameof(Index));
            }

        }
    }
}
