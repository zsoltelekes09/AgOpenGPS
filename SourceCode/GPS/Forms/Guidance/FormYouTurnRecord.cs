using AgOpenGPS.Properties;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormYouTurnRecord : Form
    {
        //properties
        private readonly FormGPS mf;

        public FormYouTurnRecord(Form callingForm)
        {
            Owner = mf = callingForm as FormGPS;
            InitializeComponent();
            btnStop.Text = String.Get("gsDone");
            btnRecord.Text = String.Get("gsRecord");
            label1.Text = String.Get("gsTurnRIGHTwhilerecording");
            this.Text = String.Get("gsYouTurnRecorder");
        }

        private void BtnRecord_Click(object sender, EventArgs e)
        {
            btnRecord.ForeColor = Color.Red;
            if (mf.yt.youFileList.Count > 0) mf.yt.youFileList.Clear();
            mf.yt.isRecordingCustomYouTurn = true;
            btnRecord.Enabled = false;
            btnStop.Enabled = true;
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            //go make the list
            mf.yt.isRecordingCustomYouTurn = false;

            //first one is the reference the rest are subtracted from, remove it.
            mf.yt.youFileList.RemoveAt(0);

            int numShapePoints = mf.yt.youFileList.Count;
            int i;
            Vec2[] pt = new Vec2[numShapePoints];

            //put the driven list into an array
            for (i = 0; i < numShapePoints; i++)
            {
                pt[i].Easting = mf.yt.youFileList[i].Easting;
                pt[i].Northing = mf.yt.youFileList[i].Northing;
            }

            //empty out the youFileList
            mf.yt.youFileList.Clear();

            //rotate pattern to match AB Line heading
            double head = (mf.ABLines.CurrentLine < mf.ABLines.ABLines.Count && mf.ABLines.CurrentLine > -1) ? mf.ABLines.ABLines[mf.ABLines.CurrentLine].Heading : 0;
            for (i = 0; i < pt.Length; i++)
            {
                //since we want to unwind the heading, we go not negative for heading unlike GPS circle
                double xr, yr;
                xr = (Math.Cos(head) * pt[i].Easting) - (Math.Sin(head) * pt[i].Northing);
                yr = (Math.Sin(head) * pt[i].Easting) + (Math.Cos(head) * pt[i].Northing);

                //update the array
                pt[i].Easting = xr;
                pt[i].Northing = yr;
            }

            //scale the drawing to match exactly the ABLine width
            double adjustFactor = pt[pt.Length - 1].Easting;

            adjustFactor = (mf.Guidance.WidthMinusOverlap + mf.Guidance.GuidanceOffset) / adjustFactor;

            for (i = 0; i < pt.Length; i++)
            {
                pt[i].Easting *= adjustFactor;
                pt[i].Northing *= adjustFactor;
            }

            // 2nd pass scale it so coords are based on 10m
            //last point is the width
            adjustFactor = pt[pt.Length - 1].Easting;
            adjustFactor = 10.0 / adjustFactor;
            for (i = 0; i < pt.Length; i++)
            {
                pt[i].Easting *= adjustFactor;
                pt[i].Northing *= adjustFactor;
                mf.yt.youFileList.Add(pt[i]);
            }

            //Save the file.
            string Data = "";
            Data += mf.yt.youFileList[0].Easting + "," + mf.yt.youFileList[0].Northing;
            for (i = 1; i < mf.yt.youFileList.Count; i++)
                Data += "\r\n" + mf.yt.youFileList[i].Easting + "," + mf.yt.youFileList[i].Northing;

            Settings.Default.Custom = Data;
            Settings.Default.Save();

            mf.yt.LoadYouTurnShapeFromData(Settings.Default.Custom);
            Close();
        }

        private void FormYouTurnRecord_Load(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
        }
    }
}