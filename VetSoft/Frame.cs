using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetSoft
{
    class Frame
    {
        private bool _escaped;

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

        public Frame()
        {
            _frameContent = new List<byte>();
            _contentLength = 0;
            _escaped = false;
        }

        public bool addByte(byte newByte)
        {
            _frameContent.Add(newByte);

            if (_frameContent.Count > 2 && _contentLength.Equals(0))
                _contentLength = Convert.ToInt32(_frameContent[1]) + Convert.ToInt32(_frameContent[2]);

            return true;
        }
    }
}
