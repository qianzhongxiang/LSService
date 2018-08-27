using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;

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
            // var list = Npgsql.NpgsqlConnection.GlobalTypeMapper.Mappings.ToList();
            // var builder = new Npgsql.TypeMapping.NpgsqlTypeMappingBuilder();
            // builder.ClrTypes = new Type[] { typeof(DateTime), typeof(DateTimeOffset) };
            // builder.PgTypeName = "timestamp with time zone";
            // Npgsql.npgsql
            // builder.NpgsqlDbType = NpgsqlDbType.TimestampTz;
            // builder.DbTypes = new System.Data.DbType[] { System.Data.DbType.DateTime, System.Data.DbType.DateTime2, System.Data.DbType.DateTimeOffset };
            // builder.InferredDbType = System.Data.DbType.DateTime;
            // builder.TypeHandlerFactory = list.First(i => i.PgTypeName == "timestamp with time zone").TypeHandlerFactory;
            // Npgsql.NpgsqlConnection.GlobalTypeMapper.AddMapping(builder.Build());

            // Npgsql.NpgsqlConnection.GlobalTypeMapper.RemoveMapping("timestamp");
            locationOptionsBuilder = new DbContextOptionsBuilder<LocationContext>();
            locationOptionsBuilder.UseNpgsql(connectionString);

            profileOptionsBuilder = new DbContextOptionsBuilder<ProfileContext>();
            profileOptionsBuilder.UseNpgsql(connectionString);
            // Npgsql.NpgsqlConnection.GlobalTypeMapper.UnmapComposite<DateTime>();
            // Npgsql.NpgsqlConnection.GlobalTypeMapper.MapComposite<DateTime>("timestamp with time zone");

            //list.First(i => i.PgTypeName == "").ClrTypes = new System.Type[] { typeof(NpgsqlTypes.NpgsqlDateTime) };
            //list.First(i => i.PgTypeName == "").ClrTypes = new System.Type[] { typeof(DateTime), typeof(DateTimeOffset )};

            // while (enumer.MoveNext())
            // {
            //     var mapper = enumer.Current;
            // }
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