using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace RoosterPlanner.Api {
    public class Program {
        public static void Main(string[] args) {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();

            Log.Information("Starting Roosterplanner API");

            CreateHostBuilder(args).UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            )
            .Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) {


            return Host.CreateDefaultBuilder(args)
            .ConfigureLogging(logging => {
                logging.ClearProviders();
                logging.AddConsole();
            })
            .ConfigureAppConfiguration((context, config) => {
                var configuration = config.Build();
                // var azureServiceTokenProvider = new AzureServiceTokenProvider();
                // var keyVaultClient = new KeyVaultClient(
                //     new KeyVaultClient.AuthenticationCallback(
                //         azureServiceTokenProvider.KeyVaultTokenCallback));
                //
                // config.AddAzureKeyVault(
                //     $"https://{configuration["KeyVaultName"]}.vault.azure.net/",
                //     keyVaultClient,
                //     new DefaultKeyVaultSecretManager());
            }).ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
            ;
        }
    }
}
