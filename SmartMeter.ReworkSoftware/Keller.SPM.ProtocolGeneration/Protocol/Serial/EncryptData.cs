using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keller.SPM.ProcotolGeneration.Protocol.Serial
{
    public class EncryptData
    {
        public byte[] CipheredData { get; set; }
        public byte[] AuthenticationTag { get; set; }
    }
}
