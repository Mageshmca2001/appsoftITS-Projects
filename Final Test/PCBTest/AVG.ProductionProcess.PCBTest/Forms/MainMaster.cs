using AVG.ProductionProcess.PCBTest.Commands;
using DLMS_MeterReading;
using Encryption;
using Keller.SPM.ProcotolGeneration.Protocol.Serial;
using Keller.SPM.ProscotolGeneration.Protocol.Serial;
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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Keller.SPM.Communication.MUT;
using static Keller.SPM.Communication.MUT.SerialCOM;

namespace AVG.ProductionProcess.PCBTest.Forms
{

    public partial class MainMaster : Form
    {
        public string ConStr = "Data Source=" + Directory.GetCurrentDirectory() + ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString.ToString();
        StatusKeyClass1 statusKeyClass = new StatusKeyClass1();
        #region Global Objects
        public SerialCOM MUTSerialCOM = new SerialCOM();
        StatusClass statusClass = new StatusClass();
        HDLC objHDLC = new HDLC();
        ErrorDetection objerror = new ErrorDetection();
        ControlBytes objControlByte = new ControlBytes();
        RequestTag objTag = new RequestTag();
        ClassId objcls = new ClassId();
        OBISCode objOBIS = new OBISCode();
        CommanCommands comm = new CommanCommands();
        DataTable ObjDbt = new DataTable();
        FunctionalTestLogicalName functionalTest = new FunctionalTestLogicalName();
        #endregion

        #region Global Variable
        public delegate void DelMethod(byte[] s);
        public string popUpTitle = "PCB Test";
        public byte[] ReceiveStr; UInt32 SetICContinously = 9;
        public bool MainCmdSendFlag = false; public bool isWriteProcessStatus = false;
        public byte[] DisconnectCmd; byte[] Encrypt_StoC = new byte[16]; byte[] ActReqDecp = new byte[24];
        public byte[] GKPlaintext = new byte[45]; public byte[] HLSPlaintext = new byte[31];
        byte[] DedicatedKey = new byte[16]; byte[] invocationCounter = new byte[4]; byte[] FinalNonce = new byte[12];
        byte[] AssociationKey = new byte[17]; byte[] _CipheredData; byte[] _AuthenticationTag = new byte[12]; public bool StopStatus = false;
        public byte[] ServerNonce = new byte[8]; public byte[] bInstantData = new byte[170]; byte[] StoC = new byte[16];
        FinalTesting.Properties.Settings settings = new FinalTesting.Properties.Settings(); public byte[] invocationCounterReset;
        public int UpdateRows; int SuccessRetVal = 0; public byte[] CalibConstWrite = new byte[42]; public string ReadWriteEncode = string.Empty;
        public int ResetCount = 15; public int DiscResetCnt = 0; public int ReadRealRTC = 0; public int returnStatus;
        public double InstVolt; public double ScalarInstVolt; public string HexScalarInstVolt;
        public double InstCurr; public double ScalarInstCurr; public string HexScalarInstCurr;
        public double InstNeutralCurr; public double ScalarInstNeutralCurr;
        public int cmdSequence = 0; public byte[] ReadDataFixedBytes; public int seqIncDecCount = 6;
        public byte[] invocationCounterResetCont = new byte[] { 0x00, 0x00, 0x00, 0x09 };
        #endregion

        #region Global  CommandConstants
        public int CmdType = 0;

        public const int CMD_IDLE = 0;
        public const int CMD_SNRM = 1;
        public const int CMD_AARQ_PC = 2;
        public const int CMD_GETREQ_PC = 3;
        public const int CMD_READIC_PC = 4;
        public const int CMD_DISCONNECT = 5;
        public const int CMD_SNRM_US = 6;
        public const int CMD_AARQ_US = 7;
        public const int CMD_ACTREQ_US = 8;
        public const int CMD_GET_CALIBREAD = 9;
        public const int CMD_DISCONNECT_US = 10;
        public const int CMD_SET_GLKEY_US = 11;
        public const int CMD_SET_HLSKEY_US = 12;
        public const int CMD_GETREQ_SNO = 13;
        public const int CMD_SETREQ_SNO = 14;
        public const int CMD_GETREQ_NEW_SNO = 15;
        public const int CMD_GET_RTC_US = 16;
        public const int CMD_GET_VOLT_US = 17;
        public const int CMD_GETREQ_SCALARUNIT_US = 18;
        public const int CMD_GET_INSTCURR_US = 19;
        public const int CMD_GETREQ_SCALARUNITCURR_US = 20;
        public const int CMD_SETREQ_STARTMETER = 21;
        public const int CMD_GETREQ_READMETERCONTIOUSLY = 22;
        public const int CMD_SETREQ_STOPMETER = 23;
        public const int CMD_GET_INSTNEUTRALCURR_US = 24;
        public const int CMD_GET_SCALARUNITNeutralCURR_US = 25;
        #endregion


        #region Global RequestConstants
        public int requestType = 0;
        public const int REQ_GET_IC_PC = 0;
        public const int REQ_GET_CALIBREAD = 1;
        public const int REQ_SET_GLKEY_US = 2;
        public const int REQ_SET_HLSKEY_US = 3;
        public const int REQ_GET_SNO_US = 4;
        public const int REQ_SET_SNO_US = 5;
        public const int REQ_GET_NEW_SNO_US = 6;
        public const int REQ_GET_RTC_UC = 7;
        public const int REQ_GET_VOLT_US = 8;
        public const int REQ_GET_INSTANTVOLT_US = 9;
        public const int REQ_GET_CURR_US = 10;
        public const int REQ_GET_INSTANTCURR_US = 11;
        public const int REQ_SET_STARTMETERCONT_US = 12;
        public const int REQ_GET_METERDATACONT_US = 13;
        public const int REQ_SET_STOPMETERCMD = 14;
        public const int REQ_GET_IC_KEYCHGE = 15;
        public const int REQ_GET_INSTANTNEUTRALCURR = 16;
        
        #endregion
        public MainMaster()
        {
            MUTSerialCOM.DataReceived += new dataReceived(MUTSerialCOM_DataReceived);
            InitializeComponent();
        }
        private void MUTSerialCOM_DataReceived(object sender, SerialPortEventArgs arg)
        {
            ReceiveStr = new byte[arg.ReceivedData.Length];
            ReceiveStr = arg.ReceivedData;

            //CmdCount = CmdCount + 1;

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
                                        UpdateLogStatus(" Invocation Counter : " + IC);
                                        pcOpticalStatusGood.Visible = true; pcOpticalStatusGood.Enabled = true; pcOpticalStatusGood.Refresh();
                                        lblStatus.Text = "Invocation Counter Read";
                                    }
                                    else
                                    {
                                        pcOpticalStatusFail.Visible = false;
                                    }
                                }
                            }
                        }
                        CheckNextCmdReadInvocation(RespStatus);
                    }
                    else if (CmdType == CMD_DISCONNECT)
                    {
                        if (requestType == REQ_GET_IC_PC) { if (ReceiveStr[5] == 115) { RespStatus = true; requestType = REQ_GET_CALIBREAD; } }
                        if (requestType == REQ_GET_CALIBREAD)
                        {
                            MainCmdSendFlag = false;
                            statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                            if (ReceiveStr[5] == 115) { RespStatus = true; CmdType = CMD_SNRM_US; MainCmdSendFlag = true; }
                        }
                        else if (requestType == REQ_GET_IC_KEYCHGE)
                        {
                            MainCmdSendFlag = false;
                            statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                            if (ReceiveStr[5] == 115) { RespStatus = true; requestType = REQ_GET_SNO_US; CmdType = CMD_SNRM_US; MainCmdSendFlag = true; }
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
                    else if (CmdType == CMD_GET_CALIBREAD)
                    {
                        if (ReceiveStr[0] != 0)
                        {
                            byte[] _SDecNonce = ServerNonce;
                            byte[] _SDecsubNonce_FrameCount = new byte[4];
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
                    else if (CmdType == CMD_DISCONNECT_US)
                    {
                        if (requestType == REQ_GET_CALIBREAD)
                        {
                            if (CalibConstWrite[41] == 1)
                            {
                                MainCmdSendFlag = false;
                                statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                                CmdType = CMD_SNRM_US;
                                RespStatus = true; requestType = REQ_SET_HLSKEY_US; MainCmdSendFlag = true;// REQ_SET_GLKEY_US
                            }
                            else
                            {
                                MessageBox.Show("Meter not Eligible for this process...", "Smartmeter", MessageBoxButtons.OK, MessageBoxIcon.Warning); btnStarttest.Enabled = true;
                                return;
                            }
                        }
                        else if (requestType == REQ_SET_GLKEY_US)
                        {
                            statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                            requestType = REQ_GET_SNO_US;
                            CmdType = CMD_SNRM_US;
                            MainCmdSendFlag = true;
                        }
                        else if(requestType == REQ_GET_IC_KEYCHGE)
                        {
                            statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                            //requestType = REQ_GET_SNO_US;
                            CmdType = CMD_SNRM;
                            MainCmdSendFlag = true;
                        }
                        
                    }
                    else if (CmdType == CMD_SET_GLKEY_US)
                    {
                        SecurityKeys securityKeys = new SecurityKeys();
                       
                        if (ReceiveStr[0] != 0)
                        {
                           // statusKeyClass.GlobalKey = settings.GlobalKey;
                           
                            byte[] _SDecNonce = ServerNonce;
                            byte[] _SDecsubNonce_FrameCount = new byte[4];
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

                            UpdateLogStatus("Globalkey DECODED : " + sCipherResults + Environment.NewLine);
                            if (bDecryptResult[3] == 0) { RespStatus = true; statusKeyClass.GlobalKey = statusKeyClass.NewGlobalkey; string SQLQuery = "Update Login set Globalkey='" + statusKeyClass.NewGlobalkey + "' where TestZig = '" + statusKeyClass.ZigNumber + "'"; returnStatus = InsertConfigData(SQLQuery); }
                            if (returnStatus == 1)
                            {
                                // requestType = REQ_SET_GLKEY_US; 
                                requestType = REQ_GET_IC_KEYCHGE; CheckNextCmdSetCalibNxtLvl(RespStatus);
                                //CheckNextCmdGetSNO(RespStatus);
                            }
                        }
                    }
                    else if (CmdType == CMD_SET_HLSKEY_US)
                    {
                        if (ReceiveStr[0] != 0)
                        {
                           // statusKeyClass.GlobalKey = settings.GlobalKey;
                            byte[] _SDecNonce = ServerNonce;
                            byte[] _SDecsubNonce_FrameCount = new byte[4];
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

                            UpdateLogStatus("HLS DECODED : " + sCipherResults + Environment.NewLine);
                            if (bDecryptResult[3] == 0)
                            {
                                RespStatus = true; statusKeyClass.HLS = statusKeyClass.NewHLS;
                                string SQLQuery = "Update Login set HLS='" + statusKeyClass.NewHLS + "' where TestZig = '" + statusKeyClass.ZigNumber + "'"; returnStatus = InsertConfigData(SQLQuery);
                                lblKeyStatus.Text = "Done";
                            }
                        }
                        if (returnStatus == 1)
                        {
                            requestType = REQ_SET_GLKEY_US;
                            CheckNextCmdActionResponse(RespStatus);
                        }
                    }
                    else if (CmdType == CMD_GETREQ_SNO)
                    {
                        if (ReceiveStr[0] != 0)
                        {
                            string asciiString = GetSNOProcess(ReceiveStr);
                            this.lblSerialNumberRead.Invoke((MethodInvoker)(() => lblSerialNumberRead.Text = asciiString));
                            lblSerialNumberRead.ForeColor = Color.Green; RespStatus = true;
                            CheckNextCmdGetReq(RespStatus);
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
                            UpdateLogStatus("Server Nonce : " + serverNonceData + Environment.NewLine);
                            string assignedserverNonceData = String.Concat(_SDecNonce.Select(b => b.ToString("X2") + " "));
                            UpdateLogStatus("Assigned Server Nonce : " + assignedserverNonceData + Environment.NewLine);

                            byte[] _SFinalNonce = FrameNonce(_SDecNonce, _SDecsubNonce_FrameCount, _SDecNonce.Length);

                            string fiinalNonceData = String.Concat(_SFinalNonce.Select(b => b.ToString("X2") + " "));

                            UpdateLogStatus("Final Nonce : " + fiinalNonceData);

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
                                lblSNOStatus.Text = "Done"; requestType = REQ_GET_NEW_SNO_US;
                                CheckNextCmdActionResponse(isWriteProcessStatus);
                                //this.btnSetRTC.Invoke((MethodInvoker)(() => btnSetRTC.Visible = true));
                                //CheckNextCmdSetRTC(isWriteProcessStatus);
                            }
                            UpdateLogStatus("SNO Set WriteStatus : " + isWriteProcessStatus.ToString());
                        }
                    }
                    else if (CmdType == CMD_GETREQ_NEW_SNO)
                    {
                        if (ReceiveStr[0] != 0)
                        {
                            string asciiString = GetSNOProcess(ReceiveStr);
                            this.lblNewSNO.Invoke((MethodInvoker)(() => lblNewSNO.Text = asciiString));
                            lblNewSNO.ForeColor = Color.Green; RespStatus = true; requestType = REQ_GET_RTC_UC;
                            CheckNextCmdGetReq(RespStatus);
                        }
                    }
                    else if (CmdType == CMD_GET_RTC_US)
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
                            lblRTCRead.Text = Date + ":" + _Month + ":" + Year + " " + _Hour + "-" + _Minute + "-" + _Seconds + " " + Day; RespStatus = true;
                            RespStatus = true; requestType = REQ_GET_VOLT_US; CheckNextCmdGetReq(RespStatus);
                            //CheckNextCmdDisconnect(RespStatus);
                        }
                    }
                    else if (CmdType == CMD_GET_VOLT_US)
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
                                requestType = REQ_GET_INSTANTVOLT_US;
                                //int combinedValue = (a1 << 8) | a2;
                                CheckNextCmdInstantVolt(true);
                            }
                        }
                    }
                    else if (CmdType == CMD_GETREQ_SCALARUNIT_US)
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
                                lblInstVolt.Text = Vol.ToString(); requestType = REQ_GET_CURR_US;
                                CheckNextCmdInstantVolt(true);
                            }
                        }
                    }
                    else if (CmdType == CMD_GET_INSTCURR_US)
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
                                string sCipherResults = String.Concat(bDecryptResult.Select(b => b.ToString("X2") + " ")); requestType = REQ_GET_INSTANTCURR_US;
                                this.LogBox.Invoke((MethodInvoker)(() => LogBox.AppendText(Environment.NewLine + "Currebt Decrypt : " + sCipherResults + Environment.NewLine)));
                                InstCurr = (bDecryptResult[5] << 24) | (bDecryptResult[6] << 16) | (bDecryptResult[7] << 8) | bDecryptResult[8];
                                CheckNextCmdInstantCurr(true);
                            }
                        }
                    }
                    else if (CmdType == CMD_GETREQ_SCALARUNITCURR_US)
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
                                lblInstCurr.Text = Curr.ToString();
                                //requestType = REQ_SET_STARTMETERCONT_US;
                                requestType = REQ_GET_INSTANTNEUTRALCURR;
                                CheckNextCmdVoltCurrRead(true);
                            }
                        }
                    }
                    else if (CmdType == CMD_GET_INSTNEUTRALCURR_US)
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
                                CheckNextCmdInstantNeutralCurr(true);
                            }
                        }
                    }
                    else if (CmdType == CMD_GET_SCALARUNITNeutralCURR_US)
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
                                this.LogBox.Invoke((MethodInvoker)(() => LogBox.AppendText(Environment.NewLine + "Neutral Current Scalar Decrypt : " + sCipherResults + Environment.NewLine)));
                                ScalarInstNeutralCurr = Convert.ToDouble(bDecryptResult[7]);
                                sbyte signedValue = (sbyte)ScalarInstNeutralCurr;
                                double Val = Math.Pow(10, signedValue);
                                double Curr = Math.Round((InstNeutralCurr * Val), 3);
                                lblNTCurr.Text = Curr.ToString();
                                requestType = REQ_SET_STARTMETERCONT_US;
                                //requestType = REQ_GET_INSTANTNEUTRALCURR;
                                CheckNextCmdVoltCurrRead(true);
                            }
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
                                invocationCounterReset = new byte[] { 0x00, 0x00, 0x00, 0x08 };
                                requestType = REQ_GET_METERDATACONT_US;
                                CheckNextCmdContRead(isWriteProcessStatus); //btnStop.Enabled = true; btnTestStart.Enabled = false;
                                // CheckNextCmdDisconnect(isWriteProcessStatus);
                                // MessageBox.Show("Success...");
                            }
                            this.LogBox.Invoke((MethodInvoker)(() => LogBox.AppendText(Environment.NewLine + "WriteStatus : " + isWriteProcessStatus.ToString() + Environment.NewLine)));
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
                                lblStatus.Text = "Continuous testing Process";
                                //this.txtFlagStatus.Invoke((MethodInvoker)(() => txtFlagStatus.Text = resultValue));
                                FinalResult(resultValue);//tmrContinuosRead.Enabled = true;
                                                         // CheckNextCmdActionResponse(true);
                                if (StopStatus == false) { CheckNextCmdContRead(true); }
                                else
                                {
                                    statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                                    requestType = REQ_SET_STOPMETERCMD; cmdSequence = 0;
                                    CmdType = CMD_SETREQ_STOPMETER;//CmdType = CMD_SNRM_US;
                                    //seqIncDecCount = 4;
                                    MainCmdSendFlag = true;
                                }
                            }
                            else
                            {
                                lblStatus.Text = "Wrong Response...";
                               // MessageBox.Show("Wrong Response...");
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
                                InsertFinalResultData();
                                lblStatus.Text = "Meter Stop successfully";
                                lblHardwareStatus.ForeColor = Color.Green; lblHardwareStatus.Text = "Done"; CheckNextCmdDisconnect(true); btnStarttest.Enabled = true; btnStopDisc.Enabled = false;
                            }
                            else
                            {
                                lblStatus.Text = "Wrong Response...";
                                //MessageBox.Show("Wrong Response...");
                            }
                        }

                    }
                }
            }
            catch (Exception ex) { }
        }
        private void InsertFinalResultData()
        {
            string ZigNum = statusKeyClass.ZigNumber; string UName = settings.UserName;
            string OldSNo = lblSerialNumberRead.Text; string NewSNo = lblNewSNO.Text;
            string HarwareStatus;
            if (RAMResult.Text == "OK" && flashResult.Text == "OK") { HarwareStatus = "Done"; }
            else { HarwareStatus = "Fail"; }
            try
            {
                string SQLQuery = "select count(*) from FinalTestingDetails where ZigNumber = '" + ZigNum + "' and UserName = '" + UName + "'";
                SuccessRetVal = TotalCountData(SQLQuery);
                if (SuccessRetVal >= 1)
                {
                    SQLQuery = "Update FinalTestingDetails set OldSerialNumber='" + OldSNo + "',OriginalSerialNumber='" + NewSNo + "',Status = '" + HarwareStatus + "' where UserName = '" + UName + "' and ZigNumber = '" + ZigNum + "'";
                    SuccessRetVal = InsertConfigData(SQLQuery);
                    if (SuccessRetVal >= 1) { lblStatus.Text = "User Updated Successfully..."; GetFinalDetailsRecord(); /*MessageBox.Show("User Updated Successfully...", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information);*/ } //LoadUserDetails(); ClearTextBoxes(); }
                    else { lblStatus.Text = "User Not Updated Successfully..."; }
                }
                else if (SuccessRetVal == 0)
                {
                    SQLQuery = "Insert into FinalTestingDetails(ZigNumber,UserName,OldSerialNumber,OriginalSerialNumber,Status) values('" + ZigNum + "','" + UName + "','" + OldSNo + "','" + NewSNo + "','" + HarwareStatus + "')";
                    SuccessRetVal = InsertConfigData(SQLQuery);
                    if (SuccessRetVal >= 1)
                    {
                        lblStatus.Text = "User Inserted Successfully..."; GetFinalDetailsRecord();
                    }
                }
            }
            catch { }
        }

        private string GetSNOProcess(byte[] SNO)
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

            UpdateLogStatus("Serial No DECODED : " + sCipherResults + Environment.NewLine);
            byte[] SNoHex = new byte[13];
            Buffer.BlockCopy(bDecryptResult, 6, SNoHex, 0, SNoHex.Length); requestType = REQ_SET_SNO_US;
            string asciiString = ConvertBytesToAscii(SNoHex);
            return asciiString;
        }
        static string ConvertByteToBits(byte value)
        {
            return Convert.ToString(value, 2).PadLeft(8, '0');
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
                tmrResetCount.Enabled = true;
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
                        case 28:
                            if (result[i] == 0)
                            {
                                this.RAMResult.Invoke((MethodInvoker)(() => RAMResult.Text = "NOT OK "));
                            }
                            else if (result[i] == 1)
                            {
                                this.RAMResult.Invoke((MethodInvoker)(() => RAMResult.Text = "OK"));
                            }
                            break;
                        case 27:
                            if (result[i] == 0)
                            {
                                this.flashResult.Invoke((MethodInvoker)(() => flashResult.Text = "NOT OK"));
                            }
                            else if (result[i] == 1)
                            {
                                this.flashResult.Invoke((MethodInvoker)(() => flashResult.Text = "OK"));
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }

        public void CheckNextCmdSNRM(bool status)
        {
            try
            {
                statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                if (requestType == REQ_GET_IC_PC || requestType== REQ_GET_IC_KEYCHGE)
                {
                    if (status) { CmdType = CMD_AARQ_PC; MainCmdSendFlag = true; }
                    else { DiscCmdSend(); }
                }
                else if (requestType == REQ_GET_CALIBREAD || requestType == REQ_SET_GLKEY_US || requestType == REQ_SET_HLSKEY_US || requestType == REQ_GET_SNO_US || requestType == REQ_SET_STOPMETERCMD || requestType == REQ_GET_IC_KEYCHGE)
                {
                   
                    if (status) { CmdType = CMD_AARQ_US; MainCmdSendFlag = true; }
                    else { DiscCmdSend(); }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void CheckNextCmdAARQ(bool status)
        {
            SecurityKeys securityKey = new SecurityKeys();
            try
            {
                statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                if (requestType == REQ_GET_IC_PC || requestType == REQ_GET_IC_KEYCHGE)
                {
                    if (status) { CmdType = CMD_GETREQ_PC; MainCmdSendFlag = true; }
                    else { DiscCmdSend(); }
                }
                else if (requestType == REQ_GET_CALIBREAD || requestType == REQ_SET_GLKEY_US || requestType == REQ_SET_HLSKEY_US || requestType == REQ_GET_SNO_US || requestType == REQ_SET_STOPMETERCMD)
                {
                    {
                        if (status)
                        {
                            Buffer.BlockCopy(ReceiveStr, 40, ServerNonce, 0, 8);
                            Buffer.BlockCopy(ReceiveStr, 65, StoC, 0, 16);
                            string StoC_Str = string.Concat(StoC.Select(b => b.ToString("X2") + " "));
                            UpdateLogStatus("S to C : " + StoC_Str + Environment.NewLine);
                            byte[] tempPwd = securityKey.SecretKey;
                            DLMS_AES.Encrypt(StoC, tempPwd);

                            string EncryptStr = string.Concat(StoC.Select(b => b.ToString("X2") + " "));
                            UpdateLogStatus("Encrypted S to C : " + EncryptStr + Environment.NewLine);
                            Buffer.BlockCopy(StoC, 0, Encrypt_StoC, 0, 16);
                            CmdType = CMD_ACTREQ_US; MainCmdSendFlag = true;
                        }
                        else { DiscCmdSend(); }
                    }
                }
            }
            catch (Exception ex)
            {

            }
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
                else if (requestType == REQ_SET_SNO_US)
                {
                    if (status) { CmdType = CMD_SETREQ_SNO; MainCmdSendFlag = true; }
                    else { DiscCmdSend(); }
                }
                else if (requestType == REQ_GET_RTC_UC)
                {
                    if (status) { CmdType = CMD_GET_RTC_US; MainCmdSendFlag = true; }
                    else { DiscCmdSend(); }
                }
                else if (requestType == REQ_GET_VOLT_US)
                {
                    if (status) { CmdType = CMD_GET_VOLT_US; MainCmdSendFlag = true; }
                    else { DiscCmdSend(); }
                }
                else if(requestType == REQ_GET_IC_KEYCHGE)
                {
                    if (status) { CmdType = CMD_READIC_PC; MainCmdSendFlag = true; }
                    else { DiscCmdSend(); }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void CheckNextCmdReadInvocation(bool status)
        {
            try
            {
                statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                if (requestType == REQ_GET_IC_PC || requestType == REQ_GET_IC_KEYCHGE)
                {
                    if (status) { CmdType = CMD_DISCONNECT; MainCmdSendFlag = true; }
                    else
                    { DiscCmdSend(); }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void CheckNextCmdActionResponse(bool status)
        {
            statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
            try
            {
                if (requestType == REQ_GET_CALIBREAD)
                {
                    if (status == true)
                    { CmdType = CMD_GET_CALIBREAD; MainCmdSendFlag = true; }
                }
                else if (requestType == REQ_SET_GLKEY_US)
                {
                    if (status == true) { CmdType = CMD_SET_GLKEY_US; MainCmdSendFlag = true; }
                }
                else if (requestType == REQ_SET_HLSKEY_US)
                {
                    if (status == true) { CmdType = CMD_SET_HLSKEY_US; MainCmdSendFlag = true; }
                }
                else if (requestType == REQ_GET_SNO_US)
                {
                    if (status == true) { CmdType = CMD_GETREQ_SNO; MainCmdSendFlag = true; }
                }
                else if (requestType == REQ_SET_STOPMETERCMD)
                {
                    if (status == true) { CmdType = CMD_SETREQ_STOPMETER; MainCmdSendFlag = true; }
                }
                else if (requestType == REQ_GET_NEW_SNO_US)
                {
                    if (status == true) { CmdType = CMD_GETREQ_NEW_SNO; MainCmdSendFlag = true; }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void CheckNextCmdSetCalibNxtLvl(bool status)
        {
            statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
            try
            {
                if (requestType == REQ_GET_CALIBREAD || requestType == REQ_SET_GLKEY_US)
                {
                    if (status == true) { CmdType = CMD_DISCONNECT_US; MainCmdSendFlag = true; }
                }
                else if(requestType == REQ_GET_IC_KEYCHGE)
                {
                    if (status == true) { CmdType = CMD_DISCONNECT_US; MainCmdSendFlag = true; }
                }
            }
            catch (Exception ex) { }
        }
        public void CheckNextCmdInstantVolt(bool status)
        {
            try
            {
                statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                if (requestType == REQ_GET_INSTANTVOLT_US)
                {
                    if (status) { CmdType = CMD_GETREQ_SCALARUNIT_US; MainCmdSendFlag = true; }
                    else
                    { DiscCmdSend(); }
                }
                else if (requestType == REQ_GET_CURR_US)
                {
                    if (status) { CmdType = CMD_GET_INSTCURR_US; MainCmdSendFlag = true; }
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
                if (requestType == REQ_GET_INSTANTCURR_US)
                {
                    if (status) { CmdType = CMD_GETREQ_SCALARUNITCURR_US; MainCmdSendFlag = true; }
                    else
                    { DiscCmdSend(); }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void CheckNextCmdInstantNeutralCurr(bool status)
        {
            try
            {
                statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                if (requestType == REQ_GET_INSTANTNEUTRALCURR)
                {
                    if (status) { CmdType = CMD_GET_SCALARUNITNeutralCURR_US; MainCmdSendFlag = true; }
                    else
                    { DiscCmdSend(); }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void CheckNextCmdVoltCurrRead(bool status)
        {
            try
            {
                //statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
               
                statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                if (requestType == REQ_GET_INSTANTNEUTRALCURR)
                {
                    if (status) { CmdType = CMD_GET_INSTNEUTRALCURR_US; MainCmdSendFlag = true; }
                    else
                    { DiscCmdSend(); }
                }
                if (requestType == REQ_SET_STARTMETERCONT_US)
                {
                    if (status) { CmdType = CMD_SETREQ_STARTMETER; MainCmdSendFlag = true; }
                    else
                    { DiscCmdSend(); }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void CheckNextCmdContRead(bool status)
        {
            try
            {
                statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                if (requestType == REQ_GET_METERDATACONT_US)
                {
                    if (status) { CmdType = CMD_GETREQ_READMETERCONTIOUSLY; MainCmdSendFlag = true; }
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
                if (requestType == REQ_SET_STOPMETERCMD)//REQ_SET_STOPMETERCMD
                {
                    if (StopStatus == true)
                    {
                        StopStatus = false; tmrResetCount.Enabled = false; ResetCount = 15; CmdType = CMD_DISCONNECT; MainCmdSendFlag = true;
                    }
                }
                else
                {
                    if (status) { CmdType = CMD_DISCONNECT; MainCmdSendFlag = true; }
                }
            }
            catch (Exception ex) { }
        }
        public void CompareRealRTCMeterRTC()
        {
            string SystemRTC = lblHardwareStatus.Text; string MeterRTC = lblRTCRead.Text;

            string[] SplitSysRTC = SystemRTC.Split('-'); string[] yearHour = SplitSysRTC[2].Split(" ");
            string[] SplitSysRTC1 = MeterRTC.Split('-'); string[] yearHour1 = SplitSysRTC1[2].Split(" ");
            DateTime time1 = DateTime.ParseExact(yearHour[1].ToString(), "HH:mm:ss", null);
            DateTime time2 = DateTime.ParseExact(yearHour1[2].ToString(), "HH:mm:ss", null);

            TimeSpan difference = time1 - time2; btnStarttest.Enabled = true;
            if (difference.TotalSeconds <= 2) { lblDiff.Text = difference.ToString(); lblRTCStatus.Text = "RTC Pass"; lblRTCStatus.ForeColor = Color.Green; lblStatus.Text = "Initialization Process Completed Successfully..."; }
            InsertUpdateData(); //CalibWriteFunction();
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
        public void InsertUpdateData()
        {
            string Username = settings.UserName; string ZigNumber = statusKeyClass.ZigNumber;
            string serialno = lblSerialNumberRead.Text; string RTC = lblRTCRead.Text;
            string RAMClear = lblRAMClear.Text;
            try
            {
                string SQLQuery = "select count(*) from DeviceDetails where serialNumber = '" + serialno + "'";
                SuccessRetVal = TotalCountData(SQLQuery);
                if (SuccessRetVal >= 1)
                {
                    //Update Login set UserName = '" + txtUsername.Text + "',password = '" + Password + "',Status='" + Status + "',Usertype='2' where Guid = '" + Guid + "'
                    SQLQuery = "Update DeviceDetails set ZigNumber = '" + ZigNumber + "',UserName='" + Username + "',OpticalPort = 'OK',ProcessStatus= 'OK',RTC = '" + RTC + "',RAMClear = '" + RAMClear + "' where SerialNumber = '" + serialno + "'";
                    SuccessRetVal = InsertConfigData(SQLQuery);
                    if (SuccessRetVal >= 1)
                    {
                        lblStatus.Text = "Status Updated Successfully..."; LoadDeviceDetails();
                    }
                    else { lblStatus.Text = "status not Updated"; }
                }
                else
                {
                    SQLQuery = "Insert into DeviceDetails(ZigNumber,UserName,RTC,RAMClear,OpticalPort,ProcessStatus,SerialNumber)  values('" + ZigNumber + "','" + Username + "','" + RTC + "','" + RAMClear + "','OK','OK','" + serialno + "')";
                    SuccessRetVal = InsertConfigData(SQLQuery);
                    if (SuccessRetVal >= 1)
                    {
                        lblStatus.Text = "Status Inserted Successfully..."; LoadDeviceDetails();
                    }
                    else { lblStatus.Text = "Status not Updated"; }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void CalibWriteFunction()
        {
            lblStatus.Text = "Read Calib Value";
            //requestType = REQ_GET_CALIBVALUE;
            CmdType = CMD_SNRM_US;
            MainCmdSendFlag = true;
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
                        //btnCOMMOpen.Image = Properties.Resources.UnPlug;
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
                    //btnCOMMOpen.Image = Properties.Resources.plug;
                }
            }
            catch (Exception ex)
            {

            }
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
        private void LoadCOMPort()
        {
            //COMM Port
            string[] Port = SerialPort.GetPortNames();
            if (Port.Length == 0) { MessageBox.Show("No COMM Port are detected..."); return; }
            cmbSerialPort.Items.AddRange(Port);
            cmbSerialPort.SelectedIndex = 0;
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
        private string ConvertBytesToAscii(byte[] bytes)
        {
            Encoding asciiEncoding = Encoding.ASCII;
            string asciiString = asciiEncoding.GetString(bytes);
            return asciiString;
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
        public void Clearlbl()
        {
            pcOpticalStatusGood.Visible = false; pcOpticalStatusFail.Visible = false;
            lblSerialNumberRead.Text = "NIL"; lblRealRTC.Text = "NIL"; lblRAMClear.Text = "NIL"; lblHardwareStatus.Text = "NIL";
            lblRTCStatus.Text = "NIL"; lblHardwareStatus.Text = "NIL"; lblKeyStatus.Text = "NIL"; lblNTCurr.Text = "NIL"; /*lblRTCOK.Text = "NIL";*/
            lblRTCRead.Text = "NIL"; lblDiff.Text = "NIL";lblNewSNO.Text = "NIL";lblInstVolt.Text = "NIL";lblInstCurr.Text = "NIL";
        }

        private void btnStarttest_Click(object sender, EventArgs e)
        {
            Clearlbl(); if (txtSNo.Text == "") {   MessageBox.Show("Please fill the serial number field in the appropriate textbox"); return; }
            seqIncDecCount = 6;
            lblStatus.Text = "Started to test";
            requestType = REQ_GET_IC_PC;
            CmdType = CMD_SNRM; btnStopDisc.Enabled = true;
            MainCmdSendFlag = true;
        }
        //////////////////-----------------Commands-----------------------------/////////////////
        private void SNRMCmd()
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
                MUTSerialCOM.Send(snrmCmd); btnStarttest.Enabled = false; //Thread.Sleep(250);
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
                MUTSerialCOM.Send(AARQcmd); //Thread.Sleep(500);
            }
            catch { }

        }
        private void PublicClientLogicalName()
        {
            try
            {
                byte[] logicalNameCmd = new byte[27];
                byte[] plainText;// = new byte[13];

                byte[] hdlc = FramePublicHDLC(0x19, objControlByte.ActionCByte);

                Buffer.BlockCopy(hdlc, 0, logicalNameCmd, 0, hdlc.Length);

                plainText = CreatePlainText(objTag.GetRequest, 0x01, objcls.Class1, objOBIS.InvocationOBIS);
                Buffer.BlockCopy(plainText, 0, logicalNameCmd, hdlc.Length, plainText.Length);


                objerror.FCS = FCS.ComputeFCS(logicalNameCmd);
                Buffer.BlockCopy(objerror.FCS, 0, logicalNameCmd, hdlc.Length + plainText.Length, objerror.FCS.Length);
                Buffer.BlockCopy(hdlc, 0, logicalNameCmd, hdlc.Length + plainText.Length + objerror.FCS.Length, 1);


                string finalCommand = String.Concat(logicalNameCmd.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus(" Public Client LogicalName : " + finalCommand);
                MUTSerialCOM.Send(logicalNameCmd); //Thread.Sleep(500);
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
                MUTSerialCOM.Send(ReadInvocationCmd); //Thread.Sleep(500);


            }
            catch { }
        }
        private void DiscCmdSend()
        {
            DisconnectCmd = comm.PublicClientDisconnect();
            string Disconnect = String.Concat(DisconnectCmd.Select(b => b.ToString("X2") + " "));
            UpdateLogStatus("Disconnect : " + Disconnect);
            MUTSerialCOM.Send(DisconnectCmd); MainCmdSendFlag = false;
        }
        private void DiscCmdSend_US()
        {
            DisconnectCmd = new byte[] { 0x7E, 0xA0, 0x07, 0x03, 0x61, 0x53, 0x65, 0x81, 0x7E };
            //DisconnectCmd = comm.PublicClientDisconnect();
            string Disconnect = String.Concat(DisconnectCmd.Select(b => b.ToString("X2") + " "));
            UpdateLogStatus("Disconnect : " + Disconnect);
            MUTSerialCOM.Send(DisconnectCmd); MainCmdSendFlag = false;
            if (DiscResetCnt == 1)
            {
                // tmrResetCount.Enabled = true;
            }

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
                DedicatedKey = new byte[16];
                Buffer.BlockCopy(objAARQ.AARQPlainText, 3, DedicatedKey, 0, 16);
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
                //this.RTCBox.AppendText(Environment.NewLine + "\nAction CIPHER : " + PlainStr + Environment.NewLine);
                // this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\nBefore Action CIPHER : " + PlainStr + Environment.NewLine)));
                UpdateLogStatus("\nBefore Action CIPHER : " + PlainStr + Environment.NewLine);

                byte[] bResult = AESGCM.SimpleEncrypt(objAction.ActPlainText, securityKey.PasswordKey, _FinalNonce, _AAD);
                string CipheredText = String.Concat(bResult.Select(b => b.ToString("X2") + " "));
                //  this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\nAfter Action CIPHER : " + CipheredText + Environment.NewLine)));
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
                //  this.RTCBox.AppendText(Environment.NewLine + "\nACTION REQUEST : " + finalCommand + Environment.NewLine);
                // this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\nACTION REQUEST : " + finalCommand + Environment.NewLine)));
                UpdateLogStatus("\nACTION REQUEST : " + finalCommand + Environment.NewLine);
                MUTSerialCOM.Send(FinalCmd); invocationCounter = IterateInvocation(invocationCounter);// Thread.Sleep(500);
            }

            catch (Exception ex)
            {
            }
        }
        private void SetGlobalKey()
        {
            GlobalkeyWrite globalkeyWrite = new GlobalkeyWrite();
            SecurityKeys securityKey = new SecurityKeys();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();
            try
            {
                GetKeyDetails();

                byte[] bMasterKey = Encoding.ASCII.GetBytes(statusKeyClass.MasterKey);
                byte[] bNewGlobalKey = Encoding.ASCII.GetBytes(statusKeyClass.NewGlobalkey);
                byte[] WrappedKey = DLMS_AES.Wrap(bMasterKey, bNewGlobalKey);
                //invocationCounter = IterateInvocation(invocationCounter);
                invocationCounter = new byte[] { 0x00, 0x00, 0x00, 0x01 };
                byte[] _FinalNonce = FrameNonce(securityKey.CNonce, invocationCounter, securityKey.CNonce.Length);
                string Psd = String.Concat(securityKey.PasswordKey.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("\nGK Psd : " + Psd + Environment.NewLine);
                byte[] _AAD = FrameAAD(securityKey.PasswordKey);

                Buffer.BlockCopy(globalkeyWrite.SetGKPlainText, 0, GKPlaintext, 0, 21);
                Buffer.BlockCopy(WrappedKey, 0, GKPlaintext, 21, 24);

                string DK = String.Concat(DedicatedKey.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("\nGK Dedicated Key : " + DK + Environment.NewLine);
                byte[] bResult = AESGCM.SimpleEncrypt(GKPlaintext, DedicatedKey, _FinalNonce, _AAD);
                string CipheredText = String.Concat(bResult.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("\nAfter Global CIPHER : " + CipheredText + Environment.NewLine);

                _CipheredData = new byte[45]; _AuthenticationTag = new byte[12];
                string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));
                Buffer.BlockCopy(bResult, 0, _CipheredData, 0, 45);
                Buffer.BlockCopy(bResult, 45, _AuthenticationTag, 0, 12);

                //---------------- Frame the GlobalKeySet Send Request ----------------------------// 

                byte[] SAEChar = protocolFlags.StartFlag;
                byte[] FrameFormat = securityKey.HDLCFrame_Format;
                byte[] _HDLCLength = globalkeyWrite.SetGKLenght;
                encryptData.CipheredData = _CipheredData;
                encryptData.AuthenticationTag = _AuthenticationTag;

                //---------------Final SETGK Command ------------------//
                byte[] FinalCmd = new byte[_HDLCLength[0] + 2];
                FinalCmd[0] = SAEChar[0];
                FinalCmd[1] = FrameFormat[0];
                FinalCmd[2] = _HDLCLength[0];
                globalkeyWrite.SetGKFixedBytes.CopyTo(FinalCmd, 3);

                int len = protocolFlags.StartFlag.Length + securityKey.HDLCFrame_Format.Length + globalkeyWrite.SetGKLenght.Length + globalkeyWrite.SetGKFixedBytes.Length;

                globalkeyWrite.SetGKActionBlock1.CopyTo(FinalCmd, len);
                len += globalkeyWrite.SetGKActionBlock1.Length;
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
                //  this.RTCBox.AppendText(Environment.NewLine + "\nACTION REQUEST : " + finalCommand + Environment.NewLine);
                // this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\nACTION REQUEST : " + finalCommand + Environment.NewLine)));
                UpdateLogStatus("\nSETGLOBALKEY REQUEST : " + finalCommand + Environment.NewLine);
                MUTSerialCOM.Send(FinalCmd);
            }
            catch (Exception ex)
            {

            }
        }
        private void SetHLSKey()
        {
            try
            {
                HLSKeyWrite hLSKeyWrite = new HLSKeyWrite();
                SecurityKeys securityKey = new SecurityKeys();
                ProtocolFlags protocolFlags = new ProtocolFlags();
                EncryptData encryptData = new EncryptData();
                ErrorDetection errorDetection = new ErrorDetection();

                GetKeyDetails();
                byte[] invocationCounterReset = { 0x00, 0x00, 0x00, 0x00 };
                byte[] _FinalNonce = FrameNonce(securityKey.CNonce, invocationCounterReset, securityKey.CNonce.Length);
                string Psd = String.Concat(securityKey.PasswordKey.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("\nHLS Psd : " + Psd + Environment.NewLine);
                byte[] _AAD = FrameAAD(securityKey.PasswordKey);
                _CipheredData = new byte[31];
                byte[] newHLSKey = Encoding.ASCII.GetBytes(statusKeyClass.NewHLS);

                Buffer.BlockCopy(hLSKeyWrite.SetHLSPlainText, 0, HLSPlaintext, 0, 15);
                Buffer.BlockCopy(newHLSKey, 0, HLSPlaintext, 15, 16);

                string DK = String.Concat(DedicatedKey.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("\nHLS Dedicated Key : " + DK + Environment.NewLine);
                byte[] bResult = AESGCM.SimpleEncrypt(HLSPlaintext, DedicatedKey, _FinalNonce, _AAD);
                string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("HLS Ciphered : " + ss);

                Buffer.BlockCopy(bResult, 0, _CipheredData, 0, _CipheredData.Length);
                Buffer.BlockCopy(bResult, 31, _AuthenticationTag, 0, 12);

                //---------------- Frame the GlobalKeySet Send Request ----------------------------// 

                byte[] SAEChar = protocolFlags.StartFlag;
                byte[] FrameFormat = securityKey.HDLCFrame_Format;
                byte[] _HDLCLength = hLSKeyWrite.SetGKLenght;
                encryptData.CipheredData = _CipheredData;
                encryptData.AuthenticationTag = _AuthenticationTag;

                //---------------Final SETGK Command ------------------//

                byte[] FinalCmd = new byte[_HDLCLength[0] + 2];
                FinalCmd[0] = SAEChar[0];
                FinalCmd[1] = FrameFormat[0];
                FinalCmd[2] = _HDLCLength[0];
                hLSKeyWrite.SetHLSFixedBytes.CopyTo(FinalCmd, 3);

                int len = protocolFlags.StartFlag.Length + securityKey.HDLCFrame_Format.Length + hLSKeyWrite.SetGKLenght.Length + hLSKeyWrite.SetHLSFixedBytes.Length;

                hLSKeyWrite.SetHLSActionBlock1.CopyTo(FinalCmd, len);
                len += hLSKeyWrite.SetHLSActionBlock1.Length;
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
                UpdateLogStatus("\nSETHLSKEY REQUEST : " + finalCommand + Environment.NewLine);
                MUTSerialCOM.Send(FinalCmd);
            }
            catch (Exception ex) { }
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
                //invocationCounter = IterateInvocation(invocationCounter);
                byte[] invocationCounter1 = { 0x00, 0x00, 0x00, 0x00 };
                byte[] FinalCmd;

                //   byte[] PlainText = { 0xC0, 0x01, 0xC1, 0x00, 0x01, 0x01, 0x00, 0x60, 0x02, 0x83, 0x00 };
                Buffer.BlockCopy(serialNumber.PlainText, 0, serialNumber.SPlainText, 0, 11);
                serialNumber.SPlainText[11] = 0x01;
                serialNumber.SPlainText[12] = 0x00;
                //Buffer.BlockCopy(serialNumber.InvocationCounter, 0, serialNumber.SInvocationCounter, 0, 3); //---check this 2 or 3
                //serialNumber.SInvocationCounter[3] = 0x00;

                byte[] _FinalNonce = FrameNonce(securityKeys.CNonce, invocationCounter1, securityKeys.CNonce.Length);
                byte[] _AAD = FrameAAD(securityKeys.PasswordKey);
                _CipheredData = new byte[13];

                byte[] bResult = AESGCM.SimpleEncrypt(serialNumber.SPlainText, DedicatedKey, _FinalNonce, _AAD);

                string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));
                //   this.RTCBox.AppendText(Environment.NewLine + "\nSerial No CIPHER : " + ss + Environment.NewLine);
                //this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\nSerial No CIPHER : " + ss + Environment.NewLine)));
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
                // this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\nSerial No Logical Name : " + finalCommand + Environment.NewLine)));
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
            byte[] bInstantData;
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
        private void WriteSNo()
        {
            byte[] bInstantData;
            string SerialNo = txtSNo.Text;
            byte[] bSerialNo = Encoding.ASCII.GetBytes(SerialNo);
            SecurityKeys securityKeys = new SecurityKeys();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();


            try
            {
                // invocationCounter = IterateInvocation(invocationCounter);
                byte[] invocationCounterReset = { 0x00, 0x00, 0x00, 0x01 };
                byte[] PlainText = new byte[28];
                byte[] FPlainText = new byte[] { 0xC1, 0x01, 0xC1, 0x00, 0x01, 0x01, 0x00, 0x60, 0x02, 0x84, 0x00, 0x02, 0x00, 0x0A, 0x0D };
                Buffer.BlockCopy(FPlainText, 0, PlainText, 0, FPlainText.Length);
                Buffer.BlockCopy(bSerialNo, 0, PlainText, 15, bSerialNo.Length);

                // byte[] InvocationCounter = new byte[4] { 0x00, 0x00, 0x00, 0x00 };
                byte[] FinalCmd; // 
                try
                {
                    byte[] _FinalNonce = FrameNonce(securityKeys.CNonce, invocationCounterReset, securityKeys.CNonce.Length);
                    byte[] _AAD = FrameAAD(securityKeys.PasswordKey);
                    _CipheredData = new byte[28];

                    byte[] bResult = AESGCM.SimpleEncrypt(PlainText, DedicatedKey, _FinalNonce, _AAD);

                    string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));

                    UpdateLogStatus(" Serial Number Ciphered : " + ss);
                    //this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\n Serial Number Ciphered : " + ss + Environment.NewLine)));
                    Buffer.BlockCopy(bResult, 0, _CipheredData, 0, _CipheredData.Length);
                    Buffer.BlockCopy(bResult, 28, _AuthenticationTag, 0, 12);


                    byte[] SAEChar = protocolFlags.StartFlag;
                    byte[] FrameFormat = securityKeys.HDLCFrame_Format;
                    byte[] _HDLCLength = { 0x3B };
                    byte[] FixedBytes = new byte[8] { 0x03, 0x61, 0x76, 0x0C, 0x2C, 0xE6, 0xE6, 0x00 };
                    byte[] ReqBlock = new byte[3] { 0xD1, 0x2D, 0x30 };
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
                    UpdateLogStatus(" Serial Number Write Request : " + finalCommand + ss);
                    //this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\n Serial Number Write Request : " + finalCommand + Environment.NewLine)));
                    MUTSerialCOM.Send(FinalCmd);
                    //Thread.Sleep(500);



                }
                catch (Exception ex)
                { }
            }
            catch (Exception ex)
            { }
        }
        private void ReadNewSerialNo()
        {
            SecurityKeys securityKeys = new SecurityKeys();
            ReadSerialNo serialNumber = new ReadSerialNo();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();
            byte[] bInstantData;
            try
            {
                byte[] invocationCounterReset = { 0x00, 0x00, 0x00, 0x02 };
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
                    serialNumber.ReadFixedBytes1.CopyTo(FinalCmd, 3);

                    int len = protocolFlags.StartFlag.Length + securityKeys.HDLCFrame_Format.Length +
                              serialNumber.HDLCLen.Length + serialNumber.ReadFixedBytes1.Length;

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
        private void ReadRTC()
        {
            RTCRead objDataReq = new RTCRead();
            SecurityKeys securityKey = new SecurityKeys();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();
            try
            {
                //  invocationCounter = IterateInvocation(invocationCounter);
                byte[] invocationCounterReset = { 0x00, 0x00, 0x00, 0x03 };

                byte[] bInstantData;
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
                    //this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\nDATA REQUEST CIPHER : " + ss + Environment.NewLine)));
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
                    // this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\n RTC Request : " + finalCommand + Environment.NewLine)));
                    UpdateLogStatus("Read RTC Request: " + finalCommand + Environment.NewLine);
                    MUTSerialCOM.Send(FinalCmd); //Thread.Sleep(500);



                }
                catch (Exception ex)
                {
                    ErrorDetection(ReceiveStr);
                }


            }
            catch { };
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
                invocationCounterReset = new byte[] { 0x00, 0x00, 0x00, 0x04 };
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
        private void GetInstVoltScalarUnit()
        {
            SecurityKeys securityKeys = new SecurityKeys();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();
            VoltScalarUnit voltScalarUnit = new VoltScalarUnit();
            try
            {
                byte[] FinalCmd; invocationCounterReset = new byte[] { 0x00, 0x00, 0x00, 0x05 };
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
            try
            {
                byte[] FinalCmd; invocationCounterReset = new byte[] { 0x00, 0x00, 0x00, 0x06 };
                Buffer.BlockCopy(instantCurrLogicalName.PlainText, 0, instantCurrLogicalName.PlainText, 0, 11);

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
                byte[] FinalCmd; invocationCounterReset = new byte[] { 0x00, 0x00, 0x00, 0x07 };
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
                byte[] FinalCmd; invocationCounterReset = new byte[] { 0x00, 0x00, 0x00, 0x08 };
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
            //  InstantNeutralCurrLogicalName instantNeutralCurrLogicalName = new InstantNeutralCurrLogicalName();
            NeutralCurrScalarUnit neutralCurrScalarUnit = new NeutralCurrScalarUnit();
            try
            {
                byte[] FinalCmd; invocationCounterReset = new byte[] { 0x00, 0x00, 0x00, 0x09 };
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
            byte[] FixedBytes;
            SecurityKeys securityKeys = new SecurityKeys();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();

            try
            {
                invocationCounterResetCont = GetNextInvocation();//new byte[] { 0x00, 0x00, 0x00, 0x08 };
                byte[] FPlainText = new byte[] { 0xC1, 0x01, 0xC1, 0x00, 0x01, 0x01, 0x00, 0x60, 0x02, 0x80, 0x00, 0x02, 0x00, 0x12, 0x1B, 0x58 };
                Buffer.BlockCopy(value, 0, FPlainText, 14, value.Length);
                byte[] FinalCmd;
                try
                {
                    byte[] _FinalNonce = FrameNonce(securityKeys.CNonce, invocationCounterResetCont, securityKeys.CNonce.Length);
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
                    FixedBytes = new byte[8] { 0x03, 0x61, 0x98, 0x31, 0x93, 0xE6, 0xE6, 0x00 };
                    if (StopStatus == true)
                    {
                        byte[] arraySequence = { 0x93, 0x10, 0x32, 0x54, 0x76, 0x98, 0xBA, 0xDC, 0xFE };
                        ReadDataFixedBytes = new byte[6];
                        ReadDataFixedBytes[0] = objHDLC.Flag; ReadDataFixedBytes[1] = objHDLC.FrameFormat;
                        ReadDataFixedBytes[2] = _HDLCLength[0];
                        ReadDataFixedBytes[3] = 0x03; ReadDataFixedBytes[4] = 0x61;
                        ReadDataFixedBytes[5] = arraySequence[seqIncDecCount];

                        byte[] _hcs = FCS.ComputeHCSContinuos(ReadDataFixedBytes);

                        FixedBytes = new byte[8];
                        FixedBytes[0] = 0x03; FixedBytes[1] = 0x61;
                        FixedBytes[2] = arraySequence[seqIncDecCount];
                        FixedBytes[3] = _hcs[0]; FixedBytes[4] = _hcs[1];

                        FixedBytes[5] = 0xE6; FixedBytes[6] = 0xE6; FixedBytes[7] = 0x00;

                        //FixedBytes = new byte[8] { 0x03, 0x61, 0x10, 0x71, 0x9B, 0xE6, 0xE6, 0x00 };
                    }
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
                    invocationCounterResetCont.CopyTo(FinalCmd, len);
                    len += invocationCounterResetCont.Length;
                    encryptData.CipheredData.CopyTo(FinalCmd, len);
                    len += encryptData.CipheredData.Length;
                    encryptData.AuthenticationTag.CopyTo(FinalCmd, len);
                    len += encryptData.AuthenticationTag.Length;

                    errorDetection.FCS = FCS.ComputeFCS(FinalCmd);
                    errorDetection.FCS.CopyTo(FinalCmd, len);

                    FinalCmd[FinalCmd.Length - 1] = SAEChar[0];

                    string finalCommand = String.Concat(FinalCmd.Select(b => b.ToString("X2") + " "));
                    this.LogBox.Invoke((MethodInvoker)(() => LogBox.AppendText(Environment.NewLine + "\n Start Meter Request : " + finalCommand + Environment.NewLine)));
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
                invocationCounterResetCont = GetNextInvocation();
                byte[] FinalCmd;
                Buffer.BlockCopy(clsId, 0, functionalTest.PlainText, 3, 2);
                Buffer.BlockCopy(obisCode, 0, functionalTest.PlainText, 5, 6);

                functionalTest.PlainText[11] = 0x02;

                byte[] _FinalNonce = FrameNonce(securityKey.CNonce, invocationCounterResetCont, securityKey.CNonce.Length);
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

                if (cmdSequence == 0) { ReadDataFixedBytes = new byte[] { 0x03, 0x61, 0xBA, 0xEC, 0xB4, 0xE6, 0xE6, 0x00 }; }
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
                invocationCounterResetCont.CopyTo(FinalCmd, len);
                len += invocationCounterResetCont.Length;
                encryptData.CipheredData.CopyTo(FinalCmd, len);
                len += encryptData.CipheredData.Length;
                encryptData.AuthenticationTag.CopyTo(FinalCmd, len);
                len += encryptData.AuthenticationTag.Length;

                errorDetection.FCS = FCS.ComputeFCS(FinalCmd);
                errorDetection.FCS.CopyTo(FinalCmd, len);

                FinalCmd[FinalCmd.Length - 1] = SAEChar[0];

                string finalCommand = String.Concat(FinalCmd.Select(b => b.ToString("X2") + " "));
                this.LogBox.Invoke((MethodInvoker)(() => LogBox.AppendText(Environment.NewLine + "\nFunctional Test Continuous Read : " + finalCommand + Environment.NewLine)));
                cmdSequence = 1; seqIncDecCount = seqIncDecCount + 1;
                MUTSerialCOM.Send(FinalCmd);
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
        private void WriteRTC()
        {
            byte[] bInstantData;
            SecurityKeys securityKey = new SecurityKeys();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();
            try
            {
                byte[] invocationCounterReset = { 0x00, 0x00, 0x00, 0x01 };
                byte[] DateAndTime = GetDataAndTime();
                byte[] FPlainText = new byte[27];
                byte[] PlainText = new byte[] { 0xC1, 0x01, 0xC1, 0x00, 0x08, 0x00, 0x00, 0x01, 0x00, 0x00, 0xFF, 0x02, 0x00, 0x09, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x4A, 0x00 };
                Buffer.BlockCopy(PlainText, 0, FPlainText, 0, 15);
                Buffer.BlockCopy(DateAndTime, 0, FPlainText, 15, DateAndTime.Length);
                Buffer.BlockCopy(PlainText, 23, FPlainText, 23, 4);
                byte[] FinalCmd; // 
                try
                {

                    byte[] _FinalNonce = FrameNonce(securityKey.CNonce, invocationCounterReset, securityKey.CNonce.Length);
                    byte[] _AAD = FrameAAD(securityKey.PasswordKey);
                    _CipheredData = new byte[27];

                    byte[] bResult = AESGCM.SimpleEncrypt(FPlainText, DedicatedKey, _FinalNonce, _AAD);

                    string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));

                    UpdateLogStatus(" RTC Ciphered : " + ss);
                    //  this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\n RTC Ciphered : " + ss + Environment.NewLine)));
                    Buffer.BlockCopy(bResult, 0, _CipheredData, 0, 27);
                    Buffer.BlockCopy(bResult, 27, _AuthenticationTag, 0, 12);


                    byte[] SAEChar = protocolFlags.StartFlag;
                    byte[] FrameFormat = securityKey.HDLCFrame_Format;
                    byte[] _HDLCLength = { 0x3A };
                    byte[] FixedBytes = new byte[8] { 0x03, 0x61, 0x76, 0xB7, 0x30, 0xE6, 0xE6, 0x00 };// 0x03, 0x61, 0x54, 0xA7, 0x32, 0xE6, 0xE6, 0x00
                    byte[] ReqBlock = new byte[3] { 0xD1, 0x2C, 0x30 };
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

                    UpdateLogStatus("Write RTC Request : " + finalCommand);
                    // this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\n RTC Request : " + finalCommand + Environment.NewLine)));
                    lblRealRTC.Text = DateTime.Now.ToString("dd-M-yyyy HH-mm-ss"); lblSetRTC.Text = "Set Ok";
                    MUTSerialCOM.Send(FinalCmd); //Thread.Sleep(500);



                }
                catch (Exception ex)
                {

                    //  MessageBox.Show("Error: " + ex);
                    ErrorDetection(ReceiveStr);

                }
            }
            catch (Exception ex)
            {

                //  MessageBox.Show("Error: " + ex);
                ErrorDetection(ReceiveStr);
            }
        }

        private void RAMClearWrite()
        {
            byte[] bInstantData;
            SecurityKeys securityKeys = new SecurityKeys();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();


            try
            {
                //invocationCounter = IterateInvocation(invocationCounter);
                invocationCounter = new byte[] { 0x00, 0x00, 0x00, 0x02 };
                byte[] FPlainText = new byte[] { 0xC1, 0x01, 0xC1, 0x00, 0x01, 0x01, 0x00, 0x60, 0x02, 0x80, 0x00, 0x02, 0x00, 0x12, 0xAA, 0xAA };
                // byte[] FPlainText = new byte[] { 0xC1, 0x01, 0xC1, 0x00, 0x01, 0x01, 0x00, 0x60, 0x02, 0x80, 0x00, 0x02, 0x00, 0x12, 0x1B, 0x59 };   Functional Test
                // byte[] InvocationCounter = new byte[4] { 0x00, 0x00, 0x00, 0x00 };
                byte[] FinalCmd; // 
                try
                {

                    byte[] _FinalNonce = FrameNonce(securityKeys.CNonce, invocationCounter, securityKeys.CNonce.Length);
                    byte[] _AAD = FrameAAD(securityKeys.PasswordKey);
                    _CipheredData = new byte[16];

                    byte[] bResult = AESGCM.SimpleEncrypt(FPlainText, DedicatedKey, _FinalNonce, _AAD);

                    string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));

                    //   this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\n RAM Clean Ciphered : " + ss + Environment.NewLine)));
                    UpdateLogStatus(" RAM Clean Ciphered : " + ss);
                    Buffer.BlockCopy(bResult, 0, _CipheredData, 0, 16);
                    Buffer.BlockCopy(bResult, 16, _AuthenticationTag, 0, 12);


                    byte[] SAEChar = protocolFlags.StartFlag;
                    byte[] FrameFormat = securityKeys.HDLCFrame_Format;
                    byte[] _HDLCLength = { 0x2F };
                    byte[] FixedBytes = new byte[8] { 0x03, 0x61, 0x98, 0x31, 0x93, 0xE6, 0xE6, 0x00 };
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
                    UpdateLogStatus(" RAM Clear Request : " + finalCommand);
                    MUTSerialCOM.Send(FinalCmd); //Thread.Sleep(500);



                }
                catch (Exception ex) { }
            }
            catch (Exception ex) { }

        }

        private void CalibAARQCommand()
        {
            CalibConstantAARQ objAARQ = new CalibConstantAARQ();
            SecurityKeys securityKey = new SecurityKeys();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();
            try
            {
              //  Buffer.BlockCopy(objAARQ.AARQPlainText, 3, DedicatedKey, 0, 16);
                byte[] _FinalNonce = FrameNonce(securityKey.CNonce, objAARQ.AARQInvocationCounter, securityKey.CNonce.Length);
                byte[] _AAD = FrameAAD(securityKey.PasswordKey);
                _CipheredData = new byte[31];

                byte[] bResult = AESGCM.SimpleEncrypt(objAARQ.AARQPlainText, securityKey.PasswordKey, _FinalNonce, _AAD);

                string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));
                Buffer.BlockCopy(bResult, 0, _CipheredData, 0, 31);
                Buffer.BlockCopy(bResult, 31, _AuthenticationTag, 0, 11);

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
                UpdateLogStatus("AARQ Request: " + _aarq);
                MUTSerialCOM.Send(objAARQ.AARQCommandBytes);
            }
            catch (Exception ex)
            {

            }

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
        private void GetCalibValue()
        {
            try
            {
                CalibGetRequset calibGetRequset = new CalibGetRequset();
                SecurityKeys securityKey = new SecurityKeys();
                ProtocolFlags protocolFlags = new ProtocolFlags();
                EncryptData encryptData = new EncryptData();
                ErrorDetection errorDetection = new ErrorDetection();

                byte[] invocationCounter = { 0x00, 0x00, 0x00, 0x01 };
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
                calibGetRequset.CalibReadFixedBytesRead.CopyTo(FinalCmd, 3);

                int len = protocolFlags.StartFlag.Length + securityKey.HDLCFrame_Format.Length +
                          calibGetRequset.CalibReadHDLCLength.Length + calibGetRequset.CalibReadFixedBytesRead.Length;

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
        private void SetCalibValue()
        {
            SecurityKeys securityKey = new SecurityKeys();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();
            try
            {
                byte[] invocationCounterReset = { 0x00, 0x00, 0x00, 0x02 };
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
                byte[] FixedBytes = new byte[8] { 0x03, 0x61, 0x98, 0x4F, 0x41, 0xE6, 0xE6, 0x00 };
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
        private byte[] GetDataAndTime()
        {
            //string DateAndTime = dateTimePicker1.Value.ToString("yyyy-MM-dd dddd HH:mm:ss");
            string year = DateTimeToHex(DateTime.Now.ToString("yyyy"));
            //string year = DateTimeToHex(dateTimePicker1.Value.ToString("yyyy"));
            string _year = year.PadLeft(4, '0');

            string Month = DateTimeToHex(DateTime.Now.ToString("MM"));
            //string Month = DateTimeToHex(dateTimePicker1.Value.ToString("MM"));
            string _Month = Month.PadLeft(2, '0');

            string Date = DateTimeToHex(DateTime.Now.ToString("dd"));
            //string Date = DateTimeToHex(dateTimePicker1.Value.ToString("dd"));
            string _Date = Date.PadLeft(2, '0');

            string Day = DateTimeToHex(WeekToNum(DateTime.Now.ToString("dddd")));
            string _Day = Day.PadLeft(2, '0');

            string Hours = DateTimeToHex(DateTime.Now.ToString("HH"));
            //string Hours = DateTimeToHex(dateTimePicker1.Value.ToString("HH"));
            string _Hours = Hours.PadLeft(2, '0');

            string Minutes = DateTimeToHex(DateTime.Now.ToString("mm"));
            //string Minutes = DateTimeToHex(dateTimePicker1.Value.ToString("mm"));
            string _Minutes = Minutes.PadLeft(2, '0');

            string Seconds = DateTimeToHex(DateTime.Now.ToString("ss"));
            //string Seconds = DateTimeToHex(dateTimePicker1.Value.ToString("ss"));
            string _Seconds = Seconds.PadLeft(2, '0');

            String FDateAndTime = _year + _Month + _Date + _Day + _Hours + _Minutes + _Seconds;
            byte[] DateTimeBytes = new byte[8];

            DateTimeBytes[0] = Convert.ToByte(int.Parse(FDateAndTime.Substring(0, 2), System.Globalization.NumberStyles.HexNumber).ToString()); ;
            DateTimeBytes[1] = Convert.ToByte(int.Parse(FDateAndTime.Substring(2, 2), System.Globalization.NumberStyles.HexNumber).ToString());
            DateTimeBytes[2] = Convert.ToByte(int.Parse(FDateAndTime.Substring(4, 2), System.Globalization.NumberStyles.HexNumber).ToString());
            DateTimeBytes[3] = Convert.ToByte(int.Parse(FDateAndTime.Substring(6, 2), System.Globalization.NumberStyles.HexNumber).ToString());
            DateTimeBytes[4] = Convert.ToByte(int.Parse(FDateAndTime.Substring(8, 2), System.Globalization.NumberStyles.HexNumber).ToString());
            DateTimeBytes[5] = Convert.ToByte(int.Parse(FDateAndTime.Substring(10, 2), System.Globalization.NumberStyles.HexNumber).ToString());
            DateTimeBytes[6] = Convert.ToByte(int.Parse(FDateAndTime.Substring(12, 2), System.Globalization.NumberStyles.HexNumber).ToString());
            DateTimeBytes[7] = Convert.ToByte(int.Parse(FDateAndTime.Substring(14, 2), System.Globalization.NumberStyles.HexNumber).ToString());
            return DateTimeBytes;
        }
        private string DateTimeToHex(string DateAndTime)
        {
            int year = Convert.ToInt32(DateAndTime);
            var data = year.ToString("X");
            return data;
        }
        private string WeekToNum(string _day)
        {
            string Day;
            switch (_day)
            {
                case "Monday":

                    Day = "01";
                    break;

                case "Tuesday":

                    Day = "02";
                    break;

                case "Wednesday":

                    Day = "03";
                    break;

                case "Thursday":

                    Day = "04";
                    break;

                case "Friday":

                    Day = "05";
                    break;

                case "Saturday":

                    Day = "06";
                    break;

                case "Sunday":

                    Day = "07";
                    break;
                default:
                    Day = "00";
                    break;
            }
            return Day;
        }

        private void tmrCommandSent_Tick(object sender, EventArgs e)
        {
            try
            {
                // LogBox.SelectionStart = 0;
                //lblRealRTC.Text = DateTime.Now.ToString("dd-M-yyyy HH-mm-ss");
                if (MainCmdSendFlag == true)
                {
                    MainCmdSendFlag = false;

                    switch (CmdType)
                    {
                        case CMD_SNRM:
                            SNRMCmd();
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
                            SNRMCmd();
                            break;
                        case CMD_AARQ_US:
                            AARQCommand();
                            break;
                        case CMD_ACTREQ_US:
                            ActionRequestCmd();
                            break;
                        case CMD_SET_GLKEY_US:
                            SetGlobalKey();
                            break;
                        case CMD_SET_HLSKEY_US:
                            SetHLSKey();
                            break;
                        case CMD_GETREQ_SNO:
                            ReadSerialNo();
                            break;
                        case CMD_GET_RTC_US:
                            ReadRTC();
                            break;
                        case CMD_SETREQ_SNO:
                            WriteSNo();
                            break;
                        case CMD_GETREQ_NEW_SNO:
                            ReadNewSerialNo();
                            break;
                        case CMD_GET_VOLT_US:
                            GetInstVoltLogicalName();
                            break;
                        case CMD_GETREQ_SCALARUNIT_US:
                            GetInstVoltScalarUnit();
                            break;
                        case CMD_GET_INSTCURR_US:
                            GetInstCurrLogicalName();
                            break;
                        case CMD_GETREQ_SCALARUNITCURR_US:
                            GetInstCurrScalarUnit();
                            break;
                        case CMD_GET_INSTNEUTRALCURR_US:
                            GetInstNeutralCurrLogicalName();
                            break;
                        case CMD_GET_SCALARUNITNeutralCURR_US:
                            GetInstNeutralCurrScalarUnit();
                            break;
                        case CMD_SETREQ_STARTMETER:
                            SETStartFunctionValue();
                            break;
                        case CMD_GETREQ_READMETERCONTIOUSLY:
                            GetFunctionalTestContinuosly(functionalTest.ClassId, functionalTest.OBISCode);
                            break;
                        case CMD_SETREQ_STOPMETER:
                            SETStopFunstionalTest();
                            break;
                        case CMD_DISCONNECT_US:
                            DiscCmdSend_US();
                            break;

                        case CMD_GET_CALIBREAD:
                            GetCalibValueRead();
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
        private void LoadDeviceDetails()
        {
            try
            {
                //txtZicNo.Enabled = true; 
                DataTable ObjDbt = new DataTable(); //ObjDbt.Clear();
                string SQLQuery = "Select * from DeviceDetails";
                ObjDbt = GetDatabaseDataDAL(SQLQuery);
                if (ObjDbt.Rows.Count > 0)
                {
                    gridReport.DataSource = ObjDbt;
                }
                gridReport.Columns[0].DefaultCellStyle.Format = "";
            }
            catch (Exception ex)
            {

            }
        }
        private void GetKeyDetails()
        {
            ObjDbt.Clear();
            string SQLQuery = "select NewGlobalkey,Masterkey,NewHLSkey from Login";
            ObjDbt = GetDatabaseDataDAL(SQLQuery);
            statusKeyClass.NewGlobalkey = (ObjDbt.Rows[0][0]).ToString();
            statusKeyClass.MasterKey = (ObjDbt.Rows[0][1]).ToString();
            statusKeyClass.NewHLS = (ObjDbt.Rows[0][2]).ToString();
            //statusKeyClass.NewHLS
        }
        private void GetFinalDetailsRecord()
        {
            try
            {
                //txtZicNo.Enabled = true; 
                DataTable ObjDbt = new DataTable(); ObjDbt.Clear();
                string SQLQuery = "select * from FinalTestingDetails";
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
        private void MainMaster_Load(object sender, EventArgs e)
        {

            // InsertUpdateData();

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
            }
            LoadDeviceDetails(); GetFinalDetailsRecord();
            LoadCOMPort(); //btnReadSno.Enabled = false;
        }

        private void btnStopDisc_Click(object sender, EventArgs e)
        {
            Clearlbl(); tmrResetCount.Enabled = false; DiscResetCnt = 0;
            DiscCmdSend_US(); btnStarttest.Enabled = true;
        }

        private void tmrResetCount_Tick(object sender, EventArgs e)
        {
            lblHardwareStatus.ForeColor = Color.Red;
            lblHardwareStatus.Text = "Hardware Test Under progress, Wait for " + ResetCount + " Seconds";
            ResetCount = ResetCount - 1;

            if (ResetCount == 0)
            {
                tmrResetCount.Enabled = false;
                //ExportDataGrid();
                StopStatus = true;
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void lblRAMClear_Click(object sender, EventArgs e)
        {

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            MUTSerialCOM.Close();
            this.Hide(); Application.Exit();
        }

        private void MainMaster_FormClosed(object sender, FormClosedEventArgs e)
        {
            MUTSerialCOM.Close(); Application.Exit();//this.Close();
        }

        private void MainMaster_FormClosing(object sender, FormClosingEventArgs e)
        {
            MUTSerialCOM.Close(); Application.Exit();
        }
    }
}
