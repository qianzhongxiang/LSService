using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DONN.LS.Entities
{
    public enum CommandCode
    {
        None = 0,
        IntervalSetting = 0x11,
        QueryTeminals = 0x22,
        CodeExisted = 0x77,
        Finish = 0x88,
        WholeFinish = 0x99
    }
    public class CommandEntity
    {
        public CommandCode Code { get; set; }
        public string Value { get; set; }
        public TeminalInfo Teminal { get; set; }
        public string ServiceName { get; set; }
        public Guid Id { get; set; }
        public string SessionId { get; set; }
    }
    public class ReturnEntity
    {
        public Guid Id { get; set; }
        public CommandCode Code { get; set; }
        public CommandCode OriginCode { get; set; }
        public string Msg { get; set; }
    }
}
