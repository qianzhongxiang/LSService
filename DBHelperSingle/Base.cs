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

namespace DONN.LS.DBHelperSingle
{

    public abstract class Base
    {
        protected readonly string table_schema = "public";

        public LocationContext LocationContext = null;
        protected DbContextOptionsBuilder<LocationContext> locationOptionsBuilder;
        public Base(string connectionString)
        {
            InitDbContextOptionsBuilder(connectionString);
            LocationContext = new LocationContext(locationOptionsBuilder.Options);
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
                    return LocationContext.TempLocations.Local.Count;
                case "p":
                    return LocationContext.DeviceProfile.Local.Count;
                default:
                    return 0;
            }
        }
        protected abstract void InitDbContextOptionsBuilder(string connectionString);

        private SemaphoreSlim semaphore4Location = new SemaphoreSlim(1, 1);
        /// <summary>
        /// 需要改进性能
        /// </summary>
        /// <returns></returns>
        public virtual async Task<int> SaveChangeAsync()
        {
            return await Task.Run(async () =>
            {
                var count = await LocationContext.SaveChangesAsync();
                DetachLocations();
                return count;
            });

        }
        private void DetachLocations()
        {
            var list = LocationContext.TempLocations.Local.ToList();
            list.ForEach(item =>
            {
                var entry = LocationContext.Entry(item);
                if (entry.State == Microsoft.EntityFrameworkCore.EntityState.Unchanged)
                    entry.State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            });
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
                item.SendTime = item.SendTime?.ToUniversalTime() ?? item.CollectTime;
                var device = LocationContext.DeviceProfile.FirstOrDefault(d => d.Uid == item.UniqueId);
                if (device == null)
                {
                    LocationContext.DeviceProfile.Add(new DeviceProfile { Uid = item.UniqueId, Type = item.Type, IdLoactionData = item.Id, UpdateTime = item.SendTime.Value });
                }
                else
                {
                    device.IdLoactionData = item.Id;
                    device.UpdateTime = item.SendTime.Value;
                    LocationContext.DeviceProfile.Update(device);
                }
            }
            LocationContext.TempLocations.AddRangeAsync(items);
        }
        public virtual void UpdateProfile(IEnumerable<DeviceProfile> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            foreach (var item in items)
            {
                var previous = LocationContext.DeviceProfile.FirstOrDefault(i => i.Uid == item.Uid && i.Type == item.Type);
                item.UpdateTime = item.UpdateTime.ToUniversalTime();
                if (previous == null)
                    LocationContext.DeviceProfile.Add(item);
                else if (item != previous)
                {
                    LocationContext.Entry(previous).State = EntityState.Detached;
                    LocationContext.DeviceProfile.Update(item);
                }
            }
        }

        protected abstract ParameterBuilder ParameterBuilder(IEnumerable<object[]> items);


        public IEnumerable<DONN.LS.Entities.TempLocations> GetItems(string uid, string type, int interval, DateTime sTime, DateTime eTime, int index, int count)
        {
            type = type.ToLower();
            eTime = eTime.ToUniversalTime();
            sTime = sTime.ToUniversalTime();

            var query = LocationContext.TempLocations.AsNoTracking().Where(
                 t => t.UniqueId == uid && t.Type == type && t.SendTime < eTime
                 && t.SendTime > sTime
             );
            if (interval != 0)
            {
                query = query.Where(t => t.CustomInterval > interval);
            }
            return query.Skip((index - 1) * count).Take(count);
        }

        public IEnumerable<DONN.LS.Entities.DeviceProfile> GetProfiles()
        {
            return LocationContext.DeviceProfile.ToList();
        }
    }
}
