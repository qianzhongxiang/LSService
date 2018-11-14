using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;

namespace DONN.LS.DBHelperSingle
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