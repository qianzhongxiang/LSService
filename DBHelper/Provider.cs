using System.Collections.Generic;
using System.Linq;

namespace DONN.LS.DBHELPER
{

    public abstract class Provider
    {
        private static Dictionary<string, Base> providers { get; set; }
        /// <summary>
        /// 获取Provider Instance
        /// </summary>
        /// <param name="key"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static Base Instance(string key, string connectionString)
        {
            if (providers.ContainsKey(key)) return providers[key];
            else
            {
                //extract type from connectionString 
                var provider = "";

                //
                switch (provider)
                {
                    case "pgsql":
                        return PGSQL.Instance;
                    default:
                        return null;
                }
                return providers[key] = new PGSQL();
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