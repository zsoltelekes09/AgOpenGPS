using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormUDP : Form
    {
        //class variables
        private readonly FormGPS mf;
        int AutoSteerPort = 8888, AogPort = 9999;

        public FormUDP(Form callingForm)
        {
            //get copy of the calling main form
            Owner = mf = callingForm as FormGPS;
            InitializeComponent();
            groupBox4.Text = String.Get("gsAgOpenGPSServer");
            label11.Text = String.Get("gsAllmodulessendto");
            tboxHostName.Text = String.Get("gsHostName");
            label14.Text = String.Get("gsHost");
            label1.Text = String.Get("gsThisComputer");
            label9.Text = String.Get("gsPort");
            label7.Text = String.Get("gsYoumustRESTARTAgOpenGPS") + "\r\n";
            btnSerialOK.Text = String.Get("gsSave");
            label4.Text = String.Get("gsModulePort");
            label6.Text = String.Get("gsPort");
            groupBox1.Text = String.Get("gsModuleAdressandPorts");
            label8.Text = String.Get("gsAllmodulesuse");
            cboxIsUDPOn.Text = String.Get("gsUDPOn");
            groupBox2.Text = String.Get("gsNetworking");
            this.Text = String.Get("gsEthernetConfiguration");
        }

        private void BtnSerialOK_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.setIP_thisPort = AogPort;

            Properties.Settings.Default.setIP_autoSteerIP = tboxAutoSteerIP.Text;
            Properties.Settings.Default.setIP_autoSteerPort = AutoSteerPort;

            Properties.Settings.Default.setUDP_isOn = cboxIsUDPOn.Checked;
            Properties.Settings.Default.setUDP_isInterAppOn = cboxIsInterAppOn.Checked;

            Properties.Settings.Default.Save();
            if (cboxIsUDPOn.Checked) mf.StartUDPServer();
            else mf.StopUDPServer();

            Close();
        }

        private void BtnSerialCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormUDp_Load(object sender, EventArgs e)
        {
            string hostName = Dns.GetHostName(); // Retrieve the Name of HOST
            tboxHostName.Text = hostName;

            //IPAddress[] ipaddress = Dns.GetHostAddresses(hostName);
            tboxThisIP.Text = GetIP4Address();
            tboxAutoSteerIP.Text = Properties.Settings.Default.setIP_autoSteerIP;
            TboxAogPort.Text = (AogPort = Properties.Settings.Default.setIP_thisPort).ToString();
            TboxAutoSteerPort.Text = (AutoSteerPort = Properties.Settings.Default.setIP_autoSteerPort).ToString();

            cboxIsUDPOn.Checked = Properties.Settings.Default.setUDP_isOn;
            cboxIsInterAppOn.Checked = Properties.Settings.Default.setUDP_isInterAppOn;
        }

        //get the ipv4 address only
        public static string GetIP4Address()
        {
            string IP4Address = string.Empty;

            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily == AddressFamily.InterNetwork)
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            return IP4Address;
        }

        public bool CheckIPValid(string strIP)
        {
            //  Split string by ".", check that array length is 3
            string[] arrOctets = strIP.Split('.');

            //at least 4 groups in the IP
            if (arrOctets.Length != 4) return false;

            //  Check each substring checking that the int value is less than 255 and that is char[] length is !> 2
            const short MAXVALUE = 255;
            int temp; // Parse returns Int32
            foreach (string strOctet in arrOctets)
            {
                //check if at least 3 digits but not more OR 0 length
                if (strOctet.Length > 3 | strOctet.Length == 0) return false;

                //make sure all digits
                if (!int.TryParse(strOctet, out int temp2)) return false;

                //make sure not more then 255
                temp = int.Parse(strOctet);
                if (temp > MAXVALUE | temp < 0) return false;
            }
            return true;
        }

        private void TboxAutoSteerIP_Validating(object sender, CancelEventArgs e)
        {
            if (!CheckIPValid(tboxAutoSteerIP.Text))
            {
                tboxAutoSteerIP.Text = "127.0.0.1";
                tboxAutoSteerIP.Focus();
                mf.TimedMessageBox(2000, "Invalid IP Address", "Set to Default Local 127.0.0.1");
            }
        }

        private void TboxAutoSteerIP_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
                btnSerialCancel.Focus();
            }
        }

        private void TboxAutoSteerPort_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(1025, 65535, AutoSteerPort, this, 0, false))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxAutoSteerPort.Text = (AutoSteerPort = (int)form.ReturnValue).ToString();
                }
            }
            btnSerialOK.Focus();
        }

        private void TboxAogPort_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(1025, 65535, AogPort, this, 0, false))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxAogPort.Text = (AogPort = (int)form.ReturnValue).ToString();
                }
            }
            btnSerialOK.Focus();
        }
    }
}