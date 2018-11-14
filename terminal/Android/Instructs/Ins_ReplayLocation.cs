using DONN.LS.Terminal.Android.Instructs;
using System;
using DONN.LS.IDataAdapter;

namespace DONN.LS.Terminal.Android.Instructs
{
    public class Ins_ReplayLocation : Ins_CommonLocation, IInstructs
    {
        public Ins_ReplayLocation()
        {
            InsCode = 0x81;
        }
    }
}
