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
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Keller.SPM.Communication.MUT;

namespace AVG.ProductionProcess.FunctionalTest.Forms
{
    public partial class MainForm : Form
    {
        #region Global Object
        public SerialCOM MUTSerialCOM = new SerialCOM();
        CommonCommand comm = new CommonCommand();


        HDLC objHDLC = new HDLC();
        ErrorDetection objerror = new ErrorDetection();
        ControlBytes objControlByte = new ControlBytes();
        RequestTag objTag = new RequestTag();
        ClassId objcls = new ClassId();
        OBISCode objOBIS = new OBISCode();

        #endregion

        #region Global Variable
        public byte[] ReceiveStr;
        public int CmdCount = 0;
        public int OpticalTest = 0;
        public delegate void DelMethod(byte[] s);

        byte[] invocationCounter = new byte[4];

        byte[] Encrypt_StoC = new byte[16];
        byte[] StoC = new byte[16];
        byte[] _CipheredData;
        byte[] _AuthenticationTag = new byte[12];
        byte[] FinalNonce = new byte[12];
        byte[] AssociationKey = new byte[17];
        byte[] DedicatedKey = new byte[16];
        public byte[] ServerNonce = new byte[8];

        public bool isStartWrite = false;
        public bool isStopFunction = false;
        #endregion

        public MainForm()
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

                    this.RTCBox.AppendText("\n" + DateTime.Now.ToString("\ndd/MM/yyyy HH:mm:ss") + " Response Data : " + TxtStr + " \n");
                    return;
                }
            }
            catch (Exception ex) { }
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
                    if (isConnected == true)
                    {
                        btnCOMMOpen.Text = "COM Close";
                        btnCOMMOpen.ForeColor = Color.Red;
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
                    btnCOMMOpen.ForeColor = Color.Black;
                    MUTSerialCOM.Close();
                }

            }
            catch (Exception ex) { throw ex; }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadCOMPort();
        }











        //------------------------------------- COMMON METHODS -----------------------------
        private void LoadCOMPort()
        {
            string[] Port = SerialPort.GetPortNames();
            cmbSerialPort.Items.AddRange(Port);
            //cmbSerialPort1.SelectedIndex = 0;

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
        private void ReadFunctionalTest()
        {
            FunctionalTestLogicalName functionalTest = new FunctionalTestLogicalName();


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

                LogicalName(functionalTest.ClassId, functionalTest.OBISCode);
                // Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "Functional Logical NAME"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 80));

                GetFunctionalTest(functionalTest.ClassId, functionalTest.OBISCode);
                // Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "Test Read"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 90));

                DisconnectCmd = comm.DisconnectCommand();
                string dd = String.Concat(DisconnectCmd.Select(b => b.ToString("X2") + " "));
                this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText("\n" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss ") + "  Disconnect Request : " + dd)));
                MUTSerialCOM.Send(DisconnectCmd);
                Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "DISCONNECT"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 100));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Visible = false));
            }
            catch { }
        }
        private void LogicalName(byte[] clsId, byte[] obisCode)
        {
            FunctionalTestLogicalName functionalTest = new FunctionalTestLogicalName();
            SecurityKeys securityKey = new SecurityKeys();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();
            try
            {
                invocationCounter = IterateInvocation(invocationCounter);
                byte[] FinalCmd;
                Buffer.BlockCopy(clsId, 0, functionalTest.PlainText, 3, 2);
                Buffer.BlockCopy(obisCode, 0, functionalTest.PlainText, 5, 6);

                byte[] _FinalNonce = FrameNonce(securityKey.CNonce, invocationCounter, securityKey.CNonce.Length);
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
                this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\nFunctional OBIS Code : " + finalCommand + Environment.NewLine)));

                MUTSerialCOM.Send(FinalCmd); Thread.Sleep(500);



            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
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
                invocationCounter = IterateInvocation(invocationCounter);
                byte[] FinalCmd;
                Buffer.BlockCopy(clsId, 0, functionalTest.PlainText, 3, 2);
                Buffer.BlockCopy(obisCode, 0, functionalTest.PlainText, 5, 6);

                // functionalTest.InvocationCounter[3] = 0x01;
                functionalTest.PlainText[11] = 0x02;

                byte[] _FinalNonce = FrameNonce(securityKey.CNonce, invocationCounter, securityKey.CNonce.Length);
                byte[] _AAD = FrameAAD(securityKey.PasswordKey);
                _CipheredData = new byte[13];

                byte[] bResult = AESGCM.SimpleEncrypt(functionalTest.PlainText, DedicatedKey, _FinalNonce, _AAD);

                string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));
                this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\nFUNCTIONAL TEST CIPHER : " + ss + Environment.NewLine)));
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
                this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\nFUNCTIONAL OBIS CODE : " + finalCommand + Environment.NewLine)));

                MUTSerialCOM.Send(FinalCmd); Thread.Sleep(500);


                if (ReceiveStr[0] != 0)
                {
                    // byte 12 (From 0 ) - Length
                    // byte 14,15,16,17  - FrameCount
                    // From Byte 18      - Data Started (Length - 6)
                    // Server Nonce

                    //  byte[] _SDecNonce = Encoding.UTF8.GetBytes("AVG12345");

                    byte[] _SDecNonce = ServerNonce; //new byte[] { 0x41, 0x56, 0x47, 0x30, 0x30, 0x30, 0x30, 0x34 }; // (Server System Title from AARE )                   
                    byte[] _SDecsubNonce_FrameCount = new byte[4]; // { 0x00, 0x00, 0x00, 0x03 };
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

                    if (bDecryptResult[0] == 0xC4)
                    {
                        string sCipherResults = String.Concat(bDecryptResult.Select(b => b.ToString("X2") + " "));

                        //this.RTCBox.AppendText(Environment.NewLine + "Serial No DECODED : " + sCipherResults + Environment.NewLine);
                        this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "TEST RESULT DECODED : " + sCipherResults + Environment.NewLine)));

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
                        FinalResult(resultValue);
                    }
                    else
                    {
                        MessageBox.Show("Wrong Response...");
                    }
                }


            }
            catch (Exception ex)
            {
                ErrorDetection(ReceiveStr);
                //MessageBox.Show("Error: " + ex);
            }
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
                                RAMLive.ForeColor = Color.Red;
                                this.RAMLive.Invoke((MethodInvoker)(() => RAMLive.Text = "NOT OK"));
                            }
                            else if (result[i] == 1)
                            {
                                RAMLive.ForeColor = Color.Green;
                                RAMResult.ForeColor = Color.Green;
                                this.RAMLive.Invoke((MethodInvoker)(() => RAMLive.Text = "OK"));
                                this.RAMResult.Invoke((MethodInvoker)(() => RAMResult.Text = "OK"));
                            }
                            break;
                        case 27:
                            if (result[i] == 0)
                            {
                                flashLive.ForeColor = Color.Red;
                                this.flashLive.Invoke((MethodInvoker)(() => flashLive.Text = "NOT OK"));
                            }
                            else if (result[i] == 1)
                            {
                                flashLive.ForeColor = Color.Green;
                                flashResult.ForeColor = Color.Green;
                                this.flashLive.Invoke((MethodInvoker)(() => flashLive.Text = "OK"));
                                this.flashResult.Invoke((MethodInvoker)(() => flashResult.Text = "OK"));
                            }
                            break;

                        case 26:
                            if (result[i] == 0)
                            {
                                scrollLive.ForeColor = Color.Green;
                                scrollResult.ForeColor = Color.Green;
                                this.scrollLive.Invoke((MethodInvoker)(() => scrollLive.Text = "Relesed"));
                                //this.scrollResult.Invoke((MethodInvoker)(() => scrollResult.Text = "OK"));
                            }
                            else if (result[i] == 1)
                            {
                                scrollLive.ForeColor = Color.Red;
                                this.scrollLive.Invoke((MethodInvoker)(() => scrollLive.Text = "Pressed"));
                                this.scrollResult.Invoke((MethodInvoker)(() => scrollResult.Text = "OK"));
                            }
                            break;

                        case 24:
                            if (result[i] == 0)
                            {
                                TopLive.ForeColor = Color.Green;
                                topResult.ForeColor = Color.Green;
                                this.TopLive.Invoke((MethodInvoker)(() => TopLive.Text = "Closed"));
                                this.topResult.Invoke((MethodInvoker)(() => topResult.Text = "OK"));
                            }
                            else if (result[i] == 1)
                            {
                                TopLive.ForeColor = Color.Red;
                                this.TopLive.Invoke((MethodInvoker)(() => TopLive.Text = "Open"));
                            }
                            break;


                        //case 24:
                        //    if (result[i] == 0)
                        //    {
                        //        TerminalLive.ForeColor = Color.Green;
                        //        terminalResult.ForeColor = Color.Green;
                        //        this.TerminalLive.Invoke((MethodInvoker)(() => TerminalLive.Text = "Close"));
                        //        this.terminalResult.Invoke((MethodInvoker)(() => terminalResult.Text = "OK"));
                        //    }
                        //    else if (result[i] == 1)
                        //    {
                        //        TerminalLive.ForeColor = Color.Red;
                        //        this.TerminalLive.Invoke((MethodInvoker)(() => TerminalLive.Text = "Open"));
                        //    }
                        //    break;

                        case 23:
                            if (result[i] == 0)
                            {
                                magnetLive.ForeColor = Color.Green;
                                this.magnetLive.Invoke((MethodInvoker)(() => magnetLive.Text = "No Magnet"));
                            }
                            else if (result[i] == 1)
                            {
                                magnetLive.ForeColor = Color.Red;
                                this.magnetLive.Invoke((MethodInvoker)(() => magnetLive.Text = "Magnet"));
                                magnetResult.ForeColor = Color.Green;
                                this.magnetResult.Invoke((MethodInvoker)(() => magnetResult.Text = "Magnet OK"));
                            }
                            break;





                        //case 29:
                        //    if (result[i] == 1)
                        //    {
                        //        if (result[i + 1] == 1)
                        //        {
                        //            lineLive.ForeColor = Color.Green;
                        //            lineResult.ForeColor = Color.Green;
                        //            this.lineLive.Invoke((MethodInvoker)(() => lineLive.Text = "REVERSE"));
                        //            this.lineResult.Invoke((MethodInvoker)(() => lineLive.Text = "REVERSE"));
                        //        }
                        //        else if (result[i + 1] == 0)
                        //        {
                        //            lineLive.ForeColor = Color.Green;
                        //            lineResult.ForeColor = Color.Green;
                        //            this.lineLive.Invoke((MethodInvoker)(() => lineLive.Text = "FORWARD"));
                        //            this.lineResult.Invoke((MethodInvoker)(() => lineLive.Text = "FORWARD"));
                        //        }

                        //    }
                        //    else if (result[i] == 0)
                        //    {
                        //        lineLive.ForeColor = Color.Red;
                        //        this.lineLive.Invoke((MethodInvoker)(() => lineLive.Text = "Fault"));
                        //    }
                        //    break;
                        //case 27:
                        //    if (result[i] == 1)
                        //    {
                        //        if (result[i + 1] == 1)
                        //        {
                        //            neutralLive.ForeColor = Color.Green;
                        //            neutralResult.ForeColor = Color.Green;
                        //            this.neutralLive.Invoke((MethodInvoker)(() => neutralLive.Text = "REVERSE"));
                        //            this.neutralResult.Invoke((MethodInvoker)(() => neutralResult.Text = "REVERSE"));
                        //        }
                        //        else if (result[i + 1] == 0)
                        //        {
                        //            neutralLive.ForeColor = Color.Green;
                        //            neutralResult.ForeColor = Color.Green;
                        //            this.neutralLive.Invoke((MethodInvoker)(() => neutralLive.Text = "FORWARD"));
                        //            this.neutralResult.Invoke((MethodInvoker)(() => neutralResult.Text = "FORWARD"));
                        //        }
                        //    }
                        //    else if (result[i] == 0)
                        //    {
                        //        neutralLive.ForeColor = Color.Red;
                        //        this.neutralLive.Invoke((MethodInvoker)(() => neutralLive.Text = "Fault"));
                        //    }
                        //    break;
                       
                        
                       
                       
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex) { throw ex; }


        }
        static string ConvertByteToBits(byte value)
        {
            return Convert.ToString(value, 2).PadLeft(8, '0');
        }
        private void FunctionalTest(byte[] value)
        {
            byte[] bInstantData;
            SecurityKeys securityKeys = new SecurityKeys();
            ProtocolFlags protocolFlags = new ProtocolFlags();
            EncryptData encryptData = new EncryptData();
            ErrorDetection errorDetection = new ErrorDetection();


            try
            {
                invocationCounter = IterateInvocation(invocationCounter);
                byte[] FPlainText = new byte[] { 0xC1, 0x01, 0xC1, 0x00, 0x01, 0x01, 0x00, 0x60, 0x02, 0x80, 0x00, 0x02, 0x00, 0x12, 0x1B, 0x58 };
                Buffer.BlockCopy(value, 0, FPlainText, 14, value.Length);
                //  byte[] InvocationCounter = new byte[4] { 0x00, 0x00, 0x00, 0x00 };
                byte[] FinalCmd; // 
                try
                {

                    byte[] _FinalNonce = FrameNonce(securityKeys.CNonce, invocationCounter, securityKeys.CNonce.Length);
                    byte[] _AAD = FrameAAD(securityKeys.PasswordKey);
                    _CipheredData = new byte[16];

                    byte[] bResult = AESGCM.SimpleEncrypt(FPlainText, DedicatedKey, _FinalNonce, _AAD);

                    string ss = string.Concat(bResult.Select(b => b.ToString("X2") + " "));
                    this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\n FUNCTION Ciphered : " + ss + Environment.NewLine)));
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
                    //   this.RTCBox.AppendText(Environment.NewLine + "\n RAM Clear Request : " + finalCommand + Environment.NewLine);
                    this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\n FUNCTIONAL Test Request : " + finalCommand + Environment.NewLine)));
                    MUTSerialCOM.Send(FinalCmd); Thread.Sleep(500);


                    if (ReceiveStr[0] != 0)
                    {
                        // byte 12 (From 0 ) - Length
                        // byte 14,15,16,17  - FrameCount
                        // From Byte 18      - Data Started (Length - 6)
                        // Server Nonce

                        //  byte[] _SDecNonce = Encoding.UTF8.GetBytes("AVG12345");

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
                        this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "FUNCTIONAL TEST DECODED : " + sCipherResults + Environment.NewLine)));

                        bool isWriteProcessStatus = bDecryptResult[3] == 00 ? true : false;

                        if (isWriteProcessStatus == true)
                        {
                            MessageBox.Show("Success...");
                        }

                        this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "WriteStatus : " + isWriteProcessStatus.ToString() + Environment.NewLine)));


                    }
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
        private void FunctionWriteValue()
        {
            try
            {
                byte[] startValue = new byte[2] { 0x1B, 0x59 };
                FunctionalTest(startValue);
            }
            catch (Exception ex) { throw ex; }

        }
        private void FunctionStopValue()
        {
            try
            {
                byte[] stopValue = new byte[2] { 0x1B, 0x5A };
                FunctionalTest(stopValue);
            }
            catch (Exception ex) { throw ex; }
        }

        private void RunFunctionalTest()
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
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 40));

                ActionRequestCmd();
                Thread.Sleep(650);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "ACTION"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 50));

                FunctionStopValue();
                Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "STOP TEST"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 80));

                DisconnectCmd = comm.DisconnectCommand();
                string dd = String.Concat(DisconnectCmd.Select(b => b.ToString("X2") + " "));
                this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText("\n" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss ") + "  Disconnect Request : " + dd)));
                MUTSerialCOM.Send(DisconnectCmd);
                Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "DISCONNECT"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 100));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Visible = false));
                isStopFunction = false;
            }
            catch { }
        }
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

        private void LCDTest()
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
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 40));

                ActionRequestCmd();
                Thread.Sleep(650);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "ACTION"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 60));

                FunctionWriteValue();
                Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "FUNCTIONAL TEST"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 80));

                DisconnectCmd = comm.DisconnectCommand();
                string dd = String.Concat(DisconnectCmd.Select(b => b.ToString("X2") + " "));
                this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText("\n" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss ") + "  Disconnect Request : " + dd)));
                MUTSerialCOM.Send(DisconnectCmd);
                Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "DISCONNECT"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 100));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Visible = false));
                isStartWrite = false;
            }
            catch { }


        }

        private void ButtonDisable()
        {
            btnRead.Enabled = false;
            btnStart.Enabled = false;
            btn_SNoRead.Enabled = false;
        }
        private void ButtonEnable()
        {
            btnRead.Enabled = true;
            btnStart.Enabled = true;
            btn_SNoRead.Enabled = true;
        }
        //---------------------------------------- CONTROL METHODS -----------------------


        private void btn_SNoRead_Click(object sender, EventArgs e)
        {
            CmdCount = 0;
            bwReadMeter.RunWorkerAsync();
        }

        public void ResultClear()
        {
            scrollResult.Text = "NIL"; terminalResult.Text = "NIL"; magnetResult.Text = "NIL";
            topResult.Text = "NIL"; RAMResult.Text = "NIL"; flashResult.Text = "NIL";
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            ResultClear();
            isStartWrite = true;
            bwFunctionTest.RunWorkerAsync();
        }


        private void btnRead_Click(object sender, EventArgs e)
        {
            ButtonDisable();
            ReadInvocationCounter();
            bwReadTest.RunWorkerAsync();
        }

        private void btnTestOptical_Click(object sender, EventArgs e)
        {
            btnOpticalStatus.Visible = false;
            btnSuccess.Visible = false;
            try
            {
                CmdCount = 0;
                OpticalTest = 0;
                timer1.Enabled = true;
            }
            catch (Exception ex)
            {
                throw ex;
                //  UpdateLogStatus(Convert.ToString(ex));
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {

            //if (bwReadTest.IsBusy)
            //{
                ButtonEnable();
                bwFunctionTest.RunWorkerAsync();
                StopFunction();
           // }

        }

        private void StopFunction()
        {
            isStopFunction = true;
            bwReadTest.CancelAsync();
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
                invocationCounter = IterateInvocation(invocationCounter);

                byte[] _FinalNonce = FrameNonce(securityKey.CNonce, invocationCounter, securityKey.CNonce.Length);
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
                this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\nACTION REQUEST : " + finalCommand + Environment.NewLine)));
                MUTSerialCOM.Send(FinalCmd); Thread.Sleep(500);
            }

            catch (Exception ex)
            {
            }

        }
        //-------------------------------- HDLC Read Serial Number Commands ----------------------------

        private void ReadSno()
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
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 60));

                SNOLogicalName();
                //int milliseconds = 650;
                // Thread.Sleep(650);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "OBIS Code"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 80));

                ReadSerialNo();
                //  Thread.Sleep(650);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "Read SNo"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 90));

                DisconnectCmd = comm.DisconnectCommand();
                string dd = String.Concat(DisconnectCmd.Select(b => b.ToString("X2") + " "));
                //this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText("\n" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss ") + "  Disconnect Request : " + dd)));
                UpdateLogStatus("  Disconnect Request : " + dd);
                MUTSerialCOM.Send(DisconnectCmd);
                Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "DISCONNECT"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 100));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Visible = false));

                //  isSNoWrite = false;
            }
            catch (Exception ex)
            {
                throw ex;
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
                //this.RTCBox.AppendText(Environment.NewLine + "\nSerial No Logical Name : " + finalCommand + Environment.NewLine);
                this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "\nSerial No Logical Name : " + finalCommand + Environment.NewLine)));

                MUTSerialCOM.Send(FinalCmd); Thread.Sleep(500);

                if (ReceiveStr[0] != 0)
                {
                    // byte 13 (From 0 ) - Length           12
                    // byte 15,16,17,18  - FrameCount       14,15,16,17
                    // From Byte 19      - Data Started (Length - 6)   18
                    // Server Nonce

                    //  byte[] _SDecNonce = Encoding.UTF8.GetBytes("AVG12345");

                    byte[] _SDecNonce = ServerNonce; //new byte[] { 0x41, 0x56, 0x47, 0x30, 0x30, 0x30, 0x30, 0x34 }; // (Server System Title from AARE )                   
                    byte[] _SDecsubNonce_FrameCount = new byte[4]; // { 0x00, 0x00, 0x00, 0x02 };
                    Buffer.BlockCopy(ReceiveStr, 14, _SDecsubNonce_FrameCount, 0, 4);

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


            catch (Exception ex)
            {
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
                //byte[] PlainText = { 0xC0, 0x01, 0xC1, 0x00, 0x01, 0x01, 0x00, 0x60, 0x02, 0x83, 0x00 };
                Buffer.BlockCopy(serialNumber.PlainText, 0, serialNumber.SPlainText, 0, 11);
                // Buffer.BlockCopy(PlainText, 0, serialNumber.SPlainText, 0, 11);
                serialNumber.SPlainText[11] = 0x02;
                serialNumber.SPlainText[12] = 0x00;
                //Buffer.BlockCopy(serialNumber.InvocationCounter, 0, serialNumber.SInvocationCounter, 0, 3);
                //serialNumber.SInvocationCounter[3] = 0x01;


                byte[] FinalCmd; // 
                try
                {

                    byte[] _FinalNonce = FrameNonce(securityKeys.CNonce, invocationCounter, securityKeys.CNonce.Length);
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
                        byte[] _SDecsubNonce_FrameCount = new byte[4]; // { 0x00, 0x00, 0x00, 0x02 };
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
                        this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText(Environment.NewLine + "Serial No DECODED : " + sCipherResults + Environment.NewLine)));

                        // Serial Number Byte to Ascii Conversion
                        byte[] SNoHex = new byte[13];
                        Buffer.BlockCopy(bDecryptResult, 6, SNoHex, 0, SNoHex.Length);
                        string asciiString = ConvertBytesToAscii(SNoHex);

                        this.txtSerialNo.Invoke((MethodInvoker)(() => txtSerialNo.Text = asciiString));



                    }
                }
                catch (Exception ex)
                {
                    ErrorDetection(ReceiveStr);
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
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 60));

                RTCOBISCodeReadCmd();
                // Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "RTC LOGICAL NAME"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 80));

                ReadRTC();
                // Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "RTC READ"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 90));

                DisconnectCmd = comm.DisconnectCommand();
                string dd = String.Concat(DisconnectCmd.Select(b => b.ToString("X2") + " "));
                this.RTCBox.Invoke((MethodInvoker)(() => RTCBox.AppendText("\n" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss ") + "  Disconnect Request : " + dd)));
                MUTSerialCOM.Send(DisconnectCmd);
                Thread.Sleep(waitSecond);
                this.LblCmdCount.Invoke((MethodInvoker)(() => LblCmdCount.Text = "DISCONNECT"));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = 100));
                this.progressBar1.Invoke((MethodInvoker)(() => progressBar1.Visible = false));

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
                invocationCounter = IterateInvocation(invocationCounter);
                byte[] FinalCmd;
                Buffer.BlockCopy(objDataReq.PlainText, 0, objDataReq.OBISPlainText, 0, 11);
                objDataReq.OBISPlainText[11] = 0x01;
                objDataReq.OBISPlainText[12] = 0x00;
                //Buffer.BlockCopy(objDataReq.InvocationCounter, 0, invocationCounter, 0, 3);
                //objDataReq.InvocationCounterBytes[3] = 0x00;

                byte[] _FinalNonce = FrameNonce(securityKey.CNonce, invocationCounter, securityKey.CNonce.Length);
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
                invocationCounter = IterateInvocation(invocationCounter);

                byte[] bInstantData;
                byte[] FinalCmd;
                Buffer.BlockCopy(objDataReq.PlainText, 0, objDataReq.OBISPlainText, 0, 11);
                objDataReq.OBISPlainText[11] = 0x02;
                objDataReq.OBISPlainText[12] = 0x00;
                //Buffer.BlockCopy(objDataReq.InvocationCounter, 0, objDataReq.InvocationCounterBytes, 0, 3);
                //objDataReq.InvocationCounterBytes[3] = 0x01;


                //  byte[] FinalCmd; // 
                try
                {

                    byte[] _FinalNonce = FrameNonce(securityKey.CNonce, invocationCounter, securityKey.CNonce.Length);
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
                    ErrorDetection(ReceiveStr);
                }


            }
            catch { };
        }
        //------------------------------------- DO WORKER -----------------------------------
        private void bwReadMeter_DoWork(object sender, DoWorkEventArgs e)
        {
            ReadInvocationCounter();
            ReadSno();
            Thread.Sleep(300);
            ReadRealTimeClock();

        }

        private void bwFunctionTest_DoWork(object sender, DoWorkEventArgs e)
        {
            if (isStartWrite == true)
            {
                ReadInvocationCounter();
                LCDTest();
            }
            else if (isStopFunction == true)
            {
                ReadInvocationCounter();
                RunFunctionalTest();
            }
        }

        private void bwReadTest_DoWork(object sender, DoWorkEventArgs e)
        {

            while (!bwReadTest.CancellationPending)
            {
                CmdCount = 0;
                ReadFunctionalTest();
            }
            if (bwReadTest.CancellationPending)
            {
                e.Cancel = true;
            }

        }

        private void bwReadTest_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            if (e.Cancelled)
            {
                MessageBox.Show("Operation Cancelled");
                // StopFunction();
            }
            //else
            //{
            //    CmdCount = 0;
            //    bwReadTest.RunWorkerAsync();
            //}

        }

        // --------------------------------- TIMER ------------------------------------------
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (CmdCount == 0)
                {
                    byte[] SNRMCmd = comm.SNRMCommand();
                    string ss = String.Concat(SNRMCmd.Select(b => b.ToString("X2") + " "));
                    UpdateLogStatus("  SNRM Request : " + ss);
                    MUTSerialCOM.Send(SNRMCmd);
                    //Thread.Sleep(250);
                    OpticalTest++;
                    if (OpticalTest >= 3)
                    {
                        UpdateLogStatus("  Optical Test Failed... ");
                        btnOpticalStatus.Visible = true;
                        btnSuccess.Visible = false;
                        timer1.Enabled = false;
                    }


                }
                else if (CmdCount == 1)
                {
                    byte[] DisconnectCmd = comm.DisconnectCommand();
                    string dd = String.Concat(DisconnectCmd.Select(b => b.ToString("X2") + " "));
                    UpdateLogStatus("  Disconnect Request : " + dd);
                    MUTSerialCOM.Send(DisconnectCmd);
                    //Thread.Sleep(350);

                    timer1.Enabled = false;
                    btnOpticalStatus.Visible = false;
                    btnSuccess.Visible = true;

                }
                //else
                //{
                //    timer1.Enabled = false;
                //}
            }
            catch
            {

            }

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

        private void panelConfig_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
