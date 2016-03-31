using System;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;

namespace VetSoft
{
    /// <summary>
    /// Deals with a XBee module connected on a serial port
    /// Xbee frames can be recieved and send to through the serial port
    /// </summary>
    class Serial
    {
        private SerialPort _serialPort;
        private string _lastError;

        public delegate void serialEvent(Frame frame);
        public event serialEvent dataRecieved;

        public string LastError { get { return _lastError; } }

        /// <summary>
        /// Event triggered when a new frame is detected on the serial port
        /// </summary>
        /// <param name="frame">The frame that is available on the serial port</param>
        protected virtual void onDataRecieved(Frame frame)
        {
            if (dataRecieved != null)
                dataRecieved(frame);
        }

        /// <summary>
        /// Create a serial port object
        /// </summary>
        public Serial()
        {
            _serialPort = new SerialPort();
            _lastError = string.Empty;
        }

        public void Flush()
        {
            _serialPort.DiscardInBuffer();
        }

        /// <summary>
        /// Connect to a serial port
        /// </summary>
        /// <param name="comPort">The port to which the connection must be made</param>
        /// <param name="baudRate">The baudrate used to connect with the port</param>
        /// <returns>True if the connection is succesfull</returns>
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

        /// <summary>
        /// Disconnect from the serial port
        /// </summary>
        /// <returns>True if the port is released succesfully </returns>
        public bool disconnect()
        {
            bool retVal = true;

            _serialPort.DiscardInBuffer();
            _serialPort.DiscardOutBuffer();
            _serialPort.Close();
            _serialPort.Dispose();

            return retVal;
        }

        /// <summary>
        /// Send a frame to the serial port
        /// </summary>
        /// <param name="frame">The frame that must be transmitted</param>
        public void send(Frame frame)
        {
            byte[] byFrame = frame.FrameContent.ToArray();
            _serialPort.Write(byFrame, 0, byFrame.Length);
        }

        /***************************** PRIVATE ZONE *****************************/

        /// <summary>
        /// Read the next meaningfull byte from a serial port. Escaped characters are omitted
        /// </summary>
        /// <param name="port">The port to read the next byte from</param>
        /// <returns>The next meaningfull byte</returns>
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
            
        /// <summary>
        /// Event handler that indicates that new data has appeared on the serial port
        /// </summary>
        /// <param name="sender">The serial port that detected new data</param>
        /// <param name="e">Serial parameters</param>
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            
            while (sp.BytesToRead > 0)
            {
                byte delimiter = ReadNextByte(sp);               

                if (delimiter.Equals(0x7E))
                {
                    int length;
                    Frame xbFrame;
                    byte frameType;
                    byte[] frameLength = new byte[2];

                    frameLength[0] = ReadNextByte(sp);
                    frameLength[1] = ReadNextByte(sp);
                    length = Convert.ToInt32(frameLength[0]) + Convert.ToInt32(frameLength[1]);

                    frameType = ReadNextByte(sp);
                    xbFrame = new Frame(frameType, length);

                    while (sp.BytesToRead < length) ;       //Wait till complete packet is read [Escaped characters are not counted].

                    for (int i = 0; i < length; i++)
                        xbFrame.AddByte(ReadNextByte(sp));

                    dataRecieved(xbFrame);
                }
            }
        }

        /// <summary>
        /// Close the serial connection with the serial port
        /// </summary>
        private void closeConnection()
        {
            _serialPort.Close();
            _serialPort.Dispose();
        }

        /// <summary>
        /// Destructor
        /// </summary>
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
