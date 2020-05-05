using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormColorPicker : Form
    {
        //class variables
        private readonly FormGPS mf = null;
        readonly Color inColor;
        public Color UseThisColor { get; set; }

        private bool isUse = true;

        public FormColorPicker(Form callingForm, Color _inColor)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();

            inColor = _inColor;

            btnNight.BackColor = inColor;
            btnDay.BackColor = inColor;

            UseThisColor = inColor;


            //this.bntOK.Text = gStr.gsForNow;
            //this.btnSave.Text = gStr.gsToFile;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ColorPick_ColorPicked(object sender, EventArgs e)
        {
            UseThisColor = colorPick.SelectedColor;
            btnNight.BackColor = colorPick.SelectedColor;
            btnDay.BackColor = colorPick.SelectedColor;
        }

        private void FormColorPicker_Load(object sender, EventArgs e)
        {
            //Properties.Settings.Default.setDisplay_customColors = "";
            {
                btn00.BackColor = Color.FromArgb(mf.customColorsList[0]);
                btn01.BackColor = Color.FromArgb(mf.customColorsList[1]);
                btn02.BackColor = Color.FromArgb(mf.customColorsList[2]);
                btn03.BackColor = Color.FromArgb(mf.customColorsList[3]);
                btn04.BackColor = Color.FromArgb(mf.customColorsList[4]);
                btn05.BackColor = Color.FromArgb(mf.customColorsList[5]);
                btn06.BackColor = Color.FromArgb(mf.customColorsList[6]);
                btn07.BackColor = Color.FromArgb(mf.customColorsList[7]);
                btn08.BackColor = Color.FromArgb(mf.customColorsList[8]);
                btn09.BackColor = Color.FromArgb(mf.customColorsList[9]);
                btn10.BackColor = Color.FromArgb(mf.customColorsList[10]);
                btn11.BackColor = Color.FromArgb(mf.customColorsList[11]);
                btn12.BackColor = Color.FromArgb(mf.customColorsList[12]);
                btn13.BackColor = Color.FromArgb(mf.customColorsList[13]);
                btn14.BackColor = Color.FromArgb(mf.customColorsList[14]);
                btn15.BackColor = Color.FromArgb(mf.customColorsList[15]);
            }

            //Properties.Settings.Default.setDisplay_customColors += mf.customColorsList[i].ToString() + ",";
            //Properties.Settings.Default.setDisplay_customColors += mf.customColorsList[15].ToString();

        }

        private void SaveCustomColor()
        {
            Properties.Settings.Default.setDisplay_customColors = "";
            for (int i = 0; i < 15; i++)
                Properties.Settings.Default.setDisplay_customColors += mf.customColorsList[i].ToString() + ",";
            Properties.Settings.Default.setDisplay_customColors += mf.customColorsList[15].ToString();

            Properties.Settings.Default.Save();
        }

        private void UpdateColor(Color col)
        {
            UseThisColor = col;
            btnNight.BackColor = col;
            btnDay.BackColor = col;
        }

        private void Btn00_Click(object sender, EventArgs e)
        {
            if (isUse)
            {
                UseThisColor = btn00.BackColor;
                UpdateColor(UseThisColor);            
            }
            else
            {
                int iCol = (UseThisColor.A << 24) | (UseThisColor.R << 16) | (UseThisColor.G << 8) | UseThisColor.B;
                (mf.customColorsList[00]) = iCol;
                btn00.BackColor = UseThisColor;
                SaveCustomColor();
            }
        }
        private void Btn01_Click(object sender, EventArgs e)
        {
            if (isUse)
            {
                UseThisColor = btn01.BackColor;
                UpdateColor(UseThisColor);
            }
            else
            {
                int iCol = (UseThisColor.A << 24) | (UseThisColor.R << 16) | (UseThisColor.G << 8) | UseThisColor.B;
                (mf.customColorsList[01]) = iCol;
                btn01.BackColor = UseThisColor;
                SaveCustomColor();
            }
        }

        private void Btn02_Click(object sender, EventArgs e)
        {
            if (isUse)
            {
                UseThisColor = btn02.BackColor;
                UpdateColor(UseThisColor);
            }
            else
            {
                // To integer
                int iCol = (UseThisColor.A << 24) | (UseThisColor.R << 16) | (UseThisColor.G << 8) | UseThisColor.B;
                (mf.customColorsList[02]) = iCol;
                btn02.BackColor = UseThisColor;
                SaveCustomColor();
            }
        }

        private void Btn03_Click(object sender, EventArgs e)
        {
            if (isUse)
            {
                UseThisColor = btn03.BackColor;
                UpdateColor(UseThisColor);
            }
            else
            {
                int iCol = (UseThisColor.A << 24) | (UseThisColor.R << 16) | (UseThisColor.G << 8) | UseThisColor.B;
                (mf.customColorsList[03]) = iCol;
                btn03.BackColor = UseThisColor;
                SaveCustomColor();
            }
        }

        private void Btn04_Click(object sender, EventArgs e)
        {
            if (isUse)
            {
                UseThisColor = btn04.BackColor;
                UpdateColor(UseThisColor);
            }
            else
            {
                int iCol = (UseThisColor.A << 24) | (UseThisColor.R << 16) | (UseThisColor.G << 8) | UseThisColor.B;
                (mf.customColorsList[04]) = iCol;
                btn04.BackColor = UseThisColor;
                SaveCustomColor();
            }
        }

        private void Btn05_Click(object sender, EventArgs e)
        {
            if (isUse)
            {
                UseThisColor = btn05.BackColor;
                UpdateColor(UseThisColor);
            }
            else
            {
                int iCol = (UseThisColor.A << 24) | (UseThisColor.R << 16) | (UseThisColor.G << 8) | UseThisColor.B;
                (mf.customColorsList[05]) = iCol;
                btn05.BackColor = UseThisColor;
                SaveCustomColor();
            }
        }

        private void Btn06_Click(object sender, EventArgs e)
        {
            if (isUse)
            {
                UseThisColor = btn06.BackColor;
                UpdateColor(UseThisColor);
            }
            else
            {
                int iCol = (UseThisColor.A << 24) | (UseThisColor.R << 16) | (UseThisColor.G << 8) | UseThisColor.B;
                (mf.customColorsList[06]) = iCol;
                btn06.BackColor = UseThisColor;
                SaveCustomColor();
            }
        }

        private void Btn07_Click(object sender, EventArgs e)
        {
            if (isUse)
            {
                UseThisColor = btn07.BackColor;
                UpdateColor(UseThisColor);
            }
            else
            {
                int iCol = (UseThisColor.A << 24) | (UseThisColor.R << 16) | (UseThisColor.G << 8) | UseThisColor.B;
                (mf.customColorsList[07]) = iCol;
                btn07.BackColor = UseThisColor;
                SaveCustomColor();
            }
        }

        private void Btn08_Click(object sender, EventArgs e)
        {
            if (isUse)
            {
                UseThisColor = btn08.BackColor;
                UpdateColor(UseThisColor);
            }
            else
            {
                int iCol = (UseThisColor.A << 24) | (UseThisColor.R << 16) | (UseThisColor.G << 8) | UseThisColor.B;
                (mf.customColorsList[08]) = iCol;
                btn08.BackColor = UseThisColor;
                SaveCustomColor();
            }
        }

        private void Btn09_Click(object sender, EventArgs e)
        {
            if (isUse)
            {
                UseThisColor = btn09.BackColor;
                UpdateColor(UseThisColor);
            }
            else
            {
                int iCol = (UseThisColor.A << 24) | (UseThisColor.R << 16) | (UseThisColor.G << 8) | UseThisColor.B;
                (mf.customColorsList[09]) = iCol;
                btn09.BackColor = UseThisColor;
                SaveCustomColor();
            }
        }

        private void Btn10_Click(object sender, EventArgs e)
        {
            if (isUse)
            {
                UseThisColor = btn10.BackColor;
                UpdateColor(UseThisColor);
            }
            else
            {
                int iCol = (UseThisColor.A << 24) | (UseThisColor.R << 16) | (UseThisColor.G << 8) | UseThisColor.B;
                (mf.customColorsList[10]) = iCol;
                btn10.BackColor = UseThisColor;
                SaveCustomColor();
            }
        }

        private void Btn11_Click(object sender, EventArgs e)
        {
            if (isUse)
            {
                UseThisColor = btn11.BackColor;
                UpdateColor(UseThisColor);
            }
            else
            {
                int iCol = (UseThisColor.A << 24) | (UseThisColor.R << 16) | (UseThisColor.G << 8) | UseThisColor.B;
                (mf.customColorsList[11]) = iCol;
                btn11.BackColor = UseThisColor;
                SaveCustomColor();
            }
        }

        private void Btn12_Click(object sender, EventArgs e)
        {
            if (isUse)
            {
                UseThisColor = btn12.BackColor;
                UpdateColor(UseThisColor);
            }
            else
            {
                int iCol = (UseThisColor.A << 24) | (UseThisColor.R << 16) | (UseThisColor.G << 8) | UseThisColor.B;
                (mf.customColorsList[12]) = iCol;
                btn12.BackColor = UseThisColor;
                SaveCustomColor();
            }
        }

        private void Btn13_Click(object sender, EventArgs e)
        {
            if (isUse)
            {
                UseThisColor = btn13.BackColor;
                UpdateColor(UseThisColor);
            }
            else
            {
                int iCol = (UseThisColor.A << 24) | (UseThisColor.R << 16) | (UseThisColor.G << 8) | UseThisColor.B;
                (mf.customColorsList[13]) = iCol;
                btn13.BackColor = UseThisColor;
                SaveCustomColor();
            }
        }

        private void Btn14_Click(object sender, EventArgs e)
        {
            if (isUse)
            {
                UseThisColor = btn14.BackColor;
                UpdateColor(UseThisColor);
            }
            else
            {
                int iCol = (UseThisColor.A << 24) | (UseThisColor.R << 16) | (UseThisColor.G << 8) | UseThisColor.B;
                (mf.customColorsList[14]) = iCol;
                btn14.BackColor = UseThisColor;
                SaveCustomColor();
            }
        }

        private void Btn15_Click(object sender, EventArgs e)
        {
            if (isUse)
            {
                UseThisColor = btn15.BackColor;
                UpdateColor(UseThisColor);
            }
            else
            {
                int iCol = (UseThisColor.A << 24) | (UseThisColor.R << 16) | (UseThisColor.G << 8) | UseThisColor.B;
                (mf.customColorsList[15]) = iCol;
                btn15.BackColor = UseThisColor;
                SaveCustomColor();
            }
        }
        private void ChkUse_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUse.Checked)
            {
                groupBox1.Text = "Pick New Color and Select Square Below to Save Preset";
                isUse = false;
            }
            else
            {
                isUse = true;
                groupBox1.Text = "Select Preset Color";
            }
        }
    }
}
            //// From integer
            //Color color = Color.FromArgb((byte)(iCol >> 24),
            //                             (byte)(iCol >> 16),
            //                             (byte)(iCol >> 8),
            //                             (byte)(iCol));
