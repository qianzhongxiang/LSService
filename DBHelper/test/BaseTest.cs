using System;
using System.Collections.Generic;
using System.Linq;
using DONN.LS.DBHelper;
using DONN.LS.Entities;
using Xunit;

namespace test
{
    public class BaseTest
    {
        [Fact]
        public void UpdateItems_PassAvailableItemsShouldBeSuccessful()
        {
            //Given
            var provider = Provider.Instance("UpdateItems1", "server=127.0.0.1;Port=5432;user id=postgres;password=postgis;database=location", Providers.Pgsql);
            //When

            //Then
            Assert.Throws<System.ArgumentNullException>("items", () => provider.UpdateItems(null));
        }

        [Fact]
        public void UpdateProfile_PassAvailableItemsShouldBeSuccessful()
        {
            //Given
            var provider = Provider.Instance("UpdateProvider1", "server=127.0.0.1;Port=5432;user id=postgres;password=postgis;database=location", Providers.Pgsql);

            //When

            //Then
            Assert.Throws<System.ArgumentNullException>("items", () => provider.UpdateProfile(null));
        }

        [Fact]
        public void SaveItemsChangeAsync()
        {
            var now = DateTime.Now;
            //Given
            var provider = Provider.Instance("SaveItemsChangeAsync1", "server=127.0.0.1;Port=5432;user id=postgres;password=postgis;database=location", Providers.Pgsql);

            //When
            provider.UpdateItems(new List<TempLocations> { new TempLocations { Type = "si_type1", UniqueId = "si_uid1" ,CollectTime=now,SendTime=now}
            , new TempLocations { Type = "si_type2", UniqueId = "si_uid2" ,CollectTime=now,SendTime=now} });
            //Then

            var count = provider.SaveItemsChangeAsync().Result;
            Assert.Equal(int.Parse(DateTime.Now.ToString("yyyyMMdd")), provider.TableNo);
            Assert.Equal("TL_" + provider.TableNo, provider.TableName);
            Assert.Equal(2, count);
            //验证旧数据是否被删除
            provider.UpdateItems(new List<TempLocations> { new TempLocations { Type = "si_type3", UniqueId = "si_uid3", CollectTime = now, SendTime = now } });
            count = provider.SaveItemsChangeAsync().Result;
            Assert.Equal(1, provider.GetVolume("l"));
        }
        [Fact]
        public void SaveProfilesChangeAsync()
        {
            var now = DateTime.Now;
            //Given
            var list = new List<DeviceProfile> { new DeviceProfile { Uid = "spcid1", Type = "spctype1" ,UpdateTime= now } };
            var i = list.Count;
            var provider = Provider.Instance("SaveProfilesChangeAsync1", "server=127.0.0.1;Port=5432;user id=postgres;password=postgis;database=location", Providers.Pgsql);
            //When
            provider.UpdateProfile(list);
            var count = provider.SaveProfilesChangeAsync().Result;

            //Then
            Assert.Equal(i, count);
            Assert.Throws<System.ArgumentNullException>("items", () => provider.UpdateProfile(null));


            list = new List<DeviceProfile> { new DeviceProfile { Uid = "spcid1", Type = "spctype1", Interval = 5, UpdateTime = now }, new DeviceProfile { Uid = "spcid2", Type = "spctype2", Interval = 5, UpdateTime = now } };
            i = list.Count;
            provider.UpdateProfile(list);
            count = provider.SaveProfilesChangeAsync().Result;
            Assert.Equal(i, count);
        }
        [Fact]
        public void GetProfiles() {
            var now = DateTime.Now;
            //Given
            var list = new List<DeviceProfile> { new DeviceProfile { Uid = "gpid1", Type = "gptype1", UpdateTime = now } };
            var i = list.Count;
            var provider = Provider.Instance("GetProfiles1", "server=127.0.0.1;Port=5432;user id=postgres;password=postgis;database=location", Providers.Pgsql);
            //When
            provider.UpdateProfile(list);
            var count = provider.SaveProfilesChangeAsync().Result;
            Assert.True(provider.GetProfiles().Count()>0);
        }
        [Fact]
        public void GetItems()
        {
            //Given
            var provider = Provider.Instance("GetItems1", "server=127.0.0.1;Port=5432;user id=postgres;password=postgis;database=location", Providers.Pgsql);

            //When
            var now = DateTime.Now;
            provider.UpdateItems(new List<TempLocations> { new TempLocations { Type = "gi_type1", UniqueId = "gi_uid1" ,CollectTime=now,SendTime=now}
            , new TempLocations { Type = "gi_type2", UniqueId = "gi_uid2",CollectTime=now,SendTime=now } });
            //Then

            var count = provider.SaveItemsChangeAsync().Result;

           Assert.Single(provider.GetItems("gi_uid1", "gi_type1", 0, now.AddSeconds(-1), now.AddSeconds(1), 1, 10).ToList());
        }
        [Fact]
        public void GetExistedTableName() {
            var provider = Provider.Instance("GetExistedTableName1", "server=127.0.0.1;Port=5432;user id=postgres;password=postgis;database=location", Providers.Pgsql);

            //When
            var now = DateTime.Now;
            provider.UpdateItems(new List<TempLocations> { new TempLocations { Type = "GetExistedTableName_type1", UniqueId = "GetExistedTableName_uid1" ,CollectTime=now,SendTime=now}
            , new TempLocations { Type = "GetExistedTableName_type2", UniqueId = "GetExistedTableName_uid2",CollectTime=now,SendTime=now } });

            var count = provider.SaveItemsChangeAsync().Result;
            //Then
            Assert.Single(provider.GetExistedTableName(now, now));
            //Assert.True(provider.GetExistedTableName(now.AddDays(-1), now).Count()==2);
        }
    }
}