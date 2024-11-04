using AVG.ProductionProcess.PCBTest.Commands;
using DLMS_MeterReading;
using Encryption;
using Keller.SPM.ProcotolGeneration.Protocol.Serial;
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
        PCBSettingsParam pCBSettingsParam = new PCBSettingsParam();
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
        Properties.Settings settings = new Properties.Settings();
        public int UpdateRows; int SuccessRetVal = 0; int DeleteRow = 0; public byte[] CalibConstWrite = new byte[42]; public string ReadWriteEncode = string.Empty;
        public int ResetCount = 20; public int DiscResetCnt = 0; public int ReadRealRTC = 0;
        #endregion

        #region Global  CommandConstants
        public const int CMD_IDLE = 0;
        public const int CMD_SNRM = 1;
        public const int CMD_AARQ_PC = 2;
        public const int CMD_GETREQ_PC = 3;
        public const int CMD_READIC_PC = 4;

        public const int CMD_SNRM_US = 7;
        public const int CMD_AARQ_US = 8;
        public const int CMD_ACTREQ_US = 9;
        public const int CMD_GETREQ_LOGICNAME = 10;
        public const int CMD_GETREQ_SNO = 11;
        public const int CMD_GETREQ_RTC = 12;

        public const int CMD_SETREQ_SNO = 13;
        public const int CMD_SETREQ_RTC = 14;
        public const int CMD_SETREQ_RAMCLEAR = 15;
        //public const int CMD_SETREQ_SNO = 12;
        public const int CMD_DISCONNECT = 6;
        public const int CMD_DISCONNECT_US = 16;
        public const int CMD_GETREQ_CALIBVALUE = 17;
        public const int CMD_SETREQ_CALIBVALUE = 18;

        public const int CMD_GET_CALIBREAD = 19;
        //public const int CMD_AARQ_CALIB = 17;
        //public const int CMD_ACTREQ_CALIB = 18;

        public int CmdType = 0;
        #endregion


        #region Global RequestConstants
        public int requestType = 0;
        public const int REQ_GET_IC_PC = 0;
        public const int REQ_GET_SNO_US = 1;
        public const int REQ_SET_SNO_US = 2;
        public const int REQ_SET_RAMCLEAR = 3;
        public const int REQ_GET_RTC = 4;
        public const int REQ_GET_CALIBVALUE = 5;
        public const int REQ_GET_IC_RTC = 6;
        public const int REQ_GET_CALIBREAD = 7;
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
                    else if (CmdType == CMD_AARQ_PC || CmdType == CMD_AARQ_US)//|| CmdType == CMD_AARQ_CALIB
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
                                        pcOpticalStatusGood.Visible = true; pcOpticalStatusGood.Enabled = true; pcOpticalStatusGood.Refresh();
                                        lblStatus.Text = "Invocation Counter Read";
                                    }
                                    else
                                    {
                                        pcOpticalStatusFail.Visible = false;
                                        // StatusUpdation.Text = "Something went wrong";
                                    }
                                }
                            }
                        }
                        //requestType = REQ_GET_SNO_US;
                        //CheckNextCmdActionResponse(RespStatus);
                        CheckNextCmdReadInvocation(RespStatus);
                    }
                    else if (CmdType == CMD_DISCONNECT)
                    {
                        // if (requestType == REQ_GET_IC_PC) { if (ReceiveStr[5] == 115) { RespStatus = true; requestType = REQ_GET_SNO_US; btnReadSno.Enabled = true; } }

                        if (requestType == REQ_GET_IC_PC) { if (ReceiveStr[5] == 115) { RespStatus = true; requestType = REQ_GET_CALIBREAD; btnReadSno.Enabled = true; } }

                        if (requestType == REQ_GET_CALIBREAD)
                        {
                            MainCmdSendFlag = false;
                            statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                            if (ReceiveStr[5] == 115) { RespStatus = true; CmdType = CMD_SNRM_US; MainCmdSendFlag = true; }
                        }
                        else if (requestType == REQ_GET_IC_RTC)
                        {
                            MainCmdSendFlag = false;
                            statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                            CmdType = CMD_SNRM_US; requestType = REQ_GET_RTC;
                            MainCmdSendFlag = true;
                        }
                        if (requestType == REQ_GET_SNO_US)
                        {
                            MainCmdSendFlag = false;
                            statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                            CmdType = CMD_SNRM_US;
                            MainCmdSendFlag = true;
                        }
                    }

                    else if (CmdType == CMD_GET_CALIBREAD)
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
                            UpdateLogStatus("Calib Read value DECODED : " + sCipherResults + Environment.NewLine);



                            //Calibration 
                            //CalibConstWrite = new byte[bDecryptResult.Length];
                            //Array.Copy(bDecryptResult, CalibConstWrite, bDecryptResult.Length);
                            byte[] calibConst = { 0xC1, 0x01, 0xC1, 0x00, 0x01, 0x01, 0x00, 0x60, 0x02, 0x81, 0x00, 0x02, 0x00, 0x02, 0x07 };
                            Buffer.BlockCopy(calibConst, 0, CalibConstWrite, 0, calibConst.Length);
                            Buffer.BlockCopy(bDecryptResult, 6, CalibConstWrite, 15, 27);

                            //Buffer.BlockCopy(CalibConstWrite, 16, bDecryptResult, 0, 12);
                            //Array.Copy();
                            // CalibConstWrite[41] = 0x01;
                            //string sCipheredWriteValue = String.Concat(CalibConstWrite.Select(b => b.ToString("X2") + " "));

                            //ReadWriteEncode = sCipheredWriteValue;
                            //UpdateLogStatus("Calib Changed Increment value DECODED : " + ReadWriteEncode + Environment.NewLine);
                            if (bDecryptResult[3] == 0) { RespStatus = true; }
                            CheckNextCmdSetCalibNxtLvl(RespStatus);

                            //byte[] _FinalNonce = FrameNonce(securityKey.CNonce, invocationCounter, securityKey.CNonce.Length);
                            //byte[] _AAD = FrameAAD(securityKey.PasswordKey);
                            //byte[] bEncryptValue = AESGCM.SimpleEncrypt(CalibConstWrite, securityKey.PasswordKey, _FinalNonce, _AAD);
                            //string sEncryptWriteVal = String.Concat(bEncryptValue.Select(b => b.ToString("X2") + " "));
                            //UpdateLogStatus("Calib Changed Increment value Encoded : " + sEncryptWriteVal + Environment.NewLine);

                            ////byte[] bDecryptEncyrpt = AESGCM.SimpleDecrypt(bEncryptValue, DedicatedKey, _SFinalNonce);
                            ////string sDecryptWriteVal = String.Concat(bDecryptEncyrpt.Select(b => b.ToString("X2") + " "));
                            ////UpdateLogStatus("Calib Changed Increment value Encoded : " + sDecryptWriteVal + Environment.NewLine);
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
                    else if (CmdType == CMD_GETREQ_CALIBVALUE)
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
                            UpdateLogStatus("Calib Read value DECODED : " + sCipherResults + Environment.NewLine);



                            //Calibration 
                            //CalibConstWrite = new byte[bDecryptResult.Length];
                            //Array.Copy(bDecryptResult, CalibConstWrite, bDecryptResult.Length);
                            byte[] calibConst = { 0xC1, 0x01, 0xC1, 0x00, 0x01, 0x01, 0x00, 0x60, 0x02, 0x81, 0x00, 0x02, 0x00, 0x02, 0x07 };
                            Buffer.BlockCopy(calibConst, 0, CalibConstWrite, 0, calibConst.Length);
                            Buffer.BlockCopy(bDecryptResult, 6, CalibConstWrite, 15, 27);

                            //Buffer.BlockCopy(CalibConstWrite, 16, bDecryptResult, 0, 12);
                            //Array.Copy();
                            CalibConstWrite[41] = 0x02;
                            string sCipheredWriteValue = String.Concat(CalibConstWrite.Select(b => b.ToString("X2") + " "));

                            ReadWriteEncode = sCipheredWriteValue;
                            UpdateLogStatus("Calib Changed Increment value DECODED : " + ReadWriteEncode + Environment.NewLine);
                            if (bDecryptResult[3] == 0) { RespStatus = true; }
                            CheckNextCmdSetCalibNxtLvl(RespStatus);

                            //byte[] _FinalNonce = FrameNonce(securityKey.CNonce, invocationCounter, securityKey.CNonce.Length);
                            //byte[] _AAD = FrameAAD(securityKey.PasswordKey);
                            //byte[] bEncryptValue = AESGCM.SimpleEncrypt(CalibConstWrite, securityKey.PasswordKey, _FinalNonce, _AAD);
                            //string sEncryptWriteVal = String.Concat(bEncryptValue.Select(b => b.ToString("X2") + " "));
                            //UpdateLogStatus("Calib Changed Increment value Encoded : " + sEncryptWriteVal + Environment.NewLine);

                            ////byte[] bDecryptEncyrpt = AESGCM.SimpleDecrypt(bEncryptValue, DedicatedKey, _SFinalNonce);
                            ////string sDecryptWriteVal = String.Concat(bDecryptEncyrpt.Select(b => b.ToString("X2") + " "));
                            ////UpdateLogStatus("Calib Changed Increment value Encoded : " + sDecryptWriteVal + Environment.NewLine);
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
                            lblSerialNumberRead.ForeColor = Color.Green; RespStatus = true;
                            lblStatus.Text = "Serial Number Read Successffully...";
                            //CheckNextCmdGetReqSNo1(RespStatus);
                            CheckNextCmdGetReqSNo(RespStatus); //RTC
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
                            lblRTCOK.Text = "RTC Pass";
                            this.lblRTCRead.Invoke((MethodInvoker)(() => lblRTCRead.Text = Date + "-" + _Month + "-" + Year + " " + Day + " " + Hour + ":" + _Minute + ":" + _Seconds));
                            lblRTCRead.ForeColor = Color.Green; //btnStart.Enabled = true; btnStop.Enabled = true;
                                                                // SetToReadFunctionalTest(); //CheckNextCmdDisconnect(true);
                                                                // requestType = REQ_SET_RAMCLEAR;
                            RespStatus = true; lblStatus.Text = "RTC Read Successfully...";
                            ReadRealRTC = 1;
                            if (ReadRealRTC == 1) { lblSystemRTC.Text = DateTime.Now.ToString("dd-M-yyyy HH:mm:ss"); DiscResetCnt = 0; CompareRealRTCMeterRTC(); ReadRealRTC = 0; }
                            requestType = REQ_GET_CALIBVALUE; CheckNextCmdReadCalib(RespStatus);
                            //CheckNextCmdGetReqSNo1(RespStatus);//RAMCLEAR
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
                                //lblStatus.Text = "Invocation Counter Read";
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
                                //requestType = REQ_SET_RAMCLEAR;
                                lblStatus.Text = "RTC Write Successfully...";
                                CheckNextCmdGetReqSNo1(isWriteProcessStatus);
                                // CheckNextCmdRamClear(isWriteProcessStatus);//CMD_GETREQ_RTC
                            }
                            else
                            {
                                CheckNextCmdDisconnect(isWriteProcessStatus);
                            }
                        }
                    }
                    else if (CmdType == CMD_SETREQ_CALIBVALUE)
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
                    else if (CmdType == CMD_DISCONNECT_US)
                    {
                        if (ReadRealRTC == 1) { lblSystemRTC.Text = DateTime.Now.ToString("dd-M-yyyy HH:mm:ss"); DiscResetCnt = 0; CompareRealRTCMeterRTC(); ReadRealRTC = 0; }
                        if (requestType == REQ_GET_CALIBREAD)
                        {
                            if (CalibConstWrite[41] == 1)
                            {
                                MainCmdSendFlag = false;
                                statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                                CmdType = CMD_SNRM_US;
                                MainCmdSendFlag = true;
                                RespStatus = true; requestType = REQ_GET_SNO_US; btnReadSno.Enabled = true;
                            }
                            else
                            {
                                MessageBox.Show("Meter not Eligible for this process...", "Smartmeter", MessageBoxButtons.OK, MessageBoxIcon.Warning); btnStarttest.Enabled = true;
                                return;
                            }

                            // if (ReceiveStr[5] == 115) { } 
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
                                requestType = REQ_SET_RAMCLEAR; lblStatus.Text = "RAM Clear Inprogress...";

                                CheckNextCmdDisconnect(isWriteProcessStatus); DiscResetCnt = 1;
                            }

                            this.LogBox.Invoke((MethodInvoker)(() => LogBox.AppendText(Environment.NewLine + "WriteStatus : " + isWriteProcessStatus.ToString() + Environment.NewLine)));


                        }
                    }
                }
            }
            catch (Exception ex) { }
        }
        public void CompareRealRTCMeterRTC()
        {
            string SystemRTC = lblSystemRTC.Text; string MeterRTC = lblRTCRead.Text;

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
                string SQLQuery = "select count(*) from DeviceDetails where UserName='" + Username + "' and  serialNumber = '" + serialno + "'";
                SuccessRetVal = TotalCountData(SQLQuery);
                if (SuccessRetVal >= 1)
                {
                    //Update Login set UserName = '" + txtUsername.Text + "',password = '" + Password + "',Status='" + Status + "',Usertype='2' where Guid = '" + Guid + "'
                    SQLQuery = "Update DeviceDetails set ZigNumber = '" + ZigNumber + "',UserName='" + Username + "',OpticalPort = 'OK',ProcessStatus= 'OK',RTC = '" + RTC + "',RAMClear = '" + RAMClear + "' where UserName='" + Username + "' and SerialNumber = '" + serialno + "'";
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
                        // MessageBox.Show("User Inserted Successfully...", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information); //LoadUserDetails(); ClearTextBoxes();
                        // SQLQuery = "select HLS,GlobalKey,SystemTitle from login where Guid";

                    }
                    else { lblStatus.Text = "Status not Updated"; }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void CheckNextCmdReadCalib(bool status)
        {
            try
            {
                statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                if (requestType == REQ_GET_CALIBVALUE)
                {
                    if (status == true)
                    {
                        CmdType = CMD_GETREQ_CALIBVALUE; MainCmdSendFlag = true;//CmdType = CMD_SETREQ_RAMCLEAR;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void CheckNextCmdRAMClear(bool status)
        {
            try
            {
                statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                if (requestType == REQ_SET_RAMCLEAR)
                {
                    if (status == true)
                    {
                        CmdType = CMD_SETREQ_RAMCLEAR; MainCmdSendFlag = true;//CmdType = CMD_SETREQ_RAMCLEAR;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void CalibWriteFunction()
        {
            lblStatus.Text = "Read Calib Value";
            requestType = REQ_GET_CALIBVALUE;
            CmdType = CMD_SNRM_US;
            MainCmdSendFlag = true;
        }

        public void CheckNextCmdSNRM(bool status)
        {
            try
            {
                statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;

                if (requestType == REQ_GET_IC_PC || requestType == REQ_GET_IC_RTC)
                {
                    if (status) { CmdType = CMD_AARQ_PC; MainCmdSendFlag = true; }
                    else { DiscCmdSend(); }
                }
                else if (requestType == REQ_GET_SNO_US || requestType == REQ_GET_RTC || requestType == REQ_GET_CALIBREAD)
                {
                    if (status) { CmdType = CMD_AARQ_US; MainCmdSendFlag = true; }
                    else { DiscCmdSend(); }
                }
                //else if (requestType == REQ_GET_CALIBVALUE)
                //{
                //    if (status) { CmdType = CMD_AARQ_CALIB; MainCmdSendFlag = true; }
                //    else { DiscCmdSend(); }
                //}
                else if (requestType == REQ_SET_SNO_US)
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
                if (requestType == REQ_GET_IC_PC || requestType == REQ_GET_IC_RTC)
                {
                    if (status)
                    { CmdType = CMD_GETREQ_PC; MainCmdSendFlag = true; }
                    else { DiscCmdSend(); }
                }

                else if (requestType == REQ_GET_SNO_US || requestType == REQ_GET_RTC || requestType == REQ_GET_CALIBREAD)
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
                else if (requestType == REQ_SET_SNO_US)
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
                //else if (requestType == REQ_GET_CALIBVALUE)
                //{
                //    if (status)
                //    {
                //        Buffer.BlockCopy(ReceiveStr, 40, ServerNonce, 0, 8);
                //        Buffer.BlockCopy(ReceiveStr, 65, StoC, 0, 16);
                //        string StoC_Str = string.Concat(StoC.Select(b => b.ToString("X2") + " "));
                //        UpdateLogStatus("S to C : " + StoC_Str + Environment.NewLine);
                //        byte[] tempPwd = securityKey.SecretKey;
                //        DLMS_AES.Encrypt(StoC, tempPwd);

                //        string EncryptStr = string.Concat(StoC.Select(b => b.ToString("X2") + " "));
                //        UpdateLogStatus("Encrypted S to C : " + EncryptStr + Environment.NewLine);
                //        Buffer.BlockCopy(StoC, 0, Encrypt_StoC, 0, 16);
                //        CmdType = CMD_ACTREQ_CALIB; MainCmdSendFlag = true;
                //    }
                //}
            }
            catch (Exception ex)
            { }
        }
        public void CheckNextCmdGetReq(bool status)
        {
            try
            {
                statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                if (requestType == REQ_GET_IC_PC || requestType == REQ_GET_IC_RTC)
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
                if (requestType == REQ_GET_IC_PC || requestType == REQ_GET_IC_RTC)
                {
                    if (status) { CmdType = CMD_DISCONNECT; MainCmdSendFlag = true; }
                    else
                    { DiscCmdSend(); }
                }
            }
            catch (Exception ex)
            { }
        }
        public void CheckNextCmdRamClear(bool status)
        {
            statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
            if (requestType == REQ_GET_SNO_US)
            {
                if (status == true)
                {
                    CmdType = CMD_DISCONNECT_US; MainCmdSendFlag = true;
                    //CmdType = CMD_GETREQ_RTC; MainCmdSendFlag = true;//CmdType = CMD_SETREQ_RAMCLEAR;
                }
            }
        }
        public void CheckNextCmdDisconnect(bool status)
        {
            try
            {
                statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
                if (requestType == REQ_GET_SNO_US) { if (status) { CmdType = CMD_SNRM_US; requestType = REQ_GET_SNO_US; MainCmdSendFlag = true; } }//CmdType = CMD_SNRM_US; requestType = REQ_GET_SNO_US;
                else if (requestType == REQ_SET_RAMCLEAR)
                {
                    CmdType = CMD_DISCONNECT_US; MainCmdSendFlag = true; lblRAMClear.Text = "InProgress";
                    lblAuto.Visible = true;
                    lblSecondsCalc.Visible = true;
                    lblSec.Visible = true;
                    lblRAMClear.ForeColor = Color.Red;
                }
                else if (requestType == REQ_GET_CALIBVALUE)
                {
                    if (status == true)
                    {
                        CmdType = CMD_DISCONNECT_US; MainCmdSendFlag = true;
                    }
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
                { CmdType = CMD_GETREQ_SNO; MainCmdSendFlag = true; }//CMD_GETREQ_LOGICNAME;
            }
            else if (requestType == REQ_GET_RTC)
            {
                if (status == true)
                { CmdType = CMD_GETREQ_RTC; MainCmdSendFlag = true; }
            }
            else if (requestType == REQ_SET_SNO_US)
            {
                if (status == true)
                { CmdType = CMD_SETREQ_SNO; MainCmdSendFlag = true; }
            }
            else if (requestType == REQ_GET_CALIBREAD)
            {
                if (status == true)
                { CmdType = CMD_GET_CALIBREAD; MainCmdSendFlag = true; }
            }
        }
        public void CheckNextCmdSetCalibNxtLvl(bool status)
        {
            statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
            if (requestType == REQ_GET_CALIBVALUE)
            {
                if (status == true)
                {
                    CmdType = CMD_DISCONNECT_US; MainCmdSendFlag = true; //CmdType = CMD_SETREQ_CALIBVALUE; MainCmdSendFlag = true; // 
                }
            }
            else if (requestType == REQ_GET_CALIBREAD)
            {
                if (status == true) { CmdType = CMD_DISCONNECT_US; MainCmdSendFlag = true; }
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
                    CmdType = CMD_SETREQ_RTC; MainCmdSendFlag = true;//CmdType = CMD_GETREQ_RTC;
                }
            }
        }
        public void CheckNextCmdGetReqSNo1(bool status)
        {
            statusClass.CmdSendFlag = 1; statusClass.CmdTimeOutCount = 40; statusClass.CmdTimeOutFlag = 0;
            if (requestType == REQ_GET_SNO_US)
            {
                if (status == true)
                {
                    CmdType = CMD_SETREQ_RAMCLEAR; MainCmdSendFlag = true;//CmdType = CMD_GETREQ_RTC;
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
            lblSerialNumberRead.Text = "NIL"; lblRealRTC.Text = "NIL"; lblRAMClear.Text = "NIL"; lblSystemRTC.Text = "NIL";
            lblRTCStatus.Text = "NIL"; lblSystemRTC.Text = "NIL"; lblRTCOK.Text = "NIL"; lblRTCRead.Text = "NIL"; lblDiff.Text = "NIL";
        }

        private void btnStarttest_Click(object sender, EventArgs e)
        {
            Clearlbl();
            lblStatus.Text = "Started to test";
            requestType = REQ_GET_IC_PC;
            CmdType = CMD_SNRM;
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
                tmrResetCount.Enabled = true;
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
                byte[] invocationCounterReset = { 0x00, 0x00, 0x00, 0x00 };
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
                    byte[] FixedBytes = new byte[8] { 0x03, 0x61, 0x54, 0x1C, 0x2E, 0xE6, 0xE6, 0x00 };
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
                byte[] invocationCounterReset = { 0x00, 0x00, 0x00, 0x00 };

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
        private void WriteRTC()
        {
            byte[] bInstantData;
            SecurityKeys securityKey = new SecurityKeys();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();
            try
            {
                //invocationCounter = IterateInvocation(invocationCounter);
                byte[] invocationCounterReset = { 0x00, 0x00, 0x00, 0x01 };
                byte[] DateAndTime = GetDataAndTime();
                byte[] FPlainText = new byte[27];
                byte[] PlainText = new byte[] { 0xC1, 0x01, 0xC1, 0x00, 0x08, 0x00, 0x00, 0x01, 0x00, 0x00, 0xFF, 0x02, 0x00, 0x09, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x4A, 0x00 };
                Buffer.BlockCopy(PlainText, 0, FPlainText, 0, 15);
                Buffer.BlockCopy(DateAndTime, 0, FPlainText, 15, DateAndTime.Length);
                Buffer.BlockCopy(PlainText, 23, FPlainText, 23, 4);
                //  byte[] InvocationCounter = new byte[4] { 0x00, 0x00, 0x00, 0x00 };
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
                Buffer.BlockCopy(objAARQ.AARQPlainText, 3, DedicatedKey, 0, 16);
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
                        case CMD_GETREQ_LOGICNAME:
                            SNOLogicalName();
                            break;
                        case CMD_GETREQ_SNO:
                            ReadSerialNo();
                            break;
                        case CMD_GETREQ_RTC:
                            ReadRTC();
                            break;
                        case CMD_SETREQ_SNO:
                            WriteSNo();
                            break;
                        case CMD_SETREQ_RTC:
                            WriteRTC();
                            break;
                        case CMD_SETREQ_RAMCLEAR:
                            RAMClearWrite();
                            break;
                        case CMD_DISCONNECT_US:
                            DiscCmdSend_US();
                            break;
                        case CMD_GETREQ_CALIBVALUE:
                            GetCalibValue();
                            break;
                        case CMD_SETREQ_CALIBVALUE:
                            SetCalibValue();
                            break;
                        case CMD_GET_CALIBREAD:
                            GetCalibValueRead();
                            break;
                        //case CMD_AARQ_CALIB:
                        //    CalibAARQCommand();
                        //    break;
                        //case CMD_DISCONNECT:
                        //    DiscCmdSend();
                        // break;
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
        private void LoadUserDetails()
        {
            try
            {

                //txtZicNo.Enabled = true; 
                DataTable ObjDbt = new DataTable(); ObjDbt.Clear();
                string SQLQuery = "select UserName,ImpluserRegister,MOV,Ecap,Magnet,EPROM,Flash,Battery,NICModule,Supercapacitor,PCB,Soldering,LCD,Relay,SMPS from settingsDetails";
                ObjDbt = GetDatabaseDataDAL(SQLQuery);
                if (ObjDbt.Rows.Count > 0)
                {
                    gridCriticalComponents.DataSource = ObjDbt;
                }
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
                string UName = settings.UserName;
                settings.HLS = ObjDbt.Rows[0][5].ToString();
                settings.GlobalKey = ObjDbt.Rows[0][6].ToString();
                settings.SystemTitle = ObjDbt.Rows[0][7].ToString();
                settings.ZigNumber = ObjDbt.Rows[0][4].ToString();
                statusKeyClass.HLS = settings.HLS; statusKeyClass.GlobalKey = settings.GlobalKey;
                statusKeyClass.SystemTitle = settings.SystemTitle; statusKeyClass.ZigNumber = settings.ZigNumber;
            }
            LoadDeviceDetails(); LoadUserDetails();
            LoadCOMPort(); btnReadSno.Enabled = false;
        }

        private void btnStopDisc_Click(object sender, EventArgs e)
        {
            Clearlbl(); tmrResetCount.Enabled = false; DiscResetCnt = 0;
            DiscCmdSend_US(); btnStarttest.Enabled = true;
        }

        private void tmrResetCount_Tick(object sender, EventArgs e)
        {
            if (tmrResetCount.Enabled == true)
            {
                ResetCount = ResetCount - 1;
                //lblResetCount.Text = ResetCount.ToString();
                lblSecondsCalc.Text = ResetCount.ToString();
                if (ResetCount == 0)
                {
                    tmrResetCount.Enabled = false;
                    ResetCount = 20; DiscResetCnt = 0; lblRAMClear.Text = "Done"; lblRAMClear.ForeColor = Color.Green;
                    CmdType = CMD_SNRM; requestType = REQ_GET_IC_RTC; MainCmdSendFlag = true; lblRTCStatus.Text = "Inprogres"; lblRTCStatus.ForeColor = Color.Red;
                    lblAuto.Visible = false;
                    lblSecondsCalc.Visible = false;
                    lblSec.Visible = false;

                    //tmrResetCount.Enabled = false;
                    //ResetCount = 15; DiscResetCnt = 0; lblRAMClear.Text = "Done"; lblRAMClear.ForeColor = Color.Green;
                    //CmdType = CMD_SNRM_US; requestType = REQ_GET_RTC; MainCmdSendFlag = true; lblRTCStatus.Text = "Inprogres"; lblRTCStatus.ForeColor = Color.Red;
                    //lblAuto.Visible = false;
                    //lblSecondsCalc.Visible = false;
                    //lblSec.Visible = false;
                }
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

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            Guid objGuid = Guid.NewGuid();

            if (txtImpulseReg.Text == "" || txtMOV.Text == "" || txtEcap.Text == "" || txtMagnet.Text == "" || txtEPROM.Text == "" || txtFlash.Text == "" || txtBattery.Text == "" || txtNIC.Text == "" || txtSupCap.Text == "" || txtPCB.Text == "" || txtSoldering.Text == "" || txtLCD.Text == "" || txtRelay.Text == "" || txtSMPS.Text == "")
            {
                MessageBox.Show("Please Fill all the Fields...", "Smartmeter", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            string ImpulseReg = txtImpulseReg.Text; pCBSettingsParam.ImpulseRegistor = ImpulseReg;
            string MOV = txtMOV.Text; pCBSettingsParam.MOVParam = MOV;
            string Ecap = txtEcap.Text; pCBSettingsParam.Ecap = Ecap;
            string Magnet = txtMagnet.Text; pCBSettingsParam.MagnetParam = Magnet;
            string EPROM = txtEPROM.Text; pCBSettingsParam.EPROMParam = EPROM;
            string Flash = txtFlash.Text; pCBSettingsParam.FlashParam = Flash;
            string Battery = txtBattery.Text; pCBSettingsParam.BatteryParam = Battery;
            string NICModule = txtNIC.Text; pCBSettingsParam.NICModule = NICModule;
            string SuperCapacitor = txtSupCap.Text; pCBSettingsParam.SuperCapacitor = SuperCapacitor;
            string PCB = txtPCB.Text; pCBSettingsParam.PCBParam = PCB;
            string Soldering = txtSoldering.Text; pCBSettingsParam.SolderingParam = Soldering;
            string LCD = txtLCD.Text; pCBSettingsParam.LCDParam = LCD;
            string Relay = txtRelay.Text; pCBSettingsParam.RelayParam = Relay;
            string SMPSTramform = txtSMPS.Text; pCBSettingsParam.SMPSTransform = SMPSTramform;

            try
            {
                string SQLQuery = "Insert into SettingsDetails(GUID,DateTime,ImpluserRegister,MOV,Ecap,Magnet,EPROM,Flash,Battery,NICModule,Supercapacitor,PCB,Soldering,LCD,Relay,SMPS) values('" + objGuid + "','" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "','" + ImpulseReg + "','" + MOV + "','" + Ecap + "','" + Magnet + "','" + EPROM + "','" + Flash + "','" + Battery + "','" + NICModule + "','" + SuperCapacitor + "','" + PCB + "','" + Soldering + "','" + LCD + "','" + Relay + "','" + SMPSTramform + "')";
                SuccessRetVal = InsertConfigData(SQLQuery);
                if (SuccessRetVal >= 1)
                {
                    MessageBox.Show("User Inserted Successfully...", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information); LoadUserDetails();
                  //LoadUserDetails(); ClearTextBoxes();
                }
            }
            catch (Exception ex)
            {

            }

        }
    }
}
