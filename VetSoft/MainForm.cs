using System;
using System.IO;
using System.Xml;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing;
using Newtonsoft.Json;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;


namespace VetSoft
{
    public partial class frmMain : Form
    {
        private readonly string COMPORT = "COM5";
        private readonly int BAUDRATE = 57600;

        //Flags
        private int _answer;
        private bool _console;

        //Variables
        private int _renderCounter;
        private double _scale;
        private string _fileName;

        private Equine _equine;
        private Serial _serialReader;
        private List<Hoof> _hoofList;
        private DataLogger _dataLogger;

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

            //Charts setup
            setupCharts();          
                     
            //Flags setup
            _renderCounter = 0;
            _answer = -1;
            _console = true;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            pbNoConnect.Visible = false;
            lsConsole.Items.Clear();

            if (connectToSerial(COMPORT, BAUDRATE))
            {
                if (_console)
                    lsConsole.Items.Add("Connected to " + COMPORT + "@" + BAUDRATE + " bauds");

                btnAnalyse.Enabled = false;
                btnLoadFile.Enabled = false;
                btnConnect.Visible = false;
                btnDisconnect.Visible = true;
                btnMeasure.Visible = true;
                btnMeasure.Enabled = true;
                btnSave.Enabled = false;
            }
            else
            {
                if (_console)
                    lsConsole.Items.Add("Error: " + _serialReader.LastError);

                pbNoConnect.Visible = true;
                btnConnect.Visible = true;
                btnMeasure.Enabled = false;
                btnDisconnect.Visible = false;
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

                btnAnalyse.Enabled = true;
                btnLoadFile.Enabled = true;
                btnConnect.Visible = true;
                btnDisconnect.Visible = false;
                btnMeasure.Enabled = false;
                btnSave.Enabled = false;
            }
        }

        private void btnMeasure_Click(object sender, EventArgs e)
        {
            foreach(Hoof hoof in _hoofList)
            {                
                TxFrame frame = new TxFrame(hoof.Address, "{\"type\":0,\"hoof\":0,\"command\":\"update\",\"parameter\":\"transmitRaw\",\"value\":\"true\"}", 0x00);
                _serialReader.send(frame);
                Thread.Sleep(50);                
            }
            
            btnDisconnect.Enabled = false;
            btnMeasure.Visible = false;
            btnIdle.Visible = true;
            btnSave.Enabled = false;
        }

        private void btnIdle_Click(object sender, EventArgs e)
        {
            foreach (Hoof hoof in _hoofList)
            {
                _answer = -1;
                
                TxFrame frame = new TxFrame(hoof.Address, "{\"type\":0,\"hoof\":0,\"command\":\"update\",\"parameter\":\"transmitRaw\",\"value\":\"false\"}", 0x00);
                _serialReader.send(frame);

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
                clearCharts();

                foreach (Hoof hoof in _hoofList)
                {
                    hoof.setPresent(false);
                    hoof.Clear();
                }

                loadFile(selectedValue.Split('●')[1].TrimStart(' '));
                renderHoofList();
            }           
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (Hoof hoof in _hoofList)
                foreach (Sample sample in hoof.SampleList)
                    _dataLogger.WriteSample(sample, hoof.HoofLocation);

            _dataLogger.SaveToFile(Types.RandomString(6) + ".xml");
        }

        /// <summary>
        /// Analysis button
        /// </summary>
        private void btnAnalyse_Click(object sender, EventArgs e)
        {
            _equine = new Equine(_hoofList);
            _equine.process();

            foreach(Hoof hoof in _hoofList)
            {
                MethodInvoker graphAction;
                Chart chartPlaceHolder = getChart(hoof.HoofLocation);

                if (chartPlaceHolder != null)
                {
                    writeLine(hoof.Steps + " steps analysed at " + hoof.HoofLocation);
                    
                    //Plot sensor step outline
                    foreach(Sensor sensor in hoof.SensorList)
                    {
                        string seriesName = getChartSeries(sensor.SensorLocation);

                        graphAction = delegate
                        {
                            foreach (ForcePoint forcePoint in sensor.StepStream)
                                chartPlaceHolder.Series[seriesName].Points.AddXY(forcePoint.TimeStamp, forcePoint.ForceValue);
                        };

                        chartPlaceHolder.BeginInvoke(graphAction);
                    }

                    //Plot general step outline
                    graphAction = delegate
                    {
                        foreach (ForcePoint forcePoint in hoof.StepStream)
                            chartPlaceHolder.Series["GeneralStep"].Points.AddXY(forcePoint.TimeStamp, forcePoint.ForceValue);
                    };
                    
                    chartPlaceHolder.BeginInvoke(graphAction);
                }
            }
        }

        private void pnScrollLeft_MouseClick(object sender, MouseEventArgs e)
        {
            if (chFR.ChartAreas[0].AxisX.Minimum > 999)
            {
                chFR.ChartAreas[0].AxisX.Minimum -= 1000;
                chFR.ChartAreas[0].AxisX.Maximum -= 1000;
                chFL.ChartAreas[0].AxisX.Minimum -= 1000;
                chFL.ChartAreas[0].AxisX.Maximum -= 1000;
                chFR.Update();
                chFL.Update();
            }
        }
        
        private void pnScrollRight_MouseClick(object sender, MouseEventArgs e)
        {
            chFR.ChartAreas[0].AxisX.Minimum += 3000;
            chFR.ChartAreas[0].AxisX.Maximum += 3000;
            chFL.ChartAreas[0].AxisX.Minimum += 3000;
            chFL.ChartAreas[0].AxisX.Maximum += 3000;
            
            chFL.ChartAreas[0].AxisY.Maximum = double.NaN;
            chFR.Update();
            chFL.Update();
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            _scale = 0.8 * (chFR.ChartAreas["ChartArea1"].AxisX.Maximum - chFR.ChartAreas["ChartArea1"].AxisX.Minimum);
            updateScale();
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            _scale = 1.2 * (chFR.ChartAreas["ChartArea1"].AxisX.Maximum - chFR.ChartAreas["ChartArea1"].AxisX.Minimum);
            updateScale();
        }

        private void cbTopLeft_CheckedChanged(object sender, EventArgs e)
        {

            chFR.Series["TopLeft"].Color = Color.Transparent;
            chFR.Series["TopLeftStep"].Color = Color.Transparent;
            chFL.Series["TopLeft"].Color = Color.Transparent;
            chFL.Series["TopLeftStep"].Color = Color.Transparent;

            if (cbTopLeft.Checked)
            {
                chFR.Series["TopLeft"].Color = Color.LightSteelBlue;
                chFR.Series["TopLeftStep"].Color = Color.BlueViolet;
                chFL.Series["TopLeft"].Color = Color.LightSteelBlue;
                chFL.Series["TopLeftStep"].Color = Color.BlueViolet;
            }
        }

        private void cbRearLeft_CheckedChanged(object sender, EventArgs e)
        {
            chFR.Series["RearLeft"].Color = Color.Transparent;
            chFR.Series["RearLeftStep"].Color = Color.Transparent;
            chFL.Series["RearLeft"].Color = Color.Transparent;
            chFL.Series["RearLeftStep"].Color = Color.Transparent;

            if (cbRearLeft.Checked)
            {
                chFR.Series["RearLeft"].Color = Color.Orange;
                chFR.Series["RearLeftStep"].Color = Color.OrangeRed;
                chFL.Series["RearLeft"].Color = Color.Orange;
                chFL.Series["RearLeftStep"].Color = Color.OrangeRed;
            }
        }

        private void cbTopRight_CheckedChanged(object sender, EventArgs e)
        {
            chFR.Series["TopRight"].Color = Color.Transparent;
            chFR.Series["TopRightStep"].Color = Color.Transparent;
            chFL.Series["TopRight"].Color = Color.Transparent;
            chFL.Series["TopRightStep"].Color = Color.Transparent;

            if (cbTopRight.Checked)
            {
                chFR.Series["TopRight"].Color = Color.White;
                chFR.Series["TopRightStep"].Color = Color.NavajoWhite;
                chFL.Series["TopRight"].Color = Color.White;
                chFL.Series["TopRightStep"].Color = Color.NavajoWhite;
            }
        }

        private void cbRearRight_CheckedChanged(object sender, EventArgs e)
        {
            chFR.Series["RearRight"].Color = Color.Transparent;
            chFR.Series["RearRightStep"].Color = Color.Transparent;
            chFL.Series["RearRight"].Color = Color.Transparent;
            chFL.Series["RearRightStep"].Color = Color.Transparent;

            if (cbRearRight.Checked)
            {
                chFR.Series["RearRight"].Color = Color.Gray;
                chFR.Series["RearRightStep"].Color = Color.GreenYellow;
                chFL.Series["RearRight"].Color = Color.Gray;
                chFL.Series["RearRightStep"].Color = Color.GreenYellow;
            }
        }

        /// <summary>
        /// Update the scale of the graphs in the GUI
        /// </summary>
        private void updateScale()
        {
            chFR.ChartAreas[0].AxisX.Maximum = chFR.ChartAreas[0].AxisX.Minimum + _scale;
            chFL.ChartAreas[0].AxisX.Maximum = chFL.ChartAreas[0].AxisX.Minimum + _scale;

            chFR.Update();
            chFL.Update();
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
                for (int i = 0; i < 4; i++)
                {
                    TxFrame frame = new TxFrame(i,"{\"type\":0,\"command\":\"read\",\"parameter\":status}", 0x00);
                    _serialReader.send(frame);

                    Thread.Sleep(50);
                }

                return true;
            }
            return false;
        }

        /// <summary>
        /// Deal with events from Serial line
        /// </summary>
        /// <param name="data">Data coming from the serial Line</param>
        private void recievedData(Frame frame)
        {
            switch(frame.FrameType)
            {
                case Types.STATUS_FRAME:
                    //writeLine("Status ok");
                    break;

                case Types.RxFRAME_16BIT:
                    RxFrame rxFrame = (RxFrame) frame.GetFrame();
                    JObject objJson = JObject.Parse(rxFrame.Message);
                    int addressSender = Convert.ToInt32(rxFrame.Address[0]) + Convert.ToInt32(rxFrame.Address[1]);

                    switch ((int) objJson.GetValue("type"))
                    {
                        case Types.RESPONSE_PACKAGE_TYPE:
                            processResponsePacket(rxFrame.Message, addressSender);
                            break;

                        case Types.DATA_PACKAGE_TYPE:
                            processDataPacket(rxFrame.Message, addressSender);
                            break;
                    }
                    break;

                default:
                    writeLine("Unknown Packet type");
                    break;
            }

        }

        /// <summary>
        /// Assigns the data packet to the correct hoof in memory.
        /// </summary>
        /// <param name="data">The json description of the data packet</param>
        private void processDataPacket(string data, int address)
        {
            Types.DataPackage dataPackage = JsonConvert.DeserializeObject<Types.DataPackage>(data);
            Hoof hoof = _hoofList.Find(x => x.Address.Equals(address));

            foreach(Types.DataContent dataContent in dataPackage.data)
            {
                hoof.addSample(new Sample(dataContent.time, dataContent.forcePoint));                
                updateCharts(hoof.HoofLocation, new Sample(dataContent.time, dataContent.forcePoint));
            }

            renderChart(getChart(hoof.HoofLocation));
            

            //if (_console)
            //    writeLine(data);           
        }

        private void renderHoofList()
        {
            foreach (Hoof hoof in _hoofList.Where(x => x.Present))
            {
                foreach (Sample sample in hoof.SampleList)
                    updateCharts(hoof.HoofLocation, sample);

                renderChart(getChart(hoof.HoofLocation));
            }            
        }

        /// <summary>
        /// Update the charts with a data sample.
        /// </summary>
        private void updateCharts(Types.HoofLocation hoofLocation, Sample sample)
        {            
            Chart chartPlaceHolder = getChart(hoofLocation);

            MethodInvoker graphAction = delegate
            {
                chartPlaceHolder.Series["TopLeft"].Points.AddXY(sample.Time, sample.Data[1]);
                chartPlaceHolder.Series["TopRight"].Points.AddXY(sample.Time, sample.Data[3]);
                chartPlaceHolder.Series["RearLeft"].Points.AddXY(sample.Time, sample.Data[0]);
                chartPlaceHolder.Series["RearRight"].Points.AddXY(sample.Time, sample.Data[2]);
            };

            chartPlaceHolder.BeginInvoke(graphAction);
        }

        /// <summary>
        /// Render the charts
        /// </summary>
        private void renderChart(Chart chartPlaceHolder)
        {
            MethodInvoker graphAction = delegate
            {
                chartPlaceHolder.Update();
                _scale = chartPlaceHolder.ChartAreas["ChartArea1"].AxisX.Maximum - chartPlaceHolder.ChartAreas["ChartArea1"].AxisX.Minimum;
            };

            chartPlaceHolder.BeginInvoke(graphAction);
        }

        /// <summary>
        /// Deal with a response from the hoofs
        /// </summary>
        /// <param name="data">The json description of the response packet</param>
        private void processResponsePacket(string data, int address)
        {
            Types.ResponsePackage answer = JsonConvert.DeserializeObject<Types.ResponsePackage>(data);

            switch (answer.parameter)
            {
                case "status":
                    hoofDetection(answer.hoofLocation, address);
                    break;
            }
            
            _answer = (int) answer.hoofLocation;

            if (_console)
                writeLine(data);
        }

        /// <summary>
        /// Set a hoof on active, check if there are doubles
        /// </summary>
        /// <param name="hoofName">The name of the hoof</param>
        /// <returns>True if the hoof is properly detected, false if there's a problem</returns>
        private bool hoofDetection(Types.HoofLocation hooflocation, int address)
        {
            Hoof hoof = new Hoof(hooflocation, address);

            _hoofList.Add(hoof);

            writeLine(hooflocation + " detected [" + _hoofList.Count + "/" + _hoofList.Count + "]");
            return true;
        }

        /// <summary>
        /// Load a XmlFile from the hard drive and store the data in a hoof object
        /// </summary>
        /// <param name="file">The name off the xml file</param>
        private void loadFile(string file)
        {
            _hoofList.Add(new Hoof(Types.HoofLocation.FRONT_LEFT, 0));
            _hoofList.Add(new Hoof(Types.HoofLocation.FRONT_RIGHT, 0));
            _hoofList.Add(new Hoof(Types.HoofLocation.HIND_LEFT, 0));
            _hoofList.Add(new Hoof(Types.HoofLocation.HIND_RIGHT, 0));

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
                    _hoofList.Find(x => x.HoofLocation.Equals((Types.HoofLocation) int.Parse(xmlElement.SelectSingleNode("Hoof").InnerXml))).setPresent();
                    _hoofList.Find(x => x.HoofLocation.Equals((Types.HoofLocation) int.Parse(xmlElement.SelectSingleNode("Hoof").InnerXml))).addSample(new Sample(time, data));
                }
                else
                {
                    _hoofList.Find(x => x.HoofLocation.Equals(Types.HoofLocation.FRONT_RIGHT)).setPresent();
                    _hoofList.Find(x => x.HoofLocation.Equals(Types.HoofLocation.FRONT_RIGHT)).addSample(new Sample(time, data));
                }                
            }          
        }

        /// <summary>
        /// Clear all the graphs in the GUI from data
        /// </summary>
        private void clearCharts()
        {
            for (int i = 0; i < chFR.Series.Count; i++)
                chFR.Series[i].Points.Clear();

            for (int i = 0; i < chFL.Series.Count; i++)
                chFL.Series[i].Points.Clear();
        }

        /// <summary>
        /// Write a line in the console window in the GUI
        /// </summary>
        /// <param name="line">The line to write to the console</param>
        private void writeLine(string line)
        {
            MethodInvoker consoleAction = delegate { lsConsole.Items.Insert(0, line); };
            lsConsole.BeginInvoke(consoleAction);
        }

        private Chart getChart(Types.HoofLocation hoofLocation)
        {
            Chart retVal = null;

            switch (hoofLocation)
            {
                case Types.HoofLocation.FRONT_LEFT:
                    retVal = chFL;
                    break;

                case Types.HoofLocation.FRONT_RIGHT:
                    retVal = chFR;
                    break;
            }

            return retVal;
        }

        private string getChartSeries(Types.SensorLocation sensorLocation)
        {
            string retVal = "";

            switch (sensorLocation)
            {
                case Types.SensorLocation.REAR_LEFT:
                    retVal = "RearLeftStep";
                    break;

                case Types.SensorLocation.REAR_RIGHT:
                    retVal = "RearRightStep";
                    break;

                case Types.SensorLocation.TOP_LEFT:
                    retVal = "TopLeftStep";
                    break;

                case Types.SensorLocation.TOP_RIGHT:
                    retVal = "TopRightStep";
                    break;
            }

            return retVal;
        }

        private void setupCharts()
        {
            chFR.Series.Add("TopLeft");
            chFR.Series.Add("TopLeftStep");

            chFR.Series.Add("TopRight");
            chFR.Series.Add("TopRightStep");

            chFR.Series.Add("RearLeft");
            chFR.Series.Add("RearLeftStep");

            chFR.Series.Add("RearRight");
            chFR.Series.Add("RearRightStep");

            chFR.Series.Add("GeneralStep");

            for (int i = 0; i < chFR.Series.Count; i++)
            {
                chFR.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
                chFR.Series[i].BorderWidth = 4;
            }

            chFR.Series["TopLeft"].Color = Color.LightSteelBlue;
            chFR.Series["TopLeftStep"].Color = Color.BlueViolet;

            chFR.Series["TopRight"].Color = Color.White;
            chFR.Series["TopRightStep"].Color = Color.NavajoWhite;

            chFR.Series["RearLeft"].Color = Color.Orange;
            chFR.Series["RearLeftStep"].Color = Color.OrangeRed;

            chFR.Series["RearRight"].Color = Color.Gray;
            chFR.Series["RearRightStep"].Color = Color.YellowGreen;

            chFR.Series["GeneralStep"].Color = Color.DarkSlateGray;

            chFR.ChartAreas[0].AxisX.LineColor = Color.White;
            chFR.ChartAreas[0].AxisY.LineColor = Color.White;
            chFR.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Transparent;
            chFR.ChartAreas[0].AxisX.MinorGrid.LineColor = Color.Transparent;
            chFR.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Transparent;
            chFR.ChartAreas[0].AxisY.MinorGrid.LineColor = Color.Transparent;
            chFR.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.White;
            chFR.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.White;

            /*****************************************************************************************************************/
            chFL.Series.Add("TopLeft");
            chFL.Series.Add("TopLeftStep");

            chFL.Series.Add("TopRight");
            chFL.Series.Add("TopRightStep");

            chFL.Series.Add("RearLeft");
            chFL.Series.Add("RearLeftStep");

            chFL.Series.Add("RearRight");
            chFL.Series.Add("RearRightStep");

            chFL.Series.Add("GeneralStep");

            for (int i = 0; i < chFL.Series.Count; i++)
            {
                chFL.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
                chFL.Series[i].BorderWidth = 4;
            }

            chFL.Series["TopLeft"].Color = Color.LightSteelBlue;
            chFL.Series["TopLeftStep"].Color = Color.BlueViolet;

            chFL.Series["TopRight"].Color = Color.White;
            chFL.Series["TopRightStep"].Color = Color.NavajoWhite;

            chFL.Series["RearLeft"].Color = Color.Orange;
            chFL.Series["RearLeftStep"].Color = Color.OrangeRed;

            chFL.Series["RearRight"].Color = Color.Gray;
            chFL.Series["RearRightStep"].Color = Color.YellowGreen;

            chFL.Series["GeneralStep"].Color = Color.DarkSlateGray;

            chFL.ChartAreas[0].AxisX.LineColor = Color.White;
            chFL.ChartAreas[0].AxisY.LineColor = Color.White;
            chFL.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Transparent;
            chFL.ChartAreas[0].AxisX.MinorGrid.LineColor = Color.Transparent;
            chFL.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Transparent;
            chFL.ChartAreas[0].AxisY.MinorGrid.LineColor = Color.Transparent;
            chFL.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.White;
            chFL.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.White;
        }
    }
}