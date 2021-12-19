using Microsoft.EntityFrameworkCore;

namespace test_app.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Facility> Facility { get; set; }
        public DbSet<FacilityStatus> FacilityStatus { get; set;}

        public ApplicationContext( DbContextOptions<ApplicationContext> options)
            : base(options) => Database.EnsureCreated();
    }
}
