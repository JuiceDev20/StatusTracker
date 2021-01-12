using StatusTracker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StatusTracker.Services
{
    public interface ISTProjectService
    {
            public Task<bool> IsUserOnProject(string userId, int projectId);

            public Task<ICollection<Project>> ListUserProjects(string userId);

            public Task AddUserToProject(string userId, int projectId);

            public Task RemoveUserFromProject(string userId, int projectId);

            public Task<ICollection<STUser>> UsersOnProject(int projectId);

            public Task<ICollection<STUser>> UsersNotOnProject(int projectId);

    }
}
