
using DONN.LS.ENTITIES;
using Microsoft.EntityFrameworkCore;

namespace DONN.LS.DBHelper
{
    public partial class LocationContext : DbContext
    {
        // private static readonly string prefix = "TL_";
        private static readonly string table_schema = "public";
        // public static string NowStr { get; set; }
        // public DbCompiledModel Model;

        private string _tableName;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        public LocationContext(DbContextOptions<LocationContext> options, string tableName) : base(options)
        {
            _tableName = tableName;
            //this.Configuration.LazyLoadingEnabled = false;
            //this.Configuration.ProxyCreationEnabled = false;

            ////Helpful for debugging            
            //this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }
        // public Db_Entities(string nameOrConnectionString, string tableName, DbCompiledModel model) : base(nameOrConnectionString: nameOrConnectionString, model: model)
        // {
        //     this.Model = model;
        //     //this.Configuration.LazyLoadingEnabled = false;
        //     //this.Configuration.ProxyCreationEnabled = false;
        //     ////Helpful for debugging            
        //     //this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        // }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ModelOptions(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        public void ModelOptions(ModelBuilder modelBuilder)
        {
            // Database.SetInitializer<Db_Entities>(null);
            modelBuilder.HasDefaultSchema(table_schema);
            modelBuilder.Entity<TempLocations>().ToTable(_tableName).HasKey(c => c.Id);
            // modelBuilder.Properties().Configure(c =>
            // {
            //     var name = c.ClrPropertyInfo.Name;
            //     var newName = name.ToLower();
            //     c.HasColumnName(newName);
            // });
        }
        // private static Dictionary<string, DbCompiledModel> modelCache = new Dictionary<string, DbCompiledModel>();
        // public static DbCompiledModel CreateModel(string nameOrConnectionString)
        // {
        //     var tName = GetTableName();
        //     if (modelCache.ContainsKey(tName)) return modelCache[tName];
        //     modelCache.Clear();
        //     DbModelBuilder builder = new DbModelBuilder();
        //     // Setup configurations
        //     nameOrConnectionString = ConfigurationManager.ConnectionStrings[nameOrConnectionString].ConnectionString;
        //     var connection = new NpgsqlConnectionFactory().CreateConnection(nameOrConnectionString);
        //     ModelOptions(builder);
        //     DbModel model = builder.Build(connection);
        //     DbCompiledModel compiledModel = modelCache[tName] = model.Compile();
        //     return compiledModel;
        // }
        // public static int Day { get; set; }

        // private static string GetTableName()
        // {
        //     var now = DateTime.Now;
        //     NowStr = now.ToString("yyyyMMdd");
        //     Day = int.Parse(NowStr);
        //     DayChange(Day);
        //     return $"{prefix}{NowStr}";
        // }
        public virtual DbSet<TempLocations> TempLocations { get; set; }
    }

    public partial class ProfileContext : DbContext
    {
        private static readonly string table_schema = "public";
        public ProfileContext(DbContextOptions<ProfileContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ModelOptions(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        public static void ModelOptions(ModelBuilder modelBuilder)
        {
            // Database.SetInitializer<Dev_Db_Entities>(null);
            modelBuilder.HasDefaultSchema(table_schema);
            modelBuilder.Entity<DeviceProfile>().HasKey(c => new { c.Uid, c.Type });
            // modelBuilder.Properties().Configure(c =>
            // {
            //     var name = c.ClrPropertyInfo.Name;
            //     var newName = name.ToLower();
            //     c.HasColumnName(newName);
            // });
        }

        // public static DbCompiledModel CreateModel(string nameOrConnectionString)
        // {
        //     DbModelBuilder builder = new DbModelBuilder();
        //     // Setup configurations
        //     nameOrConnectionString = ConfigurationManager.ConnectionStrings[nameOrConnectionString].ConnectionString;
        //     var connection = new NpgsqlConnectionFactory().CreateConnection(nameOrConnectionString);
        //     ModelOptions(builder);
        //     DbModel model = builder.Build(connection);
        //     DbCompiledModel compiledModel = model.Compile();
        //     return compiledModel;
        // }

        public virtual DbSet<DeviceProfile> DeviceProfile { get; set; }
    }
}