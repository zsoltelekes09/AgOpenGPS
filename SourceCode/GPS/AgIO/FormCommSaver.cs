using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormCommSaver : Form
    {
        //class variables
        private readonly FormLoop mf = null;

        public FormCommSaver(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormLoop;
            InitializeComponent();

            //this.bntOK.Text = gStr.gsForNow;
            //this.btnSave.Text = gStr.gsToFile;

            //this.Text = gStr.gsSaveEnvironment;
        }

        private void FormCommSaver_Load(object sender, EventArgs e)
        {
            lblLast.Text = "Current " + mf.commFileName;
            DirectoryInfo dinfo = new DirectoryInfo(mf.commDirectory);
            FileInfo[] Files = dinfo.GetFiles("*.txt");

            if (Files.Length == 0) cboxEnv.Enabled = false;

            foreach (FileInfo file in Files)
            {
                cboxEnv.Items.Add(Path.GetFileNameWithoutExtension(file.Name));
            }
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
                mf.FileSaveComm(mf.commDirectory + cboxEnv.SelectedItem.ToString() + ".txt");
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
                mf.FileSaveComm(mf.commDirectory + tboxName.Text.Trim() + ".txt");
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