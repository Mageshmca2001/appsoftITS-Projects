using AVG.ProductionProcess.PCBTest.Commands;
using DLMS_MeterReading;
using Encryption;
using Keller.SPM.ProcotolGeneration.Protocol.Serial;
using SmartMeterDLMS.Models;
using System;
using System.ComponentModel;
using System.Data;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static Keller.SPM.Communication.MUT;


namespace AVG.ProductionProcess.PCBTest.Forms
{
    public partial class MasterForm : Form
    {
        public enum CurrentProcess
        {
            ReadRTC = 0,
            WriteRTC = 1,
            ReadSerialNo = 2,
            WriteSerialN0 = 3
        }

        public bool isWriteProcessStatus = false;

        #region Global Objects

        public SerialCOM MUTSerialCOM = new SerialCOM();
        public SerialCOM BarCodeSerialCOM = new SerialCOM();
        CommanCommands comm = new CommanCommands();

        HDLC objHDLC = new HDLC();
        ErrorDetection objerror = new ErrorDetection();
        ControlBytes objControlByte = new ControlBytes();
        RequestTag objTag = new RequestTag();
        ClassId objcls = new ClassId();
        OBISCode objOBIS = new OBISCode();

        #endregion

        #region Global Variable

        //  bool isConnected=false;
        public int OpticalTest = 0;

        public string popUpTitle = "PCB Test";
        public int CmdCount = 0;
        public byte[] ReceiveStr;
        public delegate void DelMethod(byte[] s);

        byte[] invocationCounter = new byte[4];

        byte[] StoC = new byte[16];
        byte[] Encrypt_StoC = new byte[16];
        byte[] _CipheredData;
        byte[] _AuthenticationTag = new byte[12];
        byte[] FinalNonce = new byte[12];
        byte[] AssociationKey = new byte[17];
        byte[] DedicatedKey = new byte[16];
        public byte[] ServerNonce = new byte[8]; // { 0x41, 0x56, 0x47, 0x30, 0x30, 0x30, 0x30, 0x34 };


        public bool isRTCRead = false;
        public bool isRTCWrite = false;
        public bool isSNoWrite = false;
        public bool isSNoRead = false;
        public bool isRAMClear = false;
        public bool isCompleted = false;
        public string cmdName = string.Empty;

        public int dataSendCount = 0;

        #endregion

        public MasterForm()
        {
            MUTSerialCOM.DataReceived += new dataReceived(MUTSerialCOM_DataReceived);
            InitializeComponent();
        }

        // -------------------------------- RESPONSE DATA --------------------
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

                if (ReceiveStr[0] != 0)
                {
                    string TxtStr = string.Empty;

                    //if (CmdCount == 2)
                    //{
                    //    Buffer.BlockCopy(ReceiveStr, 40, ServerNonce, 0, 8);
                    //}

                    int recLength = ReceiveStr[2] + 2;
                    for (int i = 0; i < recLength; i++)
                    {
                        TxtStr = TxtStr + " " + ReceiveStr[i].ToString("X2");
                    }
                    UpdateLogStatus(" Response Data : " + TxtStr);
                    // dataSendCount++;
                    return;
                }
            }
            catch (Exception ex)
            {
                UpdateLogStatus("Error :" + ex.Message);
            }
        }



        // --------------------------------- LOAD METHODS -----------------------
        private void LoadCOMPort()
        {
            //COMM Port
            string[] Port = SerialPort.GetPortNames();
            cmbSerialPort.Items.AddRange(Port);
            cmbSerialPort.SelectedIndex = 0;
        }

        private void MasterForm_Load(object sender, EventArgs e)
        {
            dataSendCount = 0;
            LoadCOMPort();
            ReadSystemRTC.Enabled = true;

        }


        // ----------------------------------- BUTTON CLICK -------------------------
        private void btnSerialPortOpen_Click(object sender, EventArgs e)
        {
            try
            {


                if (btnSerialPortOpen.Text == "Open")
                {
                    bool isConnected;
                    string PortName = cmbSerialPort.Text;
                    isConnected = MUTSerialCOM.OpenCOM(PortName, 9600, 8, Parity.None, StopBits.One);
                    if (isConnected == true)
                    {

                        MessageBox.Show("Successfully COM Port opened", popUpTitle);
                        UpdateLogStatus("Successfully Serial COM Port opened");
                        btnSerialPortOpen.Text = "Close";
                        btnSerialPortOpen.ForeColor = System.Drawing.Color.Red;
                        btnSerialPortOpen.Image = FinalTesting.Properties.Resources.UnPlug;
                        //btnSerialPortClose.Visible = true;
                        //btnSerialPortOpen.Visible = false;

                    }
                    else
                    {
                        MessageBox.Show("Please select another COM", popUpTitle);
                    }
                }
                else //if (btnSerialPortOpen.Text == "Close")
                {
                    MUTSerialCOM.Close();
                    //  isConnected = false;
                    btnSerialPortOpen.Text = "Open";
                    btnSerialPortOpen.ForeColor = System.Drawing.Color.Teal;
                    btnSerialPortOpen.Image = FinalTesting.Properties.Resources.plug;


                }


            }
            catch (Exception ex) { throw ex; }
        }


        //private bool CheckConnection()
        //{
        //    return isConnected;
        //}

        private void btnClose_Click(object sender, EventArgs e)
        {

            MUTSerialCOM.Close();
            //this.btnSerialPortClose.Visible = false;
            btnSerialPortOpen.Visible = true;
            btnStatusFail.Visible = false;
            btnStatusGood.Visible = false;
            btnSetRTC.Visible = false;
            btnReadRTC.Visible = false;
            btnWriteSNo.Visible = false;
            btnRAMClear.Visible = false;
            txtSNo.Clear();
            lblRTCRead.Text = "";

        }


        private void btnTest_Click(object sender, EventArgs e)
        {
            btnStatusFail.Visible = false;
            btnStatusGood.Visible = false;
            try
            {
                OpticalTest = 0;
                CmdCount = 0;
                timer1.Enabled = true;
            }
            catch (Exception ex)
            {
                UpdateLogStatus(Convert.ToString(ex));
            }

        }

        private void btnWriteSNo_Click(object sender, EventArgs e)
        {
            SerialNoWriteToMeter();
        }

        private bool CheckSpl(string input)
        {
            bool isSpecialChar = false;
            string specialChars = "`~!@#$%^&*()-_+=/|?<>,.[]{};:'\"";

            foreach (char c in input)
            {
                if (specialChars.Contains(c))
                {
                    isSpecialChar = true;
                }
            }

            return isSpecialChar;
        }
        private void btnSetRTC_Click(object sender, EventArgs e)
        {
            RTCWriteToMeter();
        }

        private void btnReadRTC_Click(object sender, EventArgs e)
        {
            //if(CheckConnection())
            //{
            try
            {
                RTCBox.Clear();
                isRTCRead = true;
                CmdCount = 0;
                isWriteProcessStatus = false;
                ServerNonce = new byte[8];
                bw.RunWorkerAsync();
            }
            catch { }
        }

        private void btnRAMClear_Click(object sender, EventArgs e)
        {
            //if(CheckConnection())
            //{
            try
            {
                RTCBox.Clear();
                isRAMClear = true;
                CmdCount = 0;
                isWriteProcessStatus = false;
                ServerNonce = new byte[8];
                bw.RunWorkerAsync();
            }
            catch { }
            //}           
            //  else
            //{
            //    MessageBox.Show("Please check the port connection");
            //}
        }

        // ---------------------------------COMMON METHODS--------------------
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
        private string DateTimeToHex(string DateAndTime)
        {
            int year = Convert.ToInt32(DateAndTime);
            var data = year.ToString("X");
            return data;
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
        static string ConvertBytesToAscii(byte[] bytes)
        {
            Encoding asciiEncoding = Encoding.ASCII;
            string asciiString = asciiEncoding.GetString(bytes);
            return asciiString;
        }

        // -------------------------------------------- DLMS COMMANDS ---------------------------------
        private void SNRMCmd()
        {
            try
            {
                byte[] SNRMCmd = comm.SNRMCommand();
                string ss = String.Concat(SNRMCmd.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("  SNRM Request : " + ss);
                // this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText("\n" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss ") + "  SNRM Request : " + ss)));

                MUTSerialCOM.Send(SNRMCmd);
                //Thread.Sleep(250);
                btnStatusFail.Image = FinalTesting.Properties.Resources.thumbsups2;
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

                securityKey.CNonce.CopyTo(objAARQ.AARQCommandBytes, len);  // ClientSystemTitle = CNonce
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
                UpdateLogStatus("  AARQ Request : " + _aarq);
                // this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText("\n" + DateTime.Now.ToString("\ndd/MM/yyyy HH:mm:ss ") + "  AARQ Request : " + _aarq)));
                MUTSerialCOM.Send(objAARQ.AARQCommandBytes);
                Thread.Sleep(500);

                Buffer.BlockCopy(ReceiveStr, 40, ServerNonce, 0, 8);   // 28.09.2023 server Nonce
                string sNonce = string.Concat(ServerNonce.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("SERVER NONCE: " + sNonce);
                Buffer.BlockCopy(ReceiveStr, 65, StoC, 0, 16);
                string StoC_Str = string.Concat(StoC.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("S to C : " + StoC_Str);
                // this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "S to C : " + StoC_Str + Environment.NewLine)));

                byte[] tempPwd = securityKey.SecretKey; // Encoding.UTF8.GetBytes("AVG_SECRET_ASSC2");
                DLMS_AES.Encrypt(StoC, tempPwd);

                string EncryptStr = string.Concat(StoC.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("Encrypted S to C : " + EncryptStr);
                //  this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "Encrypted S to C : " + EncryptStr + Environment.NewLine)));
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
                invocationCounter = IterateInvocation(invocationCounter);

                byte[] _FinalNonce = FrameNonce(securityKey.CNonce, invocationCounter, securityKey.CNonce.Length);
                byte[] _AAD = FrameAAD(securityKey.PasswordKey);

                Buffer.BlockCopy(Encrypt_StoC, 0, objAction.ActPlainText, 15, 16);
                string PlainStr = String.Concat(objAction.ActPlainText.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus(" Action CIPHER : " + PlainStr);
                // this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\nAction CIPHER : " + PlainStr + Environment.NewLine)));


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
                UpdateLogStatus(" ACTION REQUEST : " + finalCommand);
                //   this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\nACTION REQUEST : " + finalCommand + Environment.NewLine)));
                //               CmdCount = 2;   //25.08.2023 for server Nonce
                MUTSerialCOM.Send(FinalCmd);
                Thread.Sleep(500);
            }

            catch { }

        }

        // ------------------------------ RTC LOGICAL NAME, READ & WRITE COMMANDS ---------------------
        private void RTCOBISCodeReadCmd()
        {
            OBISReadRequest objDataReq = new OBISReadRequest();
            SecurityKeys securityKey = new SecurityKeys();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();
            try
            {
                invocationCounter = IterateInvocation(invocationCounter);
                byte[] FinalCmd;
                Buffer.BlockCopy(objDataReq.PlainText, 0, objDataReq.OBISPlainText, 0, 11);
                objDataReq.OBISPlainText[11] = 0x01;
                objDataReq.OBISPlainText[12] = 0x00;
                //  Buffer.BlockCopy(objDataReq.InvocationCounter, 0, objDataReq.InvocationCounterBytes, 0, 3);
                objDataReq.InvocationCounterBytes[3] = 0x00;

                byte[] _FinalNonce = FrameNonce(securityKey.CNonce, invocationCounter, securityKey.CNonce.Length);
                byte[] _AAD = FrameAAD(securityKey.PasswordKey);
                _CipheredData = new byte[13];

                byte[] bResult = AESGCM.SimpleEncrypt(objDataReq.OBISPlainText, DedicatedKey, _FinalNonce, _AAD);

                string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus(" DATA REQUEST CIPHER : " + ss);
                //   this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\nDATA REQUEST CIPHER : " + ss + Environment.NewLine)));
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
                UpdateLogStatus(" RTC OBIS Code) : " + finalCommand);
                //   this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\nRTC OBIS Code) : " + finalCommand + Environment.NewLine)));

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
                invocationCounter = IterateInvocation(invocationCounter);
                byte[] bInstantData;
                byte[] FinalCmd;
                Buffer.BlockCopy(objDataReq.PlainText, 0, objDataReq.OBISPlainText, 0, 11);
                objDataReq.OBISPlainText[11] = 0x02;
                objDataReq.OBISPlainText[12] = 0x00;
                //  Buffer.BlockCopy(objDataReq.InvocationCounter, 0, objDataReq.InvocationCounterBytes, 0, 3);
                objDataReq.InvocationCounterBytes[3] = 0x01;


                //  byte[] FinalCmd; // 
                try
                {

                    byte[] _FinalNonce = FrameNonce(securityKey.CNonce, invocationCounter, securityKey.CNonce.Length);
                    byte[] _AAD = FrameAAD(securityKey.PasswordKey);
                    _CipheredData = new byte[13];

                    byte[] bResult = AESGCM.SimpleEncrypt(objDataReq.OBISPlainText, DedicatedKey, _FinalNonce, _AAD);

                    string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));
                    UpdateLogStatus("\nDATA REQUEST CIPHER : " + ss);
                    //this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\nDATA REQUEST CIPHER : " + ss + Environment.NewLine)));
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
                    UpdateLogStatus(" RTC Request : " + finalCommand);
                    //  this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\n RTC Request : " + finalCommand + Environment.NewLine)));
                    MUTSerialCOM.Send(FinalCmd); Thread.Sleep(500);


                    if (ReceiveStr[0] != 0)
                    {
                        int length = ReceiveStr[2];
                        if (ReceiveStr[length + 1] == 0x7E)
                        {
                            if (length + 2 == 0x33)
                            {

                                // byte 12 (From 0 ) - Length
                                // byte 14,15,16,17  - FrameCount
                                // From Byte 18      - Data Started (Length - 6)
                                // Server Nonce

                                //  byte[] _SDecNonce = Encoding.UTF8.GetBytes("AVG12345");

                                byte[] _SDecNonce = ServerNonce; //new byte[] { 0x41, 0x56, 0x47, 0x31, 0x32, 0x33, 0x32, 0x33 }; // (Server System Title from AARE )                   
                                byte[] _SDecsubNonce_FrameCount = new byte[4]; // { 0x00, 0x00, 0x00, 0x01 };
                                Buffer.BlockCopy(ReceiveStr, 14, _SDecsubNonce_FrameCount, 0, _SDecsubNonce_FrameCount.Length);

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

                                UpdateLogStatus("RTC DECODED : " + sCipherResults);
                                //  this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "RTC DECODED : " + sCipherResults + Environment.NewLine)));

                                isWriteProcessStatus = bDecryptResult[3] == 00 ? true : false;
                                UpdateLogStatus(" WriteStatus : " + isWriteProcessStatus.ToString());
                                //   this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "WriteStatus : " + isWriteProcessStatus.ToString() + Environment.NewLine)));


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

                                this.lblRTCRead.Invoke((MethodInvoker)(() => lblRTCRead.Text = Date + "-" + _Month + "-" + Year + " " + Day + " " + Hour + ":" + _Minute + ":" + _Seconds));


                                isWriteProcessStatus = bDecryptResult[3] == 00 ? true : false;

                                if (isWriteProcessStatus == true)
                                {
                                    btnRAMClear.Invoke((MethodInvoker)(() => btnRAMClear.Visible = true));

                                }
                            }
                            else
                            {
                                MessageBox.Show("Something went wrong");
                            }
                        }







                    }
                }
                catch (Exception ex)
                {

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
                invocationCounter = IterateInvocation(invocationCounter);

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

                    byte[] _FinalNonce = FrameNonce(securityKey.CNonce, invocationCounter, securityKey.CNonce.Length);
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
                    byte[] FixedBytes = new byte[8] { 0x03, 0x61, 0x54, 0xA7, 0x32, 0xE6, 0xE6, 0x00 };
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

                    UpdateLogStatus(" RTC Request : " + finalCommand);
                    // this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\n RTC Request : " + finalCommand + Environment.NewLine)));
                    MUTSerialCOM.Send(FinalCmd); Thread.Sleep(500);


                    if (ReceiveStr[0] != 0)
                    {
                        // byte 12 (From 0 ) - Length
                        // byte 14,15,16,17  - FrameCount
                        // From Byte 18      - Data Started (Length - 6)
                        // Server Nonce

                        //  byte[] _SDecNonce = Encoding.UTF8.GetBytes("AVG12345");

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
                            btnReadRTC.Invoke((MethodInvoker)(() => btnReadRTC.Visible = true));

                        }

                    }
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

        // --------------------------- SERIAL NUMBER LOGICAL NAME, READ & WRITE COMMANDS ----------------
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
                invocationCounter = IterateInvocation(invocationCounter);
                byte[] FinalCmd;

                //   byte[] PlainText = { 0xC0, 0x01, 0xC1, 0x00, 0x01, 0x01, 0x00, 0x60, 0x02, 0x83, 0x00 };
                Buffer.BlockCopy(serialNumber.PlainText, 0, serialNumber.SPlainText, 0, 11);
                serialNumber.SPlainText[11] = 0x01;
                serialNumber.SPlainText[12] = 0x00;
                //Buffer.BlockCopy(serialNumber.InvocationCounter, 0, serialNumber.SInvocationCounter, 0, 3); //---check this 2 or 3
                //serialNumber.SInvocationCounter[3] = 0x00;

                byte[] _FinalNonce = FrameNonce(securityKeys.CNonce, invocationCounter, securityKeys.CNonce.Length);
                byte[] _AAD = FrameAAD(securityKeys.PasswordKey);
                _CipheredData = new byte[13];

                byte[] bResult = AESGCM.SimpleEncrypt(serialNumber.SPlainText, DedicatedKey, _FinalNonce, _AAD);

                string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));

                //   this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\nSerial No CIPHER : " + ss + Environment.NewLine)));
                UpdateLogStatus("\nSerial No CIPHER : " + ss);
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

                UpdateLogStatus(" Serial No Logical Name : " + finalCommand);
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

                UpdateLogStatus(" Serial No OBIS CODE DECODED : " + sCipherResults);
                //  this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "Serial No OBIS CODE DECODED : " + sCipherResults + Environment.NewLine)));

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
                invocationCounter = IterateInvocation(invocationCounter);
                byte[] PlainText = { 0xC0, 0x01, 0xC1, 0x00, 0x01, 0x01, 0x00, 0x60, 0x02, 0x83, 0x00 };
                Buffer.BlockCopy(PlainText, 0, serialNumber.SPlainText, 0, 11);
                serialNumber.SPlainText[11] = 0x02;
                serialNumber.SPlainText[12] = 0x00;
                //Buffer.BlockCopy(invocationCounter, 0, serialNumber.SInvocationCounter, 0, 3);
                //serialNumber.SInvocationCounter[3] = 0x01;


                byte[] FinalCmd; // 
                try
                {

                    byte[] _FinalNonce = FrameNonce(securityKeys.CNonce, invocationCounter, securityKeys.CNonce.Length);
                    byte[] _AAD = FrameAAD(securityKeys.PasswordKey);
                    _CipheredData = new byte[13];

                    byte[] bResult = AESGCM.SimpleEncrypt(serialNumber.SPlainText, DedicatedKey, _FinalNonce, _AAD);

                    string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));
                    UpdateLogStatus(" S.No REQUEST CIPHER : " + ss);
                    //    this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\nS.No REQUEST CIPHER : " + ss + Environment.NewLine)));
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

                    UpdateLogStatus(" Serial No Request : " + finalCommand);
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
                        byte[] _SDecsubNonce_FrameCount = new byte[4]; // { 0x00, 0x00, 0x00, 0x01 };
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

                        string sCipherResults = String.Concat(bDecryptResult.Select(b => b.ToString("X2") + " "));

                        UpdateLogStatus(" Serial No DECODED : " + sCipherResults);
                        this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "Serial No DECODED : " + sCipherResults + Environment.NewLine)));

                        // Serial Number Byte to Ascii Conversion
                        byte[] SNoHex = new byte[8];
                        Buffer.BlockCopy(bDecryptResult, 6, SNoHex, 0, 8);
                        string asciiString = ConvertBytesToAscii(SNoHex);


                        // this.txtSNo.Text = asciiString; //Convert.ToString(Convert.ToInt16(Convert.ToInt32(bDecryptResult[6] << 8 | bDecryptResult[7]).ToString("X"), 16));
                        this.txtSNo.Invoke((MethodInvoker)(() => txtSNo.Text = asciiString));
                        //CmdCount += 1;
                    }
                }
                catch (Exception ex)
                {

                }


            }
            catch { };
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
                invocationCounter = IterateInvocation(invocationCounter);
                byte[] PlainText = new byte[28];
                byte[] FPlainText = new byte[] { 0xC1, 0x01, 0xC1, 0x00, 0x01, 0x01, 0x00, 0x60, 0x02, 0x84, 0x00, 0x02, 0x00, 0x0A, 0x0D };
                Buffer.BlockCopy(FPlainText, 0, PlainText, 0, FPlainText.Length);
                Buffer.BlockCopy(bSerialNo, 0, PlainText, 15, bSerialNo.Length);

                // byte[] InvocationCounter = new byte[4] { 0x00, 0x00, 0x00, 0x00 };
                byte[] FinalCmd; // 
                try
                {
                    byte[] _FinalNonce = FrameNonce(securityKeys.CNonce, invocationCounter, securityKeys.CNonce.Length);
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
                    UpdateLogStatus(" Serial Number Write Request : " + finalCommand + ss);
                    //this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\n Serial Number Write Request : " + finalCommand + Environment.NewLine)));
                    MUTSerialCOM.Send(FinalCmd);
                    Thread.Sleep(500);


                    if (ReceiveStr[0] != 0)
                    {
                        // byte 12 (From 0 ) - Length
                        // byte 14,15,16,17  - FrameCount
                        // From Byte 18      - Data Started (Length - 6)
                        // Server Nonce

                        //  byte[] _SDecNonce = Encoding.UTF8.GetBytes("AVG12345");

                        byte[] _SDecNonce = ServerNonce; //new byte[] { 0x41, 0x56, 0x47, 0x31, 0x32, 0x33, 0x32, 0x33 }; // (Server System Title from AARE )                   
                        byte[] _SDecsubNonce_FrameCount = new byte[4]; // { 0x00, 0x00, 0x00, 0x00 };
                        Buffer.BlockCopy(ReceiveStr, 14, _SDecsubNonce_FrameCount, 0, _SDecsubNonce_FrameCount.Length);

                        string serverNonceData = String.Concat(ServerNonce.Select(b => b.ToString("X2") + " "));
                        this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "Server Nonce : " + serverNonceData + Environment.NewLine)));
                        string assignedserverNonceData = String.Concat(_SDecNonce.Select(b => b.ToString("X2") + " "));
                        this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "Assigned Server Nonce : " + assignedserverNonceData + Environment.NewLine)));


                        byte[] _SFinalNonce = FrameNonce(_SDecNonce, _SDecsubNonce_FrameCount, _SDecNonce.Length);

                        //Printing
                        string fiinalNonceData = String.Concat(_SFinalNonce.Select(b => b.ToString("X2") + " "));

                        UpdateLogStatus("Final Nonce : " + fiinalNonceData);
                        this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "Final Nonce : " + fiinalNonceData + Environment.NewLine)));



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
                            this.btnSetRTC.Invoke((MethodInvoker)(() => btnSetRTC.Visible = true));

                        }
                        UpdateLogStatus("WriteStatus : " + isWriteProcessStatus.ToString());


                    }
                }
                catch (Exception ex)
                { }
            }
            catch (Exception ex)
            { }
        }


        // -----------------------------RAM CLEAR WRITE -------------------------------
        private void RAMClearWrite()
        {
            byte[] bInstantData;
            SecurityKeys securityKeys = new SecurityKeys();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();


            try
            {
                invocationCounter = IterateInvocation(invocationCounter);
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
                    MUTSerialCOM.Send(FinalCmd); Thread.Sleep(500);


                    if (ReceiveStr[0] != 0)
                    {
                        // byte 12 (From 0 ) - Length
                        // byte 14,15,16,17  - FrameCount
                        // From Byte 18      - Data Started (Length - 6)
                        // Server Nonce

                        //  byte[] _SDecNonce = Encoding.UTF8.GetBytes("AVG12345");

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
                        }

                        this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "WriteStatus : " + isWriteProcessStatus.ToString() + Environment.NewLine)));


                    }
                }
                catch (Exception ex) { }
            }
            catch (Exception ex) { }

        }


        // --------------------------------- TIMERS --------------------------------

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (CmdCount == 0)
                {
                    byte[] SNRMCmd = comm.SNRMCommand();
                    string ss = String.Concat(SNRMCmd.Select(b => b.ToString("X2") + " "));
                    UpdateLogStatus("  SNRM Request : " + ss);
                    MUTSerialCOM.Send(SNRMCmd); Thread.Sleep(250);
                    OpticalTest++;
                    if (OpticalTest >= 2)
                    {
                        UpdateLogStatus("  Optical Test Failed... ");
                        btnStatusFail.Visible = true;
                        btnStatusGood.Visible = false;
                        timer1.Enabled = false;
                    }
                    //  btnStatusFail.Image = Properties.Resources.thumbsups2;

                }
                else if (CmdCount == 1)
                {
                    byte[] DisconnectCmd = comm.DisconnectCommand();
                    string dd = String.Concat(DisconnectCmd.Select(b => b.ToString("X2") + " "));
                    //   this.RTCBox.AppendText("\n" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss ") + "  Disconnect Request : " + dd);
                    UpdateLogStatus("  Disconnect Request : " + dd);
                    MUTSerialCOM.Send(DisconnectCmd); Thread.Sleep(250);

                    timer1.Enabled = false;
                    btnStatusFail.Visible = false;
                    btnStatusGood.Visible = true;
                    btnWriteSNo.Visible = true;

                }
                else
                {
                    timer1.Enabled = false;
                }
            }
            catch
            {

            }

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                if (CmdCount == 0)
                {
                    SNRMCmd();
                    int milliseconds = 650;
                    Thread.Sleep(milliseconds);
                }
                else if (CmdCount == 1)
                {
                    AARQCommand();
                }
                else if (CmdCount == 2)
                {
                    ActionRequestCmd();
                    int milliseconds = 650;
                    Thread.Sleep(milliseconds);
                }
                else if (CmdCount == 3)
                {
                    SNOLogicalName();
                }
                else if (CmdCount == 4)
                {
                    ReadSerialNo();
                }
                else
                {
                    byte[] DisconnectCmd = comm.DisconnectCommand();
                    string dd = String.Concat(DisconnectCmd.Select(b => b.ToString("X2") + " "));
                    UpdateLogStatus("  Disconnect Request : " + dd);
                    //     this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText("\n" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss ") + "  Disconnect Request : " + dd)));
                    MUTSerialCOM.Send(DisconnectCmd); Thread.Sleep(250);
                    timer2.Enabled = false;
                    //isSNoWrite = false;
                    //bw.
                }


            }
            catch { }

        }

        private void ReadSystemRTC_Tick(object sender, EventArgs e)
        {
            txtRTCWrite.Text = DateTime.Now.ToString("dd-MM-yyyy dddd HH:mm:ss"); //"02-02-2024 Friday 7:10:00";
        }


        // -------------------------- TIMER READ & WRITE METHODS --------------------------




        //------------------------------ DO WORKER METHODS -----------------------------------------

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            ReadInvocationCounter();
            WriteSerialNo();
         
            WriteRealTimeClock();
            
            WriteRAMClear();

            Thread.Sleep(15000);

            ReadInvocationCounter();
            ReadRealTimeClock();
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string msg = isWriteProcessStatus == true ? "Write or Read Process successfully Completed .!" : "Write or Read Process fail !";
            UpdateLogStatus("  Process Completed ");
            MessageBox.Show(msg, popUpTitle);
        }

        //--------------------------------- ALL COMMANDS FOR DO WORKER ----------------------------
        private void WriteSerialNo()
        {
            CmdCount = 0;
            try
            {
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 0));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Visible = true));
                int waitSecond = 100;

                byte[] DisconnectCmd = comm.DisconnectCommand();
                MUTSerialCOM.Send(DisconnectCmd);
                Thread.Sleep(waitSecond);

                SNRMCmd();
                Thread.Sleep(650);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "SNRM"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 10));

                AARQCommand();
                Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "AARQ"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 40));

                ActionRequestCmd();
                Thread.Sleep(650);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "ACTION"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 50));

                WriteSNo();
                Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "SERIAL NUMBER WRITE"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 90));

                DisconnectCmd = comm.DisconnectCommand();
                string dd = String.Concat(DisconnectCmd.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("  Disconnect Request : " + dd);
                MUTSerialCOM.Send(DisconnectCmd);
                Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "DISCONNECT"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 100));

                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Visible = false));
                dataSendCount++; isSNoWrite = false; lblStatus.Text = "Serial Number Write Successfully...";
            }
            catch
            {
            }
        }
        private void WriteRealTimeClock()
        {
            try
            {
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 0));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Visible = true));
                int waitSecond = 100;

                byte[] DisconnectCmd = comm.DisconnectCommand();
                MUTSerialCOM.Send(DisconnectCmd);
                Thread.Sleep(waitSecond);

                SNRMCmd();
                Thread.Sleep(650);  // 1000
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "SNRM"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 10));

                AARQCommand();
                Thread.Sleep(waitSecond);  //
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "AARQ"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 30));

                ActionRequestCmd();
                Thread.Sleep(650);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "ACTION"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 50));

                WriteRTC();
                Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "RTC WRITE"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 90));

                DisconnectCmd = comm.DisconnectCommand();
                string dd = String.Concat(DisconnectCmd.Select(b => b.ToString("X2") + " "));
                //     this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText("\n" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss ") + "  Disconnect Request : " + dd)));
                UpdateLogStatus("  Disconnect Request : " + dd);
                MUTSerialCOM.Send(DisconnectCmd);
                Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "DISCONNECT"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 100));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Visible = false));
                isRTCWrite = false; lblStatus.Text = "RTC Write Successfully...";
            }
            catch { }
        }
        private void ReadRealTimeClock()
        {
            CmdCount = 0;
            try
            {
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 0));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Visible = true));
                int waitSecond = 100;

                byte[] DisconnectCmd = comm.DisconnectCommand();
                MUTSerialCOM.Send(DisconnectCmd);
                Thread.Sleep(waitSecond);

                SNRMCmd();
                Thread.Sleep(650);  // 1000
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "SNRM"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 10));
                Thread.Sleep(waitSecond);

                AARQCommand();
                Thread.Sleep(waitSecond);  // 1000
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "AARQ"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 30));

                ActionRequestCmd();
                Thread.Sleep(650);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "ACTION"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 50));

                RTCOBISCodeReadCmd();
                // Thread.Sleep(650); //waitSecond
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "RTC LOGICAL NAME"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 70));

                ReadRTC();
                // Thread.Sleep(waitSecond);//waitSecond
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "RTC READ"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 90));

                DisconnectCmd = comm.DisconnectCommand();
                string dd = String.Concat(DisconnectCmd.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("  Disconnect Request : " + dd);
                MUTSerialCOM.Send(DisconnectCmd);
                Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "DISCONNECT"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 100));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Visible = false));
                isRTCRead = false;
                lblStatus.Text = "RTC Read Successfully...";
            }
            catch { }
        }
        private void WriteRAMClear()
        {
            try
            {
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 0));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Visible = true));
                int waitSecond = 100;

                byte[] DisconnectCmd = comm.DisconnectCommand();
                MUTSerialCOM.Send(DisconnectCmd);
                Thread.Sleep(waitSecond);

                SNRMCmd();
                Thread.Sleep(650);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "SNRM"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 10));

                AARQCommand();
                Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "AARQ"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 30));

                ActionRequestCmd();
                Thread.Sleep(650);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "ACTION"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 50));

                RAMClearWrite();
                Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "RAM CLEAR"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 90));

                DisconnectCmd = comm.DisconnectCommand();
                string dd = String.Concat(DisconnectCmd.Select(b => b.ToString("X2") + " "));
                UpdateLogStatus("  Disconnect Request : " + dd);
                MUTSerialCOM.Send(DisconnectCmd);
                Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "DISCONNECT"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 100));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Visible = false));
                lblStatus.Text = "RAM Clear Successfully...";
            }
            catch { }
        }

        private void ReadInvocationCounter()
        {
            CmdCount = 0;
            try
            {
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 0));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Visible = true));
                int waitSecond = 100;

                byte[] DisconnectCmd = comm.PublicClientDisconnect();
                MUTSerialCOM.Send(DisconnectCmd);
                Thread.Sleep(waitSecond);

                PublicClientSNRMCmd();
                Thread.Sleep(650);  // 1000
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "SNRM"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 10));
                Thread.Sleep(waitSecond);

                PublicClientAARQ();
                Thread.Sleep(waitSecond);  // 1000
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "AARQ"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 50));


                PublicClientLogicalName();
                // Thread.Sleep(650); //waitSecond
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "Invocation LogicalName"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 70));

                ReadInvocation();
                // Thread.Sleep(waitSecond);//waitSecond
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "Invocation Counter"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 90));

                DisconnectCmd = comm.PublicClientDisconnect();
                string dd = String.Concat(DisconnectCmd.Select(b => b.ToString("X2") + " "));
                //     this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText("\n" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss ") + "  Disconnect Request : " + dd)));
                UpdateLogStatus("  Disconnect Request : " + dd);
                MUTSerialCOM.Send(DisconnectCmd);
                Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "DISCONNECT"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 100));

                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Visible = false));
                isRTCRead = false;
            }
            catch { }
        }

        //------------------------------------------ Log Updates -------------------------------
        private void UpdateLogStatus(string message)
        {
            try
            {
                RTCBox.BeginInvoke(new Action(() =>
                {
                    RTCBox.AppendText(DateTime.Now.ToString("\ndd-MM-yyyy HH:mm:ss") + ": " + message + Environment.NewLine);
                }));
            }
            catch { }
        }

        // ------------------------------------ Public Client Commands (InvocationCounter) -----------------

        private void PublicClientSNRMCmd()
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
                UpdateLogStatus(" Public Client SNRM : " + finalCommand);
                MUTSerialCOM.Send(snrmCmd); Thread.Sleep(250);
                // MessageBox.Show("Success");
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
                UpdateLogStatus(" Public Client AARQ : " + finalCommand);
                MUTSerialCOM.Send(AARQcmd); Thread.Sleep(500);
                // MessageBox.Show("Success");
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
                MUTSerialCOM.Send(logicalNameCmd); Thread.Sleep(500);
                // MessageBox.Show("Success");
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
                MUTSerialCOM.Send(ReadInvocationCmd); Thread.Sleep(500);
                // MessageBox.Show("Success");

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
                        }
                        else
                        {
                            MessageBox.Show("Something went wrong");
                        }
                    }
                }
            }
            catch { }
        }

        //------------------------------------------ COMMON -------------------------
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

        private void SerialNoWriteToMeter()
        {
            try
            {
                RTCBox.Clear();
                if (txtSNo.Text == " " || txtSNo.Text.Length < 8)
                {
                    MessageBox.Show("Serialnumber should not be empty or less than 13 character...", popUpTitle);
                }
                else if (txtSNo.Text.Contains(" ") || CheckSpl(txtSNo.Text))
                {
                    MessageBox.Show("Please Check that you entered the serial number with white space or Special character...", popUpTitle);
                }
                else
                {
                    // isSNoWrite = true;
                    CmdCount = 0;
                    isWriteProcessStatus = false;
                    ServerNonce = new byte[8];
                    bw.RunWorkerAsync();
                }
            }
            catch { }
        }

        private void RTCWriteToMeter()
        {
            try
            {
                RTCBox.Clear();
                // isRTCWrite = true;
                CmdCount = 0;
                isWriteProcessStatus = false;
                ServerNonce = new byte[8];
            }
            catch (Exception ex)
            {
            }
        }

        private void btnReadDatas_Click(object sender, EventArgs e)
        {
            try
            {
                SerialNoWriteToMeter();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
            }
        }
    }
}
