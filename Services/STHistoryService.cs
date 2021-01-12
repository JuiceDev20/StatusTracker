using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using StatusTracker.Data;
using StatusTracker.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace StatusTracker.Services
{
    [Authorize("Admin, ProjectManager, Developer")]
    public class STHistoryService : ISTHistoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<STUser> _userManager;
        private readonly IEmailSender _emailSender;

        public STHistoryService(ApplicationDbContext context, UserManager<STUser> userManager, IEmailSender emailSender)

        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task AddHistory(Ticket oldTicket, Ticket newTicket, string userId)
        {
            // If oldTicket.prop != newTicket.prop { addHistory }
            if (oldTicket.Title != newTicket.Title)
            {
                TicketHistory history = new TicketHistory
                {
                    TicketId = newTicket.Id,
                    Property = "Title",
                    OldValue = oldTicket.Title,
                    NewValue = newTicket.Title,
                    Created = DateTimeOffset.Now,
                    UserId = userId
                };
                await _context.TicketHistories.AddAsync(history);
            }
            if (oldTicket.Description != newTicket.Description)
            {
                TicketHistory history = new TicketHistory
                {
                    TicketId = newTicket.Id,
                    Property = "Description",
                    OldValue = oldTicket.Description,
                    NewValue = newTicket.Description,
                    Created = DateTimeOffset.Now,
                    UserId = userId
                };
                await _context.TicketHistories.AddAsync(history);
            }
            if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
            {
                TicketHistory history = new TicketHistory
                {
                    TicketId = newTicket.Id,
                    Property = "Ticket Type",
                    OldValue = oldTicket.TicketType.Name,
                    NewValue = newTicket.TicketType.Name,
                    Created = DateTimeOffset.Now,
                    UserId = userId
                };
                await _context.TicketHistories.AddAsync(history);
            }
            if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
            {
                TicketHistory history = new TicketHistory
                {
                    TicketId = newTicket.Id,
                    Property = "Ticket Priority",
                    OldValue = oldTicket.TicketPriority.Name,
                    NewValue = newTicket.TicketPriority.Name,
                    Created = DateTimeOffset.Now,
                    UserId = userId
                };
                await _context.TicketHistories.AddAsync(history);
            }
            if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
            {
                TicketHistory history = new TicketHistory
                {
                    TicketId = newTicket.Id,
                    Property = "Ticket Status",
                    OldValue = oldTicket.TicketStatus.Name,
                    NewValue = newTicket.TicketStatus.Name,
                    Created = DateTimeOffset.Now,
                    UserId = userId
                };
                await _context.TicketHistories.AddAsync(history);
            }
            if (oldTicket.DeveloperUserId != newTicket.DeveloperUserId)
            {
                if (String.IsNullOrWhiteSpace(oldTicket.DeveloperUserId))
                {
                    TicketHistory history = new TicketHistory
                    {
                        TicketId = newTicket.Id,
                        Property = "Developer",
                        OldValue = "No Developer Assigned",
                        NewValue = newTicket.DeveloperUser.FullName,
                        Created = DateTimeOffset.Now,
                        UserId = userId
                    };
                    await _context.TicketHistories.AddAsync(history);
                }
                else if (String.IsNullOrWhiteSpace(newTicket.DeveloperUserId))
                {
                    TicketHistory history = new TicketHistory
                    {
                        TicketId = newTicket.Id,
                        Property = "Developer",
                        OldValue = oldTicket.DeveloperUser.FullName,
                        NewValue = "No Developer Assigned",
                        Created = DateTimeOffset.Now,
                        UserId = userId
                    };
                    await _context.TicketHistories.AddAsync(history);
                }
                else
                {
                    TicketHistory history = new TicketHistory
                    {
                        TicketId = newTicket.Id,
                        Property = "Developer",
                        OldValue = oldTicket.DeveloperUser.FullName,
                        NewValue = newTicket.DeveloperUser.FullName,
                        Created = DateTimeOffset.Now,
                        UserId = userId
                    };
                    await _context.TicketHistories.AddAsync(history);
                }
                // For now - maybe we can get a notification service
                Notification notification = new Notification
                {
                    TicketId = newTicket.Id,
                    Description = "You have been assigned a new ticket.",
                    Created = DateTime.Now,
                    SenderId = userId,
                    RecipientId = newTicket.DeveloperUserId
                };
                await _context.Notifications.AddAsync(notification);
                try
                {
                    // Send email
                    string devEmail = newTicket.DeveloperUser.Email;
                    string subject = "New Ticket Assignment";
                    string message = $"You have a new ticket for project: {newTicket.Project.Name}";
                    await _emailSender.SendEmailAsync(devEmail, subject, message);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Couldn't send an email... {ex}");
                }
            }
            await _context.SaveChangesAsync();
        }

        //public async Task AddHistory(Ticket oldTicket, Ticket newTicket, string userId)
        //{
        //    if (oldTicket.Title != newTicket.Title)
        //    {
        //        TicketHistory history = new TicketHistory
        //        {
        //            TicketId = newTicket.Id,
        //            Property = "Title",
        //            OldValue = oldTicket.Title,
        //            NewValue = newTicket.Title,
        //            Created = DateTimeOffset.Now,
        //            UserId = userId
        //        };
        //        await _context.TicketHistories.AddAsync(history);
        //    }

        //    if (oldTicket.Description != newTicket.Description)
        //    {
        //        TicketHistory history = new TicketHistory
        //        {
        //            TicketId = newTicket.Id,
        //            Property = "Description",
        //            OldValue = oldTicket.Description,
        //            NewValue = newTicket.Description,
        //            Created = DateTimeOffset.Now,
        //            UserId = userId
        //        };
        //        await _context.TicketHistories.AddAsync(history);
        //    }

        //    if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
        //    {
        //        TicketHistory history = new TicketHistory
        //        {
        //            TicketId = newTicket.Id,
        //            Property = "Priority",
        //            OldValue = _context.TicketPriorities.Find(oldTicket.TicketPriorityId).Name,
        //            NewValue = _context.TicketPriorities.Find(newTicket.TicketPriorityId).Name,
        //            Created = DateTimeOffset.Now,
        //            UserId = userId
        //        };
        //        await _context.TicketHistories.AddAsync(history);
        //    }

        //    if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
        //    {
        //        TicketHistory history = new TicketHistory
        //        {
        //            TicketId = newTicket.Id,
        //            Property = "TicketType",
        //            OldValue = _context.TicketTypes.Find(oldTicket.TicketTypeId).Name,
        //            NewValue = _context.TicketTypes.Find(newTicket.TicketTypeId).Name,
        //            Created = DateTimeOffset.Now,
        //            UserId = userId
        //        };
        //        await _context.TicketHistories.AddAsync(history);
        //    }

        //    if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
        //    {
        //        TicketHistory history = new TicketHistory
        //        {
        //            TicketId = newTicket.Id,
        //            Property = "Status",
        //            OldValue = _context.TicketStatuses.Find(oldTicket.TicketStatusId).Name,
        //            NewValue = _context.TicketStatuses.Find(newTicket.TicketStatusId).Name,
        //            Created = DateTimeOffset.Now,
        //            UserId = userId
        //        };
        //        await _context.TicketHistories.AddAsync(history);
        //    }

        //    if (oldTicket.DeveloperUserId != newTicket.DeveloperUserId)
        //    {
        //        if (String.IsNullOrWhiteSpace(oldTicket.DeveloperUserId))
        //        {
        //            TicketHistory history = new TicketHistory
        //            {
        //                TicketId = newTicket.Id,
        //                Property = "Developer",
        //                OldValue = "No Developer Assigned",
        //                NewValue = _context.Users.Find(newTicket.DeveloperUserId).FullName,
        //                Created = DateTimeOffset.Now,
        //                UserId = userId
        //            };
        //            await _context.TicketHistories.AddAsync(history);

        //        }
        //        else if (String.IsNullOrWhiteSpace(newTicket.DeveloperUserId))
        //        {
        //            TicketHistory history = new TicketHistory
        //            {
        //                TicketId = newTicket.Id,
        //                Property = "Developer",
        //                OldValue = _context.Users.Find(oldTicket.DeveloperUserId).FullName,
        //                NewValue = "No Developer Assigned",
        //                Created = DateTimeOffset.Now,
        //                UserId = userId
        //            };
        //            await _context.TicketHistories.AddAsync(history);
        //        }
        //        else
        //        {
        //            TicketHistory history = new TicketHistory
        //            {
        //                TicketId = newTicket.Id,
        //                Property = "Developer",
        //                OldValue = _context.Users.Find(oldTicket.DeveloperUserId).FullName,
        //                NewValue = _context.Users.Find(newTicket.DeveloperUserId).FullName,
        //                Created = DateTimeOffset.Now,
        //                UserId = userId
        //            };
        //            await _context.TicketHistories.AddAsync(history);

        //            New Service will use MS IEmailService
        //            Notification notification = new Notification
        //            {
        //                TicketId = newTicket.Id,
        //                Description = "You have a new ticket.",
        //                Created = DateTime.Now,
        //                SenderId = userId,
        //                RecipientId = newTicket.DeveloperUserId,
        //            };
        //            await _context.Notifications.AddAsync(notification);

        //            Send an Email
        //            string devEmail = newTicket.DeveloperUser.Email;
        //            string subject = "New Ticket Assignment";
        //            string message = $"You have a new ticket for project: {newTicket.Project.Name}";

        //            await _emailSender.SendEmailAsync(devEmail, subject, message);

        //        }

        //    }
        //    await _context.SaveChangesAsync();
        //}

    }
}
