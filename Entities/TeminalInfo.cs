using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DONN.LS.Entities
{
    /// <summary>
    /// For teminal db & memory
    /// </summary>
    public class TeminalInfo : RemoteDataBase
    {
        public string UniqueId { get; set; }
        public string Type { get; set; }
        public decimal? X { get; set; }
        public decimal? Y { get; set; }
        public decimal? Z { get; set; }

        public EPSG EPSG { get; set; }

        //public string RemoteHostName { get; set; }
        //public int? RemotePort { get; set; }
        //public string LocalHostName { get; set; }
        //public int? LocalPort { get; set; }

        //public TransportTypeEx TransportType { get; set; }
        public string ServiceName { get; set; }

        [NotMapped]
        public int? Interval { get; set; }
        public override string ToString()
        {
            return $"{UniqueId}_{Type}";
        }
    }
}
