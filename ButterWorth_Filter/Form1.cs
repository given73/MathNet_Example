//using DSP;
using MathNet.Filtering;
using MathNet.Filtering.FIR;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace ButterWorth_Filter
{
    public partial class Form1 : Form
    {

        DSP.LowpassFilterButterworthImplementation LowPassF;

        public Form1()
        {
            InitializeComponent();

            // double cutoffFrequencyHz, int numSections, double Fs)
            LowPassF = new DSP.LowpassFilterButterworthImplementation(500000, 625, 1250);
            //mathnetFilterTest();

            mathnetFilterInit();
        }
        int buffer_size=0;
        
        //System.Collections.Generic.IEnumerable<double> y;
        OnlineFilter lowpass;
        OnlineFilter hipass;
        OnlineFilter bandpass;
        OnlineFilter bandpassnarrow;

        private void mathnetFilterTest()
        {
            //signal + noise
            double fs = 250000000;          //sampling rate
            double fw =   2000000;          //signal frequency
            double fn =    430000;          //noise frequency
            double n  =       100;         //number of periods to show
            double A  =         5;          //signal amplitude
            double N  =         5;          //noise amplitude
            buffer_size  = (int)(n * fs / fw); //sample size

            var t = Enumerable.Range(1, buffer_size).Select(p => p * 1 / fs).ToArray();
            var y = t.Select(p => (A * Math.Sin(2 * Math.PI * fw * p)) + (N * Math.Sin(2 * Math.PI * fn * p))).ToArray(); //Original
            // y = t.Select(p => (A * Math.Sin(2 * Math.PI * fw * p)) + (N * Math.Sin(2 * Math.PI * fn * p))).ToArray(); //Original
            //lowpass filter
            double fc_L = 500000;         //cutoff frequency

            //var lowpass = OnlineFirFilter.CreateLowpass(ImpulseResponse.Finite, fs, fc_L,50);
            lowpass = OnlineFirFilter.CreateLowpass(ImpulseResponse.Finite, fs, fc_L, 50);

            //hipass filter 
            double fc_H = 2000000;      //cutoff frequency 
            //var hipass = OnlineFirFilter.CreateHighpass(ImpulseResponse.Finite, fs, fc_H,200);
            hipass = OnlineFirFilter.CreateHighpass(ImpulseResponse.Finite, fs, fc_H, 200);


            //bandpass filter
            double fc1 = 0;         //low cutoff frequency
            double fc2 = 900000;        //high cutoff frequency
            //var bandpass = OnlineFirFilter.CreateBandpass(ImpulseResponse.Finite, fs, fc1, fc2);
            bandpass = OnlineFirFilter.CreateBandpass(ImpulseResponse.Finite, fs, fc1, fc2);

            //narrow bandpass filter
            fc1 = 350000;                //low cutoff frequency
            fc2 = 450000;                //high cutoff frequency
            //var bandpassnarrow = OnlineFirFilter.CreateBandpass(ImpulseResponse.Finite, fs, fc1, fc2);
            bandpassnarrow = OnlineFirFilter.CreateBandpass(ImpulseResponse.Finite, fs, fc1, fc2);

            double[] yf1 = lowpass.ProcessSamples(y);           //Lowpass
            double[] yf2 = hipass.ProcessSamples(y);    //Hipass 
            double[] yf3 = bandpass.ProcessSamples(y);          //Bandpass
            double[] yf4 = bandpassnarrow.ProcessSamples(y);    //Bandpass Narrow

            textBox1.Text += "Y   Origin          : " + y.Length.ToString() + Environment.NewLine;
            textBox1.Text += "Yf1 Lowpass         : " + yf1.Length.ToString() + Environment.NewLine;
            textBox1.Text += "Yf2 Hipass          : " + yf2.Length.ToString() + Environment.NewLine;
            textBox1.Text += "Yf3 Bandpass        : " + yf3.Length.ToString() + Environment.NewLine;
            textBox1.Text += "Yf4 Banspass Narrow : " + yf4.Length.ToString() + Environment.NewLine;

            double aa = 0;
            for (int j = 0; j < y.Length; j++)
            {
                chart1.Series[0].Points.AddXY(aa, y[j] - 20);
                chart1.Series[1].Points.AddXY(aa, yf1[j] / 5);
                chart1.Series[2].Points.AddXY(aa, yf2[j] * 3 + 20);
                //chart1.Series[3].Points.AddXY(aa, yf3[j]);
                //chart1.Series[4].Points.AddXY(aa, yf4[j]*5);
                aa += 1;
            }

        }

        //private double fs = 250000000;          //sampling rate
        

        private double wave_index=0;
        double[] y   = new double[11627];
        double[] yf1 = new double[11627];//owpass.ProcessSamples(y);           //Lowpass
        double[] yf2 = new double[11627];//hipass.ProcessSamples(y);           //Hipass 
                                         //double[] yf3 = new double[625];//hipass.ProcessSamples(y);           //Hipass 
        double fs = 200000000;         //sampling rate
        double fw = 20000000;         //signal frequency
        double fn = 430000;         //noise frequency
        double n = 100;         //number of periods to show
        double A = 3;         //signal amplitude
        double N = 3;         //noise amplitude

        private void mathnetFilterTest1()
        {            
            MakeProcessData(); ////signal + noise         

            DrawChartData();

        }


        private void mathnetFilterInit()
        {
            //lowpass filter
            double fc_L = 500000;         //cutoff frequency
            //var lowpass = OnlineFirFilter.CreateLowpass(ImpulseResponse.Finite, fs, fc_L,50);
            lowpass = OnlineFirFilter.CreateLowpass(ImpulseResponse.Finite, fs, fc_L, 50);


            //hipass filter 
            double fc_H = 20000000;      //cutoff frequency 
            //var hipass = OnlineFirFilter.CreateHighpass(ImpulseResponse.Finite, fs, fc_H,200);
            hipass = OnlineFirFilter.CreateHighpass(ImpulseResponse.Finite, fs, fc_H, 200);

            //bandpass filter
            double fc1 = 0;         //low cutoff frequency
            double fc2 = 900000;        //high cutoff frequency
            //var bandpass = OnlineFirFilter.CreateBandpass(ImpulseResponse.Finite, fs, fc1, fc2);
            bandpass = OnlineFirFilter.CreateBandpass(ImpulseResponse.Finite, fs, fc1, fc2);

            //narrow bandpass filter
            fc1 = 350000;                //low cutoff frequency
            fc2 = 450000;                //high cutoff frequency
            //var bandpassnarrow = OnlineFirFilter.CreateBandpass(ImpulseResponse.Finite, fs, fc1, fc2);
            bandpassnarrow = OnlineFirFilter.CreateBandpass(ImpulseResponse.Finite, fs, fc1, fc2);

        }


        private void MakeProcessData()
        {
            double check = 1 / fs;
            for (int i = 0; i < buffer_size; i++)
            {
                y[i] = (A * Math.Sin(2 * Math.PI * fw * wave_index)) + (N * Math.Sin(2 * Math.PI * fn * wave_index));
                yf1[i] = lowpass.ProcessSample(y[i]);
                yf2[i] = hipass.ProcessSample(y[i]);
                //yf3[i] = LowPassF.compute(y[i]);
                //yf4[i] = LowPassF.compute(y[i]);
                wave_index += check;
            }

        }

        private void DrawChartData()
        {
            textBox1.Text = "Size : " + buffer_size.ToString();
            //textBox1.Text += "Y   Origin          : " + y.Length.ToString()   + Environment.NewLine;
            //textBox1.Text += "Yf1 Lowpass         : " + yf1.Length.ToString() + Environment.NewLine;
            //textBox1.Text += "Yf2 Hipass          : " + yf2.Length.ToString() + Environment.NewLine;
            //textBox1.Text += "Yf3 Bandpass        : " + yf3.Length.ToString() + Environment.NewLine;
            //textBox1.Text += "Yf4 Banspass Narrow : " + yf4.Length.ToString() + Environment.NewLine;

            double aa = 0;
            for (int j = 0; j < buffer_size; j++)
            {
                chart1.Series[0].Points.AddXY(aa, y[j]);
                chart1.Series[1].Points.AddXY(aa, yf1[j] / 10);
                chart1.Series[2].Points.AddXY(aa, yf2[j] * 3);
                //chart1.Series[3].Points.AddXY(aa, yf3[j]);
                //chart1.Series[4].Points.AddXY(aa, yf4[j]*5);
                aa += 1;
            }

        }


        //private int buffersize =0;
        private void button_OneStepRun_Click(object sender, EventArgs e)
        {
                  BufferReset(); 
            ClearChart();
            mathnetFilterTest1();
        }

        private void button_ChartClear_Click(object sender, EventArgs e)
        {
            ClearChart();
        }

        private void button_FreeRun_Click(object sender, EventArgs e)
        {
            if(timer1.Enabled)
            {
                timer1.Stop();
            }
            else
            {
                buffer_size = (int)(n * fs / fw);     //sample size
                y = new double[buffer_size];
                yf1 = new double[buffer_size];//owpass.ProcessSamples(y);           //Lowpass
                yf2 = new double[buffer_size];//hipass.ProcessSamples(y);           //Hipass 
                timer1.Interval = 100;   // 50 Hz
                timer1.Start();
            }            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ClearChart();
            mathnetFilterTest1();

        }

        private void BufferReset()
        {
            // buffer Reset()
            buffer_size = (int)(n * fs / fw);     //sample size
            y = new double[buffer_size];
            yf1 = new double[buffer_size];//owpass.ProcessSamples(y);           //Lowpass
            yf2 = new double[buffer_size];//hipass.ProcessSamples(y);           //Hipass 

        }

        private void ClearChart()
        {
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart1.Series[2].Points.Clear();
            chart1.Series[3].Points.Clear();
            chart1.Series[4].Points.Clear();
        }

        private void button_MakeSineWave_Click(object sender, EventArgs e)
        {
            double SineWaveIndex = 0;
            buffer_size = (int)(n * fs / fw);     //sample size
            double check = 1 / fs;            

            for (int j = 0; j < buffer_size; j++)
            {
                chart1.Series[0].Points.AddXY(SineWaveIndex, (A * Math.Sin(2 * Math.PI * fw * SineWaveIndex)) + (N * Math.Sin(2 * Math.PI * fn * SineWaveIndex)));
                SineWaveIndex += check;
            }
        }
    }
}
