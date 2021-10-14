using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BackgroundEmailService.Data.Repository;
using BackgroundEmailService.MVC.Hubs;
using BackgroundEmailService.MVC.Services;
using BackgroundEmailService.Service.EmailProviderService;
using BackgroundEmailService.Service.EmailService;
using BackgroundEmailService.Service.EmailService.SendEmailAdapter;

namespace BackgroundEmailService.MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<ISendEmail, SendEmail>();
            services.AddTransient<IEmailProviderService, EmailProviderService>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddSingleton<EmailHub>();
            services.AddSingleton<EmailTaskService>();
            services.AddHostedService(provider => provider.GetService<EmailTaskService>());
            services.AddSignalR();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<EmailHub>("/hubs/emailHub");
            });
        }
    }
}
