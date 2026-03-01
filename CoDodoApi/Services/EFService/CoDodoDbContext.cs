using CoDodoApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoDodoApi.Services.EFService
{
    public class CoDodoDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<EFProcess> Processes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //The inmemory db seems to be using Name + UriForAssignment as the key
            //So i do the same
            modelBuilder.Entity<EFProcess>(entity =>
            {
                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(e => e.UriForAssignment)
                      .IsRequired()
                      .HasMaxLength(500);

                entity.HasKey(e => new { e.Name, e.UriForAssignment });

                entity.HasIndex(e => new { e.Name, e.UriForAssignment })
                      .IsUnique();
            });
            base.OnModelCreating(modelBuilder);
        }

        public async Task<Process[]> GetAllProcesses(TimeProvider timeProvider)
        {
            EFProcess[] efProcesses = await Processes.ToArrayAsync();
            Process[] processes = [.. efProcesses.Select(efProcess => efProcess.ToProcess(timeProvider))];

            return processes;
        }

        public async Task<EFProcess?> GetProcessByKey(string name, string uriForAssignment)
        {
            return await Processes.FindAsync(name, uriForAssignment);
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
