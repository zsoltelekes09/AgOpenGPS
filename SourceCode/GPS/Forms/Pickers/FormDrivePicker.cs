using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormDrivePicker : Form
    {
        private readonly FormGPS mf;

        public ListViewItem Itm { get; set; }

        public FormDrivePicker(FormGPS _mf, Form callingForm, string _fileList)
        {
            //get copy of the calling main form
            mf = _mf;
            Owner = callingForm;
            InitializeComponent();

            string[] fileList = _fileList.Split(',');
            for (int i = 0; i < fileList.Length; i++)
            {
                Itm = new ListViewItem(fileList[i]);
                lvLines.Items.Add(Itm);
            }
        }

        private void FormFilePicker_Load(object sender, EventArgs e)
        {
        }

        private void BtnOpenExistingLv_Click(object sender, EventArgs e)
        {
            int count = lvLines.SelectedItems.Count;
            if (count > 0)
            {
                mf.filePickerFileAndDirectory = (mf.fieldsDirectory + lvLines.SelectedItems[0].SubItems[0].Text + "\\Field.txt");
                Close();
            }
        }

        private void BtnDeleteAB_Click(object sender, EventArgs e)
        {
            mf.filePickerFileAndDirectory = "";
        }

    }
}