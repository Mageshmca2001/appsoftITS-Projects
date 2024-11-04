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
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Keller.SPM.Communication.MUT;
using static Keller.SPM.Communication.MUT.SerialCOM;

namespace AVG.ProductionProcess.FunctionalTest.Forms
{
    public partial class FunctionalForm : Form
    {
        public string ConStr = "Data Source=" + Directory.GetCurrentDirectory() + ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString.ToString();

        #region Global Objects
        Properties.Settings settings = new Properties.Settings();
        public SerialCOM MUTSerialCOM = new SerialCOM();
        StatusClass statusClass = new StatusClass();
        StatusKeyClass statusKeyClass = new StatusKeyClass();
        HDLC objHDLC = new HDLC();
        ErrorDetection objerror = new ErrorDetection();
        ControlBytes objControlByte = new ControlBytes();
        RequestTag objTag = new RequestTag();
        ClassId objcls = new ClassId();
        OBISCode objOBIS = new OBISCode();
        CommonCommand comm = new CommonCommand();
        FunctionalTestLogicalName functionalTest = new FunctionalTestLogicalName();
        #endregion

        #region Global Variable
        public delegate void DelMethod(byte[] s);
        public string popUpTitle = "PCB Test";
        public byte[] ReceiveStr;
        public bool MainCmdSendFlag = false; public bool isWriteProcessStatus = false;
        public byte[] DisconnectCmd; byte[] Encrypt_StoC = new byte[16]; byte[] ActReqDecp = new byte[24];
        byte[] DedicatedKey = new byte[16]; byte[] invocationCounter = new byte[4]; byte[] FinalNonce = new byte[12];
        byte[] AssociationKey = new byte[17]; byte[] _CipheredData; byte[] _AuthenticationTag = new byte[12];
        public byte[] ServerNonce = new byte[8]; public byte[] bInstantData = new byte[170]; byte[] StoC = new byte[16];
        public int cmdSequence = 0; public byte[] ReadDataFixedBytes; public int seqIncDecCount = 4;
        public byte[] invocationCounterReset; UInt32 SetICContinously = 2; public int startCount = 0;
        public int UpdateRows; int SuccessRetVal = 0; int DeleteRow = 0; public byte[] CalibConstWrite = new byte[42]; public string ReadWriteEncode = string.Empty;
        public byte[] arraySequence = { 0x93, 0x10, 0x32, 0x54, 0x76, 0x98, 0xBA, 0xDC, 0xFE };
        public int ResetCount = 25; public double InstNeutralCurr; public double ScalarInstNeutralCurr;
        public double InstVolt; public double ScalarInstVolt; public string HexScalarInstVolt;
        public double InstCurr; public double ScalarInstCurr; public string HexScalarInstCurr;
        #endregion

        #region Global  CommandConstants
        public const int CMD_IDLE = 0;
        public const int CMD_SNRM = 1;
        public const int CMD_AARQ_PC = 2;
        public const int CMD_GETREQ_PC = 3;
        public const int CMD_READIC_PC = 4;
        public const int CMD_DISCONNECT = 6;

        public const int CMD_SNRM_US = 7;
        public const int CMD_AARQ_US = 8;
        public const int CMD_ACTREQ_US = 9;
        public const int CMD_GETREQ_LOGICNAME = 10;
        public const int CMD_GETREQ_SNO = 11;
        public const int CMD_GETREQ_RTC = 12;

        public const int CMD_SETREQ_SNO = 13;
        public const int CMD_SETREQ_RTC = 14;
        public const int CMD_SETREQ_RAMCLEAR = 15;

        public const int CMD_SETREQ_STARTMETER = 16;
        public const int CMD_SETREQ_STOPMETER = 17;
        public const int CMD_GETREQ_READMETERCONTIOUSLY = 18;
        public const int CMD_GET_INSTANTDATA = 19;
        public const int CMD_GETREQ_SCALARUNIT = 20;
        public const int CMD_GETREQ_INSTANTCURR = 21;
        public const int CMD_GETREQ_SCALARUNITCURR = 22;
        public const int CMD_GET_CALIBVALUE = 23;
        public const int CMD_DISCONNECT_US = 24;
        public const int CMD_GET_CALIBINST = 25;
        public const int CMD_SET_CALIBVALUE = 26;
        public const int CMD_GET_INSTNEUCURR = 27;
        public const int CMD_GETINSTNEUSCACURR = 28;
        public int CmdType = 0; public bool StopStatus = false;
        #endregion

        #region Global RequestConstants
        public int requestType = 0;
        public const int REQ_GET_IC_PC = 0;
        public const int REQ_GET_SNO_US = 1;
        public const int REQ_SET_SNO_US = 2;
        public const int REQ_SET_RAMCLEAR = 3;
        public const int REQ_SET_STARTMETERCMD = 4;
        public const int REQ_SET_STOPMETERCMD = 5;
        public const int REQ_GET_FUNCTIONLITYTEST = 6;
        public const int REQ_GET_INSTANTCURR = 9;
        public const int REQ_DEFAULTDISC = 8;
        public const int REQ_GET_INSTANTDATA = 7;
        public const int REQ_GET_CALIBREAD = 10;
        public const int REQ_SET_CALIBVALUE = 11;
        public const int REQ_GET_INSTANTVOLT = 12;
        public const int REQ_GET_INSTANTNEUTRALCURR = 13;
        //-------------InstantParameters---------------
        // public const int REQ_GET_INSTANTVOLT = 9;

        #endregion

        public FunctionalForm()
        {
            MUTSerialCOM.DataReceived += new dataReceived(MUTSerialCOM_DataReceived);
            InitializeComponent();
        }
        private void MUTSerialCOM_DataReceived(object sender, SerialPortEventArgs arg)
        {
            ReceiveStr = new byte[arg.ReceivedData.Length];
            ReceiveStr = arg.ReceivedData;

            this.BeginInvoke(new DelMethod(DataSegMethod), new object[] { ReceiveStr });
        }
        public void DataSegMethod(byte[] ReceiveStr)
        {
            try
            {
                bool RespStatus = false;

                SecurityKeys securityKey = new SecurityKeys();
                if (ReceiveStr[0] != 0)
                {
                    string TxtStr = string.Empty;
                    for (int i = 0; i < ReceiveStr[2] + 2; i++)
                    {
                        TxtStr = TxtStr + " " + ReceiveStr[i].ToString("X2");
                    }

                    UpdateLogStatus("Response Data : " + TxtStr + " \n");

                    //----------------------Modified by Vinishya(21-05-2024)-------------------------------

                    if (CmdType == CMD_SNRM || CmdType == CMD_SNRM_US)
                    {
                        if (ReceiveStr[5] == 115) { RespStatus = true; }
                        CheckNextCmdSNRM(RespStatus);
                    }
                    else if (CmdType == CMD_AARQ_PC || CmdType == CMD_AARQ_US)
                    {
                        if (ReceiveStr[28] == 0) { RespStatus = true; }
                        CheckNextCmdAARQ(RespStatus);
                    }
                    else if (CmdType == CMD_GETREQ_PC)
                    {
                        if (ReceiveStr[14] == 0) { RespStatus = true; }
                        CheckNextCmdGetReq(RespStatus);
                    }
                    else if (CmdType == CMD_READIC_PC)
                    {
                        if (ReceiveStr[14] == 0)
                        {
                            RespStatus = true;

                            if (ReceiveStr[0] != 0)
                            {
                                int length = ReceiveStr[2];
                                if (ReceiveStr[length + 1] == 0x7E)
                                {
                                    if (length + 2 == 0x17)
                                    {
                                        Buffer.BlockCopy(ReceiveStr, 16, invocationCounter, 0, invocationCounter.Length);
                                        string IC = String.Concat(invocationCounter.Select(b => b.ToString("X2") + " "));
                                        UpdateLogStatus(" Invocation Counter : " + IC); btnReadSno.Enabled = true;
                                        pcOpticalStatusGood.Visible = true; pcOpticalStatusGood.Enabled = true; //pcOpticalStatusGood.Refresh();
                                        StatusTrip.Text = "Invocation Counter Read";
                                    }
                                    else
                                    {
                                        pcOpticalStatusFail.Visible = false;
                                        // StatusUpdation.Text = "Something went wrong";
                                    }
                                }
                            }
                        }
                        CheckNextCmdReadInvocation(RespStatus);
                    }
                    else if (CmdType == CMD_DISCONNECT)
                    {
                        // if (requestType == REQ_GET_IC_PC) { if (ReceiveStr[5] == 115) { RespStatus = true; requestType = REQ_GET_INSTANTVOLT; btnReadSno.Enabled = true; } }
                        //if (requestType == REQ_GET_INSTANTVOLT)
                        //{
                        //    MainCmdSendFlag = false;
                        //    statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                        //    if (ReceiveStr[5] == 115) { RespStatus = true; CmdType = CMD_SNRM_US; MainCmdSendFlag = true; }
                        //}


                        if (requestType == REQ_GET_IC_PC) { if (ReceiveStr[5] == 115) { RespStatus = true; requestType = REQ_GET_CALIBREAD; btnReadSno.Enabled = true; } }


                        if (requestType == REQ_GET_CALIBREAD)
                        {
                            MainCmdSendFlag = false;
                            statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                            if (ReceiveStr[5] == 115) { RespStatus = true; CmdType = CMD_SNRM_US; MainCmdSendFlag = true; }
                        }

                        else if (requestType == REQ_GET_SNO_US)
                        {
                            MainCmdSendFlag = false;
                            statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                            CmdType = CMD_SNRM_US;
                            MainCmdSendFlag = true;
                        }
                        else if (requestType == REQ_DEFAULTDISC)
                        {
                            if (ReceiveStr[5] == 115) { RespStatus = true; }
                        }
                        else if (requestType == REQ_SET_STOPMETERCMD)
                        {
                            MainCmdSendFlag = false;
                            statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                            requestType = REQ_SET_CALIBVALUE; CmdType = CMD_SNRM_US; MainCmdSendFlag = true;

                            if (StopStatus == true)
                            {
                                statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                                { requestType = REQ_SET_CALIBVALUE; CmdType = CMD_SNRM_US; MainCmdSendFlag = true; }
                            }
                        }
                    }
                    else if (CmdType == CMD_GET_CALIBVALUE)
                    {
                        if (ReceiveStr[0] != 0)
                        {
                            byte[] _SDecNonce = ServerNonce; //new byte[] { 0x41, 0x56, 0x47, 0x31, 0x32, 0x33, 0x32, 0x33 }; // (Server System Title from AARE )                   
                            byte[] _SDecsubNonce_FrameCount = new byte[4]; // { 0x00, 0x00, 0x00, 0x02 };
                            Buffer.BlockCopy(ReceiveStr, 14, _SDecsubNonce_FrameCount, 0, 4);

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

                            UpdateLogStatus("Calib Read value DECODED : " + sCipherResults + Environment.NewLine);

                            byte[] calibConst = { 0xC1, 0x01, 0xC1, 0x00, 0x01, 0x01, 0x00, 0x60, 0x02, 0x81, 0x00, 0x02, 0x00, 0x02, 0x07 };
                            Buffer.BlockCopy(calibConst, 0, CalibConstWrite, 0, calibConst.Length);
                            Buffer.BlockCopy(bDecryptResult, 6, CalibConstWrite, 15, 27);

                            if (bDecryptResult[3] == 0) { RespStatus = true; }
                            CheckNextCmdSetCalibNxtLvl(RespStatus);

                        }
                    }
                    else if (CmdType == CMD_GET_CALIBINST)
                    {
                        if (ReceiveStr[0] != 0)
                        {
                            byte[] _SDecNonce = ServerNonce; //new byte[] { 0x41, 0x56, 0x47, 0x31, 0x32, 0x33, 0x32, 0x33 }; // (Server System Title from AARE )                   
                            byte[] _SDecsubNonce_FrameCount = new byte[4]; // { 0x00, 0x00, 0x00, 0x02 };
                            Buffer.BlockCopy(ReceiveStr, 14, _SDecsubNonce_FrameCount, 0, 4);

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

                            UpdateLogStatus("Calib Read value DECODED : " + sCipherResults + Environment.NewLine);

                            byte[] calibConst = { 0xC1, 0x01, 0xC1, 0x00, 0x01, 0x01, 0x00, 0x60, 0x02, 0x81, 0x00, 0x02, 0x00, 0x02, 0x07 };
                            Buffer.BlockCopy(calibConst, 0, CalibConstWrite, 0, calibConst.Length);
                            Buffer.BlockCopy(bDecryptResult, 6, CalibConstWrite, 15, 27);

                            CalibConstWrite[41] = 0x03;
                            if (bDecryptResult[3] == 0) { RespStatus = true; }
                            CheckNextCmdSetCalibNxtLvl(RespStatus);
                        }
                    }
                    else if (CmdType == CMD_SET_CALIBVALUE)
                    {
                        if (ReceiveStr[0] != 0)
                        {
                            byte[] _SDecNonce = ServerNonce; //new byte[] { 0x41, 0x56, 0x47, 0x31, 0x32, 0x33, 0x32, 0x33 }; // (Server System Title from AARE )                   
                            byte[] _SDecsubNonce_FrameCount = new byte[4]; // { 0x00, 0x00, 0x00, 0x00 };
                            Buffer.BlockCopy(ReceiveStr, 14, _SDecsubNonce_FrameCount, 0, _SDecsubNonce_FrameCount.Length);

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

                            UpdateLogStatus("Calib Const Write DECODED : " + sCipherResults);
                            //  this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "RAM Clear Write DECODED : " + sCipherResults + Environment.NewLine)));

                            isWriteProcessStatus = bDecryptResult[3] == 00 ? true : false;
                            if (isWriteProcessStatus == true)
                            {
                                //    this.btnRAMClear.Invoke((MethodInvoker)(() => btnRAMClear.Visible = false));

                                //    this.btnSetRTC.Invoke((MethodInvoker)(() => btnComplete.Visible = true));
                                //requestType = REQ_SET_RAMCLEAR; //lblStatus.Text = "RAM Clear Inprogress...";

                                CheckNextCmdDisconnect(isWriteProcessStatus); //DiscResetCnt = 1;
                            }

                            this.LogBox.Invoke((MethodInvoker)(() => LogBox.AppendText(Environment.NewLine + "WriteStatus : " + isWriteProcessStatus.ToString() + Environment.NewLine)));


                        }
                    }
                    else if (CmdType == CMD_ACTREQ_US)
                    {
                        if (ReceiveStr[0] == 126)
                        {
                            byte[] _SDecNonce = ServerNonce;
                            byte[] _SDecsubNonce_FrameCount = new byte[4];
                            Buffer.BlockCopy(ReceiveStr, 14, _SDecsubNonce_FrameCount, 0, 4);
                            byte[] _SFinalNonce = FrameNonce(_SDecNonce, _SDecsubNonce_FrameCount, _SDecNonce.Length);

                            byte[] _bFrameCount = new byte[4]; bInstantData = new byte[24];
                            byte[] _bAuthenticationTag = new byte[12];

                            Buffer.BlockCopy(ReceiveStr, 14, _bFrameCount, 0, 4);
                            Buffer.BlockCopy(ReceiveStr, 18, ActReqDecp, 0, 24);

                            // string ReceivedData = String.Concat(ReceiveStr.Select(b => b.ToString("X2") + " "));

                            string sBeforeInstantCrypt = String.Concat(ActReqDecp.Select(b => b.ToString("X2") + " "));

                            byte[] bDecryptResult = AESGCM.SimpleDecrypt(ActReqDecp, securityKey.PasswordKey, _SFinalNonce);

                            string sCipherResults = String.Concat(bDecryptResult.Select(b => b.ToString("X2") + " "));
                            if (bDecryptResult[3] == 0) { RespStatus = true; }
                            CheckNextCmdActionResponse(RespStatus);
                        }
                    }
                    else if (CmdType == CMD_GETREQ_LOGICNAME)
                    {
                        if (ReceiveStr[0] != 0)
                        {
                            byte[] _SDecNonce = ServerNonce;
                            byte[] _SDecsubNonce_FrameCount = new byte[4];
                            Buffer.BlockCopy(ReceiveStr, 14, _SDecsubNonce_FrameCount, 0, 4);

                            byte[] _SFinalNonce = FrameNonce(_SDecNonce, _SDecsubNonce_FrameCount, _SDecNonce.Length);

                            int iLen = Convert.ToInt32(ReceiveStr[12].ToString("X2"), 16);

                            byte[] _bFrameCount = new byte[4]; bInstantData = new byte[(iLen - 5) + 12];

                            byte[] _bAuthenticationTag = new byte[12];
                            Buffer.BlockCopy(ReceiveStr, 14, _bFrameCount, 0, 4);
                            Buffer.BlockCopy(ReceiveStr, 18, bInstantData, 0, iLen - 5);
                            Buffer.BlockCopy(ReceiveStr, 30, _bAuthenticationTag, 0, 12);

                            string sBeforeInstantCrypt = String.Concat(bInstantData.Select(b => b.ToString("X2") + " "));
                            byte[] bDecryptResult = AESGCM.SimpleDecrypt(bInstantData, DedicatedKey, _SFinalNonce);
                            string sCipherResults = String.Concat(bDecryptResult.Select(b => b.ToString("X2") + " "));
                            if (bDecryptResult[3] == 0) { RespStatus = true; }
                            CheckNextCmdGetReqLogicName(RespStatus);

                            // this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "Serial No OBIS CODE DECODED : " + sCipherResults + Environment.NewLine)));
                        }
                    }
                    else if (CmdType == CMD_GETREQ_SNO)
                    {
                        if (ReceiveStr[0] != 0)
                        {
                            byte[] _SDecNonce = ServerNonce;
                            byte[] _SDecsubNonce_FrameCount = new byte[4];
                            Buffer.BlockCopy(ReceiveStr, 14, _SDecsubNonce_FrameCount, 0, 4);

                            byte[] _SFinalNonce = FrameNonce(_SDecNonce, _SDecsubNonce_FrameCount, _SDecNonce.Length);

                            int iLen = Convert.ToInt32(ReceiveStr[12].ToString("X2"), 16);  //13
                            byte[] _bFrameCount = new byte[4];
                            bInstantData = new byte[(iLen - 5) + 12];

                            byte[] _bAuthenticationTag = new byte[12];
                            Buffer.BlockCopy(ReceiveStr, 14, _bFrameCount, 0, 4);   //15
                            Buffer.BlockCopy(ReceiveStr, 18, bInstantData, 0, 19);    //19  , iLen - 5
                            Buffer.BlockCopy(ReceiveStr, 32, _bAuthenticationTag, 0, 12);

                            string sBeforeInstantCrypt = String.Concat(bInstantData.Select(b => b.ToString("X2") + " "));

                            byte[] bDecryptResult = AESGCM.SimpleDecrypt(bInstantData, DedicatedKey, _SFinalNonce);

                            string sCipherResults = String.Concat(bDecryptResult.Select(b => b.ToString("X2") + " "));

                            //this.RTCBox.AppendText(Environment.NewLine + "Serial No DECODED : " + sCipherResults + Environment.NewLine);
                            // this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "Serial No DECODED : " + sCipherResults + Environment.NewLine)));
                            UpdateLogStatus("Serial No DECODED : " + sCipherResults + Environment.NewLine);
                            //if (bDecryptResult[3] == 0) { RespStatus = true; }


                            // Serial Number Byte to Ascii Conversion
                            byte[] SNoHex = new byte[13];
                            Buffer.BlockCopy(bDecryptResult, 6, SNoHex, 0, SNoHex.Length);
                            string asciiString = ConvertBytesToAscii(SNoHex);

                            // this.txtSerialNo.Invoke((MethodInvoker)(() => txtSerialNo.Text = asciiString));
                            this.lblSerialNumberRead.Invoke((MethodInvoker)(() => lblSerialNumberRead.Text = asciiString));
                            lblSerialNumberRead.ForeColor = Color.Green; RespStatus = true; StatusTrip.Text = "Serial Number Read";
                            CheckNextCmdGetReqSNo(RespStatus);
                        }
                    }
                    else if (CmdType == CMD_GETREQ_RTC)
                    {
                        if (ReceiveStr[0] != 0)
                        {
                            byte[] _SDecNonce = ServerNonce; //new byte[] { 0x41, 0x56, 0x47, 0x31, 0x32, 0x33, 0x32, 0x33 }; // (Server System Title from AARE )                   
                            byte[] _SDecsubNonce_FrameCount = new byte[4]; // { 0x00, 0x00, 0x00, 0x02 };
                            Buffer.BlockCopy(ReceiveStr, 14, _SDecsubNonce_FrameCount, 0, 4);

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

                            //this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "RTC DECODED : " + sCipherResults + Environment.NewLine)));
                            UpdateLogStatus("RTC DECODED : " + sCipherResults + Environment.NewLine);
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
                            //lblRTCRead.Text = Day + " " + _Hour + "-" + _Minute + "-" + _Seconds; RespStatus = true;
                            this.lblRTCRead.Invoke((MethodInvoker)(() => lblRTCRead.Text = Date + "-" + _Month + "-" + Year + " " + Day + " " + Hour + ":" + _Minute + ":" + _Seconds));
                            StatusTrip.Text = "RTC Read Successfully"; lblRTCRead.ForeColor = Color.Green; btnStart.Enabled = true; btnStop.Enabled = true;
                            SetToReadFunctionalTest(); //CheckNextCmdDisconnect(true);
                        }
                    }
                    else if (CmdType == CMD_SETREQ_SNO)
                    {
                        if (ReceiveStr[0] != 0)
                        {
                            byte[] _SDecNonce = ServerNonce; //new byte[] { 0x41, 0x56, 0x47, 0x31, 0x32, 0x33, 0x32, 0x33 }; // (Server System Title from AARE )                   
                            byte[] _SDecsubNonce_FrameCount = new byte[4]; // { 0x00, 0x00, 0x00, 0x00 };
                            Buffer.BlockCopy(ReceiveStr, 14, _SDecsubNonce_FrameCount, 0, _SDecsubNonce_FrameCount.Length);

                            string serverNonceData = String.Concat(ServerNonce.Select(b => b.ToString("X2") + " "));
                            // this.LogBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "Server Nonce : " + serverNonceData + Environment.NewLine)));
                            UpdateLogStatus("Server Nonce : " + serverNonceData + Environment.NewLine);
                            string assignedserverNonceData = String.Concat(_SDecNonce.Select(b => b.ToString("X2") + " "));
                            //this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "Assigned Server Nonce : " + assignedserverNonceData + Environment.NewLine)));
                            UpdateLogStatus("Assigned Server Nonce : " + assignedserverNonceData + Environment.NewLine);

                            byte[] _SFinalNonce = FrameNonce(_SDecNonce, _SDecsubNonce_FrameCount, _SDecNonce.Length);

                            string fiinalNonceData = String.Concat(_SFinalNonce.Select(b => b.ToString("X2") + " "));

                            UpdateLogStatus("Final Nonce : " + fiinalNonceData);
                            // this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "Final Nonce : " + fiinalNonceData + Environment.NewLine)));

                            int iLen = Convert.ToInt32(ReceiveStr[12].ToString("X2"), 16);  //13
                            byte[] _bFrameCount = new byte[4];
                            bInstantData = new byte[(iLen - 5) + 12];

                            byte[] _bAuthenticationTag = new byte[12];
                            Buffer.BlockCopy(ReceiveStr, 14, _bFrameCount, 0, 4);   //15
                            Buffer.BlockCopy(ReceiveStr, 18, bInstantData, 0, iLen - 17);    //19  , iLen - 5
                            Buffer.BlockCopy(ReceiveStr, 22, _bAuthenticationTag, 0, 12);

                            string sBeforeInstantCrypt = String.Concat(bInstantData.Select(b => b.ToString("X2") + " "));

                            byte[] bDecryptResult = AESGCM.SimpleDecrypt(bInstantData, DedicatedKey, _SFinalNonce);

                            string sCipherResults = String.Concat(bDecryptResult.Select(b => b.ToString("X2") + " "));

                            UpdateLogStatus("Serial Number Write DECODED : " + sCipherResults);


                            isWriteProcessStatus = bDecryptResult[3] == 00 ? true : false;

                            if (isWriteProcessStatus == true)
                            {
                                // this.btnSetRTC.Invoke((MethodInvoker)(() => btnSetRTC.Visible = true));
                                CheckNextCmdSetRTC(isWriteProcessStatus);
                            }
                            UpdateLogStatus("WriteStatus : " + isWriteProcessStatus.ToString());
                        }
                    }
                    else if (CmdType == CMD_SETREQ_RTC)
                    {
                        if (ReceiveStr[0] != 0)
                        {

                            byte[] _SDecNonce = ServerNonce; //new byte[] { 0x41, 0x56, 0x47, 0x31, 0x32, 0x33, 0x32, 0x33 }; // (Server System Title from AARE )                   
                            byte[] _SDecsubNonce_FrameCount = new byte[4]; // { 0x00, 0x00, 0x00, 0x00 };
                            Buffer.BlockCopy(ReceiveStr, 14, _SDecsubNonce_FrameCount, 0, _SDecsubNonce_FrameCount.Length);


                            string serverNonceData = String.Concat(ServerNonce.Select(b => b.ToString("X2") + " "));

                            UpdateLogStatus("Server Nonce : " + serverNonceData);
                            string assignedserverNonceData = String.Concat(_SDecNonce.Select(b => b.ToString("X2") + " "));
                            UpdateLogStatus("Assigned Server Nonce : " + assignedserverNonceData);

                            byte[] _SFinalNonce = FrameNonce(_SDecNonce, _SDecsubNonce_FrameCount, _SDecNonce.Length);
                            int iLen = Convert.ToInt32(ReceiveStr[12].ToString("X2"), 16);  //13
                            byte[] _bFrameCount = new byte[4];
                            bInstantData = new byte[(iLen - 5) + 12];

                            byte[] _bAuthenticationTag = new byte[12];
                            Buffer.BlockCopy(ReceiveStr, 14, _bFrameCount, 0, 4);   //15
                            Buffer.BlockCopy(ReceiveStr, 18, bInstantData, 0, iLen - 17);    //19  , iLen - 5
                            Buffer.BlockCopy(ReceiveStr, 22, _bAuthenticationTag, 0, 12);

                            string sBeforeInstantCrypt = String.Concat(bInstantData.Select(b => b.ToString("X2") + " "));

                            byte[] bDecryptResult = AESGCM.SimpleDecrypt(bInstantData, DedicatedKey, _SFinalNonce);

                            string sCipherResults = String.Concat(bDecryptResult.Select(b => b.ToString("X2") + " "));


                            UpdateLogStatus(" RTC Write DECODED : " + sCipherResults);
                            isWriteProcessStatus = bDecryptResult[3] == 00 ? true : false;
                            UpdateLogStatus("WriteStatus : " + isWriteProcessStatus.ToString());

                            if (isWriteProcessStatus == true)
                            {
                                //btnReadRTC.Invoke((MethodInvoker)(() => btnReadRTC.Visible = true));
                                // CheckNextCmdDisconnect(isWriteProcessStatus);
                                requestType = REQ_SET_RAMCLEAR;
                                CheckNextCmdRamClear(isWriteProcessStatus);
                            }
                            else
                            {
                                CheckNextCmdDisconnect(isWriteProcessStatus);
                            }
                        }
                    }
                    else if (CmdType == CMD_SETREQ_RAMCLEAR)
                    {
                        if (ReceiveStr[0] != 0)
                        {

                            byte[] _SDecNonce = ServerNonce; //new byte[] { 0x41, 0x56, 0x47, 0x31, 0x32, 0x33, 0x32, 0x33 }; // (Server System Title from AARE )                   
                            byte[] _SDecsubNonce_FrameCount = new byte[4]; // { 0x00, 0x00, 0x00, 0x00 };
                            Buffer.BlockCopy(ReceiveStr, 14, _SDecsubNonce_FrameCount, 0, _SDecsubNonce_FrameCount.Length);

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

                            UpdateLogStatus(" RAM Clear Write DECODED : " + sCipherResults);
                            //  this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "RAM Clear Write DECODED : " + sCipherResults + Environment.NewLine)));

                            isWriteProcessStatus = bDecryptResult[3] == 00 ? true : false;

                            if (isWriteProcessStatus == true)
                            {
                                //    this.btnRAMClear.Invoke((MethodInvoker)(() => btnRAMClear.Visible = false));
                                //    this.btnSetRTC.Invoke((MethodInvoker)(() => btnComplete.Visible = true));
                                CheckNextCmdDisconnect(isWriteProcessStatus);
                            }

                            this.LogBox.Invoke((MethodInvoker)(() => LogBox.AppendText(Environment.NewLine + "WriteStatus : " + isWriteProcessStatus.ToString() + Environment.NewLine)));


                        }
                    }
                    else if (CmdType == CMD_SETREQ_STARTMETER)
                    {
                        if (ReceiveStr[0] != 0)
                        {

                            byte[] _SDecNonce = ServerNonce; //new byte[] { 0x41, 0x56, 0x47, 0x31, 0x32, 0x33, 0x32, 0x33 }; // (Server System Title from AARE )                   
                            byte[] _SDecsubNonce_FrameCount = new byte[4]; // { 0x00, 0x00, 0x00, 0x02 };
                            Buffer.BlockCopy(ReceiveStr, 14, _SDecsubNonce_FrameCount, 0, 4);

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
                            this.LogBox.Invoke((MethodInvoker)(() => LogBox.AppendText(Environment.NewLine + "FUNCTIONAL TEST DECODED : " + sCipherResults + Environment.NewLine)));

                            bool isWriteProcessStatus = bDecryptResult[3] == 00 ? true : false;

                            if (isWriteProcessStatus == true)
                            {
                                invocationCounterReset = new byte[] { 0x00, 0x00, 0x00, 0x03 };
                                requestType = REQ_GET_FUNCTIONLITYTEST;
                                CheckNextCmdActionResponse(isWriteProcessStatus); btnStop.Enabled = true; btnTestStart.Enabled = false;
                                // CheckNextCmdDisconnect(isWriteProcessStatus);
                                // MessageBox.Show("Success...");
                            }
                            this.LogBox.Invoke((MethodInvoker)(() => LogBox.AppendText(Environment.NewLine + "WriteStatus : " + isWriteProcessStatus.ToString() + Environment.NewLine)));
                        }
                    }
                    else if (CmdType == CMD_DISCONNECT_US)
                    {
                        //  if (ReadRealRTC == 1) { lblSystemRTC.Text = DateTime.Now.ToString("dd-M-yyyy HH:mm:ss"); DiscResetCnt = 0; CompareRealRTCMeterRTC(); ReadRealRTC = 0; }
                        if (requestType == REQ_GET_CALIBREAD)
                        {
                            if (CalibConstWrite[41] == 1)
                            {
                                CalibConstWrite[41] = 3;
                                MainCmdSendFlag = false;
                                statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                                CmdType = CMD_SNRM_US;
                                MainCmdSendFlag = true;
                                RespStatus = true; requestType = REQ_GET_SNO_US; btnReadSno.Enabled = true;
                            }
                            else
                            {
                                MessageBox.Show("Meter not Eligible for this process...", "Smartmeter", MessageBoxButtons.OK, MessageBoxIcon.Warning); //btnStarttest.Enabled = true;
                                return;
                            }

                            // if (ReceiveStr[5] == 115) { } 
                        }

                    }
                    else if (CmdType == CMD_GETREQ_SCALARUNIT)
                    {
                        if (ReceiveStr[0] != 0)
                        {
                            byte[] _SDecNonce = ServerNonce;
                            byte[] _SDecsubNonce_FrameCount = new byte[4];
                            Buffer.BlockCopy(ReceiveStr, 14, _SDecsubNonce_FrameCount, 0, _SDecsubNonce_FrameCount.Length);

                            byte[] _SFinalNonce = FrameNonce(_SDecNonce, _SDecsubNonce_FrameCount, _SDecNonce.Length);

                            int iLen = Convert.ToInt32(ReceiveStr[12].ToString("X2"), 16);  //13
                            byte[] _bFrameCount = new byte[4]; bInstantData = new byte[(iLen - 5) + 12];

                            byte[] _bAuthenticationTag = new byte[12];
                            Buffer.BlockCopy(ReceiveStr, 14, _bFrameCount, 0, 4);   //15
                            Buffer.BlockCopy(ReceiveStr, 18, bInstantData, 0, 14);    //19  , iLen - 5
                            Buffer.BlockCopy(ReceiveStr, 32, _bAuthenticationTag, 0, 12);

                            string sBeforeInstantCrypt = String.Concat(bInstantData.Select(b => b.ToString("X2") + " "));

                            byte[] bDecryptResult = AESGCM.SimpleDecrypt(bInstantData, DedicatedKey, _SFinalNonce);
                            if (bDecryptResult[0] == 196)
                            {
                                string sCipherResults = String.Concat(bDecryptResult.Select(b => b.ToString("X2") + " "));
                                this.LogBox.Invoke((MethodInvoker)(() => LogBox.AppendText(Environment.NewLine + "Voltage Scalar Decrypt : " + sCipherResults + Environment.NewLine)));
                                ScalarInstVolt = Convert.ToDouble(bDecryptResult[7]);
                                //byte byteSclVol = Convert.ToByte(HexScalarInstVolt, 16);
                                sbyte signedValue = (sbyte)ScalarInstVolt;
                                double Val = Math.Pow(10, signedValue);
                                double Vol = Math.Round((InstVolt * Val), 2);
                                lblInstVolt.Text = Vol.ToString();
                                CheckNextCmdScalarInstVolt(true);
                            }
                        }
                    }
                    else if (CmdType == CMD_GETREQ_INSTANTCURR)
                    {
                        if (ReceiveStr[0] != 0)
                        {
                            byte[] _SDecNonce = ServerNonce;
                            byte[] _SDecsubNonce_FrameCount = new byte[4];
                            Buffer.BlockCopy(ReceiveStr, 14, _SDecsubNonce_FrameCount, 0, _SDecsubNonce_FrameCount.Length);

                            byte[] _SFinalNonce = FrameNonce(_SDecNonce, _SDecsubNonce_FrameCount, _SDecNonce.Length);

                            int iLen = Convert.ToInt32(ReceiveStr[12].ToString("X2"), 16);  //13
                            byte[] _bFrameCount = new byte[4]; bInstantData = new byte[(iLen - 5) + 12];

                            byte[] _bAuthenticationTag = new byte[12];
                            Buffer.BlockCopy(ReceiveStr, 14, _bFrameCount, 0, 4);   //15
                            Buffer.BlockCopy(ReceiveStr, 18, bInstantData, 0, 14);    //19  , iLen - 5
                            Buffer.BlockCopy(ReceiveStr, 32, _bAuthenticationTag, 0, 12);

                            string sBeforeInstantCrypt = String.Concat(bInstantData.Select(b => b.ToString("X2") + " "));

                            byte[] bDecryptResult = AESGCM.SimpleDecrypt(bInstantData, DedicatedKey, _SFinalNonce);
                            if (bDecryptResult[0] == 196)
                            {
                                string sCipherResults = String.Concat(bDecryptResult.Select(b => b.ToString("X2") + " "));
                                this.LogBox.Invoke((MethodInvoker)(() => LogBox.AppendText(Environment.NewLine + "Currebt Decrypt : " + sCipherResults + Environment.NewLine)));
                                InstCurr = (bDecryptResult[5] << 24) | (bDecryptResult[6] << 16) | (bDecryptResult[7] << 8) | bDecryptResult[8];
                                CheckNextCmdInstantCurr(true);
                            }
                        }
                    }
                    else if (CmdType == CMD_GET_INSTANTDATA)
                    {
                        if (ReceiveStr[0] != 0)
                        {
                            byte[] _SDecNonce = ServerNonce;
                            byte[] _SDecsubNonce_FrameCount = new byte[4];
                            Buffer.BlockCopy(ReceiveStr, 14, _SDecsubNonce_FrameCount, 0, _SDecsubNonce_FrameCount.Length);

                            byte[] _SFinalNonce = FrameNonce(_SDecNonce, _SDecsubNonce_FrameCount, _SDecNonce.Length);

                            int iLen = Convert.ToInt32(ReceiveStr[12].ToString("X2"), 16);  //13
                            byte[] _bFrameCount = new byte[4]; bInstantData = new byte[(iLen - 5) + 12];

                            byte[] _bAuthenticationTag = new byte[12];
                            Buffer.BlockCopy(ReceiveStr, 14, _bFrameCount, 0, 4);   //15
                            Buffer.BlockCopy(ReceiveStr, 18, bInstantData, 0, 14);    //19  , iLen - 5
                            Buffer.BlockCopy(ReceiveStr, 32, _bAuthenticationTag, 0, 12);

                            string sBeforeInstantCrypt = String.Concat(bInstantData.Select(b => b.ToString("X2") + " "));

                            byte[] bDecryptResult = AESGCM.SimpleDecrypt(bInstantData, DedicatedKey, _SFinalNonce);
                            if (bDecryptResult[0] == 196)
                            {
                                string sCipherResults = String.Concat(bDecryptResult.Select(b => b.ToString("X2") + " "));
                                this.LogBox.Invoke((MethodInvoker)(() => LogBox.AppendText(Environment.NewLine + "Voltage Decrypt : " + sCipherResults + Environment.NewLine)));
                                InstVolt = (bDecryptResult[5] << 8) | bDecryptResult[6];
                                requestType = REQ_GET_INSTANTVOLT;
                                //int combinedValue = (a1 << 8) | a2;
                                CheckNextCmdInstantVolt(true);
                            }
                        }
                    }
                    else if (CmdType == CMD_GETREQ_SCALARUNITCURR)
                    {
                        if (ReceiveStr[0] != 0)
                        {
                            byte[] _SDecNonce = ServerNonce;
                            byte[] _SDecsubNonce_FrameCount = new byte[4];
                            Buffer.BlockCopy(ReceiveStr, 14, _SDecsubNonce_FrameCount, 0, _SDecsubNonce_FrameCount.Length);

                            byte[] _SFinalNonce = FrameNonce(_SDecNonce, _SDecsubNonce_FrameCount, _SDecNonce.Length);

                            int iLen = Convert.ToInt32(ReceiveStr[12].ToString("X2"), 16);  //13
                            byte[] _bFrameCount = new byte[4]; bInstantData = new byte[(iLen - 5) + 12];

                            byte[] _bAuthenticationTag = new byte[12];
                            Buffer.BlockCopy(ReceiveStr, 14, _bFrameCount, 0, 4);   //15
                            Buffer.BlockCopy(ReceiveStr, 18, bInstantData, 0, 14);    //19  , iLen - 5
                            Buffer.BlockCopy(ReceiveStr, 32, _bAuthenticationTag, 0, 12);

                            string sBeforeInstantCrypt = String.Concat(bInstantData.Select(b => b.ToString("X2") + " "));

                            byte[] bDecryptResult = AESGCM.SimpleDecrypt(bInstantData, DedicatedKey, _SFinalNonce);
                            if (bDecryptResult[0] == 196)
                            {
                                string sCipherResults = String.Concat(bDecryptResult.Select(b => b.ToString("X2") + " "));
                                this.LogBox.Invoke((MethodInvoker)(() => LogBox.AppendText(Environment.NewLine + "Current Scalar Decrypt : " + sCipherResults + Environment.NewLine)));
                                ScalarInstCurr = Convert.ToDouble(bDecryptResult[7]);
                                //byte byteSclVol = Convert.ToByte(HexScalarInstVolt, 16);
                                sbyte signedValue = (sbyte)ScalarInstCurr;
                                double Val = Math.Pow(10, signedValue);
                                double Curr = Math.Round((InstCurr * Val), 3);
                                lblInstCurr.Text = Curr.ToString();
                                requestType = REQ_GET_INSTANTNEUTRALCURR;
                                CheckNextCmdActionResponse(true);
                                //CheckNextCmdScalarInstVolt(true);
                            }
                        }
                    }
                    else if (CmdType == CMD_GET_INSTNEUCURR)
                    {
                        if (ReceiveStr[0] != 0)
                        {
                            byte[] _SDecNonce = ServerNonce;
                            byte[] _SDecsubNonce_FrameCount = new byte[4];
                            Buffer.BlockCopy(ReceiveStr, 14, _SDecsubNonce_FrameCount, 0, _SDecsubNonce_FrameCount.Length);

                            byte[] _SFinalNonce = FrameNonce(_SDecNonce, _SDecsubNonce_FrameCount, _SDecNonce.Length);

                            int iLen = Convert.ToInt32(ReceiveStr[12].ToString("X2"), 16);  //13
                            byte[] _bFrameCount = new byte[4]; bInstantData = new byte[(iLen - 5) + 12];

                            byte[] _bAuthenticationTag = new byte[12];
                            Buffer.BlockCopy(ReceiveStr, 14, _bFrameCount, 0, 4);   //15
                            Buffer.BlockCopy(ReceiveStr, 18, bInstantData, 0, 14);    //19  , iLen - 5
                            Buffer.BlockCopy(ReceiveStr, 32, _bAuthenticationTag, 0, 12);

                            string sBeforeInstantCrypt = String.Concat(bInstantData.Select(b => b.ToString("X2") + " "));

                            byte[] bDecryptResult = AESGCM.SimpleDecrypt(bInstantData, DedicatedKey, _SFinalNonce);
                            if (bDecryptResult[0] == 196)
                            {
                                string sCipherResults = String.Concat(bDecryptResult.Select(b => b.ToString("X2") + " ")); //requestType = REQ_GET_INSTANTCURR_US;
                                this.LogBox.Invoke((MethodInvoker)(() => LogBox.AppendText(Environment.NewLine + "Neutral Currebt Decrypt : " + sCipherResults + Environment.NewLine)));
                                InstNeutralCurr = (bDecryptResult[5] << 24) | (bDecryptResult[6] << 16) | (bDecryptResult[7] << 8) | bDecryptResult[8];
                                CheckNextCmdGetReqSNo(true);
                            }
                        }
                    }
                    else if (CmdType == CMD_GETINSTNEUSCACURR)
                    {
                        if (ReceiveStr[0] != 0)
                        {
                            byte[] _SDecNonce = ServerNonce;
                            byte[] _SDecsubNonce_FrameCount = new byte[4];
                            Buffer.BlockCopy(ReceiveStr, 14, _SDecsubNonce_FrameCount, 0, _SDecsubNonce_FrameCount.Length);

                            byte[] _SFinalNonce = FrameNonce(_SDecNonce, _SDecsubNonce_FrameCount, _SDecNonce.Length);

                            int iLen = Convert.ToInt32(ReceiveStr[12].ToString("X2"), 16);  //13
                            byte[] _bFrameCount = new byte[4]; bInstantData = new byte[(iLen - 5) + 12];

                            byte[] _bAuthenticationTag = new byte[12];
                            Buffer.BlockCopy(ReceiveStr, 14, _bFrameCount, 0, 4);   //15
                            Buffer.BlockCopy(ReceiveStr, 18, bInstantData, 0, 14);    //19  , iLen - 5
                            Buffer.BlockCopy(ReceiveStr, 32, _bAuthenticationTag, 0, 12);

                            string sBeforeInstantCrypt = String.Concat(bInstantData.Select(b => b.ToString("X2") + " "));

                            byte[] bDecryptResult = AESGCM.SimpleDecrypt(bInstantData, DedicatedKey, _SFinalNonce);
                            if (bDecryptResult[0] == 196)
                            {
                                string sCipherResults = String.Concat(bDecryptResult.Select(b => b.ToString("X2") + " "));
                                this.LogBox.Invoke((MethodInvoker)(() => LogBox.AppendText(Environment.NewLine + "Current Scalar Decrypt : " + sCipherResults + Environment.NewLine)));
                                ScalarInstCurr = Convert.ToDouble(bDecryptResult[7]);
                                sbyte signedValue = (sbyte)ScalarInstCurr;
                                double Val = Math.Pow(10, signedValue);
                                double Curr = Math.Round((InstCurr * Val), 3);
                                lblReadNeutralCurrFuncTest.Text = Curr.ToString();
                                requestType = REQ_SET_STARTMETERCMD;
                                CheckNextCmdActionResponse(true);
                            }
                        }
                    }
                    else if (CmdType == CMD_SETREQ_STOPMETER)
                    {
                        if (ReceiveStr[0] != 0)
                        {
                            byte[] _SDecNonce = ServerNonce;
                            byte[] _SDecsubNonce_FrameCount = new byte[4];
                            Buffer.BlockCopy(ReceiveStr, 14, _SDecsubNonce_FrameCount, 0, _SDecsubNonce_FrameCount.Length);

                            byte[] _SFinalNonce = FrameNonce(_SDecNonce, _SDecsubNonce_FrameCount, _SDecNonce.Length);

                            int iLen = Convert.ToInt32(ReceiveStr[12].ToString("X2"), 16);  //13
                            byte[] _bFrameCount = new byte[4]; bInstantData = new byte[(iLen - 5) + 12];

                            byte[] _bAuthenticationTag = new byte[12];
                            Buffer.BlockCopy(ReceiveStr, 14, _bFrameCount, 0, 4);   //15
                            Buffer.BlockCopy(ReceiveStr, 18, bInstantData, 0, 14);    //19  , iLen - 5
                            Buffer.BlockCopy(ReceiveStr, 32, _bAuthenticationTag, 0, 12);

                            string sBeforeInstantCrypt = String.Concat(bInstantData.Select(b => b.ToString("X2") + " "));

                            byte[] bDecryptResult = AESGCM.SimpleDecrypt(bInstantData, DedicatedKey, _SFinalNonce);

                            if (bDecryptResult[0] == 197)
                            {
                                string sCipherResults = String.Concat(bDecryptResult.Select(b => b.ToString("X2") + " "));

                                this.LogBox.Invoke((MethodInvoker)(() => LogBox.AppendText(Environment.NewLine + "TEST RESULT DECODED : " + sCipherResults + Environment.NewLine)));

                                byte[] ResultHex = new byte[4];
                                Buffer.BlockCopy(bDecryptResult, 5, ResultHex, 0, 4);

                                string resultValue = string.Empty;
                                foreach (byte b in ResultHex)
                                {
                                    string bitString = ConvertByteToBits(b);
                                    resultValue = resultValue + bitString;
                                }
                                //this.txtFlagStatus.Invoke((MethodInvoker)(() => txtFlagStatus.Text = resultValue));
                                /*FinalResult(resultValue);*/
                                LabelNILChangeStop(); CheckNextCmdDisconnect(true); btnTestStart.Enabled = true; btnStop.Enabled = false;
                            }
                            else
                            {
                                MessageBox.Show("Wrong Response...");
                            }
                        }

                    }
                    else if (CmdType == CMD_GETREQ_READMETERCONTIOUSLY)
                    {
                        if (ReceiveStr[0] != 0)
                        {
                            byte[] _SDecNonce = ServerNonce;
                            byte[] _SDecsubNonce_FrameCount = new byte[4];
                            Buffer.BlockCopy(ReceiveStr, 14, _SDecsubNonce_FrameCount, 0, _SDecsubNonce_FrameCount.Length);

                            byte[] _SFinalNonce = FrameNonce(_SDecNonce, _SDecsubNonce_FrameCount, _SDecNonce.Length);

                            int iLen = Convert.ToInt32(ReceiveStr[12].ToString("X2"), 16);  //13
                            byte[] _bFrameCount = new byte[4]; bInstantData = new byte[(iLen - 5) + 12];

                            byte[] _bAuthenticationTag = new byte[12];
                            Buffer.BlockCopy(ReceiveStr, 14, _bFrameCount, 0, 4);   //15
                            Buffer.BlockCopy(ReceiveStr, 18, bInstantData, 0, 14);    //19  , iLen - 5
                            Buffer.BlockCopy(ReceiveStr, 32, _bAuthenticationTag, 0, 12);

                            string sBeforeInstantCrypt = String.Concat(bInstantData.Select(b => b.ToString("X2") + " "));

                            byte[] bDecryptResult = AESGCM.SimpleDecrypt(bInstantData, DedicatedKey, _SFinalNonce);

                            if (bDecryptResult[0] == 196)
                            {
                                string sCipherResults = String.Concat(bDecryptResult.Select(b => b.ToString("X2") + " "));

                                //this.RTCBox.AppendText(Environment.NewLine + "Serial No DECODED : " + sCipherResults + Environment.NewLine);
                                this.LogBox.Invoke((MethodInvoker)(() => LogBox.AppendText(Environment.NewLine + "TEST RESULT DECODED : " + sCipherResults + Environment.NewLine)));

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
                                StatusTrip.Text = "Continuous testing Process";
                                //this.txtFlagStatus.Invoke((MethodInvoker)(() => txtFlagStatus.Text = resultValue));
                                FinalResult(resultValue);//tmrContinuosRead.Enabled = true;
                                                         // CheckNextCmdActionResponse(true);
                                if (StopStatus == false) { CheckNextCmdActionResponse(true); }
                                else
                                {
                                    statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                                    requestType = REQ_SET_STOPMETERCMD; cmdSequence = 0;
                                    CmdType = CMD_SNRM_US; seqIncDecCount =4;
                                    MainCmdSendFlag = true;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Wrong Response...");
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }
        private void UpdateLogStatus(string message)
        {
            try
            {
                LogBox.ScrollToCaret();
                LogBox.BeginInvoke(new Action(() =>
                {
                    LogBox.AppendText(DateTime.Now.ToString("\ndd-MM-yyyy HH:mm:ss") + ": " + message + Environment.NewLine);
                }));
                // LogBox.SelectionStart = 0;
                LogBox.ScrollToCaret();
            }
            catch { }
        }
        private string ConvertBytesToAscii(byte[] bytes)
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
        private void LoadCOMPort()
        {
            //COMM Port
            string[] Port = SerialPort.GetPortNames();
            if (Port.Length == 0) { MessageBox.Show("No COMM Port are detected..."); return; }
            cmbSerialPort.Items.AddRange(Port);
            cmbSerialPort.SelectedIndex = 0;
        }
        public void SetToReadFunctionalTest()
        {
            MainCmdSendFlag = false; StopStatus = false;
            statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
            requestType = REQ_SET_STARTMETERCMD; CmdType = CMD_GET_INSTANTDATA; //CmdType = CMD_SETREQ_STARTMETER;

            cmdSequence = 0; seqIncDecCount = 4;
            MainCmdSendFlag = true;
        }
        private byte[] FramePublicHDLC(byte length, byte controlByte)
        {
            byte[] hdlc = new byte[11];
            hdlc[0] = objHDLC.Flag;
            hdlc[1] = objHDLC.FrameFormat;
            hdlc[2] = length;
            Buffer.BlockCopy(objHDLC.PCAddress, 0, hdlc, 3, 2);
            hdlc[5] = controlByte;

            byte[] _hcs = FCS.ComputeHCS(hdlc);
            Buffer.BlockCopy(_hcs, 0, hdlc, 6, 2);

            // Buffer.BlockCopy(hcs, 0, hdlc, 6, 2);   
            Buffer.BlockCopy(objHDLC.LLC, 0, hdlc, 8, 3);
            return hdlc;
        }
        private byte[] CreatePlainText(byte tag, byte attribute, byte[] classId, byte[] logicalName)
        {
            byte[] plainText = new byte[13];
            try
            {
                int len = 0;
                byte type = 0x01;
                byte invokeId = 0xC1;
                byte parameter = 0x00;
                plainText[0] = tag;
                plainText[1] = type;
                plainText[2] = invokeId;
                len = 3;
                Buffer.BlockCopy(classId, 0, plainText, len, classId.Length);
                Buffer.BlockCopy(logicalName, 0, plainText, len += classId.Length, logicalName.Length);
                plainText[len + logicalName.Length] = attribute;
                plainText[len + 1] = parameter;
            }
            catch { }
            return plainText;
        }
        public void CheckNextCmdSetCalibNxtLvl(bool status)
        {
            statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
            if (requestType == REQ_GET_CALIBREAD)
            {
                if (status == true) { CmdType = CMD_DISCONNECT_US; MainCmdSendFlag = true; }
            }
            else if (requestType == REQ_SET_CALIBVALUE)
            {
                if (status == true) { CmdType = CMD_SET_CALIBVALUE; MainCmdSendFlag = true; }// CmdType = CMD_SET_CALIBVALUE; MainCmdSendFlag = true; 
            }
        }
        public void CheckNextCmdSNRM(bool status)
        {
            try
            {
                statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;

                if (requestType == REQ_GET_IC_PC)
                {
                    if (status) { CmdType = CMD_AARQ_PC; MainCmdSendFlag = true; }
                    else { DiscCmdSend(); }
                }
                else if (requestType == REQ_GET_SNO_US || requestType == REQ_GET_FUNCTIONLITYTEST || requestType == REQ_GET_INSTANTVOLT || requestType == REQ_GET_CALIBREAD)
                {
                    if (status) { CmdType = CMD_AARQ_US; MainCmdSendFlag = true; }
                    else { DiscCmdSend(); }
                }
                else if (requestType == REQ_SET_SNO_US || requestType == REQ_SET_STARTMETERCMD || requestType == REQ_SET_STOPMETERCMD || requestType == REQ_SET_CALIBVALUE)
                {
                    if (status) { CmdType = CMD_AARQ_US; MainCmdSendFlag = true; }
                    else { DiscCmdSend(); }
                }
            }
            catch (Exception ex)
            { }
        }
        public void CheckNextCmdAARQ(bool status)
        {
            SecurityKeys securityKey = new SecurityKeys();
            try
            {
                statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                if (requestType == REQ_GET_IC_PC)
                {
                    if (status)
                    { CmdType = CMD_GETREQ_PC; MainCmdSendFlag = true; }
                    else { DiscCmdSend(); }
                }
                else if (requestType == REQ_GET_SNO_US || requestType == REQ_GET_INSTANTVOLT || requestType == REQ_GET_CALIBREAD)
                {
                    if (status)
                    {
                        Buffer.BlockCopy(ReceiveStr, 40, ServerNonce, 0, 8);
                        Buffer.BlockCopy(ReceiveStr, 65, StoC, 0, 16);
                        string StoC_Str = string.Concat(StoC.Select(b => b.ToString("X2") + " "));
                        //this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "S to C : " + StoC_Str + Environment.NewLine)));
                        UpdateLogStatus("S to C : " + StoC_Str + Environment.NewLine);
                        byte[] tempPwd = securityKey.SecretKey; // Encoding.UTF8.GetBytes("AVG_SECRET_ASSC2");
                        DLMS_AES.Encrypt(StoC, tempPwd);

                        string EncryptStr = string.Concat(StoC.Select(b => b.ToString("X2") + " "));
                        // this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "Encrypted S to C : " + EncryptStr + Environment.NewLine)));
                        UpdateLogStatus("Encrypted S to C : " + EncryptStr + Environment.NewLine);
                        Buffer.BlockCopy(StoC, 0, Encrypt_StoC, 0, 16);
                        CmdType = CMD_ACTREQ_US; MainCmdSendFlag = true;
                    }
                    else { DiscCmdSend(); }
                }
                else if (requestType == REQ_SET_SNO_US || requestType == REQ_SET_STARTMETERCMD || requestType == REQ_SET_STOPMETERCMD || requestType == REQ_GET_FUNCTIONLITYTEST || requestType == REQ_SET_CALIBVALUE)
                {
                    if (status)
                    {
                        Buffer.BlockCopy(ReceiveStr, 40, ServerNonce, 0, 8);
                        Buffer.BlockCopy(ReceiveStr, 65, StoC, 0, 16);
                        string StoC_Str = string.Concat(StoC.Select(b => b.ToString("X2") + " "));
                        //this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "S to C : " + StoC_Str + Environment.NewLine)));
                        UpdateLogStatus("S to C : " + StoC_Str + Environment.NewLine);
                        byte[] tempPwd = securityKey.SecretKey; // Encoding.UTF8.GetBytes("AVG_SECRET_ASSC2");
                        DLMS_AES.Encrypt(StoC, tempPwd);

                        string EncryptStr = string.Concat(StoC.Select(b => b.ToString("X2") + " "));
                        // this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "Encrypted S to C : " + EncryptStr + Environment.NewLine)));
                        UpdateLogStatus("Encrypted S to C : " + EncryptStr + Environment.NewLine);
                        Buffer.BlockCopy(StoC, 0, Encrypt_StoC, 0, 16);
                        CmdType = CMD_ACTREQ_US; MainCmdSendFlag = true;
                    }
                    else { DiscCmdSend(); }
                }
            }
            catch (Exception ex)
            { }
        }
        public void CheckNextCmdGetReq(bool status)
        {
            try
            {
                statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                if (requestType == REQ_GET_IC_PC)
                {
                    if (status) { CmdType = CMD_READIC_PC; MainCmdSendFlag = true; }
                    else { DiscCmdSend(); }
                }
            }
            catch (Exception ex)
            { }
        }
        public void CheckNextCmdReadInvocation(bool status)
        {
            try
            {
                statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                if (requestType == REQ_GET_IC_PC)
                {
                    if (status) { CmdType = CMD_DISCONNECT; MainCmdSendFlag = true; }
                    else
                    { DiscCmdSend(); }
                }
            }
            catch (Exception ex)
            { }
        }
        public void CheckNextCmdScalarInstVolt(bool status)
        {
            try
            {
                statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;

                if (requestType == REQ_GET_INSTANTVOLT)
                {
                    if (status) { CmdType = CMD_GETREQ_INSTANTCURR; MainCmdSendFlag = true; }
                    else
                    { DiscCmdSend(); }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void CheckNextCmdInstantVolt(bool status)
        {
            try
            {
                statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                if (requestType == REQ_GET_INSTANTVOLT)
                {
                    if (status) { CmdType = CMD_GETREQ_SCALARUNIT; MainCmdSendFlag = true; }
                    else
                    { DiscCmdSend(); }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void CheckNextCmdInstantCurr(bool status)
        {
            try
            {
                statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                if (requestType == REQ_GET_INSTANTVOLT)
                {
                    if (status) { CmdType = CMD_GETREQ_SCALARUNITCURR; MainCmdSendFlag = true; }
                    else
                    { DiscCmdSend(); }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void CheckNextCmdDisconnect(bool status)
        {
            try
            {
                statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                if (requestType == REQ_SET_STOPMETERCMD)
                {
                    if (StopStatus == true)
                    {
                        StopStatus = false; tmrAutoStop.Enabled = false; ResetCount = 25;
                        requestType = REQ_GET_INSTANTVOLT; CmdType = CMD_SNRM_US; MainCmdSendFlag = true;
                    }
                }
                else
                {
                    if (status) { CmdType = CMD_DISCONNECT; MainCmdSendFlag = true; }
                }
            }
            catch (Exception ex) { }
        }
        public void CheckNextCmdActionResponse(bool status)
        {
            statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
            if (requestType == REQ_GET_SNO_US)
            {
                if (status == true)
                { CmdType = CMD_GETREQ_SNO; MainCmdSendFlag = true; };
            }
            else if (requestType == REQ_SET_SNO_US)
            {
                if (status == true)
                { CmdType = CMD_SETREQ_SNO; MainCmdSendFlag = true; }
            }
            else if (requestType == REQ_SET_STARTMETERCMD)
            {
                statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                if (status == true) { CmdType = CMD_SETREQ_STARTMETER; MainCmdSendFlag = true; }
            }
            else if (requestType == REQ_GET_FUNCTIONLITYTEST)
            {
                statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                if (status == true) { CmdType = CMD_GETREQ_READMETERCONTIOUSLY; MainCmdSendFlag = true; }
            }
            else if (requestType == REQ_GET_INSTANTVOLT)
            {
                statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                { CmdType = CMD_GET_INSTANTDATA; MainCmdSendFlag = true; }
            }
            else if (requestType == REQ_SET_STOPMETERCMD)
            {
                statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                if (status == true) { CmdType = CMD_SETREQ_STOPMETER; MainCmdSendFlag = true; }
            }
            else if (requestType == REQ_GET_CALIBREAD)
            {
                statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                if (status == true) { CmdType = CMD_GET_CALIBVALUE; MainCmdSendFlag = true; }
            }
            else if (requestType == REQ_SET_CALIBVALUE)
            {
                statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                if (status == true) { CmdType = CMD_GET_CALIBINST; MainCmdSendFlag = true; }
            }
            else if (requestType == REQ_GET_INSTANTNEUTRALCURR)
            {
                statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                if (status == true) { CmdType = CMD_GET_INSTNEUCURR; MainCmdSendFlag = true; }
            }
        }
        public void CheckNextCmdGetReqLogicName(bool status)
        {
            statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
            if (requestType == REQ_GET_SNO_US)
            {
                if (status == true)
                {
                    CmdType = CMD_GETREQ_SNO; MainCmdSendFlag = true;
                }
            }
        }
        public void CheckNextCmdGetReqSNo(bool status)
        {
            statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
            if (requestType == REQ_GET_SNO_US)
            {
                if (status == true)
                {
                    CmdType = CMD_GETREQ_RTC; MainCmdSendFlag = true;
                }
            }
            else if (requestType == REQ_GET_INSTANTNEUTRALCURR)
            {
                if (status == true)
                {
                    CmdType = CMD_GETINSTNEUSCACURR; MainCmdSendFlag = true;
                }
            }
        }
        public void CheckNextCmdSetRTC(bool status)
        {
            statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
            if (requestType == REQ_SET_SNO_US)
            {
                if (status == true)
                {
                    CmdType = CMD_SETREQ_RTC; MainCmdSendFlag = true;
                }
            }
        }
        public void CheckNextCmdRamClear(bool status)
        {
            statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
            if (requestType == REQ_SET_RAMCLEAR)
            {
                if (status == true)
                {
                    CmdType = CMD_SETREQ_RAMCLEAR; MainCmdSendFlag = true;
                }
            }
        }
        private string DayOfWeek(int _day)
        {
            string Day = string.Empty;
            try
            {
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
            }
            catch (Exception ex)
            {

            }
            return Day;
        }
        private byte[] IterateInvocation(byte[] dst)
        {
            byte[] src = { 0x00, 0x00, 0x00, 0x01 };
            int carry = 0;

            for (int i = 0; i < dst.Length; ++i)
            {
                byte odst = dst[i];
                byte osrc = i < src.Length ? src[i] : (byte)0;

                byte ndst = (byte)(odst + osrc + carry);

                dst[i] = ndst;
                carry = ndst < odst ? 1 : 0;
            }
            return dst;
        }
        private byte[] GetNextInvocation()
        {
            byte[] dst = { 0x00, 0x00, 0x00, 0x00 };
            SetICContinously++;
            dst[0] = (byte)((SetICContinously >> 24) & 0xff);
            dst[1] = (byte)((SetICContinously >> 16) & 0xff);
            dst[2] = (byte)((SetICContinously >> 8) & 0xff);
            dst[3] = (byte)((SetICContinously) & 0xff);
            return dst;
        }
        private void ErrorDetection(byte[] response)
        {
            byte endFlag = 0x7E;
            byte exceptionResponse = 0xD8;

            try
            {
                if (response[8] == endFlag)
                {
                    switch (response[5])
                    {
                        case 0x1F:
                            MessageBox.Show("HDLC Frame Error: Disconnect Mode", "Exception Message");
                            break;
                        case 0x97:
                            MessageBox.Show("HDLC Frame Error: Frame rejection response", "Exception Message");
                            break;
                        default:
                            MessageBox.Show("Something went wrong...");
                            break;
                    }

                }
                else if (response[16] == endFlag)
                {
                    if (response[11] == exceptionResponse)
                    {
                        MessageBox.Show("Exception Response...", "Exception Message");
                    }
                }
                else
                {
                    MessageBox.Show("Something went wrong...");
                }
            }
            catch { }

        }
        private void FinalResult(string result)
        {
            try
            {
                char[] s1 = new char[result.Length];
                int[] data = new int[result.Length];
                s1 = result.ToCharArray();
                int i = 0;
                foreach (char c in s1)
                {
                    data[i] = Convert.ToInt16(c.ToString());
                    i++;
                }
                tmrAutoStop.Enabled = true;
                DisplayResult(data);
            }
            catch (Exception ex)
            { throw ex; }
        }
        private void DisplayResult(int[] result)
        {
            try
            {
                for (int i = 0; i < result.Length; i++)
                {
                    switch (i)
                    {
                        case 31:

                            if (result[i] == 0)
                            {
                                lblLEDOK.ForeColor = Color.Red; LEDresult.ForeColor = Color.Red;
                                this.lblLEDOK.Invoke((MethodInvoker)(() => lblLEDOK.Text = "NOT OK"));
                                this.LEDresult.Invoke((MethodInvoker)(() => LEDresult.Text = "NOT OK"));
                            }
                            else if (result[i] == 1)
                            {
                                lblLED.BackColor = Color.Yellow;
                                lblLEDOK.ForeColor = Color.Green;
                                LEDresult.ForeColor = Color.Green;
                                this.lblLEDOK.Invoke((MethodInvoker)(() => lblLEDOK.Text = "OK"));
                                this.LEDresult.Invoke((MethodInvoker)(() => LEDresult.Text = "OK"));
                            }
                            break;
                        case 30:

                            if (result[i] == 0)
                            {
                                lblLCDOK.ForeColor = Color.Red; LCDresult.ForeColor = Color.Red;
                                this.lblLCDOK.Invoke((MethodInvoker)(() => lblLCDOK.Text = "NOT OK"));
                                this.LCDresult.Invoke((MethodInvoker)(() => LCDresult.Text = "NOT OK"));
                            }
                            else if (result[i] == 1)
                            {
                                lblLCD.BackColor = Color.Yellow;
                                lblLCDOK.ForeColor = Color.Green;
                                LCDresult.ForeColor = Color.Green;
                                this.lblLCDOK.Invoke((MethodInvoker)(() => lblLCDOK.Text = "OK"));
                                this.LCDresult.Invoke((MethodInvoker)(() => LCDresult.Text = "OK"));
                            }
                            break;
                        case 29:

                            if (result[i] == 0)
                            {
                                lblRelayOK.ForeColor = Color.Red;
                                lblRelayResult.ForeColor = Color.Red;
                                this.lblRelayOK.Invoke((MethodInvoker)(() => lblRelayOK.Text = "NOT OK"));
                                this.lblRelayResult.Invoke((MethodInvoker)(() => lblRelayResult.Text = "NOT OK "));
                            }
                            else if (result[i] == 1)
                            {
                                lblRelay.BackColor = Color.Yellow;
                                lblRelayOK.ForeColor = Color.Green;
                                lblRelayResult.ForeColor = Color.Green;
                                this.lblRelayOK.Invoke((MethodInvoker)(() => lblRelayOK.Text = "OK"));
                                this.lblRelayResult.Invoke((MethodInvoker)(() => lblRelayResult.Text = "OK"));
                            }
                            break;

                        case 28:

                            if (result[i] == 0)
                            {
                                RAMLive.ForeColor = Color.Red;
                                RAMResult.ForeColor = Color.Red;
                                this.RAMLive.Invoke((MethodInvoker)(() => RAMLive.Text = "NOT OK"));
                                this.RAMResult.Invoke((MethodInvoker)(() => RAMResult.Text = "NOT OK "));
                            }
                            else if (result[i] == 1)
                            {
                                lblRAM.BackColor = Color.Yellow;
                                RAMLive.ForeColor = Color.Green;
                                RAMResult.ForeColor = Color.Green;
                                this.RAMLive.Invoke((MethodInvoker)(() => RAMLive.Text = "OK"));
                                this.RAMResult.Invoke((MethodInvoker)(() => RAMResult.Text = "OK"));
                            }
                            break;
                        case 27:

                            if (result[i] == 0)
                            {
                                flashLive.ForeColor = Color.Red; flashResult.ForeColor = Color.Red;
                                this.flashLive.Invoke((MethodInvoker)(() => flashLive.Text = "NOT OK"));
                                this.flashResult.Invoke((MethodInvoker)(() => flashResult.Text = "NOT OK"));
                            }
                            else if (result[i] == 1)
                            {
                                lblFlash.BackColor = Color.Yellow;
                                flashLive.ForeColor = Color.Green;
                                flashResult.ForeColor = Color.Green;
                                this.flashLive.Invoke((MethodInvoker)(() => flashLive.Text = "OK"));
                                this.flashResult.Invoke((MethodInvoker)(() => flashResult.Text = "OK"));
                            }
                            break;

                        case 26:

                            if (result[i] == 0)
                            {
                                lblScroll.BackColor = Color.Yellow;
                                scrollLive.ForeColor = Color.Green;
                                scrollResult.ForeColor = Color.Green;
                                this.scrollLive.Invoke((MethodInvoker)(() => scrollLive.Text = "Relesed"));
                                //this.scrollResult.Invoke((MethodInvoker)(() => scrollResult.Text = "NOT OK"));
                            }
                            else if (result[i] == 1)
                            {
                                lblScroll.BackColor = Color.Yellow;
                                scrollLive.ForeColor = Color.Red; scrollResult.ForeColor = Color.Red;
                                this.scrollLive.Invoke((MethodInvoker)(() => scrollLive.Text = "Pressed"));
                                this.scrollResult.Invoke((MethodInvoker)(() => scrollResult.Text = "OK"));
                            }
                            break;

                        case 24:

                            if (result[i] == 0)
                            {
                                lblTopCover.BackColor = Color.Yellow;
                                TopLive.ForeColor = Color.Green;
                                topResult.ForeColor = Color.Green;
                                this.TopLive.Invoke((MethodInvoker)(() => TopLive.Text = "Closed"));
                                this.topResult.Invoke((MethodInvoker)(() => topResult.Text = "OK"));
                            }
                            else if (result[i] == 1)
                            {
                                lblTopCover.BackColor = Color.Yellow;
                                TopLive.ForeColor = Color.Red;
                                topResult.ForeColor = Color.Red;
                                this.TopLive.Invoke((MethodInvoker)(() => TopLive.Text = "Open"));
                                this.topResult.Invoke((MethodInvoker)(() => topResult.Text = "NOT OK "));
                            }
                            break;

                        case 23:

                            if (result[i] == 0)
                            {
                                lblMagnet.BackColor = Color.Yellow;
                                magnetLive.ForeColor = Color.Green;
                                this.magnetLive.Invoke((MethodInvoker)(() => magnetLive.Text = "No Magnet"));
                                magnetResult.ForeColor = Color.Green;
                                this.magnetResult.Invoke((MethodInvoker)(() => magnetResult.Text = "No Magnet"));
                            }
                            else if (result[i] == 1)
                            {
                                lblMagnet.BackColor = Color.Yellow;
                                magnetLive.ForeColor = Color.Red;
                                this.magnetLive.Invoke((MethodInvoker)(() => magnetLive.Text = "Magnet"));
                                magnetResult.ForeColor = Color.Red;
                                this.magnetResult.Invoke((MethodInvoker)(() => magnetResult.Text = "Magnet OK"));
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }
        private void RaiseNodifyIcon(string NodifyTitle, string NodifyMsg, int NodifyDuration, ToolTipIcon objToolTipIcon)
        {
            try
            {
                this.DLMSNotification.Visible = true;
                this.DLMSNotification.BalloonTipText = NodifyMsg;
                this.DLMSNotification.BalloonTipTitle = NodifyTitle;
                this.DLMSNotification.BalloonTipIcon = objToolTipIcon;
                this.DLMSNotification.ShowBalloonTip(NodifyDuration);
            }
            catch (Exception ex)
            {
                UpdateLogStatus("Method : " + MethodBase.GetCurrentMethod().Name + "\n Error : " + ex.Message);
            }
        }
        private void ToolStripButtons()
        {
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(this.btnCOMMOpen, "Click here to Open COMM Port");
            ToolTip1.SetToolTip(this.btnTestStart, "Click here to Test communication");
            ToolTip1.SetToolTip(this.btnReadSno, "Click here to Read Serial Number & RTC");
            ToolTip1.SetToolTip(this.btnStart, "Click here to Start Read");
            ToolTip1.SetToolTip(this.btnStop, "Click here to Stop Read");
            ToolTip1.SetToolTip(this.btnLogout, "Click here to Logout");

        }
        private void LoadButtonEnable()
        {
            btnTestStart.Enabled = false; btnReadSno.Enabled = false; rdButtonlog.Enabled = false;
            btnStart.Enabled = false; btnStop.Enabled = false;// btnRead.Enabled = false;
        }
        private void LabelNILChangeStop()
        {
            lblLEDOK.Text = "NIL"; LEDresult.Text = "NIL"; lblLCDOK.Text = "NIL"; LCDresult.Text = "NIL"; lblRelayOK.Text = "NIL";
            lblRelayResult.Text = "NIL"; RAMLive.Text = "NIL"; RAMResult.Text = "NIL"; magnetLive.Text = "NIL"; magnetResult.Text = "NIL";
            scrollLive.Text = "NIL"; scrollResult.Text = "NIL"; TopLive.Text = "NIL"; topResult.Text = "NIL"; flashLive.Text = "NIL"; flashResult.Text = "NIL";
            lblInstVolt.Text = "NIL"; lblInstCurr.Text = "NIL";
            lblLEDOK.ForeColor = Color.RoyalBlue; LEDresult.ForeColor = Color.Red; lblLCDOK.ForeColor = Color.RoyalBlue; LCDresult.ForeColor = Color.Red; lblRelayOK.ForeColor = Color.RoyalBlue;
            lblRelayResult.ForeColor = Color.Red; RAMLive.ForeColor = Color.RoyalBlue; RAMResult.ForeColor = Color.Red; magnetLive.ForeColor = Color.RoyalBlue; magnetResult.ForeColor = Color.Red;
            scrollLive.ForeColor = Color.RoyalBlue; scrollResult.ForeColor = Color.Red; TopLive.ForeColor = Color.RoyalBlue; topResult.ForeColor = Color.Red; flashLive.ForeColor = Color.RoyalBlue; flashResult.ForeColor = Color.Red;
            lblLED.BackColor = Color.White; lblLCD.BackColor = Color.White; lblRelay.BackColor = Color.White; lblRAM.BackColor = Color.White;
            lblFlash.BackColor = Color.White; lblScroll.BackColor = Color.White; lblTopCover.BackColor = Color.White; lblMagnet.BackColor = Color.White;
            lblSerialNumberRead.Text = string.Empty; lblRTCRead.Text = string.Empty; StatusTrip.Text = "Meter Stop reading process...";
        }
        static string ConvertByteToBits(byte value)
        {
            return Convert.ToString(value, 2).PadLeft(8, '0');
        }
        private void btnCOMMOpen_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnCOMMOpen.Text == "COM Open")
                {
                    bool isConnected;
                    string PortName = cmbSerialPort.Text;
                    isConnected = MUTSerialCOM.OpenCOM(PortName, 9600, 8, Parity.None, StopBits.One);
                    int SC = statusClass.CmdSendFlag;
                    if (isConnected == true)
                    {
                        MessageBox.Show("Successfully COM Port opened", popUpTitle);
                        UpdateLogStatus("Successfully Serial COM Port opened");
                        btnCOMMOpen.Text = "COM Close";
                        btnCOMMOpen.ForeColor = System.Drawing.Color.Red;
                        btnTestStart.Enabled = true; rdButtonlog.Enabled = true;
                        // btnCOMMOpen.Image = Properties.Resources.UnPlug;
                    }
                    else
                    {
                        MessageBox.Show("Please select another COM", popUpTitle);
                    }
                }
                else if (btnCOMMOpen.Text == "COM Close")
                {
                    MUTSerialCOM.Close();
                    //  isConnected = false;
                    btnCOMMOpen.Text = "COM Open";
                    btnCOMMOpen.ForeColor = System.Drawing.Color.Teal;
                    // btnCOMMOpen.Image = Properties.Resources.plug;
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void tmrCommandSent_Tick(object sender, EventArgs e)
        {
            try
            {
                if (MainCmdSendFlag == true)
                {
                    MainCmdSendFlag = false;

                    switch (CmdType)
                    {
                        case CMD_SNRM:
                            PublicSNRMCmd();
                            break;
                        case CMD_AARQ_PC:
                            PublicClientAARQ();
                            break;
                        case CMD_GETREQ_PC:
                            PublicClientLogicalName();
                            break;
                        case CMD_READIC_PC:
                            ReadInvocation();
                            break;
                        case CMD_DISCONNECT:
                            DiscCmdSend();
                            break;
                        case CMD_SNRM_US:
                            CommonSNRMCMD();
                            break;
                        case CMD_AARQ_US:
                            AARQCommand();
                            break;
                        case CMD_ACTREQ_US:
                            ActionRequestCmd();
                            break;
                        case CMD_GETREQ_LOGICNAME:
                            SNOLogicalName();
                            break;
                        case CMD_GETREQ_SNO:
                            ReadSerialNo();
                            break;
                        case CMD_GETREQ_RTC:
                            ReadRTC();
                            break;
                        case CMD_SETREQ_STARTMETER:
                            SETStartFunctionValue();
                            break;
                        case CMD_SETREQ_STOPMETER:
                            SETStopFunstionalTest();
                            break;
                        case CMD_GETREQ_READMETERCONTIOUSLY:
                            GetFunctionalTestContinuosly(functionalTest.ClassId, functionalTest.OBISCode);
                            break;
                        case CMD_GET_INSTANTDATA:
                            GetInstVoltLogicalName();
                            break;
                        case CMD_GETREQ_SCALARUNIT:
                            GetInstVoltScalarUnit();
                            break;
                        case CMD_GETREQ_INSTANTCURR:
                            GetInstCurrLogicalName();
                            break;
                        case CMD_GETREQ_SCALARUNITCURR:
                            GetInstCurrScalarUnit();
                            break;
                        case CMD_GET_CALIBVALUE:
                            GetCalibValueRead();
                            break;
                        case CMD_DISCONNECT_US:
                            DiscCmdSend_US();
                            break;
                        case CMD_GET_CALIBINST:
                            GetCalibValueRead();
                            break;
                        case CMD_SET_CALIBVALUE:
                            SetCalibValue();
                            break;
                        case CMD_GET_INSTNEUCURR:
                            GetInstNeutralCurrLogicalName();
                            break;
                        case CMD_GETINSTNEUSCACURR:
                            GetInstNeutralCurrScalarUnit();
                            break;
                        default:
                            return;
                    }
                    statusClass.CmdSendFlag = 1;
                    statusClass.CmdTimeOutCount = 40;
                    statusClass.CmdTimeOutFlag = 0;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void btnTestStart_Click(object sender, EventArgs e)
        {
            StatusTrip.Text = "Ready to Read the data"; lblInstVolt.Text = "NIL"; lblInstCurr.Text = "NIL";
            lblCounterIncr.Text = startCount++.ToString();
            requestType = REQ_GET_IC_PC;
            CmdType = CMD_SNRM;
            MainCmdSendFlag = true;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
        }
        private void FunctionalForm_Load(object sender, EventArgs e)
        {
            DataTable ObjDbt = new DataTable(); ObjDbt.Clear();
            string SQLQuery = "Select * from Login";
            ObjDbt = GetDatabaseDataDAL(SQLQuery);
            if (ObjDbt.Rows.Count > 0)
            {
                settings.HLS = ObjDbt.Rows[0][5].ToString();
                settings.GlobalKey = ObjDbt.Rows[0][6].ToString();
                settings.SystemTitle = ObjDbt.Rows[0][7].ToString();
                settings.ZigNumber = ObjDbt.Rows[0][4].ToString();
                statusKeyClass.HLS = settings.HLS; statusKeyClass.GlobalKey = settings.GlobalKey;
                statusKeyClass.SystemTitle = settings.SystemTitle; statusKeyClass.ZigNumber = settings.ZigNumber;
                settings.Save(); settings.Reload();
            }

            LoadDeviceDetails();
            string userType = settings.UserType;
            if (userType == "1")
            {
                tabMain.TabPages.Remove(tabPage1);
                RaiseNodifyIcon(Constants.TOOLNAME, "Application Started Successfully.", Constants.NOTIFICATION_DURATION, ToolTipIcon.Info);
                ToolStripButtons(); LoadButtonEnable(); LoadCOMPort(); LoadUserDetails(); btnUpdateUser.Enabled = false; btnDeleteUser.Enabled = false;
            }
            else
            {
                tabMain.TabPages.Remove(tabPage2);
                RaiseNodifyIcon(Constants.TOOLNAME, "Application Started Successfully.", Constants.NOTIFICATION_DURATION, ToolTipIcon.Info);
                ToolStripButtons(); LoadButtonEnable(); LoadCOMPort(); //LoadUserDetails();
            }
        }

        private void btnReadSno_Click(object sender, EventArgs e)
        {
            MainCmdSendFlag = false;
            statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
            requestType = REQ_GET_SNO_US;
            CmdType = CMD_SNRM_US;
            MainCmdSendFlag = true;
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            MainCmdSendFlag = false;
            statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
            requestType = REQ_GET_INSTANTVOLT;
            CmdType = CMD_SNRM_US;
            MainCmdSendFlag = true;

        }
        private void ExportDataGrid()
        {
            string Username = settings.UserName; string ZigNumber = statusKeyClass.ZigNumber; //string ZigNumber = "AEI01";
            string serialno = lblSerialNumberRead.Text; string LED = LEDresult.Text; string LCD = LCDresult.Text;
            string Relay = lblRelayResult.Text; string EPRom = RAMResult.Text; string magnet = magnetResult.Text;
            string ScrollS = scrollResult.Text; string TopCover = topResult.Text; string Flash = flashResult.Text;
            string RTC = lblRTCRead.Text;
            try
            {
                string SQLQuery = "select count(*) from DeviceDetails where UserName='" + Username + "' and serialNumber = '" + serialno + "'";
                SuccessRetVal = TotalCountData(SQLQuery);
                if (SuccessRetVal >= 1)
                {
                    //Update Login set UserName = '" + txtUsername.Text + "',password = '" + Password + "',Status='" + Status + "',Usertype='2' where Guid = '" + Guid + "'
                    SQLQuery = "Update DeviceDetails set ZigNumber = '" + ZigNumber + "',UserName='" + Username + "',OpticalPort = 'OK',LED='" + LED + "',LCD = '" + LCD + "',Relay='" + Relay + "',EPROM = '" + EPRom + "',Magnet='" + magnet + "',Scroll = '" + ScrollS + "',Topcover='" + TopCover + "',Flash = '" + Flash + "',RTC = '" + RTC + "' where UserName='" + Username + "' and SerialNumber = '" + serialno + "'";
                    SuccessRetVal = InsertConfigData(SQLQuery);
                    if (SuccessRetVal >= 1)
                    {
                        StatusTrip.Text = "Status Updated Successfully..."; LoadDeviceDetails();
                    }
                    else { StatusTrip.Text = "status not Updated"; }
                }
                else
                {
                    SQLQuery = "Insert into DeviceDetails(ZigNumber,UserName,OpticalPort,LED,LCD,Relay,EPROM,Magnet,Scroll,Topcover,Flash,SerialNumber,RTC)  values('" + ZigNumber + "','" + Username + "','OK','" + LED + "','" + LCD + "','" + Relay + "','" + EPRom + "','" + magnet + "','" + ScrollS + "','" + TopCover + "','" + Flash + "','" + serialno + "','" + RTC + "')";
                    SuccessRetVal = InsertConfigData(SQLQuery);
                    if (SuccessRetVal >= 1)
                    {
                        StatusTrip.Text = "Status Inserted Successfully..."; LoadDeviceDetails();
                        // MessageBox.Show("User Inserted Successfully...", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information); //LoadUserDetails(); ClearTextBoxes();
                        // SQLQuery = "select HLS,GlobalKey,SystemTitle from login where Guid";

                    }
                    else { StatusTrip.Text = "Status not Updated"; }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            ExportDataGrid();
            StopStatus = true; //MainCmdSendFlag = true;
        }
        private void btnRead_Click(object sender, EventArgs e)
        {
            MainCmdSendFlag = false;
            statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
            requestType = REQ_GET_INSTANTVOLT;
            CmdType = CMD_SNRM_US;
            MainCmdSendFlag = true;
        }
        bool isChecked = false;
        private void rdButtonlog_CheckedChanged(object sender, EventArgs e)
        {
            isChecked = rdButtonlog.Checked;
        }
        private void rdButtonlog_Click(object sender, EventArgs e)
        {
            if (rdButtonlog.Checked && !isChecked)
            {
                rdButtonlog.Checked = false;
                LogBox.Visible = false;
            }
            else
            {
                rdButtonlog.Checked = true;
                isChecked = false;
                LogBox.Visible = true;
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            Guid objGuid = Guid.NewGuid();

            if (txtZicNo.Text == "" || txtUsername.Text == "" || txtpassword.Text == "" || txtHLS.Text == "" || txtGlobalkey.Text == "" || txtSystemTitle.Text == "")
            {
                MessageBox.Show("Please Fill all the Fields...", "Smartmeter", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }

            int ZigNumber = Convert.ToInt32(txtZicNo.Text); string UserName = txtUsername.Text;
            string Password = txtpassword.Text; string HLS = txtHLS.Text;
            string GlobalKey = txtGlobalkey.Text; string Systemtilte = txtSystemTitle.Text;

            try
            {
                string SQLQuery = "select count(*) from Login where TestZig = '" + ZigNumber + "' and UserName = '" + UserName + "'";
                SuccessRetVal = TotalCountData(SQLQuery);
                if (SuccessRetVal >= 1) { MessageBox.Show("Same Zig and UserName Already Exists", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                else
                {
                    //SQLQuery = "Insert into Login(TestZig,UserName,Password,UserType,status)  values('" + ZigNumber + "','" + UserName + "','" + Password + "','2','1')";
                    SQLQuery = "Insert into Login(Guid,TestZig,UserName,Password,UserType,status,HLS,GlobalKey,SystemTitle)  values('" + objGuid + "','" + ZigNumber + "','" + UserName + "','" + Password + "','2','1','" + HLS + "','" + GlobalKey + "','" + Systemtilte + "')";
                    SuccessRetVal = InsertConfigData(SQLQuery);
                    if (SuccessRetVal >= 1)
                    {
                        MessageBox.Show("User Inserted Successfully...", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information); LoadUserDetails(); ClearTextBoxes();
                        // SQLQuery = "select HLS,GlobalKey,SystemTitle from login where Guid";

                    }
                    else { MessageBox.Show("User not Inserted", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
            }
        }
        private void btnUpdateUser_Click(object sender, EventArgs e)
        {
            //string Guid = txtGUID.Text;
            if (txtZicNo.Text == "" || txtUsername.Text == "" || txtpassword.Text == "")
            {
                MessageBox.Show("Please Fill all the Fields...", "Smartmeter", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            else if (txtZicNo.Text == "1" && txtUsername.Text == "Admin" && txtpassword.Text == "admin")
            {
                MessageBox.Show("You are not a authorised person to Edit the User", "Smartmeter", MessageBoxButtons.OK, MessageBoxIcon.Information); return;
            }
            int Status = 0;
            int ZigNumber = Convert.ToInt32(txtZicNo.Text);
            string UserName = txtUsername.Text;
            string Password = txtpassword.Text; string Guid = txtGUID.Text;
            if (chkStatus.Checked == true) { Status = 1; } else if (chkStatus.Checked == false) { Status = 0; }

            try
            {
                string SQLQuery = "Update Login set UserName = '" + txtUsername.Text + "',password = '" + Password + "',Status='" + Status + "',Usertype='2' where Guid = '" + Guid + "'";
                SuccessRetVal = InsertConfigData(SQLQuery);
                if (SuccessRetVal >= 1) { MessageBox.Show("User Updated Successfully...", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information); LoadUserDetails(); ClearTextBoxes(); }
                else { MessageBox.Show("User not Updated,Please Check", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            }
            catch (Exception ex)
            {

            }
        }
        private void ClearTextBoxes()
        {
            txtUsername.Text = string.Empty; txtpassword.Text = string.Empty; txtZicNo.Text = string.Empty;
            txtGlobalkey.Text = string.Empty; txtHLS.Text = string.Empty; txtSystemTitle.Text = string.Empty;

        }

        private void btnClearUser_Click(object sender, EventArgs e)
        {
            ClearTextBoxes();
            txtZicNo.Enabled = true; btnDeleteUser.Enabled = false; chkStatus.Visible = false;
            btnAddUser.Enabled = true; btnUpdateUser.Enabled = false;
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            string Guid = txtGUID.Text;
            if (txtZicNo.Text == "" || txtUsername.Text == "" || txtpassword.Text == "")
            {
                MessageBox.Show("Please Fill all the Fields...", "Smartmeter", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            else if (txtZicNo.Text == "1" && txtUsername.Text == "Admin" && txtpassword.Text == "admin")
            {
                MessageBox.Show("You are not a authorised person to Delete the User", "Smartmeter", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            int ZigNumber = Convert.ToInt32(txtZicNo.Text);
            try
            {
                string SQLQuery = "Delete from Login where Guid = '" + Guid + "'";
                SuccessRetVal = DeleteProcessDataDAL(SQLQuery);
                if (SuccessRetVal >= 1) { MessageBox.Show("User Deleted Successfully...", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information); LoadUserDetails(); ClearTextBoxes(); }
                else { MessageBox.Show("User not Delete,Please Check", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            }
            catch (Exception ex)
            {

            }

        }
        private void griduser_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DialogResult d;
            d = MessageBox.Show("Do You want to Edit/Delete the Configuration?", "SmartMeter", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (d == DialogResult.Yes)
            {
                chkStatus.Visible = true; txtZicNo.Enabled = false; btnAddUser.Enabled = false;
                btnUpdateUser.Enabled = true; btnDeleteUser.Enabled = true;
                if (griduser[0, e.RowIndex].Value != null)
                {
                    txtZicNo.Text = griduser[4, e.RowIndex].Value.ToString(); settings.ZigNumber = txtZicNo.Text;
                    txtUsername.Text = griduser[0, e.RowIndex].Value.ToString();
                    txtpassword.Text = griduser[1, e.RowIndex].Value.ToString();
                    int status = Convert.ToInt32(griduser[3, e.RowIndex].Value.ToString());
                    if (status == 1) { chkStatus.Checked = true; } else if (status == 0) { chkStatus.Checked = false; }
                    txtHLS.Text = griduser[5, e.RowIndex].Value.ToString();
                    txtGlobalkey.Text = griduser[6, e.RowIndex].Value.ToString();
                    txtSystemTitle.Text = griduser[7, e.RowIndex].Value.ToString();
                    txtGUID.Text = griduser[8, e.RowIndex].Value.ToString();
                }
            }
        }
        public int InsertConfigData(string SQLQuery)
        {
            using (SQLiteConnection ObjSQLiteCon = new SQLiteConnection(ConStr))
            {
                using (SQLiteCommand ObjSQLiteCmd = new SQLiteCommand(SQLQuery, ObjSQLiteCon))
                {
                    ObjSQLiteCmd.CommandType = CommandType.Text;
                    ObjSQLiteCon.Open();
                    UpdateRows = ObjSQLiteCmd.ExecuteNonQuery();
                }
            }
            return UpdateRows;
        }
        public int TotalCountData(string SQLQuery)
        {
            try
            {
                using (SQLiteConnection ObjSQLiteCon = new SQLiteConnection(ConStr))
                {
                    using (SQLiteCommand ObjSQLiteCmd = new SQLiteCommand(SQLQuery, ObjSQLiteCon))
                    {
                        ObjSQLiteCmd.CommandType = CommandType.Text;
                        ObjSQLiteCon.Open();
                        int recordCount = Convert.ToInt32(ObjSQLiteCmd.ExecuteScalar());
                        return recordCount;
                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public DataTable GetDatabaseDataDAL(string SQLQuery)
        {
            DataTable Dt = new DataTable();
            try
            {
                using (SQLiteConnection ObjSQLiteCon = new SQLiteConnection(ConStr))
                {
                    using (SQLiteCommand ObjSQLiteCmd = new SQLiteCommand(SQLQuery, ObjSQLiteCon))
                    {
                        ObjSQLiteCmd.CommandType = CommandType.Text;
                        ObjSQLiteCon.Open();
                        SQLiteDataAdapter sQLiteDataAdapter = new SQLiteDataAdapter(SQLQuery, ObjSQLiteCon);
                        sQLiteDataAdapter.Fill(Dt);
                        return Dt;
                    }
                }
            }
            catch (Exception ex)
            {
                return Dt;
            }
        }
        public int DeleteProcessDataDAL(string SQLQuery)
        {
            try
            {
                using (SQLiteConnection ObjSQLiteCon = new SQLiteConnection(ConStr))
                {
                    using (SQLiteCommand ObjSQLiteCmd = new SQLiteCommand(SQLQuery, ObjSQLiteCon))
                    {
                        ObjSQLiteCmd.CommandType = CommandType.Text;
                        ObjSQLiteCon.Open();
                        DeleteRow = ObjSQLiteCmd.ExecuteNonQuery();
                    }
                }
                return DeleteRow;

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message.ToString());
            }
            finally
            {
                //ObjCon.Close();
            }
        }
        private void LoadUserDetails()
        {
            try
            {
                DataTable ObjDbt = new DataTable(); ObjDbt.Clear();
                string SQLQuery = "select UserName,Password,Usertype,Status,(TestZig) as ZigNumber,SystemTitle,HLS,GlobalKey from Login";
                ObjDbt = GetDatabaseDataDAL(SQLQuery);
                if (ObjDbt.Rows.Count > 0)
                {
                    griduser.DataSource = ObjDbt;
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void LoadDeviceDetails()
        {
            try
            {
                //txtZicNo.Enabled = true; 
                DataTable ObjDbt = new DataTable(); ObjDbt.Clear();
                string SQLQuery = "Select * from DeviceDetails";
                ObjDbt = GetDatabaseDataDAL(SQLQuery);
                if (ObjDbt.Rows.Count > 0)
                {
                    gridReport.DataSource = ObjDbt;
                }
                // griduser.Rows[0].ReadOnly = true;
                // griduser.Rows[0].Selected = false;
            }
            catch (Exception ex)
            {

            }
        }

        //////////////////-----------------Commands-----------------------------/////////////////
        private void CommonSNRMCMD()
        {
            byte[] SNRMCmd = comm.SNRMCommand();
            string ss = String.Concat(SNRMCmd.Select(b => b.ToString("X2") + " "));
            this.LogBox.Invoke((MethodInvoker)(() => LogBox.AppendText("\n" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss ") + "  SNRM Request : " + ss)));
            MUTSerialCOM.Send(SNRMCmd);
        }
        private void PublicSNRMCmd()
        {
            SNRM objSNRM = new SNRM();
            try
            {
                byte[] snrmCmd = new byte[34];
                objHDLC.HCS = new byte[] { 0x7D, 0xD9 };
                byte[] hdlc = FramePublicHDLC(32, objControlByte.SNRMCByte);
                Buffer.BlockCopy(hdlc, 0, snrmCmd, 0, 8);
                Buffer.BlockCopy(objSNRM.SNRMPCData, 0, snrmCmd, hdlc.Length - 3, objSNRM.SNRMPCData.Length);

                objerror.FCS = FCS.ComputeFCS(snrmCmd);
                Buffer.BlockCopy(objerror.FCS, 0, snrmCmd, objSNRM.SNRMPCData.Length + 8, objerror.FCS.Length);
                Buffer.BlockCopy(hdlc, 0, snrmCmd, objSNRM.SNRMPCData.Length + 8 + objerror.FCS.Length, 1);

                string finalCommand = String.Concat(snrmCmd.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("SNRM : " + finalCommand);
                MUTSerialCOM.Send(snrmCmd);
            }
            catch { }
        }
        private void PublicClientAARQ()
        {
            PublicClientAARQ objAARQ = new PublicClientAARQ();

            try
            {
                byte[] AARQcmd = new byte[45];
                objHDLC.HCS = new byte[] { 0xFB, 0xAF };
                int len = 0;

                byte[] hdlc = FramePublicHDLC(43, objControlByte.AARQCByte);
                Buffer.BlockCopy(hdlc, 0, AARQcmd, 0, 11);
                AARQcmd[11] = objTag.AARQTag;
                AARQcmd[12] = 0x1D;
                len = hdlc.Length + 1 + 1;

                Buffer.BlockCopy(objAARQ.PCBloack1, 0, AARQcmd, len, objAARQ.PCBloack1.Length);
                Buffer.BlockCopy(objAARQ.PCPlainText, 0, AARQcmd, len += objAARQ.PCBloack1.Length, objAARQ.PCPlainText.Length);

                objerror.FCS = FCS.ComputeFCS(AARQcmd);
                Buffer.BlockCopy(objerror.FCS, 0, AARQcmd, len += objAARQ.PCPlainText.Length, objerror.FCS.Length);
                Buffer.BlockCopy(hdlc, 0, AARQcmd, len += objerror.FCS.Length, 1);

                string finalCommand = String.Concat(AARQcmd.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("AARQ : " + finalCommand);
                MUTSerialCOM.Send(AARQcmd);
            }
            catch { }
        }
        private void PublicClientLogicalName()
        {
            try
            {
                byte[] logicalNameCmd = new byte[27];
                byte[] plainText;

                byte[] hdlc = FramePublicHDLC(0x19, objControlByte.ActionCByte);

                Buffer.BlockCopy(hdlc, 0, logicalNameCmd, 0, hdlc.Length);

                plainText = CreatePlainText(objTag.GetRequest, 0x01, objcls.Class1, objOBIS.InvocationOBIS);
                Buffer.BlockCopy(plainText, 0, logicalNameCmd, hdlc.Length, plainText.Length);

                objerror.FCS = FCS.ComputeFCS(logicalNameCmd);
                Buffer.BlockCopy(objerror.FCS, 0, logicalNameCmd, hdlc.Length + plainText.Length, objerror.FCS.Length);
                Buffer.BlockCopy(hdlc, 0, logicalNameCmd, hdlc.Length + plainText.Length + objerror.FCS.Length, 1);

                string finalCommand = String.Concat(logicalNameCmd.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus(" Public Client LogicalName : " + finalCommand);
                MUTSerialCOM.Send(logicalNameCmd);
            }
            catch { }
        }
        private void ReadInvocation()
        {
            try
            {
                byte[] ReadInvocationCmd = new byte[27];
                byte[] plainText;
                byte[] hdlc = FramePublicHDLC(0x19, objControlByte.LogicalCByte);
                Buffer.BlockCopy(hdlc, 0, ReadInvocationCmd, 0, hdlc.Length);
                plainText = CreatePlainText(objTag.GetRequest, 0x02, objcls.Class1, objOBIS.InvocationOBIS);
                Buffer.BlockCopy(plainText, 0, ReadInvocationCmd, hdlc.Length, plainText.Length);
                objerror.FCS = FCS.ComputeFCS(ReadInvocationCmd);
                Buffer.BlockCopy(objerror.FCS, 0, ReadInvocationCmd, hdlc.Length + plainText.Length, objerror.FCS.Length);
                Buffer.BlockCopy(hdlc, 0, ReadInvocationCmd, hdlc.Length + plainText.Length + objerror.FCS.Length, 1);
                string finalCommand = String.Concat(ReadInvocationCmd.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus(" Invocation Counter : " + finalCommand);
                MUTSerialCOM.Send(ReadInvocationCmd);
            }
            catch { }
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
                Buffer.BlockCopy(objAARQ.AARQPlainText, 3, DedicatedKey, 0, 16); string test = settings.GlobalKey;
                byte[] _FinalNonce = FrameNonce(securityKey.CNonce, invocationCounter, securityKey.CNonce.Length);
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

                securityKey.CNonce.CopyTo(objAARQ.AARQCommandBytes, len);
                len += securityKey.CNonce.Length;


                objAARQ.AARQFixedBlock3.CopyTo(objAARQ.AARQCommandBytes, len);
                len += objAARQ.AARQFixedBlock3.Length;

                objAARQ.AARQFixedBlock4.CopyTo(objAARQ.AARQCommandBytes, len);
                len += objAARQ.AARQFixedBlock4.Length;

                objAARQ.AARQFixedBlock5.CopyTo(objAARQ.AARQCommandBytes, len);
                len += objAARQ.AARQFixedBlock5.Length;

                invocationCounter.CopyTo(objAARQ.AARQCommandBytes, len);
                len += invocationCounter.Length;

                encryptData.CipheredData.CopyTo(objAARQ.AARQCommandBytes, len);
                len += encryptData.CipheredData.Length;

                encryptData.AuthenticationTag.CopyTo(objAARQ.AARQCommandBytes, len);
                len += encryptData.AuthenticationTag.Length;

                errorDetection.FCS = FCS.ComputeFCS(objAARQ.AARQCommandBytes);
                errorDetection.FCS.CopyTo(objAARQ.AARQCommandBytes, len);

                objAARQ.AARQCommandBytes[objAARQ.AARQCommandBytes.Length - 1] = SAEChar[0];

                string _aarq = String.Concat(objAARQ.AARQCommandBytes.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("AARQ Request: " + _aarq);
                MUTSerialCOM.Send(objAARQ.AARQCommandBytes);
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
                invocationCounter = IterateInvocation(invocationCounter);

                byte[] _FinalNonce = FrameNonce(securityKey.CNonce, invocationCounter, securityKey.CNonce.Length);
                byte[] _AAD = FrameAAD(securityKey.PasswordKey);

                Buffer.BlockCopy(Encrypt_StoC, 0, objAction.ActPlainText, 15, 16);
                string PlainStr = String.Concat(objAction.ActPlainText.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("\nBefore Action CIPHER : " + PlainStr + Environment.NewLine);

                byte[] bResult = AESGCM.SimpleEncrypt(objAction.ActPlainText, securityKey.PasswordKey, _FinalNonce, _AAD);
                string CipheredText = String.Concat(bResult.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("\nAfter Action CIPHER : " + CipheredText + Environment.NewLine);

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
                invocationCounter.CopyTo(FinalCmd, len);
                len += invocationCounter.Length;
                encryptData.CipheredData.CopyTo(FinalCmd, len);
                len += encryptData.CipheredData.Length;
                encryptData.AuthenticationTag.CopyTo(FinalCmd, len);
                len += encryptData.AuthenticationTag.Length;

                errorDetection.FCS = FCS.ComputeFCS(FinalCmd);
                errorDetection.FCS.CopyTo(FinalCmd, len);

                FinalCmd[FinalCmd.Length - 1] = SAEChar[0];

                string finalCommand = String.Concat(FinalCmd.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("\nACTION REQUEST : " + finalCommand + Environment.NewLine);
                MUTSerialCOM.Send(FinalCmd); invocationCounter = IterateInvocation(invocationCounter);// Thread.Sleep(500);
            }
            catch (Exception ex)
            {
            }
        }
        private void SNOLogicalName()
        {
            SecurityKeys securityKeys = new SecurityKeys();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();
            SerialNumber serialNumber = new SerialNumber();
            try
            {
                byte[] invocationCounter1 = { 0x00, 0x00, 0x00, 0x00 };
                byte[] FinalCmd;

                Buffer.BlockCopy(serialNumber.PlainText, 0, serialNumber.SPlainText, 0, 11);
                serialNumber.SPlainText[11] = 0x01;
                serialNumber.SPlainText[12] = 0x00;

                byte[] _FinalNonce = FrameNonce(securityKeys.CNonce, invocationCounter1, securityKeys.CNonce.Length);
                byte[] _AAD = FrameAAD(securityKeys.PasswordKey);
                _CipheredData = new byte[13];

                byte[] bResult = AESGCM.SimpleEncrypt(serialNumber.SPlainText, DedicatedKey, _FinalNonce, _AAD);

                string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("\nSerial No CIPHER : " + ss + Environment.NewLine);
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
                invocationCounter1.CopyTo(FinalCmd, len);
                len += invocationCounter1.Length;
                encryptData.CipheredData.CopyTo(FinalCmd, len);
                len += encryptData.CipheredData.Length;
                encryptData.AuthenticationTag.CopyTo(FinalCmd, len);
                len += encryptData.AuthenticationTag.Length;

                errorDetection.FCS = FCS.ComputeFCS(FinalCmd);
                errorDetection.FCS.CopyTo(FinalCmd, len);

                FinalCmd[FinalCmd.Length - 1] = SAEChar[0];

                string finalCommand = String.Concat(FinalCmd.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("\nSerial No Logical Name : " + finalCommand + Environment.NewLine);
                MUTSerialCOM.Send(FinalCmd);
            }
            catch (Exception ex)
            {
                ErrorDetection(ReceiveStr);
            }
        }
        private void ReadSerialNo()
        {
            SecurityKeys securityKeys = new SecurityKeys();
            ReadSerialNo serialNumber = new ReadSerialNo();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();
            try
            {
                byte[] invocationCounterReset = { 0x00, 0x00, 0x00, 0x00 };
                Buffer.BlockCopy(serialNumber.PlainText, 0, serialNumber.SPlainText, 0, 11);
                serialNumber.SPlainText[11] = 0x02;
                serialNumber.SPlainText[12] = 0x00;

                byte[] FinalCmd;
                try
                {
                    byte[] _FinalNonce = FrameNonce(securityKeys.CNonce, invocationCounterReset, securityKeys.CNonce.Length);
                    byte[] _AAD = FrameAAD(securityKeys.PasswordKey);
                    _CipheredData = new byte[13];

                    byte[] bResult = AESGCM.SimpleEncrypt(serialNumber.SPlainText, DedicatedKey, _FinalNonce, _AAD);

                    string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));
                    UpdateLogStatus("\nS.No REQUEST CIPHER : " + ss + Environment.NewLine);
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
                    invocationCounterReset.CopyTo(FinalCmd, len);
                    len += invocationCounterReset.Length;
                    encryptData.CipheredData.CopyTo(FinalCmd, len);
                    len += encryptData.CipheredData.Length;
                    encryptData.AuthenticationTag.CopyTo(FinalCmd, len);
                    len += encryptData.AuthenticationTag.Length;

                    errorDetection.FCS = FCS.ComputeFCS(FinalCmd);
                    errorDetection.FCS.CopyTo(FinalCmd, len);

                    FinalCmd[FinalCmd.Length - 1] = SAEChar[0];

                    string finalCommand = String.Concat(FinalCmd.Select(b => b.ToString("X2") + " "));
                    UpdateLogStatus("\n Serial No Request : " + finalCommand + Environment.NewLine);
                    MUTSerialCOM.Send(FinalCmd);

                }
                catch (Exception ex)
                {
                    ErrorDetection(ReceiveStr);
                }
            }
            catch (Exception ex) { };
        }
        private void GetCalibValueRead()
        {
            try
            {
                CalibGetRequset calibGetRequset = new CalibGetRequset();
                SecurityKeys securityKey = new SecurityKeys();
                ProtocolFlags protocolFlags = new ProtocolFlags();
                EncryptData encryptData = new EncryptData();
                ErrorDetection errorDetection = new ErrorDetection();

                byte[] invocationCounter = { 0x00, 0x00, 0x00, 0x00 };
                // invocationCounter = IterateInvocation(invocationCounter);
                byte[] bInstantData;
                byte[] FinalCmd;
                Buffer.BlockCopy(calibGetRequset.PlainText, 0, calibGetRequset.OBISPlainText, 0, 11);
                calibGetRequset.OBISPlainText[11] = 0x02;
                calibGetRequset.OBISPlainText[12] = 0x00;


                byte[] _FinalNonce = FrameNonce(securityKey.CNonce, invocationCounter, securityKey.CNonce.Length);
                byte[] _AAD = FrameAAD(securityKey.PasswordKey);
                _CipheredData = new byte[13];

                byte[] bResult = AESGCM.SimpleEncrypt(calibGetRequset.OBISPlainText, DedicatedKey, _FinalNonce, _AAD);

                string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));
                //this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\nDATA REQUEST CIPHER : " + ss + Environment.NewLine)));
                UpdateLogStatus("\nDATA Read Calib REQUEST CIPHER : " + ss + Environment.NewLine);

                Buffer.BlockCopy(bResult, 0, _CipheredData, 0, 13);
                Buffer.BlockCopy(bResult, 13, _AuthenticationTag, 0, 12);

                //---------------- Frame the Data Request 2(RTC DATA) Send Request ----------------------------// 


                byte[] SAEChar = protocolFlags.StartFlag;
                byte[] FrameFormat = securityKey.HDLCFrame_Format;
                byte[] _HDLCLength = calibGetRequset.CalibReadHDLCLength;
                encryptData.CipheredData = _CipheredData;
                encryptData.AuthenticationTag = _AuthenticationTag;

                //---------------Final RTC DATA Command ------------------//

                FinalCmd = new byte[_HDLCLength[0] + 2];
                FinalCmd[0] = SAEChar[0];
                FinalCmd[1] = FrameFormat[0];
                FinalCmd[2] = _HDLCLength[0];
                calibGetRequset.CalibReadFixedBytes.CopyTo(FinalCmd, 3);

                int len = protocolFlags.StartFlag.Length + securityKey.HDLCFrame_Format.Length +
                          calibGetRequset.CalibReadHDLCLength.Length + calibGetRequset.CalibReadFixedBytes.Length;

                calibGetRequset.CalibReadRequestBlock.CopyTo(FinalCmd, len);
                len += calibGetRequset.CalibReadRequestBlock.Length;
                invocationCounter.CopyTo(FinalCmd, len);
                len += invocationCounter.Length;
                encryptData.CipheredData.CopyTo(FinalCmd, len);
                len += encryptData.CipheredData.Length;
                encryptData.AuthenticationTag.CopyTo(FinalCmd, len);
                len += encryptData.AuthenticationTag.Length;

                errorDetection.FCS = FCS.ComputeFCS(FinalCmd);
                errorDetection.FCS.CopyTo(FinalCmd, len);

                FinalCmd[FinalCmd.Length - 1] = SAEChar[0];

                string finalCommand = String.Concat(FinalCmd.Select(b => b.ToString("X2") + " "));
                // this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\n RTC Request : " + finalCommand + Environment.NewLine)));
                UpdateLogStatus("Read Calib Const Request: " + finalCommand + Environment.NewLine);
                MUTSerialCOM.Send(FinalCmd); //Thread.Sleep(500);

            }
            catch (Exception ex)
            {

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
                byte[] invocationCounterReset = { 0x00, 0x00, 0x00, 0x01 };

                byte[] FinalCmd;
                Buffer.BlockCopy(objDataReq.PlainText, 0, objDataReq.OBISPlainText, 0, 11);
                objDataReq.OBISPlainText[11] = 0x02;
                objDataReq.OBISPlainText[12] = 0x00;
                try
                {
                    byte[] _FinalNonce = FrameNonce(securityKey.CNonce, invocationCounterReset, securityKey.CNonce.Length);
                    byte[] _AAD = FrameAAD(securityKey.PasswordKey);
                    _CipheredData = new byte[13];

                    byte[] bResult = AESGCM.SimpleEncrypt(objDataReq.OBISPlainText, DedicatedKey, _FinalNonce, _AAD);

                    string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));
                    UpdateLogStatus("\nDATA REQUEST CIPHER : " + ss + Environment.NewLine);
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
                    invocationCounterReset.CopyTo(FinalCmd, len);
                    len += invocationCounterReset.Length;
                    encryptData.CipheredData.CopyTo(FinalCmd, len);
                    len += encryptData.CipheredData.Length;
                    encryptData.AuthenticationTag.CopyTo(FinalCmd, len);
                    len += encryptData.AuthenticationTag.Length;

                    errorDetection.FCS = FCS.ComputeFCS(FinalCmd);
                    errorDetection.FCS.CopyTo(FinalCmd, len);

                    FinalCmd[FinalCmd.Length - 1] = SAEChar[0];

                    string finalCommand = String.Concat(FinalCmd.Select(b => b.ToString("X2") + " "));
                    UpdateLogStatus("RTC Request: " + finalCommand + Environment.NewLine);
                    MUTSerialCOM.Send(FinalCmd);/*CmdType = CMD_DISCONNECT;requestType = REQ_DEFAULTDISC;*/
                }
                catch (Exception ex)
                {
                    ErrorDetection(ReceiveStr);
                }
            }
            catch { };
        }
        private void SETStartFunctionValue()
        {
            try
            {
                byte[] startValue = new byte[2] { 0x1B, 0x59 };
                FunctionalTest(startValue);
            }
            catch (Exception ex) { throw ex; }

        }
        private void FunctionalTest(byte[] value)
        {
            SecurityKeys securityKeys = new SecurityKeys();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();

            try
            {
                // if (StopStatus == true) { invocationCounterReset = new byte[] { 0x00, 0x00, 0x00, 0x00 }; }
                /* else { */
                invocationCounterReset = GetNextInvocation();/* }*/
                byte[] FPlainText = new byte[] { 0xC1, 0x01, 0xC1, 0x00, 0x01, 0x01, 0x00, 0x60, 0x02, 0x80, 0x00, 0x02, 0x00, 0x12, 0x1B, 0x58 };
                Buffer.BlockCopy(value, 0, FPlainText, 14, value.Length);
                byte[] FinalCmd;
                try
                {
                    byte[] _FinalNonce = FrameNonce(securityKeys.CNonce, invocationCounterReset, securityKeys.CNonce.Length);
                    byte[] _AAD = FrameAAD(securityKeys.PasswordKey);
                    _CipheredData = new byte[16];

                    byte[] bResult = AESGCM.SimpleEncrypt(FPlainText, DedicatedKey, _FinalNonce, _AAD);

                    string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));
                    this.LogBox.Invoke((MethodInvoker)(() => LogBox.AppendText(Environment.NewLine + "\n FUNCTION Ciphered : " + ss + Environment.NewLine)));
                    Buffer.BlockCopy(bResult, 0, _CipheredData, 0, 16);
                    Buffer.BlockCopy(bResult, 16, _AuthenticationTag, 0, 12);


                    byte[] SAEChar = protocolFlags.StartFlag;
                    byte[] FrameFormat = securityKeys.HDLCFrame_Format;
                    byte[] _HDLCLength = { 0x2F };
                    byte[] FixedBytes = new byte[8] { 0x03, 0x61, 0x54, 0x51, 0x9F, 0xE6, 0xE6, 0x00 };
                    if (StopStatus == true) { FixedBytes = new byte[8] { 0x03, 0x61, 0x54, 0x51, 0x9F, 0xE6, 0xE6, 0x00 }; }
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
                    invocationCounterReset.CopyTo(FinalCmd, len);
                    len += invocationCounterReset.Length;
                    encryptData.CipheredData.CopyTo(FinalCmd, len);
                    len += encryptData.CipheredData.Length;
                    encryptData.AuthenticationTag.CopyTo(FinalCmd, len);
                    len += encryptData.AuthenticationTag.Length;

                    errorDetection.FCS = FCS.ComputeFCS(FinalCmd);
                    errorDetection.FCS.CopyTo(FinalCmd, len);

                    FinalCmd[FinalCmd.Length - 1] = SAEChar[0];

                    string finalCommand = String.Concat(FinalCmd.Select(b => b.ToString("X2") + " "));
                    this.LogBox.Invoke((MethodInvoker)(() => LogBox.AppendText(Environment.NewLine + "\n FUNCTIONAL Test Request : " + finalCommand + Environment.NewLine)));
                    MUTSerialCOM.Send(FinalCmd);
                }
                catch (Exception ex)
                {
                    ErrorDetection(ReceiveStr);
                }
            }
            catch (Exception ex)
            {
                ErrorDetection(ReceiveStr);
            }
        }
        private void SETStopFunstionalTest()
        {
            try
            {
                byte[] stopValue = new byte[2] { 0x1B, 0x5A };
                FunctionalTest(stopValue);
            }
            catch (Exception ex) { throw ex; }
        }
        private void GetFunctionalTestContinuosly(byte[] clsId, byte[] obisCode)
        {
            FunctionalTestLogicalName functionalTest = new FunctionalTestLogicalName();
            ReadFunctionalTest readFunctional = new ReadFunctionalTest();
            SecurityKeys securityKey = new SecurityKeys();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();
            try
            {
                invocationCounterReset = GetNextInvocation();
                byte[] FinalCmd;
                Buffer.BlockCopy(clsId, 0, functionalTest.PlainText, 3, 2);
                Buffer.BlockCopy(obisCode, 0, functionalTest.PlainText, 5, 6);

                functionalTest.PlainText[11] = 0x02;

                byte[] _FinalNonce = FrameNonce(securityKey.CNonce, invocationCounterReset, securityKey.CNonce.Length);
                byte[] _AAD = FrameAAD(securityKey.PasswordKey);
                _CipheredData = new byte[13];

                byte[] bResult = AESGCM.SimpleEncrypt(functionalTest.PlainText, DedicatedKey, _FinalNonce, _AAD);

                string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));
                this.LogBox.Invoke((MethodInvoker)(() => LogBox.AppendText(Environment.NewLine + "\nFUNCTIONAL TEST CIPHER : " + ss + Environment.NewLine)));
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

                //---------------Continuos Sequence-------------------//

                byte[] arraySequence = { 0x93, 0x10, 0x32, 0x54, 0x76, 0x98, 0xBA, 0xDC, 0xFE };

                if (cmdSequence == 0) { ReadDataFixedBytes = new byte[] { 0x03, 0x61, 0x76, 0x8C, 0xB8, 0xE6, 0xE6, 0x00 }; }
                else
                {
                    ReadDataFixedBytes = new byte[6];
                    ReadDataFixedBytes[0] = objHDLC.Flag; ReadDataFixedBytes[1] = objHDLC.FrameFormat;
                    ReadDataFixedBytes[2] = _HDLCLength[0];
                    ReadDataFixedBytes[3] = 0x03; ReadDataFixedBytes[4] = 0x61;
                    ReadDataFixedBytes[5] = arraySequence[seqIncDecCount];

                    byte[] _hcs = FCS.ComputeHCSContinuos(ReadDataFixedBytes);

                    ReadDataFixedBytes = new byte[8];
                    ReadDataFixedBytes[0] = 0x03; ReadDataFixedBytes[1] = 0x61;
                    ReadDataFixedBytes[2] = arraySequence[seqIncDecCount];
                    ReadDataFixedBytes[3] = _hcs[0]; ReadDataFixedBytes[4] = _hcs[1];

                    ReadDataFixedBytes[5] = 0xE6; ReadDataFixedBytes[6] = 0xE6; ReadDataFixedBytes[7] = 0x00;

                    if (ReadDataFixedBytes[2] == 254)
                    { seqIncDecCount = 0; }
                }

                ReadDataFixedBytes.CopyTo(FinalCmd, 3);

                int len = protocolFlags.StartFlag.Length + securityKey.HDLCFrame_Format.Length +
                          functionalTest.HDLCLen.Length + ReadDataFixedBytes.Length;

                functionalTest.Header.CopyTo(FinalCmd, len);
                len += functionalTest.Header.Length;
                invocationCounterReset.CopyTo(FinalCmd, len);
                len += invocationCounterReset.Length;
                encryptData.CipheredData.CopyTo(FinalCmd, len);
                len += encryptData.CipheredData.Length;
                encryptData.AuthenticationTag.CopyTo(FinalCmd, len);
                len += encryptData.AuthenticationTag.Length;

                errorDetection.FCS = FCS.ComputeFCS(FinalCmd);
                errorDetection.FCS.CopyTo(FinalCmd, len);

                FinalCmd[FinalCmd.Length - 1] = SAEChar[0];

                string finalCommand = String.Concat(FinalCmd.Select(b => b.ToString("X2") + " "));
                this.LogBox.Invoke((MethodInvoker)(() => LogBox.AppendText(Environment.NewLine + "\nFUNCTIONAL OBIS CODE : " + finalCommand + Environment.NewLine)));
                cmdSequence = 1; seqIncDecCount = seqIncDecCount + 1;
                MUTSerialCOM.Send(FinalCmd);
            }
            catch (Exception ex)
            {
                ErrorDetection(ReceiveStr);
            }
        }
        private void DiscCmdSend()
        {
            DisconnectCmd = comm.PublicClientDisconnect();
            string Disconnect = String.Concat(DisconnectCmd.Select(b => b.ToString("X2") + " "));
            UpdateLogStatus("Disconnect : " + Disconnect);
            MUTSerialCOM.Send(DisconnectCmd); /*MainCmdSendFlag = true;*/
        }
        private void GetInstVoltLogicalName()
        {
            SecurityKeys securityKeys = new SecurityKeys();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();
            InstantVoltLogicalName instantVoltLogicalName = new InstantVoltLogicalName();

            // SerialNumber serialNumber = new SerialNumber();
            try
            {
                invocationCounterReset = new byte[] { 0x00, 0x00, 0x00, 0x02 };
                byte[] FinalCmd;
                Buffer.BlockCopy(instantVoltLogicalName.PlainText, 0, instantVoltLogicalName.PlainText, 0, 11);
                //instantVoltLogicalName.PlainText[11] = 0x01;
                //instantVoltLogicalName.PlainText[12] = 0x00;

                byte[] _FinalNonce = FrameNonce(securityKeys.CNonce, invocationCounterReset, securityKeys.CNonce.Length);
                byte[] _AAD = FrameAAD(securityKeys.PasswordKey);
                _CipheredData = new byte[13];

                byte[] bResult = AESGCM.SimpleEncrypt(instantVoltLogicalName.PlainText, DedicatedKey, _FinalNonce, _AAD);

                string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("\nInstant Volt Value CIPHER : " + ss + Environment.NewLine);
                Buffer.BlockCopy(bResult, 0, _CipheredData, 0, 13);
                Buffer.BlockCopy(bResult, 13, _AuthenticationTag, 0, 12);

                //---------------- Frame the Data Request 1(OBIS Code) Send Request ----------------------------// 

                byte[] SAEChar = protocolFlags.StartFlag;
                byte[] FrameFormat = securityKeys.HDLCFrame_Format;
                byte[] _HDLCLength = instantVoltLogicalName.HDLCLen;
                encryptData.CipheredData = _CipheredData;
                encryptData.AuthenticationTag = _AuthenticationTag;

                //---------------Final OBIS Code Command ------------------//

                FinalCmd = new byte[_HDLCLength[0] + 2];
                FinalCmd[0] = SAEChar[0];
                FinalCmd[1] = FrameFormat[0];
                FinalCmd[2] = _HDLCLength[0];
                instantVoltLogicalName.FixedBytes.CopyTo(FinalCmd, 3);

                int len = protocolFlags.StartFlag.Length + securityKeys.HDLCFrame_Format.Length +
                              instantVoltLogicalName.HDLCLen.Length + instantVoltLogicalName.FixedBytes.Length;

                instantVoltLogicalName.RequestBlock.CopyTo(FinalCmd, len);
                len += instantVoltLogicalName.RequestBlock.Length;
                invocationCounterReset.CopyTo(FinalCmd, len);
                len += invocationCounterReset.Length;

                encryptData.CipheredData.CopyTo(FinalCmd, len);
                len += encryptData.CipheredData.Length;
                encryptData.AuthenticationTag.CopyTo(FinalCmd, len);
                len += encryptData.AuthenticationTag.Length;

                errorDetection.FCS = FCS.ComputeFCS(FinalCmd);
                errorDetection.FCS.CopyTo(FinalCmd, len);

                FinalCmd[FinalCmd.Length - 1] = SAEChar[0];

                string finalCommand = String.Concat(FinalCmd.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("\n Voltage Request : " + finalCommand + Environment.NewLine);
                MUTSerialCOM.Send(FinalCmd);
            }
            catch (Exception ex)
            {

            }
        }
        private void DiscCmdSend_US()
        {
            DisconnectCmd = new byte[] { 0x7E, 0xA0, 0x07, 0x03, 0x61, 0x53, 0x65, 0x81, 0x7E };
            string Disconnect = String.Concat(DisconnectCmd.Select(b => b.ToString("X2") + " "));
            UpdateLogStatus("Disconnect : " + Disconnect);
            MUTSerialCOM.Send(DisconnectCmd); MainCmdSendFlag = false;
        }
        private void GetInstVoltScalarUnit()
        {
            SecurityKeys securityKeys = new SecurityKeys();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();
            VoltScalarUnit voltScalarUnit = new VoltScalarUnit();
            try
            {
                byte[] FinalCmd; invocationCounterReset = GetNextInvocation();
                Buffer.BlockCopy(voltScalarUnit.PlainText, 0, voltScalarUnit.PlainText, 0, 11);
                byte[] _FinalNonce = FrameNonce(securityKeys.CNonce, invocationCounterReset, securityKeys.CNonce.Length);
                byte[] _AAD = FrameAAD(securityKeys.PasswordKey);
                _CipheredData = new byte[13];

                byte[] bResult = AESGCM.SimpleEncrypt(voltScalarUnit.PlainText, DedicatedKey, _FinalNonce, _AAD);

                string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("\nInstant Volt Scalar CIPHER : " + ss + Environment.NewLine);
                Buffer.BlockCopy(bResult, 0, _CipheredData, 0, 13);
                Buffer.BlockCopy(bResult, 13, _AuthenticationTag, 0, 12);

                //---------------- Frame the Data Request 1(OBIS Code) Send Request ----------------------------// 

                byte[] SAEChar = protocolFlags.StartFlag;
                byte[] FrameFormat = securityKeys.HDLCFrame_Format;
                byte[] _HDLCLength = voltScalarUnit.HDLCLen;
                encryptData.CipheredData = _CipheredData;
                encryptData.AuthenticationTag = _AuthenticationTag;

                //---------------Final OBIS Code Command ------------------//

                FinalCmd = new byte[_HDLCLength[0] + 2];
                FinalCmd[0] = SAEChar[0];
                FinalCmd[1] = FrameFormat[0];
                FinalCmd[2] = _HDLCLength[0];
                voltScalarUnit.FixedBytes.CopyTo(FinalCmd, 3);

                int len = protocolFlags.StartFlag.Length + securityKeys.HDLCFrame_Format.Length +
                              voltScalarUnit.HDLCLen.Length + voltScalarUnit.FixedBytes.Length;

                voltScalarUnit.RequestBlock.CopyTo(FinalCmd, len);
                len += voltScalarUnit.RequestBlock.Length;
                invocationCounterReset.CopyTo(FinalCmd, len);
                len += invocationCounterReset.Length;

                encryptData.CipheredData.CopyTo(FinalCmd, len);
                len += encryptData.CipheredData.Length;
                encryptData.AuthenticationTag.CopyTo(FinalCmd, len);
                len += encryptData.AuthenticationTag.Length;

                errorDetection.FCS = FCS.ComputeFCS(FinalCmd);
                errorDetection.FCS.CopyTo(FinalCmd, len);

                FinalCmd[FinalCmd.Length - 1] = SAEChar[0];

                string finalCommand = String.Concat(FinalCmd.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("\n Voltage Scalar Request : " + finalCommand + Environment.NewLine);
                MUTSerialCOM.Send(FinalCmd);
            }
            catch (Exception ex)
            {

            }
        }
        private void GetInstCurrLogicalName()
        {
            SecurityKeys securityKeys = new SecurityKeys();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();
            InstantCurrLogicalName instantCurrLogicalName = new InstantCurrLogicalName();

            // SerialNumber serialNumber = new SerialNumber();
            try
            {
                // byte[] invocationCounter1 = { 0x00, 0x00, 0x00, 0x00 };
                byte[] FinalCmd; invocationCounterReset = GetNextInvocation();
                Buffer.BlockCopy(instantCurrLogicalName.PlainText, 0, instantCurrLogicalName.PlainText, 0, 11);
                //instantVoltLogicalName.PlainText[11] = 0x01;
                //instantVoltLogicalName.PlainText[12] = 0x00;

                byte[] _FinalNonce = FrameNonce(securityKeys.CNonce, invocationCounterReset, securityKeys.CNonce.Length);
                byte[] _AAD = FrameAAD(securityKeys.PasswordKey);
                _CipheredData = new byte[13];

                byte[] bResult = AESGCM.SimpleEncrypt(instantCurrLogicalName.PlainText, DedicatedKey, _FinalNonce, _AAD);

                string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("\nInstant Curr Value CIPHER : " + ss + Environment.NewLine);
                Buffer.BlockCopy(bResult, 0, _CipheredData, 0, 13);
                Buffer.BlockCopy(bResult, 13, _AuthenticationTag, 0, 12);

                //---------------- Frame the Data Request 1(OBIS Code) Send Request ----------------------------// 

                byte[] SAEChar = protocolFlags.StartFlag;
                byte[] FrameFormat = securityKeys.HDLCFrame_Format;
                byte[] _HDLCLength = instantCurrLogicalName.HDLCLen;
                encryptData.CipheredData = _CipheredData;
                encryptData.AuthenticationTag = _AuthenticationTag;

                //---------------Final OBIS Code Command ------------------//

                FinalCmd = new byte[_HDLCLength[0] + 2];
                FinalCmd[0] = SAEChar[0];
                FinalCmd[1] = FrameFormat[0];
                FinalCmd[2] = _HDLCLength[0];
                instantCurrLogicalName.FixedBytes.CopyTo(FinalCmd, 3);

                int len = protocolFlags.StartFlag.Length + securityKeys.HDLCFrame_Format.Length +
                              instantCurrLogicalName.HDLCLen.Length + instantCurrLogicalName.FixedBytes.Length;

                instantCurrLogicalName.RequestBlock.CopyTo(FinalCmd, len);
                len += instantCurrLogicalName.RequestBlock.Length;
                invocationCounterReset.CopyTo(FinalCmd, len);
                len += invocationCounterReset.Length;

                encryptData.CipheredData.CopyTo(FinalCmd, len);
                len += encryptData.CipheredData.Length;
                encryptData.AuthenticationTag.CopyTo(FinalCmd, len);
                len += encryptData.AuthenticationTag.Length;

                errorDetection.FCS = FCS.ComputeFCS(FinalCmd);
                errorDetection.FCS.CopyTo(FinalCmd, len);

                FinalCmd[FinalCmd.Length - 1] = SAEChar[0];

                string finalCommand = String.Concat(FinalCmd.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("\n Current Request : " + finalCommand + Environment.NewLine);
                MUTSerialCOM.Send(FinalCmd);
            }
            catch (Exception ex)
            {

            }
        }
        private void GetInstCurrScalarUnit()
        {
            SecurityKeys securityKeys = new SecurityKeys();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();
            CurrScalarUnit currScalarUnit = new CurrScalarUnit();
            try
            {
                byte[] FinalCmd; invocationCounterReset = GetNextInvocation();//GetNextInvocation();
                Buffer.BlockCopy(currScalarUnit.PlainText, 0, currScalarUnit.PlainText, 0, 11);
                byte[] _FinalNonce = FrameNonce(securityKeys.CNonce, invocationCounterReset, securityKeys.CNonce.Length);
                byte[] _AAD = FrameAAD(securityKeys.PasswordKey);
                _CipheredData = new byte[13];

                byte[] bResult = AESGCM.SimpleEncrypt(currScalarUnit.PlainText, DedicatedKey, _FinalNonce, _AAD);

                string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("\nInstant Curr Scalar CIPHER : " + ss + Environment.NewLine);
                Buffer.BlockCopy(bResult, 0, _CipheredData, 0, 13);
                Buffer.BlockCopy(bResult, 13, _AuthenticationTag, 0, 12);

                //---------------- Frame the Data Request 1(OBIS Code) Send Request ----------------------------// 

                byte[] SAEChar = protocolFlags.StartFlag;
                byte[] FrameFormat = securityKeys.HDLCFrame_Format;
                byte[] _HDLCLength = currScalarUnit.HDLCLen;
                encryptData.CipheredData = _CipheredData;
                encryptData.AuthenticationTag = _AuthenticationTag;

                //---------------Final OBIS Code Command ------------------//

                FinalCmd = new byte[_HDLCLength[0] + 2];
                FinalCmd[0] = SAEChar[0];
                FinalCmd[1] = FrameFormat[0];
                FinalCmd[2] = _HDLCLength[0];
                currScalarUnit.FixedBytes.CopyTo(FinalCmd, 3);

                int len = protocolFlags.StartFlag.Length + securityKeys.HDLCFrame_Format.Length +
                              currScalarUnit.HDLCLen.Length + currScalarUnit.FixedBytes.Length;

                currScalarUnit.RequestBlock.CopyTo(FinalCmd, len);
                len += currScalarUnit.RequestBlock.Length;
                invocationCounterReset.CopyTo(FinalCmd, len);
                len += invocationCounterReset.Length;

                encryptData.CipheredData.CopyTo(FinalCmd, len);
                len += encryptData.CipheredData.Length;
                encryptData.AuthenticationTag.CopyTo(FinalCmd, len);
                len += encryptData.AuthenticationTag.Length;

                errorDetection.FCS = FCS.ComputeFCS(FinalCmd);
                errorDetection.FCS.CopyTo(FinalCmd, len);

                FinalCmd[FinalCmd.Length - 1] = SAEChar[0];

                string finalCommand = String.Concat(FinalCmd.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("\n Current Scalar Request : " + finalCommand + Environment.NewLine);
                MUTSerialCOM.Send(FinalCmd);
            }
            catch (Exception ex)
            {

            }
        }
        private void GetInstNeutralCurrLogicalName()
        {
            SecurityKeys securityKeys = new SecurityKeys();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();
            InstantNeutralCurrLogicalName instantNeutralCurrLogicalName = new InstantNeutralCurrLogicalName();
            try
            {
                byte[] FinalCmd; invocationCounterReset = GetNextInvocation();
                Buffer.BlockCopy(instantNeutralCurrLogicalName.PlainText, 0, instantNeutralCurrLogicalName.PlainText, 0, 11);

                byte[] _FinalNonce = FrameNonce(securityKeys.CNonce, invocationCounterReset, securityKeys.CNonce.Length);
                byte[] _AAD = FrameAAD(securityKeys.PasswordKey);
                _CipheredData = new byte[13];

                byte[] bResult = AESGCM.SimpleEncrypt(instantNeutralCurrLogicalName.PlainText, DedicatedKey, _FinalNonce, _AAD);

                string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("\nInstant Neutral Curr Value CIPHER : " + ss + Environment.NewLine);
                Buffer.BlockCopy(bResult, 0, _CipheredData, 0, 13);
                Buffer.BlockCopy(bResult, 13, _AuthenticationTag, 0, 12);

                //---------------- Frame the Data Request 1(OBIS Code) Send Request ----------------------------// 

                byte[] SAEChar = protocolFlags.StartFlag;
                byte[] FrameFormat = securityKeys.HDLCFrame_Format;
                byte[] _HDLCLength = instantNeutralCurrLogicalName.HDLCLen;
                encryptData.CipheredData = _CipheredData;
                encryptData.AuthenticationTag = _AuthenticationTag;

                //---------------Final OBIS Code Command ------------------//

                FinalCmd = new byte[_HDLCLength[0] + 2];
                FinalCmd[0] = SAEChar[0];
                FinalCmd[1] = FrameFormat[0];
                FinalCmd[2] = _HDLCLength[0];
                instantNeutralCurrLogicalName.FixedBytes.CopyTo(FinalCmd, 3);

                int len = protocolFlags.StartFlag.Length + securityKeys.HDLCFrame_Format.Length +
                              instantNeutralCurrLogicalName.HDLCLen.Length + instantNeutralCurrLogicalName.FixedBytes.Length;

                instantNeutralCurrLogicalName.RequestBlock.CopyTo(FinalCmd, len);
                len += instantNeutralCurrLogicalName.RequestBlock.Length;
                invocationCounterReset.CopyTo(FinalCmd, len);
                len += invocationCounterReset.Length;

                encryptData.CipheredData.CopyTo(FinalCmd, len);
                len += encryptData.CipheredData.Length;
                encryptData.AuthenticationTag.CopyTo(FinalCmd, len);
                len += encryptData.AuthenticationTag.Length;

                errorDetection.FCS = FCS.ComputeFCS(FinalCmd);
                errorDetection.FCS.CopyTo(FinalCmd, len);

                FinalCmd[FinalCmd.Length - 1] = SAEChar[0];

                string finalCommand = String.Concat(FinalCmd.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("\n Neutral Current Request : " + finalCommand + Environment.NewLine);
                MUTSerialCOM.Send(FinalCmd);
            }
            catch (Exception ex)
            {

            }
        }
        private void GetInstNeutralCurrScalarUnit()
        {
            SecurityKeys securityKeys = new SecurityKeys();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();
            NeutralCurrScalarUnit neutralCurrScalarUnit = new NeutralCurrScalarUnit();
            try
            {
                byte[] FinalCmd; invocationCounterReset = GetNextInvocation();
                Buffer.BlockCopy(neutralCurrScalarUnit.PlainText, 0, neutralCurrScalarUnit.PlainText, 0, 11);
                byte[] _FinalNonce = FrameNonce(securityKeys.CNonce, invocationCounterReset, securityKeys.CNonce.Length);
                byte[] _AAD = FrameAAD(securityKeys.PasswordKey);
                _CipheredData = new byte[13];

                byte[] bResult = AESGCM.SimpleEncrypt(neutralCurrScalarUnit.PlainText, DedicatedKey, _FinalNonce, _AAD);

                string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("\nInstant Neutral Curr Scalar CIPHER : " + ss + Environment.NewLine);
                Buffer.BlockCopy(bResult, 0, _CipheredData, 0, 13);
                Buffer.BlockCopy(bResult, 13, _AuthenticationTag, 0, 12);

                //---------------- Frame the Data Request 1(OBIS Code) Send Request ----------------------------// 

                byte[] SAEChar = protocolFlags.StartFlag;
                byte[] FrameFormat = securityKeys.HDLCFrame_Format;
                byte[] _HDLCLength = neutralCurrScalarUnit.HDLCLen;
                encryptData.CipheredData = _CipheredData;
                encryptData.AuthenticationTag = _AuthenticationTag;

                //---------------Final OBIS Code Command ------------------//

                FinalCmd = new byte[_HDLCLength[0] + 2];
                FinalCmd[0] = SAEChar[0];
                FinalCmd[1] = FrameFormat[0];
                FinalCmd[2] = _HDLCLength[0];
                neutralCurrScalarUnit.FixedBytes.CopyTo(FinalCmd, 3);

                int len = protocolFlags.StartFlag.Length + securityKeys.HDLCFrame_Format.Length +
                              neutralCurrScalarUnit.HDLCLen.Length + neutralCurrScalarUnit.FixedBytes.Length;

                neutralCurrScalarUnit.RequestBlock.CopyTo(FinalCmd, len);
                len += neutralCurrScalarUnit.RequestBlock.Length;
                invocationCounterReset.CopyTo(FinalCmd, len);
                len += invocationCounterReset.Length;

                encryptData.CipheredData.CopyTo(FinalCmd, len);
                len += encryptData.CipheredData.Length;
                encryptData.AuthenticationTag.CopyTo(FinalCmd, len);
                len += encryptData.AuthenticationTag.Length;

                errorDetection.FCS = FCS.ComputeFCS(FinalCmd);
                errorDetection.FCS.CopyTo(FinalCmd, len);

                FinalCmd[FinalCmd.Length - 1] = SAEChar[0];

                string finalCommand = String.Concat(FinalCmd.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("\n Neutral Current Scalar Request : " + finalCommand + Environment.NewLine);
                MUTSerialCOM.Send(FinalCmd);
            }
            catch (Exception ex)
            {

            }
        }
        private void SetCalibValue()
        {
            SecurityKeys securityKey = new SecurityKeys();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();
            try
            {
                byte[] invocationCounterReset = { 0x00, 0x00, 0x00, 0x01 };
                byte[] PlainText = new byte[28];
                byte[] FinalCmd;

                byte[] _FinalNonce = FrameNonce(securityKey.CNonce, invocationCounterReset, securityKey.CNonce.Length);
                byte[] _AAD = FrameAAD(securityKey.PasswordKey);
                _CipheredData = new byte[42];

                string dedicated_Key = String.Concat(DedicatedKey.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("Dedicated Key : " + dedicated_Key);

                byte[] bEncryptValue = AESGCM.SimpleEncrypt(CalibConstWrite, DedicatedKey, _FinalNonce, _AAD);
                string sEncryptWriteVal = String.Concat(bEncryptValue.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("Calib Changed Increment value Encoded : " + sEncryptWriteVal + Environment.NewLine);

                Buffer.BlockCopy(bEncryptValue, 0, _CipheredData, 0, _CipheredData.Length);
                Buffer.BlockCopy(bEncryptValue, 42, _AuthenticationTag, 0, 12);


                byte[] SAEChar = protocolFlags.StartFlag;
                byte[] FrameFormat = securityKey.HDLCFrame_Format;
                byte[] _HDLCLength = { 0x49 };
                byte[] FixedBytes = new byte[8] { 0x03, 0x61, 0x76, 0x3F, 0x4F, 0xE6, 0xE6, 0x00 };
                byte[] ReqBlock = new byte[3] { 0xD1, 0x3B, 0x30 };
                encryptData.CipheredData = _CipheredData;
                encryptData.AuthenticationTag = _AuthenticationTag;

                //---------------Final RTC DATA Command ------------------//

                FinalCmd = new byte[_HDLCLength[0] + 2];
                FinalCmd[0] = SAEChar[0];
                FinalCmd[1] = FrameFormat[0];
                FinalCmd[2] = _HDLCLength[0];
                FixedBytes.CopyTo(FinalCmd, 3);

                int len = protocolFlags.StartFlag.Length + securityKey.HDLCFrame_Format.Length +
                          1 + FixedBytes.Length;

                ReqBlock.CopyTo(FinalCmd, len);
                len += ReqBlock.Length;
                invocationCounterReset.CopyTo(FinalCmd, len);
                len += invocationCounterReset.Length;
                encryptData.CipheredData.CopyTo(FinalCmd, len);
                len += encryptData.CipheredData.Length;
                encryptData.AuthenticationTag.CopyTo(FinalCmd, len);
                len += encryptData.AuthenticationTag.Length;

                errorDetection.FCS = FCS.ComputeFCS(FinalCmd);
                errorDetection.FCS.CopyTo(FinalCmd, len);

                FinalCmd[FinalCmd.Length - 1] = SAEChar[0];

                string finalCommand = String.Concat(FinalCmd.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("Calib const Write Request : " + finalCommand);
                //this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\n Serial Number Write Request : " + finalCommand + Environment.NewLine)));
                MUTSerialCOM.Send(FinalCmd);
            }
            catch (Exception ex) { }
        }
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                StatusTrip.Text = "Ready to Read the data";
                lblCounterIncr.Text = startCount++.ToString();
                requestType = REQ_GET_IC_PC;
                CmdType = CMD_SNRM;
                MainCmdSendFlag = true;
            }
            catch (Exception ex)
            {

            }
        }
        private void FunctionalForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            MUTSerialCOM.Close(); Application.Exit(); this.Hide();
        }
        private void btnLogout_Click(object sender, EventArgs e)
        {
            MUTSerialCOM.Close();
            Application.Exit(); this.Hide();
        }
        private void FunctionalForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MUTSerialCOM.Close(); Application.Exit(); this.Hide();
        }
        private void tmrAutoStop_Tick(object sender, EventArgs e)
        {
            ResetCount = ResetCount - 1;
            lbltmrCnt.Text = ResetCount.ToString();

            if (ResetCount == 0)
            {
                tmrAutoStop.Enabled = false;
                ExportDataGrid();
                StopStatus = true;
            }
        }
    }
}
