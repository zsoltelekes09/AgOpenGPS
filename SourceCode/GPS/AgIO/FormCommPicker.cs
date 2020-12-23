using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormCommPicker : Form
    {
        //class variables
        private readonly FormLoop mf = null;

        public FormCommPicker(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormLoop;
            InitializeComponent();

            //this.bntOK.Text = gStr.gsForNow;
            //this.btnSave.Text = gStr.gsToFile;

            //this.Text = gStr.gsLoadComm;
        }

        private void FormCommPicker_Load(object sender, EventArgs e)
        {
            DirectoryInfo dinfo = new DirectoryInfo(mf.commDirectory);
            FileInfo[] Files = dinfo.GetFiles("*.txt");
            if (Files.Length == 0)
            {
                DialogResult = DialogResult.Ignore;
                Close();
                mf.TimedMessageBox(2000, "Non Saved", "Save one First");
            }

            else
            {
                foreach (FileInfo file in Files)
                {
                    cboxEnv.Items.Add(Path.GetFileNameWithoutExtension(file.Name));
                }
            }
        }

        private void CboxVeh_SelectedIndexChanged(object sender, EventArgs e)
        {
            DialogResult resul = mf.FileOpenComm(mf.commDirectory + cboxEnv.SelectedItem.ToString() + ".txt");
            DialogResult = resul;
            Close();
        }
    }
}