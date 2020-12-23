using System;
using System.Drawing;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormTreePlant : Form
    {
        private readonly FormGPS mf;
        private double lastDist;
        private bool wasRed, isRunning;
        private int trees;

        public FormTreePlant(Form callingForm)
        {
            Owner = mf = callingForm as FormGPS;

            //winform initialization
            InitializeComponent();

            this.Text = String.Get("gsTreePlantControl");

            //Label
            label12.Text = String.Get("gsSpacing");
            label1.Text = String.Get("gsStep");
            label3.Text = String.Get("gsTrees");

            //Button
            btnZeroDistance.Text = String.Get("gsBegin");
            btnStop.Text = String.Get("gsDone");
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            if (mf.autoBtnState != FormGPS.btnStates.Off)
            {
                mf.autoBtnState = FormGPS.btnStates.Off;
                mf.btnSection_Update();
            }
            Properties.Settings.Default.setDistance_TreeSpacing = mf.vehicle.treeSpacing;
            Properties.Settings.Default.Save();
            mf.vehicle.treeSpacing = 0;
            Close();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (isRunning)
            {
                lblDistanceTree.Text = ((UInt16)mf.treeSpacingCounter).ToString();
                if (lastDist > mf.treeSpacingCounter)
                {
                    //lblSpacing.Text = mf.vehicle.treeSpacing.ToString();
                    wasRed = !wasRed;
                    trees++;
                    if (wasRed) btnZeroDistance.BackColor = Color.DarkSeaGreen;
                    else btnZeroDistance.BackColor = Color.LightGreen;
                }
                btnZeroDistance.Text = "Stop";
            }
            else
            {
                btnZeroDistance.Text = "Start";
            }

            lblStepDistance.Text = (mf.distanceCurrentStepFix * 100).ToString("N1");
            lblSpeed.Text = mf.pn.speed.ToString("N1");
            lblTrees.Text = trees.ToString();
            lastDist = mf.treeSpacingCounter;
        }

        private void BtnZeroDistance_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                if (mf.autoBtnState != FormGPS.btnStates.Off)
                {
                    mf.autoBtnState = FormGPS.btnStates.Off;
                    mf.btnSection_Update();
                }
                btnZeroDistance.BackColor = Color.OrangeRed;
            }
            else
            {
                trees = 0;
                if (mf.autoBtnState != FormGPS.btnStates.On)
                {
                    mf.autoBtnState = FormGPS.btnStates.On;
                    mf.btnSection_Update();
                }
                btnZeroDistance.BackColor = Color.LightGreen;
            }
            lastDist = 0;
            mf.treeSpacingCounter = 0;
            mf.distanceCurrentStepFix = 0;
            lblDistanceTree.Text = 0.ToString();
            lblStepDistance.Text = (mf.distanceCurrentStepFix * 100).ToString("N1");

            isRunning = !isRunning;
        }

        private void TboxTreeSpacing_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(1, 50, mf.vehicle.treeSpacing, this, mf.Decimals, true, mf.Unit2Mtr, mf.Mtr2Unit))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxTreeSpacing.Text = ((mf.vehicle.treeSpacing = form.ReturnValue) * mf.Mtr2Unit).ToString(mf.GuiFix);
                }
            }
            btnStop.Focus();
        }

        private void FormTreePlant_Load(object sender, EventArgs e)
        {
            if (mf.autoBtnState != FormGPS.btnStates.Off)
            {
                mf.autoBtnState = FormGPS.btnStates.Off;
                mf.btnSection_Update();
            }

            TboxTreeSpacing.Text = mf.vehicle.treeSpacing.ToString();

            lastDist = 0;
            mf.treeSpacingCounter = 0;
            trees = 0;
            isRunning = false;
            btnZeroDistance.Text = "Start";
            btnZeroDistance.BackColor = Color.OrangeRed;
        }
    }
}
