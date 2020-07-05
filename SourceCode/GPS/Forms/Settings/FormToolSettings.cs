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

            hitchLength = Math.Abs(Properties.Vehicle.Default.setVehicle_hitchLength);
            if (nudHitchLength.CheckValueCm(ref hitchLength)) nudHitchLength.BackColor = System.Drawing.Color.OrangeRed;

            toolTrailingHitchLength = Math.Abs(Properties.Vehicle.Default.setTool_toolTrailingHitchLength);
            if (nudForeAft.CheckValueCm(ref toolTrailingHitchLength)) nudForeAft.BackColor = System.Drawing.Color.OrangeRed;

            tankTrailingHitchLength = Math.Abs(Properties.Vehicle.Default.setVehicle_tankTrailingHitchLength);
            if (nudTankHitch.CheckValueCm(ref tankTrailingHitchLength)) nudTankHitch.BackColor = System.Drawing.Color.OrangeRed;

            toolOverlap = Properties.Vehicle.Default.setVehicle_toolOverlap;
            if (nudOverlap.CheckValueCm(ref toolOverlap)) nudOverlap.BackColor = System.Drawing.Color.OrangeRed;

            toolOffset = Properties.Vehicle.Default.setVehicle_toolOffset;
            if (nudOffset.CheckValueCm(ref toolOffset)) nudOffset.BackColor = System.Drawing.Color.OrangeRed;

            defaultSectionWidth = Properties.Vehicle.Default.setTool_defaultSectionWidth;
            if (nudDefaultSectionWidth.CheckValueCm(ref defaultSectionWidth)) nudDefaultSectionWidth.BackColor = System.Drawing.Color.OrangeRed;

            decimal temp;
            toolTurnOffDelay = Properties.Vehicle.Default.setVehicle_toolOffDelay;
            temp = (decimal)toolTurnOffDelay;
            if (nudTurnOffDelay.CheckValue(ref temp)) nudTurnOffDelay.BackColor = System.Drawing.Color.OrangeRed;
            toolTurnOffDelay = (double)temp;

            toolLookAheadOff = Properties.Vehicle.Default.setVehicle_toolLookAheadOff;
            temp = (decimal)toolLookAheadOff;
            if (nudLookAheadOff.CheckValue(ref temp)) nudLookAheadOff.BackColor = System.Drawing.Color.OrangeRed;
            toolLookAheadOff = (double)temp;

            MappingOnDelay = Properties.Vehicle.Default.setVehicle_MappingOnDelay;
            temp = (decimal)MappingOnDelay;
            if (nudMappingOnDelay.CheckValue(ref temp)) nudMappingOnDelay.BackColor = System.Drawing.Color.OrangeRed;
            MappingOnDelay = (double)temp;

            MappingOffDelay = Properties.Vehicle.Default.setVehicle_MappingOffDelay;
            temp = (decimal)MappingOffDelay;
            if (nudMappingOffDelay.CheckValue(ref temp)) nudMappingOffDelay.BackColor = System.Drawing.Color.OrangeRed;
            MappingOffDelay = (double)temp;



            toolLookAheadOn = Properties.Vehicle.Default.setVehicle_toolLookAheadOn;
            temp = (decimal)toolLookAheadOn;
            if (nudLookAhead.CheckValue(ref temp)) nudLookAhead.BackColor = System.Drawing.Color.OrangeRed;
            toolLookAheadOn = (double)temp;


            minApplied = Properties.Vehicle.Default.setVehicle_minApplied;
            temp = minApplied;
            if (nudMinApplied.CheckValue(ref temp)) nudMinApplied.BackColor = System.Drawing.Color.OrangeRed;


            NumSections.Value = numberOfSections = Properties.Vehicle.Default.setVehicle_numSections;

            isToolBehindPivot = Properties.Vehicle.Default.setTool_isToolBehindPivot;
            isToolTrailing = Properties.Vehicle.Default.setTool_isToolTrailing;
            isToolTBT = Properties.Vehicle.Default.setTool_isToolTBT;

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

            nudDefaultSectionWidth.Value = (decimal)(defaultSectionWidth * m2MetImp);
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

            btnChangeAttachment.BackColor = System.Drawing.Color.Transparent;
            btnChangeAttachment.Enabled = false;


            cutoffSpeed = Properties.Vehicle.Default.setVehicle_slowSpeedCutoff / cutoffMetricImperial;
            temp = (decimal)cutoffSpeed;
            if (nudCutoffSpeed.CheckValue(ref temp)) nudCutoffSpeed.BackColor = System.Drawing.Color.OrangeRed;
            cutoffSpeed = (double)temp;

            nudCutoffSpeed.ValueChanged -= NudCutoffSpeed_ValueChanged;
            nudCutoffSpeed.Value = (decimal)cutoffSpeed;
            nudCutoffSpeed.ValueChanged += NudCutoffSpeed_ValueChanged;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            //Tool  ------------------------------------------------------------------------------------------
            int Here = 0;
            //trailing hitch is always behind
            toolTrailingHitchLength *= -1;
            mf.Tools[Here].toolTrailingHitchLength = toolTrailingHitchLength;
            Properties.Vehicle.Default.setTool_toolTrailingHitchLength = mf.Tools[Here].toolTrailingHitchLength;

            tankTrailingHitchLength *= -1;
            mf.Tools[Here].toolTankTrailingHitchLength = tankTrailingHitchLength;
            Properties.Vehicle.Default.setVehicle_tankTrailingHitchLength = mf.Tools[Here].toolTankTrailingHitchLength;




            mf.Tools[Here].ToolOverlap = toolOverlap;
            Properties.Vehicle.Default.setVehicle_toolOverlap = mf.Tools[Here].ToolOverlap;

            mf.Tools[Here].ToolOffset = toolOffset;
            Properties.Vehicle.Default.setVehicle_toolOffset = mf.Tools[Here].ToolOffset;
            Properties.Vehicle.Default.setVehicle_toolWidth = mf.Tools[Here].ToolWidth;

            mf.Tools[Here].WidthMinusOverlap = mf.Tools[Here].ToolWidth - mf.Tools[Here].ToolOverlap;





            mf.Tools[Here].LookAheadOnSetting = toolLookAheadOn;
            Properties.Vehicle.Default.setVehicle_toolLookAheadOn = mf.Tools[Here].LookAheadOnSetting;
           
            mf.Tools[Here].LookAheadOffSetting = toolLookAheadOff;
            Properties.Vehicle.Default.setVehicle_toolLookAheadOff = mf.Tools[Here].LookAheadOffSetting;

            mf.Tools[Here].TurnOffDelay = toolTurnOffDelay;
            Properties.Vehicle.Default.setVehicle_toolOffDelay = mf.Tools[Here].TurnOffDelay;

            mf.Tools[Here].MappingOnDelay = MappingOnDelay;
            Properties.Vehicle.Default.setVehicle_MappingOnDelay = mf.Tools[Here].MappingOnDelay;

            mf.Tools[Here].MappingOffDelay = MappingOffDelay;
            Properties.Vehicle.Default.setVehicle_MappingOffDelay = mf.Tools[Here].MappingOffDelay;


            mf.Tools[Here].isToolTrailing = isToolTrailing;
            Properties.Vehicle.Default.setTool_isToolTrailing = mf.Tools[Here].isToolTrailing;

            mf.Tools[Here].isToolBehindPivot = isToolBehindPivot;
            Properties.Vehicle.Default.setTool_isToolBehindPivot = mf.Tools[Here].isToolBehindPivot;

            mf.Tools[Here].isToolTBT = isToolTBT;
            Properties.Vehicle.Default.setTool_isToolTBT = mf.Tools[Here].isToolTBT;

            if (isToolBehindPivot) hitchLength *= -1;
            mf.Tools[Here].HitchLength = hitchLength;
            Properties.Vehicle.Default.setVehicle_hitchLength = mf.Tools[Here].HitchLength;

            //Slow speed cutoff
            Properties.Vehicle.Default.setVehicle_slowSpeedCutoff = cutoffSpeed * cutoffMetricImperial;
            mf.vehicle.slowSpeedCutoff = cutoffSpeed * cutoffMetricImperial;

            //Sections ------------------------------------------------------------------------------------------



            mf.Tools[Here].numOfSections = numberOfSections;
            Properties.Vehicle.Default.setVehicle_numSections = mf.Tools[Here].numOfSections;

            mf.Tools[Here].SetSections();

            mf.Tools[Here].toolMinUnappliedPixels = minApplied;
            Properties.Vehicle.Default.setVehicle_minApplied = minApplied;

            //take the section widths and convert to meters and positions along 

            CalculateSectionPositions();


            //line up manual buttons based on # of sections
            mf.LineUpManualBtns();

            //update the sections to newly configured widths and positions in main
            //update the widths of sections and tool width in main
            mf.SectionCalcWidths();


            //WorkSwitch settings
            mf.mc.isWorkSwitchActiveLow = isWorkSwActiveLow;
            Properties.Settings.Default.setF_IsWorkSwitchActiveLow = isWorkSwActiveLow;

            mf.mc.isWorkSwitchEnabled = isWorkSwEn;
            Properties.Settings.Default.setF_IsWorkSwitchEnabled = isWorkSwEn;

            mf.mc.isWorkSwitchManual = isWorkSwitchManual;
            Properties.Settings.Default.setF_IsWorkSwitchManual = isWorkSwitchManual;

            mf.tram.abOffset = (Math.Round((mf.Tools[Here].ToolWidth - mf.Tools[Here].ToolOverlap) / 2.0, 3));

            Properties.Vehicle.Default.setTool_defaultSectionWidth = defaultSectionWidth;
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
            btnChangeAttachment.BackColor = System.Drawing.Color.Transparent;
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
                   


                    Section[i].Value = Math.Max((Math.Abs(Properties.Vehicle.Default.Section_position[i + 1] - Properties.Vehicle.Default.Section_position[i]) * (decimal)m2MetImp), 1);
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

            defaultSectionWidth = (double)nudDefaultSectionWidth.Value * metImp2m;
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

        //Convert section width to positions along toolbar
        private void CalculateSectionPositions()
        {
            decimal Width = 0;
            for (int i = 0; i < Section.Count; i++)
            {
                Width += Section[i].Value * (decimal)metImp2m;
            }

            Width /= -2;

            //save the values in each spinner for section position widths in settings
            for (int i = 0; i < Section.Count; i++)
            {   
                Properties.Vehicle.Default.Section_position[i] = Width;
                Width += Section[i].Value * (decimal)metImp2m;
            }
            Properties.Vehicle.Default.Section_position[Section.Count] = Width;
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
