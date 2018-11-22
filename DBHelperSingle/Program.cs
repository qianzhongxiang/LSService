using DONN.LS.DBHelperSingle;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBHelperSingle
{
    public class LocationContextFactory : IDesignTimeDbContextFactory<LocationContext>
    {
        public LocationContext CreateDbContext(string[] args)
        {
            return Provider.Instance("key", "Host=192.168.8.36;Port=27866;Database=locationTest;Username=postgres;Password=postgres", Providers.Pgsql).LocationContext; ;
        }
    }
    class Program
    {
        public void Main(object[] args) {
        }
    }
}
