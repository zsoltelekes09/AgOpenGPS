using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormColor : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        private bool daySet;

        //constructor
        public FormColor(Form callingForm)
        {
            //get copy of the calling main form
            Owner = mf = callingForm as FormGPS;
            InitializeComponent();

            //Language keys
            this.Text = String.Get("gsColors");
        }
        private void FormDisplaySettings_Load(object sender, EventArgs e)
        {
            daySet = mf.isDay;
        }
        private void BntOK_Click(object sender, EventArgs e)
        {
            if (daySet != mf.isDay) mf.SwapDayNightMode();
            Close();
        }

        private void BtnFrameDay_Click(object sender, EventArgs e)
        {
            if (!mf.isDay) mf.SwapDayNightMode();

            using (var form = new FormColorPicker(mf, this, mf.dayColor))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    mf.dayColor = form.UseThisColor;
                }
            }

            Properties.Settings.Default.setDisplay_colorDayMode = mf.dayColor;
            Properties.Settings.Default.Save();

            mf.SwapDayNightMode();
            mf.SwapDayNightMode();
        }

        private void BtnFrameNight_Click(object sender, EventArgs e)
        {
            if (mf.isDay) mf.SwapDayNightMode();

            using (var form = new FormColorPicker(mf, this, mf.nightColor))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    mf.nightColor = form.UseThisColor;
                }
            }

            Properties.Settings.Default.setDisplay_colorNightMode = mf.nightColor;
            Properties.Settings.Default.Save();

            mf.SwapDayNightMode();
            mf.SwapDayNightMode();
        }

        private void BtnFieldDay_Click(object sender, EventArgs e)
        {
            if (!mf.isDay) mf.SwapDayNightMode();

            using (var form = new FormColorPicker(mf, this, mf.fieldColorDay))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    mf.fieldColorDay = form.UseThisColor;
                }
            }


            Properties.Settings.Default.setDisplay_colorFieldDay = mf.fieldColorDay;
            Properties.Settings.Default.Save();
        }

        private void BtnFieldNight_Click(object sender, EventArgs e)
        {
            if (mf.isDay) mf.SwapDayNightMode();

            using (var form = new FormColorPicker(mf, this, mf.fieldColorNight))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    mf.fieldColorNight = form.UseThisColor;
                }
            }

            Properties.Settings.Default.setDisplay_colorFieldNight = mf.fieldColorNight;
            Properties.Settings.Default.Save();
        }

        private void BtnSwap_Click(object sender, EventArgs e)
        {
            mf.SwapDayNightMode();
        }
    }
}