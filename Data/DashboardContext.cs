using Microsoft.EntityFrameworkCore;
using newVisitSuadi2026.Models;

namespace newVisitSuadi2026.Data
{
    public class DashboardContext : DbContext
    {
        public DashboardContext(DbContextOptions<DashboardContext> options)
            : base(options)
        {
        }

        public DbSet<Package> Package { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<TourCompany> TourCompany { get; set; }
        public DbSet<TourGuide> TourGuide { get; set; }
    }
}