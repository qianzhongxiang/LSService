using DONN.LS.Entities;
using DONN.LS.IDataAdapter;
using DONN.LS.Terminal.Android.Entity;
using DONN.LS.Terminal.Android.Instructs;
using DONN.Tools.Logger;
using DONN.Tools.Protocol.DataResolver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DONN.LS.Terminal.Android
{
    public class Main : IAdapter
    {
        private Dictionary<byte, IInstructs> _ips;
        public Main()
        {
            Converter = new Converter();
            var ins = new List<IInstructs> { new Ins_CommonLocation(), new Ins_ReplayLocation() };
            _ips = ins.ToDictionary(i => i.InsCode);
        }
        public string EncodingStr { get; set; }
        public IConverter Converter { get; set; }

        public object[] Deserialize(Stream s, LinkParmeters linkParmeters, ref Dictionary<RelevantType, List<object>> Relevants)
        {
            List<object> list = new List<object>();
            try
            {
                s.Seek(0, SeekOrigin.Begin);
                long l = s.Length;
                ushort scount = 0;
                while (s.Position < l)
                {
                    if (s.ReadByte() == 0x29) scount++;
                    else scount = 0;
                    if (scount == 2)
                    {
                        byte ins_code = Convert.ToByte(s.ReadByte());
                        if (!_ips.ContainsKey(ins_code))
                        {
                            list.Add(new Ins_BaseEntity { ExceptionsPackage = InstructsHelper.GeneratePackage(new byte[] { 0x0A, ins_code }, 0xFF) });
                            scount = 0;
                            continue;
                        }
                        //if (ins_code == 0x81)
                        //{
                        //    LogHelper.log("0x81");
                        //}
                        //if (ins_code == 0x80)
                        //{
                        //    LogHelper.log("0x80");
                        //}
                        IInstructs c = _ips[ins_code];
                        byte[] bs = new byte[2];
                        s.Read(bs, 0, 2);
                        int il = bs[0] * 256 + bs[1];
                        byte[] sTv = new byte[il + 3];
                        sTv[0] = 0x29;
                        sTv[1] = 0x29;
                        sTv[2] = ins_code;
                        sTv[3] = bs[0];
                        sTv[4] = bs[1];
                        bs = new byte[il - 2];
                        s.Read(bs, 0, il - 2);
                        Array.Copy(bs, 0, sTv, 5, il - 2);
                        int[] last = new int[] { s.ReadByte(), s.ReadByte() };
                        #region 校验
                        if (last[1] != 0x0D)
                        {
                            //包尾 throw exception
                            list.Add(new Ins_BaseEntity { ExceptionsPackage = InstructsHelper.GeneratePackage(new byte[] { 0x05, ins_code }, 0xFF) });
                            scount = 0;
                            continue;
                        }
                        if (last[0] != EncodeingHelper.BCCValidateCode(sTv, 0, il + 3))
                        {
                            //校验 
                            list.Add(new Ins_BaseEntity { ExceptionsPackage = InstructsHelper.GeneratePackage(new byte[] { 0x01, ins_code }, 0xFF) });
                            scount = 0;
                            continue;
                        }
                        #endregion

                        object o = c.FromIns(bs);
                        list.Add(o);
                        if (!Relevants.ContainsKey(c.RelevantType)) Relevants[c.RelevantType] = new List<object>();
                        Relevants[c.RelevantType].Add(o);
                        scount = 0;
                    }
                }
                if (list.Count == 0 & l < 7) list.Add(new Ins_BaseEntity { ExceptionsPackage = InstructsHelper.GeneratePackage(new byte[] { 0x15, 0x00 }, 0xFF) });

            }
            catch (Exception e)
            {
                LogHelper.Error(e.ToString());
            }
            return list.ToArray();
        }

        public byte[] CallbackProcess(string type, byte[] content)
        {
            switch (type)
            {
                case "InqueryLocation":
                    return InstructsHelper.GeneratePackage(null, 0x30);
                default:
                    return null;
            }
        }
    }

    public class Converter : IConverter
    {
        public TempLocations ToTempLoc(object original)
        {
            Ins_CommonLocationEntity obj = original as Ins_CommonLocationEntity;
            return new TempLocations { CollectTime = obj.Date, SendTime = obj.Date, X = (decimal)obj.Longitude, Y = (decimal)obj.Latitude, UniqueId = obj.UniqueID, Type = "cellphone" };
        }
    }
}
