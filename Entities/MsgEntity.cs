using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DONN.LS.Entities
{
    public class MsgEntity
    {
        public MsgType MsgType { get; set; }

        public string SubType { get; set; }

        public string Uid { get; set; }

        public string DevType { get; set; }

        public string Msg { get; set; }

        public DateTime EventTime { get; set; }
    }
}
