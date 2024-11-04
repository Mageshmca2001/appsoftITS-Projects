using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keller.SPM.ProtocolGeneration.Protocol.Serial
{
    public class FunctionalTestLogicalName : LogicalName
    {
        public byte[] ClassId { get; set; } = { 0x00, 0x01 };
        public byte[] OBISCode { get; set; } = { 0x01, 0x00, 0x60, 0x02, 0x86, 0x00 };

    }
}
