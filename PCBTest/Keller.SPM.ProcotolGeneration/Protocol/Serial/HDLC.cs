using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keller.SPM.ProcotolGeneration.Protocol.Serial
{
    public class HDLC
    {
        public byte Flag { get; set; } = 0x7E;
        public byte FrameFormat { get; set; } = 0xA0;
        public byte FrameLength { get; set; }
        public byte[] Address { get; set; } = { 0x03, 0x61 };
        public byte[] PCAddress { get; set; } = { 0x03, 0x21 };
        public byte ControlByte { get; set; }
        public byte[] HCS { get; set; }
        public byte[] LLC { get; set; } = { 0xE6, 0xE6, 0x00 };

    }
}
