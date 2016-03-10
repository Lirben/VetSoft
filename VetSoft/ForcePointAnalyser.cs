using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///TODO: Get description right
namespace VetSoft
{
    class ForcePointAnalyser
    {
        public ForcePointStream StepSequence(ForcePointStream forcePointStream)
        {
            ForcePointStream retVal = null;

            if (forcePointStream.StreamType.Equals(Types.StreamType.RAW))
            {
                retVal = SignalSquare(forcePointStream);
                retVal = LPFilter(retVal, 3);
                retVal = Differential(retVal);
                retVal = Integrate(retVal);
                retVal = CompensateInitialWaitSequence(retVal);
                retVal = calculateStepSequence(retVal);
            }

            return retVal;
        }

        /// <summary>
        /// Multiply the signal with its own. This accentuates the slope coefficient for high force values.
        /// </summary>
        /// <returns>The multiplied list of ForcePoints</returns>
        private ForcePointStream SignalSquare(ForcePointStream forcePointStream)
        {
            ForcePointStream retVal = new ForcePointStream(forcePointStream.HoofLocation, forcePointStream.SensorLocation, Types.StreamType.FILTERED);

            foreach (ForcePoint value in forcePointStream.RawStream)
                retVal.addForcePoint(new ForcePoint(value.TimeStamp, (value.ForceValue * value.ForceValue)));

            return retVal;
        }


        /// <summary>
        /// Low Pass filter a list of ForcePoints by mean of a moving average.
        /// </summary>
        /// <param name="alpha">Defines te amount of samples required to take the average from</param>
        /// <returns>The filtered result of ForcePoints</returns>
        private ForcePointStream LPFilter(ForcePointStream forcePointStream, int alpha)
        {
            ForcePointStream retVal = new ForcePointStream(forcePointStream.HoofLocation, forcePointStream.SensorLocation, Types.StreamType.FILTERED);

            for (int i = 0; i < (forcePointStream.RawStream.Count - alpha); i++)
                retVal.addForcePoint(new ForcePoint(forcePointStream.RawStream[i].TimeStamp, forcePointStream.RawStream.GetRange(i, alpha).Average(x => x.ForceValue)));

            return retVal;
        }


        /// <summary>
        /// Calculate the differential of a ForcePoint array
        /// </summary>
        /// <returns>The differentiated list of ForcePoints</returns>
        private ForcePointStream Differential(ForcePointStream forcePointStream)
        {
            ForcePointStream retVal = new ForcePointStream(forcePointStream.HoofLocation, forcePointStream.SensorLocation, Types.StreamType.FILTERED);
            ForcePoint previousPoint = forcePointStream.RawStream[0];

            foreach (ForcePoint forcePoint in forcePointStream.RawStream)
            {
                double differential = 0.0;

                if (!forcePoint.TimeStamp.Equals(previousPoint.TimeStamp))
                    differential = (forcePoint.ForceValue - previousPoint.ForceValue) / (forcePoint.TimeStamp - previousPoint.TimeStamp);

                retVal.addForcePoint(new ForcePoint(forcePoint.TimeStamp, differential));
                previousPoint = forcePoint;
            }

            return retVal;
        }

        /// <summary>
        /// Calculate the integral of a ForcePoint array
        /// </summary>
        /// <returns>The integrated list of ForcePoints</returns>
        private ForcePointStream Integrate(ForcePointStream forcePointStream)
        {
            double sum = 0.0;
            double samplePeriod = CalculateSamplePeriod(forcePointStream);
            ForcePointStream retVal = new ForcePointStream(forcePointStream.HoofLocation, forcePointStream.SensorLocation, Types.StreamType.FILTERED);
            ForcePoint previousValue = forcePointStream.RawStream[0];

            foreach (ForcePoint forcePoint in forcePointStream.RawStream)
            {                       //Take the time difference into account in case if some samples are missing
                sum += (forcePoint.ForceValue * ((forcePoint.TimeStamp - previousValue.TimeStamp) / samplePeriod));
                retVal.addForcePoint(new ForcePoint(forcePoint.TimeStamp, sum));
                previousValue = forcePoint;
            }

            return retVal;
        }

        /// <summary>
        /// Offset a set of ForcePoints so that an initial waiting sequence is not influencing the result
        /// </summary>
        /// <returns>The list of Forcepoints with an offset</returns>
        private ForcePointStream CompensateInitialWaitSequence(ForcePointStream forcePointStream)
        {
            double minForcePoint = forcePointStream.RawStream.Min(x => x.ForceValue);
            ForcePointStream retVal = new ForcePointStream(forcePointStream.HoofLocation, forcePointStream.SensorLocation, Types.StreamType.FILTERED);

            foreach (ForcePoint forcePoint in forcePointStream.RawStream)
            {
                //minForcePoint = Math.Min(minForcePoint, forcePoint.ForceValue);
                retVal.addForcePoint(new ForcePoint(forcePoint.TimeStamp, forcePoint.ForceValue + Math.Abs(minForcePoint)));
            }

            return retVal;
        }

        /// <summary>
        /// Calculate the ON/OFF function that can be plotted to show the steps of the horse
        /// </summary>
        /// <param name="inputList">The ist of filtered force values from which the steps can be calculated</param>
        private ForcePointStream calculateStepSequence(ForcePointStream forcePointStream)
        {
            double treshHold = (GetAmplitude(forcePointStream) * 0.075);
            ForcePointStream retVal = new ForcePointStream(forcePointStream.HoofLocation, forcePointStream.SensorLocation, Types.StreamType.STEP);

            foreach (ForcePoint forcePoint in forcePointStream.RawStream)
            {
                if (forcePoint.ForceValue > treshHold)
                    retVal.addForcePoint(new ForcePoint(forcePoint.TimeStamp, 500.0));
                else
                    retVal.addForcePoint(new ForcePoint(forcePoint.TimeStamp, 0.0));
            }

            return retVal;
        }

        /// <summary>
        /// Calucate the sample period from a list of forcepoints
        /// </summary>
        /// <returns>The sample period. If infinite, no valid sample period could be determined</returns>
        private double CalculateSamplePeriod(ForcePointStream forcePointStream)
        {
            double retVal = double.PositiveInfinity;
            ForcePoint previousVal = forcePointStream.RawStream[forcePointStream.RawStream.Count - 1];

            foreach (ForcePoint forcePoint in forcePointStream.RawStream)
            {
                retVal = Math.Min(retVal, (forcePoint.TimeStamp - previousVal.TimeStamp));
                previousVal = forcePoint;
            }

            return retVal;
        }

        private double GetAmplitude(ForcePointStream forcePointStream)
        {
            return forcePointStream.RawStream.Max(x => x.ForceValue);
        }
    }
}