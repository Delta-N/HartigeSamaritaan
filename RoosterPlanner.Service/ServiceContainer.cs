using System;
using System.Net;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoosterPlanner.Common.Config;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Context;
using RoosterPlanner.Email;

namespace RoosterPlanner.Service
{
    public static class ServiceContainer
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddDbContext<RoosterPlannerContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("RoosterPlannerDatabase")));
            services.BuildServiceProvider().GetService<RoosterPlannerContext>()?.Database.Migrate();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            

            EmailConfig config = new EmailConfig();
            configuration.Bind(EmailConfig.ConfigSectionName,config);
            SmtpClient smtpClient = new SmtpClient(config.SMTPadres)
            {
                Port = config.Port,
                Credentials = new NetworkCredential(config.Emailadres, config.Password),
                EnableSsl = config.EnableSsl
            };
            services.AddScoped<IEmailService>(s => new SMTPEmailService(smtpClient,config.Emailadres));
        }
    }
}