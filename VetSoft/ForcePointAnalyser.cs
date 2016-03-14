using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace VetSoft
{
    class ForcePointAnalyser
    {
        public Sensor StepSequence(Sensor forcePointStream)
        {
            Sensor retVal = null;
            
            retVal = LPFilter(forcePointStream, 5);
            retVal = Differential(retVal);
            retVal = BinFilter(retVal, 0.15);

            retVal = calculateStepSequence(retVal);
            
            return retVal;
        }

        /// <summary>
        /// Filter the ForcePoint stream based on a treshold. If a force value is smaller than the threshold it's set to 0
        /// </summary>
        /// <param name="forcePointStream">The input stream of ForcePoints</param>
        /// <param name="treshold">The threshold</param>
        /// <returns>The filtered ForcePoint stream</returns>
        private Sensor BinFilter(Sensor forcePointStream, double treshold)
        {
            Sensor retVal = new Sensor(forcePointStream.HoofLocation, forcePointStream.SensorLocation, Types.StreamType.FILTERED);

            foreach (ForcePoint value in forcePointStream.RawStream)
                if (Math.Abs(value.ForceValue) <= treshold)
                    retVal.AddForcePoint(new ForcePoint(value.TimeStamp, 0));
                else
                    retVal.AddForcePoint(new ForcePoint(value.TimeStamp, value.ForceValue));

            return retVal;
        }
        
        /// <summary>
        /// Low Pass filter a list of ForcePoints by mean of a moving average.
        /// </summary>
        /// <param name="alpha">Defines te amount of samples required to take the average from</param>
        /// <returns>The filtered result of ForcePoints</returns>
        private Sensor LPFilter(Sensor forcePointStream, int alpha)
        {
            int median = (int)Math.Floor((double) alpha / 2);
            Sensor retVal = new Sensor(forcePointStream.HoofLocation, forcePointStream.SensorLocation, Types.StreamType.FILTERED);

            for (int i = (alpha - median); i < (forcePointStream.RawStream.Count - (alpha - median)); i++)
                retVal.AddForcePoint(new ForcePoint(forcePointStream.RawStream[i].TimeStamp, forcePointStream.RawStream.GetRange((i - (alpha - median)), alpha).Average(x => x.ForceValue)));

            return retVal;
        }


        /// <summary>
        /// Calculate the differential of a ForcePoint array
        /// </summary>
        /// <returns>The differentiated list of ForcePoints</returns>
        private Sensor Differential(Sensor forcePointStream)
        {
            Sensor retVal = new Sensor(forcePointStream.HoofLocation, forcePointStream.SensorLocation, Types.StreamType.FILTERED);
            ForcePoint previousPoint = forcePointStream.RawStream[0];

            foreach (ForcePoint forcePoint in forcePointStream.RawStream)
            {
                double differential = 0.0;

                if (!forcePoint.TimeStamp.Equals(previousPoint.TimeStamp))
                    differential = (forcePoint.ForceValue - previousPoint.ForceValue) / (forcePoint.TimeStamp - previousPoint.TimeStamp);

                retVal.AddForcePoint(new ForcePoint(forcePoint.TimeStamp, differential));
                previousPoint = forcePoint;
            }

            return retVal;
        }
        
        /// <summary>
        /// Calculate the step sequence from a filtered set of ForcePoints
        /// </summary>
        /// <param name="forcePointStream">The input stream of ForcePoints</param>
        /// <returns>The step sequence</returns>
        private Sensor calculateStepSequence(Sensor forcePointStream)
        {
            double stepVal = 500;
            double lastVal = 0;
            Sensor retVal = new Sensor(forcePointStream.HoofLocation, forcePointStream.SensorLocation, Types.StreamType.STEP);

            for(int i = 0; i < (forcePointStream.RawStream.Count - 1); i++)
            {
                bool endStep = false;
                bool startStep = false;

                if ((forcePointStream.RawStream[i].ForceValue < 0) && (forcePointStream.RawStream[i + 1].ForceValue >= 0))
                    endStep = true;

                if ((forcePointStream.RawStream[i].ForceValue <= 0) && (forcePointStream.RawStream[i + 1].ForceValue > 0))
                    startStep = true;

                if (endStep)
                    if (lastVal > 0)
                        lastVal -= stepVal;

                if (!endStep && startStep)
                    if (lastVal < stepVal)
                        lastVal += stepVal;

                retVal.AddForcePoint(new ForcePoint(forcePointStream.RawStream[i].TimeStamp, lastVal));                
            }

            return retVal;
        }
    }
}