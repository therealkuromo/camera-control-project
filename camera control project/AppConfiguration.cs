using Microsoft.Extensions.Configuration;

namespace camera_control_project
{
    public static class AppConfiguration
    {
        public static IConfiguration Configuration { get; }

        static AppConfiguration()
        {
            Configuration =
                new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile(
                        "appsettings.json",
                        optional: false,
                        reloadOnChange: true
                    )
                    .Build();
        }

        public static TrafficSettings TrafficSettings
        {
            get
            {
                return Configuration
                    .GetSection("TrafficSettings")
                    .Get<TrafficSettings>()
                    ?? throw new InvalidOperationException(
                        "TrafficSettings configuration is missing."
                    );
            }
        }
    }
}