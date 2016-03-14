using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace VetSoft
{
    class Hoof
    {
        public int Steps { get { return GetSteps(); } }
        public bool Present { get { return _present; } }
        public List<Sample> SampleList { get { return _sampleList; } }
        public Types.HoofLocation HoofLocation { get { return _hoofLocation; } }
        public List<Sensor> SensorList { get { return _sensorList; } }
        public List<ForcePoint> StepStream { get { return _stepStream; } }

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
        /// <param name="name">The name of the hoof</param>
        public Hoof(Types.HoofLocation hoofLocation)
        {
            _hoofLocation = hoofLocation;
            this.Clear();
        }

        /// <summary>
        /// Set the hoof to present in the system
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
            if (Present)
            {
                foreach (Sensor fpStream in _sensorList)
                    fpStream.AddForcePoint(new ForcePoint(sample.Time, sample.Data[(int)fpStream.SensorLocation]));

                _sampleList.Add(sample);

                return true;
            }

            return false;
        }


        private int GetSteps()
        {
            if (_steps.Equals(-1))
                CalculateSteps();

            return _steps;
        }

        /// <summary>
        /// Version 2 of rough step calculation
        /// </summary>
        public void calcSteps()
        {
            //Generate the list of all the steps that belong together
            List<List<Step>> stepSetArray = GenerateStepSet(_sensorList);

            for (int currentStep = 0; currentStep < stepSetArray.Count; currentStep++)
            {
                uint minStartTime = stepSetArray[currentStep].Min(x => x.StartTime);
                uint maxStartTime = stepSetArray[currentStep].Max(x => x.StartTime);
                uint minStopTime = stepSetArray[currentStep].Min(x => x.StopTime);
                uint maxStopTime = stepSetArray[currentStep].Max(x => x.StopTime);

                //If one of the steps is a step that belongs to a step in the past
                if (_stepListHoof.Count > 0)
                    if (minStartTime < _stepListHoof[_stepListHoof.Count - 1].StopTime)
                    {
                        Step stepToRemove = stepSetArray[currentStep].Find(x => x.StartTime.Equals(minStartTime));
                        removeStep(stepToRemove, stepSetArray);

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

                if (stepSetArray[currentStep].Count > 0)
                {
                    //Calculate necesserry parameters
                    int stepNr = stepSetArray[currentStep].Min(x => x.StepNumber);

                    minStartTime = stepSetArray[currentStep].Min(x => x.StartTime);
                    maxStopTime = stepSetArray[currentStep].Max(x => x.StopTime);
                    
                    

                    //Add new general step to the hoof's steplist
                    _stepListHoof.Add(new Step(minStartTime, maxStopTime, _hoofLocation, stepNr));
                }
            }

            generateStepStream();
        }

        private void shiftStep(Step stepToShift, List<List<Step>> stepListArray)
        {
            List<Step> stepList = stepListArray.Find(x => x.Exists(y => y.Equals(stepToShift)));
            Step stepHolder = stepToShift;
            int stepIndex = stepListArray.FindIndex(x => x.Equals(stepList));
            int sensorIndex = stepList.FindIndex(x => x.SensorLocation.Equals(stepToShift.SensorLocation));

            for(int index = stepIndex; index < stepListArray.Count; index++)
            {
                if (stepHolder != null)
                {
                    stepHolder.StepNumber++;
                    stepListArray[index].Remove(stepHolder);

                    if ((index + 1).Equals(stepListArray.Count))
                        stepListArray.Add(new List<Step>());

                    if (sensorIndex > stepListArray[index + 1].Count)
                        sensorIndex = stepListArray[index + 1].Count;

                    stepListArray[index + 1].Insert(sensorIndex, stepHolder);
                    stepHolder = stepListArray[index + 1].Find(x => (x.SensorLocation.Equals(stepToShift.SensorLocation)) && (!x.StartTime.Equals(stepHolder.StartTime)));
                }
            }
        }

        private void removeStep(Step stepToRemove, List<List<Step>> stepListArray)
        {
            List<Step> stepList = stepListArray.Find(x => x.Exists(y => y.Equals(stepToRemove)));
            int stepIndex = stepListArray.FindIndex(x => x.Equals(stepList));
            int sensorIndex = stepList.FindIndex(x => x.SensorLocation.Equals(stepToRemove.SensorLocation));

            stepList.Remove(stepToRemove);

            for (int index = stepIndex; index < (stepListArray.Count - 1); index++)
            {
                int nextStepSensorIndex = stepListArray[index + 1].FindIndex(x => x.SensorLocation.Equals(stepToRemove.SensorLocation));

                if (!nextStepSensorIndex.Equals(-1))
                {
                    stepListArray[index].Insert(sensorIndex, stepListArray[index + 1][nextStepSensorIndex]);
                    stepListArray[index + 1].RemoveAt(nextStepSensorIndex);
                    stepListArray[index][sensorIndex].StepNumber--;                    
                }

                if (stepListArray[index + 1].Count.Equals(0))
                    stepListArray.RemoveAt(index + 1);
            }
        }

        /// <summary>
        /// Generate a 2 dimensional list of steps, where the same steps on different sensors are grouped together based on step number
        /// </summary>
        /// <param name="sensorList">The sensors of which the steps must be taken into account</param>
        /// <returns>The 2 dimensional list of steps</returns>
        private List<List<Step>> GenerateStepSet(List<Sensor> sensorList)
        {
            int maxSteps = _sensorList.Max(x => x.Steps.Count);
            List<List<Step>> retVal = new List<List<Step>>();

            for (int currentStep = 1; currentStep <= maxSteps; currentStep++)
            {
                List<Step> sameStep = new List<Step>();

                foreach (Sensor sensor in _sensorList)
                {
                    Step step = sensor.Steps.Find(x => x.StepNumber.Equals(currentStep));

                    if (step != null)
                        sameStep.Add(step);
                }

                retVal.Add(sameStep);
            }

            return retVal;
        }

        private void CheckStep(List<Step> stepSet)
        {
            int stepNr = stepSet.Min(x => x.StepNumber);
            uint minStartTime = stepSet.Min(x => x.StartTime);
            uint maxStartTime = stepSet.Max(x => x.StartTime);
            uint minStopTime = stepSet.Min(x => x.StopTime);
            uint maxStopTime = stepSet.Max(x => x.StopTime);

            //If one of the steps is a step that is further away in the future
            if (maxStartTime > minStopTime)
            { }

            //If one of the steps is a step that belongs to a step in the past
            if (_stepListHoof.Count > 0)
                if (minStartTime < _stepListHoof[_stepListHoof.Count - 1].StopTime)
                {
                    stepSet.RemoveAll(x => x.StartTime.Equals(minStartTime));
                }

            _stepListHoof.Add(new Step(minStartTime, maxStopTime, _hoofLocation, stepNr));

        }

        public void CalculateSteps()
        {
            List<List<Step>> stepListSensors = new List<List<Step>>(_sensorList.Count);

            for (int i = 0; i < _sensorList.Count; i++)
                stepListSensors.Add(_sensorList[i].Steps);

            int maxSteps = stepListSensors.Max(x => x.Max(y => y.StepNumber));

            for (int curStep = 1; curStep < maxSteps; curStep++)
            {
                List<Step> currentStep = new List<Step>();
                maxSteps = stepListSensors.Max(x => x.Max(y => y.StepNumber));

                foreach (List<Step> stepList in stepListSensors)
                {
                    Step dummy = stepList.Find(x => x.StepNumber.Equals(curStep));

                    if (dummy != null)
                        currentStep.Add(dummy);
                }

                double avgStopTime = double.PositiveInfinity;

                if (currentStep.Count > 0)
                    avgStopTime = currentStep.Average(x => x.StopTime);

                for (int i = 0; i < currentStep.Count; i++)
                {
                    //If some steps are outside of the other steps => create a new step on that line
                    if (currentStep[i].StartTime > avgStopTime)
                    {
                        List<Step> update = stepListSensors.Find(x => x.Exists(y => y.Equals(currentStep[i])));

                        currentStep.RemoveAt(i);
                        for (int j = curStep; j < update.Count; j++)
                            update[j].StepNumber++;
                    }

                    //If multiple steps are detected inside other steps => remove steps from that line
                    if (_stepListHoof.Count > 0 && i < currentStep.Count)
                        if (currentStep[i].StartTime < _stepListHoof[_stepListHoof.Count - 1].StopTime)
                        {
                            List<Step> update = stepListSensors.Find(x => x.Exists(y => y.Equals(currentStep[i])));

                            currentStep.RemoveAt(i);
                            update.RemoveAll(x => x.StepNumber.Equals(curStep));

                            for (int j = curStep; j < update.Count; j++)
                                update[j].StepNumber--;
                        }
                }

                createNewStep(currentStep);
            }

            _steps = _stepListHoof.Count;

            generateStepStream();
        }

        private void createNewStep(List<Step> stepList)
        {
            if (stepList.Count > 0)
            {
                int stepNr = stepList.Min(x => x.StepNumber);
                uint startTime = stepList.Min(x => x.StartTime);
                uint stopTime = stepList.Max(x => x.StopTime);

                _stepListHoof.Add(new Step(startTime, stopTime, _hoofLocation, stepNr));
            }
        }



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
