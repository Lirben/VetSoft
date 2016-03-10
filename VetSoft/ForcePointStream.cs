using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetSoft
{
    class ForcePointStream
    {
        private int _steps;
        private ForcePointAnalyser _fpAnalyser;
        private Types.StreamType _streamType;
        private Types.HoofLocation _hoofLocation;
        private Types.SensorLocation _sensorLocation;
        private List<ForcePoint> _rawStream;
        private List<ForcePoint> _stepStream;


        public int Steps { get { return ReturnSteps(); } }
        public Types.StreamType StreamType { get { return _streamType; } }
        public Types.HoofLocation HoofLocation { get { return _hoofLocation; } }
        public Types.SensorLocation SensorLocation { get { return _sensorLocation; } }
        public List<ForcePoint> RawStream { get { return _rawStream; } }
        public List<ForcePoint> StepStream { get { return ReturnStepStream(); } }


        /// <summary>
        /// Create a ForcePoint stream
        /// </summary>
        /// <param name="hoofLocation">The hoof whereto the stream belongs</param>
        /// <param name="sensorLocation">The sensor on the hoof whereto the stream belongs</param>
        public ForcePointStream(Types.HoofLocation hoofLocation, Types.SensorLocation sensorLocation, Types.StreamType streamType = Types.StreamType.RAW)
        {
            _steps = -1;
            _streamType = streamType;
            _hoofLocation = hoofLocation;
            _sensorLocation = sensorLocation;
            _rawStream = new List<ForcePoint>();  
            _stepStream = new List<ForcePoint>();
        }
        

        /// <summary>
        /// Add a ForcePoint to the raw stream
        /// </summary>
        /// <param name="forcePoint">The ForcePoint that will be added to the stream</param>
        public void addForcePoint(ForcePoint forcePoint)
        {
            _rawStream.Add(forcePoint);
        }

        private int ReturnSteps()
        {
            if (_steps.Equals(-1))
                CalculateSteps();

            return _steps;
        }

        private List<ForcePoint> ReturnStepStream()
        {
            if (_stepStream.Count.Equals(0))
                CalculateStepStream();

            return _stepStream;
        }

        private void CalculateStepStream()
        {
            _fpAnalyser = new ForcePointAnalyser();
            _stepStream = _fpAnalyser.StepSequence(this).RawStream;
        }

        /// <summary>
        /// Calculate the steps from a filtered input stream of forcepoints
        /// </summary>
        /// <returns>The amount of steps</returns>
        private void CalculateSteps()
        {
            _steps = 0;
            ForcePoint previousPoint;

            if(_stepStream.Count.Equals(0))
                CalculateStepStream();

            previousPoint = _stepStream[0];

            foreach (ForcePoint forcePoint in _stepStream)
            {
                if (forcePoint.ForceValue > previousPoint.ForceValue)
                    _steps++;

                previousPoint = forcePoint;
            }
        }       
    }
}
