using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StatusTracker.Data;
using StatusTracker.Models;
using StatusTracker.Models.ViewModels;
using StatusTracker.Services;

namespace StatusTracker.Controllers
{
    [Authorize]
    public class UserRolesController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly ISTRolesService _rolesService;
        private readonly UserManager<STUser> _userManager;

        public UserRolesController(ApplicationDbContext context, ISTRolesService rolesService, UserManager<STUser> userManager)
        {
            _context = context;
            _rolesService = rolesService;
            _userManager = userManager;
        }

        //Get
        [Authorize(Roles = "Admin, ProjectManager")]
        public async Task<IActionResult> ManageUserRoles()
        {
            List<ManageUserRolesViewModel> model = new List<ManageUserRolesViewModel>();
            List<STUser> users = _context.Users.ToList();

            foreach (var user in users)
            {
                ManageUserRolesViewModel vm = new ManageUserRolesViewModel();
                vm.User = user;
                var selected = await _rolesService.ListUserRoles(user);
                vm.Roles = new MultiSelectList(_context.Roles, "Name", "Name", selected);
                model.Add(vm);
            }
            return View(model);

        }

        [Authorize(Roles = "Admin, ProjectManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageUserRoles(ManageUserRolesViewModel stuser)
        {
            STUser user = await _context.Users.FindAsync(stuser.User.Id);

            IEnumerable<string> roles = await _rolesService.ListUserRoles(user);
            await _userManager.RemoveFromRolesAsync(user, roles);
            var userRoles = stuser.SelectedRoles;

            foreach (var role in userRoles)
            {
                if (Enum.TryParse(role, out Roles roleValue))
                {
                    await _rolesService.AddUserToRole(user, role);
                }
            }
             return RedirectToAction("ManageUserRoles");  //Success


        }

    }
}
