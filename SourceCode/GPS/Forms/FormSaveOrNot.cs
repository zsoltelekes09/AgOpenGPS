using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormSaveOrNot : Form
    {
        //class variables

        public FormSaveOrNot()
        {
            InitializeComponent();

            this.label7.Text = gStr.gsReturn;
            this.label1.Text = gStr.gsSaveAndExit;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            //back to FormGPS
            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnReturn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Ignore;
            Close();
        }
    }
}