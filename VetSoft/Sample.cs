using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetSoft
{
    class Sample
    {
        public uint Time { get { return _time; } }
        public double[] Data { get { return _data; } }

        private uint _time;
        private double[] _data;

        public Sample(uint time, double[] data)
        {
            _time = time;
            _data = data;
        }

        public Sample(uint time, string[] data)
        {
            _time = time;
            _data = new double[data.Length];

            for (int i = 0; i < data.Length; i++)
                _data[i] = double.Parse(data[i]);
            
        }
    }
}
