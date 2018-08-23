using System;
using System.Collections.Generic;
using DONN.LS.DBHelper;
using DONN.LS.ENTITIES;
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
            //Given
            var provider = Provider.Instance("SaveItemsChangeAsync1", "server=127.0.0.1;Port=5432;user id=postgres;password=postgis;database=location", Providers.Pgsql);

            //When
            provider.UpdateItems(new List<TempLocations> { new TempLocations { Type = "si_type1", UniqueId = "si_uid1" }
            , new TempLocations { Type = "si_type2", UniqueId = "si_uid2" } });
            //Then

            var count = provider.SaveItemsChangeAsync().Result;
            Assert.Equal(int.Parse(DateTime.Now.ToString("yyyyMMdd")), provider.TableNo);
            Assert.Equal("TL_" + provider.TableNo, provider.TableName);
            Assert.Equal(2, count);

            provider.UpdateItems(new List<TempLocations> { new TempLocations { Type = "si_type3", UniqueId = "si_uid3" } });
            count = provider.SaveItemsChangeAsync().Result;
            Assert.Equal(1, provider.GetVolume("l"));
        }
    }
}