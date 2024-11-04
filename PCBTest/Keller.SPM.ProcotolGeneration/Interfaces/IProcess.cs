using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keller.SPM.ProcotolGeneration.Interfaces
{
    public interface IProcess
    {
        byte[] SendCommand(byte[] cmd);
        byte[] WriteCommand(byte[] cmd);
    }
}
