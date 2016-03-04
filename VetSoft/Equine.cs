using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///TODO:
///     1: Const Sample period must come from the hoof!
///     2: Provide zero point calibration between steps!

namespace VetSoft
{
    class Equine
    {
        private List<Hoof> _hoofSet;
        private List<ForcePoint> _stepSequence;
        private List<ForcePoint> _sourceList;
        private List<ForcePoint> _finalList;


        public List<ForcePoint> SOURCELIST { get { return _sourceList; } }
        public List<ForcePoint> FINALLIST { get { return _finalList; } }

        public int Steps { get { return CalculateSteps(); } }
        

        public Equine(List<Hoof> hoofSet)
        {
            _hoofSet = hoofSet;
            _stepSequence = new List<ForcePoint>();
        }


        /// <summary>
        /// Main entry point => first run this function
        /// </summary>
        public void process()
        {
            Hoof hoof = _hoofSet.Find(x => x.Name.Equals(Types.FRONT_RIGHT));
            List<ForcePoint> forceValues = new List<ForcePoint>();

            foreach(Sample sample in hoof.SampleList)
                forceValues.Add(new ForcePoint(sample.Time, sample.Data[1]));

            _sourceList = forceValues;

            //Perform low pass filtering
            List<ForcePoint> squareValues = SignalSquare(forceValues);
            List<ForcePoint> filteredValues = LPFilter(squareValues, 3);
            List<ForcePoint> differential = Differential(filteredValues);
            List<ForcePoint> integrated = Integrate(differential);            
            List<ForcePoint> compensated = CompensateInitialWaitSequence(integrated);
            
            
            calculateStepSequence(compensated);

            
            _finalList = _stepSequence;
        }

        private int CalculateSteps()
        {
            int retVal = 0;
            ForcePoint previousPoint = _stepSequence[0];
            
            foreach(ForcePoint forcePoint in _stepSequence)
            {
                if (forcePoint.ForceValue > previousPoint.ForceValue)
                    retVal++;

                previousPoint = forcePoint;
            }

            return retVal;
        }


        /// <summary>
        /// Multiply the signal with its own. This accentuates the slope coefficient for high force values.
        /// </summary>
        /// <param name="inputList">The list that must be multiplied</param>
        /// <returns>The multiplied list of ForcePoints</returns>
        private List<ForcePoint> SignalSquare(List<ForcePoint> inputList)
        {
            List<ForcePoint> retVal = new List<ForcePoint>();

            foreach (ForcePoint value in inputList)
                retVal.Add(new ForcePoint(value.TimeStamp, (value.ForceValue * value.ForceValue)));

            return retVal;
        }


        /// <summary>
        /// Low Pass filter a list of ForcePoints by mean of a moving average.
        /// </summary>
        /// <param name="inputList">The list that must be filtered</param>
        /// <param name="alpha">Defines te amount of samples required to take the average from</param>
        /// <returns>The filtered result of ForcePoints</returns>
        private List<ForcePoint> LPFilter(List<ForcePoint> inputList, int alpha)
        {
            List<ForcePoint> retVal = new List<ForcePoint>();

            for (int i = 0; i < (inputList.Count - alpha); i++)
                retVal.Add(new ForcePoint(inputList[i].TimeStamp, inputList.GetRange(i, alpha).Average(x => x.ForceValue)));
            
            return retVal;
        }


        /// <summary>
        /// Calculate the differential of a ForcePoint array
        /// </summary>
        /// <param name="inputList">The list that must be differentiated</param>
        /// <returns>The differentiated list of ForcePoints</returns>
        private List<ForcePoint> Differential(List<ForcePoint> inputList)
        {
            List<ForcePoint> retVal = new List<ForcePoint>();
            ForcePoint previousPoint = inputList[0];

            foreach(ForcePoint forcePoint in inputList)
            {
                double differential = 0.0;

                if(!forcePoint.TimeStamp.Equals(previousPoint.TimeStamp))
                    differential = (forcePoint.ForceValue - previousPoint.ForceValue) / (forcePoint.TimeStamp - previousPoint.TimeStamp);

                retVal.Add(new ForcePoint(forcePoint.TimeStamp, differential));
                previousPoint = forcePoint;
            }

            return retVal;
        }

        /// <summary>
        /// Calculate the integral of a ForcePoint array
        /// </summary>
        /// <param name="inputList">The list that must be integrated</param>
        /// <returns>The integrated list of ForcePoints</returns>
        private List<ForcePoint> Integrate(List<ForcePoint> inputList)
        {
            double sum = 0.0;
            List<ForcePoint> retVal = new List<ForcePoint>();
            ForcePoint previousValue = inputList[0];

            foreach(ForcePoint forcePoint in inputList)
            {                       //Take the time difference into account in case if some samples are missing
                sum += (forcePoint.ForceValue * ((forcePoint.TimeStamp - previousValue.TimeStamp) / 50));   ///TODO: Const Sample period must come from the hoof!
                retVal.Add(new ForcePoint(forcePoint.TimeStamp, sum));
                previousValue = forcePoint;
            }

            return retVal;
        }

        /// <summary>
        /// Offset a set of ForcePoints so that an initial waiting sequence is not influencing the result
        /// </summary>
        /// <param name="inputList">The input list of ForcePoints</param>
        /// <returns>The list of Forcepoints with an offset</returns>
        private List<ForcePoint> CompensateInitialWaitSequence(List<ForcePoint> inputList)
        {
            double minForcePoint = 0.0;
            List<ForcePoint> retVal = new List<ForcePoint>();

            foreach(ForcePoint forcePoint in inputList)
            {
                minForcePoint = Math.Min(minForcePoint, forcePoint.ForceValue);
                retVal.Add(new ForcePoint(forcePoint.TimeStamp, forcePoint.ForceValue + Math.Abs(minForcePoint)));
            }

            return retVal;
        }

        /// <summary>
        /// Calculate the ON/OFF function that can be plotted to show the steps of the horse
        /// </summary>
        /// <param name="inputList">The ist of filtered force values from which the steps can be calculated</param>
        private void calculateStepSequence(List<ForcePoint> inputList)
        {
            _stepSequence = new List<ForcePoint>();

            foreach (ForcePoint forcePoint in inputList)
                if (forcePoint.ForceValue > 250.0)
                    _stepSequence.Add(new ForcePoint(forcePoint.TimeStamp, 500.0));
                else
                    _stepSequence.Add(new ForcePoint(forcePoint.TimeStamp, 0.0));
        }
    }
}
