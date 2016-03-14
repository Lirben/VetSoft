using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetSoft
{
    class Step
    {
        private int _stepNumber;
        private uint _stepStart;
        private uint _stepStop;
        private Types.HoofLocation _hoofLocation;
        private Types.SensorLocation _sensorLocation;
        private List<ForcePoint> _rawStream;

        public int StepNumber { get { return _stepNumber; } set { _stepNumber = value; } }
        public uint StartTime { get { return _stepStart; } }
        public uint StopTime { get { return _stepStop; } }

        public Types.HoofLocation HoofLocation { get { return _hoofLocation; } }
        public Types.SensorLocation SensorLocation { get { return _sensorLocation; } }

        public Step(List<ForcePoint> rawStream, Types.HoofLocation hoofLocation, Types.SensorLocation sensorLocation, int stepNumber)
        {
            _stepNumber = stepNumber;
            _stepStart = rawStream[0].TimeStamp;
            _stepStop = rawStream[rawStream.Count - 1].TimeStamp;
            _hoofLocation = hoofLocation;
            _sensorLocation = sensorLocation;
            _rawStream = new List<ForcePoint>();
        }

        public Step(uint stepStart, uint stepStop, Types.HoofLocation hoofLocation, int stepNumber)
        {
            _stepNumber = stepNumber;
            _stepStart = stepStart;
            _stepStop = stepStop;
            _hoofLocation = hoofLocation;
            _rawStream = new List<ForcePoint>();
        }
    }
}
