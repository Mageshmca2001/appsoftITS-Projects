using AVG.ProductionProcess.FunctionalTest.CommonConstant;
using DLMS_MeterReading;
using Encryption;
using Keller.SPM.ProcotolGeneration.Protocol;
using Keller.SPM.ProcotolGeneration.Protocol.Serial;
using Keller.SPM.ProtocolGeneration.Protocol.Serial;
using SmartMeterDLMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Keller.SPM.Communication.MUT;

namespace AVG.ProductionProcess.FunctionalTest.Forms
{
    public partial class MasterForm : System.Windows.Forms.Form
    {
        #region
        public SerialCOM MUTSerialCOM = new SerialCOM();
        public delegate void DelMethod(byte[] s);

        CommonCommand comm = new CommonCommand();
        #endregion

        #region Global Variable
        public byte[] ReceiveStr;
        public int CmdCount = 0;

        byte[] Encrypt_StoC = new byte[16];
        byte[] StoC = new byte[16];
        byte[] _CipheredData;
        byte[] _AuthenticationTag = new byte[12];
        byte[] FinalNonce = new byte[12];
        byte[] AssociationKey = new byte[17];
        byte[] DedicatedKey = new byte[16];
        public byte[] ServerNonce = new byte[8];

        #endregion
        public MasterForm()
        {
            MUTSerialCOM.DataReceived += new dataReceived(MUTSerialCOM_DataReceived);
            InitializeComponent();
        }

        private void MUTSerialCOM_DataReceived(object sender, SerialPortEventArgs arg)
        {
            ReceiveStr = new byte[arg.ReceivedData.Length];
            ReceiveStr = arg.ReceivedData;

            CmdCount = CmdCount + 1;

            this.BeginInvoke(new DelMethod(DataSegMethod), new object[] { ReceiveStr });
        }

        public void DataSegMethod(byte[] ReceiveStr)
        {

            try
            {
                //  int i = 1;

                if (ReceiveStr[0] != 0)
                {
                    string TxtStr = string.Empty;
                    for (int i = 0; i < ReceiveStr[2] + 2; i++)
                    {
                        TxtStr = TxtStr + " " + ReceiveStr[i].ToString("X2");
                    }

                    this.RTCBox.AppendText("\n" + DateTime.Now.ToString("\ndd/MM/yyyy HH:mm:ss") + " Response Data : " + TxtStr + " 7E\n");
                    return;
                }
            }
            catch (Exception ex) { }
        }

        private void LoadCOMPort()
        {
            string[] Port = SerialPort.GetPortNames();
            cmbSerialPort.Items.AddRange(Port);
            //cmbSerialPort1.SelectedIndex = 0;
           
        }
        private void btnCOMMOpen_Click(object sender, EventArgs e)
        {

            try
            {
                if(btnCOMMOpen.Text=="COM Open")
                {
                    bool isConnected;
                    string PortName = cmbSerialPort.Text;
                    isConnected = MUTSerialCOM.OpenCOM(PortName, 9600, 8, Parity.None, StopBits.One);
                    if (isConnected == true)
                    {
                        btnCOMMOpen.Text = "COM Close";
                        MessageBox.Show("Successfully COM Port opened");
                        this.RTCBox.AppendText(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss  : ") + "Successfully Serial COM Port opened");
                    }
                    else
                    {
                        MessageBox.Show("Please select another COM");
                    }
                }
                else
                {
                    btnCOMMOpen.Text = "COM Open";
                    MUTSerialCOM.Close();
                }
              
            }
            catch (Exception ex) { throw ex; }
        }

        private void MasterForm_Load(object sender, EventArgs e)
        {
           
            LoadCOMPort();
        }

        private void btn_SNoRead_Click(object sender, EventArgs e)
        {
             bwReadMeter.RunWorkerAsync();

        }
    
    

        private void bwReadMeter_DoWork(object sender, DoWorkEventArgs e)
        {
            if(bwReadTest.CancellationPending)
            {
                e.Cancel = true;
            }
            // Sno();                        
            ReadSno();

            Thread.Sleep(300);
            ReadRealTimeClock();
            //RTC();
        }



       

        //--------------------- SNRM, AARQ, ACTION  ------------------------------------
        private void SNRMCmd()
        {
            byte[] SNRMCmd = comm.SNRMCommand();
            string ss = String.Concat(SNRMCmd.Select(b => b.ToString("X2") + " "));
            //this.RTCBox.AppendText("\n" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss ") + "  SNRM Request : " + ss);
            this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText("\n" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss ") + "  SNRM Request : " + ss)));
            MUTSerialCOM.Send(SNRMCmd); Thread.Sleep(250);
        }

        private void AARQCommand()
        {
            AARQ objAARQ = new AARQ();
            SecurityKeys securityKey = new SecurityKeys();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();
            try
            {

                Buffer.BlockCopy(objAARQ.AARQPlainText, 3, DedicatedKey, 0, 16);
                byte[] _FinalNonce = FrameNonce(securityKey.CNonce, objAARQ.AARQInvocationCounter, securityKey.CNonce.Length);
                byte[] _AAD = FrameAAD(securityKey.PasswordKey);
                _CipheredData = new byte[31];

                byte[] bResult = AESGCM.SimpleEncrypt(objAARQ.AARQPlainText, securityKey.PasswordKey, _FinalNonce, _AAD);

                string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));
                Buffer.BlockCopy(bResult, 0, _CipheredData, 0, 31);
                Buffer.BlockCopy(bResult, 31, _AuthenticationTag, 0, 12);

                //----------- Frame the AARQ Send Request-----------------//
                byte[] SAEChar = protocolFlags.StartFlag;
                byte[] FrameFormat = securityKey.HDLCFrame_Format;
                byte[] _HDLCLength = objAARQ.AARQHDLCLength;
                byte[] AARQRequest = objAARQ.AARQReq;
                byte[] FrameLen = objAARQ.AARQFrameLen;
                encryptData.CipheredData = _CipheredData;
                encryptData.AuthenticationTag = _AuthenticationTag;


                //---------------Final AARQ Command ------------------//



                // byte[] FinalCmd = new byte[objAARQ.AARQHDLCLength[0] + 2];
                objAARQ.AARQCommandBytes[0] = SAEChar[0];
                objAARQ.AARQCommandBytes[1] = FrameFormat[0];
                objAARQ.AARQCommandBytes[2] = _HDLCLength[0];

                objAARQ.AARQFixedBytes.CopyTo(objAARQ.AARQCommandBytes, 3);
                objAARQ.AARQCommandBytes[objAARQ.AARQFixedBytes.Length + 3] = AARQRequest[0];
                objAARQ.AARQCommandBytes[objAARQ.AARQFixedBytes.Length + 4] = FrameLen[0];


                int len = protocolFlags.StartFlag.Length + securityKey.HDLCFrame_Format.Length + objAARQ.AARQHDLCLength.Length + objAARQ.AARQFixedBytes.Length +
                          objAARQ.AARQReq.Length + objAARQ.AARQFrameLen.Length;

                objAARQ.AARQFixedBlock1.CopyTo(objAARQ.AARQCommandBytes, len);
                len += objAARQ.AARQFixedBlock1.Length;

                objAARQ.AARQFixedBlock2.CopyTo(objAARQ.AARQCommandBytes, len);
                len += objAARQ.AARQFixedBlock2.Length;

                //objAARQ.ClientSystemTitle.CopyTo(FinalCmd, len);
                //len += objAARQ.ClientSystemTitle.Length;
                securityKey.CNonce.CopyTo(objAARQ.AARQCommandBytes, len);  // ClientSystemTitle = CNonce
                len += securityKey.CNonce.Length;


                objAARQ.AARQFixedBlock3.CopyTo(objAARQ.AARQCommandBytes, len);
                len += objAARQ.AARQFixedBlock3.Length;

                objAARQ.AARQFixedBlock4.CopyTo(objAARQ.AARQCommandBytes, len);
                len += objAARQ.AARQFixedBlock4.Length;

                objAARQ.AARQFixedBlock5.CopyTo(objAARQ.AARQCommandBytes, len);
                len += objAARQ.AARQFixedBlock5.Length;

                objAARQ.AARQInvocationCounter.CopyTo(objAARQ.AARQCommandBytes, len);
                len += objAARQ.AARQInvocationCounter.Length;

                encryptData.CipheredData.CopyTo(objAARQ.AARQCommandBytes, len);
                len += encryptData.CipheredData.Length;

                encryptData.AuthenticationTag.CopyTo(objAARQ.AARQCommandBytes, len);
                len += encryptData.AuthenticationTag.Length;

                errorDetection.FCS = FCS.ComputeFCS(objAARQ.AARQCommandBytes);
                errorDetection.FCS.CopyTo(objAARQ.AARQCommandBytes, len);

                objAARQ.AARQCommandBytes[objAARQ.AARQCommandBytes.Length - 1] = SAEChar[0];



                string _aarq = String.Concat(objAARQ.AARQCommandBytes.Select(b => b.ToString("X2") + " "));
                //  this.RTCBox.AppendText("\n" + DateTime.Now.ToString("\ndd/MM/yyyy HH:mm:ss ") + "  AARQ Request : " + _aarq);
                this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText("\n" + DateTime.Now.ToString("\ndd/MM/yyyy HH:mm:ss ") + "  AARQ Request : " + _aarq)));
                MUTSerialCOM.Send(objAARQ.AARQCommandBytes); Thread.Sleep(500);


                Buffer.BlockCopy(ReceiveStr, 40, ServerNonce, 0, 8);  
                Buffer.BlockCopy(ReceiveStr, 65, StoC, 0, 16);
                string StoC_Str = string.Concat(StoC.Select(b => b.ToString("X2") + " "));
                // this.RTCBox.AppendText(Environment.NewLine + "S to C : " + StoC_Str + Environment.NewLine);
                this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "S to C : " + StoC_Str + Environment.NewLine)));

                byte[] tempPwd = securityKey.SecretKey; // Encoding.UTF8.GetBytes("AVG_SECRET_ASSC2");
                DLMS_AES.Encrypt(StoC, tempPwd);

                string EncryptStr = string.Concat(StoC.Select(b => b.ToString("X2") + " "));
                // this.RTCBox.AppendText(Environment.NewLine + "Encrypted S to C : " + EncryptStr + Environment.NewLine);
                this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "Encrypted S to C : " + EncryptStr + Environment.NewLine)));
                Buffer.BlockCopy(StoC, 0, Encrypt_StoC, 0, 16);

            }
            catch (Exception ex)
            {

            }

        }

        private void ActionRequestCmd()
        {
            SecurityKeys securityKey = new SecurityKeys();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();

            ActionRequest objAction = new ActionRequest();
            try
            {

                byte[] _FinalNonce = FrameNonce(securityKey.CNonce, objAction.ActInvocationCounter, securityKey.CNonce.Length);
                byte[] _AAD = FrameAAD(securityKey.PasswordKey);

                Buffer.BlockCopy(Encrypt_StoC, 0, objAction.ActPlainText, 15, 16);
                string PlainStr = String.Concat(objAction.ActPlainText.Select(b => b.ToString("X2") + " "));
                //this.RTCBox.AppendText(Environment.NewLine + "\nAction CIPHER : " + PlainStr + Environment.NewLine);
                this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\nAction CIPHER : " + PlainStr + Environment.NewLine)));

                byte[] bResult = AESGCM.SimpleEncrypt(objAction.ActPlainText, securityKey.PasswordKey, _FinalNonce, _AAD);

                _CipheredData = new byte[31];
                string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));
                Buffer.BlockCopy(bResult, 0, _CipheredData, 0, 31);
                Buffer.BlockCopy(bResult, 31, _AuthenticationTag, 0, 12);

                //---------------- Frame the ACTION Send Request ----------------------------// 

                byte[] SAEChar = protocolFlags.StartFlag;
                byte[] FrameFormat = securityKey.HDLCFrame_Format;
                byte[] _HDLCLength = objAction.ActionHDLCLength;
                encryptData.CipheredData = _CipheredData;
                encryptData.AuthenticationTag = _AuthenticationTag;

                //---------------Final ACTION Command ------------------//

                byte[] FinalCmd = new byte[_HDLCLength[0] + 2];
                FinalCmd[0] = SAEChar[0];
                FinalCmd[1] = FrameFormat[0];
                FinalCmd[2] = _HDLCLength[0];
                objAction.ActionFixedBytes.CopyTo(FinalCmd, 3);

                int len = protocolFlags.StartFlag.Length + securityKey.HDLCFrame_Format.Length + objAction.ActionHDLCLength.Length + objAction.ActionFixedBytes.Length;

                objAction.ActionBlock1.CopyTo(FinalCmd, len);
                len += objAction.ActionBlock1.Length;
                objAction.ActInvocationCounter.CopyTo(FinalCmd, len);
                len += objAction.ActInvocationCounter.Length;
                encryptData.CipheredData.CopyTo(FinalCmd, len);
                len += encryptData.CipheredData.Length;
                encryptData.AuthenticationTag.CopyTo(FinalCmd, len);
                len += encryptData.AuthenticationTag.Length;

                errorDetection.FCS = FCS.ComputeFCS(FinalCmd);
                errorDetection.FCS.CopyTo(FinalCmd, len);

                FinalCmd[FinalCmd.Length - 1] = SAEChar[0];

                string finalCommand = String.Concat(FinalCmd.Select(b => b.ToString("X2") + " "));
                //  this.RTCBox.AppendText(Environment.NewLine + "\nACTION REQUEST : " + finalCommand + Environment.NewLine);
                this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\nACTION REQUEST : " + finalCommand + Environment.NewLine)));
                MUTSerialCOM.Send(FinalCmd); Thread.Sleep(500);
            }

            catch(Exception ex)
            {
            }

        }
        //-------------------------------- HDLC Read Serial Number Commands ----------------------------

        private void ReadSno()
        {
            CmdCount = 0;
            try
            {
                int waitSecond = 100;

                byte[] DisconnectCmd = comm.DisconnectCommand();
                //MUTSerialCOM.Send(DisconnectCmd);
                //Thread.Sleep(waitSecond);

                SNRMCmd();
                Thread.Sleep(650);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "SNRM"));

                AARQCommand();
                Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "AARQ"));

                ActionRequestCmd();
                Thread.Sleep(650);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "ACTION"));

                SNOLogicalName();
                //int milliseconds = 650;
                // Thread.Sleep(650);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "OBIS Code"));

                ReadSerialNo();
                //  Thread.Sleep(650);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "Read SNo"));

                DisconnectCmd = comm.DisconnectCommand();
                string dd = String.Concat(DisconnectCmd.Select(b => b.ToString("X2") + " "));
                this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText("\n" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss ") + "  Disconnect Request : " + dd)));
                MUTSerialCOM.Send(DisconnectCmd);
                Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "DISCONNECT"));

                //  isSNoWrite = false;
            }
            catch { }


        }
        private void SNOLogicalName()
        {
            SecurityKeys securityKeys = new SecurityKeys();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();
            SerialNumber serialNumber = new SerialNumber();
            byte[] bInstantData;
            try
            {
                byte[] FinalCmd;

                //   byte[] PlainText = { 0xC0, 0x01, 0xC1, 0x00, 0x01, 0x01, 0x00, 0x60, 0x02, 0x83, 0x00 };
                Buffer.BlockCopy(serialNumber.PlainText, 0, serialNumber.SPlainText, 0, 11);
                serialNumber.SPlainText[11] = 0x01;
                serialNumber.SPlainText[12] = 0x00;
                Buffer.BlockCopy(serialNumber.InvocationCounter, 0, serialNumber.SInvocationCounter, 0, 3); //---check this 2 or 3
                serialNumber.SInvocationCounter[3] = 0x00;

                byte[] _FinalNonce = FrameNonce(securityKeys.CNonce, serialNumber.SInvocationCounter, securityKeys.CNonce.Length);
                byte[] _AAD = FrameAAD(securityKeys.PasswordKey);
                _CipheredData = new byte[13];

                byte[] bResult = AESGCM.SimpleEncrypt(serialNumber.SPlainText, DedicatedKey, _FinalNonce, _AAD);

                string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));
                //   this.RTCBox.AppendText(Environment.NewLine + "\nSerial No CIPHER : " + ss + Environment.NewLine);
                this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\nSerial No CIPHER : " + ss + Environment.NewLine)));
                Buffer.BlockCopy(bResult, 0, _CipheredData, 0, 13);
                Buffer.BlockCopy(bResult, 13, _AuthenticationTag, 0, 12);

                //---------------- Frame the Data Request 1(OBIS Code) Send Request ----------------------------// 


                byte[] SAEChar = protocolFlags.StartFlag;
                byte[] FrameFormat = securityKeys.HDLCFrame_Format;
                byte[] _HDLCLength = serialNumber.HDLCLen;
                encryptData.CipheredData = _CipheredData;
                encryptData.AuthenticationTag = _AuthenticationTag;

                //---------------Final OBIS Code Command ------------------//

                FinalCmd = new byte[_HDLCLength[0] + 2];
                FinalCmd[0] = SAEChar[0];
                FinalCmd[1] = FrameFormat[0];
                FinalCmd[2] = _HDLCLength[0];
                serialNumber.FixedBytes.CopyTo(FinalCmd, 3);

                int len = protocolFlags.StartFlag.Length + securityKeys.HDLCFrame_Format.Length +
                          serialNumber.HDLCLen.Length + serialNumber.FixedBytes.Length;

                serialNumber.RequestBlock.CopyTo(FinalCmd, len);
                len += serialNumber.RequestBlock.Length;
                serialNumber.SInvocationCounter.CopyTo(FinalCmd, len);
                len += serialNumber.SInvocationCounter.Length;
                encryptData.CipheredData.CopyTo(FinalCmd, len);
                len += encryptData.CipheredData.Length;
                encryptData.AuthenticationTag.CopyTo(FinalCmd, len);
                len += encryptData.AuthenticationTag.Length;

                errorDetection.FCS = FCS.ComputeFCS(FinalCmd);
                errorDetection.FCS.CopyTo(FinalCmd, len);

                FinalCmd[FinalCmd.Length - 1] = SAEChar[0];


                string finalCommand = String.Concat(FinalCmd.Select(b => b.ToString("X2") + " "));
                //this.RTCBox.AppendText(Environment.NewLine + "\nSerial No Logical Name : " + finalCommand + Environment.NewLine);
                this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\nSerial No Logical Name : " + finalCommand + Environment.NewLine)));

                MUTSerialCOM.Send(FinalCmd); Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
            if (ReceiveStr[0] != 0)
            {
                // byte 13 (From 0 ) - Length           12
                // byte 15,16,17,18  - FrameCount       14,15,16,17
                // From Byte 19      - Data Started (Length - 6)   18
                // Server Nonce

                //  byte[] _SDecNonce = Encoding.UTF8.GetBytes("AVG12345");

                byte[] _SDecNonce = ServerNonce; //new byte[] { 0x41, 0x56, 0x47, 0x30, 0x30, 0x30, 0x30, 0x34 }; // (Server System Title from AARE )                   
                byte[] _SDecsubNonce_FrameCount = new byte[] { 0x00, 0x00, 0x00, 0x02 };

                byte[] _SFinalNonce = FrameNonce(_SDecNonce, _SDecsubNonce_FrameCount, _SDecNonce.Length);



                int iLen = Convert.ToInt32(ReceiveStr[12].ToString("X2"), 16);    //13

                byte[] _bFrameCount = new byte[4]; bInstantData = new byte[(iLen - 5) + 12];

                byte[] _bAuthenticationTag = new byte[12];
                Buffer.BlockCopy(ReceiveStr, 14, _bFrameCount, 0, 4);   // 15
                Buffer.BlockCopy(ReceiveStr, 18, bInstantData, 0, iLen - 5);   // 19
                Buffer.BlockCopy(ReceiveStr, 30, _bAuthenticationTag, 0, 12);   // iLen - 18

                string sBeforeInstantCrypt = String.Concat(bInstantData.Select(b => b.ToString("X2") + " "));

                byte[] bDecryptResult = AESGCM.SimpleDecrypt(bInstantData, DedicatedKey, _SFinalNonce);

                string sCipherResults = String.Concat(bDecryptResult.Select(b => b.ToString("X2") + " "));

                //  this.RTCBox.AppendText(Environment.NewLine + "Serial No OBIS CODE DECODED : " + sCipherResults + Environment.NewLine);
                this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "Serial No OBIS CODE DECODED : " + sCipherResults + Environment.NewLine)));

            }


        }

        private void ReadSerialNo()
        {
            SecurityKeys securityKeys = new SecurityKeys();
            ReadSerialNo serialNumber = new ReadSerialNo();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();
            byte[] bInstantData;
            try
            {
                //byte[] PlainText = { 0xC0, 0x01, 0xC1, 0x00, 0x01, 0x01, 0x00, 0x60, 0x02, 0x83, 0x00 };
                Buffer.BlockCopy(serialNumber.PlainText, 0, serialNumber.SPlainText, 0, 11);
                // Buffer.BlockCopy(PlainText, 0, serialNumber.SPlainText, 0, 11);
                serialNumber.SPlainText[11] = 0x02;
                serialNumber.SPlainText[12] = 0x00;
                Buffer.BlockCopy(serialNumber.InvocationCounter, 0, serialNumber.SInvocationCounter, 0, 3);
                serialNumber.SInvocationCounter[3] = 0x01;


                byte[] FinalCmd; // 
                try
                {

                    byte[] _FinalNonce = FrameNonce(securityKeys.CNonce, serialNumber.SInvocationCounter, securityKeys.CNonce.Length);
                    byte[] _AAD = FrameAAD(securityKeys.PasswordKey);
                    _CipheredData = new byte[13];

                    byte[] bResult = AESGCM.SimpleEncrypt(serialNumber.SPlainText, DedicatedKey, _FinalNonce, _AAD);

                    string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));
                    //this.RTCBox.AppendText(Environment.NewLine + "\nS.No REQUEST CIPHER : " + ss + Environment.NewLine);
                    this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\nS.No REQUEST CIPHER : " + ss + Environment.NewLine)));
                    Buffer.BlockCopy(bResult, 0, _CipheredData, 0, 13);
                    Buffer.BlockCopy(bResult, 13, _AuthenticationTag, 0, 12);

                    //---------------- Frame the Data Request 2(RTC DATA) Send Request ----------------------------// 


                    byte[] SAEChar = protocolFlags.StartFlag;
                    byte[] FrameFormat = securityKeys.HDLCFrame_Format;
                    byte[] _HDLCLength = serialNumber.HDLCLen;
                    encryptData.CipheredData = _CipheredData;
                    encryptData.AuthenticationTag = _AuthenticationTag;

                    //---------------Final RTC DATA Command ------------------//

                    FinalCmd = new byte[_HDLCLength[0] + 2];
                    FinalCmd[0] = SAEChar[0];
                    FinalCmd[1] = FrameFormat[0];
                    FinalCmd[2] = _HDLCLength[0];
                    serialNumber.ReadFixedBytes.CopyTo(FinalCmd, 3);

                    int len = protocolFlags.StartFlag.Length + securityKeys.HDLCFrame_Format.Length +
                              serialNumber.HDLCLen.Length + serialNumber.ReadFixedBytes.Length;

                    serialNumber.RequestBlock.CopyTo(FinalCmd, len);
                    len += serialNumber.RequestBlock.Length;
                    serialNumber.SInvocationCounter.CopyTo(FinalCmd, len);
                    len += serialNumber.SInvocationCounter.Length;
                    encryptData.CipheredData.CopyTo(FinalCmd, len);
                    len += encryptData.CipheredData.Length;
                    encryptData.AuthenticationTag.CopyTo(FinalCmd, len);
                    len += encryptData.AuthenticationTag.Length;

                    errorDetection.FCS = FCS.ComputeFCS(FinalCmd);
                    errorDetection.FCS.CopyTo(FinalCmd, len);

                    FinalCmd[FinalCmd.Length - 1] = SAEChar[0];

                    string finalCommand = String.Concat(FinalCmd.Select(b => b.ToString("X2") + " "));
                    //this.RTCBox.AppendText(Environment.NewLine + "\n Serial No Request : " + finalCommand + Environment.NewLine);
                    this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\n Serial No Request : " + finalCommand + Environment.NewLine)));
                    MUTSerialCOM.Send(FinalCmd); Thread.Sleep(500);


                    if (ReceiveStr[0] != 0)
                    {
                        // byte 12 (From 0 ) - Length
                        // byte 14,15,16,17  - FrameCount
                        // From Byte 18      - Data Started (Length - 6)
                        // Server Nonce

                        //  byte[] _SDecNonce = Encoding.UTF8.GetBytes("AVG12345");

                        byte[] _SDecNonce = ServerNonce; //new byte[] { 0x41, 0x56, 0x47, 0x30, 0x30, 0x30, 0x30, 0x34 }; // (Server System Title from AARE )                   
                        byte[] _SDecsubNonce_FrameCount = new byte[] { 0x00, 0x00, 0x00, 0x03 };

                        byte[] _SFinalNonce = FrameNonce(_SDecNonce, _SDecsubNonce_FrameCount, _SDecNonce.Length);



                        int iLen = Convert.ToInt32(ReceiveStr[12].ToString("X2"), 16);  //13
                        byte[] _bFrameCount = new byte[4]; bInstantData = new byte[(iLen - 5) + 12];

                        byte[] _bAuthenticationTag = new byte[12];
                        Buffer.BlockCopy(ReceiveStr, 14, _bFrameCount, 0, 4);   //15
                        Buffer.BlockCopy(ReceiveStr, 18, bInstantData, 0, 14);    //19  , iLen - 5
                        Buffer.BlockCopy(ReceiveStr, 32, _bAuthenticationTag, 0, 12);

                        string sBeforeInstantCrypt = String.Concat(bInstantData.Select(b => b.ToString("X2") + " "));

                        byte[] bDecryptResult = AESGCM.SimpleDecrypt(bInstantData, DedicatedKey, _SFinalNonce);

                        string sCipherResults = String.Concat(bDecryptResult.Select(b => b.ToString("X2") + " "));

                        //this.RTCBox.AppendText(Environment.NewLine + "Serial No DECODED : " + sCipherResults + Environment.NewLine);
                        this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "Serial No DECODED : " + sCipherResults + Environment.NewLine)));

                        // Serial Number Byte to Ascii Conversion
                        byte[] SNoHex = new byte[8];
                        Buffer.BlockCopy(bDecryptResult, 6, SNoHex, 0, 8);
                        string asciiString = ConvertBytesToAscii(SNoHex);

                        this.txtSerialNo.Invoke((MethodInvoker)(() => txtSerialNo.Text = asciiString));

                       

                    }
                }
                catch (Exception ex)
                {
                    // MessageBox.Show("Error: " + ex);
                }


            }
            catch (Exception ex) { };
        }


        //----------------------------- RTC READ ----------------------------------------

        private void ReadRealTimeClock()
        {
            try
            {
                int waitSecond = 100;

                byte[] DisconnectCmd = comm.DisconnectCommand();
                //MUTSerialCOM.Send(DisconnectCmd);
                //Thread.Sleep(waitSecond);

                SNRMCmd();
                Thread.Sleep(650);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "SNRM"));

                AARQCommand();
                Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "AARQ"));

                ActionRequestCmd();
                Thread.Sleep(650);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "ACTION"));

                RTCOBISCodeReadCmd();
                // Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "RTC LOGICAL NAME"));

                ReadRTC();
                // Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "RTC READ"));

                DisconnectCmd = comm.DisconnectCommand();
                string dd = String.Concat(DisconnectCmd.Select(b => b.ToString("X2") + " "));
                this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText("\n" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss ") + "  Disconnect Request : " + dd)));
                MUTSerialCOM.Send(DisconnectCmd);
                Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "DISCONNECT"));

              
            }
            catch { }
        }

        private void RTCOBISCodeReadCmd()
        {
            RTCOBISReadRequest objDataReq = new RTCOBISReadRequest();
            SecurityKeys securityKey = new SecurityKeys();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();
            try
            {
                byte[] FinalCmd;
                Buffer.BlockCopy(objDataReq.PlainText, 0, objDataReq.OBISPlainText, 0, 11);
                objDataReq.OBISPlainText[11] = 0x01;
                objDataReq.OBISPlainText[12] = 0x00;
                Buffer.BlockCopy(objDataReq.InvocationCounter, 0, objDataReq.InvocationCounterBytes, 0, 3);
                objDataReq.InvocationCounterBytes[3] = 0x00;

                byte[] _FinalNonce = FrameNonce(securityKey.CNonce, objDataReq.InvocationCounterBytes, securityKey.CNonce.Length);
                byte[] _AAD = FrameAAD(securityKey.PasswordKey);
                _CipheredData = new byte[13];

                byte[] bResult = AESGCM.SimpleEncrypt(objDataReq.OBISPlainText, DedicatedKey, _FinalNonce, _AAD);

                string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));
                this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\nDATA REQUEST CIPHER : " + ss + Environment.NewLine)));
                Buffer.BlockCopy(bResult, 0, _CipheredData, 0, 13);
                Buffer.BlockCopy(bResult, 13, _AuthenticationTag, 0, 12);

                //---------------- Frame the Data Request 1(OBIS Code) Send Request ----------------------------// 


                byte[] SAEChar = protocolFlags.StartFlag;
                byte[] FrameFormat = securityKey.HDLCFrame_Format;
                byte[] _HDLCLength = objDataReq.HDLCLen;
                encryptData.CipheredData = _CipheredData;
                encryptData.AuthenticationTag = _AuthenticationTag;

                //---------------Final OBIS Code Command ------------------//

                FinalCmd = new byte[_HDLCLength[0] + 2];
                FinalCmd[0] = SAEChar[0];
                FinalCmd[1] = FrameFormat[0];
                FinalCmd[2] = _HDLCLength[0];
                objDataReq.OBISFixedBytes.CopyTo(FinalCmd, 3);

                int len = protocolFlags.StartFlag.Length + securityKey.HDLCFrame_Format.Length +
                          objDataReq.HDLCLen.Length + objDataReq.OBISFixedBytes.Length;

                objDataReq.ReqBlock.CopyTo(FinalCmd, len);
                len += objDataReq.ReqBlock.Length;
                objDataReq.InvocationCounterBytes.CopyTo(FinalCmd, len);
                len += objDataReq.InvocationCounterBytes.Length;
                encryptData.CipheredData.CopyTo(FinalCmd, len);
                len += encryptData.CipheredData.Length;
                encryptData.AuthenticationTag.CopyTo(FinalCmd, len);
                len += encryptData.AuthenticationTag.Length;

                errorDetection.FCS = FCS.ComputeFCS(FinalCmd);
                errorDetection.FCS.CopyTo(FinalCmd, len);

                FinalCmd[FinalCmd.Length - 1] = SAEChar[0];


                string finalCommand = String.Concat(FinalCmd.Select(b => b.ToString("X2") + " "));
                this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\nRTC OBIS Code : " + finalCommand + Environment.NewLine)));

                MUTSerialCOM.Send(FinalCmd); Thread.Sleep(500);



            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }

        }
        private void ReadRTC()
        {
            RTCRead objDataReq = new RTCRead();
            SecurityKeys securityKey = new SecurityKeys();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();
            try
            {
                byte[] bInstantData;
                byte[] FinalCmd;
                Buffer.BlockCopy(objDataReq.PlainText, 0, objDataReq.OBISPlainText, 0, 11);
                objDataReq.OBISPlainText[11] = 0x02;
                objDataReq.OBISPlainText[12] = 0x00;
                Buffer.BlockCopy(objDataReq.InvocationCounter, 0, objDataReq.InvocationCounterBytes, 0, 3);
                objDataReq.InvocationCounterBytes[3] = 0x01;


                //  byte[] FinalCmd; // 
                try
                {

                    byte[] _FinalNonce = FrameNonce(securityKey.CNonce, objDataReq.InvocationCounterBytes, securityKey.CNonce.Length);
                    byte[] _AAD = FrameAAD(securityKey.PasswordKey);
                    _CipheredData = new byte[13];

                    byte[] bResult = AESGCM.SimpleEncrypt(objDataReq.OBISPlainText, DedicatedKey, _FinalNonce, _AAD);

                    string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));
                    this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\nDATA REQUEST CIPHER : " + ss + Environment.NewLine)));
                    Buffer.BlockCopy(bResult, 0, _CipheredData, 0, 13);
                    Buffer.BlockCopy(bResult, 13, _AuthenticationTag, 0, 12);

                    //---------------- Frame the Data Request 2(RTC DATA) Send Request ----------------------------// 


                    byte[] SAEChar = protocolFlags.StartFlag;
                    byte[] FrameFormat = securityKey.HDLCFrame_Format;
                    byte[] _HDLCLength = objDataReq.HDLCLen;
                    encryptData.CipheredData = _CipheredData;
                    encryptData.AuthenticationTag = _AuthenticationTag;

                    //---------------Final RTC DATA Command ------------------//

                    FinalCmd = new byte[_HDLCLength[0] + 2];
                    FinalCmd[0] = SAEChar[0];
                    FinalCmd[1] = FrameFormat[0];
                    FinalCmd[2] = _HDLCLength[0];
                    objDataReq.RTCFixedBytes.CopyTo(FinalCmd, 3);

                    int len = protocolFlags.StartFlag.Length + securityKey.HDLCFrame_Format.Length +
                              objDataReq.HDLCLen.Length + objDataReq.RTCFixedBytes.Length;

                    objDataReq.ReqBlock.CopyTo(FinalCmd, len);
                    len += objDataReq.ReqBlock.Length;
                    objDataReq.InvocationCounterBytes.CopyTo(FinalCmd, len);
                    len += objDataReq.InvocationCounterBytes.Length;
                    encryptData.CipheredData.CopyTo(FinalCmd, len);
                    len += encryptData.CipheredData.Length;
                    encryptData.AuthenticationTag.CopyTo(FinalCmd, len);
                    len += encryptData.AuthenticationTag.Length;

                    errorDetection.FCS = FCS.ComputeFCS(FinalCmd);
                    errorDetection.FCS.CopyTo(FinalCmd, len);

                    FinalCmd[FinalCmd.Length - 1] = SAEChar[0];

                    string finalCommand = String.Concat(FinalCmd.Select(b => b.ToString("X2") + " "));
                    this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\n RTC Request : " + finalCommand + Environment.NewLine)));
                    MUTSerialCOM.Send(FinalCmd); Thread.Sleep(500);


                    if (ReceiveStr[0] != 0)
                    {
                        // byte 12 (From 0 ) - Length
                        // byte 14,15,16,17  - FrameCount
                        // From Byte 18      - Data Started (Length - 6)
                        // Server Nonce

                        //  byte[] _SDecNonce = Encoding.UTF8.GetBytes("AVG12345");

                        byte[] _SDecNonce = ServerNonce; //new byte[] { 0x41, 0x56, 0x47, 0x31, 0x32, 0x33, 0x32, 0x33 }; // (Server System Title from AARE )                   
                        byte[] _SDecsubNonce_FrameCount = new byte[] { 0x00, 0x00, 0x00, 0x03 };

                        byte[] _SFinalNonce = FrameNonce(_SDecNonce, _SDecsubNonce_FrameCount, _SDecNonce.Length);



                        int iLen = Convert.ToInt32(ReceiveStr[12].ToString("X2"), 16);  //13
                        byte[] _bFrameCount = new byte[4]; bInstantData = new byte[(iLen - 5) + 12];

                        byte[] _bAuthenticationTag = new byte[12];
                        Buffer.BlockCopy(ReceiveStr, 14, _bFrameCount, 0, 4);   //15
                        Buffer.BlockCopy(ReceiveStr, 18, bInstantData, 0, iLen - 17);    //19  , iLen - 5
                        Buffer.BlockCopy(ReceiveStr, 36, _bAuthenticationTag, 0, 12);

                        string sBeforeInstantCrypt = String.Concat(bInstantData.Select(b => b.ToString("X2") + " "));

                        byte[] bDecryptResult = AESGCM.SimpleDecrypt(bInstantData, DedicatedKey, _SFinalNonce);

                        string sCipherResults = String.Concat(bDecryptResult.Select(b => b.ToString("X2") + " "));

                        this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "RTC DECODED : " + sCipherResults + Environment.NewLine)));

                        //isWriteProcessStatus = bDecryptResult[3] == 00 ? true : false;
                        //this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "WriteStatus : " + isWriteProcessStatus.ToString() + Environment.NewLine)));


                        int Year = Convert.ToInt32(Convert.ToInt16(Convert.ToInt32(bDecryptResult[6] << 8 | bDecryptResult[7]).ToString("X"), 16));

                        string Month = Convert.ToString(Convert.ToInt16(Convert.ToInt32(bDecryptResult[8]).ToString("X"), 16));
                        string _Month = Convert.ToString(Month.PadLeft(2, '0'));

                        int Date = Convert.ToInt32(Convert.ToInt16(Convert.ToInt32(bDecryptResult[9]).ToString("X"), 16));
                        int _day = Convert.ToInt16(Convert.ToInt32(bDecryptResult[10]).ToString("X"), 16);

                        string Day = DayOfWeek(_day);

                        string Hour = Convert.ToString(Convert.ToInt16(Convert.ToInt32(bDecryptResult[11]).ToString("X"), 16));
                        string _Hour = Convert.ToString(Hour.PadLeft(2, '0'));

                        string Minute = Convert.ToString(Convert.ToInt16(Convert.ToInt32(bDecryptResult[12]).ToString("X"), 16));
                        string _Minute = Convert.ToString(Minute.PadLeft(2, '0'));

                        string Seconds = Convert.ToString(Convert.ToInt16(Convert.ToInt32(bDecryptResult[13]).ToString("X"), 16));
                        string _Seconds = Convert.ToString(Seconds.PadLeft(2, '0'));

                        string HOSecond = Convert.ToString(Convert.ToInt16(Convert.ToInt32(bDecryptResult[14]).ToString("X"), 16));
                        string UTC = Convert.ToString(Convert.ToInt16(Convert.ToInt32(bDecryptResult[15] << 8 | bDecryptResult[16]).ToString("X"), 16));
                        string Status = Convert.ToString(Convert.ToInt16(Convert.ToInt32(bDecryptResult[17]).ToString("X"), 16));

                        this.txtRTCRead.Invoke((MethodInvoker)(() => txtRTCRead.Text = Date + "-" + _Month + "-" + Year + " " + Day + " " + Hour + ":" + _Minute + ":" + _Seconds));


                    //    isWriteProcessStatus = bDecryptResult[3] == 00 ? true : false;

                       



                    }
                }
                catch (Exception ex)
                {

                }


            }
            catch { };
        }


        //--------------------------------------- Functional Test --------------------------

        private void Fun1()
        {
            byte[] test1 = new byte[2] { 0x1B, 0x59 };
            FunTest(test1);
        }
        private void FunTest(byte[] value)
        {
            byte[] bInstantData;
            SecurityKeys securityKeys = new SecurityKeys();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();


            try
            {

                byte[] FPlainText = new byte[] { 0xC1, 0x01, 0xC1, 0x00, 0x01, 0x01, 0x00, 0x60, 0x02, 0x80, 0x00, 0x02, 0x00, 0x12, 0x1B, 0x59 };
                byte[] InvocationCounter = new byte[4] { 0x00, 0x00, 0x00, 0x00 };
                byte[] FinalCmd; // 
                try
                {

                    byte[] _FinalNonce = FrameNonce(securityKeys.CNonce, InvocationCounter, securityKeys.CNonce.Length);
                    byte[] _AAD = FrameAAD(securityKeys.PasswordKey);
                    _CipheredData = new byte[16];

                    byte[] bResult = AESGCM.SimpleEncrypt(FPlainText, DedicatedKey, _FinalNonce, _AAD);

                    string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));                   
                    this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\n LCD Ciphered : " + ss + Environment.NewLine)));
                    Buffer.BlockCopy(bResult, 0, _CipheredData, 0, 16);
                    Buffer.BlockCopy(bResult, 16, _AuthenticationTag, 0, 12);


                    byte[] SAEChar = protocolFlags.StartFlag;
                    byte[] FrameFormat = securityKeys.HDLCFrame_Format;
                    byte[] _HDLCLength = { 0x2F };
                    byte[] FixedBytes = new byte[8] { 0x03, 0x61, 0x54, 0x51, 0x9F, 0xE6, 0xE6, 0x00 };
                    byte[] ReqBlock = new byte[3] { 0xD1, 0x21, 0x30 };
                    encryptData.CipheredData = _CipheredData;
                    encryptData.AuthenticationTag = _AuthenticationTag;

                    //---------------Final RTC DATA Command ------------------//

                    FinalCmd = new byte[_HDLCLength[0] + 2];
                    FinalCmd[0] = SAEChar[0];
                    FinalCmd[1] = FrameFormat[0];
                    FinalCmd[2] = _HDLCLength[0];
                    FixedBytes.CopyTo(FinalCmd, 3);

                    int len = protocolFlags.StartFlag.Length + securityKeys.HDLCFrame_Format.Length +
                              1 + FixedBytes.Length;

                    ReqBlock.CopyTo(FinalCmd, len);
                    len += ReqBlock.Length;
                    InvocationCounter.CopyTo(FinalCmd, len);
                    len += InvocationCounter.Length;
                    encryptData.CipheredData.CopyTo(FinalCmd, len);
                    len += encryptData.CipheredData.Length;
                    encryptData.AuthenticationTag.CopyTo(FinalCmd, len);
                    len += encryptData.AuthenticationTag.Length;

                    errorDetection.FCS = FCS.ComputeFCS(FinalCmd);
                    errorDetection.FCS.CopyTo(FinalCmd, len);

                    FinalCmd[FinalCmd.Length - 1] = SAEChar[0];

                    string finalCommand = String.Concat(FinalCmd.Select(b => b.ToString("X2") + " "));
                    //   this.RTCBox.AppendText(Environment.NewLine + "\n RAM Clear Request : " + finalCommand + Environment.NewLine);
                    this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\n LCD Test Request : " + finalCommand + Environment.NewLine)));
                    MUTSerialCOM.Send(FinalCmd); Thread.Sleep(500);


                    if (ReceiveStr[0] != 0)
                    {
                        // byte 12 (From 0 ) - Length
                        // byte 14,15,16,17  - FrameCount
                        // From Byte 18      - Data Started (Length - 6)
                        // Server Nonce

                        //  byte[] _SDecNonce = Encoding.UTF8.GetBytes("AVG12345");

                        byte[] _SDecNonce = ServerNonce; //new byte[] { 0x41, 0x56, 0x47, 0x31, 0x32, 0x33, 0x32, 0x33 }; // (Server System Title from AARE )                   
                        byte[] _SDecsubNonce_FrameCount = new byte[] { 0x00, 0x00, 0x00, 0x02 };

                        byte[] _SFinalNonce = FrameNonce(_SDecNonce, _SDecsubNonce_FrameCount, _SDecNonce.Length);



                        int iLen = Convert.ToInt32(ReceiveStr[12].ToString("X2"), 16);  //13
                        byte[] _bFrameCount = new byte[4]; bInstantData = new byte[(iLen - 5) + 12];

                        byte[] _bAuthenticationTag = new byte[12];
                        Buffer.BlockCopy(ReceiveStr, 14, _bFrameCount, 0, 4);   //15
                        Buffer.BlockCopy(ReceiveStr, 18, bInstantData, 0, iLen - 17);    //19  , iLen - 5
                        Buffer.BlockCopy(ReceiveStr, 22, _bAuthenticationTag, 0, 12);

                        string sBeforeInstantCrypt = String.Concat(bInstantData.Select(b => b.ToString("X2") + " "));

                        byte[] bDecryptResult = AESGCM.SimpleDecrypt(bInstantData, DedicatedKey, _SFinalNonce);

                        string sCipherResults = String.Concat(bDecryptResult.Select(b => b.ToString("X2") + " "));

                        //  this.RTCBox.AppendText(Environment.NewLine + "RAM Clear Write DECODED : " + sCipherResults + Environment.NewLine);
                        this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "LCD Test DECODED : " + sCipherResults + Environment.NewLine)));

                       bool isWriteProcessStatus = bDecryptResult[3] == 00 ? true : false;

                        if (isWriteProcessStatus == true)
                        {
                            MessageBox.Show("Test OK");
                        }

                        this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "WriteStatus : " + isWriteProcessStatus.ToString() + Environment.NewLine)));


                    }
                }
                catch (Exception ex) { }
            }
            catch (Exception ex) { }

        }


        //----------------------------------------------------------------------
        static string ConvertBytesToAscii(byte[] bytes)
        {
            Encoding asciiEncoding = Encoding.ASCII;
            string asciiString = asciiEncoding.GetString(bytes);
            return asciiString;
        }

        private byte[] FrameNonce(byte[] Nonce, byte[] I_Counter, int NonceLen)
        {
            Buffer.BlockCopy(Nonce, 0, FinalNonce, 0, NonceLen);
            Buffer.BlockCopy(I_Counter, 0, FinalNonce, 8, I_Counter.Length);
            return FinalNonce;
        }
        private byte[] FrameAAD(byte[] PasswordKey)
        {
            AssociationKey[0] = 0x30;
            Buffer.BlockCopy(PasswordKey, 0, AssociationKey, 1, PasswordKey.Length);
            return AssociationKey;
        }
        private string DayOfWeek(int _day)
        {
            string Day = string.Empty;
            switch (_day)
            {
                case 1:

                    Day = "Monday";
                    break;

                case 2:

                    Day = "Tuesday";
                    break;

                case 3:

                    Day = "Wednesday";
                    break;

                case 4:

                    Day = "Thursday";
                    break;

                case 5:

                    Day = "Friday";
                    break;

                case 6:

                    Day = "Saturday";
                    break;

                case 7:

                    Day = "Sunday";
                    break;
                default:
                    Day = "Error";
                    break;
            }
            return Day;
        }

        //------------------------------------------------------------------------
        private void LCDTest()
        {
            try
            {
                int waitSecond = 100;

                byte[] DisconnectCmd = comm.DisconnectCommand();
                //MUTSerialCOM.Send(DisconnectCmd);
                //Thread.Sleep(waitSecond);

                SNRMCmd();
                Thread.Sleep(650);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "SNRM"));

                AARQCommand();
                Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "AARQ"));

                ActionRequestCmd();
                Thread.Sleep(650);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "ACTION"));

                Fun1();
                Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "LED Test"));

                DisconnectCmd = comm.DisconnectCommand();
                string dd = String.Concat(DisconnectCmd.Select(b => b.ToString("X2") + " "));
                this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText("\n" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss ") + "  Disconnect Request : " + dd)));
                MUTSerialCOM.Send(DisconnectCmd);
                Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "DISCONNECT"));

              
            }
            catch { }


        }
        private void btnTestLCD_Click(object sender, EventArgs e)
        {
            bwFunctionTest.RunWorkerAsync();
        }

        private void bwFunctionTest_DoWork(object sender, DoWorkEventArgs e)
        {
            LCDTest();
            
        }

        //-------------------------------------- Logical Name Read ---------------------

        private void LogicalName(byte[] clsId, byte[] obisCode)
        {
            FunctionalTestLogicalName functionalTest = new FunctionalTestLogicalName();
            SecurityKeys securityKey = new SecurityKeys();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();
            try
            {
                byte[] FinalCmd;
                Buffer.BlockCopy(clsId, 0, functionalTest.PlainText, 3, 2);
                Buffer.BlockCopy(obisCode, 0, functionalTest.PlainText, 5, 6);                                            

                byte[] _FinalNonce = FrameNonce(securityKey.CNonce, functionalTest.InvocationCounter, securityKey.CNonce.Length);
                byte[] _AAD = FrameAAD(securityKey.PasswordKey);
                _CipheredData = new byte[13];

                byte[] bResult = AESGCM.SimpleEncrypt(functionalTest.PlainText, DedicatedKey, _FinalNonce, _AAD);

                string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));
                this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\nDATA REQUEST CIPHER : " + ss + Environment.NewLine)));
                Buffer.BlockCopy(bResult, 0, _CipheredData, 0, 13);
                Buffer.BlockCopy(bResult, 13, _AuthenticationTag, 0, 12);

                //---------------- Frame the Data Request 1(OBIS Code) Send Request ----------------------------// 


                byte[] SAEChar = protocolFlags.StartFlag;
                byte[] FrameFormat = securityKey.HDLCFrame_Format;
                byte[] _HDLCLength = functionalTest.HDLCLen;
                encryptData.CipheredData = _CipheredData;
                encryptData.AuthenticationTag = _AuthenticationTag;

                //---------------Final OBIS Code Command ------------------//

                FinalCmd = new byte[_HDLCLength[0] + 2];
                FinalCmd[0] = SAEChar[0];
                FinalCmd[1] = FrameFormat[0];
                FinalCmd[2] = _HDLCLength[0];
                functionalTest.FixedBytes.CopyTo(FinalCmd, 3);

                int len = protocolFlags.StartFlag.Length + securityKey.HDLCFrame_Format.Length +
                          functionalTest.HDLCLen.Length + functionalTest.FixedBytes.Length;

                functionalTest.Header.CopyTo(FinalCmd, len);
                len += functionalTest.Header.Length;
                functionalTest.InvocationCounter.CopyTo(FinalCmd, len);
                len += functionalTest.InvocationCounter.Length;
                encryptData.CipheredData.CopyTo(FinalCmd, len);
                len += encryptData.CipheredData.Length;
                encryptData.AuthenticationTag.CopyTo(FinalCmd, len);
                len += encryptData.AuthenticationTag.Length;

                errorDetection.FCS = FCS.ComputeFCS(FinalCmd);
                errorDetection.FCS.CopyTo(FinalCmd, len);

                FinalCmd[FinalCmd.Length - 1] = SAEChar[0];


                string finalCommand = String.Concat(FinalCmd.Select(b => b.ToString("X2") + " "));
                this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\nFunctional OBIS Code : " + finalCommand + Environment.NewLine)));

                MUTSerialCOM.Send(FinalCmd); Thread.Sleep(500);



            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }

        private void btnTestStatus_Click(object sender, EventArgs e)
        {
            bwReadTest.RunWorkerAsync();

        }
        private void ReadFunctionalTest()
        {
            FunctionalTestLogicalName functionalTest = new FunctionalTestLogicalName();
           
           
            try
            {
                int waitSecond = 100;

                byte[] DisconnectCmd = comm.DisconnectCommand();
                //MUTSerialCOM.Send(DisconnectCmd);
                //Thread.Sleep(waitSecond);

                SNRMCmd();
                Thread.Sleep(650);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "SNRM"));

                AARQCommand();
                Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "AARQ"));

                ActionRequestCmd();
                Thread.Sleep(650);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "ACTION"));

                // FunctionalTestLogicalName functionalTest = new FunctionalTestLogicalName();
                LogicalName(functionalTest.ClassId, functionalTest.OBISCode);
                // Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "RTC LOGICAL NAME"));

                GetFunctionalTest(functionalTest.ClassId, functionalTest.OBISCode);
                // Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "RTC READ"));

                DisconnectCmd = comm.DisconnectCommand();
                string dd = String.Concat(DisconnectCmd.Select(b => b.ToString("X2") + " "));
                this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText("\n" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss ") + "  Disconnect Request : " + dd)));
                MUTSerialCOM.Send(DisconnectCmd);
                Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "DISCONNECT"));


            }
            catch { }
        }


    private void GetFunctionalTest(byte[] clsId, byte[] obisCode)
        {
            FunctionalTestLogicalName functionalTest = new FunctionalTestLogicalName();
            ReadFunctionalTest readFunctional = new ReadFunctionalTest();
            SecurityKeys securityKey = new SecurityKeys();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();
            byte[] bInstantData;
            try
            {
                byte[] FinalCmd;
                Buffer.BlockCopy(clsId, 0, functionalTest.PlainText, 3, 2);
                Buffer.BlockCopy(obisCode, 0, functionalTest.PlainText, 5, 6);

                functionalTest.InvocationCounter[3] =0x01;
                functionalTest.PlainText[11] = 0x02;

                byte[] _FinalNonce = FrameNonce(securityKey.CNonce, functionalTest.InvocationCounter, securityKey.CNonce.Length);
                byte[] _AAD = FrameAAD(securityKey.PasswordKey);
                _CipheredData = new byte[13];

                byte[] bResult = AESGCM.SimpleEncrypt(functionalTest.PlainText, DedicatedKey, _FinalNonce, _AAD);

                string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));
                this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\nFunctional Test CIPHER : " + ss + Environment.NewLine)));
                Buffer.BlockCopy(bResult, 0, _CipheredData, 0, 13);
                Buffer.BlockCopy(bResult, 13, _AuthenticationTag, 0, 12);

                //---------------- Frame the Data Request 1(OBIS Code) Send Request ----------------------------// 


                byte[] SAEChar = protocolFlags.StartFlag;
                byte[] FrameFormat = securityKey.HDLCFrame_Format;
                byte[] _HDLCLength = functionalTest.HDLCLen;
                encryptData.CipheredData = _CipheredData;
                encryptData.AuthenticationTag = _AuthenticationTag;

                //---------------Final OBIS Code Command ------------------//

                FinalCmd = new byte[_HDLCLength[0] + 2];
                FinalCmd[0] = SAEChar[0];
                FinalCmd[1] = FrameFormat[0];
                FinalCmd[2] = _HDLCLength[0];
                readFunctional.ReadDataFixedBytes.CopyTo(FinalCmd, 3);

                int len = protocolFlags.StartFlag.Length + securityKey.HDLCFrame_Format.Length +
                          functionalTest.HDLCLen.Length + readFunctional.ReadDataFixedBytes.Length;

                functionalTest.Header.CopyTo(FinalCmd, len);
                len += functionalTest.Header.Length;
                functionalTest.InvocationCounter.CopyTo(FinalCmd, len);
                len += functionalTest.InvocationCounter.Length;
                encryptData.CipheredData.CopyTo(FinalCmd, len);
                len += encryptData.CipheredData.Length;
                encryptData.AuthenticationTag.CopyTo(FinalCmd, len);
                len += encryptData.AuthenticationTag.Length;

                errorDetection.FCS = FCS.ComputeFCS(FinalCmd);
                errorDetection.FCS.CopyTo(FinalCmd, len);

                FinalCmd[FinalCmd.Length - 1] = SAEChar[0];


                string finalCommand = String.Concat(FinalCmd.Select(b => b.ToString("X2") + " "));
                this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\nFunctional OBIS Code : " + finalCommand + Environment.NewLine)));

                MUTSerialCOM.Send(FinalCmd); Thread.Sleep(500);


                if (ReceiveStr[0] != 0)
                {
                    // byte 12 (From 0 ) - Length
                    // byte 14,15,16,17  - FrameCount
                    // From Byte 18      - Data Started (Length - 6)
                    // Server Nonce

                    //  byte[] _SDecNonce = Encoding.UTF8.GetBytes("AVG12345");

                    byte[] _SDecNonce = ServerNonce; //new byte[] { 0x41, 0x56, 0x47, 0x30, 0x30, 0x30, 0x30, 0x34 }; // (Server System Title from AARE )                   
                    byte[] _SDecsubNonce_FrameCount = new byte[] { 0x00, 0x00, 0x00, 0x03 };

                    byte[] _SFinalNonce = FrameNonce(_SDecNonce, _SDecsubNonce_FrameCount, _SDecNonce.Length);



                    int iLen = Convert.ToInt32(ReceiveStr[12].ToString("X2"), 16);  //13
                    byte[] _bFrameCount = new byte[4]; bInstantData = new byte[(iLen - 5) + 12];

                    byte[] _bAuthenticationTag = new byte[12];
                    Buffer.BlockCopy(ReceiveStr, 14, _bFrameCount, 0, 4);   //15
                    Buffer.BlockCopy(ReceiveStr, 18, bInstantData, 0, 14);    //19  , iLen - 5
                    Buffer.BlockCopy(ReceiveStr, 32, _bAuthenticationTag, 0, 12);

                    string sBeforeInstantCrypt = String.Concat(bInstantData.Select(b => b.ToString("X2") + " "));

                    byte[] bDecryptResult = AESGCM.SimpleDecrypt(bInstantData, DedicatedKey, _SFinalNonce);

                    string sCipherResults = String.Concat(bDecryptResult.Select(b => b.ToString("X2") + " "));

                    //this.RTCBox.AppendText(Environment.NewLine + "Serial No DECODED : " + sCipherResults + Environment.NewLine);
                    this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "Serial No DECODED : " + sCipherResults + Environment.NewLine)));

                    // Serial Number Byte to Ascii Conversion
                    byte[] ResultHex = new byte[4];
                    Buffer.BlockCopy(bDecryptResult, 5, ResultHex, 0, 4);
                    //  string asciiString = ConvertBytesToAscii(SNoHex);


                    string resultValue = string.Empty;
                    foreach (byte b in ResultHex)
                    {
                        string bitString = ConvertByteToBits(b);
                        resultValue = resultValue + bitString;                                                                        
                    }
                 
                    this.txtFlagStatus.Invoke((MethodInvoker)(() => txtFlagStatus.Text = resultValue));
                   
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }

        static string ConvertByteToBits(byte value)
        {
            return Convert.ToString(value, 2).PadLeft(8, '0');
        }

        private void bwReadTest_DoWork(object sender, DoWorkEventArgs e)
        {
            ReadFunctionalTest();
        }

        private void tabTesting_Click(object sender, EventArgs e)
        {

        }
    }
}
