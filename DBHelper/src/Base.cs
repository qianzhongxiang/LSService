using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using DONN.LS.ENTITIES;
using Microsoft.EntityFrameworkCore;
using DONN.Tools.Logger;
using System.Data.Common;
using System.Data;

namespace DONN.LS.DBHelper
{
    public abstract class Base
    {
        protected readonly string prefix = "TL_";
        private readonly string table_schema = "public";

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
        }

        protected abstract void InitDbContextOptionsBuilder(string connectionString);
        protected virtual LocationContext CreateLoactionContext()
        {
            var context = new LocationContext(locationOptionsBuilder.Options, TableName);
            context.ChangeTracker.AutoDetectChangesEnabled = false;
            return context;
        }
        protected virtual ProfileContext CreateProfileContext()
        {
            var context = new ProfileContext(profileOptionsBuilder.Options);
            return context;
        }
        public virtual void UpdateItems(IEnumerable<TempLocations> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            data[switcher].AddRange(items);
        }

        private SemaphoreSlim semaphore4Location = new SemaphoreSlim(1, 1);
        /// <summary>
        /// 需要改进性能
        /// </summary>
        /// <returns></returns>
        public virtual async void SaveItemsChangeAsync()
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
                    CreateLoactionContext();
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
                locationContext.SaveChanges();
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
        }
        public virtual async void SaveProfilesChangeAsync()
        {
            try
            {
                await profileContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                LogHelper.Error("SaveProfilesChangeAsync:failed", e);
            }
            finally
            {

            }
        }
        public virtual void UpdateProfile(IEnumerable<DeviceProfile> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            profileContext.DeviceProfile.UpdateRange(items);
        }
        private int _tableNo = 0;
        public int TableNo { get => _tableNo; private set => _tableNo = value; }
        private IEnumerable<string> GetExistedTableName(DateTime sTime, DateTime eTime)
        {
            var days = new List<string>();
            int s = int.Parse(sTime.ToString("yyyyMMdd")), e = int.Parse(eTime.ToString("yyyyMMdd"));
            for (int i = s; i <= e; i++)
            {
                days.Add($"'{prefix}{i}'");
            }
            var tableNQeryStr = $"SELECT table_name FROM information_schema.tables  WHERE table_schema = '{table_schema}' AND table_type = 'BASE TABLE' AND table_name in ({string.Join(",", days)}) order by table_name; ";
            return profileContext.Query<string>().FromSql(tableNQeryStr);
        }
        protected abstract ParameterBuilder ParameterBuilder(IEnumerable<object[]> items);


        public IEnumerable<DONN.LS.ENTITIES.TempLocations> GetItems(string uid, string type, int interval, DateTime sTime, DateTime eTime, int index, int count)
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
                string queryStr = string.Join(" UNION ALL", names.Select(n => $"(Select * from {table_schema}.\"{n}\" WHERE uniqueid=@UniqueId AND type=@Type AND sendtime<@eTime AND sendtime>@sTime {(interval != 0 ? " AND custominterval>=@interval" : "")})")) + $" ORDER BY sendtime LIMIT {count} OFFSET {count * (index - 1)}";
                if (interval != 0)
                {
                    parametersBuilder.Add("@interval", interval);
                }
                return profileContext.Query<TempLocations>().FromSql(queryStr, parametersBuilder.Build());
            }
            return new List<TempLocations>();
        }

        public IEnumerable<DONN.LS.ENTITIES.DeviceProfile> GetProfiles()
        {
            return profileContext.DeviceProfile.ToList();
        }
    }
}
