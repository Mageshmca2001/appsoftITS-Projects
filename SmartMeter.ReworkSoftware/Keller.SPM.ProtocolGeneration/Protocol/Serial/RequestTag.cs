using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keller.SPM.ProcotolGeneration.Protocol.Serial
{
    public class RequestTag
    {
        public byte InitiateRequest { get; set; } = 0x21;
        public byte AARQTag { get; set; } = 0x60;
        public byte ActionTag { get; set; } = 0xCB;
        public byte GetHerader { get; set; } = 0xD0;
        public byte GetRequest { get; set; } = 0xC0;
        public byte SetHerader { get; set; } = 0xD1;
        public byte SetRequest { get; set; } = 0xC1;
        public byte ExceptionRequest { get; set; } = 0xD8;
    }
}
