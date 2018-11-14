using DONN.Tools.Protocol.DataResolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DONN.LS.Terminal.Android
{
    public class InstructsHelper
    {
        public static byte[] GenerateConfirmPackage(byte validateCode, byte mainCode, byte subCode)
        {
            return GeneratePackage(new byte[] { validateCode, mainCode, subCode }, 0x21);
        }
        public static byte[] GeneratePackage(byte[] content, byte mainCode)
        {
            int l = content == null ? 0 : content.Length;
            byte[] s = new byte[l + 7];
            s[0] = s[1] = 0x29;
            s[2] = mainCode;
            s[3] = Convert.ToByte((l + 2) / 256);
            s[4] = Convert.ToByte((l + 2) % 256);
            s[l + 6] = 0x0D;
            int i = l;
            while (--i >= 0) s[i + 5] = content[i]; //content
            s[l + 5] = EncodeingHelper.BCCValidateCode(s, 0, l + 5);//校验
            return s;
        }
    }
}
