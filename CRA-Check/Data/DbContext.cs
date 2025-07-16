using CRA_Check.Models;
using Microsoft.EntityFrameworkCore;

namespace CRA_Check.Data
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        private readonly string _databaseFilename;
        public DbSet<WorkspaceInformation> WorkspaceInformation { get; set; }
        public DbSet<Software> Softwares { get; set; }
        public DbSet<Release> Releases { get; set; }
        public DbSet<Vulnerability> Vulnerabilities { get; set; }
        public DbSet<Cvss> Cvsses { get; set; }

        public DbContext(string databaseFilename)
        {
            _databaseFilename = databaseFilename;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.UseSqlite($"Data Source={_databaseFilename}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Software>()
                .HasMany(s => s.Releases)
                .WithOne(r => r.Software)
                .HasForeignKey(r => r.SoftwareId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Release>()
                .HasMany(r => r.Vulnerabilities)
                .WithOne(v => v.Release)
                .HasForeignKey(v => v.ReleaseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Vulnerability>()
                .HasMany(r => r.CvssList)
                .WithOne(v => v.Vulnerability)
                .HasForeignKey(v => v.VulnerabilityId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
