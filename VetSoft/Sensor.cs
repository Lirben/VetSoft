using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

///TODO:
///

namespace VetSoft
{
    /// <summary>
    /// Represents a force resistive sensor.
    /// Holds a stream of force measurements and the timestamp when it was acquired
    /// </summary>
    class Sensor
    {
        private Filters _signalFilter;
        private Types.HoofLocation _hoofLocation;
        private Types.SensorLocation _sensorLocation;
        private List<ForcePoint> _rawStream;
        private List<ForcePoint> _stepStream;
        private List<Step> _stepList;

        /// <summary>Get a list of the steps that are detected on this sensor.</summary>
        public List<Step> StepList { get { return _stepList; } }

        ///<summary>The hoof to which this sensor is attached </summary>
        public Types.HoofLocation HoofLocation { get { return _hoofLocation; } }

        /// <summary>The location on a hoof where this sensor is placed </summary>
        public Types.SensorLocation SensorLocation { get { return _sensorLocation; } }

        /// <summary>The stream of acquired forcepoints by this sensor</summary>
        public List<ForcePoint> RawStream { get { return _rawStream; } }

        /// <summary>A representation of the steplist by digital forcePoints</summary>
        public List<ForcePoint> StepStream { get { return _stepStream; } }


        /// <summary>
        /// Create a ForcePoint stream
        /// </summary>
        /// <param name="hoofLocation">The hoof whereto the stream belongs</param>
        /// <param name="sensorLocation">The sensor on the hoof whereto the stream belongs</param>
        public Sensor(Types.HoofLocation hoofLocation, Types.SensorLocation sensorLocation)
        {            
            _hoofLocation = hoofLocation;
            _sensorLocation = sensorLocation;
            _stepList = new List<Step>();
            _rawStream = new List<ForcePoint>();  
            _stepStream = new List<ForcePoint>();
        }
        

        /// <summary>
        /// Add a ForcePoint to the raw sensor stream
        /// </summary>
        /// <param name="forcePoint">The ForcePoint that will be added to the stream</param>
        public void AddForcePoint(ForcePoint forcePoint)
        {
            _rawStream.Add(forcePoint);
        }

        /// <summary>
        /// Searches the raw stream for steps
        /// </summary>
        public void Analyse()
        {
            CalculateStepStream();
            CalculateSteps();
        }

        /// <summary>
        /// Clear this sensor from the acquisition history
        /// </summary>
        public void Clear()
        {
            _stepList = new List<Step>();
            _rawStream = new List<ForcePoint>();
            _stepStream = new List<ForcePoint>();
        }


        /***************************** PRIVATE ZONE *****************************/
        /// <summary>
        /// Calculates the forcepoint stream representation of the steps 
        /// </summary>
        private void CalculateStepStream()
        {
            _signalFilter = new Filters();
            _stepStream = _signalFilter.CalculateStepSequence(this);
        }

        /// <summary>
        /// Calculate the steps from a filtered input stream of forcepoints
        /// </summary>
        /// <returns>The amount of steps</returns>
        private void CalculateSteps()
        {
            int stepNumber = 0;
            ForcePoint previousPoint;
            List<ForcePoint> _rawSteps = new List<ForcePoint>();
            
            if(_stepStream.Count.Equals(0))
                CalculateStepStream();

            previousPoint = _stepStream[0];

            foreach (ForcePoint forcePoint in _stepStream)
            {
                if (forcePoint.ForceValue > previousPoint.ForceValue)
                    _rawSteps = new List<ForcePoint>();

                if (forcePoint.ForceValue > 0)
                    _rawSteps.Add(_rawStream.Find(x => x.TimeStamp.Equals(forcePoint.TimeStamp)));


                if (forcePoint.ForceValue < previousPoint.ForceValue)
                    _stepList.Add(new Step(_rawSteps, _hoofLocation, _sensorLocation, ++stepNumber));

                previousPoint = forcePoint;
            }
        }       
    }
}
