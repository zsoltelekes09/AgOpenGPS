using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormPicker : Form
    {
        //class variables
        private readonly int Mode = 0;
        private readonly FormGPS mf;
        private readonly string DirectoryPath = "";
        private int NewName = -1;

        public FormPicker(Form callingForm, int mode)
        {
            Mode = mode;
            //get copy of the calling main form
            Owner = mf = callingForm as FormGPS;
            InitializeComponent();

            if (Mode == 0)
            {
                Text = String.Get("gsSaveTool");
                lblLast.Text = String.Get("gsCurrent") + mf.toolFileName;
                DirectoryPath = mf.toolsDirectory;
            }
            else if(Mode == 1)
            {
                Text = String.Get("gsLoadTool");
                lblLast.Text = String.Get("gsCurrent") + mf.toolFileName;
                DirectoryPath = mf.toolsDirectory;
            }
            else if (Mode == 2)
            {
                Text = String.Get("gsSaveEnvironment");
                lblLast.Text = String.Get("gsCurrent") + mf.envFileName;
                DirectoryPath = mf.envDirectory;
            }
            else if (Mode == 3)
            {
                Text = String.Get("gsLoadEnvironment");
                lblLast.Text = String.Get("gsCurrent") + mf.envFileName;
                DirectoryPath = mf.envDirectory;
            }
            else if (Mode == 4)
            {
                Text = String.Get("gsSaveVehicle");
                lblLast.Text = String.Get("gsCurrent") + mf.vehicleFileName;
                DirectoryPath = mf.vehiclesDirectory;
            }
            else if (Mode == 5)
            {
                Text = String.Get("gsLoadVehicle");
                lblLast.Text = String.Get("gsCurrent") + mf.vehicleFileName;
                DirectoryPath = mf.vehiclesDirectory;
            }

            if (Mode % 2 == 1)
            {
                label2.Visible = false;
                TboxName.Visible = false;
                TextName.Visible = false;
                BtnOk.Image = Properties.Resources.FileLoad;
            }
        }

        private void FormToolSaver_Load(object sender, EventArgs e)
        {
            if (Directory.Exists(Path.GetDirectoryName(DirectoryPath)))
            {
                DirectoryInfo dinfo = new DirectoryInfo(DirectoryPath);
                FileInfo[] Files = dinfo.GetFiles("*.txt");

                if (Files.Length == 0)
                {
                    if (Mode == 1 || Mode == 3 || Mode == 5)
                    {
                        Close();
                        //mf.TimedMessageBox(2000, String.Get("gsNoToolSaved"), String.Get("gsSaveAToolFirst"));
                        //mf.TimedMessageBox(2000, String.Get("gsNoEnvironmentSaved"), String.Get("gsSaveAnEnvironmentFirst"));
                        //mf.TimedMessageBox(2000, String.Get("gsNoVehiclesSaved"), String.Get("gsSaveAVehicleFirst"));
                    }
                    CboxTool.Enabled = false;
                }

                foreach (FileInfo file in Files)
                {
                    CboxTool.Items.Add(Path.GetFileNameWithoutExtension(file.Name));
                }
            }
            else if (Mode == 1 || Mode == 3 || Mode == 5)
            {
                Close();
                //mf.TimedMessageBox(2000, String.Get("gsNoToolSaved"), String.Get("gsSaveAToolFirst"));
                //mf.TimedMessageBox(2000, String.Get("gsNoEnvironmentSaved"), String.Get("gsSaveAnEnvironmentFirst"));
                //mf.TimedMessageBox(2000, String.Get("gsNoVehiclesSaved"), String.Get("gsSaveAVehicleFirst"));
            }
            else
            {
                CboxTool.Enabled = false;
            }
        }


        private void CboxVeh_SelectedIndexChanged(object sender, EventArgs e)
        {
            NewName = 0;
        }

        private void TboxName_TextChanged(object sender, EventArgs e)
        {
            var textboxSender = (TextBox)sender;
            var cursorPosition = textboxSender.SelectionStart;
            textboxSender.Text = Regex.Replace(textboxSender.Text, Glm.fileRegex, "");

            textboxSender.SelectionStart = cursorPosition;
            NewName = 1;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (NewName == 0)
            {
                if (Mode == 0)
                    mf.FileSaveTool(mf.toolsDirectory + CboxTool.SelectedItem.ToString() + ".txt");
                else if (Mode == 1)
                    mf.FileOpenTool(mf.toolsDirectory + CboxTool.SelectedItem.ToString() + ".txt");
                else if (Mode == 2)
                    mf.FileSaveEnvironment(mf.envDirectory + CboxTool.SelectedItem.ToString() + ".txt");
                else if (Mode == 3)
                    mf.FileOpenEnvironment(mf.envDirectory + CboxTool.SelectedItem.ToString() + ".txt");
                else if (Mode == 4)
                    mf.FileSaveVehicle(mf.vehiclesDirectory + CboxTool.SelectedItem.ToString() + ".txt");
                else if (Mode == 5)
                    mf.FileOpenVehicle(mf.vehiclesDirectory + CboxTool.SelectedItem.ToString() + ".txt");

                Close();
            }
            else if (NewName == 1 && TboxName.Text.Trim().Length > 0)
            {
                if (Mode == 0)
                    mf.FileSaveTool(mf.toolsDirectory + TboxName.Text.Trim() + ".txt");
                else if (Mode == 2)
                    mf.FileSaveEnvironment(mf.envDirectory + TboxName.Text.Trim() + ".txt");
                else if (Mode == 4)
                    mf.FileSaveVehicle(mf.vehiclesDirectory + TboxName.Text.Trim() + ".txt");
                Close();
            }
        }

        private void TboxName_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
                BtnOk.Focus();
            }
            NewName = 1;
        }

    }
}