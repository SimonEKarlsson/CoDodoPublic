using Microsoft.EntityFrameworkCore;

namespace CoDodoApi.Services.EFService
{
    public class CoDodoDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<EFProcess> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }

    public static class AddCoDodoDbContextExtension
    {
        public static void AddCoDodoDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<CoDodoDbContext>(options =>
                options.UseSqlServer(connectionString));
        }
    }
}
