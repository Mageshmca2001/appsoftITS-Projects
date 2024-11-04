using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keller.SPM.ProtocolGeneration.Protocol.Serial
{
    public class SerialNumber
    {
        public byte[] SPlainText = new byte[13];
        public byte[] SInvocationCounter = new byte[4];
        public byte[] PlainText { get; set; } = { 0xC0, 0x01, 0xC1, 0x00, 0x01, 0x01, 0x00, 0x60, 0x02, 0x84, 0x00 };  // 1.0.96.2.132.0
        public byte[] HDLCLen { get; set; } = { 0x2C };
        public byte[] InvocationCounter { get; set; } = { 0x00, 0x00, 0x00 };
        public byte[] FixedBytes { get; set; } = { 0x03, 0x61, 0x76, 0xB8, 0x8C, 0xE6, 0xE6, 0x00 };
        public byte[] RequestBlock { get; set; } = { 0xD0, 0x1E, 0x30 };
    }
}
