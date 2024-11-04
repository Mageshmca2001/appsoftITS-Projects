using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keller.SPM.ProcotolGeneration.Protocol.Serial
{
    public class RTCRead :OBISReadRequest
    {
        public byte[] RTCMeterInit { get; set; } = { 0x03, 0x61, 0x54, 0x9C, 0xBA, 0xE6, 0xE6, 0x00 };
        public byte[] RTCFixedBytes { get; set; } = { 0x03, 0x61, 0x76, 0x8C, 0xB8, 0xE6, 0xE6, 0x00 };
        public byte[] RTCFixedBytesFT { get; set; } = { 0x03, 0x61, 0xBA, 0xEC, 0xB4, 0xE6, 0xE6, 0x00 };
    }
}
