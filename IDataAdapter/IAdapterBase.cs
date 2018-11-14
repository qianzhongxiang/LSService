using DONN.LS.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace DONN.LS.IDataAdapter
{
    public enum RelevantType {
        None = 0,
        Location =1
    }
    public struct LinkParmeters {
        public string RemoteHostName { get; set; }

        public int RemotePort { get; set; }

        public TransportTypeEx TransportType { get; set; }
    }
    public interface IAdapter
    {
        string EncodingStr { get; set; }
        IConverter Converter { get; set; }
        object[] Deserialize(Stream s, LinkParmeters linkParmeters,ref Dictionary<RelevantType, List<object>> Relevants);
        byte[] CallbackProcess(string type, byte[] content);
    }
}
