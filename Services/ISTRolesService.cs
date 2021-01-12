using StatusTracker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StatusTracker.Services
{
    public interface ISTRolesService
    {
        public Task<bool> AddUserToRole(STUser user, string roleName);

        public Task<bool> IsUserInRole(STUser user, string roleName);

        public Task<IEnumerable<string>> ListUserRoles(STUser user);

        public Task<bool> RemoveUserFromRole(STUser user, string roleName);

        public Task<ICollection<STUser>> UsersInRole(string roleName);

        public Task<IEnumerable<STUser>> UsersNotInRole(string roleName);

    }
}
