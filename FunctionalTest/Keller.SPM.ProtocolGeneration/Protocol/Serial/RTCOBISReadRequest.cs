using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keller.SPM.ProtocolGeneration.Protocol.Serial
{
   public class RTCOBISReadRequest
    {
        public byte[] OBISPlainText = new byte[13];
        public byte[] InvocationCounterBytes = new byte[4];
        public byte[] PlainText { get; set; } = { 0xC0, 0x01, 0xC1, 0x00, 0x08, 0x00, 0x00, 0x01, 0x00, 0x00, 0xFF };
        public byte[] HDLCLen { get; set; } = { 0x2C };
        public byte[] ReqBlock { get; set; } = { 0xD0, 0x1E, 0x30 };
        public byte[] InvocationCounter { get; set; } = { 0x00, 0x00, 0x00 };
        public byte[] OBISFixedBytes { get; set; } = { 0x03, 0x61, 0x54, 0x9C, 0xBA, 0xE6, 0xE6, 0x00 };
    }
}
