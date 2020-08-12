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
    public partial class FormTram : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf;

        private double snapAdj = 0;

        public FormTram(Form callingForm)
        {
            //get copy of the calling main form
            Owner = mf = callingForm as FormGPS;

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
            snapAdj = (Math.Round((mf.Guidance.GuidanceWidth - mf.Guidance.GuidanceOverlap) / 2.0, 3));
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

            mf.ABLine.BuildTram();

            //cboxTramPassEvery.SelectedIndexChanged -= cboxTramPassEvery_SelectedIndexChanged;
            //cboxTramPassEvery.Text = Properties.Vehicle.Default.setTram_Skips.ToString();
            //cboxTramPassEvery.SelectedIndexChanged += cboxTramPassEvery_SelectedIndexChanged;
            //mf.ABLine.tramPassEvery = Properties.Vehicle.Default.setTram_Skips;

            //cboxTramBasedOn.SelectedIndexChanged -= cboxTramBasedOn_SelectedIndexChanged;
            //cboxTramBasedOn.Text = Properties.Vehicle.Default.setTram_BasedOn.ToString();
            //cboxTramBasedOn.SelectedIndexChanged += cboxTramBasedOn_SelectedIndexChanged;
            //mf.ABLine.tramBasedOn = Properties.Vehicle.Default.setTram_BasedOn;

            mf.ABLine.isEditing = true;
            mf.layoutPanelRight.Enabled = false;

            //if off, turn it on because they obviously want a tram.
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

        private void BtnExit_Click(object sender, EventArgs e)
        {
            int idx = mf.ABLine.numABLineSelected - 1;

            if (idx >= 0)
            {
                mf.ABLine.lineArr[idx].heading = mf.ABLine.abHeading;
                //calculate the new points for the reference line and points
                mf.ABLine.lineArr[idx].origin.easting = mf.ABLine.refPoint1.easting;
                mf.ABLine.lineArr[idx].origin.northing = mf.ABLine.refPoint1.northing;

                //sin x cos z for endpoints, opposite for additional lines
                mf.ABLine.lineArr[idx].ref1.easting = mf.ABLine.lineArr[idx].origin.easting - (Math.Sin(mf.ABLine.lineArr[idx].heading) *   1600.0);
                mf.ABLine.lineArr[idx].ref1.northing = mf.ABLine.lineArr[idx].origin.northing - (Math.Cos(mf.ABLine.lineArr[idx].heading) * 1600.0);
                mf.ABLine.lineArr[idx].ref2.easting = mf.ABLine.lineArr[idx].origin.easting + (Math.Sin(mf.ABLine.lineArr[idx].heading) *   1600.0);
                mf.ABLine.lineArr[idx].ref2.northing = mf.ABLine.lineArr[idx].origin.northing + (Math.Cos(mf.ABLine.lineArr[idx].heading) * 1600.0);
            }

            mf.FileSaveABLines();

            mf.ABLine.moveDistance = 0;
            mf.ABLine.isEditing = false;
            mf.layoutPanelRight.Enabled = true;
            mf.panelDrag.Visible = false;
            mf.offX = 0;
            mf.offY = 0;

            Close();
        }

        private void BtnLeft_Click(object sender, EventArgs e)
        {
            double dist = -0.1;
            mf.ABLine.MoveABLine(dist);
            mf.ABLine.BuildTram();
        }

        private void BtnRight_Click(object sender, EventArgs e)
        {
            double dist = 0.1;
            mf.ABLine.MoveABLine(dist);
            mf.ABLine.BuildTram();
        }

        private void BtnAdjLeft_Click(object sender, EventArgs e)
        {
            mf.ABLine.MoveABLine(-snapAdj);
            mf.ABLine.BuildTram();
        }

        private void BtnAdjRight_Click(object sender, EventArgs e)
        {
            mf.ABLine.BuildTram();
            mf.ABLine.MoveABLine(snapAdj);
        }


        //determine mins maxs of patches and whole field.
        private void NudSnapAdj_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender, this);
            btnCancel.Focus();
        }

        private void NudPasses_ValueChanged(object sender, EventArgs e)
        {
            mf.tram.passes = (int)nudPasses.Value;
            Properties.Settings.Default.setTram_passes = mf.tram.passes;
            Properties.Settings.Default.Save();
            mf.ABLine.BuildTram();
        }

        private void NudPasses_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender, this);
            btnCancel.Focus();
            mf.ABLine.BuildTram();
        }

        private void BtnCreateTramLines_Click(object sender, EventArgs e)
        {
            mf.ABLine.BuildTram();
        }

        private void NudOffset_ValueChanged(object sender, EventArgs e)
        {
            mf.tram.abOffset = (double)nudOffset.Value;
            Properties.Settings.Default.setTram_offset = mf.tram.abOffset;
            Properties.Settings.Default.Save();
            mf.ABLine.BuildTram();
        }

        private void NudOffset_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender, this);
            btnCancel.Focus();
        }

        private void BtnSwapAB_Click(object sender, EventArgs e)
        {
            mf.ABLine.abHeading = (mf.ABLine.abHeading + Math.PI) % Glm.twoPI;

            mf.ABLine.refABLineP1.easting = mf.ABLine.refPoint1.easting - (Math.Sin(mf.ABLine.abHeading) *   1600.0);
            mf.ABLine.refABLineP1.northing = mf.ABLine.refPoint1.northing - (Math.Cos(mf.ABLine.abHeading) * 1600.0);
                                                                                                             
            mf.ABLine.refABLineP2.easting = mf.ABLine.refPoint1.easting + (Math.Sin(mf.ABLine.abHeading) *   1600.0);
            mf.ABLine.refABLineP2.northing = mf.ABLine.refPoint1.northing + (Math.Cos(mf.ABLine.abHeading) * 1600.0);

            mf.ABLine.refPoint2.easting = mf.ABLine.refABLineP2.easting;
            mf.ABLine.refPoint2.northing = mf.ABLine.refABLineP2.northing;

            mf.ABLine.BuildTram();
        }

        private void BtnTriggerDistanceUp_MouseDown(object sender, MouseEventArgs e)
        {
            nudPasses.UpButton();
            //mf.ABLine.BuildTram();
        }

        private void BtnTriggerDistanceDn_MouseDown(object sender, MouseEventArgs e)
        {
            nudPasses.DownButton();
            //mf.ABLine.BuildTram();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            mf.ABLine.tramArr?.Clear();
            mf.ABLine.tramList?.Clear();
            mf.tram.tramBndArr?.Clear();

            //mf.ABLine.tramPassEvery = 0;
            //mf.ABLine.tramBasedOn = 0;
            mf.ABLine.isEditing = false;
            mf.layoutPanelRight.Enabled = true;
            mf.panelDrag.Visible = false;
            mf.offX = 0;
            mf.offY = 0;

            mf.tram.displayMode = 0;
            Close();
        }

        //private void cboxTramBasedOn_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    mf.ABLine.tramBasedOn = cboxTramBasedOn.SelectedIndex;
        //    Properties.Vehicle.Default.setTram_BasedOn = mf.ABLine.tramBasedOn;
        //    Properties.Vehicle.Default.Save();
        //}

        //private void cboxTramPassEvery_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (cboxTramPassEvery.SelectedIndex > 0)
        //        mf.ABLine.tramPassEvery = cboxTramPassEvery.SelectedIndex;
        //    else mf.ABLine.tramPassEvery = 0;
        //    Properties.Vehicle.Default.setTram_Skips = cboxTramPassEvery.SelectedIndex;
        //    Properties.Vehicle.Default.Save();
        //}

        private void NudSnapAdj_ValueChanged(object sender, EventArgs e)
        {
            snapAdj = (double)nudSnapAdj.Value;
            Properties.Settings.Default.setTram_snapAdj = snapAdj;
            Properties.Settings.Default.Save();
            mf.ABLine.BuildTram();
        }

        private void NudEqWidth_ValueChanged(object sender, EventArgs e)
        {
            mf.tram.tramWidth  = (double)nudEqWidth.Value;
            Properties.Settings.Default.setTram_eqWidth = mf.tram.tramWidth;
            Properties.Settings.Default.Save();
            mf.ABLine.BuildTram();

        }

        private void NudEqWidth_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender, this);
            btnCancel.Focus();
        }

        private void NudWheelSpacing_ValueChanged(object sender, EventArgs e)
        {
            mf.tram.wheelTrack = (double)nudWheelSpacing.Value;
            mf.tram.halfWheelTrack = mf.tram.wheelTrack * 0.5;
            Properties.Settings.Default.setTram_wheelSpacing = mf.tram.wheelTrack;
            Properties.Settings.Default.Save();
            mf.ABLine.BuildTram();

        }

        private void NudWheelSpacing_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender, this);
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