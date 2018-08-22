using DONN.LS.DBHelper;
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
    }
}