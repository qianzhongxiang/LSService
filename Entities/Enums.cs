using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DONN.LS.Entities
{
    /// <summary>
    /// Do not use 3 that is for TransportType.All
    /// </summary>
    public enum TransportTypeEx
    {
        None = 0,
        Udp = 1,
        Tcp = 2,
        Command = 4,
        Mqtt = 11
    }

    public enum EPSG
    {
        EPSG4326 = 0,
        EPSG3857 = 1
    }

    public enum DeviceStatus
    {
        Online = 1,
        Offline = 2
    }

    public enum MsgType
    {
        Warnning = 1,
        Info = 2,
        Error = 3
    }
}
