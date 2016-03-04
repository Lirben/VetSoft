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

        public delegate void serialEvent(string data);
        public event serialEvent dataRecieved;

        public string LastError { get { return _lastError; } }

        protected virtual void onDataRecieved(string data)
        {
            if (dataRecieved != null)
                dataRecieved(data);
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

            _serialPort.Close();
            _serialPort.Dispose();

            return retVal;
        }

        public string readNext()
        {
            if (_serialPort.IsOpen)
                if (_serialPort.BytesToRead > 0)
                    return _serialPort.ReadLine();

            return string.Empty;
        }

        public void send(string payload)
        {
            _serialPort.WriteLine(payload);
        }
                
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            onDataRecieved(sp.ReadLine());
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
