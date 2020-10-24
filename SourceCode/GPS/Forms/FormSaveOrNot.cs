using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormSaveOrNot : Form
    {
        //class variables

        public FormSaveOrNot(bool closing,Form callingForm)
        {
            Owner = callingForm;
            InitializeComponent();

            this.label7.Text = String.Get("gsReturn");
            this.label1.Text = String.Get("gsSaveAndExit");
            this.label3.Text = String.Get("gsSaveAs");

            if (closing) btnSaveAs.Enabled = false;
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

        private void BtnSaveAs_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            Close();
        }

        private void FormSaveOrNot_Activated(object sender, EventArgs e)
        {
            Left = Owner.Left + Owner.Width / 2 - Width / 2;
            Top = Owner.Top + Owner.Height / 2 - Height / 2;
        }
    }
}