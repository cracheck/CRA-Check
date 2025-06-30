using CRA_Check.Models;
using Microsoft.EntityFrameworkCore;

namespace CRA_Check.Data
{
    public class AppDbContext : DbContext
    {
        private readonly string _databaseFilename;
        public DbSet<ProjectInformation> ProjectInformation { get; set; }
        public DbSet<Software> Softwares { get; set; }
        public DbSet<Release> Releases { get; set; }
        public DbSet<Vulnerability> Vulnerabilities { get; set; }

        public AppDbContext(string databaseFilename)
        {
            _databaseFilename = databaseFilename;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder _OptionBuilder)
        {
            _OptionBuilder.UseSqlite($"Data Source={_databaseFilename}");
        }

        protected override void OnModelCreating(ModelBuilder _ModelBuilder)
        {
            _ModelBuilder.Entity<Software>()
                .HasMany(s => s.Release)
                .WithOne(r => r.Software)
                .HasForeignKey(r => r.SoftwareId)
                .OnDelete(DeleteBehavior.Cascade);

            _ModelBuilder.Entity<Release>()
                .HasMany(r => r.Vulnerabilities)
                .WithOne(v => v.Release)
                .HasForeignKey(v => v.ReleaseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
