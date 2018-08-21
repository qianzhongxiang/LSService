
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DONN.LS.Entities
{
    public interface IRemoteDataBase
    {
        string RemoteHostName { get; set; }

        int Port { get; set; }

        int LocalPort { get; set; }

        TransportTypeEx TType { get; set; }
    }
    //[DataContract]
    public class RemoteDataBase : IRemoteDataBase
    {
        //[DataMember]
        [Newtonsoft.Json.JsonIgnore]
        public string RemoteHostName { get; set; }

        //[DataMember]
        [Newtonsoft.Json.JsonIgnore]
        public int Port { get; set; }

        [NotMapped]
        [Newtonsoft.Json.JsonIgnore]
        public int LocalPort { get; set; }

        [NotMapped]
        //[DataMember]
        [Newtonsoft.Json.JsonIgnore]
        public TransportTypeEx TType { get; set; }
    }

    //[DataContract]
    public class BusinessEntity : RemoteDataBase
    {
        //[DataMember]
        public byte[] CenterConfirmPackage { get; set; }

        //[DataMember]
        public byte[] ExceptionsPackage { get; set; }

        //[DataMember]
        public string UniqueID { get; set; }

        public string Type { get; set; }

        public override string ToString()
        {
            return $"{this.UniqueID}_{this.Type}";
        }
    }
}
