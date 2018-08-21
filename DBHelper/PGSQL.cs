using Microsoft.EntityFrameworkCore;

namespace DONN.LS.DBHELPER
{
    class PGSQL : Base
    {
        public PGSQL(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new System.ArgumentException("message", nameof(connectionString));
            }

            locationOptionsBuilder = new DbContextOptionsBuilder<LocationContext>();
            locationOptionsBuilder.UseNpgsql(connectionString);
        }
    }
}