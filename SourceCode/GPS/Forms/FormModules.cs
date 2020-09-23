using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormModules : Form
    {
        private readonly FormGPS mf = null;

        public FormModules(Form callingForm)
        {
            Owner = mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Send_AutoSteer0.Text = mf.mc.Send_AutoSteer[0].ToString();
            Send_AutoSteer1.Text = mf.mc.Send_AutoSteer[1].ToString();
            Send_AutoSteer2.Text = mf.mc.Send_AutoSteer[2].ToString();
            Send_AutoSteer3.Text = mf.mc.Send_AutoSteer[3].ToString();
            Send_AutoSteer4.Text = mf.mc.Send_AutoSteer[4].ToString();
            Send_AutoSteer5.Text = mf.mc.Send_AutoSteer[5].ToString();
            Send_AutoSteer6.Text = mf.mc.Send_AutoSteer[6].ToString();
            Send_AutoSteer7.Text = mf.mc.Send_AutoSteer[7].ToString();
            Send_AutoSteer8.Text = mf.mc.Send_AutoSteer[8].ToString();

            Send_Sections0.Text = mf.mc.Send_Sections[0].ToString();
            Send_Sections1.Text = mf.mc.Send_Sections[1].ToString();
            Send_Sections2.Text = mf.mc.Send_Sections[2].ToString();
            Send_Sections3.Text = mf.mc.Send_Sections[3].ToString();
            Send_Sections4.Text = mf.mc.Send_Sections[4].ToString();
            Send_Sections5.Text = mf.mc.Send_Sections[5] == 0x01 ? "On" : "Off";

            Send_AutoSteerButton0.Text = mf.mc.Send_AutoSteerButton[0].ToString();
            Send_AutoSteerButton1.Text = mf.mc.Send_AutoSteerButton[1].ToString();
            Send_AutoSteerButton2.Text = mf.mc.Send_AutoSteerButton[2].ToString();
            Send_AutoSteerButton3.Text = mf.mc.Send_AutoSteerButton[3] == 0x01 ? "On" : "Off";

            Send_Uturn0.Text = mf.mc.Send_Uturn[0].ToString();
            Send_Uturn1.Text = mf.mc.Send_Uturn[1].ToString();
            Send_Uturn2.Text = mf.mc.Send_Uturn[2].ToString();
            Send_Uturn3.Text = Convert.ToString(mf.mc.Send_Uturn[3], 2).PadLeft(8, '0');

            Send_HydraulicLift0.Text = mf.mc.Send_HydraulicLift[0].ToString();
            Send_HydraulicLift1.Text = mf.mc.Send_HydraulicLift[1].ToString();
            Send_HydraulicLift2.Text = mf.mc.Send_HydraulicLift[2].ToString();
            Send_HydraulicLift3.Text = mf.mc.Send_HydraulicLift[3] == 0x02 ? "Up" : mf.mc.Send_HydraulicLift[3] == 0x01 ? "Down" : "Off";

            Send_Treeplant0.Text = mf.mc.Send_Treeplant[0].ToString();
            Send_Treeplant1.Text = mf.mc.Send_Treeplant[1].ToString();
            Send_Treeplant2.Text = mf.mc.Send_Treeplant[2].ToString();
            Send_Treeplant3.Text = mf.mc.Send_Treeplant[3] == 0x01 ? "On" : "Off";





            Recieve_AutoSteer0.Text = mf.mc.Recieve_AutoSteer[0].ToString();
            Recieve_AutoSteer1.Text = mf.mc.Recieve_AutoSteer[1].ToString();
            Recieve_AutoSteer2.Text = mf.mc.Recieve_AutoSteer[2].ToString();
            Recieve_AutoSteer3.Text = mf.mc.Recieve_AutoSteer[3].ToString();
            Recieve_AutoSteer4.Text = mf.mc.Recieve_AutoSteer[4].ToString();
            Recieve_AutoSteer5.Text = mf.mc.Recieve_AutoSteer[5].ToString();

            Recieve_SectionsStatus0.Text = mf.mc.Recieve_SectionsStatus[0].ToString();
            Recieve_SectionsStatus1.Text = mf.mc.Recieve_SectionsStatus[1].ToString();
            Recieve_SectionsStatus2.Text = mf.mc.Recieve_SectionsStatus[2].ToString();
            Recieve_SectionsStatus3.Text = mf.mc.Recieve_SectionsStatus[3].ToString();
            Recieve_SectionsStatus4.Text = mf.mc.Recieve_SectionsStatus[4].ToString();
            Recieve_SectionsStatus5.Text = mf.mc.Recieve_SectionsStatus[5].ToString();

            Recieve_Heading0.Text = mf.mc.Recieve_Heading[0].ToString();
            Recieve_Heading1.Text = mf.mc.Recieve_Heading[1].ToString();
            Recieve_Heading2.Text = mf.mc.Recieve_Heading[2].ToString();
            Recieve_Heading3.Text = mf.mc.Recieve_Heading[3].ToString();
            Recieve_Heading4.Text = mf.mc.Recieve_Heading[4].ToString();

            Recieve_Roll0.Text = mf.mc.Recieve_Roll[0].ToString();
            Recieve_Roll1.Text = mf.mc.Recieve_Roll[1].ToString();
            Recieve_Roll2.Text = mf.mc.Recieve_Roll[2].ToString();
            Recieve_Roll3.Text = mf.mc.Recieve_Roll[3].ToString();
            Recieve_Roll4.Text = mf.mc.Recieve_Roll[4].ToString();

            Recieve_AutoSteerButton0.Text = mf.mc.Recieve_AutoSteerButton[0].ToString();
            Recieve_AutoSteerButton1.Text = mf.mc.Recieve_AutoSteerButton[1].ToString();
            Recieve_AutoSteerButton2.Text = mf.mc.Recieve_AutoSteerButton[2].ToString();
            Recieve_AutoSteerButton3.Text = mf.mc.Recieve_AutoSteerButton[3].ToString();

            Recieve_WorkSwitch0.Text = mf.mc.Recieve_WorkSwitch[0].ToString();
            Recieve_WorkSwitch1.Text = mf.mc.Recieve_WorkSwitch[1].ToString();
            Recieve_WorkSwitch2.Text = mf.mc.Recieve_WorkSwitch[2].ToString();
            Recieve_WorkSwitch3.Text = mf.mc.Recieve_WorkSwitch[3].ToString();

            Recieve_Checksum0.Text = mf.mc.Recieve_Checksum[0].ToString();
            Recieve_Checksum1.Text = mf.mc.Recieve_Checksum[1].ToString();
            Recieve_Checksum2.Text = mf.mc.Recieve_Checksum[2].ToString();
            Recieve_Checksum3.Text = mf.mc.Recieve_Checksum[3].ToString();
        }
    }
}