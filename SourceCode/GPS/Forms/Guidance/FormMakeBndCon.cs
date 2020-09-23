using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormMakeBndCon : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf;
        private double passes = 1, spacing = 0;

        public FormMakeBndCon(Form _mf)
        {
            Owner = mf = _mf as FormGPS;
            InitializeComponent();

            lblHz.Text = gStr.gsPass;
            label1.Text = gStr.gsSpacing;

            Text = gStr.gsMakeBoundaryContours;
            TboxPasses.Text = passes.ToString();
            TboxSpacing.Text = spacing.ToString();
        }

        private void BtnOk_Click(object sender, System.EventArgs e)
        {
            mf.ct.BuildBoundaryContours(passes, Math.Round(spacing * mf.metImp2m * 0.01,2));
            Close();
        }

        private void BtnCancel_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void TboxPasses_Enter(object sender, System.EventArgs e)
        {
            using (var form = new FormNumeric(1, 10, passes, this, true,0))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxPasses.Text = (passes = form.ReturnValue).ToString();
                }
            }
            btnCancel.Focus();
        }

        private void TboxSpacing_Enter(object sender, System.EventArgs e)
        {
            using (var form = new FormNumeric(0, Math.Round(50 * mf.m2MetImp, mf.decimals), spacing, this, mf.isMetric, mf.decimals))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxSpacing.Text = (spacing = Math.Round(form.ReturnValue * mf.m2MetImp, mf.decimals)).ToString();
                }
            }
            btnCancel.Focus();
        }
    }
}