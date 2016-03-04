using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetSoft
{
    class ForcePoint
    {
        private uint _timeStamp;
        private double _forceValue;

        public uint TimeStamp { get { return _timeStamp; } }
        public double ForceValue { get { return _forceValue; } }

        public ForcePoint(uint timestamp, double forceValue)
        {
            _timeStamp = timestamp;
            _forceValue = forceValue;
        }
    }
}
