
using DONN.LS.Entities;
using Microsoft.EntityFrameworkCore;

namespace DONN.LS.DBHelperSingle
{
    public partial class LocationContext : DbContext
    {
        private static readonly string table_schema = "public";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        public LocationContext(DbContextOptions<LocationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ModelOptions(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        public void ModelOptions(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(table_schema);
            modelBuilder.Entity<TempLocations>().HasIndex(t => t.SendTime);
            modelBuilder.Entity<DeviceProfile>().Property(p => p.UpdateTime).IsConcurrencyToken();
            modelBuilder.Entity<DeviceProfile>().HasKey(c => new { c.Uid, c.Type });
        }

        public virtual DbSet<TempLocations> TempLocations { get; set; }
        public virtual DbSet<DeviceProfile> DeviceProfile { get; set; }
    }

}