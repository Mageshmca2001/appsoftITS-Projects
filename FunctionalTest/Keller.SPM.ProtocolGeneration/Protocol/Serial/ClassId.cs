using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keller.SPM.ProtocolGeneration.Protocol.Serial
{
    public class ClassId
    {
        public byte[] Class1 { get; set; } = { 0x00, 0x01 };
        public byte[] Class7 { get; set; } = { 0x00, 0x07 };
        public byte[] Class8 { get; set; } = { 0x00, 0x08 };
        public byte[] Class40 { get; set; } = { 0x00, 0x28 };
        public byte[] Class41 { get; set; } = { 0x00, 0x29 };
        public byte[] Class45 { get; set; } = { 0x00, 0x2D };
    }
}
