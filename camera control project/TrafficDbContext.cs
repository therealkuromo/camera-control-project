using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace camera_control_project
{
    public class TrafficDbContext : DbContext
    {
        public DbSet<TrafficEvent> TrafficEvents { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            string configuredPath =
                AppConfiguration.Configuration
                    .GetConnectionString(
                        "DefaultConnection"
                    )
                ?? throw new InvalidOperationException(
                    "Database connection string is missing."
                );

            string databasePath =
                Path.Combine(
                    AppContext.BaseDirectory,
                    configuredPath
                );

            optionsBuilder.UseSqlite(
                $"Data Source={databasePath}"
            );
        }
    }
}