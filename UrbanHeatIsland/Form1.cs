using Microsoft.ML;
using Microsoft.ML.Data;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
namespace UrbanHeatIsland
{
    public partial class Form1 : Form
    {
        static bool IsCompared, IsLoaded;
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
        public Form1()
        {
            InitializeComponent();
        }
        float Forecast()
        {
            return Forecast(DateTime.Now);
        }
        float Forecast2()
        {
            return Forecast2(DateTime.Now);
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
            var data = context.Data.LoadFromTextFile<Input2>(cFileName, hasHeader: true, separatorChar: ',');
            // Split the data into a training set and a test set
            // One-hot encode the values in the "UseCode" column and train the model
            var pipeline = context.Transforms.Categorical.OneHotEncoding(inputColumnName: "UseCode", outputColumnName: "UseCodeEncoded")
                .Append(context.Transforms.Concatenate("Features", "UseCodeEncoded", "YEAR", "MO", "DY", "T2M"))
                .Append(context.Regression.Trainers.FastForest(numberOfTrees: 200, minimumExampleCountPerLeaf: 4));
            var model = pipeline.Fit(data);
            // Use the model to make a prediction
            var predictor = context.Model.CreatePredictionEngine<Input2, Output2>(model);
            var input2 = new Input2
            {
                YEAR = selectedDate.Year,
                MO = selectedDate.Month,
                DY = selectedDate.Day,
                T2M = 0,
                PRECTOTCORR = 0,
                UseCode = " ",
            };
            var prediction = predictor.Predict(input2);
            float pressure = prediction.Score2;
            return pressure;
        }
        void Convert(string fileName)
        {
            IsLoaded = false;
            StreamReader foundReader = new StreamReader(fileName);
            cFileName = fileName.Remove(fileName.Length - 4, 4);
            cFileName += "Tmp.csv";
            if (!IsCompared)
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
                if (!IsCompared)
                {
                    dates.Add(new DateTime(int.Parse(lv[0]), int.Parse(lv[1]), int.Parse(lv[2])));
                    temperatures.Add(v1);
                }
                else if (dates[pointsCount].Year == int.Parse(lv[0]) && dates[pointsCount].Month == int.Parse(lv[1]) && dates[pointsCount].Day == int.Parse(lv[2]))
                    temperatures2.Add(v1);
                pointsCount++;
                if (IsCompared)
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
            if (IsCompared)
            {
                label3.Text = Forecast().ToString();
                label4.Text = Forecast2().ToString();
                label6.Text = extremeTemperature2.ToString();
                label5.Text = extremePressure2.ToString();
                label5.Visible = true;
                label6.Visible = true;
                label3.Visible = true;
                label4 .Visible = true;
                if (temperatures2.Count>0)
                    IsLoaded = true;
            }
            else
            {
                label1.Text = Forecast().ToString();
                label2.Text = Forecast2().ToString();
                label8.Text = extremeTemperature.ToString();
                label7.Text = extremePressure.ToString();
                if (temperatures.Count > 0)
                    IsLoaded = true;
            }
            
        }
        void CSVOpen ()
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
                groupBox2 .Visible = false;
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
            if (cFileName!=string.Empty)
                File.Delete(cFileName);
            if (cFileName0!=string.Empty)
                File.Delete(cFileName0);
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            if (!IsCompared)
            {
                label1.Text = Forecast(e.End).ToString();
                label2.Text = Forecast2(e.End).ToString();
            }
        }
        void ChartDraw(List<float> temperatures, Color ChartColor)
        {
            Graphics g = CreateGraphics();
            ChartDraw(g, temperatures, ChartColor);
        }
        void ChartDraw (Graphics g, List<float> temperatures, Color ChartColor)
        {
            g.DrawLine(new Pen(new SolidBrush(Color.Black), 1), new PointF(260, 0), new PointF(260, 1000));
            g.DrawLine(new Pen(new SolidBrush(Color.Black), 1), new PointF(260, 500), new PointF(1750, 500));
            for (int i = 0; i < 10; i++)
            {
                g.DrawString(((5 - i) * 10).ToString(), new System.Drawing.Font("Calibry", 12), new SolidBrush(Color.Black), 260, i * 100);
                g.DrawLine(new Pen(new SolidBrush(Color.Black), 1), new PointF(255, i * 100), new PointF(265, i * 100));
            }
            float interval = 1500f / ((dates.Last() - dates.First()).Days);
            for (int i = 0; i < temperatures.Count - 1; i++)
            {
                float X = (dates[i] - dates.First()).Days * interval;
                float nextX = (dates[i + 1] - dates.First()).Days * interval;
                g.DrawEllipse(new Pen(new SolidBrush(ChartColor), 3), 260 + X, -temperatures[i] * 10 + 500, 2, 2);
                g.DrawLine(new Pen(new SolidBrush(ChartColor), 1), new PointF(260 + X, -temperatures[i] * 10 + 500), new PointF(260 + nextX, -temperatures[i + 1] * 10 + 500));
            }
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (IsLoaded)
            {
                cGraphics = e.Graphics;
                if (!IsCompared)
                    ChartDraw(temperatures, Color.Red);
                else
                    ChartDraw(temperatures2, Color.Blue);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!IsFirstOpen)
                CSVOpen();
            else
            {
                IsCompared = true;
                CSVOpen();
                ChartDraw(temperatures2, Color.Blue);
            }
        }
    }
}
