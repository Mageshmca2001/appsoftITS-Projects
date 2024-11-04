using Keller.SPM.ProcotolGeneration.Protocol;
using Keller.SPM.ProcotolGeneration.Protocol.Serial;
using Keller.SPM.ProtocolGeneration.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVG.ProductionProcess.FunctionalTest.CommonConstant
{
    public class CommonCommand
    {
        #region Global Objects
        public CommonMV commonMV = new CommonMV();


        #endregion

        public byte[] SNRMCommand()
        {

            try
            {
                commonMV.SNRM = new SNRM();
                commonMV.ProtocolFlags = new ProtocolFlags();
                commonMV.SecurityKeys = new SecurityKeys();

                Buffer.BlockCopy(commonMV.ProtocolFlags.StartFlag, 0, commonMV.SNRM.SNRMCommandBytes, 0, commonMV.ProtocolFlags.StartFlag.Length);//Starting Character 
                Buffer.BlockCopy(commonMV.SecurityKeys.HDLCFrame_Format, 0, commonMV.SNRM.SNRMCommandBytes, 1, commonMV.SecurityKeys.HDLCFrame_Format.Length);//Command 
                Buffer.BlockCopy(commonMV.SNRM.SNRMLen, 0, commonMV.SNRM.SNRMCommandBytes, 2, commonMV.SNRM.SNRMLen.Length);
                Buffer.BlockCopy(commonMV.SNRM.SNRMFixedBytes, 0, commonMV.SNRM.SNRMCommandBytes, 3, commonMV.SNRM.SNRMFixedBytes.Length);
                Buffer.BlockCopy(commonMV.SNRM.SNRMInfoField, 0, commonMV.SNRM.SNRMCommandBytes, 8, commonMV.SNRM.SNRMInfoField.Length);
                Buffer.BlockCopy(commonMV.SNRM.SNRMFCS, 0, commonMV.SNRM.SNRMCommandBytes, 31, commonMV.SNRM.SNRMFCS.Length);
                Buffer.BlockCopy(commonMV.ProtocolFlags.EndFlage, 0, commonMV.SNRM.SNRMCommandBytes, 33, commonMV.ProtocolFlags.EndFlage.Length);

                return commonMV.SNRM.SNRMCommandBytes;

            }

            catch (Exception ex)
            {
                //  MessageBox.Show("\nError: " + ex);  //-----
                return commonMV.SNRM.SNRMCommandBytes;
            }
        }
        public byte[] DisconnectCommand()
        {
            try
            {
                commonMV.Disconnect = new Disconnect();
                commonMV.ProtocolFlags = new ProtocolFlags();
                commonMV.SecurityKeys = new SecurityKeys();
                Buffer.BlockCopy(commonMV.ProtocolFlags.StartFlag, 0, commonMV.Disconnect.DisconnectCommandBytes, 0, 1);
                Buffer.BlockCopy(commonMV.Disconnect.DiscFixedBytes, 0, commonMV.Disconnect.DisconnectCommandBytes, 1, 7);
                Buffer.BlockCopy(commonMV.ProtocolFlags.EndFlage, 0, commonMV.Disconnect.DisconnectCommandBytes, 8, 1);

            }
            catch { }
            return commonMV.Disconnect.DisconnectCommandBytes;
        }

        public byte[] PublicClientDisconnect()
        {


            try
            {
                commonMV.Disconnect = new Disconnect();
                commonMV.ProtocolFlags = new ProtocolFlags();
                commonMV.SecurityKeys = new SecurityKeys();
                Buffer.BlockCopy(commonMV.ProtocolFlags.StartFlag, 0, commonMV.Disconnect.DisconnectCommandBytes, 0, 1);
                Buffer.BlockCopy(commonMV.Disconnect.DiscPCFixedByte, 0, commonMV.Disconnect.DisconnectCommandBytes, 1, 7);
                Buffer.BlockCopy(commonMV.ProtocolFlags.EndFlage, 0, commonMV.Disconnect.DisconnectCommandBytes, 8, 1);

            }
            catch { }
            return commonMV.Disconnect.DisconnectCommandBytes;

        }
    }
}
