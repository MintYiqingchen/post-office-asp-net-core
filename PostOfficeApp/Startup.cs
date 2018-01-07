using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostOfficeApp.Data;
using PostOfficeApp.Models;
using PostOfficeApp.Services;
using PostOffice.Models;
using Microsoft.AspNetCore.Authorization;
using PostOfficeApp.Authorization;
using System;

namespace PostOfficeApp
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
            /*var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionBuilder.UseMySQL(Configuration.GetConnectionString("MySQL"));
            services.AddSingleton(optionBuilder.Options);*/

            services.AddDbContext<ApplicationDbContext>(options=>
                options.UseMySQL(Configuration.GetConnectionString("MySQL")));
            // is every table need a context ?
            services.AddDbContext<MovieDbContext>(options => 
                options.UseMySQL(Configuration.GetConnectionString("MySQL")));
            services.AddDbContext<AddressDbContext>(options =>
               options.UseMySQL(Configuration.GetConnectionString("MySQL")));
            services.AddDbContext<NewspaperDbContext>(options =>
                options.UseMySQL(Configuration.GetConnectionString("MySQL")));
            services.AddDbContext<OrdersDbContext>(options =>
                options.UseMySQL(Configuration.GetConnectionString("MySQL")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options=>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddSingleton<IAuthorizationHandler, AdministratorArthorizationHandler>();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            // add cookie services
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "postofficecookie";
                options.Cookie.HttpOnly = true;
            });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            /*
            // get manager seed passwd
            var testUserPw = Configuration["SeedUserPW"];
            if (string.IsNullOrEmpty(testUserPw))
            {
                throw new System.Exception("use dotnet user-secrets set SeedUserPW <pw>");
            }
            else
            {
                System.Console.WriteLine(testUserPw);
            }

            // initialize administrator of this application
            try
            {
                var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
                using (var scope = scopeFactory.CreateScope())
                {
                    var dbcontext = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    var myService = scope.ServiceProvider.GetService<IServiceProvider>();
                    SeedDB.Initialize(myService, dbcontext, testUserPw).Wait();
                }
            }
            catch
            {
                System.Console.WriteLine("seed database error");
            }*/
            
        }
    }
}
