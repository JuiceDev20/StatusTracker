using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using StatusTracker.Data;
using System.Linq;
using System.Threading.Tasks;

namespace StatusTracker.Services
{
    [Authorize]
    public class STAccessService : ISTAccessService
    {
        private readonly ApplicationDbContext _context;
        public STAccessService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CanInteractProject(string userId, string roleName, int projectId)
        {
            switch (roleName)  //Switches are Best when used in discrete lists of items
            {
                case "Admin":
                    return true;
                case "DemoAdmin":
                    return true;
                case "DemoProjectManager":
                    return true;
                case "ProjectManager":
                    if (await _context.ProjectUsers.Where(pu => pu.UserId == userId && pu.ProjectId == projectId).AnyAsync())
                    {
                        return true;
                    }
                    return false;
                default:
                    return false;
            }
        }

        public async Task<bool> CanInteractTicket(string userId, string roleName, int ticketId)
        {

            bool result = false;
            switch (roleName)
            {
                case "Admin":
                    result = true;
                    break;
                case "DemoAdmin":
                    result = true;
                    break;
                case "DemoProjectManager":
                    result = true;
                    break;
                case "DemoDeveloper":
                    result = true;
                    break;
                case "DemoSubmitter":
                    result = true;
                    break;

                case "ProjectManager":
                    var projectId = (await _context.Tickets.FindAsync(ticketId)).ProjectId;
                    if (await _context.ProjectUsers.Where(pu => pu.UserId == userId && pu.ProjectId == projectId).AnyAsync())
                    {
                        result = true;
                    }
                    break;
                case "Developer":
                    if (await _context.Tickets.Where(t => t.Id == ticketId && t.DeveloperUserId == userId).AnyAsync())
                    {
                        result = true;
                    }
                    break;
                case "Submitter":
                    if (await _context.Tickets.Where(t => t.Id == ticketId && t.OwnerUserId == userId).AnyAsync())
                    {
                        result = true;
                    }
                    break;
                default:
                    break;

            }
            return result;

        }

        public async Task<bool> CanInteractAttachment(string userId, string roleName, int ticketId)
        {
            bool result = false;
            switch (roleName)
            {
                case "Admin":
                    result = true;
                    break;

                case "ProjectManager":
                    if (await _context.Tickets.Where(t => t.Id == ticketId && t.OwnerUserId == userId).AnyAsync())
                    {
                        result = true;                        
                    }
                    break;

                case "Developer":
                    if (await _context.Tickets.Where(t => t.Id == ticketId && t.DeveloperUserId == userId).AnyAsync())
                    {
                        result = true;
                    }
                    break;
                case "Submitter":
                    if (await _context.Tickets.Where(t => t.Id == ticketId && t.OwnerUserId == userId).AnyAsync())
                    {
                        result = true;
                    }
                    break;
                default:
                    break;

            }
            return result;

        }
        public async Task<bool> CanInteractComment(string userId, int ticketCommentId, string roleName)
        {
            bool result = false;
            switch (roleName)
            {
                case "Admin":
                    result = true;
                    break;

                case "ProjectManager":
                    var ticketId = (await _context.TicketComments.FindAsync()).TicketId;
                    var projectId = (await _context.Tickets.FindAsync(ticketId)).ProjectId;
                    if (await _context.ProjectUsers.Where(pu => pu.UserId == userId && pu.ProjectId == projectId).AnyAsync())
                    {
                        result = true;
                    }
                    break;

                case "Developer":
                case "Submitter":
                    if (await _context.TicketComments.Where(t => t.Id == ticketCommentId && t.UserId == userId).AnyAsync()) 
                    {
                        result = true;
                    }
                    break;
                default:
                    break;

            }
            return result;
        }

    }
      
}

