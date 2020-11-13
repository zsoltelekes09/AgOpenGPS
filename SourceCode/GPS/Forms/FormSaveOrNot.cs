using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormSaveOrNot : Form
    {
        public FormSaveOrNot(bool closing,Form callingForm)
        {
            Owner = callingForm;
            InitializeComponent();

            label7.Text = String.Get("gsReturn");
            label1.Text = String.Get("gsSaveAndExit");
            label3.Text = String.Get("gsSaveAs");

            if (closing) btnSaveAs.Enabled = false;
        }

        private void FormSaveOrNot_Activated(object sender, EventArgs e)
        {
            Left = Owner.Left + Owner.Width / 2 - Width / 2;
            Top = Owner.Top + Owner.Height / 2 - Height / 2;
        }

        private void FormSaveOrNot_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
                DialogResult = DialogResult.OK;
        }
    }
}