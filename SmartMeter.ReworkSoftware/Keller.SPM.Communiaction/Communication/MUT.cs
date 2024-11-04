using System;
using System.IO.Ports;

namespace Keller.SPM.Communication
{
    public class MUT
    {

        public delegate void dataReceived(object sender, SerialPortEventArgs args);

        public class SerialPortEventArgs : EventArgs
        {
            public byte[] ReceivedData { get; private set; }
            public SerialPortEventArgs(byte[] data)
            {
                ReceivedData = data;
            }
        }
        public class SerialCOM
        {
            SerialPort ObjSerialPort = new SerialPort();
            Communication.MUT objCOM = new Communication.MUT();
            public event dataReceived DataReceived;
            public bool OpenCOM(string portName, int baudrate, int databits, Parity parity, StopBits stopbits)
            {
                try
                {
                    StatusClass statusClass = new StatusClass();
                    int SC = statusClass.CmdSendFlag;
                    statusClass.CmdSendFlag = 5;
                    SC = statusClass.CmdSendFlag;

                    ObjSerialPort.PortName = portName;
                    ObjSerialPort.BaudRate = baudrate;
                    ObjSerialPort.DataBits = databits;
                    ObjSerialPort.Parity = parity;
                    ObjSerialPort.StopBits = stopbits;
                    ObjSerialPort.RtsEnable = true; ObjSerialPort.DtrEnable = true;
                    ObjSerialPort.ReadTimeout = 500; ObjSerialPort.WriteTimeout = 500;
                    ObjSerialPort.DataReceived += new SerialDataReceivedEventHandler(SP_DataReceived);
                    StartCountdown();// StartCountdowmRcvChk();
                    ObjSerialPort.Open();
                }
                catch (Exception ex)
                {
                    return false;
                }
                return true;

            }

            public bool Send(byte[] data)
            {
                try
                {
                    OriginalData = new byte[4096];
                    ObjSerialPort.Write(data, 0, data.Length);
                }
                catch { return false; }
                return true;
            }

            void StartCountdown()
            {
                var timer = new System.Timers.Timer(5); // Tick every second, var timer
                timer.Elapsed += UpdateCountdown;
                timer.Enabled = true;
            }
            void StartCountdowmRcvChk()
            {
                var timer = new System.Timers.Timer(50); // Tick every second, var timer
                timer.Elapsed += UpdateCountdownRcvChk;
                timer.Enabled = true;
            }
            public void UpdateCountdownRcvChk(Object source, System.Timers.ElapsedEventArgs e)
            {
                StatusClass statusClass = new StatusClass();

                try
                {
                    if (statusClass.SerialDataRcvrCmpltFlag == 1)
                    {
                        statusClass.SerialDataRcvrCmpltFlag = 0;
                        SerialPortEventArgs args = new SerialPortEventArgs(OriginalData);
                        this.DataReceived(this, args);
                    }
                    if (statusClass.CmdTimeOutFlag == 1)
                    {

                    }
                }
                catch (Exception ex)
                {

                }
            }

            public void UpdateCountdown(Object source, System.Timers.ElapsedEventArgs e)
            {
                try
                {
                    StatusClass statusClass = new StatusClass();

                    if (statusClass.SerialTimerOnFlag == 1)
                    {
                        if (statusClass.SerialTimeOutCount > 0)
                        {
                            statusClass.CmdSendFlag = 0;
                            statusClass.SerialTimeOutCount--;
                        }
                        else
                        {
                            statusClass.SerialDataRcvrCmpltFlag = 1;
                            PreviousLength = 0;
                            statusClass.SerialTimerOnFlag = 0;
                            SerialPortEventArgs args = new SerialPortEventArgs(OriginalData);
                            this.DataReceived(this, args);
                        }
                    }
                    if (statusClass.CmdSendFlag == 1)
                    {
                        if (statusClass.CmdTimeOutCount > 0)
                        {
                            statusClass.CmdTimeOutCount--;
                        }
                        else
                        {
                            statusClass.CmdTimeOutFlag = 1;
                            statusClass.CmdSendFlag = 0;
                            PreviousLength = 0;
                        }
                    }

                }
                catch (Exception ex)
                {

                }
            }

            int RcvDataLength = 0; byte[] OriginalData = new byte[4096]; int PreviousLength = 0;

            private void SP_DataReceived(object sender, SerialDataReceivedEventArgs e)
            {
                try
                {
                    StatusClass statusClass = new StatusClass();
                    statusClass.SerialTimerOnFlag = 1;
                    statusClass.SerialTimeOutCount = 4;
                    byte[] buffer = new byte[ObjSerialPort.BytesToRead];
                    RcvDataLength = RcvDataLength + buffer.Length;
                    ObjSerialPort.Read(OriginalData, PreviousLength, buffer.Length);
                    PreviousLength = buffer.Length + PreviousLength;

                }
                catch (Exception ex) { }
            }
            public void Close()
            {
                try
                {
                    ObjSerialPort.DataReceived -= SP_DataReceived;
                    ObjSerialPort.Close();
                    ObjSerialPort.Dispose();
                }
                catch { }
            }
            public class StatusClass
            {
                static int STOF, STOC, CSF, SDRCF, CTOC, CTOF, RDLF;
                public int SerialTimerOnFlag { get { return STOF; } set { STOF = value; } }
                public int SerialTimeOutCount { get { return STOC; } set { STOC = value; } }
                public int CmdSendFlag { get { return CSF; } set { CSF = value; } }
                public int SerialDataRcvrCmpltFlag { get { return SDRCF; } set { SDRCF = value; } }
                public int CmdTimeOutCount { get { return CTOC; } set { CTOC = value; } }

                public int CmdTimeOutFlag { get { return CTOF; } set { CTOF = value; } }

                public int RcvDataLengthFlag { get { return RDLF; } set { RDLF = value; } }

            }
            public class StatusKeyClass1
            {
                static string HLSV, GLK, STL,ZGN, NHLSV, NGLK, MSK;
                public string HLS { get { return HLSV; } set { HLSV = value; } }
                public string GlobalKey { get { return GLK; } set { GLK = value; } }
                public string SystemTitle { get { return STL; } set { STL = value; } }
                public string ZigNumber { get { return ZGN; } set { ZGN = value; } }
                public string NewHLS { get { return NHLSV; } set { NHLSV = value; } }
                public string NewGlobalkey { get { return NGLK; } set { NGLK = value; } }
                public string MasterKey { get { return MSK; } set { MSK = value; } }
            }
            public class PCBSettingsParam
            {
                static string IMPR, MOV, ECAP, Magnet, EPROM, Flash, Battery, NICMod, SupCap, PCB, Soldering, LCD, Relay, SMPS;
                public string ImpulseRegistor { get { return IMPR; }set { IMPR = value; } }
                public string MOVParam { get { return MOV; } set { MOV = value; } }
                public string Ecap { get { return ECAP; } set { ECAP = value; } }
                public string MagnetParam { get { return Magnet; } set { Magnet = value; } }
                public string EPROMParam { get { return EPROM; } set { EPROM = value; } }
                public string FlashParam { get { return Flash; } set { Flash = value; } }
                public string BatteryParam { get { return Battery; } set { Battery = value; } }
                public string NICModule { get { return NICMod; } set { NICMod = value; } }
                public string SuperCapacitor { get { return SupCap; } set { SupCap = value; } }
                public string PCBParam { get { return PCB; } set { PCB = value; } }
                public string SolderingParam { get { return Soldering; } set { Soldering = value; } }
                public string LCDParam { get { return LCD; } set { LCD = value; } }
                public string RelayParam { get { return Relay; } set { Relay = value; } }
                public string SMPSTransform { get { return SMPS; } set { SMPS = value; } }
            }
        }

    }
}
