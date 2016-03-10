using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VetSoft
{
    class DataLogger
    {
        private XmlDocument _xmlDoc;

        public DataLogger()
        {
            _xmlDoc = new XmlDocument();

            XmlElement rootEl = _xmlDoc.CreateElement("Equine");

            _xmlDoc.AppendChild(rootEl);

        }

        public void WriteSample(Sample sample, Types.HoofLocation hoof)
        {
            XmlElement sampleEl = _xmlDoc.CreateElement("Sample");
            XmlElement timeEl = _xmlDoc.CreateElement("Time");
            XmlElement hoofEl = _xmlDoc.CreateElement("Hoof");
            XmlElement Data0El = _xmlDoc.CreateElement("RearLeft");
            XmlElement Data1El = _xmlDoc.CreateElement("TopLeft");
            XmlElement Data2El = _xmlDoc.CreateElement("RearRight");
            XmlElement Data3El = _xmlDoc.CreateElement("TopRight");

            timeEl.InnerText = sample.Time.ToString();
            hoofEl.InnerText = hoof.ToString();
            Data0El.InnerText = sample.Data[0].ToString();
            Data1El.InnerText = sample.Data[1].ToString();
            Data2El.InnerText = sample.Data[2].ToString();
            Data3El.InnerText = sample.Data[3].ToString();

            sampleEl.AppendChild(timeEl);
            sampleEl.AppendChild(hoofEl);
            sampleEl.AppendChild(Data0El);
            sampleEl.AppendChild(Data1El);
            sampleEl.AppendChild(Data2El);
            sampleEl.AppendChild(Data3El);

            _xmlDoc.DocumentElement.AppendChild(sampleEl);
        }

        public void SaveToFile(string xmlFileName)
        {
            _xmlDoc.Save("..\\..\\..\\DataLogs\\" + xmlFileName);
        }

    }
}
