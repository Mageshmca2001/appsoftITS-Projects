using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keller.SPM.ProcotolGeneration.Protocol.Serial
{
    public class HLSKeyWrite
    {
        public byte[] SetHLSCommandBytes = new byte[64];
        public byte[] SetHLSFixedBytes { get; set; } = { 0x03, 0x61, 0x54, 0x4B,0x40, 0xE6, 0xE6, 0x00 };
        public byte[] SetGKLenght { get; set; } = { 0x3E };
        public byte[] SetHLSActionBlock1 { get; set; } = { 0xD1, 0x30, 0x30 };
        public byte[] SetHLSPlainText { get; set; } = { 0XC1, 0x01, 0xC1, 0x00, 0x0F, 0x00, 0x00, 0x28, 0x00, 0x03, 0xFF, 0x07, 0x00, 0x09, 0x10}; //15
    }
}
