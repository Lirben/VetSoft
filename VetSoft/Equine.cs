using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

///TODO:
///     Cleanup
/// 

namespace VetSoft
{
    class Equine
    {
        private List<Hoof> _hoofSet;
        private List<ForcePoint> _stepSequence;            

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
            List<Hoof> presentHoofs = _hoofSet.FindAll(x => x.Present);

            foreach (Hoof hoof in presentHoofs)            
                hoof.Analyse();            
        }       
    }
}
