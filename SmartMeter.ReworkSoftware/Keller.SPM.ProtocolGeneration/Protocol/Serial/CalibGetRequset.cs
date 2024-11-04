using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keller.SPM.ProcotolGeneration.Protocol.Serial
{
    public class CalibGetRequset
    {
        public byte[] CalibReadCommandBytes = new byte[44];
        public byte[] OBISPlainText = new byte[13];
        public byte[] CalibReadHDLCLength { get; set; } = { 0x2C };
        public byte[] CalibReadFixedBytes { get; set; } = { 0x03, 0x61, 0x54, 0x9C, 0xBA, 0xE6, 0xE6, 0x00 };
        public byte[] CalibReadFixedBytesRead { get; set; } = { 0x03, 0x61, 0x76, 0x8C, 0xB8, 0xE6, 0xE6, 0x00 };
        public byte[] CalibReadRequestBlock { get; set; } = { 0xD0, 0x1E, 0x30 };
        public byte[] PlainText { get; set; } = { 0xC0, 0x01, 0xC1, 0x00, 0x01, 0x01, 0x00, 0x60, 0x02, 0x81, 0x00 };  // 1.0.96.2.129.0
    }
}
