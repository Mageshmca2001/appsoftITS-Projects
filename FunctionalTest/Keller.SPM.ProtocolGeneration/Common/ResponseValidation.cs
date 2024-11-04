using Keller.SPM.ProcotolGeneration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keller.SPM.ProcotolGeneration.Common
{
    public class ResponseValidation
    {
        public ResponseCode ValidateResponse(byte[] response)
        {
            byte testCode = response[0];
            ResponseCode responseCodeDescription = new ResponseCode();
            switch (testCode)


            {
                case 0:
                    responseCodeDescription.CodeNo = Convert.ToInt32(testCode);
                    responseCodeDescription.CodeDescription = "";
                    break;
                case 23:
                    responseCodeDescription.CodeNo = Convert.ToInt32(testCode);
                    responseCodeDescription.CodeDescription = "";
                    break;
            }
            return responseCodeDescription;
        }
    }
}
