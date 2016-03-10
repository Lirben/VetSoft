using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


///TODO:
///     Check if the hoofname is valid

namespace VetSoft
{
    class Hoof
    {
        public bool Present { get { return _present; } }
        public List<Sample> SampleList { get { return _sampleList; } }
        public Types.HoofLocation HoofLocation { get {return _hoofLocation; } }
        public List<ForcePointStream> ForcePointStreams { get { return _forcePointStream; } }
        
        private Types.HoofLocation _hoofLocation;
        private List<Sample> _sampleList;
        private List<ForcePointStream> _forcePointStream;
        private bool _present;


        /// <summary>
        /// Constructor of the hoof class
        /// </summary>
        /// <param name="name">The name of the hoof</param>
        public Hoof(Types.HoofLocation hoofLocation)
        {           
            _hoofLocation = hoofLocation;
            _present = false;
            _sampleList = new List<Sample>();
            _forcePointStream = new List<ForcePointStream>(4);

            for (int i = 0; i < 4; i++)
                _forcePointStream.Add(new ForcePointStream(hoofLocation, (Types.SensorLocation)i));
        }

        /// <summary>
        /// Set the hoof to present in the system
        /// </summary>
        public void setPresent(bool present = true)
        {
            _present = present;
        }
        
        /// <summary>
        /// Add a data sample to this hoof. The hoof must be present
        /// </summary>
        /// <param name="sample">The sample that is part of this hoof</param>
        /// <returns>Returns false if the hoof is not present. True if everything is ok</returns>
        public bool addSample(Sample sample)
        {
            if(Present)
            { 
                foreach (ForcePointStream fpStream in _forcePointStream)
                    fpStream.addForcePoint(new ForcePoint(sample.Time, sample.Data[(int) fpStream.SensorLocation]));

                _sampleList.Add(sample);

                return true;
            }

            return false;
        }
    }
}
