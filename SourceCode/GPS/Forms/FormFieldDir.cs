using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormFieldDir : Form
    {
        //class variables
        private readonly FormGPS mf;

        public FormFieldDir(Form _callingForm)
        {
            //get copy of the calling main form
            Owner = mf = _callingForm as FormGPS;

            InitializeComponent();

            label1.Text = String.Get("gsEnterFieldName");
            label2.Text = String.Get("gsDateWillBeAdded");
            label4.Text = String.Get("gsEnterTask");
            label5.Text = String.Get("gsEnterVehicleUsed");
            Text = String.Get("gsCreateNewField");
        }

        private void FormFieldDir_Load(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            //tboxVehicle.Text = mf.vehicleFileName + " " + mf.toolFileName;
            lblFilename.Text = "";
        }

        private void TboxFieldName_TextChanged(object sender, EventArgs e)
        {
            var textboxSender = (TextBox)sender;
            var cursorPosition = textboxSender.SelectionStart;
            textboxSender.Text = Regex.Replace(textboxSender.Text, Glm.fileRegex, "");
            textboxSender.SelectionStart = cursorPosition;

            if (string.IsNullOrEmpty(tboxFieldName.Text.Trim()))
            {
                btnSave.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
            }

            lblFilename.Text = tboxFieldName.Text.Trim() + " " + tboxTask.Text.Trim()
                + " " + tboxVehicle.Text.Trim() + " " + DateTime.Now.ToString("yyyy.MMM.dd HH_mm", CultureInfo.InvariantCulture);
        }

        private void TboxTask_TextChanged(object sender, EventArgs e)
        {
            var textboxSender = (TextBox)sender;
            var cursorPosition = textboxSender.SelectionStart;
            textboxSender.Text = Regex.Replace(textboxSender.Text, Glm.fileRegex, "");
            textboxSender.SelectionStart = cursorPosition;

            lblFilename.Text = tboxFieldName.Text.Trim() + " " + tboxTask.Text.Trim()
                + " " + tboxVehicle.Text.Trim() + " " + DateTime.Now.ToString("yyyy.MMM.dd HH_mm", CultureInfo.InvariantCulture);
        }

        private void TboxVehicle_TextChanged(object sender, EventArgs e)
        {
            var textboxSender = (TextBox)sender;
            var cursorPosition = textboxSender.SelectionStart;
            textboxSender.Text = Regex.Replace(textboxSender.Text, Glm.fileRegex, "");
            textboxSender.SelectionStart = cursorPosition;

            lblFilename.Text = tboxFieldName.Text.Trim() + " " + tboxTask.Text.Trim()
                + " " + tboxVehicle.Text.Trim() + " " + DateTime.Now.ToString("yyyy.MMM.dd HH_mm", CultureInfo.InvariantCulture);
        }

        private void BtnSerialCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            //fill something in
            if (string.IsNullOrEmpty(tboxFieldName.Text.Trim()))
            {
                Close();
                return;
            }

            //append date time to name

            mf.currentFieldDirectory = tboxFieldName.Text.Trim() + " ";

            //task
            if (!string.IsNullOrEmpty(tboxTask.Text.Trim())) mf.currentFieldDirectory += tboxTask.Text.Trim() + " ";

            //vehicle
            if (!string.IsNullOrEmpty(tboxVehicle.Text.Trim())) mf.currentFieldDirectory += tboxVehicle.Text.Trim() + " ";

            //date
            mf.currentFieldDirectory += string.Format("{0}", DateTime.Now.ToString("yyyy.MMM.dd HH_mm", CultureInfo.InvariantCulture));

            //get the directory and make sure it exists, create if not
            string dirNewField = mf.fieldsDirectory + mf.currentFieldDirectory + "\\";

            //if no template set just make a new file.
                try
                {
                    //start a new job
                    mf.JobNew();

                    //create it for first save
                    string directoryName = Path.GetDirectoryName(dirNewField);

                    if ((!string.IsNullOrEmpty(directoryName)) && (Directory.Exists(directoryName)))
                    {
                        MessageBox.Show(String.Get("gsChooseADifferentName"), String.Get("gsDirectoryExists"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    else
                    {
                        //reset the offsets
                        mf.pn.utmEast = (int)mf.pn.actualEasting;
                        mf.pn.utmNorth = (int)mf.pn.actualNorthing;

                        mf.worldGrid.CheckWorldGrid(0, 0);

                        //calculate the central meridian of current zone
                        mf.pn.centralMeridian = -177 + ((mf.pn.zone - 1) * 6);

                        //Azimuth Error - utm declination
                        mf.pn.convergenceAngle = Math.Atan(Math.Sin(Glm.ToRadians(mf.pn.latitude))
                                                    * Math.Tan(Glm.ToRadians(mf.pn.longitude - mf.pn.centralMeridian)));

                        //make sure directory exists, or create it
                        if ((!string.IsNullOrEmpty(directoryName)) && (!Directory.Exists(directoryName)))
                        { Directory.CreateDirectory(directoryName); }

                        //create the field file header info
                        mf.FileCreateField();
                        mf.FileCreateSections();
                        mf.FileCreateContour();
                    }
                }
                catch (Exception ex)
                {
                    mf.WriteErrorLog("Creating new field " + ex);

                    MessageBox.Show(String.Get("gsError"), ex.ToString());
                    mf.currentFieldDirectory = "";
                }            

            DialogResult = DialogResult.OK;
            Close();
        }

        private void TboxFieldName_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
                btnSerialCancel.Focus();
            }
        }

        private void TboxTask_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
                btnSerialCancel.Focus();
            }
        }

        private void TboxVehicle_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
                btnSerialCancel.Focus();
            }
        }
    }
}