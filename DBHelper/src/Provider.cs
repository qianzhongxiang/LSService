using System.Collections.Generic;
using System.Linq;

namespace DONN.LS.DBHelper
{
    public enum Providers
    {
        Pgsql,
        Mysql,
        SQLServer
    }
    public abstract class Provider
    {
        private static Dictionary<string, Base> providers = new Dictionary<string, Base>();
        /// <summary>
        /// 获取Provider Instance
        /// </summary>
        /// <param name="key"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static Base Instance(string key, string connectionString, Providers provider)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new System.ArgumentException("is required", nameof(key));
            }
            if (providers.ContainsKey(key))
            {
                throw new System.ArgumentException("was existed", nameof(key));
            }

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new System.ArgumentException("is required", nameof(connectionString));
            }
            if (providers.ContainsKey(key)) return providers[key];
            else
            {
                Base instance;
                //distribute 
                switch (provider)
                {
                    case Providers.Pgsql:
                        instance = new PGSQL(connectionString);
                        break;
                    default:
                        throw new System.ArgumentException("support pgsql only, now", "provider");
                }
                if (providers == null) System.Diagnostics.Debug.WriteLine("providers is null");
                providers.Add(key, instance);
                return instance;
            }


        }
        /// <summary>
        /// Get corresponsible provider by key, by default, get the last one
        /// </summary>
        /// <param name="key">optional</param>
        /// <returns></returns>
        public static Base Instance(string key = "")
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return providers.Values.Last();
            }
            else
            {
                return providers[key];
            }

        }
    }
}