//Please, if you use this, share the improvements

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormToolSettings : Form
    {
        //class variables
        private readonly FormGPS mf;

        private double toolOverlap, toolOffset, toolTurnOffDelay, toolLookAheadOn, toolLookAheadOff, MappingOnDelay, MappingOffDelay;
        private double HitchLength, TankWheelLength, TankHitchLength, ToolWheelLength, ToolHitchLength, SectionWidth, cutoffSpeed;
        private bool isToolTrailing, isToolBehindPivot, isToolTBT;
        public int Here = 0, numberOfSections, MinApplied;


        public List<TextBox> Section = new List<TextBox>();
        public List<Label> SectionText = new List<Label>();


        //constructor
        public FormToolSettings(Form callingForm, int page)
        {
            //get copy of the calling main form
            Owner = mf = callingForm as FormGPS;
            InitializeComponent();

            //Language keys
            gboxAttachment.Text = String.Get("gsAttachmentStyle");
            tabHitch.Text = String.Get("gsHitch");
            tabConfig.Text = String.Get("gsConfiguration");
            tabSections.Text = String.Get("gsSection");
            tabSettings.Text = String.Get("gsSettings");

            label23.Text = String.Get("gsTurnOffDelaySecs");
            label8.Text = String.Get("gsTurnOffAheadSecs");
            label3.Text = String.Get("gsTurnOnAheadSecs");
            label41.Text = String.Get("gsMinUnapplied");
            label2.Text = String.Get("gs#OfSections");
            label4.Text = String.Get("gsSectionWidth");

            label30.Text = String.Get("gsSectionsTurnOffBelow");

            label17.Text = String.Get("gsMeasurementsIn");
            label16.Text = String.Get("gsToolWidth");

            Text = String.Get("gsToolSettings");

            if (mf.isMetric)
            {
                lblInchesCm.Text = String.Get("gsCentimeters");
                lblSecTotalWidthFeet.Visible = false;
                lblSecTotalWidthInches.Visible = false;
                lblSecTotalWidthMeters.Visible = true;
                lblDoNotExceed.Text = "* < 7500 cm*";
                lblTurnOffBelowUnits.Text = String.Get("gsKMH");
            }
            else
            {
                lblInchesCm.Text = String.Get("gsInches");
                lblSecTotalWidthFeet.Visible = true;
                lblSecTotalWidthInches.Visible = true;
                lblSecTotalWidthMeters.Visible = false;
                lblDoNotExceed.Text = "* < 2952,7559 inches *";
                lblTurnOffBelowUnits.Text = String.Get("gsMPH");
            }
            //select the page as per calling menu or button from mainGPS form
            tabControl1.SelectedIndex = page;
        }

        //do any field initializing for form here
        private void FormToolSettings_Load(object sender, EventArgs e)
        {
            mf.CheckToolSettings();

            //HitchTab
            TboxHitchLength.Text = (HitchLength = Math.Round(Math.Abs(Properties.Vehicle.Default.ToolSettings[Here].HitchLength) * mf.m2MetImp, mf.decimals)).ToString();
            TboxHitchLength.CheckValue(ref HitchLength, 0, Math.Round(20 * mf.m2MetImp, mf.decimals));
            TboxTankWheelLength.Text = (TankWheelLength = Math.Round(Math.Abs(Properties.Vehicle.Default.ToolSettings[Here].TankWheelLength) * mf.m2MetImp, mf.decimals)).ToString();
            TboxTankWheelLength.CheckValue(ref TankWheelLength, 0, Math.Round(20 * mf.m2MetImp, mf.decimals));
            TboxTankHitchLength.Text = (TankHitchLength = Math.Round(Math.Abs(Properties.Vehicle.Default.ToolSettings[Here].TankHitchLength) * mf.m2MetImp, mf.decimals)).ToString();
            TboxTankHitchLength.CheckValue(ref TankHitchLength, 0, Math.Round(20 * mf.m2MetImp, mf.decimals));
            TboxToolWheelLength.Text = (ToolWheelLength = Math.Round(Math.Abs(Properties.Vehicle.Default.ToolSettings[Here].ToolWheelLength) * mf.m2MetImp, mf.decimals)).ToString();
            TboxToolWheelLength.CheckValue(ref ToolWheelLength, 0, Math.Round(20 * mf.m2MetImp, mf.decimals));
            TboxToolHitchLength.Text = (ToolHitchLength = Math.Round(Math.Abs(Properties.Vehicle.Default.ToolSettings[Here].ToolHitchLength) * mf.m2MetImp, mf.decimals)).ToString();
            TboxToolHitchLength.CheckValue(ref ToolHitchLength, 0, Math.Round(20 * mf.m2MetImp, mf.decimals));

            //SettingsTab
            TboxOverlap.Text = (toolOverlap = Math.Round(Properties.Vehicle.Default.GuidanceOverlap * mf.m2MetImp, mf.decimals)).ToString();
            TboxOverlap.CheckValue(ref toolOverlap, Math.Round(-25 * mf.m2MetImp, mf.decimals), Math.Round(25 * mf.m2MetImp, mf.decimals));
            TboxOffset.Text = (toolOffset = Math.Round(Properties.Vehicle.Default.GuidanceOffset * mf.m2MetImp, mf.decimals)).ToString();
            TboxOffset.CheckValue(ref toolOffset, Math.Round(-25 * mf.m2MetImp, mf.decimals), Math.Round(25 * mf.m2MetImp, mf.decimals));
            TboxCutoffSpeed.Text = (cutoffSpeed = Math.Round(Properties.Vehicle.Default.ToolSettings[Here].SlowSpeedCutoff / mf.cutoffMetricImperial,3)).ToString("0.0#");
            TboxCutoffSpeed.CheckValue(ref cutoffSpeed, 0, Math.Round(30 / mf.cutoffMetricImperial, mf.decimals));




            TboxLookAheadOn.Text = (toolLookAheadOn = Properties.Vehicle.Default.ToolSettings[Here].LookAheadOn).ToString("0.0#");
            TboxMappingOnDelay.Text = (MappingOnDelay = Properties.Vehicle.Default.ToolSettings[Here].MappingOnDelay).ToString("0.0#");
            TboxLookAheadOff.Text = (toolLookAheadOff = Properties.Vehicle.Default.ToolSettings[Here].LookAheadOff).ToString("0.0#");
            TboxMappingOffDelay.Text = (MappingOffDelay = Properties.Vehicle.Default.ToolSettings[Here].MappingOffDelay).ToString("0.0#");
            TboxTurnOffDelay.Text = (toolTurnOffDelay = Properties.Vehicle.Default.ToolSettings[Here].TurnOffDelay).ToString("0.0#");

            //SectionsTab
            TboxSectionWidth.Text = (SectionWidth = Math.Round(Properties.Vehicle.Default.setTool_defaultSectionWidth * mf.m2MetImp, mf.decimals)).ToString();
            TboxMinApplied.Text = (MinApplied = Properties.Vehicle.Default.ToolSettings[Here].MinApplied).ToString();
            TboxNumSections.Text = (numberOfSections = Properties.Vehicle.Default.ToolSettings[Here].Sections.Count).ToString();


            isToolBehindPivot = Properties.Vehicle.Default.ToolSettings[Here].BehindPivot;
            isToolTrailing = Properties.Vehicle.Default.ToolSettings[Here].Trailing;
            isToolTBT = Properties.Vehicle.Default.ToolSettings[Here].TBT;

            rbtnTBT.Checked = false;
            rbtnTrailing.Checked = false;
            rbtnFixedRear.Checked = false;
            rbtnFront.Checked = false;

            if (!isToolBehindPivot) rbtnFront.Checked = true;
            else if (isToolTBT) rbtnTBT.Checked = true;
            else if (!isToolTrailing) rbtnFixedRear.Checked = true;
            else rbtnTrailing.Checked = true;

            btnChangeAttachment.Enabled = false;

            FixRadioButtonsAndImages();


            //based on number of sections and values update the page before displaying
            UpdateNumberOfSections();

            btnChangeAttachment.BackColor = Color.Transparent;
            btnChangeAttachment.Enabled = false;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            mf.CheckToolSettings();
            double Width = 0;
            for (int i = 0; i < Section.Count; i++)
            {
                Width += Convert.ToDouble(Section[i].Text) * mf.metImp2m;//go back to meters
            }

            if (Width > 75) return;

            //Tool  ------------------------------------------------------------------------------------------
            if (Here > Properties.Vehicle.Default.ToolSettings.Count) Here = 0;
            Properties.Vehicle.Default.ToolSettings[Here].LookAheadOn = toolLookAheadOn;
            Properties.Vehicle.Default.ToolSettings[Here].LookAheadOff = toolLookAheadOff;
            Properties.Vehicle.Default.ToolSettings[Here].TurnOffDelay = toolTurnOffDelay;
            Properties.Vehicle.Default.ToolSettings[Here].MappingOnDelay = MappingOnDelay;
            Properties.Vehicle.Default.ToolSettings[Here].MappingOffDelay = MappingOffDelay;

            Properties.Vehicle.Default.ToolSettings[Here].HitchLength = Math.Round(HitchLength * mf.metImp2m * (isToolBehindPivot ? -1 : 1), 2);
            Properties.Vehicle.Default.ToolSettings[Here].TankWheelLength = Math.Round(TankWheelLength * -mf.metImp2m, 2);
            Properties.Vehicle.Default.ToolSettings[Here].TankHitchLength = Math.Round(TankHitchLength * -mf.metImp2m, 2);
            Properties.Vehicle.Default.ToolSettings[Here].ToolWheelLength = Math.Round(ToolWheelLength * -mf.metImp2m, 2);
            Properties.Vehicle.Default.ToolSettings[Here].ToolHitchLength = Math.Round(ToolHitchLength * -mf.metImp2m, 2);

            Properties.Vehicle.Default.ToolSettings[Here].Trailing = isToolTrailing;
            Properties.Vehicle.Default.ToolSettings[Here].BehindPivot = isToolBehindPivot;
            Properties.Vehicle.Default.ToolSettings[Here].TBT = isToolTBT;

            Properties.Vehicle.Default.ToolSettings[Here].MinApplied = MinApplied;
            Properties.Vehicle.Default.ToolSettings[Here].SlowSpeedCutoff = Math.Round(cutoffSpeed * mf.cutoffMetricImperial, 2);
            Properties.Vehicle.Default.GuidanceOverlap = mf.Guidance.GuidanceOverlap = Math.Round(toolOverlap * mf.metImp2m,2);
            Properties.Vehicle.Default.GuidanceOffset = mf.Guidance.GuidanceOffset = Math.Round(toolOffset * mf.metImp2m,2);
            Properties.Vehicle.Default.GuidanceWidth = mf.Guidance.GuidanceWidth = Math.Round(Width, 2);
            mf.Guidance.WidthMinusOverlap = mf.Guidance.GuidanceWidth - mf.Guidance.GuidanceOverlap;

            List<double[]> aa = new List<double[]>();

            Width /= -2;
            Width = Math.Round(Width, 2);
            Width += Math.Round(toolOffset * mf.metImp2m,2);
            //save the values in each spinner for section position widths in settings
            for (int i = 0; i < Section.Count; i++)
            {
                aa.Add(new double[] { Width, Width += Math.Round(Convert.ToDouble(Section[i].Text) * mf.metImp2m,2), 0 });
            }
            Properties.Vehicle.Default.ToolSettings[Here].Sections = aa;

            Properties.Vehicle.Default.setTool_defaultSectionWidth = Math.Round(SectionWidth * mf.metImp2m,2);

            Properties.Vehicle.Default.Save();

            mf.LoadTools();

            //back to FormGPS
            DialogResult = DialogResult.OK;
            Close();
        }
               
        private void RbtnFront_CheckedChanged(object sender, EventArgs e)
        {
            var radioButton = sender as RadioButton;

            // Only do something when the event was raised by the radiobutton 
            // being checked, so we don't do this twice.
            if (radioButton.Checked)
            {
                btnChangeAttachment.Enabled = true;
                btnChangeAttachment.BackColor = SystemColors.ActiveCaption;
            }
        }

        private void FixRadioButtonsAndImages()
        {
            if (rbtnTrailing.Checked)
            {
                isToolBehindPivot = true;
                isToolTrailing = true;
                isToolTBT = false;

                TboxTankWheelLength.Visible = false;
                TboxTankHitchLength.Visible = false;
                TboxToolWheelLength.Visible = true;
                TboxToolHitchLength.Visible = true;

                TboxHitchLength.Left = 384;
                TboxToolWheelLength.Left = 596;
                TboxToolHitchLength.Left = 696;

                tabHitch.BackgroundImage = Properties.Resources.ToolHitchPageTrailing;
            }
            else if (rbtnFixedRear.Checked)
            {
                isToolBehindPivot = true;
                isToolTrailing = false;
                isToolTBT = false;

                TboxTankWheelLength.Visible = false;
                TboxTankHitchLength.Visible = false;
                TboxToolWheelLength.Visible = false;
                TboxToolHitchLength.Visible = false;

                TboxHitchLength.Left = 443;

                tabHitch.BackgroundImage = Properties.Resources.ToolHitchPageRear;
            }
            else if (rbtnFront.Checked)
            {
                isToolBehindPivot = false;
                isToolTrailing = false;
                isToolTBT = false;

                TboxTankWheelLength.Visible = false;
                TboxTankHitchLength.Visible = false;
                TboxToolWheelLength.Visible = false;
                TboxToolHitchLength.Visible = false;

                TboxHitchLength.Left = 384;

                tabHitch.BackgroundImage = Properties.Resources.ToolHitchPageFront;
            }
            else //TBT
            {
                isToolBehindPivot = true;
                isToolTrailing = true;
                isToolTBT = true;

                TboxToolWheelLength.Visible = true;
                TboxToolHitchLength.Visible = true;
                TboxTankWheelLength.Visible = true;
                TboxTankHitchLength.Visible = true;

                TboxHitchLength.Left = 283;
                TboxTankWheelLength.Left = 486;
                TboxTankHitchLength.Left = 586;
                TboxToolWheelLength.Left = 700;
                TboxToolHitchLength.Left = 800;

                tabHitch.BackgroundImage = Properties.Resources.ToolHitchPageTBT;
            }
        }

        private void BtnChangeAttachment_Click(object sender, EventArgs e)
        {
            btnChangeAttachment.Enabled = false;
            btnChangeAttachment.BackColor = Color.Transparent;
            FixRadioButtonsAndImages();
            tabControl1.SelectedTab = tabHitch;
            btnNext.Focus();
        }

        #region Sections //---------------------------------------------------------------

        public void UpdateSpinners()
        {
            double Width = 0;
            for (int i = 0; i < Section.Count; i++)
            {
                Width += Convert.ToDouble(Section[i].Text);
            }
            if (Math.Round(Width, mf.decimals) > Math.Round(75 * mf.m2MetImp, mf.decimals)) SectionPanel.BackColor = Color.Red;
            else SectionPanel.BackColor = SystemColors.Window;

            //update in settings dialog ONLY total tool width
            if (mf.isMetric)
            {
                lblSecTotalWidthMeters.Text = Width.ToString("0") + " cm";
            }
            else
            {
                double toFeet = (double)Width * 0.08333333334;
                lblSecTotalWidthFeet.Text = Convert.ToString((int)toFeet) + "'";
                lblSecTotalWidthInches.Text = Math.Round((toFeet - Math.Truncate(toFeet)) * 12, 0).ToString() + '"';
            }
        }

        private void TboxToolWheelLength_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(0, Math.Round(20 * mf.m2MetImp, mf.decimals), ToolWheelLength, this, mf.isMetric, mf.decimals))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxToolWheelLength.Text = (ToolWheelLength = Math.Round(form.ReturnValue, mf.decimals)).ToString();
                }
            }
            btnCancel.Focus();
        }

        private void TboxTankWheelLength_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(0, Math.Round(20 * mf.m2MetImp, mf.decimals), TankWheelLength, this, mf.isMetric, mf.decimals))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxTankWheelLength.Text = (TankWheelLength = Math.Round(form.ReturnValue, mf.decimals)).ToString();
                }
            }
            btnCancel.Focus();
        }

        private void TboxTankHitchLength_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(0, Math.Round(20 * mf.m2MetImp, mf.decimals), TankHitchLength, this, mf.isMetric, mf.decimals))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxTankHitchLength.Text = (TankHitchLength = Math.Round(form.ReturnValue, mf.decimals)).ToString();
                }
            }
            btnCancel.Focus();
        }

        private void TboxToolHitchLength_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(0, Math.Round(20 * mf.m2MetImp, mf.decimals), ToolHitchLength, this, mf.isMetric, mf.decimals))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxToolHitchLength.Text = (ToolHitchLength = Math.Round(form.ReturnValue, mf.decimals)).ToString();
                }
            }
            btnCancel.Focus();
        }

        private void Section_Enter(object sender, EventArgs e)
        {
            if (sender is TextBox b)
            {
                using (var form = new FormNumeric(Math.Round(0.05 * mf.m2MetImp, mf.decimals), Math.Round(75 * mf.m2MetImp, mf.decimals), Math.Round(Convert.ToDouble(b.Text), mf.decimals), this, mf.isMetric, mf.decimals))
                {
                    var result = form.ShowDialog(this);
                    if (result == DialogResult.OK)
                    {
                        b.Text = Math.Round(form.ReturnValue, mf.decimals).ToString();
                        UpdateSpinners();
                    }
                }
                btnCancel.Focus();
            }
        }

        private void TboxLookAheadOn_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(0.2, 5, Math.Round(toolLookAheadOn,2), this, false,2))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxLookAheadOn.Text = (toolLookAheadOn = Math.Round(form.ReturnValue,2)).ToString("0.0#");

                    if (toolLookAheadOff > (toolLookAheadOn * 0.8))
                    {
                        TboxLookAheadOff.Text = (toolLookAheadOff = Math.Round(toolLookAheadOn * 0.8,2)).ToString("0.0#");
                    }
                }
            }
            btnCancel.Focus();
        }

        private void TboxLookAheadOff_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(0, 4, toolLookAheadOff, this, false,2))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxLookAheadOff.Text = (toolLookAheadOff = Math.Round(form.ReturnValue,2)).ToString("0.0#");

                    if (toolLookAheadOff > (toolLookAheadOn * 0.8))
                    {
                        TboxLookAheadOff.Text = (toolLookAheadOff = Math.Round(toolLookAheadOn * 0.8,2)).ToString("0.0#");
                    }
                    if (toolLookAheadOff > 0)
                    {
                        TboxTurnOffDelay.Text = (toolTurnOffDelay = 0).ToString("0.0#");
                    }
                }
            }
            btnCancel.Focus();
        }

        private void TboxHitchLength_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(0, Math.Round(20 * mf.m2MetImp, mf.decimals), HitchLength, this, mf.isMetric, mf.decimals))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxHitchLength.Text = (HitchLength = Math.Round(form.ReturnValue,mf.decimals)).ToString();
                }
            }
            btnCancel.Focus();
        }

        private void TboxMappingOnDelay_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(0, 5, MappingOnDelay, this, false,2))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxMappingOnDelay.Text = (MappingOnDelay = Math.Round(form.ReturnValue,2)).ToString("0.0#");
                }
            }
            btnCancel.Focus();
        }

        private void TboxMappingOffDelay_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(0, 5, MappingOffDelay, this, false,2))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxMappingOffDelay.Text = (MappingOffDelay = Math.Round(form.ReturnValue, 2)).ToString("0.0#");
                }
            }
            btnCancel.Focus();
        }

        private void TboxTurnOffDelay_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(0, 5, toolTurnOffDelay, this, false,2))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxMappingOffDelay.Text = (toolTurnOffDelay = Math.Round(form.ReturnValue,2)).ToString("0.0#");

                    if (toolTurnOffDelay > 0)
                    {
                       TboxLookAheadOff.Text = (toolLookAheadOff = 0).ToString("0.0#");
                    }
                }
            }
            btnCancel.Focus();
        }

        private void TboxOffset_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(Math.Round(-25 * mf.m2MetImp, mf.decimals), Math.Round(25 * mf.m2MetImp, mf.decimals), toolOffset, this, mf.isMetric, mf.decimals))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxOffset.Text = (toolOffset = Math.Round(form.ReturnValue,mf.decimals)).ToString();
                }
            }
            btnCancel.Focus();
        }

        private void TboxCutoffSpeed_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(0, Math.Round(30 / mf.cutoffMetricImperial, mf.decimals), cutoffSpeed, this, false,2))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxCutoffSpeed.Text = (cutoffSpeed = Math.Round(form.ReturnValue,2)).ToString("0.0#");
                }
            }
            btnCancel.Focus();
        }

        private void TboxOverlap_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(Math.Round(-25 * mf.m2MetImp, mf.decimals), Math.Round(25 * mf.m2MetImp, mf.decimals), toolOverlap, this, mf.isMetric, mf.decimals))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxOverlap.Text = (toolOverlap = Math.Round(form.ReturnValue, mf.decimals)).ToString();
                }
            }
            btnCancel.Focus();
        }

        #endregion Sections //---------------------------------------------------------------

        #region TextBox

        private void TboxSectionWidth_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(Math.Round(0.05 * mf.m2MetImp, mf.decimals), Math.Round(75 * mf.m2MetImp, mf.decimals), SectionWidth, this, mf.isMetric, mf.decimals))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    SectionWidth = Math.Round(form.ReturnValue, mf.decimals);
                    if (mf.isMetric)
                    {
                        double a = 7500.0;
                        while (numberOfSections * SectionWidth > 7500)
                        {
                            SectionWidth = Math.Round(a / numberOfSections, mf.decimals);
                            a--;
                        }
                    }
                    else
                    {
                        double a = 2952.755;
                        while (numberOfSections * SectionWidth > 2952.755)
                        {
                            SectionWidth = Math.Round(a / numberOfSections, mf.decimals);
                            a--;
                        }
                    }
                    TboxSectionWidth.Text = SectionWidth.ToString();

                    for (int i = 0; i < numberOfSections; i++)
                    {
                        Section[i].Text = SectionWidth.ToString();
                    }
                    UpdateSpinners();

                }
            }
            btnCancel.Focus();
        }

        private void TboxMinApplied_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(0, 100, MinApplied, this, true, 0))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    MinApplied = (int)form.ReturnValue;
                    TboxMinApplied.Text = MinApplied.ToString();
                }
            }
            btnCancel.Focus();
        }

        private void TboxNumSections_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(0, 100, numberOfSections, this, true, 0))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    numberOfSections = (int)form.ReturnValue;

                    TboxNumSections.Text = numberOfSections.ToString();
                    UpdateNumberOfSections();

                }
            }
            btnCancel.Focus();
        }

        private void UpdateNumberOfSections()
        {
            while (Section.Count > numberOfSections)
            {
                Section[Section.Count - 1].Dispose();
                SectionText[SectionText.Count - 1].Dispose();
                Section.RemoveAt(Section.Count - 1);
                SectionText.RemoveAt(SectionText.Count - 1);
            }

            int Height = 55;
            int Width = 30;
            for (int i = 0; i < numberOfSections; i++)
            {
                if (i >= Section.Count)
                {
                    SectionText.Add(new Label());
                    SectionPanel.Controls.Add(SectionText[i]);
                    SectionText[i].Text = (i + 1).ToString();

                    SectionText[i].Font = new Font("Tahoma", 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
                    SectionText[i].Size = new Size(120, 50);
                    SectionText[i].TextAlign = ContentAlignment.MiddleCenter;


                    Section.Add(new TextBox());
                    SectionPanel.Controls.Add(Section[i]);


                    Section[i].TextAlign = HorizontalAlignment.Center;
                    Section[i].Size = new Size(120, 52);
                    Section[i].Font = new Font("Tahoma", 27.75F, FontStyle.Regular, GraphicsUnit.Point, 0);


                    if (i < Properties.Vehicle.Default.ToolSettings[Here].Sections.Count)
                        Section[i].Text = Math.Max((Math.Abs(Properties.Vehicle.Default.ToolSettings[Here].Sections[i][1] - Properties.Vehicle.Default.ToolSettings[Here].Sections[i][0]) * mf.m2MetImp), 1).ToString();
                    else Section[i].Text = (Properties.Vehicle.Default.setTool_defaultSectionWidth * mf.m2MetImp).ToString();

                    Section[i].Enter += Section_Enter;
                    Section[i].Name = i.ToString();
                }

                SectionText[i].Location = new Point(Width, Height - 55);
                Section[i].Location = new Point(Width, Height);

                Width += 150;

                if (Width > 900)
                {
                    Width %= 900;
                    Height += 150;
                }
            }
            UpdateSpinners();
        }

        # endregion TextBox
    }
}
