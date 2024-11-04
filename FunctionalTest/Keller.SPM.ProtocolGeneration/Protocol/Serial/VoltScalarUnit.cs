using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keller.SPM.ProtocolGeneration.Protocol.Serial
{
    public class VoltScalarUnit
    {
        public byte[] HDLCLen { get; set; } = { 0x2C };
        public byte[] RequestBlock { get; set; } = { 0xD0, 0x1E, 0x30, };
      //  public byte[] InvocationCounter { get; set; } = { 0x00, 0x00, 0x00, 0x00 };
        public byte[] PlainText { get; set; } = { 0xC0, 0x01, 0xC1, 0x00, 0x03, 0x01, 0x00, 0x0C, 0x07, 0x00, 0xFF, 0x03, 0x00 };
        public byte[] FixedBytes { get; set; } = { 0x03, 0x61, 0xBA, 0xEC, 0xB4, 0xE6, 0xE6, 0x00 };
    }
}
