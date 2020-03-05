using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormFilePicker : Form
    {
        private readonly FormGPS mf;

        private bool isOrderByName;

        public List<string> FileList { get; set; } = new List<string>();

        public FormFilePicker(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();
            btnByDistance.Text = gStr.gsSort;
            btnOpenExistingLv.Text = gStr.gsUseSelected;
        }
        private void FormFilePicker_Load(object sender, EventArgs e)
        {
            isOrderByName = true;
            ListViewItem itm;

            string[] dirs = Directory.GetDirectories(mf.fieldsDirectory);

            FileList?.Clear();

            foreach (string dir in dirs)
            {
                double latStart = 0;
                double lonStart = 0;
                double distance = 0;
                string fieldDirectory = Path.GetFileName(dir);
                string filename = dir + "\\Field.txt";
                string line;

                //make sure directory has a field.txt in it
                if (File.Exists(filename))
                {
                    using (StreamReader reader = new StreamReader(filename))
                    {
                        try
                        {
                            //Date time line
                            for (int i = 0; i < 8; i++)
                            {
                                line = reader.ReadLine();
                            }

                            //start positions
                            if (!reader.EndOfStream)
                            {
                                line = reader.ReadLine();
                                string[] offs = line.Split(',');

                                latStart = (double.Parse(offs[0], CultureInfo.InvariantCulture));
                                lonStart = (double.Parse(offs[1], CultureInfo.InvariantCulture));
                            }
                        }
                        catch (Exception)
                        {
                            mf.TimedMessageBox(2000, gStr.gsFieldFileIsCorrupt, gStr.gsChooseADifferentField);
                        }
                    }

                    distance = Math.Pow((latStart - mf.pn.latitude), 2) + Math.Pow((lonStart - mf.pn.longitude), 2);
                    distance = Math.Sqrt(distance);
                    distance *= 100;

                    FileList.Add(fieldDirectory);
                    FileList.Add(distance.ToString("00.###"));
                }
            }

            for (int i = 0; i < FileList.Count; i += 2)
            {
                string[] fieldNames = { FileList[i], FileList[i + 1] };
                itm = new ListViewItem(fieldNames);
                lvLines.Items.Add(itm);
            }

            //string fieldName = Path.GetDirectoryName(dir).ToString(CultureInfo.InvariantCulture);

            if (lvLines.Items.Count > 0)
            {
                this.chName.Text = "Field Name";
                this.chName.Width = 805;

                this.chDistance.Text = "Distance";
                this.chDistance.Width = 150;
            }
        }

        private void BtnByDistance_Click(object sender, EventArgs e)
        {
            ListViewItem itm;

            lvLines.Items.Clear();
            isOrderByName = !isOrderByName;

            for (int i = 0; i < FileList.Count; i += 2)
            {
                if (isOrderByName)
                {
                    string[] fieldNames = { FileList[i], FileList[i + 1] };
                    itm = new ListViewItem(fieldNames);
                }
                else
                {
                    string[] fieldNames = { FileList[i + 1], FileList[i] };
                    itm = new ListViewItem(fieldNames);
                }
                lvLines.Items.Add(itm);
            }

            if (lvLines.Items.Count > 0)
            {
                if (isOrderByName)
                {
                    this.chName.Text = "Field Name";
                    this.chName.Width = 805;

                    this.chDistance.Text = "Distance";
                    this.chDistance.Width = 150;
                }
                else
                {
                    this.chName.Text = "Distance";
                    this.chName.Width = 150;

                    this.chDistance.Text = "Field Name";
                    this.chDistance.Width = 805;
                }
            }
        }

        private void BtnOpenExistingLv_Click(object sender, EventArgs e)
        {
            int count = lvLines.SelectedItems.Count;
            if (count > 0)
            {
                if (isOrderByName) mf.filePickerFileAndDirectory = (mf.fieldsDirectory + lvLines.SelectedItems[0].SubItems[0].Text + "\\Field.txt");
                else mf.filePickerFileAndDirectory = (mf.fieldsDirectory + lvLines.SelectedItems[0].SubItems[1].Text + "\\Field.txt");
                Close();
            }
        }

        private void BtnDeleteAB_Click(object sender, EventArgs e)
        {
            mf.filePickerFileAndDirectory = "";
        }

    }
}