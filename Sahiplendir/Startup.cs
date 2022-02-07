using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sahiplendir.Models;
using Sahiplendir.Sahiplendir;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sahiplendir
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
        {
            services.AddControllersWithViews();

            services.AddDbContext<AppDbContext>(config =>
            {
                config.UseSqlServer(Configuration.GetConnectionString("SqlServer"));
            });

            services.AddIdentity<User, Role>(config =>
            {
                config.Password.RequireDigit = Configuration.GetValue<bool>("App:Security:RequireDigit");
                config.Password.RequiredLength = Configuration.GetValue<int>("App:Security:RequiredLength");
                config.Password.RequireLowercase = Configuration.GetValue<bool>("App:Security:RequireLowercase");
                config.Password.RequireNonAlphanumeric = Configuration.GetValue<bool>("App:Security:RequireNonAlphanumeric");
                config.Password.RequireUppercase = Configuration.GetValue<bool>("App:Security:RequireUppercase");
                //Configuration ayar dosyasýndan alýp kullanmaya yarýyor.

                config.Lockout.DefaultLockoutTimeSpan = Configuration.GetValue<TimeSpan>("App:Security:DefaultLockoutTimeSpan");
                config.Lockout.MaxFailedAccessAttempts = Configuration.GetValue<int>("App:Security:MaxFailedAccessAttempts");

            }
                )
                .AddEntityFrameworkStores<AppDbContext>();
            services.AddSingleton<UtilsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            AppDbContext context,
            RoleManager<Role> roleManager,
            UserManager<User> userManager
            )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
              );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            context.Database.Migrate();

            roleManager.CreateAsync(new Role { Name = "Administrators" }).Wait();
            roleManager.CreateAsync(new Role { Name = "Members" }).Wait();

            var user = new User
            {
                Name = Configuration.GetValue<string>("App:Security:DefaultAdmin:Name"),
                UserName = Configuration.GetValue<string>("App:Security:DefaultAdmin:EMail"),
                EmailConfirmed = true,
                DateCreated = DateTime.Now,
                Email = Configuration.GetValue<string>("App:Security:DefaultAdmin:EMail"),
            };

            userManager.CreateAsync(user, Configuration.GetValue<string>("App:Security:DefaultAdmin:Password")).Wait();
            userManager.AddToRoleAsync(user, "Administrators").Wait();
        }
    }
}
