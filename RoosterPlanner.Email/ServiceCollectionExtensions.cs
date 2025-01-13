using Azure.Communication.Email;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace RoosterPlanner.Email;

public static class ServiceCollectionExtensions
{
    public static IHostApplicationBuilder AddEmailServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddTransient<IEmailService, AzureEmailServicesClient>();
        builder.Services.AddSingleton(sp =>
        {
            var config = sp.GetRequiredService<ACSConfig>();
            return new EmailClient(config.ConnectionString);
        });

        builder.Services.Configure<ACSConfig>(builder.Configuration.GetSection(key: nameof(ACSConfig)));

        return builder;
    }
}