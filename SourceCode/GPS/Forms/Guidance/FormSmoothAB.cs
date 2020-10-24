using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormSmoothAB : Form
    {
        //class variables
        private readonly FormGPS mf;

        private int smoothCount = 20;

        public FormSmoothAB(Form callingForm)
        {
            //get copy of the calling main form
            Owner = mf = callingForm as FormGPS;
            InitializeComponent();

            btnCancel.Text = String.Get("gsForNow");
            btnSave.Text = String.Get("gsToFile");

            Text = String.Get("gsSmoothABCurve");
            mf.CurveLines.CurrentEditLine = mf.CurveLines.CurrentLine;
        }

        private void FormSmoothAB_Load(object sender, EventArgs e)
        {
            mf.CurveLines.isSmoothWindowOpen = true;
            smoothCount = 20;
            lblSmooth.Text = "**";
        }

        private void BtnNorth_MouseDown(object sender, MouseEventArgs e)
        {
            if (smoothCount++ > 100) smoothCount = 100;
            mf.CurveLines.SmoothAB(smoothCount * 2);
            lblSmooth.Text = smoothCount.ToString();
        }

        private void BtnSouth_MouseDown(object sender, MouseEventArgs e)
        {
            if (smoothCount-- < 2) smoothCount = 2;
            mf.CurveLines.SmoothAB(smoothCount * 2);
            lblSmooth.Text = smoothCount.ToString();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            mf.CurveLines.isSmoothWindowOpen = false;
            mf.CurveLines.smooList.Clear();
            Close();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            mf.CurveLines.isSmoothWindowOpen = false;
            mf.CurveLines.SaveSmoothList();
            mf.CurveLines.smooList.Clear();


            mf.FileSaveCurveLines();

            Close();
        }
    }
}