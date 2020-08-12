using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormShortcutKeys : Form
    {
        public FormShortcutKeys(Form callingForm)
        {
            Owner = callingForm;
            InitializeComponent();
        }
    }
}