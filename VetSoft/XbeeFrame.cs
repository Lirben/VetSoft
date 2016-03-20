using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetSoft
{
    class XbeeFrame
    {
        byte _startDelimiter;
        byte[] _length;
        byte _frameType;
        byte _frameID;
        byte[] _destAddress;
        byte _options;
        byte[] _payLoad;
        byte _checkSum;
        byte[] _rawFrame;

        public byte FrameType { get { return _frameType; } }
        public byte[] RawFrame { get { return _rawFrame; } }
        public byte[] PayLoad { get { return _payLoad; } }
        public string FrameContent { get { return System.Text.Encoding.UTF8.GetString(_payLoad); } }

        public XbeeFrame(int address, string payLoad)
        {
            _startDelimiter = 0x7E;
            _frameType = 0x00;
            _frameID = 0x01;
            _options = 0x00;
            
            _destAddress = getAddress(address);
            _payLoad = getPayLoad(payLoad);
            _length = calculateLength();

            _checkSum = calculateCheckSum();

            _rawFrame = calculatePacket();
        }

        public XbeeFrame(byte[] rawFrame)
        {
            List<byte> workFrame = new List<byte>();
            _rawFrame = rawFrame;

            for (int i = 0; i < rawFrame.Length; i++)
                workFrame.Add(rawFrame[i]);
                            
            _startDelimiter = workFrame[0];

            workFrame.RemoveAt(0);

            //Calculate length of packet content
            int length = calculateLength(workFrame, 0);

            _frameType = workFrame[0];
            workFrame.RemoveAt(0);
            length--;

            if (_frameType.Equals(Types.XBEE_TRANSMIT_STATUS))
            {
                _frameID = workFrame[0];
                workFrame.RemoveAt(0);
                length--;
            }

            if(_frameType.Equals(Types.XBEE_RECIEVE_PACKET_16BIT))
            {
                workFrame.RemoveRange(0, 4);
                length -= 4;
            }

            _payLoad = getPayLoad(workFrame);           

            _checkSum = workFrame[0];            
        }

        private int calculateLength(List<byte> rawFrame, int startLenID)
        {
            int retVal = 0;
            int maxLoop = 2;
            int lengthId = 0;
            byte[] length = new byte[2];

            for (int i = startLenID; i < maxLoop; i++)
            {
                if (!rawFrame[i].Equals(0x7D))
                    length[lengthId] = rawFrame[i];
                else
                {
                    length[lengthId] = (byte)(rawFrame[++i] ^ 0x20);
                    maxLoop++;
                }

                lengthId++;
            }

            rawFrame.RemoveRange(0, maxLoop);

            retVal = Convert.ToInt32(length[0]) + Convert.ToInt32(length[1]);

            return retVal;
        }


        private byte[] getAddress(int address)
        {
            byte[] retVal = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, (byte) address};
            return retVal;
        }

        private byte[] getPayLoad(List<byte> workFrame)
        {
            List<byte> retVal = new List<byte>();

            for(int i = 0; i < (workFrame.Count - 1); i++)
            {
                if (!workFrame[i].Equals(0x7D))
                    retVal.Add(workFrame[i]);
                else                
                    retVal.Add((byte)(workFrame[++i] ^ 0x20));
            }

            workFrame.RemoveRange(0, workFrame.Count - 1);

            return retVal.ToArray();

        }

        private byte[] getPayLoad(string payLoad)
        {
            return Encoding.ASCII.GetBytes(payLoad);
        }

        private byte[] calculateLength()
        {
            int length = 0;

            length += 1;                    //FrameType
            length += 1;                    //FrameID
            length += _destAddress.Length;  //Address field
            length += 1;                    //Options field
            length += _payLoad.Length;

            byte[] retVal = new byte[2];
            
            retVal[0] = BitConverter.GetBytes(length)[1];
            retVal[1] = BitConverter.GetBytes(length)[0];

            return retVal;
        }

        private byte calculateCheckSum()
        {
            int decSum = 0;

            for (int i = 0; i < _destAddress.Length; i++)
                decSum += Convert.ToInt32(_destAddress[i]);

            decSum += Convert.ToInt32(_options);
            decSum += Convert.ToInt32(_frameID);

            for (int i = 0; i < _payLoad.Length; i++)
                decSum += Convert.ToInt32(_payLoad[i]);
                
            decSum = 255 - decSum;

            byte[] checkSum = BitConverter.GetBytes(decSum);

            return checkSum[0];
        }

        private byte[] calculatePacket()
        {
            byte[] retVal;
            List<byte> packetArray = new List<byte>();
            packetArray.Add(_startDelimiter);

            _length = escapeSequence(_length);

            for (int i = 0; i < _length.Length; i++)
                packetArray.Add(_length[i]);

            packetArray.Add(_frameType);
            packetArray.Add(_frameID);

            _destAddress = escapeSequence(_destAddress);

            for (int i = 0; i < _destAddress.Length; i++)
                packetArray.Add(_destAddress[i]);

            packetArray.Add(_options);

            _payLoad = escapeSequence(_payLoad);

            for (int i = 0; i < _payLoad.Length; i++)
                packetArray.Add(_payLoad[i]);

            packetArray.Add(_checkSum);

            retVal = new byte[packetArray.Count];

            for (int i = 0; i < packetArray.Count; i++)
                retVal[i] = packetArray[i];

            return retVal;
        }

        private byte[] escapeSequence(byte[] sequence)
        {
            byte escape = 0x7D;
            List<byte> retValList = new List<byte>();

            for(int i = 0; i < sequence.Length; i++)
                switch(sequence[i])
                {
                    case 0x7E:
                        retValList.Add(escape);
                        retValList.Add(0x7E ^ 0x20);
                        break;

                    case 0x7D:
                        retValList.Add(escape);
                        retValList.Add(0x7D ^ 0x20);
                        break;

                    case 0x11:
                        retValList.Add(escape);
                        retValList.Add(0x11 ^ 0x20);
                        break;

                    case 0x13:
                        retValList.Add(escape);
                        retValList.Add(0x13 ^ 0x20);
                        break;

                    default:
                        retValList.Add(sequence[i]);
                        break;
                }
            
            byte[] retVal = retValList.ToArray();
            return retVal;
        }
    }
}
