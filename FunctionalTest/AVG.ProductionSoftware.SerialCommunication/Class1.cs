using System;
using System.IO.Ports;
//using System.IO.Ports;

namespace AVG.ProductionSoftware.SerialCommunication
{
    public delegate void dataReceivedMUT(object sender, MUTSerialPortEventArgs arg);
    public class MUTSerialPortEventArgs : EventArgs
    {
        public byte[] ReceivedDataMUT { get; private set; }
        public MUTSerialPortEventArgs(byte[] data)
        {
            ReceivedDataMUT = data;
        }
    }
    public class MUTSerialPortInterface
    {
        public event dataReceivedMUT DataReceivedMUT; public bool COMDataStatus;
        private int _DelayTime;
        private int _COMOpenReadTimeOut = 1300;//1300
        /// <summary>
        /// Serial port class
        /// </summary>
        private SerialPort ObjMeterserialport = new SerialPort();

        public bool MUTCOMOpen(string COMName, int BaudRateVal, int databitsVal, Parity ParityVal, StopBits StopBitVal, int delayTime = 1000)
        {
            try
            {
                ObjMeterserialport = new SerialPort();

                this.ObjMeterserialport.BaudRate = BaudRateVal;
                this.ObjMeterserialport.DataBits = databitsVal;
                this.ObjMeterserialport.Parity = ParityVal;
                this.ObjMeterserialport.PortName = COMName;
                this.ObjMeterserialport.StopBits = StopBitVal;
                this.ObjMeterserialport.RtsEnable = true;
                this.ObjMeterserialport.DtrEnable = true;
                this.ObjMeterserialport.ReadTimeout = _COMOpenReadTimeOut;
                this.ObjMeterserialport.WriteTimeout = 2250;//2000
                _DelayTime = delayTime;

                this.ObjMeterserialport.DataReceived += new SerialDataReceivedEventHandler(this._ObjMeterserialport_DataReceived);
                this.ObjMeterserialport.Open();
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool Send(byte[] data)
        {
            try
            {
                ObjMeterserialport.Write(data, 0, data.Length);
                System.Threading.Thread.Sleep(500);
                this.ObjMeterserialport.RtsEnable = false;
            }
            catch { return false; }
            return true;
        }
        private void _ObjMeterserialport_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //MUTSerialPortEventArgs args;
            try
            {
                //Console.WriteLine("Received");
                byte[] data = new byte[this.ObjMeterserialport.ReadBufferSize];
                System.Threading.Thread.Sleep(_COMOpenReadTimeOut); COMDataStatus = true;
                ObjMeterserialport.Read(data, 0, data.Length);

                MUTSerialPortEventArgs args = new MUTSerialPortEventArgs(data);
                this.DataReceivedMUT(this, args);
            }
            catch (Exception ex)
            {

            }
        }
        public void MUTCOMClose()
        {
            ObjMeterserialport.DataReceived -= _ObjMeterserialport_DataReceived;
            ObjMeterserialport.Close();
        }
    }
}
