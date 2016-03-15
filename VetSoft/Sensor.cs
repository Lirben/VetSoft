﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetSoft
{
    class Sensor
    {
        private ForcePointAnalyser _fpAnalyser;
        private Types.HoofLocation _hoofLocation;
        private Types.SensorLocation _sensorLocation;
        private List<ForcePoint> _rawStream;
        private List<ForcePoint> _stepStream;
        private List<Step> _stepList;


        public List<Step> Steps { get { return ReturnSteps(); } }
        public Types.HoofLocation HoofLocation { get { return _hoofLocation; } }
        public Types.SensorLocation SensorLocation { get { return _sensorLocation; } }
        public List<ForcePoint> RawStream { get { return _rawStream; } }
        public List<ForcePoint> StepStream { get { return ReturnStepStream(); } }


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
        /// Add a ForcePoint to the raw stream
        /// </summary>
        /// <param name="forcePoint">The ForcePoint that will be added to the stream</param>
        public void AddForcePoint(ForcePoint forcePoint)
        {
            _rawStream.Add(forcePoint);
        }

        public void Clear()
        {
            _stepList = new List<Step>();
            _rawStream = new List<ForcePoint>();
            _stepStream = new List<ForcePoint>();
        }

        private List<Step> ReturnSteps()
        {
            if (_stepList.Count.Equals(0))
                CalculateSteps();

            return _stepList;
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
