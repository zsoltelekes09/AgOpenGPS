using AgOpenGPS.Properties;
using System.Diagnostics;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormShutDown : Form
    {
        private readonly FormGPS mf;
        public FormShutDown(Form callingForm)
        {
            //get copy of the calling main form
            Owner = mf = callingForm as FormGPS;

            InitializeComponent();
        }

        private void BtnReturn_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void BtnExit_Click(object sender, System.EventArgs e)
        {
            if (mf.isJobStarted)
            {
                mf.isUDPSendConnected = false;
                Settings.Default.setF_CurrentDir = mf.currentFieldDirectory;
                Settings.Default.Save();
                mf.FileSaveEverythingBeforeClosingField();
                //shutdown and reset all module data
                mf.mc.ResetAllModuleCommValues(true);
            }
            mf.Close();
        }

        private void BtnShutDown_Click(object sender, System.EventArgs e)
        {
            if (mf.isJobStarted)
            {
                mf.isUDPSendConnected = false;
                Settings.Default.setF_CurrentDir = mf.currentFieldDirectory;
                Settings.Default.Save();
                mf.FileSaveEverythingBeforeClosingField();
                //shutdown and reset all module data
                mf.mc.ResetAllModuleCommValues(true);
            }
            mf.Close();

            Process.Start("shutdown", "/s /t 0");
        }

        private void FormShutDown_Activated(object sender, System.EventArgs e)
        {
            Left = Owner.Left + Owner.Width / 2 - Width / 2;
            Top = Owner.Top + Owner.Height / 2 - Height / 2;
        }
    }
}
