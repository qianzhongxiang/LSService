using DONN.LS.IDataAdapter;
using DONN.Tools.Protocol.DataResolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DONN.LS.Terminal.Android.Instructs
{
    public interface IInstructs
    {
        ushort MinLength { get; }
        byte InsCode { get; }
        RelevantType RelevantType { get; }
        object FromIns(byte[] bytes);
        byte[] ToIns(object obj);
    }
    public class Ins_Base : IInstructs
    {
        public Ins_Base(ushort minLength, byte insCode, RelevantType relevantType)
        {
            MinLength = minLength;
            InsCode = insCode;
            RelevantType = relevantType;
        }
        public ushort MinLength { get; protected set; }
        public byte InsCode { get; protected set; }

        public RelevantType RelevantType { get; protected set; }

        public virtual object FromIns(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public virtual byte[] ToIns(object obj)
        {
            throw new NotImplementedException();
        }

        protected string UniqueID(byte[] bytes)
        {
            return EncodeingHelper.CompressBCD(bytes).ToString("00000000");
        }
    }
}
