using Microsoft.EntityFrameworkCore.Design;
using System;

namespace DONN.LS.DBHelperSingle
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    public class LocationContextFactory : IDesignTimeDbContextFactory<LocationContext>
    {
        public LocationContext CreateDbContext(string[] args)
        {
            return Provider.Instance("key", "Host=localhost;Database=demo;Username=postgres;Password=postgis", Providers.Pgsql).LocationContext; ;
        }
    }
}