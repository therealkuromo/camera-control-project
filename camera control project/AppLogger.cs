using Microsoft.Extensions.Logging;

namespace camera_control_project
{
    public static class AppLogger
    {
        public static ILogger Logger { get; }

        static AppLogger()
        {
            using ILoggerFactory factory =
                LoggerFactory.Create(builder =>
                {
                    builder
                        .SetMinimumLevel(
                            LogLevel.Information
                        )
                        .AddConsole();
                });

            Logger =
                factory.CreateLogger(
                    "TrafficControl"
                );
        }
    }
}