using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormTramCurve : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf;

        private double snapAdj = 0;

        public FormTramCurve(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();

            this.Text = gStr.gsTramLines;
            lblSmallSnapRight.Text = gStr.gsWidth + " (m)";
            label1.Text = gStr.gsTrack + " (m)";
            label3.Text = gStr.gsPasses;

            nudWheelSpacing.Controls[0].Enabled = false;
            nudSnapAdj.Controls[0].Enabled = false;
            nudEqWidth.Controls[0].Enabled = false;
            nudPasses.Controls[0].Enabled = false;
            nudOffset.Controls[0].Enabled = false;
        }

        private void FormTram_Load(object sender, EventArgs e)
        { 
            nudSnapAdj.ValueChanged -= NudSnapAdj_ValueChanged;
            snapAdj = (Math.Round((mf.tool.ToolWidth - mf.tool.toolOverlap)/2.0,3));
            nudSnapAdj.Value = (decimal)snapAdj;
            nudSnapAdj.ValueChanged += NudSnapAdj_ValueChanged;

            nudEqWidth.ValueChanged -= NudEqWidth_ValueChanged;
            nudEqWidth.Value = (decimal)Properties.Settings.Default.setTram_eqWidth;
            nudEqWidth.ValueChanged += NudEqWidth_ValueChanged;

            nudWheelSpacing.ValueChanged -= NudWheelSpacing_ValueChanged;
            nudWheelSpacing.Value = (decimal)Properties.Settings.Default.setTram_wheelSpacing;
            nudWheelSpacing.ValueChanged += NudWheelSpacing_ValueChanged;

            nudPasses.ValueChanged -= NudPasses_ValueChanged;
            nudPasses.Value = Properties.Settings.Default.setTram_passes;
            nudPasses.ValueChanged += NudPasses_ValueChanged;

            nudOffset.ValueChanged -= NudOffset_ValueChanged;
            nudOffset.Value = (decimal)snapAdj;
            mf.tram.abOffset = snapAdj;
            nudOffset.ValueChanged += NudOffset_ValueChanged;

            mf.curve.BuildTram();
            mf.curve.isEditing = true;
            mf.layoutPanelRight.Enabled = false;

            if (mf.tram.displayMode == 0) mf.tram.displayMode = 1;
            switch (mf.tram.displayMode)
            {
                case 0:
                    btnMode.Image = Properties.Resources.TramOff;
                    break;
                case 1:
                    btnMode.Image = Properties.Resources.TramAll;
                    break;
                case 2:
                    btnMode.Image = Properties.Resources.TramLines;
                    break;
                case 3:
                    btnMode.Image = Properties.Resources.TramOuter;
                    break;

                default:
                    break;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            //mf.ABLine.moveDistance = 0;
            mf.curve.isEditing = false;
            mf.layoutPanelRight.Enabled = true;
            mf.panelDrag.Visible = false;

            mf.offX = 0;
            mf.offY = 0;
            if (mf.curve.refList.Count > 0)
            {
                //array number is 1 less since it starts at zero
                int idx = mf.curve.numCurveLineSelected - 1;

                //mf.curve.curveArr[idx].Name = textBox1.Text.Trim();
                if (idx >= 0)
                {
                    mf.curve.curveArr[idx].aveHeading = mf.curve.aveLineHeading;
                    mf.curve.curveArr[idx].curvePts.Clear();
                    //write out the Curve Points
                    foreach (var item in mf.curve.refList)
                    {
                        mf.curve.curveArr[idx].curvePts.Add(item);
                    }
                }

                //save entire list
                mf.FileSaveCurveLines();
                mf.curve.moveDistance = 0;
            }

            Close();

        }

        private void BtnLeft_Click(object sender, EventArgs e)
        {
            double dist = -0.1;
            mf.curve.MoveABCurve(dist);
            mf.curve.BuildTram();
        }

        private void BtnRight_Click(object sender, EventArgs e)
        {
            double dist = 0.1;
            mf.curve.MoveABCurve(dist);
            mf.curve.BuildTram();
        }

        private void BtnAdjLeft_Click(object sender, EventArgs e)
        {
            mf.curve.MoveABCurve(-snapAdj);
            mf.curve.BuildTram();
        }

        private void BtnAdjRight_Click(object sender, EventArgs e)
        {
            mf.curve.MoveABCurve(snapAdj);
            mf.curve.BuildTram();
        }

        private void NudSnapAdj_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender);
            btnCancel.Focus();
        }

        private void NudPasses_ValueChanged(object sender, EventArgs e)
        {
            mf.tram.passes = (int)nudPasses.Value;
            Properties.Settings.Default.setTram_passes = mf.tram.passes;
            Properties.Settings.Default.Save();
            mf.curve.BuildTram();
        }

        private void NudPasses_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender);
            btnCancel.Focus();
        }

        private void NudOffset_ValueChanged(object sender, EventArgs e)
        {
            mf.tram.abOffset = (double)nudOffset.Value;
            Properties.Settings.Default.setTram_offset = mf.tram.abOffset;
            Properties.Settings.Default.Save();
            mf.curve.BuildTram();
        }

        private void NudOffset_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender);
            btnCancel.Focus();
        }

        private void BtnSwapAB_Click(object sender, EventArgs e)
        {
            int cnt = mf.curve.refList.Count;
            if (cnt > 0)
            {
                mf.curve.refList.Reverse();

                vec3[] arr = new vec3[cnt];
                cnt--;
                mf.curve.refList.CopyTo(arr);
                mf.curve.refList.Clear();

                mf.curve.aveLineHeading = (mf.curve.aveLineHeading + Math.PI) % Glm.twoPI;

                for (int i = 1; i < cnt; i++)
                {
                    vec3 pt3 = arr[i];
                    pt3.heading += Math.PI;
                    pt3.heading %= Glm.twoPI;
                    if (pt3.heading < 0) pt3.heading += Glm.twoPI;
                    mf.curve.refList.Add(pt3);
                }
            }
            mf.curve.BuildTram();
        }

        private void BtnTriggerDistanceUp_MouseDown(object sender, MouseEventArgs e)
        {
            nudPasses.UpButton();
            //mf.curve.BuildTram();
        }

        private void BtnTriggerDistanceDn_MouseDown(object sender, MouseEventArgs e)
        {
            nudPasses.DownButton();
            //mf.curve.BuildTram();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            mf.curve.tramArr?.Clear();
            mf.curve.tramList?.Clear();
            mf.tram.tramBndArr?.Clear();

            mf.curve.isEditing = false;
            mf.layoutPanelRight.Enabled = true;
            mf.panelDrag.Visible = false;
            mf.offX = 0;
            mf.offY = 0;

            mf.tram.displayMode = 0;
            Close();
        }

        private void NudSnapAdj_ValueChanged(object sender, EventArgs e)
        {
            snapAdj = (double)nudSnapAdj.Value;
            Properties.Settings.Default.setTram_snapAdj = snapAdj;
            Properties.Settings.Default.Save();
            mf.curve.BuildTram();
        }

        private void NudEqWidth_ValueChanged(object sender, EventArgs e)
        {
            mf.tram.tramWidth  = (double)nudEqWidth.Value;
            Properties.Settings.Default.setTram_eqWidth = mf.tram.tramWidth;
            Properties.Settings.Default.Save();
            mf.curve.BuildTram();

        }

        private void NudEqWidth_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender);
            btnCancel.Focus();
        }

        private void NudWheelSpacing_ValueChanged(object sender, EventArgs e)
        {
            mf.tram.wheelTrack = (double)nudWheelSpacing.Value;
            mf.tram.halfWheelTrack = mf.tram.wheelTrack * 0.5;

            Properties.Settings.Default.setTram_wheelSpacing = mf.tram.wheelTrack;
            Properties.Settings.Default.Save();

            mf.curve.BuildTram();
        }

        private void NudWheelSpacing_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender);
            btnCancel.Focus();        
        }

        private void BtnMode_Click(object sender, EventArgs e)
        {
            mf.tram.displayMode++;
            if (mf.tram.displayMode > 3) mf.tram.displayMode = 0;

            switch (mf.tram.displayMode)
            {
                case 0:
                    btnMode.Image = Properties.Resources.TramOff;
                    break;
                case 1:
                    btnMode.Image = Properties.Resources.TramAll;
                    break;
                case 2:
                    btnMode.Image = Properties.Resources.TramLines;
                    break;
                case 3:
                    btnMode.Image = Properties.Resources.TramOuter;
                    break;

                default:
                    break;
            }

        }
    }
}