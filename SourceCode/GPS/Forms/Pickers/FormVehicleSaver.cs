using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormVehicleSaver : Form
    {
        //class variables
        private readonly FormGPS mf;

        public FormVehicleSaver(Form callingForm)
        {
            //get copy of the calling main form
            Owner = mf = callingForm as FormGPS;
            InitializeComponent();

            //this.bntOK.Text = gStr.gsForNow;
            //this.btnSave.Text = gStr.gsToFile;

            this.Text = String.Get("gsSaveVehicle");
        }

        private void FormFlags_Load(object sender, EventArgs e)
        {
            lblLast.Text = String.Get("gsCurrent") + mf.vehicleFileName;

            string dir = Path.GetDirectoryName(mf.vehiclesDirectory);
            if (Directory.Exists(dir))
            {
                DirectoryInfo dinfo = new DirectoryInfo(mf.vehiclesDirectory);
                FileInfo[] Files = dinfo.GetFiles("*.txt");

                if (Files.Length == 0) cboxVeh.Enabled = false;

                foreach (FileInfo file in Files)
                {
                    cboxVeh.Items.Add(Path.GetFileNameWithoutExtension(file.Name));
                }
            }
            else cboxVeh.Enabled = false;
        }

        private void CboxVeh_SelectedIndexChanged(object sender, EventArgs e)
        {
            DialogResult result3 = MessageBox.Show(
                "Overwrite: " + cboxVeh.SelectedItem.ToString() + ".txt",
                String.Get("gsSaveAndReturn"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (result3 == DialogResult.Yes)
            {
                mf.FileSaveVehicle(mf.vehiclesDirectory + cboxVeh.SelectedItem.ToString() + ".txt");
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
                mf.FileSaveVehicle(mf.vehiclesDirectory + tboxName.Text.Trim() + ".txt");
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