using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using DONN.LS.Entities;
using Microsoft.EntityFrameworkCore;
using DONN.Tools.Logger;
using System.Data.Common;
using System.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace DONN.LS.DBHelper
{
    public abstract class Base
    {
        protected readonly string prefix = "TL_";
        protected readonly string table_schema = "public";

        private int switcher = 0;
        private List<TempLocations>[] data = new List<TempLocations>[] { new List<TempLocations>(), new List<TempLocations>() };
        private LocationContext locationContext = null;
        private ProfileContext profileContext = null;
        protected DbContextOptionsBuilder<LocationContext> locationOptionsBuilder;
        protected DbContextOptionsBuilder<ProfileContext> profileOptionsBuilder;
        public string TableName { get => prefix + TableNo; }
        public Base(string connectionString)
        {
            InitDbContextOptionsBuilder(connectionString);
            InitProfile();
        }
        private void InitProfile()
        {
            profileContext = CreateProfileContext().Result;
        }
        /// <summary>
        /// Get local volume of table for testing
        /// </summary>
        /// <param name="type">l|p</param>
        /// <returns></returns>
        public int GetVolume(string type)
        {
            switch (type)
            {
                case "l":
                    return locationContext.TempLocations.Local.Count;
                case "p":
                    return profileContext.DeviceProfile.Local.Count;
                default:
                    return 0;
            }
        }
        protected abstract void InitDbContextOptionsBuilder(string connectionString);
        protected virtual async Task<LocationContext> CreateLoactionContext()
        {
            try
            {
                var context = new LocationContext(locationOptionsBuilder.Options, TableName);
                context.ChangeTracker.AutoDetectChangesEnabled = false;
                context.Database.AutoTransactionsEnabled = false;
                RelationalDatabaseCreator databaseCreator = (RelationalDatabaseCreator)context.Database.GetService<IDatabaseCreator>();
                var now = DateTime.Now;
                if (!TableExisted(context, $"{prefix}{int.Parse(now.ToString("yyyyMMdd"))}"))
                    await databaseCreator.CreateTablesAsync();
                return context;
            }
            catch (Exception e)
            {
                LogHelper.Error(e.ToString(), e);
            }
            return null;
        }
        private SemaphoreSlim ssProfile = new SemaphoreSlim(1, 1);
        protected virtual async Task<ProfileContext> CreateProfileContext()
        {
            try
            {
                var context = new ProfileContext(profileOptionsBuilder.Options);
                context.Database.AutoTransactionsEnabled = true;
                RelationalDatabaseCreator databaseCreator = (RelationalDatabaseCreator)context.Database.GetService<IDatabaseCreator>();
                await ssProfile.WaitAsync();
                if (!TableExisted(context, "DeviceProfile"))
                    await databaseCreator.CreateTablesAsync();
                context.DeviceProfile.Load();
                ssProfile.Release();
                return context;
            }
            catch (Exception e)
            {
                LogHelper.Error(e.ToString(), e);
            }
            return null;
        }
        public virtual void UpdateItems(IEnumerable<TempLocations> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            foreach (var item in items)
            {
                item.CollectTime = item.CollectTime.ToUniversalTime();
                item.SendTime = item.SendTime?.ToUniversalTime();
                data[switcher].Add(item);
            }
        }

        private SemaphoreSlim semaphore4Location = new SemaphoreSlim(1, 1);
        /// <summary>
        /// 需要改进性能
        /// </summary>
        /// <returns></returns>
        public virtual async Task<int> SaveItemsChangeAsync()
        {
            try
            {
                await semaphore4Location.WaitAsync();
                //renew db context
                var str = DateTime.Now.ToString("yyyyMMdd");
                if (str != TableNo.ToString())
                {
                    TableNo = int.Parse(str);
                    if (locationContext != null) locationContext.Dispose();
                    locationContext = await CreateLoactionContext();
                }
                locationContext.TempLocations.Local.ToList().ForEach(l =>
                {
                    var entry = locationContext.Entry(l);
                    if (entry.State == EntityState.Unchanged)
                        entry.State = EntityState.Detached;
                });
                switcher = (switcher + 1) % 2;
                locationContext.TempLocations.AddRange(data[1 - switcher]);
                locationContext.ChangeTracker.DetectChanges();
                var c = locationContext.SaveChanges();
                return c;
            }
            catch (Exception e)
            {
                LogHelper.Error(e.ToString(), e);
            }
            finally
            {
                data[1 - switcher].Clear();
                if (semaphore4Location.CurrentCount == 0)
                    semaphore4Location.Release();
            }
            return 0;
        }
        public virtual async Task<int> SaveProfilesChangeAsync()
        {
            try
            {
                return profileContext.SaveChanges();
            }
            catch (Exception e)
            {
                LogHelper.Error("SaveProfilesChangeAsync:failed", e);
            }
            finally
            {

            }
            return 0;
        }
        public virtual void UpdateProfile(IEnumerable<DeviceProfile> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            ssProfile.Wait();
            foreach (var item in items)
            {
                var previous = profileContext.DeviceProfile.FirstOrDefault(i => i.Uid == item.Uid && i.Type == item.Type);
                item.UpdateTime = item.UpdateTime.ToUniversalTime();
                if (previous == null)
                    profileContext.DeviceProfile.Add(item);
                else if (item != previous)
                {
                    profileContext.Entry(previous).State = EntityState.Detached;
                    profileContext.DeviceProfile.Update(item);
                }
            }
            ssProfile.Release();
        }
        private int _tableNo = 0;
        public int TableNo { get => _tableNo; private set => _tableNo = value; }
        public IEnumerable<string> GetExistedTableName(DateTime sTime, DateTime eTime)
        {
            var days = new List<string>();
            int s = int.Parse(sTime.ToString("yyyyMMdd")), e = int.Parse(eTime.ToString("yyyyMMdd"));
            for (int i = s; i <= e; i++)
            {
                days.Add($"'{prefix}{i}'");
            }

            var tableNQeryStr = $"SELECT table_name FROM information_schema.tables  WHERE table_schema = '{table_schema}' AND table_type = 'BASE TABLE' AND table_name in ({string.Join(",", days)}) order by table_name; ";
            var command = profileContext.Database.GetDbConnection().CreateCommand();
            command.CommandText = tableNQeryStr;
            profileContext.Database.OpenConnection();
            var res = new List<string>();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    res.Add(reader[0] as string);
                }
            }
            return res;
        }
        protected abstract bool TableExisted(DbContext context, string tableName);


        protected abstract ParameterBuilder ParameterBuilder(IEnumerable<object[]> items);


        public IEnumerable<DONN.LS.Entities.TempLocations> GetItems(string uid, string type, int interval, DateTime sTime, DateTime eTime, int index, int count)
        {
            var names = GetExistedTableName(sTime, eTime);
            if (names.Count() > 0)
            {
                var parametersBuilder = ParameterBuilder(new List<object[]>{
                    new object[]{"@UniqueId", uid}
                    ,new object[]{"@Type",  type.ToLower()}
                    ,new object[]{"@eTime", eTime.ToUniversalTime()}
                    ,new object[]{"@sTime",  sTime.ToUniversalTime()}
                    });
                string queryStr = string.Join(" UNION ALL", names.Select(n => $"(Select * from {table_schema}.\"{n}\" WHERE \"UniqueId\"=@UniqueId AND \"Type\"=@Type AND \"SendTime\"<@eTime AND \"SendTime\">@sTime {(interval != 0 ? " AND \"CustomInterval\">=@interval" : "")})")) + $" ORDER BY \"SendTime\" LIMIT {count} OFFSET {count * (index - 1)}";
                if (interval != 0)
                {
                    parametersBuilder.Add("@interval", interval);
                }
                semaphore4Location.Wait();
                var res = locationContext.TempLocations.AsNoTracking().FromSql(queryStr, parametersBuilder.Build().ToArray());
                semaphore4Location.Release();
                return res;
            }
            return new List<TempLocations>();
        }

        public IEnumerable<DONN.LS.Entities.DeviceProfile> GetProfiles()
        {
            return profileContext.DeviceProfile.ToList();
        }
    }
}
