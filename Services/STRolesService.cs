using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StatusTracker.Data;
using StatusTracker.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatusTracker.Services
{
    [Authorize("Admin, ProjectManager")]
    public class STRolesService : ISTRolesService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<STUser> _userManager;
        private readonly ApplicationDbContext _context;

        public STRolesService(RoleManager<IdentityRole> roleManager, UserManager<STUser> userManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        public async Task<bool> AddUserToRole(STUser user, string roleName)
        {
            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<bool> IsUserInRole(STUser user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<IEnumerable<string>> ListUserRoles(STUser user)
        {
            return await _userManager.GetRolesAsync(user);

        }

        public async Task<bool> RemoveUserFromRole(STUser user, string roleName)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<ICollection<STUser>> UsersInRole(string roleName)
        {
            return await _userManager.GetUsersInRoleAsync(roleName);
        }

        public async Task<IEnumerable<STUser>> UsersNotInRole(string roleName)
        {
            var inRole = await _userManager.GetUsersInRoleAsync(roleName);
            var users = await _userManager.Users.ToListAsync();
            return users.Except(inRole);
        }

    }
}
