using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///TODO:
///     1: Zero point calibration between steps!

namespace VetSoft
{
    class Equine
    {
        private List<Hoof> _hoofSet;
        private List<ForcePoint> _stepSequence;
        private List<Sensor> _finalList;
        private List<ForcePoint> _stepStream;


        public List<Sensor> FINALLIST { get { return _finalList; } }
        public List<ForcePoint> StepStream { get { return _stepStream; } }
             

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
            List<Sensor> finalList = new List<Sensor>();
            Hoof hoof = _hoofSet.Find(x => x.HoofLocation.Equals(Types.HoofLocation.FRONT_RIGHT));

            _finalList = hoof.SensorList;

            hoof.calcSteps();
                        
            //Console.WriteLine("Steps: " + hoof.Steps);

            
            _stepStream = hoof.StepStream;
        }       
    }
}
