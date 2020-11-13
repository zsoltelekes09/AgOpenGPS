using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormShutDown : Form
    {
        public FormShutDown(Form callingForm)
        {
            Owner = callingForm;
            InitializeComponent();
        }

        private void FormShutDown_Activated(object sender, System.EventArgs e)
        {
            Left = Owner.Left + Owner.Width / 2 - Width / 2;
            Top = Owner.Top + Owner.Height / 2 - Height / 2;
        }
    }
}
