using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keller.SPM.ProcotolGeneration.Protocol.Serial
{
    public class GlobalkeyWrite
    {
        public byte[] SetGKCommandBytes = new byte[78];
        public byte[] SetGKFixedBytes { get; set; } = { 0x03, 0x61, 0x76, 0x68, 0x21, 0xE6, 0xE6, 0x00 };
        public byte[] SetGKActionBlock1 { get; set; } = { 0xD3, 0x3E, 0x30 };
        public byte[] SetGKLenght { get; set; } = { 0x4C };
        public byte[] SetGKPlainText { get; set; } = { 0xC3, 0x01, 0xC1, 0x00, 0x40, 0x00, 0x00, 0x2B, 0x00, 0x03, 0xFF, 0x02, 0x01, 0x01, 0x01, 0x02, 0x02, 0x16, 0x00, 0x09, 0x18 };
    }
}
