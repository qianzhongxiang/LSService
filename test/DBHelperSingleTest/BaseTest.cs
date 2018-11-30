using DONN.LS.DBHelperSingle;
using DONN.LS.Entities;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DBHelperSingleTest
{

    public class BaseTest
    {
        [Fact]
        public void SaveItemsChangeAsync()
        {
            var now = DateTime.Now;
            //Given
            var provider = Provider.Instance(DateTime.Now.Millisecond.ToString(), "Host=192.168.8.36;Port=27866;Database=locationTest;Username=postgres;Password=postgres", Providers.Pgsql);

            //When
            provider.UpdateItems(new List<TempLocations> { new TempLocations { Type = "si_type1", UniqueId = "si_uid1" ,CollectTime=now,SendTime=now}
            , new TempLocations { Type = "si_type2", UniqueId = "si_uid2" ,CollectTime=now,SendTime=now} });
            //Then

            var count = provider.SaveChangeAsync().Result;
            //locations 2 + device 2
            Assert.Equal(4, count);
            //验证旧数据是否被删除
            Assert.Equal(0, provider.GetVolume("l"));
        }
        [Fact]
        public async void DeviceConcurrency()
        {
            SaveItemsChangeAsync();
            var provider = Provider.Instance("DeviceConcurrency1", "Host=192.168.8.36;Port=27866;Database=locationTest;Username=postgres;Password=postgres", Providers.Pgsql);
            var provider2 = Provider.Instance("DeviceConcurrency2", "Host=192.168.8.36;Port=27866;Database=locationTest;Username=postgres;Password=postgres", Providers.Pgsql);
            var firstEntry = provider.GetProfiles().FirstOrDefault();
            firstEntry.Day = new Random().Next(9999); 
            firstEntry.Day = new Random().Next(9999);
            provider2.GetProfiles().FirstOrDefault().Day = 33;
            Assert.Equal(1, await provider.SaveChangeAsync());
            Assert.Equal(0, await provider2.SaveChangeAsync());
        }

        [Fact]
        public async void GetItems()
        {
            var beginTime=DateTime.Now;
            SaveItemsChangeAsync();
            var provider = Provider.Instance("GetItems1", "Host=192.168.8.36;Port=27866;Database=locationTest;Username=postgres;Password=postgres", Providers.Pgsql);
            var count = provider.GetItems("si_uid1", "si_type1", 0, beginTime, DateTime.Now.AddDays(1), 1, 200).Count();
            Assert.Equal(1, count);
            provider = Provider.Instance("GetItems1");
            count = provider.GetItems("si_uid1", "si_type1", 0, beginTime, DateTime.Now.AddDays(1), 1, 200).Count();
            Assert.Equal(1, count);
        }
    }
}
