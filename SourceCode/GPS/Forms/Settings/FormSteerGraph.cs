using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace AgOpenGPS
{
    public partial class FormSteerGraph : Form
    {
        private readonly FormGPS mf;

        //chart data
        private string dataSteerAngle = "0";

        private string dataPWM = "-1";

        public FormSteerGraph(Form callingForm)
        {
            Owner = mf = callingForm as FormGPS;
            InitializeComponent();

            this.label5.Text = String.Get("gsSetPoint");
            this.label1.Text = String.Get("gsActual");

            this.Text = String.Get("gsSteerChart");
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = (int)((1 / (double)mf.HzTime) * 1000);
            DrawChart();
        }

        private void DrawChart()
        {
            //word 0 - steerangle, 1 - pwmDisplay
            double SteerAngleDisp = mf.actualSteerAngleDisp;
            dataSteerAngle = SteerAngleDisp.ToString();
            double LineSteerAngle = mf.guidanceLineSteerAngle;
            dataPWM = LineSteerAngle.ToString();

            lblSteerAng.Text = mf.ActualSteerAngle;
            lblPWM.Text = mf.SetSteerAngle;

            //chart data
            Series s = unoChart.Series["S"];
            Series w = unoChart.Series["PWM"];
            double nextX = 1;
            double nextX5 = 1;

            if (s.Points.Count > 0) nextX = s.Points[s.Points.Count - 1].XValue + 1;
            if (w.Points.Count > 0) nextX5 = w.Points[w.Points.Count - 1].XValue + 1;

            unoChart.Series["S"].Points.AddXY(nextX, dataSteerAngle);
            unoChart.Series["PWM"].Points.AddXY(nextX5, dataPWM);

            //if (isScroll)
            {
                while (s.Points.Count > 30)
                {
                    s.Points.RemoveAt(0);
                }
                while (w.Points.Count > 30)
                {
                    w.Points.RemoveAt(0);
                }
                unoChart.ResetAutoValues();
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}