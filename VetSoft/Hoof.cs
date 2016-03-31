using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

///TODO: clean up RoughStepCalculation()
namespace VetSoft
{
    /// <summary>
    /// A representation of a hoof of a horse
    /// </summary>
    class Hoof
    {

        public int Address { get { return _address; } set { _address = value; } }
        public int Steps { get { return _steps; } }
        public List<Sample> SampleList { get { return _sampleList; } }
        public Types.HoofLocation HoofLocation { get { return _hoofLocation; } }
        public List<Sensor> SensorList { get { return _sensorList; } }
        public List<ForcePoint> StepStream { get { return _stepStream; } }

        private int _address;
        private int _steps;
        private bool _present;
        private Types.HoofLocation _hoofLocation;
        private List<Sample> _sampleList;
        private List<Step> _stepListHoof;
        private List<Sensor> _sensorList;
        private List<ForcePoint> _stepStream;


        /// <summary>
        /// Constructor of the hoof class
        /// </summary>
        /// <param name=
        /// Location">The location of the hoof on the horse</param>
        public Hoof(Types.HoofLocation hoofLocation, int address)
        {
            _address = address;
            _hoofLocation = hoofLocation;
            this.Clear();
        }

        /// <summary>
        /// Perform analysis on this hoof
        /// </summary>
        public void Analyse()
        {
            if(_stepStream.Count.Equals(0))
                RoughStepCalculation();
        }

        /// <summary>
        /// Set the hoof to present on the horse
        /// </summary>
        public void setPresent(bool present = true)
        {
            _present = present;
        }

        /// <summary>
        /// Empty the hoof from all ForcePoint & step data;
        /// </summary>
        public void Clear()
        {
            _steps = -1;
            _present = false;
            _stepListHoof = new List<Step>();
            _sampleList = new List<Sample>();
            _stepStream = new List<ForcePoint>();
            _sensorList = new List<Sensor>(4);

            for (int i = 0; i < 4; i++)
                _sensorList.Add(new Sensor(_hoofLocation, (Types.SensorLocation)i));
        }

        /// <summary>
        /// Add a data sample to this hoof. The hoof must be present
        /// </summary>
        /// <param name="sample">The sample that is part of this hoof</param>
        /// <returns>Returns false if the hoof is not present. True if everything is ok</returns>
        public bool addSample(Sample sample)
        {
            foreach (Sensor sensor in _sensorList)
                sensor.AddForcePoint(new ForcePoint(sample.Time, sample.Data[(int)sensor.SensorLocation]));

            _sampleList.Add(sample);

            return true;            
        }


        /***************************** PRIVATE ZONE *****************************/

        /// <summary>
        /// Merge the rough step calculation of the sensors into a step calculation for the hoof
        /// </summary>
        private void RoughStepCalculation()
        {
            int updateStepSet = -1;

            //Perform analysis on each sensor
            foreach (Sensor sensor in _sensorList)
                sensor.Analyse();

            //Generate the list of all the steps that belong together
            List<List<Step>> stepSetArray = GenerateStepSet(_sensorList);

            for (int currentStep = 0; currentStep < stepSetArray.Count; currentStep++)
            {
                
                uint minStartTime = stepSetArray[currentStep].Min(x => x.StartTime);
                uint maxStartTime = stepSetArray[currentStep].Max(x => x.StartTime);
                uint minStopTime = stepSetArray[currentStep].Min(x => x.StopTime);
                uint maxStopTime = stepSetArray[currentStep].Max(x => x.StopTime);

                //If one of the steps is in the time frame of the previous step
                //If a new step has been created because of a step that's in the time frame of the previous step => do not execute this part
                if (_stepListHoof.Count > 0 && updateStepSet.Equals(currentStep - 1))               
                    while (minStartTime < _stepListHoof[_stepListHoof.Count - 1].StopTime)
                    {
                        updateStepSet = currentStep;
                        Step stepToShift = stepSetArray[currentStep].Find(x => x.StartTime.Equals(minStartTime));
                        shiftStep(stepToShift, stepSetArray);

                        minStartTime = stepSetArray[currentStep].Min(x => x.StartTime);
                        maxStartTime = stepSetArray[currentStep].Max(x => x.StartTime);
                        minStopTime = stepSetArray[currentStep].Min(x => x.StopTime);
                    }

                //If one of the steps is a step that belongs to a step in the future
                while (maxStartTime > minStopTime)
                {
                    Step stepToShift = stepSetArray[currentStep].Find(x => x.StartTime.Equals(maxStartTime));
                    shiftStep(stepToShift, stepSetArray);

                    maxStartTime = stepSetArray[currentStep].Max(x => x.StartTime);
                    minStopTime = stepSetArray[currentStep].Min(x => x.StopTime);
                }

                //Create a step for this hoof from the sensorsteps
                //Don't trust a step that's only detected by 1 sensor
                if (stepSetArray[currentStep].Count > 1)
                {
                    //Calculate necesserry parameters
                    int stepNr = stepSetArray[currentStep].Min(x => x.StepNumber);

                    minStartTime = stepSetArray[currentStep].Min(x => x.StartTime);
                    maxStopTime = stepSetArray[currentStep].Max(x => x.StopTime);                  

                    //Add new general step to the hoof's steplist
                    _stepListHoof.Add(new Step(minStartTime, maxStopTime, _hoofLocation, stepNr));
                }               
            }

            _steps = _stepListHoof.Count;

            generateStepStream();
        }

        /// <summary>
        /// Shift a step in a stepList array. Use this function to align steps from sensors with each other,
        /// so every column in the array holds steps that are part of the same hoof step
        /// </summary>
        /// <param name="stepToShift">The step that will be shifted further in the future</param>
        /// <param name="stepListArray">The array that contains the step that must be shifted</param>
        private void shiftStep(Step stepToShift, List<List<Step>> stepListArray)
        {
            List<Step> stepList = stepListArray.Find(x => x.Exists(y => y.Equals(stepToShift)));
            Step stepHolder = stepToShift;
            int stepIndex = stepListArray.FindIndex(x => x.Equals(stepList));
            int sensorIndex = stepList.FindIndex(x => x.SensorLocation.Equals(stepToShift.SensorLocation));

            for(int index = stepIndex; (index < stepListArray.Count); index++)
            {
                if (stepHolder != null)
                {
                    stepHolder.StepNumber++;
                    stepListArray[index].Remove(stepHolder);

                    if (stepHolder.StopTime < stepListArray[index + 1].Min(x => x.StartTime))
                    {
                        stepListArray.Insert((index + 1), new List<Step>());

                        for (int i = (index + 1); i < stepListArray.Count; i++)
                            foreach (Step step in stepListArray[i])
                                step.StepNumber++;
                    }

                    if ((index + 1).Equals(stepListArray.Count))
                        stepListArray.Add(new List<Step>());

                    if (sensorIndex > stepListArray[index + 1].Count)
                        sensorIndex = stepListArray[index + 1].Count;

                    stepListArray[index + 1].Insert(sensorIndex, stepHolder);
                    stepHolder = stepListArray[index + 1].Find(x => (x.SensorLocation.Equals(stepToShift.SensorLocation)) && (!x.StartTime.Equals(stepHolder.StartTime)));
                }
            }
        }

        /// <summary>
        /// Generate a 2 dimensional list of steps, where the same steps on different sensors are grouped together based on step number
        /// </summary>
        /// <param name="sensorList">The sensors of which the steps must be taken into account</param>
        /// <returns>The 2 dimensional list of steps</returns>
        private List<List<Step>> GenerateStepSet(List<Sensor> sensorList)
        {
            int maxSteps = _sensorList.Max(x => x.StepList.Count);
            List<List<Step>> retVal = new List<List<Step>>();

            for (int currentStep = 1; currentStep <= maxSteps; currentStep++)
            {
                List<Step> sameStep = new List<Step>();

                foreach (Sensor sensor in _sensorList)
                {
                    Step step = sensor.StepList.Find(x => x.StepNumber.Equals(currentStep));

                    if (step != null)
                        sameStep.Add(step);
                }

                retVal.Add(sameStep);
            }

            return retVal;
        }

        /// <summary>
        /// Create the stepstream with forcepoints from a regular step stream
        /// </summary>
        private void generateStepStream()
        {
            for (uint i = _sampleList[0].Time; i <= _sampleList[_sampleList.Count - 1].Time; i += 50)
                _stepStream.Add(new ForcePoint(i, 0));

            foreach (Step step in _stepListHoof)
                for (uint i = step.StartTime; i < (step.StopTime + 1); i += 50)
                {
                    int index = _stepStream.FindIndex(x => x.TimeStamp.Equals(i));
                    _stepStream.RemoveAt(index);
                    _stepStream.Insert(index, new ForcePoint(i, 500));
                }
        }
    }
}
