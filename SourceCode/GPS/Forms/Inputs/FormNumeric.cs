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
            KeyPreview = true;
            Decimals = decimals;

            Owner = callingForm;
            max = _max;
            min = _min;
            InitializeComponent();
            Divisible = divisible;
            Text = String.Get("gsEnteraValue");
            //fill in the display
            tboxNumber.Text = currentValue.ToString();

            BtnSeparator.Enabled = !(WholeNumbers = wholenumbers);
            BtnPlus.Enabled = _min < 0;
            BtnSeparator.Text = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            isFirstKey = true;
        }

        private void FormNumeric_Load(object sender, EventArgs e)
        {
            lblMax.Text = max.ToString();
            lblMin.Text = min.ToString();
            tboxNumber.SelectionStart = tboxNumber.Text.Length;
            tboxNumber.SelectionLength = 0;

            BtnOk.Focus();
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

        private void BtnOk_Click(object sender, EventArgs e)
        {
            FormNumeric_KeyPress(null, new KeyPressEventArgs('\r'));
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            FormNumeric_KeyPress(null, new KeyPressEventArgs('\u001b'));
        }

        private void Btn1_Click(object sender, EventArgs e)
        {
            FormNumeric_KeyPress(null, new KeyPressEventArgs('1'));
        }
        private void Btn2_Click(object sender, EventArgs e)
        {
            FormNumeric_KeyPress(null, new KeyPressEventArgs('2'));
        }

        private void Btn3_Click(object sender, EventArgs e)
        {
            FormNumeric_KeyPress(null, new KeyPressEventArgs('3'));
        }

        private void Btn4_Click(object sender, EventArgs e)
        {
            FormNumeric_KeyPress(null, new KeyPressEventArgs('4'));
        }

        private void Btn5_Click(object sender, EventArgs e)
        {
            FormNumeric_KeyPress(null, new KeyPressEventArgs('5'));
        }

        private void Btn6_Click(object sender, EventArgs e)
        {
            FormNumeric_KeyPress(null, new KeyPressEventArgs('6'));
        }

        private void Btn7_Click(object sender, EventArgs e)
        {
            FormNumeric_KeyPress(null, new KeyPressEventArgs('7'));
        }

        private void Btn8_Click(object sender, EventArgs e)
        {
            FormNumeric_KeyPress(null, new KeyPressEventArgs('8'));
        }

        private void Btn9_Click(object sender, EventArgs e)
        {
            FormNumeric_KeyPress(null, new KeyPressEventArgs('9'));
        }

        private void Btn0_Click(object sender, EventArgs e)
        {
            FormNumeric_KeyPress(null, new KeyPressEventArgs('0'));
        }

        private void BtnSeparator_Click(object sender, EventArgs e)
        {
            FormNumeric_KeyPress(null, new KeyPressEventArgs('.'));
        }

        private void BtnPlus_Click(object sender, EventArgs e)
        {
            FormNumeric_KeyPress(null, new KeyPressEventArgs('-'));
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            FormNumeric_KeyPress(null, new KeyPressEventArgs('\b'));
        }

        private void BtnClear2_Click(object sender, EventArgs e)
        {
            FormNumeric_KeyPress(null, new KeyPressEventArgs('C'));
        }

        private void FormNumeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (isFirstKey && char.IsNumber(e.KeyChar))
            {
                tboxNumber.Text = "";
                isFirstKey = false;
            }
            isFirstKey = false;

            //clear the error as user entered new values
            if (tboxNumber.Text == String.Get("gsError"))
            {
                tboxNumber.Text = "";
                lblMin.ForeColor = SystemColors.ControlText;
                lblMax.ForeColor = SystemColors.ControlText;
            }

            int cursorPosition = tboxNumber.SelectionStart;

            if (e.KeyChar == '\b')
            {
                if (tboxNumber.Text.Length > 0 && cursorPosition > 0)
                {
                    tboxNumber.Text = tboxNumber.Text.Remove(cursorPosition - 1, 1);
                    tboxNumber.SelectionStart = cursorPosition - 1;
                }
            }
            else if (e.KeyChar == 'C')
            {
                tboxNumber.Text = "";
            }

            int decSeparator = tboxNumber.Text.IndexOf(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);

            //if its a number just add it
            if (char.IsNumber(e.KeyChar))
            {
                if (decSeparator < 0 || tboxNumber.Text.Length - decSeparator <= Decimals)
                {
                    string tt = e.KeyChar.ToString();
                    tboxNumber.Text = tboxNumber.Text.Insert(cursorPosition, tt);
                    tboxNumber.SelectionStart = ++cursorPosition;
                }

                if (double.TryParse(tboxNumber.Text, out double value))
                {
                    if (value > max)
                    {
                        tboxNumber.Text = max.ToString();
                        tboxNumber.SelectionStart = tboxNumber.Text.Length;
                    }
                    else if (min <= 0 && value < min)
                    {
                        tboxNumber.Text = min.ToString();
                        tboxNumber.SelectionStart = tboxNumber.Text.Length;
                    }
                }
            }
            else if (e.KeyChar == '.')
            {
                if (!WholeNumbers && decSeparator == -1)
                {
                    tboxNumber.Text = tboxNumber.Text.Insert(cursorPosition, Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                    tboxNumber.SelectionStart = ++cursorPosition;

                    //if decimal is first char, prefix with a zero
                    if (tboxNumber.Text.IndexOf(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator) == 0)
                    {
                        tboxNumber.Text = "0" + tboxNumber.Text;
                        tboxNumber.SelectionStart = tboxNumber.Text.Length;
                    }

                    //neg sign then added a decimal, insert a 0 
                    if (tboxNumber.Text.IndexOf("-") == 0 && tboxNumber.Text.IndexOf(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator) == 1)
                    {
                        tboxNumber.Text = "-0" + Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                        tboxNumber.SelectionStart = tboxNumber.Text.Length;
                    
                    }
                    decSeparator = tboxNumber.Text.IndexOf(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                }
            }
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
            else if (e.KeyChar == '\u001b')
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
            else if (e.KeyChar == '\r')
            {
                //not ok if empty - just return
                if (tboxNumber.Text == "") return;

                //culture invariant parse to double
                double tryNumber = double.Parse(tboxNumber.Text, CultureInfo.CurrentCulture);

                //test if above or below min/max
                if (tryNumber < min)
                {
                    tboxNumber.Text = min.ToString();
                    lblMin.ForeColor = Color.Red;
                    return;
                }
                else if (tryNumber > max)
                {
                    tboxNumber.Text = max.ToString();
                    lblMax.ForeColor = Color.Red;
                    return;
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
            for (int i = 0; i < 10; i++)
            {
                decimal tryNumber = decimal.Parse(tboxNumber.Text + i.ToString(), CultureInfo.CurrentCulture);
                Controls["Btn" + i].Enabled = Add && (Divisible < 0 || tryNumber % Divisible == 0);
            }
            Controls["BtnSeparator"].Enabled = decSeparator == -1 && !WholeNumbers;

            tboxNumber.SelectionLength = 0;
            tboxNumber.Focus();
        }

        private void tboxNumber_Click(object sender, EventArgs e)
        {
            isFirstKey = false;
        }

        private void FormNumeric_Activated(object sender, EventArgs e)
        {
            Left = Owner.Left + Owner.Width / 2 - Width / 2;
            Top = Owner.Top + Owner.Height / 2 - Height / 2;
        }
    }
}