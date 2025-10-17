using CRA_Check.Models;
using Microsoft.EntityFrameworkCore;

namespace CRA_Check.Data
{
    /// <summary>
    /// Database context for workspace SQLite database
    /// </summary>
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        /// <summary>
        /// Database filename
        /// </summary>
        private readonly string _databaseFilename;

        /// <summary>
        /// Workspace information DBSet
        /// </summary>
        public DbSet<WorkspaceInformation> WorkspaceInformation { get; set; }

        /// <summary>
        /// Softwares DBSet
        /// </summary>
        public DbSet<Software> Softwares { get; set; }

        /// <summary>
        /// Releases DBSet
        /// </summary>
        public DbSet<Release> Releases { get; set; }

        /// <summary>
        /// Components DBSet
        /// </summary>
        public DbSet<Component> Components { get; set; }

        /// <summary>
        /// Vulnerabilites DBSet
        /// </summary>
        public DbSet<Vulnerability> Vulnerabilities { get; set; }

        /// <summary>
        /// Ratings DBSet
        /// </summary>
        public DbSet<Rating> Ratings { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="databaseFilename">Database filename</param>
        public DbContext(string databaseFilename)
        {
            _databaseFilename = databaseFilename;
        }

        /// <summary>
        /// On configuring event. Configure the database file
        /// </summary>
        /// <param name="optionBuilder">Option builder</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.UseSqlite($"Data Source={_databaseFilename}");
        }

        /// <summary>
        /// On model creating. Create the database structure
        /// </summary>
        /// <param name="modelBuilder">Model builder</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Software>()
                .HasMany(s => s.Releases)
                .WithOne(r => r.Software)
                .HasForeignKey(r => r.SoftwareId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Release>()
                .HasMany(r => r.Components)
                .WithOne(v => v.Release)
                .HasForeignKey(v => v.ReleaseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Component>()
                .HasMany(r => r.Vulnerabilities)
                .WithMany(v => v.Components)
                .UsingEntity(j => j.ToTable("ComponentVulnerabilities"));

            modelBuilder.Entity<Vulnerability>()
                .HasMany(r => r.Ratings)
                .WithOne(v => v.Vulnerability)
                .HasForeignKey(v => v.VulnerabilityId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
