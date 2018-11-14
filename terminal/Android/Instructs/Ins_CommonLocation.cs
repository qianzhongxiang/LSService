using DONN.LS.Terminal.Android.Entity;
using System;
using DONN.LS.IDataAdapter;
using DONN.Tools.Protocol.DataResolver;

namespace DONN.LS.Terminal.Android.Instructs
{
    public class Ins_CommonLocation : Ins_Base
    {
        public Ins_CommonLocation() : base(0x14, 0x80, RelevantType.Location) { }

        public override object FromIns(byte[] bytes)
        {
            if (bytes.Length < MinLength - 2)
            {

                return null;
            }

            Ins_CommonLocationEntity s = new Ins_CommonLocationEntity();
            s.UniqueID = UniqueID(EncodeingHelper.CutOff(bytes, 0, 4));
            s.Date = DateTime.Now;
            try
            {
                s.Latitude = LatLongitude(EncodeingHelper.CutOff(bytes, 10, 4));
                s.Longitude = LatLongitude(EncodeingHelper.CutOff(bytes, 14, 4));
            }
            catch (Exception)
            {
                s.ExceptionsPackage = InstructsHelper.GeneratePackage(new byte[] { 0x10, InsCode }, 0xFF);
            }
            //s.InqueryPackage= InstructsHelper.GeneratePackage(null, 0x30);
            return s;
        }

        private double LatLongitude(byte[] bytes)
        {
            if (bytes.Length < 4) return double.MinValue;
            var degree = bytes[0] / 16 * 100 + bytes[0] % 16 * 10 + bytes[1] / 16;
            double minutes = bytes[1] % 16 * 10 + bytes[2] / 16;
            var seconds = bytes[2] % 16 * 100 + bytes[3] / 16 * 10 + bytes[3] % 16;
            return degree + (minutes * 1000 + seconds) / 60000.00000;
        }


        public override byte[] ToIns(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
