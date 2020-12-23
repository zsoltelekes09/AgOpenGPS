//Please, if you use this give me some credit
//Copyright BrianTee, copy right out of it.

using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormGPSData : Form
    {
        private readonly FormGPS mf;

        public FormGPSData(Form callingForm)
        {
            Owner = mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            lblZone.Text = mf.pn.zone.ToString();

            lblEastingField.Text = Math.Round(mf.pn.fix.Easting, 1).ToString();
            lblNorthingField.Text = Math.Round(mf.pn.fix.Northing, 1).ToString();
                                                                                                                                                                  
            lblEasting.Text = ((int)mf.pn.actualEasting).ToString();
            lblNorthing.Text = ((int)mf.pn.actualNorthing).ToString();

            lblLatitude.Text = Math.Round(mf.pn.latitude, 7).ToString();
            lblLongitude.Text = Math.Round(mf.pn.longitude, 7).ToString();

            //other sat and GPS info
            lblFixQuality.Text = mf.FixQuality;
            lblSatsTracked.Text = mf.pn.satellitesTracked.ToString();
            lblStatus.Text = (mf.pn.status == "A" ? "Active": "Void");


            lblHDOP.Text = mf.pn.hdop.ToString();
            tboxNMEASerial.Lines = mf.recvSentenceSettings;
            lblSpeed.Text = mf.pn.speed.ToString("N2");

            lblUturnByte.Text = Convert.ToString(mf.mc.Send_Uturn[5], 2).PadLeft(6, '0');

            lblRoll.Text = mf.RollInDegrees;
            lblYawHeading.Text = mf.GyroInDegrees;
            lblGPSHeading.Text = Math.Round(Glm.ToDegrees(mf.fixHeading), 1) + "\u00B0";
            lblFixHeading.Text = (mf.fixHeading * 57.2957795).ToString("N1");

            if (mf.isMetric)
            {
                lblAltitude.Text = mf.Altitude;
                lblTotalFieldArea.Text = mf.fd.AreaBoundaryLessInnersHectares;
                lblTotalAppliedArea.Text = mf.fd.WorkedHectares;
                lblWorkRemaining.Text = mf.fd.WorkedAreaRemainHectares;
                lblPercentRemaining.Text = mf.fd.WorkedAreaRemainPercentage;
                lblTimeRemaining.Text = mf.fd.TimeTillFinished;
                lblEqSpec.Text = (Math.Round(mf.Guidance.GuidanceWidth, 2)).ToString() + " m  " + mf.vehicleFileName + mf.toolFileName;
            }
            else //imperial
            {
                lblAltitude.Text = mf.AltitudeFeet;
                lblTotalFieldArea.Text = mf.fd.AreaBoundaryLessInnersAcres;
                lblTotalAppliedArea.Text = mf.fd.WorkedAcres;
                lblWorkRemaining.Text = mf.fd.WorkedAreaRemainAcres;
                lblPercentRemaining.Text = mf.fd.WorkedAreaRemainPercentage;
                lblTimeRemaining.Text = mf.fd.TimeTillFinished;
                lblEqSpec.Text =  (Math.Round(mf.Guidance.GuidanceWidth * Glm.m2ft, 2)).ToString() + " ft  " + mf.vehicleFileName + mf.toolFileName;
            }

            if (mf.isUDPSendConnected)
            {
                int text1 = mf.autoSteerUDPActivity;
                int text2 = mf.machineUDPActivity;
                int text3 = mf.switchUDPActivity;

                tboxUDPSteer.Text = text1.ToString();
                tboxUDPMachine.Text = text2.ToString();
                tboxUDPSwitch.Text = text3.ToString();
            }
            else
            {
                tboxUDPSteer.Text = "NC";
                tboxUDPMachine.Text = "NC";
                tboxUDPSwitch.Text = "NC";
            }


            txtBoxRecvAutoSteer.Text = mf.mc.serialRecvAutoSteerStr;
            txtBoxRecvMachine.Text = mf.mc.serialRecvMachineStr;
        }

        private void FormGPSData_Load(object sender, EventArgs e)
        {
            lblConvergenceAngle.Text = Math.Round(Glm.ToDegrees(mf.pn.convergenceAngle), 3).ToString();
            DateTime text1 = mf.sunrise;
            DateTime text2 = mf.sunset;
            lblSunrise.Text = text1.ToString("HH:mm");
            lblSunset.Text = text2.ToString("HH:mm");

        }
    }
}