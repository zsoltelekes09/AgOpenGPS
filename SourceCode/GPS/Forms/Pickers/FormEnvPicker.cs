using System;
using System.IO;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormEnvPicker : Form
    {
        //class variables
        private readonly FormGPS mf;

        public FormEnvPicker(Form callingForm)
        {
            //get copy of the calling main form
            Owner = mf = callingForm as FormGPS;
            InitializeComponent();

            //this.bntOK.Text = gStr.gsForNow;
            //this.btnSave.Text = gStr.gsToFile;

            this.Text = String.Get("gsLoadEnvironment");
        }

        private void FormFlags_Load(object sender, EventArgs e)
        {
            lblLast.Text = String.Get("gsCurrent") + mf.envFileName;

            string dir = Path.GetDirectoryName(mf.envDirectory);
            if (Directory.Exists(dir))
            {
                DirectoryInfo dinfo = new DirectoryInfo(mf.envDirectory);
                FileInfo[] Files = dinfo.GetFiles("*.txt");

                if (Files.Length > 0)
                {
                    foreach (FileInfo file in Files)
                    {
                        cboxEnv.Items.Add(Path.GetFileNameWithoutExtension(file.Name));
                    }
                }
                else
                {
                    DialogResult = DialogResult.Ignore;
                    Close();
                    mf.TimedMessageBox(2000, String.Get("gsNoEnvironmentSaved"), String.Get("gsSaveAnEnvironmentFirst"));
                }
            }
            else
            {
                DialogResult = DialogResult.Ignore;
                Close();
                mf.TimedMessageBox(2000, String.Get("gsNoEnvironmentSaved"), String.Get("gsSaveAnEnvironmentFirst"));
            }
        }

        private void CboxVeh_SelectedIndexChanged(object sender, EventArgs e)
        {
            DialogResult resul = mf.FileOpenEnvironment(mf.envDirectory + cboxEnv.SelectedItem.ToString() + ".txt");

            if (resul == DialogResult.OK)
            {
                DialogResult = DialogResult.OK;
            }
            else if (resul == DialogResult.Abort)
            {
                DialogResult = DialogResult.Abort;
            }
            else
            {
                DialogResult = DialogResult.Cancel;
            }
            Close();
        }
    }
}