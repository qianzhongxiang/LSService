using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using DONN.LS.ENTITIES;
using Microsoft.EntityFrameworkCore;
using DONN.Tools.Logger;

namespace DONN.LS.DBHELPER
{
    public abstract class Base
    {
        protected readonly string prefix = "TL_";
        private int switcher = 0;
        private List<TempLocations>[] data = new List<TempLocations>[] { new List<TempLocations>(), new List<TempLocations>() };
        private LocationContext locationContext;
        private ProfileContext profileContext;
        protected DbContextOptionsBuilder<LocationContext> locationOptionsBuilder;
        protected DbContextOptionsBuilder<ProfileContext> profileOptionsBuilder;
        public string TableName { get => prefix + TableNo; }
        public Base()
        {
            //初始化 context
            profileContext = CreateProfileContext();
        }
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
        public virtual void UpdateItems(IEnumerable<TempLocations> items) => this.data[switcher].AddRange(items);
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
            profileContext.DeviceProfile.UpdateRange(items);
        }
        private int _tableNo = 0;
        public int TableNo { get => _tableNo; private set => _tableNo = value; }

        public IEnumerable<DONN.LS.ENTITIES.TempLocations> GetItems()
        {
            return null;
        }

        public IEnumerable<DONN.LS.ENTITIES.TeminalInfo> GetProfiles()
        {
            return null;
        }
    }
}
