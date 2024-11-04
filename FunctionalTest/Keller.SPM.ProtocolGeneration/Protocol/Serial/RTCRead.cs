using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keller.SPM.ProtocolGeneration.Protocol.Serial
{
    public class RTCRead: RTCOBISReadRequest
    {
        public byte[] RTCFixedBytes { get; set; } = { 0x03, 0x61, 0x76, 0x8C, 0xB8, 0xE6, 0xE6, 0x00 };
    }
}
