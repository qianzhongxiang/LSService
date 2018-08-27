using System;
using DONN.LS.DBHelper;
using Xunit;

namespace test
{
    public class ProviderTest
    {
        [Fact]
        public void Instance_shouldThrowException()
        {
            //Given
            //When
            //Then
            Assert.Throws<ArgumentException>("connectionString", () => Provider.Instance("Instance_shouldThrowException1", "", Providers.Pgsql));
            Assert.Throws<ArgumentException>("key", () => Provider.Instance("", "server=127.0.0.1;Port=5432;user id=postgres;password=postgis;database=location", Providers.Pgsql));
            // Provider.Instance("key1", "server=127.0.0.1;Port=5432;user id=postgres;password=postgis;database=location", Providers.Pgsql);
            // Assert.Throws<ArgumentException>("key", () =>
            // {
            //     Provider.Instance("key1", "server=127.0.0.1;Port=5432;user id=postgres;password=postgis;database=location", Providers.Pgsql);
            // });
            Assert.Throws<ArgumentException>("provider", () => Provider.Instance("Instance_shouldThrowException2", "server=127.0.0.1;Port=5432;user id=postgres;password=postgis;database=location", Providers.Mysql));

        }
        [Fact]
        public void Instance_shouldReturnRightProvider()
        {
            //Given
            var provider = Provider.Instance("Instance_shouldReturnRightProvider1", "server=127.0.0.1;Port=5432;user id=postgres;password=postgis;database=location", Providers.Pgsql);
            var provider1 = Provider.Instance("Instance_shouldReturnRightProvider2", "server=127.0.0.1;Port=5432;user id=postgres;password=postgis;database=location", Providers.Pgsql);
            //When

            //Then

            Assert.NotNull(provider);
            Assert.NotNull(provider1);
            Assert.Equal(provider, Provider.Instance("Instance_shouldReturnRightProvider1"));
            Assert.Equal(provider1, Provider.Instance("Instance_shouldReturnRightProvider2"));
            // Assert.Equal(provider1, Provider.Instance());
        }
    }
}