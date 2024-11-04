using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keller.SPM.ProcotolGeneration.Protocol.Serial
{
    public class ControlBytes
    {
        public byte SNRMCByte { get; set; } = 0x93;
        public byte AARQCByte { get; set; } = 0x10;
        public byte ActionCByte { get; set; } = 0x32;
        public byte LogicalCByte { get; set; } = 0x54;
        public byte GetDataCByte { get; set; } = 0x76;
        public byte SetDataCByte { get; set; } = 0x54;
    }
}
