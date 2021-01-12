using System.Threading.Tasks;

namespace StatusTracker.Services
{
    public interface ISTAccessService
    {
        public Task<bool> CanInteractProject(string userId, string roleName, int projectId);

        public Task<bool> CanInteractTicket(string userId, string roleName, int ticketId);

        public Task<bool> CanInteractAttachment(string userId, string roleName, int ticketId);

        public Task<bool> CanInteractComment(string userId, int ticketCommentId, string roleName);

    }
}
