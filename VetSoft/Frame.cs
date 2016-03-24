using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetSoft
{
    /// <summary>
    /// Basic Xbee frame
    /// </summary>
    class Frame
    {
        private Frame _frame;
        private List<byte> _frameContent;
                
        public byte FrameType { get { return _frameContent[3]; } }
        public List<byte> FrameContent { get { return _frameContent; } }

        /// <summary>
        /// Create a generic frame to transmit over XBee
        /// </summary>
        /// <param name="frame">Copyconstructor</param>
        public Frame(Frame frame)
        {
            _frameContent = frame._frameContent;
        }

        /// <summary>
        /// Create a frame from a byte stream
        /// </summary>
        /// <param name="frameContent">The bytestream</param>
        public Frame(byte[] frameContent)
        {
            _frameContent = new List<byte>();
            _frameContent.AddRange(frameContent);
        }

        /// <summary>
        /// Create a generic frame to transmit over XBee
        /// </summary>
        /// <param name="frameType">The type of the frame</param>
        /// <param name="packetLength">The length of the frame</param>
        public Frame(byte frameType, int packetLength)
        {
            _frameContent = new List<byte>();
            _frameContent.Add(0x7E);
            _frameContent.AddRange(BitConverter.GetBytes(Convert.ToInt16(packetLength)));
            _frameContent.Add(frameType);
        }

        /// <summary>
        /// Add a byte to the framecontent
        /// </summary>
        /// <param name="newByte">The byte to add to the framecontent</param>
        public void AddByte(byte newByte)
        {
            _frameContent.Add(newByte);
        }

        /// <summary>
        /// Factory method
        /// Retrieve the most specific type of frame destilled from the generic type Frame
        /// </summary>
        /// <returns>A more specific type of frame</returns>
        public Frame GetFrame()
        {
            switch (this.FrameType)
            {
                case Types.RxFRAME_16BIT:
                    _frame = new RxFrame(this);
                    break;

                case Types.TxFRAME_16BIT:
                    _frame = new TxFrame(this);
                    break;
            }

            return _frame;
        }
    }

    class RxFrame : Frame
    {
        public string Message { get { return Encoding.UTF8.GetString(base.FrameContent.GetRange(8, base.FrameContent.Count - 10).ToArray()); } }
        public byte[] Address { get { return base.FrameContent.GetRange(4, 2).ToArray(); } }        
        public byte CheckSum { get { return base.FrameContent[base.FrameContent.Count - 1]; } }

        /// <summary>
        /// Create a 16bit recieve frame
        /// </summary>
        /// <param name="baseFrame">The generic frame object needed to create the RxFrame</param>
        public RxFrame(Frame baseFrame) : base (baseFrame) { }
        
        /// <summary>
        /// Create a 16bit recieve frame
        /// </summary>
        /// <param name="address">The address of the XBee from where the frame is coming</param>
        /// <param name="message">The message recieved from the source</param>
        /// <param name="options">Frame options</param>
        public RxFrame(byte[] frameContent) : base(frameContent) { }
    }

    /// <summary>
    /// 16 bit frame used to transmit data from one Xbee to another
    /// </summary>
    class TxFrame : Frame
    {
        public string Message { get { return Encoding.UTF8.GetString(base.FrameContent.GetRange(8, base.FrameContent.Count - 10).ToArray()); } }
        public byte[] Address { get { return base.FrameContent.GetRange(5, 2).ToArray(); } }        
        public byte CheckSum { get { return base.FrameContent[base.FrameContent.Count - 1]; } }

        /// <summary>
        /// Create a 16bit transmit frame
        /// </summary>
        /// <param name="baseFrame">The generic frame object needed to create the TxFrame</param>
        public TxFrame(Frame baseFrame) : base (baseFrame) { }
        
        /// <summary>
        /// Create a 16bit transmit frame
        /// </summary>
        /// <param name="address">The address of the XBee to which the frame must be send</param>
        /// <param name="message">The message to send to the Xbee</param>
        /// <param name="options">Frame options</param>
        public TxFrame(int address, string message, byte options) : base(Types.TxFRAME_16BIT, 0)
        {
            //Add FrameID
            base.FrameContent.Add(0x01);
            
            //Add address
            base.FrameContent.AddRange(getAddress(address));

            //Add options
            base.FrameContent.Add(options);

            //Add message
            base.FrameContent.AddRange(getPayLoad(message));

            //Add checksum
            base.FrameContent.Add(getCheckSum());

            //Update length
            byte[] frameLength = getLength(base.FrameContent.Count - 4);
            base.FrameContent[1] = frameLength[1];
            base.FrameContent[2] = frameLength[0];

            //Escape sequence
            insertEscapes();
        }

        /***************************** PRIVATE ZONE *****************************/

        /// <summary>
        /// Convert the address from integer to byte
        /// </summary>
        /// <param name="address">The address associated to the frame</param>
        /// <returns>The address associated as a byte array</returns>
        private byte[] getAddress(int address)
        {
            byte[] retVal = { 0x00, (byte)address };
            return retVal;
        }

        /// <summary>
        /// Convert the length of the frame from int to byte array
        /// </summary>
        /// <param name="length">The length of the frame</param>
        /// <returns>A byte array with the lenght of the frame</returns>
        private byte[] getLength(int length)
        {
            return BitConverter.GetBytes(length);
        }

        /// <summary>
        /// Convert the payload of the frame to a byte array
        /// </summary>
        /// <param name="payLoad">The payload of the frame as string</param>
        /// <returns>The payload of the frame as byte array</returns>
        private byte[] getPayLoad(string payLoad)
        {
            return Encoding.ASCII.GetBytes(payLoad);
        }

        /// <summary>
        /// Calculate the checksum of the frame
        /// </summary>
        /// <returns></returns>
        private byte getCheckSum()
        {
            int decSum = 0;

            foreach (byte frameByte in base.FrameContent.GetRange(3, base.FrameContent.Count - 3))
                decSum += Convert.ToInt32(frameByte);

            decSum = 255 - decSum;

            return BitConverter.GetBytes(decSum)[0];
        }

        /// <summary>
        /// Escape the bytes of the frame when necessary
        /// </summary>
        private void insertEscapes()
        {
            for(int i = 1; i < base.FrameContent.Count; i++)
                switch(base.FrameContent[i])
                {
                    case 0x7E:
                        EscapeByte(0x7E, i);
                        break;

                    case 0x7D:
                        EscapeByte(0x7D, i);
                        break;

                    case 0x11:
                        EscapeByte(0x11, i);
                        break;

                    case 0x13:
                        EscapeByte(0x13, i);
                        break;
                }
        }

        /// <summary>
        /// Escape an explicit byte in the framecontent
        /// </summary>
        /// <param name="targetByte">The byte to be escaped</param>
        /// <param name="position">The location of the byte in the bytestream</param>
        private void EscapeByte(byte targetByte, int position)
        {
            base.FrameContent.RemoveAt(position);
            base.FrameContent.Insert(position, (byte) (targetByte ^ 0x20));
            base.FrameContent.Insert(position, 0x7D);
        }
    }

    /*class OldFrame
    {
        private byte _delimiter;
        private byte _frameType;
        private byte[] _frameLength;
        private List<byte> _frameContent;
        private byte _checksum;

        private int _contentLength;

        public int CurrentLength { get { return _frameContent.Count; } }
        public int ContentLength { get { return _contentLength; } }

        public byte FrameType { get { return _frameContent[3]; } }

        public byte[] PayLoad { get { return _frameContent.GetRange(4, _frameContent.Count - 5).ToArray(); } }
        public string Message { get { return Encoding.UTF8.GetString(_frameContent.GetRange(8, _frameContent.Count - 9).ToArray()); } }
        public byte CheckSum { get { return _checksum; } }

        public OldFrame(byte frameType, int packetLength)
        {
            _frameContent = new List<byte>();
            _contentLength = 0;
        }

        public bool addByte(byte newByte)
        {
            _frameContent.Add(newByte);

            if (_frameContent.Count > 2 && _contentLength.Equals(0))
                _contentLength = Convert.ToInt32(_frameContent[1]) + Convert.ToInt32(_frameContent[2]);

            return true;
        }
    }*/
}
