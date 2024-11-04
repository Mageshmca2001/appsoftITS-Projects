using System.Text;
using static Keller.SPM.Communication.MUT.SerialCOM;

namespace Keller.SPM.ProcotolGeneration.Protocol.Serial
{
    public class SecurityKeys
    {

        private static StatusKeyClass1 statusClass = new StatusKeyClass1();
        public byte[] PasswordKey { get; set; } = Encoding.UTF8.GetBytes(statusClass.GlobalKey);
        public byte[] CNonce { get; set; } = Encoding.UTF8.GetBytes(statusClass.SystemTitle);
        public byte[] SecretKey { get; set; } = Encoding.UTF8.GetBytes(statusClass.HLS);


        //public byte[] PasswordKey { get; set; } = Encoding.UTF8.GetBytes("AEI_Globalkey_EA");  // new version - AEI_Globalkey_EA, old version - AVG_Globalkey_EA
        //public byte[] CNonce { get; set; } = Encoding.UTF8.GetBytes("AEI00001");  // old version 
        public byte[] SNonce { get; set; }  // 08.08.2023
        public byte[] DedicatedKey { get; set; } // 08.08.2023
       // public byte[] SecretKey { get; set; } = Encoding.UTF8.GetBytes("AEI_SECRET_ASSC2");
        public byte[] HDLCProtocolSAE { get; set; } = { 0x7E };
        public byte[] HDLCFrame_Format { get; set; } = { 0xA0 };

        public byte[] DRPlainText = new byte[13];
        public byte[] DRInvocationCounter = new byte[4];
        public byte[] CRPlainText = new byte[13];
        public byte[] CRInvocationCounter = new byte[4];
    }
}
