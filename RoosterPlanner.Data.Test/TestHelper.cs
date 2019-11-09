using System;
using Microsoft.Extensions.Configuration;

namespace RoosterPlanner.Data
{
    public class TestHelper
    {
        public TestHelper()
        {
        }

        public static IConfigurationRoot GetIConfigurationRoot(string outputPath)
        {
            return new ConfigurationBuilder()
                .SetBasePath(outputPath)
                .AddJsonFile("appsettings.Test.json", optional: true)
                .Build();
        }

        public static ConnectionStringsConfig GetConnectionStringConfiguration(string outputPath)
        {
            ConnectionStringsConfig configuration = new ConnectionStringsConfig();

            var iConfig = GetIConfigurationRoot(outputPath);
            iConfig.GetSection("ConnectionStrings").Bind(configuration);

            return configuration;
        }
    }

    public class ConnectionStringsConfig
    {
        public string RoosterPlannerDatabase { get; set; }
    }
}
