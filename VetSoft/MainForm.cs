using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.IO;


namespace VetSoft
{
    public partial class frmMain : Form
    {
        private readonly string COMPORT = "COM5";
        private readonly int BAUDRATE = 57600;

        //Flags
        private bool _answer;
        private bool _console;

        //Variables
        private string _fileName;
        private Serial _serialReader;
        private DataLogger _dataLogger;
        private List<Hoof> _hoofList;
        private Equine _equine;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            //Serial setup
            _serialReader = new Serial();
            _serialReader.dataRecieved += new Serial.serialEvent(recievedData);

            //DataLogger setup
            _dataLogger = new DataLogger();
            _fileName = Types.RandomString(6) + ".xml";

            //Hoofs setup
            _hoofList = new List<Hoof>(4);
            _hoofList.Add(new Hoof(Types.FRONT_LEFT));
            _hoofList.Add(new Hoof(Types.FRONT_RIGHT));
            _hoofList.Add(new Hoof(Types.HIND_LEFT));
            _hoofList.Add(new Hoof(Types.HIND_RIGHT));

            //Charts setup
            setupCharts();          
                     
            //Flags setup
            _answer = false;
            _console = true;

            //GUI setup
            txtScale.Text = 0.ToString();
        }

        private void pnCloseConsole_MouseClick(object sender, MouseEventArgs e)
        {
            this.Width -= lsConsole.Width;

            lsConsole.Visible = false;
            pnCloseConsole.Visible = false;
            pnOpenConsole.Visible = true;

            _console = false;
        }

        private void pnOpenConsole_MouseClick(object sender, MouseEventArgs e)
        {
            this.Width += lsConsole.Width;

            lsConsole.Visible = true;
            pnCloseConsole.Visible = true;
            pnOpenConsole.Visible = false;

            _console = true;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            pbNoConnect.Visible = false;
            lsConsole.Items.Clear();

            if(connectToSerial(COMPORT, BAUDRATE))
            {
                if (_console)
                    lsConsole.Items.Add("Connected to " + COMPORT + "@" + BAUDRATE + " bauds");

                btnConnect.Visible = false;
                btnDisconnect.Visible = true;
                btnMeasure.Visible = true;
                btnSave.Enabled = false;
            }
            else
            {
                if (_console)
                    lsConsole.Items.Add("Error: " + _serialReader.LastError);

                pbNoConnect.Visible = true;
                btnConnect.Visible = true;
                btnDisconnect.Visible = false;
                btnMeasure.Visible = false;
                btnSave.Enabled = false;
            }            
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (_serialReader.disconnect())
            {
                if(_console)
                    lsConsole.Items.Add("Disconnected from " + COMPORT);

                foreach (Hoof hoof in _hoofList.FindAll(x => x.Present))
                    hoof.setPresent(false);

                btnConnect.Visible = true;
                btnDisconnect.Visible = false;
                btnMeasure.Visible = false;
                btnSave.Enabled = false;
            }
        }

        private void btnMeasure_Click(object sender, EventArgs e)
        {
            _serialReader.send("{\"type\": 0,\"command\":\"update\",\"parameter\":\"transmitRaw\",\"value\":\"true\"}");

            btnDisconnect.Enabled = false;
            btnMeasure.Visible = false;
            btnIdle.Visible = true;
            btnSave.Enabled = false;
        }

        private void btnIdle_Click(object sender, EventArgs e)
        {
            _answer = false;

            while (!_answer)
            {
                _serialReader.send("{\"type\": 0,\"command\":\"update\",\"parameter\":\"transmitRaw\",\"value\":\"false\"}");
                Thread.Sleep(50);
            }

            btnDisconnect.Enabled = true;
            btnMeasure.Visible = true;
            btnIdle.Visible = false;
            btnSave.Enabled = true;
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            string[] fileList = Directory.GetFiles("../../../DataLogs/");
                        
            foreach (string file in fileList)
            {
                string[] fileSplit = file.Split('/');
                writeLine("\t ● " + fileSplit[fileSplit.Length - 1]);
            }

            writeLine("Choose a file from the list");

            btnSave.Enabled = false;

        }

        private void lsConsole_SelectedValueChanged(object sender, EventArgs e)
        {
            string selectedValue = lsConsole.SelectedItem.ToString();

            if (selectedValue.StartsWith("\t ● "))
            {
                clearGraphs();

                foreach (Hoof hoof in _hoofList)
                {
                    hoof.setPresent(false);
                    hoof.SampleList.Clear();
                }

                loadFile(selectedValue.Split('●')[1].TrimStart(' '));
            }

            renderCharts();


            ///TODO:REFACTOR

            _equine = new Equine(_hoofList);
            _equine.process();

            List<ForcePoint> output = _equine.FINALLIST;

            MethodInvoker graphAction;

            foreach (ForcePoint value in output)
            {
                graphAction = delegate
                {
                    chFR.Series["TopRight"].Points.AddXY(value.TimeStamp, value.ForceValue);
                };

                chFL.BeginInvoke(graphAction);
            }

            writeLine(_equine.Steps + " steps analysed");
          

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (Hoof hoof in _hoofList)
                foreach (Sample sample in hoof.SampleList)
                    _dataLogger.WriteSample(sample, hoof.Name);

            _dataLogger.SaveToFile(Types.RandomString(6) + ".xml");
        }

        private void txtScale_TextChanged(object sender, EventArgs e)
        {
            double interval;

            double.TryParse(txtScale.Text, out interval);

            chFR.ChartAreas[0].AxisX.Maximum = chFR.ChartAreas[0].AxisX.Minimum + (interval * 1000);
            chFL.ChartAreas[0].AxisX.Maximum = chFL.ChartAreas[0].AxisX.Minimum + (interval * 1000);
            chSteps.ChartAreas[0].AxisX.Maximum = chSteps.ChartAreas[0].AxisX.Minimum + (interval * 1000);

            chFR.Update();
            chFL.Update();
            chSteps.Update();

        }

        private void lblLeft_Click(object sender, EventArgs e)
        {
            if (chFR.ChartAreas[0].AxisX.Minimum > 999)
            {
                chFR.ChartAreas[0].AxisX.Minimum -= 1000;
                chFR.ChartAreas[0].AxisX.Maximum -= 1000;
                chFL.ChartAreas[0].AxisX.Minimum -= 1000;
                chFL.ChartAreas[0].AxisX.Maximum -= 1000;
                chSteps.ChartAreas[0].AxisX.Minimum -= 1000;
                chSteps.ChartAreas[0].AxisX.Maximum -= 1000;
                chFR.Update();
                chFL.Update();
                chSteps.Update();
            }
        }

        private void lblRight_Click(object sender, EventArgs e)
        {
            
            chFR.ChartAreas[0].AxisX.Minimum += 1000;
            chFR.ChartAreas[0].AxisX.Maximum += 1000;
            chFL.ChartAreas[0].AxisX.Minimum += 1000;
            chFL.ChartAreas[0].AxisX.Maximum += 1000;
            chSteps.ChartAreas[0].AxisX.Minimum += 1000;
            chSteps.ChartAreas[0].AxisX.Maximum += 1000;

            chFL.ChartAreas[0].AxisY.Maximum = double.NaN;
            chFR.Update();
            chFL.Update();
            chSteps.Update();
        }

        /// <summary>
        /// Connect to the serial port
        /// </summary>
        /// <param name="comPort">The COM port to connect to</param>
        /// <param name="baudRate">The Baudrate of the communication</param>
        /// <returns>True if the connection is made, false if not</returns>
        private bool connectToSerial(string comPort, int baudRate)
        {
            if (_serialReader.connect(comPort, baudRate))
            {
                _serialReader.send("{\"type\": 0,\"command\":\"read\",\"parameter\":\"status\"}");

                return true;
            }
            return false;
        }

        /// <summary>
        /// Deal with events from Serial line
        /// </summary>
        /// <param name="data">Data coming from the serial Line</param>
        private void recievedData(string data)
        {
            JObject objJson = JObject.Parse(data);

            switch ((int) objJson.GetValue("type"))
            {
                case Types.RESPONSE_PACKAGE_TYPE:
                    processResponsePacket(data);
                    break;

                case Types.DATA_PACKAGE_TYPE:
                    processDataPacket(data);
                    break;
            }
        }

        /// <summary>
        /// Assigns the data packet to the correct hoof in memory.
        /// </summary>
        /// <param name="data">The json description of the data packet</param>
        private void processDataPacket(string data)
        {
            Types.DataPackage dataPackage = JsonConvert.DeserializeObject<Types.DataPackage>(data);
            _hoofList.Find(x => x.Name.Equals(dataPackage.hoof)).addSample(new Sample(dataPackage.time, dataPackage.data));

            renderCharts();

            if (_console)
                writeLine(data);           
        }

        /// <summary>
        /// Render the charts with data stored in the _hoofList object.
        /// </summary>
        private void renderCharts()
        {
            MethodInvoker graphAction;
            MethodInvoker scaleAction;

            foreach (Hoof hoof in _hoofList.Where(x => x.Present))
                foreach (Sample sample in hoof.SampleList)
                {
                    switch (hoof.Name)
                    {
                        case Types.FRONT_LEFT:
                            graphAction = delegate
                            {
                                chFL.Series["TopLeft"].Points.AddXY(sample.Time, sample.Data[1]);
                                /*chFL.Series["TopRight"].Points.AddXY(sample.Time, sample.Data[3]);
                                chFL.Series["RearLeft"].Points.AddXY(sample.Time, sample.Data[0]);
                                chFL.Series["RearRight"].Points.AddXY(sample.Time, sample.Data[2]);*/
                            };

                            chFL.BeginInvoke(graphAction);
                            break;

                        case Types.FRONT_RIGHT:
                            graphAction = delegate
                            {
                                chFR.Series["TopLeft"].Points.AddXY(sample.Time, sample.Data[1]);
                                /*chFR.Series["TopRight"].Points.AddXY(sample.Time, sample.Data[3]);
                                chFR.Series["RearLeft"].Points.AddXY(sample.Time, sample.Data[0]);
                                chFR.Series["RearRight"].Points.AddXY(sample.Time, sample.Data[2]);*/                               
                            };

                            chFR.BeginInvoke(graphAction);
                            break;
                    }
                }
            
            scaleAction = delegate
            {
                chFR.Update();
                txtScale.Text = ((chFR.ChartAreas["ChartArea1"].AxisX.Maximum - chFR.ChartAreas["ChartArea1"].AxisX.Minimum) / 1000).ToString();
            };

            txtScale.BeginInvoke(scaleAction);            
        }

        /// <summary>
        /// Deal with a response from the hoofs
        /// </summary>
        /// <param name="data">The json description of the response packat</param>
        private void processResponsePacket(string data)
        {
            Types.ResponsePackage answer = JsonConvert.DeserializeObject<Types.ResponsePackage>(data);

            switch (answer.parameter)
            {
                case "status":
                    hoofDetection(answer.hoof);
                    break;
            }
            
            _answer = true;

            if (_console)
                writeLine(data);
        }

        /// <summary>
        /// Set a hoof on active, check if there are doubles
        /// </summary>
        /// <param name="hoofName">The name of the hoof</param>
        /// <returns>True if the hoof is properly detected, false if there's a problem</returns>
        private bool hoofDetection(string hoofName)
        {            
            Hoof hoof;
            int activeHoofs;

            hoof = _hoofList.Find(x => x.Name.Equals(hoofName));

            if (!hoof.Present)
            {
                hoof.setPresent();
                activeHoofs = _hoofList.Count(x => x.Present);
                writeLine(hoofName + " detected [" + activeHoofs + "/" + _hoofList.Count + "]");
                return true;
            }

            writeLine("ERROR: " + hoofName + " already detected!");
            return false;
        }

        private void loadFile(string file)
        {
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load("../../../DataLogs/" + file);

            chFR.ChartAreas["ChartArea1"].AxisX.Minimum = double.NaN;
            chFR.ChartAreas["ChartArea1"].AxisX.Maximum = double.NaN;
            
            foreach (XmlElement xmlElement in xmlDoc.DocumentElement.ChildNodes)
            {
                uint time = uint.Parse(xmlElement.SelectSingleNode("Time").InnerXml);
                string[] data =
                { 
                    xmlElement.SelectSingleNode("RearLeft").InnerText,
                    xmlElement.SelectSingleNode("TopLeft").InnerText,
                    xmlElement.SelectSingleNode("RearRight").InnerText,
                    xmlElement.SelectSingleNode("TopRight").InnerText
                };

                if (xmlElement.SelectSingleNode("Hoof") != null)
                {
                    _hoofList.Find(x => x.Name.Equals(xmlElement.SelectSingleNode("Hoof").InnerXml)).setPresent();
                    _hoofList.Find(x => x.Name.Equals(xmlElement.SelectSingleNode("Hoof").InnerXml)).addSample(new Sample(time, data));
                }
                else
                {
                    _hoofList.Find(x => x.Name.Equals(Types.FRONT_RIGHT)).setPresent();
                    _hoofList.Find(x => x.Name.Equals(Types.FRONT_RIGHT)).addSample(new Sample(time, data));
                }                
            }          
        }

        private void clearGraphs()
        {
            for (int i = 0; i < chFR.Series.Count; i++)
                chFR.Series[i].Points.Clear();

            for (int i = 0; i < chFL.Series.Count; i++)
                chFL.Series[i].Points.Clear();

            for (int i = 0; i < chSteps.Series.Count; i++)
                chSteps.Series[i].Points.Clear();
        }

        private void writeLine(string line)
        {
            MethodInvoker consoleAction = delegate { lsConsole.Items.Insert(0, line); };
            lsConsole.BeginInvoke(consoleAction);
        }

        private void setupCharts()
        {
            chFL.Series.Add("TopLeft");
            chFL.Series.Add("TopRight");
            chFL.Series.Add("RearLeft");
            chFL.Series.Add("RearRight");

            for (int i = 0; i < chFL.Series.Count; i++)
            {
                chFL.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
                chFL.Series[i].BorderWidth = 4;
            }

            chFL.Series["TopLeft"].Color = Color.LightSteelBlue;
            chFL.Series["TopRight"].Color = Color.White;
            chFL.Series["RearLeft"].Color = Color.Orange;
            chFL.Series["RearRight"].Color = Color.Gray;

            chFL.ChartAreas[0].AxisX.LineColor = Color.White;
            chFL.ChartAreas[0].AxisY.LineColor = Color.White;
            chFL.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Transparent;
            chFL.ChartAreas[0].AxisX.MinorGrid.LineColor = Color.Transparent;
            chFL.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Transparent;
            chFL.ChartAreas[0].AxisY.MinorGrid.LineColor = Color.Transparent;
            chFL.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.White;
            chFL.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.White;

            /*****************************************************************************************************************/
            chFR.Series.Add("TopLeft");
            chFR.Series.Add("TopRight");
            chFR.Series.Add("RearLeft");
            chFR.Series.Add("RearRight");

            chFR.Series.Add("Steps");
            chSteps.Series.Add("Filter");

            chFR.Series["TopLeft"].Color = Color.LightSteelBlue;
            chFR.Series["TopRight"].Color = Color.White;
            chFR.Series["RearLeft"].Color = Color.Orange;
            chFR.Series["RearRight"].Color = Color.Gray;
                        
            chFR.Series["Steps"].Color = Color.White;
            chSteps.Series["Filter"].Color = Color.White;

            for (int i = 0; i < chFR.Series.Count; i++)
            {
                chFR.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
                chFR.Series[i].BorderWidth = 4;
            }

            chFR.Series["Steps"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            chSteps.Series["Filter"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            chSteps.Series["Filter"].BorderWidth = 4;

            chFR.ChartAreas[0].AxisX.LineColor = Color.White;
            chFR.ChartAreas[0].AxisY.LineColor = Color.White;
            chFR.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Transparent;
            chFR.ChartAreas[0].AxisX.MinorGrid.LineColor = Color.Transparent;
            chFR.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Transparent;
            chFR.ChartAreas[0].AxisY.MinorGrid.LineColor = Color.Transparent;
            chFR.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.White;
            chFR.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.White;
        }
    }
}
