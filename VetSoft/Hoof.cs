using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetSoft
{
    class Hoof
    {
        public string Name { get {return _name; } }
        public bool Present { get { return _present; } }
        public List<Sample> SampleList { get { return _sampleList; } }
        
        private string _name;
        private bool _present;
        private List<Sample> _sampleList;

        /// <summary>
        /// Constructor of the hoof class
        /// </summary>
        /// <param name="name">The name of the hoof</param>
        public Hoof(string name)
        {
            ///TODO: check if the name of the hoof is valid
            
            _name = name;
            _present = false;
            _sampleList = new List<Sample>();
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
            if (Present)
            {
                _sampleList.Add(sample);
                return true;
            }
            return false;
        }
    }
}
