using Keller.SPM.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Keller.SPM.Communication.MUT.SerialCOM;

namespace Keller.SPM.ProcotolGeneration.Protocol
{
    public class SecurityKeys
    {
        private static StatusKeyClass statusClass = new StatusKeyClass();
        public byte[] PasswordKey { get; set; } = Encoding.UTF8.GetBytes(statusClass.GlobalKey);
        public byte[] CNonce { get; set; } = Encoding.UTF8.GetBytes(statusClass.SystemTitle);
        public byte[] SecretKey { get; set; } = Encoding.UTF8.GetBytes(statusClass.HLS);

        //public byte[] PasswordKey { get; set; } = Encoding.UTF8.GetBytes("AEI_Globalkey_EA");
        //public byte[] CNonce { get; set; } = Encoding.UTF8.GetBytes("AEI00001");
        //public byte[] SecretKey { get; set; } = Encoding.UTF8.GetBytes("AEI_SECRET_ASSC2");
        public byte[] HDLCProtocolSAE { get; set; } = { 0x7E };
        public byte[] HDLCFrame_Format { get; set; } = { 0xA0 };

        public byte[] DRPlainText = new byte[13];
        public byte[] DRInvocationCounter = new byte[4];
        public byte[] CRPlainText = new byte[13];
        public byte[] CRInvocationCounter = new byte[4];
    }
}
