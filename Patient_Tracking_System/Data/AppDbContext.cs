using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Patient_Tracking_System.Models;


namespace PostgreSQL.Data
{
    public class AppDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public AppDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to postgres with connection string from app settings
            options.UseNpgsql(Configuration.GetConnectionString("WebApiDatabase"));
        }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Visit> Visits { get; set; }

        public DbSet<Patient_Tracking_System.Models.Patient> Patient { get; set; } = default!;
    }
}