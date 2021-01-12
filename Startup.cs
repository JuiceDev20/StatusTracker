using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StatusTracker.Data;
using StatusTracker.Models;
using StatusTracker.Services;
using StatusTracker.Utilities;

namespace StatusTracker
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {   //This adds a service that alows us to communicate with the DB
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(DataHelper.GetConnectionString(Configuration)));

            services.AddIdentity<STUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()  //Added to support new identities
                .AddDefaultTokenProviders();  //Added to support new identities

            services.AddScoped<ISTRolesService, STRolesService>();  //New service interface to assign roles
            services.AddScoped<ISTProjectService, STProjectService>(); //New service interface to assign projects
            services.AddScoped<ISTHistoryService, STHistoryService>(); //New service interface to track ticket histories
            services.AddScoped<ISTAccessService, STAccessService>(); //New service interface to grant access to tickets by role
            services.AddScoped<ISTFileService, STFileService>();  //New service interface to accept various attachment formats

            services.Configure<MailSettings>(Configuration.GetSection("MailSettings")); //Email services 44 + 45
            services.AddTransient<IEmailSender, EmailService>();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Landing}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
