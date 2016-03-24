using System;
using System.Linq;
using System.Collections.Generic;

namespace VetSoft
{
    /// <summary>
    /// Contains a series of filters to manipulate force point streams.
    /// </summary>
    class Filters
    {
        /// <summary>
        /// Get a list of ForcePoints indicating where a step is situated
        /// </summary>
        /// <param name="sensor">The sensor from which the step sequence must be calculated</param>
        /// <returns>A stream of forcepoints indicating where a step is located</returns>
        public List<ForcePoint> CalculateStepSequence(Sensor sensor)
        {
            List<ForcePoint> retVal = new List<ForcePoint>();
            
            retVal = LPFilter(sensor.RawStream, 5);
            retVal = Differential(retVal);
            retVal = BinFilter(retVal, 0.15);

            retVal = calculateStepSequence(retVal);
            
            return retVal;
        }


        /***************************** PRIVATE ZONE *****************************/


        /// <summary>
        /// Filter the ForcePoint stream based on a treshold. If a force value is smaller than the threshold it's set to 0
        /// </summary>
        /// <param name="inputStream">The input stream of ForcePoints</param>
        /// <param name="treshold">The threshold</param>
        /// <returns>The filtered ForcePoint stream</returns>
        private List<ForcePoint> BinFilter(List<ForcePoint> inputStream, double treshold)
        {
            List<ForcePoint> retVal = new List<ForcePoint>();

            foreach (ForcePoint value in inputStream)
                if (Math.Abs(value.ForceValue) <= treshold)
                    retVal.Add(new ForcePoint(value.TimeStamp, 0));
                else
                    retVal.Add(new ForcePoint(value.TimeStamp, value.ForceValue));

            return retVal;
        }
        
        /// <summary>
        /// Low Pass filter a list of ForcePoints by mean of a moving average.
        /// </summary>
        /// <param name="inputStream">The input stream of ForcePoints</param>
        /// <param name="alpha">Defines te amount of samples required to take the average from</param>
        /// <returns>The filtered result of ForcePoints</returns>
        private List<ForcePoint> LPFilter(List<ForcePoint> inputStream, int alpha)
        {
            int median = (int)Math.Floor((double) alpha / 2);
            List<ForcePoint> retVal = new List<ForcePoint>();

            for (int i = (alpha - median); i < (inputStream.Count - (alpha - median)); i++)
                retVal.Add(new ForcePoint(inputStream[i].TimeStamp, inputStream.GetRange((i - (alpha - median)), alpha).Average(x => x.ForceValue)));

            return retVal;
        }


        /// <summary>
        /// Calculate the differential of a ForcePoint array
        /// </summary>
        /// <param name="inputStream">The input stream of ForcePoints</param>
        /// <returns>The differentiated list of ForcePoints</returns>
        private List<ForcePoint> Differential(List<ForcePoint> inputStream)
        {
            List<ForcePoint> retVal = new List<ForcePoint>();
            ForcePoint previousPoint = inputStream[0];

            foreach (ForcePoint forcePoint in inputStream)
            {
                double differential = 0.0;

                if (!forcePoint.TimeStamp.Equals(previousPoint.TimeStamp))
                    differential = (forcePoint.ForceValue - previousPoint.ForceValue) / (forcePoint.TimeStamp - previousPoint.TimeStamp);

                retVal.Add(new ForcePoint(forcePoint.TimeStamp, differential));
                previousPoint = forcePoint;
            }

            return retVal;
        }
        
        /// <summary>
        /// Calculate the step sequence from a filtered set of ForcePoints
        /// </summary>
        /// <param name="inputStream">The input stream of ForcePoints</param>
        /// <returns>The step sequence</returns>
        private List<ForcePoint> calculateStepSequence(List<ForcePoint> inputStream)
        {
            double stepVal = 500;
            double lastVal = 0;
            List<ForcePoint> retVal = new List<ForcePoint>();

            for(int i = 0; i < (inputStream.Count - 1); i++)
            {
                bool endStep = false;
                bool startStep = false;

                if ((inputStream[i].ForceValue < 0) && (inputStream[i + 1].ForceValue >= 0))
                    endStep = true;

                if ((inputStream[i].ForceValue <= 0) && (inputStream[i + 1].ForceValue > 0))
                    startStep = true;

                if (endStep)
                    if (lastVal > 0)
                        lastVal -= stepVal;

                if (!endStep && startStep)
                    if (lastVal < stepVal)
                        lastVal += stepVal;

                retVal.Add(new ForcePoint(inputStream[i].TimeStamp, lastVal));                
            }

            return retVal;
        }
    }
}