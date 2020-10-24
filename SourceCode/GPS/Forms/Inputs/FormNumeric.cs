using System;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormNumeric : Form
    {
        private readonly double max;
        private readonly double min;
        private bool isFirstKey;
        private readonly bool WholeNumbers;
        private readonly int Decimals = 0;
        private readonly decimal Divisible = 1;

        public double ReturnValue { get; set; }

        public FormNumeric(double _min, double _max, double currentValue,Form callingForm, bool wholenumbers, int decimals, decimal divisible = -1)
        {
            Decimals = decimals;

            Owner = callingForm;
            max = _max;
            min = _min;
            InitializeComponent();
            Divisible = divisible;
            Text = String.Get("gsEnteraValue");
            //fill in the display
            tboxNumber.Text = currentValue.ToString();

            keypad1.Controls[4].Enabled = !(WholeNumbers = wholenumbers);
            keypad1.Controls[13].Enabled = _min < 0;

            isFirstKey = true;
        }

        private void FormNumeric_Load(object sender, EventArgs e)
        {
            lblMax.Text = max.ToString();
            lblMin.Text = min.ToString();
            tboxNumber.SelectionStart = tboxNumber.Text.Length;
            tboxNumber.SelectionLength = 0;
            keypad1.Focus();
        }

        private void RegisterKeypad1_ButtonPressed(object sender, KeyPressEventArgs e)
        {

            if (isFirstKey)
            {
                tboxNumber.Text = "";
                isFirstKey = false;
            }

            //clear the error as user entered new values
            if (tboxNumber.Text == String.Get("gsError"))
            {
                tboxNumber.Text = "";
                lblMin.ForeColor = SystemColors.ControlText;
                lblMax.ForeColor = SystemColors.ControlText;
            }

            int decSeparator = tboxNumber.Text.IndexOf(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);

            //if its a number just add it
            if (Char.IsNumber(e.KeyChar))
            {

                if (decSeparator < 0 || tboxNumber.Text.Length - decSeparator <= Decimals) tboxNumber.Text += e.KeyChar;



                if (double.TryParse(tboxNumber.Text, out double value))
                {
                    if (value > max)
                        tboxNumber.Text = max.ToString();
                    else if (min <= 0 && value < min)
                        tboxNumber.Text = min.ToString();
                }
            }

            //Backspace key, remove 1 char
            else if (e.KeyChar == 'B')
            {
                if (tboxNumber.Text.Length > 0)
                {
                    tboxNumber.Text = tboxNumber.Text.Remove(tboxNumber.Text.Length - 1);
                }
            }

            //decimal point
            else if (e.KeyChar == '.')
            {
                if (!WholeNumbers)
                {
                    //does it already have a decimal?
                    if (!tboxNumber.Text.Contains(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator))
                    {
                        tboxNumber.Text += Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;

                        //if decimal is first char, prefix with a zero
                        if (tboxNumber.Text.IndexOf(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator) == 0)
                        {
                            tboxNumber.Text = "0" + tboxNumber.Text;
                        }

                        //neg sign then added a decimal, insert a 0 
                        if (tboxNumber.Text.IndexOf("-") == 0 && tboxNumber.Text.IndexOf(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator) == 1)
                        {
                            tboxNumber.Text = "-0" + Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                        }
                    }
                }
            }

            //negative sign
            else if (e.KeyChar == '-')
            {
                //If already has a negative don't add again
                if (!tboxNumber.Text.Contains("-"))
                {
                    //prefix the negative sign
                    tboxNumber.Text = "-" + tboxNumber.Text;
                }
                else
                {
                    //if already has one, take it away = +/- does that
                    if (tboxNumber.Text.StartsWith("-"))
                    {
                        tboxNumber.Text = tboxNumber.Text.Substring(1);
                    }
                }
            }

            //Exit or cancel
            else if (e.KeyChar == 'X')
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }

            //clear whole display
            else if (e.KeyChar == 'C')
            {
                tboxNumber.Text = "";
            }

            //ok button
            else if (e.KeyChar == 'K')
            {
                //not ok if empty - just return
                if (tboxNumber.Text == "") return;

                //culture invariant parse to double
                double tryNumber = double.Parse(tboxNumber.Text, CultureInfo.CurrentCulture);

                //test if above or below min/max
                if (tryNumber < min)
                {
                    tboxNumber.Text = String.Get("gsError");
                    lblMin.ForeColor = Color.Red;
                }
                else if (tryNumber > max)
                {
                    tboxNumber.Text = String.Get("gsError");
                    lblMax.ForeColor = Color.Red;
                }
                else
                {
                    //all good, return the value
                    ReturnValue = tryNumber;
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }

            bool Add = (decSeparator < 0 || tboxNumber.Text.Length - decSeparator <= Decimals);

            byte[] ttt = new byte[] { 14, 15, 5, 6, 7, 8, 9, 10, 11, 12 };
            for (int i = 0; i < ttt.Length; i++)
            {
                decimal tryNumber = decimal.Parse(tboxNumber.Text + i.ToString(), CultureInfo.CurrentCulture);

                keypad1.Controls[ttt[i]].Enabled = Add && (Divisible < 0 || tryNumber % Divisible == 0);
            }

            //Show the cursor
            tboxNumber.SelectionStart = tboxNumber.Text.Length;
            tboxNumber.SelectionLength = 0;
            tboxNumber.Focus();
        }

        private void BtnDistanceUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (tboxNumber.Text == "" || tboxNumber.Text == "-" || tboxNumber.Text == String.Get("gsError")) tboxNumber.Text = "0";
            double tryNumber = double.Parse(tboxNumber.Text, CultureInfo.CurrentCulture);


            tryNumber++;

            if (tryNumber > max) tryNumber = max;

            tboxNumber.Text = tryNumber.ToString();

            isFirstKey = false;
        }

        private void BtnDistanceDn_MouseDown(object sender, MouseEventArgs e)
        {
            if (tboxNumber.Text == "" || tboxNumber.Text == "-" || tboxNumber.Text == String.Get("gsError")) tboxNumber.Text = "0";
            double tryNumber = double.Parse(tboxNumber.Text, CultureInfo.CurrentCulture);

            tryNumber--;
            if (tryNumber < min) tryNumber = min;

            tboxNumber.Text = tryNumber.ToString();

            isFirstKey = false;
        }
    }
}