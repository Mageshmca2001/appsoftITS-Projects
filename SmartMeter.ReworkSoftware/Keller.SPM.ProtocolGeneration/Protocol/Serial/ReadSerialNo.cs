using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keller.SPM.ProcotolGeneration.Protocol.Serial
{
    public class ReadSerialNo :SerialNumber
    {
        public byte[] ReadFixedBytes { get; set; } = { 0x03, 0x61, 0x54, 0x9C, 0xBA, 0xE6, 0xE6, 0x00 };
        public byte[] ReadFixedBytesFT { get; set; } = { 0x03, 0x61, 0x98, 0xFC, 0xB6, 0xE6, 0xE6, 0x00 };
    }
}
