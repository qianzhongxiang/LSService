using System;
using System.Linq;
using DONN.LS.DBHelper;
using Xunit;

namespace test
{
    public class ParameterBuilderTest
    {
        [Fact]
        public void Add_AddAvailableValueShouldBeSuccessful()
        {
            var builder = new ParameterBuilder<Npgsql.NpgsqlParameter>();
            builder.Add("@value", "value");
            Assert.True(builder.Build().Count() == 1);
            Assert.Equal("@value", builder.Build().First().ParameterName);
            Assert.Equal("value", builder.Build().First().Value);
        }
        [Fact]
        public void Add_AddInvailableKeyShouldBeFail()
        {
            //Given
            var builder = new ParameterBuilder<Npgsql.NpgsqlParameter>();
            //When

            //Then
            Assert.Throws<System.ArgumentException>("key is empty", () => builder.Add("", "value"));
            Assert.Throws<System.ArgumentException>("key lost @ prefix", () => builder.Add("key", "value"));
            Assert.Throws<System.ArgumentNullException>("value is null", () => builder.Add("@key", null));
        }

    }
}
