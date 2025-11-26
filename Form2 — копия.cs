using Microsoft.ML;
using Microsoft.ML.Data;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using SM.EF;
using SM.Map;
using SM.Map.G;
using System.Collections.Generic;
using System.Timers;
namespace UrbanHeatIsland
{
    public partial class Form2 : Form
    {
        static bool IsComparing, IsLoaded;
        Graphics cGraphics;
        List<DateTime> dates = new List<DateTime>();
        List<float> temperatures = new List<float>();
        List<float> temperatures2 = new List<float>();
        float extremeTemperature = float.MinValue;
        float extremePressure = float.MinValue;
        float extremeTemperature2 = float.MinValue;
        float extremePressure2 = float.MinValue;
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
        private void GetInfoFromNASAServer(string args)
        {
            //string apiUrlMain = "https://power.larc.nasa.gov/api/temporal/daily/point?parameters=T2M,PRECTOT&community=RE&longitude=37.53&latitude=58.84&start=19920101&end=20250920&format=JSON"; // Example API URL
            string apiUrlMain = "https://power.larc.nasa.gov/api/temporal/daily/point?parameters=T2M,PRECTOT&community=RE&" + args + "&format=CSV"; // Example API URL
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
        void GetInfo()
        {
            IsCrear = true;
            if (IsStartSelected && IsFinishSelected && markers.Count > 0)
            {
                string[] startDate = maskedTextBox1.Text.Split('/');
                string[] finishDate = maskedTextBox2.Text.Split('/');
                int selectedMarkerId = 0;
                if (!IsDataSetFirstReady || markers.Count == 1)
                {
                    selectedMarkerId = 0;
                    temperatures.Clear();
                    GetInfoFromNASAServer("longitude=" + markers[selectedMarkerId].Longitude.ToString().Replace(',', '.') + "&latitude=" + markers[selectedMarkerId].Latitude.ToString().Replace(',', '.') + "&start=" + startDate[2] + startDate[0] + startDate[1] + "&end=" + finishDate[2] + finishDate[0] + finishDate[1]);
                    Invalidate();
                    IsDataSetFirstReady = true;
                    IsComparing = true;
                }
                if (markers.Count > 1)
                {
                    selectedMarkerId = 1;
                    temperatures2.Clear();
                    GetInfoFromNASAServer("longitude=" + markers[selectedMarkerId].Longitude.ToString().Replace(',', '.') + "&latitude=" + markers[selectedMarkerId].Latitude.ToString().Replace(',', '.') + "&start=" + startDate[2] + startDate[0] + startDate[1] + "&end=" + finishDate[2] + finishDate[0] + finishDate[1]);
                    Invalidate();
                }
            }
        }
        System.Timers.Timer invalidateTimer = new System.Timers.Timer();
        
        void MapInvalidate(object sender, ElapsedEventArgs e)
        {
            Invalidate();
        }
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
            invalidateTimer.Interval = 5000;
            invalidateTimer.Start();
            //frmOpticMap.Activate();
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
        float Forecast()
        {
            return Forecast(DateTime.Now);
        }
        
        float Forecast(DateTime selectedDate)
        {
            var context = new MLContext(seed: 0);
            // Load the data
            var data = context.Data.LoadFromTextFile<Input>(cFileName, hasHeader: true, separatorChar: ',');
            // Split the data into a training set and a test set
            // One-hot encode the values in the "UseCode" column and train the model
            var pipeline = context.Transforms.Categorical.OneHotEncoding(inputColumnName: "UseCode", outputColumnName: "UseCodeEncoded")
                .Append(context.Transforms.Concatenate("Features", "UseCodeEncoded", "YEAR", "MO", "DY"))
                .Append(context.Regression.Trainers.FastForest(numberOfTrees: 200, minimumExampleCountPerLeaf: 4));
            var model = pipeline.Fit(data);
            // Use the model to make a prediction
            var predictor = context.Model.CreatePredictionEngine<Input, Output>(model);
            var input = new Input
            {
                YEAR = selectedDate.Year,
                MO = selectedDate.Month,
                DY = selectedDate.Day,
                T2M = 0,
                UseCode = "",
            };
            var prediction = predictor.Predict(input);
            float Temperature = prediction.Score;
            return Temperature;
        }

        float Forecast2(DateTime selectedDate)
        {
            var context = new MLContext(seed: 0);
            // Load the data
            var data = context.Data.LoadFromEnumerable(ieClimateData);
            // Split the data into a training set and a test set
            // One-hot encode the values in the "UseCode" column and train the model
            var pipeline = context.Transforms.Categorical.OneHotEncoding(inputColumnName: "UseCode", outputColumnName: "UseCodeEncoded")
                .Append(context.Transforms.Concatenate("Features", "UseCodeEncoded", "YEAR", "MO", "DY"))
                .Append(context.Regression.Trainers.FastForest(numberOfTrees: 200, minimumExampleCountPerLeaf: 4));
            var model = pipeline.Fit(data);
            // Use the model to make a prediction
            var predictor = context.Model.CreatePredictionEngine<Input, Output2>(model);
            var input = new Input
            {
                YEAR = selectedDate.Year,
                MO = selectedDate.Month,
                DY = selectedDate.Day,
                UseCode = " ",
            };
            var prediction = predictor.Predict(input);
            float temperature = prediction.Score2;
            return temperature;
        }

        public static void Example()
        {
            // Create a new context for ML.NET operations. It can be used for
            // exception tracking and logging, as a catalog of available operations
            // and as the source of randomness.
            var mlContext = new MLContext();

            // Get a small dataset as an IEnumerable.
            IEnumerable<DataPointVector> enumerableKnownSize = new DataPointVector[]
            {
               new DataPointVector{ Features = new float[]{ 1.2f, 3.4f, 4.5f, 3.2f,
                   7,5f } },

               new DataPointVector{ Features = new float[]{ 4.2f, 3.4f, 14.65f,
                   3.2f, 3,5f } },

               new DataPointVector{ Features = new float[]{ 1.6f, 3.5f, 4.5f, 6.2f,
                   3,5f } },

            };

            // Load dataset into an IDataView. 
            IDataView data = mlContext.Data.LoadFromEnumerable(enumerableKnownSize);
            var featureColumn = data.Schema["Features"].Type as VectorDataViewType;
            // Inspecting the schema
            Console.WriteLine($"Is the size of the Features column known: " +
                $"{featureColumn.IsKnownSize}.\nSize: {featureColumn.Size}");

            // Preview
            //
            // Is the size of the Features column known? True.
            // Size: 5.

            // If the size of the vector is unknown at compile time, it can be set 
            // at runtime.
            IEnumerable<DataPoint> enumerableUnknownSize = new DataPoint[]
            {
               new DataPoint{ Features = new float[]{ 1.2f, 3.4f, 4.5f } },
               new DataPoint{ Features = new float[]{ 4.2f, 3.4f, 1.6f } },
               new DataPoint{ Features = new float[]{ 1.6f, 3.5f, 4.5f } },
            };

            // The feature dimension (typically this will be the Count of the array 
            // of the features vector known at runtime).
            int featureDimension = 3;
            var definedSchema = SchemaDefinition.Create(typeof(DataPoint));
            featureColumn = definedSchema["Features"]
                .ColumnType as VectorDataViewType;

            Console.WriteLine($"Is the size of the Features column known: " +
                $"{featureColumn.IsKnownSize}.\nSize: {featureColumn.Size}");

            // Preview
            //
            // Is the size of the Features column known? False.
            // Size: 0.

            // Set the column type to be a known-size vector.
            var vectorItemType = ((VectorDataViewType)definedSchema[0].ColumnType)
                .ItemType;
            definedSchema[0].ColumnType = new VectorDataViewType(vectorItemType,
                featureDimension);

            // Read the data into an IDataView with the modified schema supplied in
            IDataView data2 = mlContext.Data
                .LoadFromEnumerable(enumerableUnknownSize, definedSchema);

            featureColumn = data2.Schema["Features"].Type as VectorDataViewType;
            // Inspecting the schema
            Console.WriteLine($"Is the size of the Features column known: " +
                $"{featureColumn.IsKnownSize}.\nSize: {featureColumn.Size}");

            // Preview
            //
            // Is the size of the Features column known? True. 
            // Size: 3.
        }
    

    public class DataPoint
    {
        public float[] Features { get; set; }
    }

    public class DataPointVector
    {
        [VectorType(5)]
        public float[] Features { get; set; }
    }
    void Convert(string fileName)
        {
            IsLoaded = false;
            StreamReader foundReader = new StreamReader(fileName);
            cFileName = fileName.Remove(fileName.Length - 4, 4);
            cFileName += "Tmp.csv";
            if (!IsComparing)
                cFileName0 = cFileName;
            StreamWriter powerFileConvertedWriter = new StreamWriter(cFileName);
            powerFileConvertedWriter.WriteLine("YEAR,MO,DY,T2M,PRECTOTCORR");
            int i = -1;
            bool IsStart = false;
            int pointsCount = 0;
            while (!foundReader.EndOfStream)
            {
                string line = foundReader.ReadLine();
                i++;
                if (line.Contains("YEAR,MO,DY,T2M,PRECTOTCORR"))
                {
                    IsStart = true;
                    line = foundReader.ReadLine();
                }
                if (!IsStart) // skip header
                    continue;
                powerFileConvertedWriter.WriteLine(line);
                string[] lv = line.Split(',');
                float v1 = float.Parse(lv[3].Replace('.', ','));
                float v2 = float.Parse(lv[4].Replace('.', ','));
                if (!IsComparing)
                {
                    dates.Add(new DateTime(int.Parse(lv[0]), int.Parse(lv[1]), int.Parse(lv[2])));
                    temperatures.Add(v1);
                }
                else if (dates[pointsCount].Year == int.Parse(lv[0]) && dates[pointsCount].Month == int.Parse(lv[1]) && dates[pointsCount].Day == int.Parse(lv[2]))
                    temperatures2.Add(v1);
                pointsCount++;
                if (IsComparing)
                {
                    if (v1 > extremeTemperature2)
                        extremeTemperature2 = v1;
                    if (v2 > extremePressure2)
                        extremePressure2 = v2;
                }
                else
                {
                    if (v1 > extremeTemperature)
                        extremeTemperature = v1;
                    if (v2 > extremePressure)
                        extremePressure = v2;
                }
            }
            powerFileConvertedWriter.Close();
            foundReader.Close();
            if (IsComparing)
            {
                //label3.Text = Forecast().ToString();
                //label4.Text = Forecast2().ToString();
                label6.Text = extremeTemperature2.ToString();
                label5.Text = extremePressure2.ToString();
                label5.Visible = true;
                label6.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                if (temperatures2.Count > 0)
                    IsLoaded = true;
            }
            else
            {
                //label1.Text = Forecast().ToString();
                //label2.Text = Forecast2().ToString();
                label8.Text = extremeTemperature.ToString();
                label7.Text = extremePressure.ToString();
                if (temperatures.Count > 0)
                    IsLoaded = true;
            }

        }

        void ReportMake(string Content)
        {
            List<Input> listClimateData = new List<Input>();
            if (!IsDataSetFirstReady)
            {
                temperatures.Clear();
                dates.Clear();
            }
            else
                temperatures2.Clear();
            IsLoaded = false;
            string[] lines = Content.Split('\n');
            bool IsStart = false;
            int pointsCount = 0;
            foreach (string line in lines)
            {
                if (line == string.Empty)
                    continue;
                if (!IsStart && line.Contains("YEAR,MO,DY,T2M,PRECTOTCORR"))
                {
                    IsStart = true;
                    continue;
                }
                if (!IsStart) // skip header
                    continue;
                string[] lv = line.Split(',');
                float v1 = float.Parse(lv[3].Replace('.', ','));
                float v2 = float.Parse(lv[4].Replace('.', ','));
                if (!IsDataSetFirstReady)
                {
                    DateTime date = new DateTime(int.Parse(lv[0]), int.Parse(lv[1]), int.Parse(lv[2]));
                    dates.Add(date);
                    var dataSet = new Input
                    {
                        YEAR = date.Year,
                        MO = date.Month,
                        DY = date.Day,
                        T2M = v1,
                        UseCode = " ",
                    };
                    listClimateData.Add(dataSet);
                    temperatures.Add(v1);
                }
                else
                {
                    if (dates[pointsCount].Year == int.Parse(lv[0]) && dates[pointsCount].Month == int.Parse(lv[1]) && dates[pointsCount].Day == int.Parse(lv[2]))
                        temperatures2.Add(v1);
                }
                pointsCount++;
                if (IsDataSetFirstReady)
                {
                    if (v1 > extremeTemperature2)
                        extremeTemperature2 = v1;
                    if (v2 > extremePressure2)
                        extremePressure2 = v2;
                }
                else
                {
                    if (v1 > extremeTemperature)
                        extremeTemperature = v1;
                    if (v2 > extremePressure)
                        extremePressure = v2;
                }
            }
            if (IsDataSetFirstReady)
            {
                //label3.Text = Forecast().ToString();
                //label4.Text = Forecast2().ToString();
                label6.Text = extremeTemperature2.ToString();
                label5.Text = extremePressure2.ToString();
                label5.Visible = true;
                label6.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                if (temperatures2.Count > 0)
                    IsLoaded = true;
            }
            else
            {
                //label1.Text = Forecast().ToString();
                //label2.Text = Forecast2().ToString();
                ieClimateData = new List<Input>(listClimateData);
                label1.Text = Forecast2(DateTime.Now).ToString();
                label8.Text = extremeTemperature.ToString();
                label7.Text = extremePressure.ToString();
                if (temperatures.Count > 0)
                    IsLoaded = true;
            }

        }
        void CSVOpen()
        {
            IsFirstOpen = true;
            OpenFileDialog openFileDialogC = new OpenFileDialog();
            openFileDialogC.Filter = "Text Files|*.csv;";
            if (openFileDialogC.ShowDialog() == DialogResult.OK)
            {
                Convert(openFileDialogC.FileName);
                groupBox1.Visible = true;
                groupBox2.Visible = true;
            }
            else
            {
                groupBox1.Visible = false;
                groupBox2.Visible = false;
                label10.Visible = true;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public class Input
        {
            [LoadColumn(0)]
            public float YEAR;

            [LoadColumn(1)]
            public float MO;

            [LoadColumn(2)]
            public float DY;

            [LoadColumn(3), ColumnName("Label")]
            public float T2M;

            [LoadColumn(4)]
            public string UseCode;
        }
        public class Input2
        {
            [LoadColumn(0)]
            public float YEAR;

            [LoadColumn(1)]
            public float MO;

            [LoadColumn(2)]
            public float DY;

            [LoadColumn(3)]
            public float T2M;

            [LoadColumn(4), ColumnName("Label")]
            public float PRECTOTCORR;

            [LoadColumn(5)]
            public string UseCode;

        }
        public class Output
        {
            [ColumnName("Score")]
            public float Score;
        }
        public class Output2
        {
            [ColumnName("Score")]
            public float Score2;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cFileName != string.Empty)
                File.Delete(cFileName);
            if (cFileName0 != string.Empty)
                File.Delete(cFileName0);
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            if (IsStartPeriod)
                maskedTextBox1.Text = e.Start.ToString("MM/dd/yyyy").Replace('.', '/');
            else
                maskedTextBox2.Text = e.Start.ToString("MM/dd/yyyy").Replace('.', '/');

            if (IsStartPeriod)
                IsStartSelected = true;
            else
                IsFinishSelected = true;


            /*
            if (!IsCompared)
            {
                label1.Text = Forecast(e.End).ToString();
                label2.Text = Forecast2(e.End).ToString();
            }*/
        }
        void ChartDraw(List<float> temperatures, Color ChartColor)
        {
            Graphics g = CreateGraphics();
            ChartDraw(g, temperatures, ChartColor);
        }
        int YAxis = 360;
        void ChartDraw(Graphics g, List<float> temperatures, Color ChartColor)
        {
            g.DrawLine(new Pen(new SolidBrush(Color.Black), 1), new PointF(YAxis, 0), new PointF(YAxis, 1200));
            g.DrawLine(new Pen(new SolidBrush(Color.Black), 1), new PointF(YAxis, 500), new PointF(1750, 500));
            for (int i = 0; i < 10; i++)
            {
                g.DrawString(((5 - i) * 10).ToString(), new System.Drawing.Font("Calibry", 12), new SolidBrush(Color.Black), YAxis, i * 100);
                g.DrawLine(new Pen(new SolidBrush(Color.Black), 1), new PointF(255, i * 100), new PointF(265, i * 100));
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
            if (IsLoaded)
            {
                //cGraphics = e.Graphics;
                e.Graphics.FillRectangle(new SolidBrush(Form2.DefaultBackColor), YAxis, 0, 2000, 2000);
                ChartDraw(temperatures, Color.DarkBlue);
                if (IsComparing)
                    ChartDraw(temperatures2, Color.Red);
                groupBox1.Visible = true;
                groupBox2.Visible =true;
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
                    GetInfo();
                    Invalidate();
                }
                else
                {
                    IsTap = true;
                    lastTapCoordinate = mapCtl.CenterCoordinate;
                    lastTap = e.Location;
                }
            }
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
        bool IsStartPeriod, IsStartSelected, IsFinishSelected;
        private void maskedTextBox1_Enter(object sender, EventArgs e)
        {
            //IsStartSelected = true;
            monthCalendar1.Visible = true;
            IsStartPeriod = true;
            monthCalendar1.Location = new Point(67, 456);
            IsDataSetFirstReady = false; // chart refreshing because of period change
            GetInfo();
        }

        private void maskedTextBox2_Enter(object sender, EventArgs e)
        {
            //IsFinishSelected = true;
            monthCalendar1.Visible = true;
            IsStartPeriod = false;
            monthCalendar1.Location = new Point(216, 456);
            IsDataSetFirstReady = false; // chart refreshing because of period change
            GetInfo();
        }

        private void groupBox3_Leave(object sender, EventArgs e)
        {
            //monthCalendar1.Visible = false;
        }

        private void monthCalendar1_Leave(object sender, EventArgs e)
        {
            monthCalendar1.Visible = false;
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
        void DrawCharts ()
        {
            IsDataSetFirstReady = false;
            GetInfo();
            //Invalidate();
            groupBox1.Visible = true;
            groupBox2.Visible = true;

        }
        private void button4_Click(object sender, EventArgs e)
        {
            DrawCharts();
            
        }
    }
}
