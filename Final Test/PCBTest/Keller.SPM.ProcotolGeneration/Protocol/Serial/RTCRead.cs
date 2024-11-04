using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keller.SPM.ProcotolGeneration.Protocol.Serial
{
    public class RTCRead :OBISReadRequest
    {
        public byte[] RTCFixedBytes { get; set; } = { 0x03, 0x61, 0xBA, 0xEC, 0xB4, 0xE6, 0xE6, 0x00 };
    }
}
