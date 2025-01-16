using Azure.Communication.Email;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace RoosterPlanner.Email;

public static class ServiceCollectionExtensions
{
    public static IHostApplicationBuilder AddEmailServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddTransient<IEmailService, AzureEmailServicesClient>();
        builder.Services.Configure<ACSConfig>(builder.Configuration.GetSection(key: nameof(ACSConfig)));

        builder.Services.AddSingleton(sp =>
        {
            var config = sp.GetRequiredService<IOptions<ACSConfig>>();
            return new EmailClient(config.Value.ConnectionString);
        });


        return builder;
    }
}