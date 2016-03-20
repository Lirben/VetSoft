using System;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;

namespace VetSoft
{
    class Serial
    {
        private SerialPort _serialPort;
        private string _lastError;

        public delegate void serialEvent(Frame frame);
        public event serialEvent dataRecieved;

        public string LastError { get { return _lastError; } }

        protected virtual void onDataRecieved(Frame frame)
        {
            if (dataRecieved != null)
                dataRecieved(frame);
        }

        public Serial()
        {
            _serialPort = new SerialPort();
            _lastError = string.Empty;
        }

        public bool connect(string comPort, int baudRate)
        {
            bool retVal = true;

            try
            {
                _serialPort.PortName = comPort;
                _serialPort.BaudRate = baudRate;

                _serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

                if (!_serialPort.IsOpen)
                    _serialPort.Open();
            }
            catch (IOException ex)
            {
                closeConnection();
                _lastError = ex.Message;                
                retVal = false;
            }
            catch(UnauthorizedAccessException ex)
            {
                closeConnection();
                _lastError = ex.Message;
                               
                retVal = false;
            }

            return retVal;
        }

        public bool disconnect()
        {
            bool retVal = true;

            _serialPort.DiscardInBuffer();
            _serialPort.DiscardOutBuffer();
            _serialPort.Close();
            _serialPort.Dispose();

            return retVal;
        }

        public void send(XbeeFrame frame)
        {
            byte[] byFrame = frame.RawFrame;
            _serialPort.Write(byFrame,0,byFrame.Length);
        }

        private byte ReadNextByte(SerialPort port)
        {
            byte[] incByte = new byte[1];

            port.Read(incByte, 0, 1);

            switch(incByte[0])
            {
                case 0x7D:                  //Escape symbol (get next symbol and Xor with 0x20)
                    port.Read(incByte, 0, 1);
                    return (byte) (incByte[0] ^ 0x20);

                default:
                    return incByte[0];
            }
        }
                
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            
            while (sp.BytesToRead > 0)
            {
                byte delimiter;
                byte[] frameLength = new byte[2];
                Frame xbFrame = new Frame();

                delimiter = ReadNextByte(sp);

                if (delimiter.Equals(0x7E))
                {
                    frameLength[0] = ReadNextByte(sp);
                    frameLength[1] = ReadNextByte(sp);

                    xbFrame.addByte(delimiter);
                    xbFrame.addByte(frameLength[0]);
                    xbFrame.addByte(frameLength[1]);

                    int length = Convert.ToInt32(frameLength[0]) + Convert.ToInt32(frameLength[1]);

                    while (sp.BytesToRead < (length + 1)) ;       //Wait till complete packet is read.

                    for (int i = 0; i < (length + 1); i++)
                        xbFrame.addByte(ReadNextByte(sp));

                    dataRecieved(xbFrame);
                }
            }
        }

        private void closeConnection()
        {
            _serialPort.Close();
            _serialPort.Dispose();
        }

        ~Serial()
        {
            try
            {
                if (_serialPort.IsOpen)
                    closeConnection();
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
