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

        private double toolOverlap, toolTrailingHitchLength, tankTrailingHitchLength, toolOffset, toolTurnOffDelay, toolLookAheadOn, toolLookAheadOff, MappingOnDelay, MappingOffDelay;
        private double hitchLength, ToolWidth = 0;
        private bool isToolTrailing, isToolBehindPivot, isToolTBT;
        private int numberOfSections, minApplied;


        public List<NumericUpDown> Section = new List<NumericUpDown>();
        public List<Label> SectionText = new List<Label>();

        private double defaultSectionWidth;

        private bool isWorkSwEn, isWorkSwActiveLow, isWorkSwitchManual;

        private readonly double metImp2m, m2MetImp, maxWidth ;
        private double cutoffSpeed, cutoffMetricImperial;

        int Here = 0;

        //constructor
        public FormToolSettings(Form callingForm, int page)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();

            //Language keys
            gboxAttachment.Text = gStr.gsAttachmentStyle;
            tabHitch.Text = gStr.gsHitch;
            tabConfig.Text = gStr.gsConfiguration;
            tabSections.Text = gStr.gsSection;
            tabSettings.Text = gStr.gsSettings;
            tabWorkSwitch.Text = gStr.gsSwitches;

            label23.Text = gStr.gsTurnOffDelaySecs;
            label8.Text = gStr.gsTurnOffAheadSecs;
            label3.Text = gStr.gsTurnOnAheadSecs;
            label41.Text = gStr.gsMinUnapplied;
            label2.Text = gStr.gs_OfSections;
            label4.Text = gStr.gsSectionWidth;

            lblTurnOffBelowUnits.Text = gStr.gsKMH;
            label30.Text = gStr.gsSectionsTurnOffBelow;

            groupBox3.Text = gStr.gsWorkSwitch;
            checkWorkSwitchManual.Text = gStr.gsWorkSwitchControlsManual;
            chkWorkSwActiveLow.Text = gStr.gsActiveLow;
            chkEnableWorkSwitch.Text = gStr.gsEnableWorkSwitch;
            label17.Text = gStr.gsMeasurementsIn;
            label16.Text = gStr.gsToolWidth;

            Text = gStr.gsToolSettings;

            nudCutoffSpeed.Controls[0].Enabled = false;
            nudForeAft.Controls[0].Enabled = false;
            nudHitchLength.Controls[0].Enabled = false;

            nudLookAhead.Controls[0].Enabled = false;
            nudLookAheadOff.Controls[0].Enabled = false;
            nudTurnOffDelay.Controls[0].Enabled = false;


            nudMappingOnDelay.Controls[0].Enabled = false;
            nudMappingOffDelay.Controls[0].Enabled = false;

            nudMinApplied.Controls[0].Enabled = false;
            nudDefaultSectionWidth.Controls[0].Enabled = false;

            nudOffset.Controls[0].Enabled = false;
            nudOverlap.Controls[0].Enabled = false;

            NumSections.Controls[0].Enabled = false;

            nudTankHitch.Controls[0].Enabled = false;

            if (mf.isMetric)
            {
                metImp2m = 0.01;
                m2MetImp = 100.0;
                lblInchesCm.Text = gStr.gsCentimeters;
                lblSecTotalWidthFeet.Visible = false;
                lblSecTotalWidthInches.Visible = false;
                lblSecTotalWidthMeters.Visible = true;
                lblDoNotExceed.Text = "* < 7500 cm*";
                maxWidth = 7500;
            }
            else
            {
                metImp2m = Glm.in2m;
                m2MetImp = Glm.m2in;
                lblInchesCm.Text = gStr.gsInches;
                lblSecTotalWidthFeet.Visible = true;
                lblSecTotalWidthInches.Visible = true;
                lblSecTotalWidthMeters.Visible = false;
                lblDoNotExceed.Text = "* < 1968 inches *";
                maxWidth = 1968;
            }
            //select the page as per calling menu or button from mainGPS form
            tabControl1.SelectedIndex = page;
        }

        //do any field initializing for form here
        private void FormToolSettings_Load(object sender, EventArgs e)
        {
            if (mf.isJobStarted)
            {
                //NumSections.Enabled = false;
                //nudDefaultSectionWidth.Enabled = false;
            }

            hitchLength = Math.Abs(Properties.Vehicle.Default.ToolSettings[Here].HitchLength);
            if (nudHitchLength.CheckValueCm(ref hitchLength)) nudHitchLength.BackColor = Color.OrangeRed;

            toolTrailingHitchLength = Math.Abs(Properties.Vehicle.Default.ToolSettings[Here].TrailingHitchLength);
            if (nudForeAft.CheckValueCm(ref toolTrailingHitchLength)) nudForeAft.BackColor = Color.OrangeRed;

            tankTrailingHitchLength = Math.Abs(Properties.Vehicle.Default.ToolSettings[Here].TankTrailingHitchLength);
            if (nudTankHitch.CheckValueCm(ref tankTrailingHitchLength)) nudTankHitch.BackColor = Color.OrangeRed;

            toolOverlap = Properties.Vehicle.Default.GuidanceOverlap;
            if (nudOverlap.CheckValueCm(ref toolOverlap)) nudOverlap.BackColor = Color.OrangeRed;

            toolOffset = Properties.Vehicle.Default.GuidanceOffset;
            if (nudOffset.CheckValueCm(ref toolOffset)) nudOffset.BackColor = Color.OrangeRed;

            defaultSectionWidth = Properties.Vehicle.Default.setTool_defaultSectionWidth * m2MetImp;
            if (nudDefaultSectionWidth.CheckValue(ref defaultSectionWidth)) nudDefaultSectionWidth.BackColor = Color.OrangeRed;

            toolTurnOffDelay = Properties.Vehicle.Default.ToolSettings[Here].TurnOffDelay;
            if (nudTurnOffDelay.CheckValue(ref toolTurnOffDelay)) nudTurnOffDelay.BackColor = Color.OrangeRed;


            toolLookAheadOff = Properties.Vehicle.Default.ToolSettings[Here].LookAheadOff;
            if (nudLookAheadOff.CheckValue(ref toolLookAheadOff)) nudLookAheadOff.BackColor = Color.OrangeRed;

            MappingOnDelay = Properties.Vehicle.Default.ToolSettings[Here].MappingOnDelay;
            if (nudMappingOnDelay.CheckValue(ref MappingOnDelay)) nudMappingOnDelay.BackColor = Color.OrangeRed;

            MappingOffDelay = Properties.Vehicle.Default.ToolSettings[Here].MappingOffDelay;
            if (nudMappingOffDelay.CheckValue(ref MappingOffDelay)) nudMappingOffDelay.BackColor = Color.OrangeRed;


            toolLookAheadOn = Properties.Vehicle.Default.ToolSettings[Here].LookAheadOn;
            if (nudLookAhead.CheckValue(ref toolLookAheadOn)) nudLookAhead.BackColor = Color.OrangeRed;


            minApplied = Properties.Vehicle.Default.ToolSettings[Here].MinApplied;
            double temp = minApplied;
            if (nudMinApplied.CheckValue(ref temp)) nudMinApplied.BackColor = Color.OrangeRed;


            NumSections.Value = numberOfSections = Properties.Vehicle.Default.ToolSettings[Here].Sections.Count;

            isToolBehindPivot = Properties.Vehicle.Default.ToolSettings[Here].BehindPivot;
            isToolTrailing = Properties.Vehicle.Default.ToolSettings[Here].Trailing;
            isToolTBT = Properties.Vehicle.Default.ToolSettings[Here].TBT;

            if (!isToolBehindPivot)
            {
                rbtnTBT.Checked = false;
                rbtnTrailing.Checked = false;
                rbtnFixedRear.Checked = false;
                rbtnFront.Checked = true;
            }
            else if (isToolTBT)
            {
                rbtnTBT.Checked = true;
                rbtnTrailing.Checked = false;
                rbtnFixedRear.Checked = false;
                rbtnFront.Checked = false;
            }
            else if (!isToolTrailing)
            {
                rbtnTBT.Checked = false;
                rbtnTrailing.Checked = false;
                rbtnFixedRear.Checked = true;
                rbtnFront.Checked = false;
            }
            else
            {
                rbtnTBT.Checked = false;
                rbtnTrailing.Checked = true;
                rbtnFixedRear.Checked = false;
                rbtnFront.Checked = false;
            }

            btnChangeAttachment.Enabled = false;

            FixRadioButtonsAndImages();

            //fix the min max based on inches - they are 2.54 times smaller then cm
            if (!mf.isMetric)
            {
                nudTankHitch.Maximum /= 2.54M;
                nudTankHitch.Minimum /= 2.54M;

                nudForeAft.Maximum /= 2.54M;
                nudForeAft.Minimum /= 2.54M;

                nudOverlap.Maximum /= 2.54M;
                nudOverlap.Minimum /= 2.54M;

                nudOffset.Maximum /= 2.54M;
                nudOffset.Minimum /= 2.54M;

                nudCutoffSpeed.Maximum /= 1.60934M;
                nudCutoffSpeed.Minimum /= 1.60934M;

                nudDefaultSectionWidth.Maximum /= 2.54M;
                nudDefaultSectionWidth.Minimum /= 2.54M;

                lblTurnOffBelowUnits.Text = gStr.gsMPH;
                cutoffMetricImperial = 1.60934;

            }
            else
            {
                lblTurnOffBelowUnits.Text = gStr.gsKMH;
                cutoffMetricImperial = 1;
            }

            nudHitchLength.ValueChanged -= NudHitchLength_ValueChanged;
            nudHitchLength.Value = (decimal)(hitchLength * m2MetImp);
            nudHitchLength.ValueChanged += NudHitchLength_ValueChanged;

            nudMinApplied.ValueChanged -= NudMinApplied_ValueChanged;
            nudMinApplied.Value = (decimal)(minApplied);
            nudMinApplied.ValueChanged += NudMinApplied_ValueChanged;

            nudOverlap.ValueChanged -= NudOverlap_ValueChanged;
            nudOverlap.Value = (decimal)(toolOverlap * m2MetImp);
            nudOverlap.ValueChanged += NudOverlap_ValueChanged;

            nudForeAft.ValueChanged -= NudForeAft_ValueChanged;
            nudForeAft.Value = (decimal)(toolTrailingHitchLength * m2MetImp);
            nudForeAft.ValueChanged += NudForeAft_ValueChanged;

            nudTankHitch.ValueChanged -= NudTankHitch_ValueChanged;
            nudTankHitch.Value = (decimal)(tankTrailingHitchLength * m2MetImp);
            nudTankHitch.ValueChanged += NudTankHitch_ValueChanged;

            nudOffset.ValueChanged -= NudOffset_ValueChanged;
            nudOffset.Value = (decimal)(toolOffset * m2MetImp);
            nudOffset.ValueChanged += NudOffset_ValueChanged;

            nudDefaultSectionWidth.Value = (decimal)(defaultSectionWidth);
            nudDefaultSectionWidth.ValueChanged += NudDefaultSectionWidth_ValueChanged;

            nudTurnOffDelay.ValueChanged -= NudTurnOffDelay_ValueChanged;
            nudTurnOffDelay.Value = (decimal)(toolTurnOffDelay);
            nudTurnOffDelay.ValueChanged += NudTurnOffDelay_ValueChanged;

            nudLookAhead.ValueChanged -= NudLookAhead_ValueChanged;
            nudLookAhead.Value = (decimal)(toolLookAheadOn);
            nudLookAhead.ValueChanged += NudLookAhead_ValueChanged;

            nudLookAheadOff.ValueChanged -= NudLookAheadOff_ValueChanged;
            nudLookAheadOff.Value = (decimal)(toolLookAheadOff);
            nudLookAheadOff.ValueChanged += NudLookAheadOff_ValueChanged;

            nudMappingOnDelay.ValueChanged -= MappingOnDelay_ValueChanged;
            nudMappingOnDelay.Value = (decimal)(MappingOnDelay);
            nudMappingOnDelay.ValueChanged += MappingOnDelay_ValueChanged;

            nudMappingOffDelay.ValueChanged -= MappingOffDelay_ValueChanged;
            nudMappingOffDelay.Value = (decimal)(MappingOffDelay);
            nudMappingOffDelay.ValueChanged += MappingOffDelay_ValueChanged;







            //based on number of sections and values update the page before displaying
            UpdateSpinners();

            isWorkSwActiveLow = Properties.Settings.Default.setF_IsWorkSwitchActiveLow;

            chkWorkSwActiveLow.CheckedChanged -= ChkWorkSwActiveLow_CheckedChanged;
            chkWorkSwActiveLow.Checked = isWorkSwActiveLow;
            chkWorkSwActiveLow.CheckedChanged += ChkWorkSwActiveLow_CheckedChanged;

            isWorkSwEn = Properties.Settings.Default.setF_IsWorkSwitchEnabled;

            chkEnableWorkSwitch.CheckedChanged -= ChkEnableWorkSwitch_CheckedChanged;
            chkEnableWorkSwitch.Checked = isWorkSwEn;
            chkEnableWorkSwitch.CheckedChanged += ChkEnableWorkSwitch_CheckedChanged;

            isWorkSwitchManual = Properties.Settings.Default.setF_IsWorkSwitchManual;

            checkWorkSwitchManual.CheckedChanged -= CheckWorkSwitchManual_CheckedChanged;
            checkWorkSwitchManual.Checked = isWorkSwitchManual;
            checkWorkSwitchManual.CheckedChanged += CheckWorkSwitchManual_CheckedChanged;

            btnChangeAttachment.BackColor = Color.Transparent;
            btnChangeAttachment.Enabled = false;


            cutoffSpeed = Properties.Vehicle.Default.ToolSettings[Here].SlowSpeedCutoff / cutoffMetricImperial;
            if (nudCutoffSpeed.CheckValue(ref cutoffSpeed)) nudCutoffSpeed.BackColor = System.Drawing.Color.OrangeRed;

            nudCutoffSpeed.ValueChanged -= NudCutoffSpeed_ValueChanged;
            nudCutoffSpeed.Value = (decimal)cutoffSpeed;
            nudCutoffSpeed.ValueChanged += NudCutoffSpeed_ValueChanged;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            //Tool  ------------------------------------------------------------------------------------------

            Properties.Vehicle.Default.ToolSettings[Here].LookAheadOn = toolLookAheadOn;
            Properties.Vehicle.Default.ToolSettings[Here].LookAheadOff = toolLookAheadOff;
            Properties.Vehicle.Default.ToolSettings[Here].TurnOffDelay = toolTurnOffDelay;
            Properties.Vehicle.Default.ToolSettings[Here].MappingOnDelay = MappingOnDelay;
            Properties.Vehicle.Default.ToolSettings[Here].MappingOffDelay = MappingOffDelay;

            Properties.Vehicle.Default.ToolSettings[Here].TrailingHitchLength = toolTrailingHitchLength *= -1;
            Properties.Vehicle.Default.ToolSettings[Here].TankTrailingHitchLength = tankTrailingHitchLength *= -1;
            Properties.Vehicle.Default.ToolSettings[Here].HitchLength = hitchLength *= isToolBehindPivot? -1 : 1;

            Properties.Vehicle.Default.ToolSettings[Here].Trailing = isToolTrailing;
            Properties.Vehicle.Default.ToolSettings[Here].BehindPivot = isToolBehindPivot;
            Properties.Vehicle.Default.ToolSettings[Here].TBT = isToolTBT;

            Properties.Vehicle.Default.ToolSettings[Here].MinApplied = minApplied;
            Properties.Vehicle.Default.ToolSettings[Here].SlowSpeedCutoff = cutoffSpeed * cutoffMetricImperial;






            double Width = 0;
            for (int i = 0; i < Section.Count; i++)
            {
                Width += (double)Section[i].Value * metImp2m;
            }
            List<double[]> aa = new List<double[]>();


            Properties.Vehicle.Default.GuidanceOverlap = mf.Guidance.GuidanceOverlap = toolOverlap;
            Properties.Vehicle.Default.GuidanceOffset = mf.Guidance.GuidanceOffset = toolOffset;
            Properties.Vehicle.Default.GuidanceWidth = mf.Guidance.GuidanceWidth = Width;
            mf.Guidance.WidthMinusOverlap = Width - mf.Guidance.GuidanceOverlap;




            Width /= -2;

            //save the values in each spinner for section position widths in settings
            for (int i = 0; i < Section.Count; i++)
            {
                aa.Add(new double[] { Width, Width += (double)Section[i].Value * metImp2m, 0 });
            }
            Properties.Vehicle.Default.ToolSettings[Here].Sections = aa;

            Properties.Settings.Default.Save();
            Properties.Vehicle.Default.Save();




            mf.LoadTools();

            mf.LineUpManualBtns();




            //WorkSwitch settings
            mf.mc.isWorkSwitchActiveLow = isWorkSwActiveLow;
            Properties.Settings.Default.setF_IsWorkSwitchActiveLow = isWorkSwActiveLow;

            mf.mc.isWorkSwitchEnabled = isWorkSwEn;
            Properties.Settings.Default.setF_IsWorkSwitchEnabled = isWorkSwEn;

            mf.mc.isWorkSwitchManual = isWorkSwitchManual;
            Properties.Settings.Default.setF_IsWorkSwitchManual = isWorkSwitchManual;




            mf.tram.abOffset = (Math.Round(mf.Guidance.WidthMinusOverlap / 2.0, 3));

            Properties.Vehicle.Default.setTool_defaultSectionWidth = defaultSectionWidth *0.01;
            Properties.Settings.Default.Save();
            Properties.Vehicle.Default.Save();

            //back to FormGPS
            DialogResult = DialogResult.OK;
            Close();
        }

        private void NudCutoffSpeed_ValueChanged(object sender, EventArgs e)
        {
            cutoffSpeed = (double)nudCutoffSpeed.Value;
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

                nudForeAft.Visible = true;
                nudHitchLength.Visible = true;
                nudTankHitch.Visible = false;

                nudForeAft.Left = 596;
                nudHitchLength.Left = 384;
                nudTankHitch.Left = 0;

                tabHitch.BackgroundImage = Properties.Resources.ToolHitchPageTrailing;

            }
            else if (rbtnFixedRear.Checked)
            {
                isToolBehindPivot = true;
                isToolTrailing = false;
                isToolTBT = false;

                nudForeAft.Visible = false;
                nudHitchLength.Visible = true;
                nudTankHitch.Visible = false;

                nudForeAft.Left = 0;
                nudHitchLength.Left = 443;
                nudTankHitch.Left = 0;

                tabHitch.BackgroundImage = Properties.Resources.ToolHitchPageRear;

            }
            else if (rbtnFront.Checked)
            {
                isToolBehindPivot = false;
                isToolTrailing = false;
                isToolTBT = false;

                nudForeAft.Visible = false;
                nudHitchLength.Visible = true;
                nudTankHitch.Visible = false;

                nudForeAft.Left = 0;
                nudHitchLength.Left = 384;
                nudTankHitch.Left = 0;

                tabHitch.BackgroundImage = Properties.Resources.ToolHitchPageFront;

            }
            else //TBT
            {
                isToolBehindPivot = true;
                isToolTrailing = true;
                isToolTBT = true;

                nudForeAft.Visible = true;
                nudHitchLength.Visible = true;
                nudTankHitch.Visible = true;

                nudForeAft.Left = 700;
                nudHitchLength.Left = 283;
                nudTankHitch.Left = 486;

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

        private void NumSections_SelectedIndexChanged(object sender, EventArgs e)
        {
            numberOfSections = (int)NumSections.Value;

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

                    SectionText[i].Font = new Font("Tahoma", 20F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                    SectionText[i].Size = new Size(120, 50);
                    SectionText[i].TextAlign = ContentAlignment.MiddleCenter;


                    Section.Add(new NumericUpDown());
                    Section[i].Controls[0].Enabled = false;
                    SectionPanel.Controls.Add(Section[i]);


                    Section[i].TextAlign = HorizontalAlignment.Center;
                    Section[i].Size = new Size(120, 52);
                    Section[i].Font = new Font("Tahoma", 27.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                    Section[i].Minimum = 1;
                    Section[i].Maximum = 7500;
                    if (!mf.isMetric)
                    {
                        Section[i].Maximum /= 2.54M;
                        Section[i].Minimum /= 2.54M;
                    }


                    if (i < Properties.Vehicle.Default.ToolSettings[Here].Sections.Count)
                        Section[i].Value = (decimal)Math.Max((Math.Abs(Properties.Vehicle.Default.ToolSettings[Here].Sections[i][1] - Properties.Vehicle.Default.ToolSettings[Here].Sections[i][0]) * m2MetImp), 1);
                    else Section[i].Value = (decimal)Properties.Vehicle.Default.setTool_defaultSectionWidth;


                    //Section[i].Value = Math.Max(Math.Abs(nudDefaultSectionWidth.Value), 1);

                    Section[i].ValueChanged += Section_ValueChanged;
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

        //don't save anything, leave the settings as before
        private void BtnCancel_Click(object sender, EventArgs e)
        { DialogResult = DialogResult.Cancel; Close(); }

        private void NudHitchLength_ValueChanged(object sender, EventArgs e)
        {
            hitchLength = (double)nudHitchLength.Value * metImp2m;
        }

        private void NudForeAft_ValueChanged(object sender, EventArgs e)
        {
            toolTrailingHitchLength = (double)(nudForeAft.Value) * metImp2m;
        }

        private void NudTankHitch_ValueChanged(object sender, EventArgs e)
        {
            tankTrailingHitchLength = (double)(nudTankHitch.Value) * metImp2m;
        }

        private void NudOffset_ValueChanged(object sender, EventArgs e)
        {
            toolOffset = (double)nudOffset.Value * metImp2m;
        }

        private void NudLookAhead_ValueChanged(object sender, EventArgs e)
        {
            if (nudLookAheadOff.Value > (nudLookAhead.Value * 0.8m))
            {
                nudLookAheadOff.Value = nudLookAhead.Value * 0.8m;
                toolLookAheadOff = (double)nudLookAheadOff.Value;
            }
            toolLookAheadOn = (double)nudLookAhead.Value;
        }

        private void NudLookAheadOff_ValueChanged(object sender, EventArgs e)
        {
            if (nudLookAheadOff.Value > (nudLookAhead.Value * 0.8m))
            {
                nudLookAheadOff.Value = nudLookAhead.Value * 0.8m;
            }
            toolLookAheadOff = (double)nudLookAheadOff.Value;

            if (nudLookAheadOff.Value > 0)
            {
                toolTurnOffDelay = 0;
                nudTurnOffDelay.Value = 0;
            }
        }

        private void MappingOnDelay_ValueChanged(object sender, EventArgs e)
        {
            MappingOnDelay = (double)nudMappingOnDelay.Value;
        }

        private void MappingOffDelay_ValueChanged(object sender, EventArgs e)
        {
            MappingOffDelay = (double)nudMappingOffDelay.Value;
        }

        private void NudTurnOffDelay_ValueChanged(object sender, EventArgs e)
        {
            toolTurnOffDelay = (double)nudTurnOffDelay.Value;

            if (nudTurnOffDelay.Value > 0)
            {
                nudLookAheadOff.Value = 0;
                toolLookAheadOff = 0;
            }
        }

        private void NudOverlap_ValueChanged(object sender, EventArgs e)
        {
            toolOverlap = (double)nudOverlap.Value * metImp2m;
        }

        private void NudDefaultSectionWidth_ValueChanged(object sender, EventArgs e)
        {


            decimal wide = nudDefaultSectionWidth.Value;


            if (mf.isMetric)
            {
                if (numberOfSections * wide > 7500) wide = 7500 / numberOfSections;
            }
            else
            {
                if (numberOfSections * wide > 2874) wide = 2874 / numberOfSections;
            }

            nudDefaultSectionWidth.ValueChanged -= NudDefaultSectionWidth_ValueChanged;
            nudDefaultSectionWidth.Value = wide;
            nudDefaultSectionWidth.ValueChanged += NudDefaultSectionWidth_ValueChanged;



            for (int i = 0; i < numberOfSections; i++)
            {
                Section[i].ValueChanged -= Section_ValueChanged;
                Section[i].Value = wide;
                Section[i].ValueChanged += Section_ValueChanged;
            }

            UpdateSpinners();

            defaultSectionWidth = (double)nudDefaultSectionWidth.Value;
        }








        #region Sections //---------------------------------------------------------------

        //enable or disable section width spinners based on number sections selected
        public void UpdateSpinners()
        {
            decimal Width = 0;
            for (int i = 0; i < Section.Count; i++)
            {
                Width += Section[i].Value;
            }
            ToolWidth = (double)Width;




            //update in settings dialog ONLY total tool width
            if (mf.isMetric)
            {
                lblSecTotalWidthMeters.Text = Width.ToString("0") + " cm";
            }
            else
            {
                double toFeet = (double)Width * 0.08334;
                lblSecTotalWidthFeet.Text = Convert.ToString((int)toFeet) + "'";
                double temp = Math.Round((toFeet - Math.Truncate(toFeet)) * 12, 0);
                lblSecTotalWidthInches.Text = Convert.ToString(temp) + '"';
            }
        }

        //the minimum speed before sections turn off

        private void NudMinApplied_ValueChanged(object sender, EventArgs e)
        {
            minApplied = (int)nudMinApplied.Value;
        }

        //Did user spin a section distance spinner?
        private void Section_ValueChanged(object sender, EventArgs e)
        {
            if (sender is NumericUpDown b)
            {
                UpdateSpinners();
                while (ToolWidth > maxWidth) Section[Convert.ToInt32(b.Name)].Value--;
            }
        }

        #endregion Sections //---------------------------------------------------------------


        #region Keypad

        private void NudHitchLength_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender);
            btnCancel.Focus();
        }

        private void NudTankHitch_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender);
            btnCancel.Focus();
        }

        private void NudForeAft_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender);
            btnCancel.Focus();
        }

        private void NudOffset_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender);
            btnCancel.Focus();
        }

        private void NudOverlap_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender);
            btnCancel.Focus();
        }

        private void NudTurnOffDelay_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender);
            btnCancel.Focus();
        }

        private void NudDefaultSectionWidth_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender);
            btnCancel.Focus();

        }

        private void NudLookAhead_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender);
            btnCancel.Focus();
        }

        private void Section_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender);
            btnCancel.Focus();
        }

        private void NudLookAheadOff_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender);
            btnCancel.Focus();
        }

        private void MappingOnDelay_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender);
            btnCancel.Focus();
        }

        private void MappingOffDelay_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender);
            btnCancel.Focus();
        }

        private void NudMinApplied_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender);
            btnCancel.Focus();
        }
        private void NudSections_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender);
            btnCancel.Focus();
        }

        private void NudCutoffSpeed_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender);
            btnCancel.Focus();
        }

        # endregion Keypad

        #region WorkSwitch //---------------------------------------------------------

        private void ChkWorkSwActiveLow_CheckedChanged(object sender, EventArgs e)
        {
            isWorkSwActiveLow = !isWorkSwActiveLow;
            chkWorkSwActiveLow.Checked = isWorkSwActiveLow;
        }

        private void ChkEnableWorkSwitch_CheckedChanged(object sender, EventArgs e)
        {
            isWorkSwEn = !isWorkSwEn;
            chkEnableWorkSwitch.Checked = isWorkSwEn;
        }

        private void CheckWorkSwitchManual_CheckedChanged(object sender, EventArgs e)
        {
            isWorkSwitchManual = !isWorkSwitchManual;
            checkWorkSwitchManual.Checked = isWorkSwitchManual;
        }

        #endregion WorkSwitch //---------------------------------------------------------
    }
}
