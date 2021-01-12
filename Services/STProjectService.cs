using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using StatusTracker.Data;
using StatusTracker.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Project = StatusTracker.Models.Project;

namespace StatusTracker.Services
{
    [Authorize("Admin, ProjectManager")]
    public class STProjectService : ISTProjectService
    {
        private readonly ApplicationDbContext _context;

        public STProjectService(ApplicationDbContext context)
        {
            _context = context;
        }
        //Methods
        public async Task<bool> IsUserOnProject(string userId, int projectId)
        {
            Project project = await _context.Projects
                .Include(u => u.ProjectUsers)
                .ThenInclude(u => u.User)
                .FirstOrDefaultAsync(u => u.Id == projectId);

            bool result = project.ProjectUsers.Any(u => u.UserId == userId);
            return result;
        }
        public async Task<ICollection<Project>> ListUserProjects(string userId)
        {
            STUser user = await _context.Users
                .Include(p => p.ProjectUsers)
                .ThenInclude(p => p.Project)
                .FirstOrDefaultAsync(p => p.Id == userId);

            List<Project> projects = user.ProjectUsers.Select(p => p.Project).ToList();

            return projects;
        }

        public async Task AddUserToProject(string userId, int projectId)
        {
            if (!await IsUserOnProject(userId, projectId))
            {
                try
                {
                    await _context.ProjectUsers.AddAsync(new ProjectUser { ProjectId = projectId, UserId = userId });
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"***ERROR*** - Error adding user to project. --> {ex.Message}");
                    throw;
                }
            }
        }

        public async Task RemoveUserFromProject(string userId, int projectId) 
        {
            if (await IsUserOnProject(userId, projectId))
            {
                try
                {
                    ProjectUser projectUser = _context.ProjectUsers.Where(u => u.UserId == userId && u.ProjectId == projectId).FirstOrDefault();
                
                    _context.ProjectUsers.Remove(projectUser); 
                    await _context.SaveChangesAsync();
                    
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"***ERROR*** - Error removing user from project. --> {ex.Message}");
                    throw;
                }
            }
        } 

        public async Task<ICollection<STUser>> UsersOnProject(int projectId)
        {
            Project project = await _context.Projects
                .Include(u => u.ProjectUsers)
                .ThenInclude(u => u.User)
                .FirstOrDefaultAsync(u => u.Id == projectId);

            List<STUser> projectUsers = project.ProjectUsers.Select(p => p.User).ToList(); 
            return projectUsers;
        }

        public async Task<ICollection<STUser>> UsersNotOnProject(int projectId)
        {
            return await _context.Users.Where(u => IsUserOnProject(u.Id, projectId).Result == false).ToListAsync();
        }

    }
}

