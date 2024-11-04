using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keller.SPM.ProcotolGeneration.Protocol.Serial
{
   public class Disconnect
    {
        public byte[] DisconnectCommandBytes = new byte[9];
        
        public byte[] DiscFixedBytes { get; set; } = { 0xA0, 0x07, 0x03, 0x61, 0x53, 0x65, 0x81 };
        public byte[] DiscPCFixedByte { get; set; } = { 0xA0, 0x07, 0x03, 0x21, 0x53, 0x03, 0xC7 };
    }
}
