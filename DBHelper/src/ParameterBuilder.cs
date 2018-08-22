using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace DONN.LS.DBHelper
{
    public abstract class ParameterBuilder
    {
        protected List<object[]> store = new List<object[]>();
        public void Add(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new System.ArgumentException("key is required", "key is empty");
            }

            if (value == null)
            {
                throw new System.ArgumentNullException("value is null");
            }

            if (!key.StartsWith("@"))
            {
                throw new System.ArgumentException("key must be one with @ prefix", "key lost @ prefix");
            }

            store.Add(new object[] { key, value });
        }
        public abstract IEnumerable<DbParameter> Build();
    }
    public class ParameterBuilder<T> : ParameterBuilder where T : DbParameter, new()
    {
        public override IEnumerable<DbParameter> Build()
        {
            return store.Select(i =>
             {
                 var a = new T() as DbParameter;
                 a.ParameterName = i[0] as string; a.Value = i[1]; return a;
             });
        }
    }

}