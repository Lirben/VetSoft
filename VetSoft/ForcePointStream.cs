using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetSoft
{
    class ForcePointStream
    {
        private ForcePointAnalyser _fpAnalyser;
        private Types.StreamType _streamType;
        private Types.HoofLocation _hoofLocation;
        private Types.SensorLocation _sensorLocation;
        private List<ForcePoint> _rawStream;

        public int Steps { get { return CalculateSteps(); } }
        public Types.StreamType StreamType { get { return _streamType; } }
        public Types.HoofLocation HoofLocation { get { return _hoofLocation; } }
        public Types.SensorLocation SensorLocation { get { return _sensorLocation; } }
        public List<ForcePoint> RawStream { get { return _rawStream; } }
        public List<ForcePoint> StepStream { get { return CalculateStepStream(); } }


        /// <summary>
        /// Create a ForcePoint stream
        /// </summary>
        /// <param name="hoofLocation">The hoof whereto the stream belongs</param>
        /// <param name="sensorLocation">The sensor on the hoof whereto the stream belongs</param>
        public ForcePointStream(Types.HoofLocation hoofLocation, Types.SensorLocation sensorLocation, Types.StreamType streamType = Types.StreamType.RAW)
        {
            _streamType = streamType;
            _hoofLocation = hoofLocation;
            _sensorLocation = sensorLocation;

            _rawStream = new List<ForcePoint>();
        }
        

        /// <summary>
        /// Add a ForcePoint to the raw stream
        /// </summary>
        /// <param name="forcePoint">The ForcePoint that will be added to the stream</param>
        public void addForcePoint(ForcePoint forcePoint)
        {
            _rawStream.Add(forcePoint);
        }

        private List<ForcePoint> CalculateStepStream()
        {
            List<ForcePoint> retVal = new List<ForcePoint>();

            _fpAnalyser = new ForcePointAnalyser();
            retVal = _fpAnalyser.StepSequence(this).RawStream;

            return retVal;
        }

        /// <summary>
        /// Calculate the steps from a filtered input stream of forcepoints
        /// </summary>
        /// <returns>The amount of steps</returns>
        private int CalculateSteps()
        {
            int retVal = 0;

            List<ForcePoint> stepStream = CalculateStepStream();

            ForcePoint previousPoint = stepStream[0];

            foreach (ForcePoint forcePoint in stepStream)
            {
                if (forcePoint.ForceValue > previousPoint.ForceValue)
                    retVal++;

                previousPoint = forcePoint;
            }

            return retVal;
        }       
    }
}
