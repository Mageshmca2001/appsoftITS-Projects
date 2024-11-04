using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keller.SPM.ProcotolGeneration.Protocol.Serial
{
    public class ProtocolFlags
    {
        public byte[] StartFlag { get; set; } = { 0x7E };
        public byte[] EndFlage { get; set; } = { 0x7E };
    }
}
