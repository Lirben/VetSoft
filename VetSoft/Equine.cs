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
        private List<ForcePointStream> _finalList;

        public List<ForcePointStream> FINALLIST { get { return _finalList; } }
             

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
            Hoof hoof = _hoofSet.Find(x => x.HoofLocation.Equals(Types.HoofLocation.FRONT_RIGHT));
            ForcePointAnalyser fpAnalyser = new ForcePointAnalyser();
            List<ForcePointStream> finalList = new List<ForcePointStream>();

            
            _finalList = hoof.ForcePointStreams;
        }       
    }
}
