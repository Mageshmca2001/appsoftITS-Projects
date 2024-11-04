using Keller.SPM.ProcotolGeneration.Protocol.Serial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keller.SPM.ProcotolGeneration.ViewModels
{
    public class CommonMV
    {
        public ProtocolFlags ProtocolFlags { get; set; }
        public SecurityKeys SecurityKeys { get; set; }
        public SNRM SNRM { get; set; }
        public Disconnect Disconnect { get; set; }
        public ErrorDetection ErrorDetection { get; set; }


    }
}
