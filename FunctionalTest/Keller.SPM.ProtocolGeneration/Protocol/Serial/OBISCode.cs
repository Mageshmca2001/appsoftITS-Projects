using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keller.SPM.ProtocolGeneration.Protocol.Serial
{
    public class OBISCode
    {
        public byte[] SerialNumber { get; set; } = { 0x01, 0x00, 0x60, 0x02, 0x84, 0x00 };   // old version- 1.0.96.2.131.0, New version - 1.0.96.2.132.0
        public byte[] RTC { get; set; } = { 0x00, 0x00, 0x01, 0x00, 0x00, 0xFF };           // 0.0.1.0.0.255
        public byte[] RAMClear { get; set; } = { 0x01, 0x00, 0x60, 0x02, 0x80, 0x00 };       // 1.0.96.2.128.0
        public byte[] IPReference { get; set; } = { 0x00, 0x00, 0x19, 0x00, 0x00, 0xFF };     // 0.0.25.0.0.255
        public byte[] APN { get; set; } = { 0x00, 0x00, 0x19, 0x04, 0x00, 0xFF };         // 0.0.25.4.0.255       
        public byte[] PushIP { get; set; } = { 0x00, 0x00, 0x19, 0x09, 0x00, 0xFF };      // 0.0.25.9.0.255
        public byte[] StartTest { get; set; } = { 0x01, 0x00, 0x60, 0x02, 0x80, 0x00 };       // 1.0.96.2.128.0
        public byte[] ReadTest { get; set; } = { 0x01, 0x00, 0x60, 0x02, 0x86, 0x00 };      // 1.0.96.2.134.0
        public byte[] InvocationOBIS { get; set; } = { 0x00, 0x00, 0x2B, 0x01, 0x03, 0xFF };  // 0.0.43.1.3.255

    }
}
