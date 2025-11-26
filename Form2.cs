using Microsoft.ML;
using Microsoft.ML.Data;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using SM.EF;
using SM.Map;
using SM.Map.G;
using System.Collections.Generic;
using System.Timers;
using System.Reflection;
using System.ComponentModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
namespace UrbanHeatIsland
{
    public partial class Form2 : Form
    {
        static bool IsPeriodChanged, IsCalendarEntry, IsComparing, IsChartLoaded, IsSecondChartLoaded, IsFormLoaded, IsReady = true, IsInfoRequest, IsStartPeriod, IsStartSelected, IsFinishSelected;
        DateTime selectedDate, finishDateLast, startDateLast;
        Graphics cGraphics;
        List<DateTime> dates = new List<DateTime>();
        List<float> temperatures = new List<float>();
        List<float> temperatures2 = new List<float>();
        float temperatureExtreme = float.MinValue;
        float pressureExtreme = float.MinValue;
        float humidityExtreme = float.MinValue;
        float temperature2Extreme = float.MinValue;
        float pressure2Extreme = float.MinValue;
        float humidity2Extreme = float.MinValue;
        float temperatureForecast, temperature2Forecast, pressureForecast, pressure2Forecast, humidityForecast, humidity2Forecast;
        static string cFileName = string.Empty;
        static string cFileName0 = string.Empty;
        static bool IsFirstOpen;
        List<Coordinate> markers = new List<Coordinate>();
        //FrmOpticMap frmOpticMap = new FrmOpticMap();
        SM.EF.Controls.MapCtl mapCtl = new SM.EF.Controls.MapCtl();
        bool IsTap, IsMarkerMode;
        Point lastTap;
        Coordinate lastTapCoordinate;
        SortedDictionary<string, List<string>> citiesData;
        IEnumerable<Input> ieClimateData = new List<Input>();
        bool IsDataSetFirstReady;
        string startDate, finishDate;
        private void GetInfoFromNASAServer(string args)
        {
            //string apiUrlMain = "https://power.larc.nasa.gov/api/temporal/daily/point?parameters=T2M,PRECTOT,RH2M&community=RE&longitude=37.53&latitude=58.84&start=19920101&end=20250920&format=CSV"; // Example API URL
            string apiUrlMain = "https://power.larc.nasa.gov/api/temporal/daily/point?parameters=T2M,PRECTOT,RH2M&community=RE&" + args + "&format=CSV"; // Example API URL
            //try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.GetAsync(apiUrlMain).Result;
                    response.EnsureSuccessStatusCode(); // Throw if not successful
                    string responseData = response.Content.ReadAsStringAsync().Result;
                    ReportMake(responseData);
                }
            }
            /*catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error");
            }*/

        }
        async Task GetInfo()
        {
            IsPeriodChanged = false;
            await Task.Run(() =>
            {
                IsChartLoaded = false;
                IsCrear = true;
                if (IsStartSelected && IsFinishSelected && markers.Count > 0)
                {
                    //string[] startDate = maskedTextBox1.Text.Split('/');
                    //string[] finishDate = maskedTextBox2.Text.Split('/');
                    int selectedMarkerId = 0;
                    if (markers.Count > 1)
                        IsReady = false;
                    if (!IsDataSetFirstReady || markers.Count == 1)
                    {
                        selectedMarkerId = 0;
                        temperatures.Clear();
                        //IsReady = false;
                        GetInfoFromNASAServer("longitude=" + markers[selectedMarkerId].Longitude.ToString().Replace(',', '.') + "&latitude=" + markers[selectedMarkerId].Latitude.ToString().Replace(',', '.') + "&start=" + startDate + "&end=" + finishDate);
                        Invalidate();
                        IsDataSetFirstReady = true;
                        IsComparing = true;
                    }
                    if (markers.Count > 1)
                    {
                        selectedMarkerId = 1;
                        IsSecondChartLoaded = false;
                        temperatures2.Clear();
                        GetInfoFromNASAServer("longitude=" + markers[selectedMarkerId].Longitude.ToString().Replace(',', '.') + "&latitude=" + markers[selectedMarkerId].Latitude.ToString().Replace(',', '.') + "&start=" + startDate + "&end=" + finishDate);
                        IsSecondChartLoaded = true;
                        Invalidate();
                    }
                }
            });
        }
        
        float Forecast(string forecastingMeasure)
        {
            var context = new MLContext(seed: 0);
            // Load the data
            var data = context.Data.LoadFromEnumerable(ieClimateData);
            // Split the data into a training set and a test set
            // One-hot encode the values in the "UseCode" column and train the model
            var pipeline = context.Transforms.Categorical.OneHotEncoding(inputColumnName: "UseCode", outputColumnName: "UseCodeEncoded")
                //.Append(context.Transforms.Concatenate("Features", "UseCodeEncoded", "YEAR", "MO", "DY", "T2MU", "RH2MU", "PRECTOTU", "T2MR", "RH2MR", "PRECTOTR"))
                .Append(context.Transforms.Concatenate("Features", "UseCodeEncoded", "YEAR", "MO", "DY", "T2MU", "T2MR"))
                .Append(context.Regression.Trainers.FastForest(labelColumnName: forecastingMeasure, numberOfTrees: 200, minimumExampleCountPerLeaf: 4));
            var model = pipeline.Fit(data);
            // Use the model to make a prediction
            var predictor = context.Model.CreatePredictionEngine<Input, Output>(model);
            var input = new Input
            {
                YEAR = selectedDate.Year,
                MO = selectedDate.Month,
                DY = selectedDate.Day,
                UseCode = " ",
            };
            var prediction = predictor.Predict(input);
            float forecastedMeasure = (float)Math.Round(prediction.Score, 1);
            return forecastedMeasure;
        }
        void ReportMake(string Content)
        {
            List<Input> listClimateData = new List<Input>(ieClimateData.ToList());
            if (!IsDataSetFirstReady)
            {
                temperatures.Clear();
                dates.Clear();
            }
            else
                temperatures2.Clear();
            string[] lines = Content.Split('\n');
            bool IsStart = false;
            int pointsCount = 0;
            bool IsT2MFirst = false;
            foreach (string line in lines)
            {
                if (line == string.Empty)
                    continue;
                //if (!IsStart && line.Contains("YEAR,MO,DY,T2M,RH2M,PRECTOTCORR"))  //YEAR,MO,DY,RH2M,T2M,PRECTOTCORR
                if (!IsStart && (line.Contains("YEAR,MO,DY,RH2M,T2M,PRECTOTCORR") || line.Contains("YEAR,MO,DY,T2M,RH2M,PRECTOTCORR")))  // I don`t understand why), but sometimes, there were two  variants of header, may be I`m not right 
                {
                    if (line.Contains("YEAR,MO,DY,T2M,RH2M,PRECTOTCORR"))
                        IsT2MFirst = true;
                    IsStart = true;
                    continue;
                }
                if (!IsStart) // skip header
                    continue;
                string[] lv = line.Split(',');
                float v1, v2, v3;
                if (IsT2MFirst)
                {
                    v1 = float.Parse(lv[3].Replace('.', ','));
                    v2 = float.Parse(lv[4].Replace('.', ','));
                }
                else
                {
                    v2 = float.Parse(lv[3].Replace('.', ','));
                    v1 = float.Parse(lv[4].Replace('.', ','));
                }
                v3 = float.Parse(lv[5].Replace('.', ','));
                if (!IsDataSetFirstReady)
                {
                    DateTime date = new DateTime(int.Parse(lv[0]), int.Parse(lv[1]), int.Parse(lv[2]));
                    dates.Add(date);
                    var dataSet = new Input
                    {
                        YEAR = date.Year,
                        MO = date.Month,
                        DY = date.Day,
                        T2MR = v1,
                        PRECTOTR = v2,
                        RH2MR = v3,
                        UseCode = " ",
                    };
                    listClimateData.Add(dataSet);
                    temperatures.Add(v1);
                }
                else
                {
                    if (dates[pointsCount].Year == int.Parse(lv[0]) && dates[pointsCount].Month == int.Parse(lv[1]) && dates[pointsCount].Day == int.Parse(lv[2]))
                        temperatures2.Add(v1);
                    if (listClimateData.Count > 0 && dates[pointsCount].Year == listClimateData[pointsCount].YEAR && dates[pointsCount].Month == listClimateData[pointsCount].MO && dates[pointsCount].Day == listClimateData[pointsCount].DY)
                    {
                        listClimateData[pointsCount].T2MU = v1;
                        listClimateData[pointsCount].PRECTOTU = v2;
                        listClimateData[pointsCount].RH2MU = v3;
                    }
                }
                pointsCount++;
                if (IsDataSetFirstReady)
                {
                    if (v1 > temperature2Extreme)
                        temperature2Extreme = v1;
                    if (v2 > pressure2Extreme)
                        pressure2Extreme = v2;
                    if (v3 > humidity2Extreme)
                        humidity2Extreme = v3;
                }
                else
                {
                    if (v1 > temperatureExtreme)
                        temperatureExtreme = v1;
                    if (v2 > pressureExtreme)
                        pressureExtreme = v2;
                    if (v3 > humidityExtreme)
                        humidityExtreme = v3;
                }
            }
            if (IsDataSetFirstReady)
            {
                /*
                label1.Text = Forecast("T2MU").ToString();
                label2.Text = Forecast("PRECTOTU").ToString();
                label13.Text = Forecast("RH2MU").ToString();
                label6.Text = temperature2Extreme.ToString();
                label5.Text = pressure2Extreme.ToString();
                label15.Text = humidity2Extreme.ToString();
                */
                temperature2Forecast = Forecast("T2MU");
                pressure2Forecast = Forecast("PRECTOTU");
                humidity2Forecast = Forecast("RH2MU");
                //groupBox1?.Invoke(() => { groupBox1.Visible = true; });
                //groupBox2?.Invoke(() => { groupBox2.Visible = true; });
                if (temperatures2.Count > 0)
                    IsChartLoaded = true;
            }
            else
            {
                ieClimateData = new List<Input>(listClimateData);/*
                label3.Text = Forecast("T2MR").ToString();
                label4.Text = Forecast("PRECTOTR").ToString();
                label14.Text = Forecast("RH2MR").ToString();
                label8.Text = temperatureExtreme.ToString();
                label7.Text = pressureExtreme.ToString();
                label16.Text = humidityExtreme.ToString();*/
                temperatureForecast = Forecast("T2MR");
                pressureForecast = Forecast("PRECTOTR");
                humidityForecast = Forecast("RH2MR");
                if (temperatures.Count > 0)
                    IsChartLoaded = true;
            }

        }
        System.Timers.Timer invalidateTimer = new System.Timers.Timer();
        void MapInvalidate(object sender, ElapsedEventArgs e)
        {
            Invalidate();
            comboBox1.Invoke(() => { comboBox1.Focus(); });
            monthCalendar1.Invoke(() => monthCalendar1.Visible = false);
        }
        static int b = 3;
        public Form2()
        {
            InitializeComponent();
            mapCtl.Width = 310;
            mapCtl.Height = 310;
            LoadCities();
            //Thread drawThread = new Thread(MapInvalidate);
            //drawThread.Start();
            //mapCtl.Invalidate();
            //mapCtl.Validated += new EventHandler(MapInvalidate);

            invalidateTimer.Elapsed += MapInvalidate;
            invalidateTimer.AutoReset = false;
            invalidateTimer.Interval = 2000;
            invalidateTimer.Start();
            //frmOpticMap.Activate();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            progressBar1.Visible = true;
            backgroundWorker1.RunWorkerAsync();
        }
        void LoadCities()
        {
            if (File.Exists("WorldCities.csv"))
            {
                List<int> selectedColumns = new List<int>();
                selectedColumns.Add(2);
                selectedColumns.Add(3);
                citiesData = DataLoadFromFile("WorldCities.csv", 1, selectedColumns);
                comboBox1.Items.AddRange(citiesData.Keys.ToArray());
            }
        }
        SortedDictionary<string, List<string>> DataLoadFromFile(string fileName, int keyColumnId, List<int> selectedColumnsId)
        {
            SortedDictionary<string, List<string>> dataSets = new SortedDictionary<string, List<string>>();
            using (StreamReader sr = new StreamReader(fileName))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] values = line.Split(',');
                    List<string> dataSet = new List<string>();
                    foreach (int id in selectedColumnsId)
                        dataSet.Add(values[id]);
                    string city = values[keyColumnId].Replace("\"", "");
                    if (!dataSets.ContainsKey(city))
                        dataSets.Add(city, dataSet);
                }
            }
            return dataSets;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            IsFormLoaded = true;
            /*comboBox1.Focus();
            monthCalendar1.Visible = false;*/
        }
        public class Input
        {
            [LoadColumn(0)]
            public float YEAR;

            [LoadColumn(1)]
            public float MO;

            [LoadColumn(2)]
            public float DY;

            [LoadColumn(3)]
            public float T2MR;

            [LoadColumn(4)]
            public float T2MU;

            [LoadColumn(5)]
            public float PRECTOTR;

            [LoadColumn(6)]
            public float PRECTOTU;

            [LoadColumn(7)]
            public float RH2MR;

            [LoadColumn(8)]
            public float RH2MU;

            [LoadColumn(9)]
            public string UseCode;
        }
        public class Output
        {
            [ColumnName("Score")]
            public float Score;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            backgroundWorker1.CancelAsync();
            backgroundWorker1.Dispose();
            IsFormLoaded = false;
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            IsPeriodChanged = false;
            IsCalendarEntry = true;
            if (IsStartPeriod)
            {
                startDate = e.Start.ToString("yyyyMMdd");
                maskedTextBox1.Text = e.Start.ToString("MM/dd/yyyy").Replace('.', '/');
                IsStartSelected = true;
                if (startDateLast != e.Start)
                    IsPeriodChanged = true;
                startDateLast = e.Start;
            }
            else
            {
                selectedDate = e.Start;
                finishDate = e.Start.ToString("yyyyMMdd");
                maskedTextBox2.Text = e.Start.ToString("MM/dd/yyyy").Replace('.', '/');
                IsFinishSelected = true;
                if (finishDateLast != e.Start)
                    IsPeriodChanged = true;
                finishDateLast = e.Start;
            }
            /*
            if (IsStartPeriod)
                IsStartSelected = true;
            else
                IsFinishSelected = true;
            */

            /*
            if (!IsCompared)
            {
                label1.Text = Forecast(e.End).ToString();
                label2.Text = Forecast2(e.End).ToString();
            }*/
        }
        void ChartDraw(int ChartId)
        {
            Graphics g = CreateGraphics();
            if (ChartId == 1)
            {
                ChartDraw(g, temperatures, Color.DarkBlue);
                label3.Text = temperatureForecast.ToString();
                label4.Text = pressureForecast.ToString();
                label14.Text = humidityForecast.ToString();
                label8.Text = temperatureExtreme.ToString();
                label7.Text = pressureExtreme.ToString();
                label16.Text = humidityExtreme.ToString();
            }
            else if (ChartId == 2)
            {
                ChartDraw(g, temperatures2, Color.Red);
                label1.Text = temperature2Forecast.ToString();
                label2.Text = pressure2Forecast.ToString();
                label13.Text = humidity2Forecast.ToString();
                label6.Text = temperature2Extreme.ToString();
                label5.Text = pressure2Extreme.ToString();
                label15.Text = humidity2Extreme.ToString();
                groupBox1.Visible = true;
                groupBox2.Visible = true;
                groupBox4.Visible = true;
            }
        }
        void ChartDraw(List<float> temperatures, Color ChartColor)
        {
            Graphics g = CreateGraphics();
            ChartDraw(g, temperatures, ChartColor);
        }
        int YAxis = 365;
        void ChartDraw(Graphics g, List<float> temperatures, Color ChartColor)
        {
            g.DrawLine(new Pen(new SolidBrush(Color.Black), 1), new PointF(YAxis, 0), new PointF(YAxis, 1200));
            g.DrawLine(new Pen(new SolidBrush(Color.Black), 1), new PointF(YAxis, 500), new PointF(1750, 500));
            for (int i = 0; i < 10; i++)
            {
                g.DrawString(((5 - i) * 10).ToString(), new System.Drawing.Font("Calibry", 12), new SolidBrush(Color.Black), YAxis, i * 100);
                g.DrawLine(new Pen(new SolidBrush(Color.Black), 1), new PointF(YAxis - 3, i * 100), new PointF(YAxis + 3, i * 100));
            }
            float interval = 1500f / ((dates.Last() - dates.First()).Days);
            for (int i = 0; i < temperatures.Count - 1; i++)
            {
                float X = (dates[i] - dates.First()).Days * interval;
                float nextX = (dates[i + 1] - dates.First()).Days * interval;
                g.DrawEllipse(new Pen(new SolidBrush(ChartColor), 3), YAxis + X, -temperatures[i] * 10 + 500, 2, 2);
                g.DrawLine(new Pen(new SolidBrush(ChartColor), 1), new PointF(YAxis + X, -temperatures[i] * 10 + 500), new PointF(YAxis + nextX, -temperatures[i + 1] * 10 + 500));
            }
        }
        bool IsCrear;
        private void Form1_Paint(object sender, PaintEventArgs e)
        {/*
            if (IsCrear)
            {
                e.Graphics.FillRectangle(new SolidBrush(Form2.DefaultBackColor), YAxis, 0, 2000, 2000);
                IsCrear = false;
            }
            */
            if (IsChartLoaded)
            {
                //cGraphics = e.Graphics;
                e.Graphics.FillRectangle(new SolidBrush(Form2.DefaultBackColor), YAxis, 0, 2000, 2000);
                //ChartDraw(temperatures, Color.DarkBlue);
                ChartDraw(1);
                if (IsComparing && IsSecondChartLoaded)
                {
                    //ChartDraw(temperatures2, Color.Red);
                    ChartDraw(2);
                    IsReady = true;
                }
            }
            //DoubleBuffered = true;
            //e.Graphics.Clip = new Region(new Rectangle(10, 10, 200, 200));
            mapCtl.Dr(e);
            MarkersDraw(e.Graphics);
            //e.Graphics.FillRectangle(new SolidBrush(Color.Green), 10, 10, 200, 200);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mapCtl.Level++;
            invalidateTimer.Interval = 500;
            invalidateTimer.Start();
            /*
            if (!IsFirstOpen)
                CSVOpen();
            else
            {
                IsCompared = true;
                CSVOpen();
                ChartDraw(temperatures2, Color.Blue);
            }*/


        }


        void MarkersDraw(Graphics g)
        {
            bool IsFirstMarker = true;
            foreach (Coordinate markerPoint in markers)
            {
                float X = GMapUtilities.GetGX(markerPoint, mapCtl.Level) - GMapUtilities.GetGX(mapCtl.CenterCoordinate, mapCtl.Level) + mapCtl.Width / 2;
                float Y = GMapUtilities.GetGY(markerPoint, mapCtl.Level) - GMapUtilities.GetGY(mapCtl.CenterCoordinate, mapCtl.Level) + mapCtl.Height / 2;
                if (X < mapCtl.Width && Y < mapCtl.Height)
                {
                    if (IsFirstMarker)
                        g.DrawEllipse(new Pen(new SolidBrush(Color.DarkBlue), 3), X, Y, 3, 3);
                    else
                        g.DrawEllipse(new Pen(new SolidBrush(Color.Red), 3), X, Y, 3, 3);
                    IsFirstMarker = false;
                }
            }
        }

        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.X < mapCtl.Width && e.Y < mapCtl.Height)
            {
                if (IsMarkerMode)
                {
                    var deltaX = e.X - mapCtl.Width / 2; // distance from map center
                    var deltaY = e.Y - mapCtl.Height / 2;
                    Coordinate markerPoint = mapCtl.CenterCoordinate + new GCoordinate(deltaX, deltaY, mapCtl.Level);
                    markers.Add(markerPoint);
                    //if (markers.Count == 2)
                    //IsCompared = true;
                    if (markers.Count == 3) // only two markers to compare, last one is deleted
                        markers.RemoveAt(1);
                    Invalidate();
                    GetInfo();
                }
                else
                {
                    IsTap = true;
                    lastTapCoordinate = mapCtl.CenterCoordinate;
                    lastTap = e.Location;
                }
            }/*
            if (IsCalendarEntry && !IsStartPeriod)
                maskedTextBox2.Focus();*/
            monthCalendar1.Visible = false;
            IsCalendarEntry = false;
        }

        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsTap)
            {
                var deltaX = lastTap.X - e.X;
                var deltaY = lastTap.Y - e.Y;
                mapCtl.CenterCoordinate = lastTapCoordinate + new GCoordinate(deltaX, deltaY, mapCtl.Level);
                Invalidate();
            }
            //Invalidate();

        }

        private void Form2_MouseUp(object sender, MouseEventArgs e)
        {
            IsTap = false;
        }

        private void maskedTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void maskedTextBox1_Enter(object sender, EventArgs e)
        {
            //IsStartSelected = true;
            
            monthCalendar1.Visible = true;
            IsStartPeriod = true;
            monthCalendar1.Location = new Point(67, 456);
            if (!string.IsNullOrEmpty(maskedTextBox1.Text))
            {
                DateTime entryDate;
                if (DateTime.TryParseExact(maskedTextBox1.Text, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out entryDate))
                {
                    //if (startDateLast != entryDate)
                    if (IsPeriodChanged)
                    {
                        startDateLast = entryDate;
                        if (IsDataSetFirstReady)
                        {
                            ieClimateData = new List<Input>();
                            temperatureExtreme = float.MinValue;
                            pressureExtreme = float.MinValue;
                            humidityExtreme = float.MinValue;
                            temperature2Extreme = float.MinValue;
                            pressure2Extreme = float.MinValue;
                            humidity2Extreme = float.MinValue;
                        }
                        IsDataSetFirstReady = false; // chart refreshing because of period change
                        GetInfo();
                    }
                }
            }
        }
        private void maskedTextBox2_Enter(object sender, EventArgs e)
        {
            //if (IsCalendarEntry && IsStartPeriod) // to set first date 
                //maskedTextBox1.Focus();
            //IsFinishSelected = true;
            monthCalendar1.Visible = true;
            IsStartPeriod = false;
            monthCalendar1.Location = new Point(216, 456);
            if (!string.IsNullOrEmpty(maskedTextBox2.Text))
            {
                DateTime entryDate;
                if (DateTime.TryParseExact(maskedTextBox2.Text, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out entryDate))
                {
                    //if (finishDateLast != entryDate)
                    if (IsPeriodChanged)
                    {
                        finishDateLast = entryDate;
                        if (IsDataSetFirstReady)
                        {
                            ieClimateData = new List<Input>();
                            temperatureExtreme = float.MinValue;
                            pressureExtreme = float.MinValue;
                            humidityExtreme = float.MinValue;
                            temperature2Extreme = float.MinValue;
                            pressure2Extreme = float.MinValue;
                            humidity2Extreme = float.MinValue;
                        }
                        IsDataSetFirstReady = false; // chart refreshing because of period change
                        GetInfo();
                    }
                }
                //else
                //MessageBox.Show("Invalid date format date must be formatted to MM/dd/yyyy", "Date entry error");
            }
            //else
            //MessageBox.Show("No date, lease provide entry date in the format of dd/MM/yyyy", "Date entry error");
        }


        private void maskedTextBox2_Validated(object sender, EventArgs e)
        {

        }

        private void maskedTextBox2_DataContextChanged(object sender, EventArgs e)
        {

        }
        private void groupBox3_Leave(object sender, EventArgs e)
        {
            //monthCalendar1.Visible = false;
        }

        private void monthCalendar1_Leave(object sender, EventArgs e)
        {
            monthCalendar1.Visible = false;
            /*if (!IsStartPeriod)
                maskedTextBox2.Focus();*/
        }



        private void button2_Click(object sender, EventArgs e)
        {
            mapCtl.Level--;
            invalidateTimer.Interval = 500;
            invalidateTimer.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (IsMarkerMode)
                IsMarkerMode = false;
            else
                IsMarkerMode = true;
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            double longitude = 0, latitude = 0;
            if (comboBox1.SelectedItem.ToString() == string.Empty) return;
            if (!double.TryParse(citiesData[comboBox1.SelectedItem.ToString()][0].Replace('.', ',').Replace("\"", ""), out latitude)) return;
            if (!double.TryParse(citiesData[comboBox1.SelectedItem.ToString()][1].Replace('.', ',').Replace("\"", ""), out longitude)) return;
            mapCtl.CenterCoordinate = new Coordinate(longitude, latitude);
            mapCtl.Level = 12;

            invalidateTimer.Interval = 500;
            invalidateTimer.Start();
        }
        void DrawCharts()
        {
            IsDataSetFirstReady = false;
            GetInfo();
            //Invalidate();
            //groupBox1.Visible = true;
            //groupBox2.Visible = true;

        }
        private void button4_Click(object sender, EventArgs e)
        {
            //DrawCharts();
        }

        private void Form2_Enter(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            /*
            for (int i = 1; i <= 1000; i++)
            {
                if (backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                Thread.Sleep(50); // Simulate work
                backgroundWorker1.ReportProgress(i);
            }*/

            while (!backgroundWorker1.CancellationPending)
            {
                /*
                
                if (IsFormLoaded)
                {
                    if (IsReady)
                    {
                        progressBar1.Visible = false;
                        //progressBar1.Invoke(() => { progressBar1.Visible = false; });
                        label10.Invoke(() => { label10.Visible = false; });
                    }
                    else
                    {
                        progressBar1.Visible = true;

                        //progressBar1.Invoke(() => { progressBar1.Visible = true; });
                        label10.Invoke(() => { label10.Visible = true; });
                    }
                }*/
                // Perform background task
                //Console.WriteLine("Background task running...");
                backgroundWorker1.ReportProgress(0);
                Thread.Sleep(100); // Simulate work

                // Optionally report progress (if needed)
            }

            e.Cancel = true; // Indicate cancellation
        }
        int f = 0;
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            /*f++;
            if (f == 10)
            {
                Invalidate();
                f=0;
            }**/
            if (IsFormLoaded)
            {
                if (IsReady)
                {
                    //progressBar1.Invoke(() => { progressBar1.Visible = false; });
                    //label10.Invoke(() => { label10.Visible = false; });
                    progressBar1.Visible = false;
                    label10.Visible = false;
                    if (IsSecondChartLoaded)
                    {
                        groupBox1.Visible = true;
                        groupBox2.Visible = true;
                        groupBox4.Visible = true;
                    }
                }
                else
                {
                    //progressBar1.Invoke(() => { progressBar1.Visible = true; });
                    progressBar1.Visible = true;
                    label10.Visible = true;
                    groupBox1.Visible = false;
                    groupBox2.Visible = false;
                    groupBox4.Visible = false;
                }
            }
            progressBar1.Value = e.ProgressPercentage;
            //progressBar1.PerformStep();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            /*
            if (e.Cancelled)
                MessageBox.Show("Operation Cancelled");
            else if (e.Error != null)
                MessageBox.Show($"Error: {e.Error.Message}");
            else
                MessageBox.Show("Operation Completed");*/
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            IsDataSetFirstReady = false;
            GetInfo();
        }
    }
}
