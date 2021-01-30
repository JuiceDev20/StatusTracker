using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StatusTracker.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace StatusTracker.Data
{
    public enum Roles
    {
        Admin,
        ProjectManager,
        Developer,
        Submitter,
        NewUser,
        Demo
    }

    public static class ContextSeed
    {
        public static async Task RunSeedMethodAsync(
            RoleManager<IdentityRole> roleManager, 
            UserManager<STUser> userManager,
            ApplicationDbContext context)
        {
            await SeedRolesAsync(roleManager);
            await SeedDefaultUserAsync(userManager);
            await SeedTicketTypesAsync(context);
            await SeedTicketStatusesAsync(context);
            await SeedTicketPrioritiesAsync(context);
            await SeedProjectsAsync(context);
            await SeedProjectUsersAsync(context, userManager);
            await SeedTicketsAsync(context, userManager);
        }
        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.ProjectManager.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Developer.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Submitter.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.NewUser.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Demo.ToString()));

        }

        private static async Task SeedDefaultUserAsync(UserManager<STUser> userManager)
        {
            #region Admin Seed
            var defaultUser = new STUser
            {
                UserName = "ojolmo@gmail.com",
                Email = "ojolmo@gmail.com",
                FirstName = "Orlando",
                LastName = "Olmo",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByNameAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************  ERROR *************");
                Debug.WriteLine("Error Seeding Default Admin User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
                throw;
            }
            #endregion

            #region Demo Admin Seed
            defaultUser = new STUser
            {
                UserName = "Denis@mailinator.com",
                Email = "Denis@mailinator.com",
                FirstName = "Denis",
                LastName = "Jojot",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByNameAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Demo123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Demo.ToString());
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************  ERROR *************");
                Debug.WriteLine("Error Seeding Default DemoAdmin User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
                throw;
            }
            #endregion

            #region ProjectManager1 Seed
            defaultUser = new STUser
            {
                UserName = "beth@mailinator.com",
                Email = "beth@mailinator.com",
                FirstName = "Beth",
                LastName = "Olmo",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByNameAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.ProjectManager.ToString());
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************  ERROR *************");
                Debug.WriteLine("Error Seeding Default PM1 User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
                throw;
            }
            #endregion

            #region ProjectManager2 Seed
            defaultUser = new STUser
            {
                UserName = "alice@mailinator.com",
                Email = "alice@mailinator.com",
                FirstName = "Alice",
                LastName = "Guilfoyle",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByNameAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.ProjectManager.ToString());
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************  ERROR *************");
                Debug.WriteLine("Error Seeding Default PM2 User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
                throw;
            }
            #endregion

            #region DemoProjectManager Seed
            defaultUser = new STUser
            {
                UserName = "Erin@mailinator.com",
                Email = "Erin@mailinator.com",
                FirstName = "Erin",
                LastName = "Crommet",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByNameAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Demo123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.ProjectManager.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Demo.ToString());
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************  ERROR *************");
                Debug.WriteLine("Error Seeding Default DemoPM User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
                throw;
            }
            #endregion

            #region Developer1 Seed
            defaultUser = new STUser
            {
                UserName = "miriam@mailinator.com",
                Email = "miriam@mailinator.com",
                FirstName = "Miriam",
                LastName = "Mendez",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByNameAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Developer.ToString());
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************  ERROR *************");
                Debug.WriteLine("Error Seeding Default Dev1 User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
                throw;
            }
            #endregion

            #region Developer2 Seed
            defaultUser = new STUser
            {
                UserName = "lucy@mailinator.com",
                Email = "lucy@mailinator.com",
                FirstName = "Lucy",
                LastName = "Black",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByNameAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Developer.ToString());
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************  ERROR *************");
                Debug.WriteLine("Error Seeding Default Dev2 User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
                throw;
            }
            #endregion

            #region Developer3 Seed
            defaultUser = new STUser
            {
                UserName = "cindy@mailinator.com",
                Email = "cindy@mailinator.com",
                FirstName = "Cindy",
                LastName = "Kane",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByNameAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Developer.ToString());
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************  ERROR *************");
                Debug.WriteLine("Error Seeding Default Dev3 User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
                throw;
            }
            #endregion

            #region Developer4 Seed
            defaultUser = new STUser
            {
                UserName = "jack@mailinator.com",
                Email = "jack@mailinator.com",
                FirstName = "Jack",
                LastName = "Guilfoyle",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByNameAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Developer.ToString());
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************  ERROR *************");
                Debug.WriteLine("Error Seeding Default Dev4 User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
                throw;
            }
            #endregion

            #region DemoDeveloper Seed
            defaultUser = new STUser
            {
                UserName = "Kit@mailinator.com",
                Email = "Kit@mailinator.com",
                FirstName = "Kit",
                LastName = "Chau",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByNameAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Demo123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Developer.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Demo.ToString());
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************  ERROR *************");
                Debug.WriteLine("Error Seeding Default DemoDev User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
                throw;
            }
            #endregion

            #region Submitter1 Seed
            defaultUser = new STUser
            {
                UserName = "van@mailinator.com",
                Email = "van@mailinator.com",
                FirstName = "Van",
                LastName = "Weinman",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByNameAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Submitter.ToString());
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************  ERROR *************");
                Debug.WriteLine("Error Seeding Default Sub1 User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
                throw;
            }
            #endregion

            #region Submitter2 Seed
            defaultUser = new STUser
            {
                UserName = "phil@mailinator.com",
                Email = "phil@mailinator.com",
                FirstName = "Phil",
                LastName = "Black",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByNameAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Submitter.ToString());
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************  ERROR *************");
                Debug.WriteLine("Error Seeding Default Sub2 User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
                throw;
            }
            #endregion 

            #region Submitter3 Seed
            defaultUser = new STUser
            {
                UserName = "john@mailinator.com",
                Email = "john@mailinator.com",
                FirstName = "John",
                LastName = "Green",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByNameAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Submitter.ToString());
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************  ERROR *************");
                Debug.WriteLine("Error Seeding Default Submitter User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
                throw;
            }
            #endregion

            #region Demo Submitter Seed
            defaultUser = new STUser
            {
                UserName = "Tere@mailinator.com",
                Email = "Tere@mailinator.com",
                FirstName = "Tere",
                LastName = "Olmo",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByNameAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Demo123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Submitter.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Demo.ToString());
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************  ERROR *************");
                Debug.WriteLine("Error Seeding Default DemoSub User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
                throw;
            }
            #endregion

            #region NewUser Seed
            defaultUser = new STUser
            {
                UserName = "gene@mailinator.com",
                Email = "gene@mailinator.com",
                FirstName = "Gene",
                LastName = "Rassmussen",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByNameAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.NewUser.ToString());
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************  ERROR *************");
                Debug.WriteLine("Error Seeding Default NewUser User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
                throw;
            }
            #endregion

            #region DemoNewUser Seed
            defaultUser = new STUser
            {
                UserName = "Tony@mailinator.com",
                Email = "Tony@mailinator.com",
                FirstName = "Tony",
                LastName = "Beavers",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByNameAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Demo123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.NewUser.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Demo.ToString());
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************  ERROR *************");
                Debug.WriteLine("Error Seeding Default DemoNew User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
                throw;
            }
            #endregion

        } 

        private static async Task SeedTicketTypesAsync(ApplicationDbContext context)
        {
            //Seed Low default Ticket-Status ticket
            var defaultSeedUI = new TicketType
            {
                Name = "UI"
            };
            try
            {
                if (!context.TicketTypes.Any(tt => tt.Name == "UI"))
                {
                    await context.TicketTypes.AddAsync(new TicketType { Name = "UI" });
                }
                if (!context.TicketTypes.Any(tt => tt.Name == "Backend"))
                {
                    await context.TicketTypes.AddAsync(new TicketType { Name = "Backend" });
                }
                if (!context.TicketTypes.Any(tt => tt.Name == "Runtime"))
                {
                    await context.TicketTypes.AddAsync(new TicketType { Name = "Runtime" });
                }
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********ERROR*********");
                Debug.WriteLine("Error Seeding Ticket Types.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("************************");
            };
            if (context.TicketTypes.Count() == 0)
            {
                var type1 = new TicketType
                {
                    Name = "UI"
                };
                var type2 = new TicketType
                {
                    Name = "Backend"
                };
                var type3 = new TicketType
                {
                    Name = "Runtime"
                };
                context.TicketTypes.Add(type1);
                context.TicketTypes.Add(type2);
                context.TicketTypes.Add(type3);
                context.SaveChanges();

            };

        }

        private static async Task SeedTicketStatusesAsync(ApplicationDbContext context)
        {
            //Seed Open default Ticket-Status ticket
            var defaultSeedOpen = new TicketStatus
            {
                Name = "Open"
            };
            try
            {
                if (!context.TicketStatuses.Any(ts => ts.Name == "Open"))
                {
                    await context.TicketStatuses.AddAsync(new TicketStatus { Name = "Open" });
                }
                if (!context.TicketStatuses.Any(ts => ts.Name == "Started"))
                {
                    await context.TicketStatuses.AddAsync(new TicketStatus { Name = "Started" });
                }
                if (!context.TicketStatuses.Any(ts => ts.Name == "WIP"))
                {
                    await context.TicketStatuses.AddAsync(new TicketStatus { Name = "WIP" });
                }
                if (!context.TicketStatuses.Any(ts => ts.Name == "Testing"))
                {
                    await context.TicketStatuses.AddAsync(new TicketStatus { Name = "Testing" });
                }
                if (!context.TicketStatuses.Any(ts => ts.Name == "Done"))  
                {
                    await context.TicketStatuses.AddAsync(new TicketStatus { Name = "Done" });
                }
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********ERROR*********");
                Debug.WriteLine("Error Seeding Ticket Status.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("************************");
            };
            if (context.TicketStatuses.Count() == 0)
            {
                var status1 = new TicketStatus
                {
                    Name = "Open"
                };
                var status2 = new TicketStatus
                {
                    Name = "Started"
                };
                var status3 = new TicketStatus
                {
                    Name = "WIP"
                };
                var status4 = new TicketStatus
                {
                    Name = "Testing"
                };
                var status5 = new TicketStatus
                {
                    Name = "Done"
                };
                context.TicketStatuses.Add(status1);
                context.TicketStatuses.Add(status2);
                context.TicketStatuses.Add(status3);
                context.TicketStatuses.Add(status4);
                context.TicketStatuses.Add(status5);
                context.SaveChanges();

            };

        }

        private static async Task SeedTicketPrioritiesAsync(ApplicationDbContext context)
        {
            //Seed Low default Ticket-Priority ticket
            var defaultSeedLow = new TicketPriority
            {
                Name = "Low"
            };
            try
            {
                if (!context.TicketPriorities.Any(tp => tp.Name == "Low"))
                {
                    await context.TicketPriorities.AddAsync(new TicketPriority { Name = "Low" });
                }
                if (!context.TicketPriorities.Any(tp => tp.Name == "Moderate"))
                {
                    await context.TicketPriorities.AddAsync(new TicketPriority { Name = "Moderate" });
                }
                if (!context.TicketPriorities.Any(tp => tp.Name == "High"))
                {
                    await context.TicketPriorities.AddAsync(new TicketPriority { Name = "High" });
                }
                if (!context.TicketPriorities.Any(tp => tp.Name == "Urgent"))
                {
                    await context.TicketPriorities.AddAsync(new TicketPriority { Name = "Urgent" });
                }
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********ERROR*********");
                Debug.WriteLine("Error Seeding Ticket Priorities.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("************************");
            };

            if (context.TicketPriorities.Count() == 0)
            {
                var prior1 = new TicketPriority
                {
                    Name = "Low"
                };
                var prior2 = new TicketPriority
                {
                    Name = "Moderate"
                };
                var prior3 = new TicketPriority
                {
                    Name = "High"
                };
                var prior4 = new TicketPriority
                {
                    Name = "Urgent"
                };
                context.TicketPriorities.Add(prior1);
                context.TicketPriorities.Add(prior2);
                context.TicketPriorities.Add(prior3);
                context.TicketPriorities.Add(prior4);
                context.SaveChanges();

            };

        }

        private static async Task SeedProjectsAsync(ApplicationDbContext context)
        {
            Project seedProject1 = new Project
            {
                Name = "Blog App"
            };
            try
            {
                var newProject = await context.Projects.FirstOrDefaultAsync(p => p.Name == "Blog App");
                if (newProject == null)
                {
                    await context.Projects.AddAsync(seedProject1);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************ERROR*************");
                Debug.WriteLine("Error Seeding Default Project1.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*******************************");
            };

            Project seedProject2 = new Project
            {
                Name = "Status Tracker"
            };
            try
            {
                var newProject = await context.Projects.FirstOrDefaultAsync(p => p.Name == "Status Tracker");
                if (newProject == null)
                {
                    await context.Projects.AddAsync(seedProject2);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************ERROR*************");
                Debug.WriteLine("Error Seeding Default Project2.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*******************************");
            };

            Project seedProject3 = new Project
            {
                Name = "Financial Portal"
            };
            try
            {
                var newProject = await context.Projects.FirstOrDefaultAsync(p => p.Name == "Financial Portal");
                if (newProject == null)
                {
                    await context.Projects.AddAsync(seedProject3);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************ERROR*************");
                Debug.WriteLine("Error Seeding Default Project3.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*******************************");
            };

            Project seedProject4 = new Project
            {
                Name = "Address Book App"
            };
            try
            {
                var newProject = await context.Projects.FirstOrDefaultAsync(p => p.Name == "Address Book App");
                if (newProject == null)
                {
                    await context.Projects.AddAsync(seedProject4);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************ERROR*************");
                Debug.WriteLine("Error Seeding Default Project4.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*******************************");
            };
        }

        private static async Task SeedProjectUsersAsync(ApplicationDbContext context, UserManager<STUser> userManager)
        {
            string adminId = (await userManager.FindByEmailAsync("Denis@mailinator.com")).Id;
            string projectManagerId = (await userManager.FindByEmailAsync("Erin@mailinator.com")).Id;
            string developerId = (await userManager.FindByEmailAsync("Kit@mailinator.com")).Id;
            string submitterId = (await userManager.FindByEmailAsync("Tere@mailinator.com")).Id;
            int project1Id = (await context.Projects.FirstOrDefaultAsync(predicate => predicate.Name == "Blog App")).Id;
            int project2Id = (await context.Projects.FirstOrDefaultAsync(predicate => predicate.Name == "Status Tracker")).Id;
            int project3Id = (await context.Projects.FirstOrDefaultAsync(predicate => predicate.Name == "Financial Portal")).Id;
            int project4Id = (await context.Projects.FirstOrDefaultAsync(predicate => predicate.Name == "Address Book App")).Id;

            ProjectUser projectUser = new ProjectUser
            {
                UserId = adminId,
                ProjectId = project1Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == adminId && r.ProjectId == project1Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************ERROR*************");
                Debug.WriteLine("Error Seeding Default Project1.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*******************************");
                throw;

            };

            projectUser = new ProjectUser
            {
                UserId = projectManagerId,
                ProjectId = project1Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == projectManagerId && r.ProjectId == project1Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************ERROR*************");
                Debug.WriteLine("Error Seeding Default Project1.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*******************************");
                throw;
            };

            projectUser = new ProjectUser
            {
                UserId = developerId,
                ProjectId = project1Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == developerId && r.ProjectId == project1Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************ERROR*************");
                Debug.WriteLine("Error Seeding Default Project1.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*******************************");
                throw;
            };

            projectUser = new ProjectUser
            {
                UserId = submitterId,
                ProjectId = project1Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == submitterId && r.ProjectId == project1Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************ERROR*************");
                Debug.WriteLine("Error Seeding Default Project1.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*******************************");
                throw;
            };

            projectUser = new ProjectUser
            {
                UserId = adminId,
                ProjectId = project2Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == adminId && r.ProjectId == project2Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************ERROR*************");
                Debug.WriteLine("Error Seeding Default Project2.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*******************************");
                throw;

            };

            projectUser = new ProjectUser
            {
                UserId = projectManagerId,
                ProjectId = project2Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == projectManagerId && r.ProjectId == project2Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************ERROR*************");
                Debug.WriteLine("Error Seeding Default Project2.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*******************************");
                throw;

            };

            projectUser = new ProjectUser
            {
                UserId = developerId,
                ProjectId = project2Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == developerId && r.ProjectId == project2Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************ERROR*************");
                Debug.WriteLine("Error Seeding Default Project2.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*******************************");
                throw;

            };

            projectUser = new ProjectUser
            {
                UserId = submitterId,
                ProjectId = project2Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == submitterId && r.ProjectId == project2Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************ERROR*************");
                Debug.WriteLine("Error Seeding Default Project2.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*******************************");
                throw;

            };

            projectUser = new ProjectUser
            {
                UserId = adminId,
                ProjectId = project3Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == adminId && r.ProjectId == project3Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************ERROR*************");
                Debug.WriteLine("Error Seeding Default Project3.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*******************************");
                throw;

            };

            projectUser = new ProjectUser
            {
                UserId = projectManagerId,
                ProjectId = project3Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == projectManagerId && r.ProjectId == project3Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************ERROR*************");
                Debug.WriteLine("Error Seeding Default Project3.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*******************************");
                throw;

            };

            projectUser = new ProjectUser
            {
                UserId = developerId,
                ProjectId = project3Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == developerId && r.ProjectId == project3Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************ERROR*************");
                Debug.WriteLine("Error Seeding Default Project3.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*******************************");
                throw;

            };

            projectUser = new ProjectUser
            {
                UserId = submitterId,
                ProjectId = project3Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == submitterId && r.ProjectId == project3Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************ERROR*************");
                Debug.WriteLine("Error Seeding Default Project3.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*******************************");
                throw;

            };

            projectUser = new ProjectUser
            {
                UserId = adminId,
                ProjectId = project4Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == adminId && r.ProjectId == project4Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************ERROR*************");
                Debug.WriteLine("Error Seeding Default Project4.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*******************************");
                throw;

            };

            projectUser = new ProjectUser
            {
                UserId = projectManagerId,
                ProjectId = project4Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == projectManagerId && r.ProjectId == project4Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************ERROR*************");
                Debug.WriteLine("Error Seeding Default Project4.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*******************************");
                throw;
            };

            projectUser = new ProjectUser
            {
                UserId = developerId,
                ProjectId = project4Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == developerId && r.ProjectId == project4Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************ERROR*************");
                Debug.WriteLine("Error Seeding Default Project4.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*******************************");
                throw;
            };

            projectUser = new ProjectUser
            {
                UserId = submitterId,
                ProjectId = project4Id
            };
            try
            {
                var record = await context.ProjectUsers.FirstOrDefaultAsync(r => r.UserId == submitterId && r.ProjectId == project4Id);
                if (record == null)
                {
                    await context.ProjectUsers.AddAsync(projectUser);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*************ERROR*************");
                Debug.WriteLine("Error Seeding Default Project4.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*******************************");
                throw;
            };

        }

        private static async Task SeedTicketsAsync(ApplicationDbContext context, UserManager<STUser> userManager)
        {
            string developerId = (await userManager.FindByEmailAsync("Denis@mailinator.com")).Id;
            string submitterId = (await userManager.FindByEmailAsync("Tere@mailinator.com")).Id;
            int project1Id = (await context.Projects.FirstOrDefaultAsync(p => p.Name == "Blog App")).Id;
            int project2Id = (await context.Projects.FirstOrDefaultAsync(p => p.Name == "Status Tracker")).Id;
            int project3Id = (await context.Projects.FirstOrDefaultAsync(p => p.Name == "Financial Portal")).Id;
            int project4Id = (await context.Projects.FirstOrDefaultAsync(p => p.Name == "Address Book App")).Id;
            int statusId = (await context.TicketStatuses.FirstOrDefaultAsync(ts => ts.Name == "Open")).Id;
            int typeId = (await context.TicketTypes.FirstOrDefaultAsync(tt => tt.Name == "UI")).Id;
            int priorityId = (await context.TicketPriorities.FirstOrDefaultAsync(tp => tp.Name == "Low")).Id;

            Ticket ticket = new Ticket
            {
                Title = "Need blog posts",
                Description = "Users want more content",
                Created = DateTimeOffset.Now.AddDays(-7),
                Updated = DateTimeOffset.Now.AddHours(-30),
                ProjectId = project1Id,
                TicketPriorityId = priorityId,
                TicketTypeId = typeId,
                TicketStatusId = statusId,
                DeveloperUserId = developerId,
                OwnerUserId = submitterId
            };
            try
            {
                var newTicket = await context.Tickets.FirstOrDefaultAsync(t => t.Title == "Need more blog posts");
                if (newTicket == null)
                {
                    await context.Tickets.AddAsync(ticket);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************* ERROR *************");
                Debug.WriteLine("Error Seeding Blog Ticket 1.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
            };

            ticket = new Ticket
            {
                Title = "New landing page",
                Description = "Minor functionality",
                Created = DateTimeOffset.Now.AddDays(-7),
                Updated = DateTimeOffset.Now.AddHours(-30),
                ProjectId = project2Id,
                TicketPriorityId = priorityId,
                TicketTypeId = typeId,
                TicketStatusId = statusId,
                DeveloperUserId = developerId,
                OwnerUserId = submitterId
            };
            try
            {
                var newTicket = await context.Tickets.FirstOrDefaultAsync(t => t.Title == "New landing page");
                if (newTicket == null)
                {
                    await context.Tickets.AddAsync(ticket);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************* ERROR *************");
                Debug.WriteLine("Error Seeding Tracker Ticket 1.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
            };

            ticket = new Ticket
            {
                Title = "New algorithm",
                Description = "Establish new calculations.",
                Created = DateTimeOffset.Now.AddDays(-7),
                Updated = DateTimeOffset.Now.AddHours(-30),
                ProjectId = project3Id,
                TicketPriorityId = priorityId,
                TicketTypeId = typeId,
                TicketStatusId = statusId,
                DeveloperUserId = developerId,
                OwnerUserId = submitterId
            };
            try
            {
                var newTicket = await context.Tickets.FirstOrDefaultAsync(t => t.Title == "Algo for transfers");
                if (newTicket == null)
                {
                    await context.Tickets.AddAsync(ticket);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************* ERROR *************");
                Debug.WriteLine("Error Seeding FinPort Ticket 1.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
            };

            ticket = new Ticket
            {
                Title = "UI & UX",
                Description = "Edit view is incomplete",
                Created = DateTimeOffset.Now.AddDays(-14),
                Updated = DateTimeOffset.Now.AddHours(-60),
                ProjectId = project1Id,
                TicketPriorityId = priorityId,
                TicketTypeId = typeId,
                TicketStatusId = statusId,
                DeveloperUserId = developerId,
                OwnerUserId = submitterId
            };
            try
            {
                var newTicket = await context.Tickets.FirstOrDefaultAsync(t => t.Title == "UI & UX");
                if (newTicket == null)
                {
                    await context.Tickets.AddAsync(ticket);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************* ERROR *************");
                Debug.WriteLine("Error Seeding Blog Ticket 2.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
            };

            ticket = new Ticket
            {
                Title = "Runtime Issues",
                Description = "Not completing the compilation.",
                Created = DateTimeOffset.Now.AddDays(-7),
                Updated = DateTimeOffset.Now.AddHours(-30),
                ProjectId = project2Id,
                TicketPriorityId = priorityId,
                TicketTypeId = typeId,
                TicketStatusId = statusId,
                DeveloperUserId = developerId,
                OwnerUserId = submitterId
            };
            try
            {
                var newTicket = await context.Tickets.FirstOrDefaultAsync(t => t.Title == "Runtime Issues");
                if (newTicket == null)
                {
                    await context.Tickets.AddAsync(ticket);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************* ERROR *************");
                Debug.WriteLine("Error Seeding Tracker Ticket 2.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
            };

            ticket = new Ticket
            {
                Title = "Backend",
                Description = "Missing functionality",
                Created = DateTimeOffset.Now.AddDays(-5),
                Updated = DateTimeOffset.Now.AddHours(-15),
                ProjectId = project3Id,
                TicketPriorityId = priorityId,
                TicketTypeId = typeId,
                TicketStatusId = statusId,
                DeveloperUserId = developerId,
                OwnerUserId = submitterId
            };
            try
            {
                var newTicket = await context.Tickets.FirstOrDefaultAsync(t => t.Title == "Backend");
                if (newTicket == null)
                {
                    await context.Tickets.AddAsync(ticket);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************* ERROR *************");
                Debug.WriteLine("Error Seeding FinPort Ticket 2.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
            };

            ticket = new Ticket
            {
                Title = "New Login Protocols",
                Description = "Update with Demo users",
                Created = DateTimeOffset.Now.AddDays(-14),
                Updated = DateTimeOffset.Now.AddHours(-60),
                ProjectId = project1Id,
                TicketPriorityId = priorityId,
                TicketTypeId = typeId,
                TicketStatusId = statusId,
                DeveloperUserId = developerId,
                OwnerUserId = submitterId
            };
            try
            {
                var newTicket = await context.Tickets.FirstOrDefaultAsync(t => t.Title == "New Login Protocols");
                if (newTicket == null)
                {
                    await context.Tickets.AddAsync(ticket);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************* ERROR *************");
                Debug.WriteLine("Error Seeding ABook Ticket 1.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
            };

            ticket = new Ticket
            {
                Title = "Add a modal",
                Description = "Edit view is incomplete",
                Created = DateTimeOffset.Now.AddDays(-14),
                Updated = DateTimeOffset.Now.AddHours(-60),
                ProjectId = project1Id,
                TicketPriorityId = priorityId,
                TicketTypeId = typeId,
                TicketStatusId = statusId,
                DeveloperUserId = developerId,
                OwnerUserId = submitterId
            };
            try
            {
                var newTicket = await context.Tickets.FirstOrDefaultAsync(t => t.Title == "Add modal");
                if (newTicket == null)
                {
                    await context.Tickets.AddAsync(ticket);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************* ERROR *************");
                Debug.WriteLine("Error Seeding ABook Ticket 2.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("*********************************");
            };


        }

    }
}
   
