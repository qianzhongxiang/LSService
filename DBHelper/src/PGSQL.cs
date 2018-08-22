using System.Collections.Generic;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace DONN.LS.DBHelper
{
    class PGSQL : Base
    {
        public PGSQL(string connectionString) : base(connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new System.ArgumentException("message", nameof(connectionString));
            }

        }

        protected override void InitDbContextOptionsBuilder(string connectionString)
        {
            locationOptionsBuilder = new DbContextOptionsBuilder<LocationContext>();
            locationOptionsBuilder.UseNpgsql(connectionString);

            profileOptionsBuilder = new DbContextOptionsBuilder<ProfileContext>();
            profileOptionsBuilder.UseNpgsql(connectionString);
        }

        protected override ParameterBuilder ParameterBuilder(IEnumerable<object[]> items)
        {
            var builder = new ParameterBuilder<Npgsql.NpgsqlParameter>();
            foreach (var item in items)
            {
                builder.Add(item[0] as string, item[1]);
            }
            return builder;
        }
    }
}