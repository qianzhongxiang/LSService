using DONN.LS.DBHelperSingle;
using DONN.LS.Entities;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
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
            var provider = Provider.Instance("SaveItemsChangeAsync1", "Host=192.168.8.36;Port=27866;Database=locationTest;Username=postgres;Password=postgres", Providers.Pgsql);

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
    }
}
