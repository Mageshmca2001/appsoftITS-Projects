using Encryption;
using Keller.SPM.ProcotolGeneration.Protocol.Serial;
using SmartMeterDLMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVG.ProductionProcess.PCBTest.Commands
{
    public class RTCReadWrite
    {
        AARQ objAARQ = new AARQ();
        SecurityKeys securityKey = new SecurityKeys();
        ProtocolFlags protocolFlags = new ProtocolFlags();
        EncryptData encryptData = new EncryptData();
        ErrorDetection errorDetection = new ErrorDetection();

        #region
        byte[] _CipheredData;
        byte[] _AuthenticationTag=new byte[12];
        byte[] FinalNonce = new byte[12];
        byte[] AssociationKey = new byte[17];
        byte[] DedicatedKey = new byte[16];
        #endregion

     

      

        private byte[] FrameNonce(byte[] Nonce, byte[] I_Counter, int NonceLen)
        {
            Buffer.BlockCopy(Nonce, 0, FinalNonce, 0, NonceLen);
            Buffer.BlockCopy(I_Counter, 0, FinalNonce, 8, I_Counter.Length);
            return FinalNonce;
        }

        private byte[] FrameAAD(byte[] PasswordKey)
        {
            //  byte[] AAD = new byte[17];
            AssociationKey[0] = 0x30;
            Buffer.BlockCopy(PasswordKey, 0, AssociationKey, 1, PasswordKey.Length);
            return AssociationKey;
        }
    }
}
