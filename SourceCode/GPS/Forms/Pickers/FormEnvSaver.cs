using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormEnvSaver : Form
    {
        //class variables
        private readonly FormGPS mf;

        public FormEnvSaver(Form callingForm)
        {
            //get copy of the calling main form
            Owner = mf = callingForm as FormGPS;
            InitializeComponent();

            //this.bntOK.Text = gStr.gsForNow;
            //this.btnSave.Text = gStr.gsToFile;

            this.Text = String.Get("gsSaveEnvironment");
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
                else cboxEnv.Enabled = false;
            }
            else cboxEnv.Enabled = false;
        }

        private void CboxVeh_SelectedIndexChanged(object sender, EventArgs e)
        {
            DialogResult result3 = MessageBox.Show(
                "Overwrite: " + cboxEnv.SelectedItem.ToString() + ".txt", 
                String.Get("gsSaveAndReturn"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (result3 == DialogResult.Yes)
            {
                mf.FileSaveEnvironment(mf.envDirectory + cboxEnv.SelectedItem.ToString() + ".txt");
                Close();
            }
        }

        private void TboxName_TextChanged(object sender, EventArgs e)
        {
            var textboxSender = (TextBox)sender;
            var cursorPosition = textboxSender.SelectionStart;
            textboxSender.Text = Regex.Replace(textboxSender.Text, Glm.fileRegex, "");

            textboxSender.SelectionStart = cursorPosition;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (tboxName.Text.Trim().Length > 0)
            {
                mf.FileSaveEnvironment(mf.envDirectory + tboxName.Text.Trim() + ".txt");
                Close();
            }
        }

        private void TboxName_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
                btnSave.Focus();
            }
        }
    }
}