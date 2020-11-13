using AgOpenGPS.Properties;
using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormYouTurn : Form
    {
        //properties
        private readonly FormGPS mf;

        //strings for comboboxes past auto and manual choices
        //pos0 is "-" no matter what

        public FormYouTurn(Form callingForm)
        {
            Owner = mf = callingForm as FormGPS;

            //winform initialization
            InitializeComponent();

            this.Text = String.Get("gsUTurn");

          // U Turn tab
            tabYouTurn.Text = String.Get("gsUTurn");
            btnYouTurnCustom.Text = String.Get("gsCustom");
            btnTurnAllOff.Text = String.Get("gsTurnallOff");
            label46.Text = String.Get("gsDubins") + " " + String.Get("gsPattern");
            label1.Text = String.Get("gsSkips");
            btnIsUsingDubins.Text = String.Get("gsDubins");
            label48.Text = String.Get("gsUTurnDistance") + " " + String.Get("gsFromBounds");
            label49.Text = String.Get("gsGeoFenceDist") + " " + String.Get("gsFromBounds");
            groupBox1.Text = String.Get("gsTurnPatterns");
            lblWhenTrig.Text = String.Get("gsUTurnLength");
            btnYouTurnRecord.Text = String.Get("gsRecord");

          // Entry tab
            tabEnter.Text = String.Get("gsEntry");
            label13.Text = String.Get("gsFunction");
            label15.Text = String.Get("gsAction");
            label14.Text = String.Get("gsDistance");


            object[] tt = new object[] { String.Get("gsTurnOff"), String.Get("gsTurnOn") };

            cboxEnterAction0.Items.AddRange(tt);
            cboxEnterAction1.Items.AddRange(tt);
            cboxEnterAction2.Items.AddRange(tt);
            cboxEnterAction3.Items.AddRange(tt);
            cboxEnterAction4.Items.AddRange(tt);
            cboxEnterAction5.Items.AddRange(tt);
            cboxEnterAction6.Items.AddRange(tt);
            cboxEnterAction7.Items.AddRange(tt);

          // Exit tab
            tabExit.Text = String.Get("gsExit");
            label16.Text = String.Get("gsAction");
            label18.Text = String.Get("gsFunction");
            label17.Text = String.Get("gsDistance");

            cboxExitAction0.Items.AddRange(tt);
            cboxExitAction1.Items.AddRange(tt);
            cboxExitAction2.Items.AddRange(tt);
            cboxExitAction3.Items.AddRange(tt);
            cboxExitAction4.Items.AddRange(tt);
            cboxExitAction5.Items.AddRange(tt);
            cboxExitAction6.Items.AddRange(tt);
            cboxExitAction7.Items.AddRange(tt);


            // Edit tab
            tabEdit.Text = String.Get("gsEdit");
            label44.Text = String.Get("gsCancel");
            label43.Text = String.Get("gsSaveNames");


        }

        private void FormYouTurn_Load(object sender, EventArgs e)
        {
            //Fill in the strings for comboboxes - editable
            string line = Vehicle.Default.seq_FunctionList;
            string[] words = line.Split(',');

            //set button text and background color
            btnToggle3.Text = mf.seq.pos3 = words[0];
            btnToggle4.Text = mf.seq.pos4 = words[1];
            btnToggle5.Text = mf.seq.pos5 = words[2];
            btnToggle6.Text = mf.seq.pos6 = words[3];
            btnToggle7.Text = mf.seq.pos7 = words[4];
            btnToggle8.Text = mf.seq.pos8 = words[5];
            FunctionButtonsOnOff();

            //the drop down lists
            LoadComboStrings();

            //the edit page of text boxes
            LoadEditFunctionNames();


            btnYouTurnCustom.BackColor = Color.Silver;
            btnYouTurnKeyHole.BackColor = Color.Silver;
            btnYouTurnSemiCircle.BackColor = Color.Silver;
            btnYouTurnWideReturn.BackColor = Color.Silver;

            if (Settings.Default.setAS_youTurnShape == "Custom")
                btnYouTurnCustom.BackColor = Color.LimeGreen;
            else if (Settings.Default.setAS_youTurnShape == "KeyHole")
                btnYouTurnKeyHole.BackColor = Color.LimeGreen;
            else if (Settings.Default.setAS_youTurnShape == "SemiCircle")
                btnYouTurnSemiCircle.BackColor = Color.LimeGreen;
            else if (Settings.Default.setAS_youTurnShape == "WideReturn")
                btnYouTurnWideReturn.BackColor = Color.LimeGreen;

            cboxRowWidth.SelectedIndex = mf.yt.rowSkipsWidth - 1;

            //populate the Enter and Exit pages.
            PopulateSequencePages();

            lblDistance.Text = mf.yt.youTurnStartOffset.ToString();
            if (mf.yt.isYouTurnBtnOn)
            {
                lblDistance.Text = Math.Abs(mf.yt.youTurnStartOffset).ToString() + " m";
            }
            else
            {
                lblDistance.Text = Math.Abs(mf.yt.youTurnStartOffset).ToString() + " m";
            }

            lblTriggerDistance.Text = mf.yt.triggerDistanceOffset.ToString() + "m";
            lblGeoFenceDistance.Text = mf.yt.geoFenceDistance + "m";

            //update dubins button
            if (mf.yt.YouTurnType == 0)
            {
                btnIsUsingDubins.Text = String.Get("gsPattern");
                btnIsUsingDubins.BackColor = Color.Salmon;
                btnYouTurnCustom.Enabled = true;
                btnYouTurnKeyHole.Enabled = true;
                btnYouTurnRecord.Enabled = true;
                btnYouTurnWideReturn.Enabled = true;
                btnYouTurnSemiCircle.Enabled = true;
            }
            else
            {
                btnIsUsingDubins.Text = String.Get("gsDubins") + (mf.yt.YouTurnType > 1 ? " Curve" : "");
                btnIsUsingDubins.BackColor = Color.LightGreen;
                btnYouTurnCustom.Enabled = false;
                btnYouTurnKeyHole.Enabled = false;
                btnYouTurnRecord.Enabled = false;
                btnYouTurnWideReturn.Enabled = false;
                btnYouTurnSemiCircle.Enabled = false;
            }
        }

        #region Procedures

        private void PopulateSequencePages()
        {
            if ((cboxEnterFunc0.SelectedIndex = mf.seq.seqEnter[0].function) == 0)
            {
                cboxEnterAction0.SelectedIndex = -1;
            }
            else
            {
                cboxEnterAction0.SelectedIndex = mf.seq.seqEnter[0].action;
                nudEnter0.Value = (decimal)mf.seq.seqEnter[0].distance;
            }

            if ((cboxEnterFunc1.SelectedIndex = mf.seq.seqEnter[1].function) == 0)
            {
                cboxEnterAction1.SelectedIndex = -1;
            }
            else
            {
                cboxEnterAction1.SelectedIndex = mf.seq.seqEnter[1].action;
                nudEnter1.Value = (decimal)mf.seq.seqEnter[1].distance;
            }

            if (mf.seq.seqEnter[2].function == 0)
            {
                cboxEnterFunc2.SelectedIndex = mf.seq.seqEnter[2].function;
                cboxEnterAction2.SelectedIndex = -1;
            }
            else
            {
                cboxEnterFunc2.SelectedIndex = mf.seq.seqEnter[2].function;
                cboxEnterAction2.SelectedIndex = mf.seq.seqEnter[2].action;
                nudEnter2.Value = (decimal)mf.seq.seqEnter[2].distance;
            }

            if (mf.seq.seqEnter[3].function == 0)
            {
                cboxEnterFunc3.SelectedIndex = mf.seq.seqEnter[3].function;
                cboxEnterAction3.SelectedIndex = -1;
            }
            else
            {
                cboxEnterFunc3.SelectedIndex = mf.seq.seqEnter[3].function;
                cboxEnterAction3.SelectedIndex = mf.seq.seqEnter[3].action;
                nudEnter3.Value = (decimal)mf.seq.seqEnter[3].distance;
            }

            if (mf.seq.seqEnter[4].function == 0)
            {
                cboxEnterFunc4.SelectedIndex = mf.seq.seqEnter[4].function;
                cboxEnterAction4.SelectedIndex = -1;
            }
            else
            {
                cboxEnterFunc4.SelectedIndex = mf.seq.seqEnter[4].function;
                cboxEnterAction4.SelectedIndex = mf.seq.seqEnter[4].action;
                nudEnter4.Value = (decimal)mf.seq.seqEnter[4].distance;
            }

            if (mf.seq.seqEnter[5].function == 0)
            {
                cboxEnterFunc5.SelectedIndex = mf.seq.seqEnter[5].function;
                cboxEnterAction5.SelectedIndex = -1;
            }
            else
            {
                cboxEnterFunc5.SelectedIndex = mf.seq.seqEnter[5].function;
                cboxEnterAction5.SelectedIndex = mf.seq.seqEnter[5].action;
                nudEnter5.Value = (decimal)mf.seq.seqEnter[5].distance;
            }

            if (mf.seq.seqEnter[6].function == 0)
            {
                cboxEnterFunc6.SelectedIndex = mf.seq.seqEnter[6].function;
                cboxEnterAction6.SelectedIndex = -1;
            }
            else
            {
                cboxEnterFunc6.SelectedIndex = mf.seq.seqEnter[6].function;
                cboxEnterAction6.SelectedIndex = mf.seq.seqEnter[6].action;
                nudEnter6.Value = (decimal)mf.seq.seqEnter[6].distance;
            }

            if (mf.seq.seqEnter[7].function == 0)
            {
                cboxEnterFunc7.SelectedIndex = mf.seq.seqEnter[7].function;
                cboxEnterAction7.SelectedIndex = -1;
            }
            else
            {
                cboxEnterFunc7.SelectedIndex = mf.seq.seqEnter[7].function;
                cboxEnterAction7.SelectedIndex = mf.seq.seqEnter[7].action;
                nudEnter7.Value = (decimal)mf.seq.seqEnter[7].distance;
            }

            //Exit page

            if (mf.seq.seqExit[0].function == 0)
            {
                cboxExitFunc0.SelectedIndex = mf.seq.seqExit[0].function;
                cboxExitAction0.SelectedIndex = -1;
            }
            else
            {
                cboxExitFunc0.SelectedIndex = mf.seq.seqExit[0].function;
                cboxExitAction0.SelectedIndex = mf.seq.seqExit[0].action;
                nudExit0.Value = (decimal)mf.seq.seqExit[0].distance;
            }

            if (mf.seq.seqExit[1].function == 0)
            {
                cboxExitFunc1.SelectedIndex = mf.seq.seqExit[1].function;
                cboxExitAction1.SelectedIndex = -1;
            }
            else
            {
                cboxExitFunc1.SelectedIndex = mf.seq.seqExit[1].function;
                cboxExitAction1.SelectedIndex = mf.seq.seqExit[1].action;
                nudExit1.Value = (decimal)mf.seq.seqExit[1].distance;
            }

            if (mf.seq.seqExit[2].function == 0)
            {
                cboxExitFunc2.SelectedIndex = mf.seq.seqExit[2].function;
                cboxExitAction2.SelectedIndex = -1;
            }
            else
            {
                cboxExitFunc2.SelectedIndex = mf.seq.seqExit[2].function;
                cboxExitAction2.SelectedIndex = mf.seq.seqExit[2].action;
                nudExit2.Value = (decimal)mf.seq.seqExit[2].distance;
            }

            if (mf.seq.seqExit[3].function == 0)
            {
                cboxExitFunc3.SelectedIndex = mf.seq.seqExit[3].function;
                cboxExitAction3.SelectedIndex = -1;
            }
            else
            {
                cboxExitFunc3.SelectedIndex = mf.seq.seqExit[3].function;
                cboxExitAction3.SelectedIndex = mf.seq.seqExit[3].action;
                nudExit3.Value = (decimal)mf.seq.seqExit[3].distance;
            }

            if (mf.seq.seqExit[4].function == 0)
            {
                cboxExitFunc4.SelectedIndex = mf.seq.seqExit[4].function;
                cboxExitAction4.SelectedIndex = -1;
            }
            else
            {
                cboxExitFunc4.SelectedIndex = mf.seq.seqExit[4].function;
                cboxExitAction4.SelectedIndex = mf.seq.seqExit[4].action;
                nudExit4.Value = (decimal)mf.seq.seqExit[4].distance;
            }

            if (mf.seq.seqExit[5].function == 0)
            {
                cboxExitFunc5.SelectedIndex = mf.seq.seqExit[5].function;
                cboxExitAction5.SelectedIndex = -1;
            }
            else
            {
                cboxExitFunc5.SelectedIndex = mf.seq.seqExit[5].function;
                cboxExitAction5.SelectedIndex = mf.seq.seqExit[5].action;
                nudExit5.Value = (decimal)mf.seq.seqExit[5].distance;
            }

            if (mf.seq.seqExit[6].function == 0)
            {
                cboxExitFunc6.SelectedIndex = mf.seq.seqExit[6].function;
                cboxExitAction6.SelectedIndex = -1;
            }
            else
            {
                cboxExitFunc6.SelectedIndex = mf.seq.seqExit[6].function;
                cboxExitAction6.SelectedIndex = mf.seq.seqExit[6].action;
                nudExit6.Value = (decimal)mf.seq.seqExit[6].distance;
            }

            if (mf.seq.seqExit[7].function == 0)
            {
                cboxExitFunc7.SelectedIndex = mf.seq.seqExit[7].function;
                cboxExitAction7.SelectedIndex = -1;
            }
            else
            {
                cboxExitFunc7.SelectedIndex = mf.seq.seqExit[7].function;
                cboxExitAction7.SelectedIndex = mf.seq.seqExit[7].action;
                nudExit7.Value = (decimal)mf.seq.seqExit[7].distance;
            }
        }

        private void SaveSequences()
        {
            //first the entry save
            if (cboxEnterFunc0.SelectedIndex == 0)
            {
                mf.seq.DisableSeqEvent(0, true);
            }
            else
            {
                mf.seq.seqEnter[0].function = cboxEnterFunc0.SelectedIndex;
                mf.seq.seqEnter[0].action = cboxEnterAction0.SelectedIndex;
                mf.seq.seqEnter[0].isTrig = false;
                mf.seq.seqEnter[0].distance = (int)nudEnter0.Value;
            }

            if (cboxEnterFunc1.SelectedIndex == 0)
            {
                mf.seq.DisableSeqEvent(1, true);
            }
            else
            {
                mf.seq.seqEnter[1].function = cboxEnterFunc1.SelectedIndex;
                mf.seq.seqEnter[1].action = cboxEnterAction1.SelectedIndex;
                mf.seq.seqEnter[1].isTrig = false;
                mf.seq.seqEnter[1].distance = (int)nudEnter1.Value;
            }

            if (cboxEnterFunc2.SelectedIndex == 0)
            {
                mf.seq.DisableSeqEvent(2, true);
            }
            else
            {
                mf.seq.seqEnter[2].function = cboxEnterFunc2.SelectedIndex;
                mf.seq.seqEnter[2].action = cboxEnterAction2.SelectedIndex;
                mf.seq.seqEnter[2].isTrig = false;
                mf.seq.seqEnter[2].distance = (int)nudEnter2.Value;
            }

            if (cboxEnterFunc3.SelectedIndex == 0)
            {
                mf.seq.DisableSeqEvent(3, true);
            }
            else
            {
                mf.seq.seqEnter[3].function = cboxEnterFunc3.SelectedIndex;
                mf.seq.seqEnter[3].action = cboxEnterAction3.SelectedIndex;
                mf.seq.seqEnter[3].isTrig = false;
                mf.seq.seqEnter[3].distance = (int)nudEnter3.Value;
            }

            if (cboxEnterFunc4.SelectedIndex == 0)
            {
                mf.seq.DisableSeqEvent(4, true);
            }
            else
            {
                mf.seq.seqEnter[4].function = cboxEnterFunc4.SelectedIndex;
                mf.seq.seqEnter[4].action = cboxEnterAction4.SelectedIndex;
                mf.seq.seqEnter[4].isTrig = false;
                mf.seq.seqEnter[4].distance = (int)nudEnter4.Value;
            }

            if (cboxEnterFunc5.SelectedIndex == 0)
            {
                mf.seq.DisableSeqEvent(5, true);
            }
            else
            {
                mf.seq.seqEnter[5].function = cboxEnterFunc5.SelectedIndex;
                mf.seq.seqEnter[5].action = cboxEnterAction5.SelectedIndex;
                mf.seq.seqEnter[5].isTrig = false;
                mf.seq.seqEnter[5].distance = (int)nudEnter5.Value;
            }

            if (cboxEnterFunc6.SelectedIndex == 0)
            {
                mf.seq.DisableSeqEvent(6, true);
            }
            else
            {
                mf.seq.seqEnter[6].function = cboxEnterFunc6.SelectedIndex;
                mf.seq.seqEnter[6].action = cboxEnterAction6.SelectedIndex;
                mf.seq.seqEnter[6].isTrig = false;
                mf.seq.seqEnter[6].distance = (int)nudEnter6.Value;
            }

            if (cboxEnterFunc7.SelectedIndex == 0)
            {
                mf.seq.DisableSeqEvent(7, true);
            }
            else
            {
                mf.seq.seqEnter[7].function = cboxEnterFunc7.SelectedIndex;
                mf.seq.seqEnter[7].action = cboxEnterAction7.SelectedIndex;
                mf.seq.seqEnter[7].isTrig = false;
                mf.seq.seqEnter[7].distance = (int)nudEnter7.Value;
            }

            //save the exit fields
            if (cboxExitFunc0.SelectedIndex == 0)
            {
                mf.seq.DisableSeqEvent(0, false);
            }
            else
            {
                mf.seq.seqExit[0].function = cboxExitFunc0.SelectedIndex;
                mf.seq.seqExit[0].action = cboxExitAction0.SelectedIndex;
                mf.seq.seqExit[0].isTrig = false;
                mf.seq.seqExit[0].distance = (int)nudExit0.Value;
            }

            if (cboxExitFunc1.SelectedIndex == 0)
            {
                mf.seq.DisableSeqEvent(1, false);
            }
            else
            {
                mf.seq.seqExit[1].function = cboxExitFunc1.SelectedIndex;
                mf.seq.seqExit[1].action = cboxExitAction1.SelectedIndex;
                mf.seq.seqExit[1].isTrig = false;
                mf.seq.seqExit[1].distance = (int)nudExit1.Value;
            }

            if (cboxExitFunc2.SelectedIndex == 0)
            {
                mf.seq.DisableSeqEvent(2, false);
            }
            else
            {
                mf.seq.seqExit[2].function = cboxExitFunc2.SelectedIndex;
                mf.seq.seqExit[2].action = cboxExitAction2.SelectedIndex;
                mf.seq.seqExit[2].isTrig = false;
                mf.seq.seqExit[2].distance = (int)nudExit2.Value;
            }

            if (cboxExitFunc3.SelectedIndex == 0)
            {
                mf.seq.DisableSeqEvent(3, false);
            }
            else
            {
                mf.seq.seqExit[3].function = cboxExitFunc3.SelectedIndex;
                mf.seq.seqExit[3].action = cboxExitAction3.SelectedIndex;
                mf.seq.seqExit[3].isTrig = false;
                mf.seq.seqExit[3].distance = (int)nudExit3.Value;
            }

            if (cboxExitFunc4.SelectedIndex == 0)
            {
                mf.seq.DisableSeqEvent(4, false);
            }
            else
            {
                mf.seq.seqExit[4].function = cboxExitFunc4.SelectedIndex;
                mf.seq.seqExit[4].action = cboxExitAction4.SelectedIndex;
                mf.seq.seqExit[4].isTrig = false;
                mf.seq.seqExit[4].distance = (int)nudExit4.Value;
            }

            if (cboxExitFunc5.SelectedIndex == 0)
            {
                mf.seq.DisableSeqEvent(5, false);
            }
            else
            {
                mf.seq.seqExit[5].function = cboxExitFunc5.SelectedIndex;
                mf.seq.seqExit[5].action = cboxExitAction5.SelectedIndex;
                mf.seq.seqExit[5].isTrig = false;
                mf.seq.seqExit[5].distance = (int)nudExit5.Value;
            }

            if (cboxExitFunc6.SelectedIndex == 0)
            {
                mf.seq.DisableSeqEvent(6, false);
            }
            else
            {
                mf.seq.seqExit[6].function = cboxExitFunc6.SelectedIndex;
                mf.seq.seqExit[6].action = cboxExitAction6.SelectedIndex;
                mf.seq.seqExit[6].isTrig = false;
                mf.seq.seqExit[6].distance = (int)nudExit6.Value;
            }

            if (cboxExitFunc7.SelectedIndex == 0)
            {
                mf.seq.DisableSeqEvent(7, false);
            }
            else
            {
                mf.seq.seqExit[7].function = cboxExitFunc7.SelectedIndex;
                mf.seq.seqExit[7].action = cboxExitAction7.SelectedIndex;
                mf.seq.seqExit[7].isTrig = false;
                mf.seq.seqExit[7].distance = (int)nudExit7.Value;
            }
        }

        private void LoadComboStrings()
        {
            object[] tt = Vehicle.Default.seq_FunctionList.Split(',');

            cboxEnterFunc0.Items.AddRange(tt);
            cboxExitFunc0.Items.AddRange(tt);

            cboxEnterFunc1.Items.AddRange(tt);
            cboxExitFunc1.Items.AddRange(tt);

            cboxEnterFunc2.Items.AddRange(tt);
            cboxExitFunc2.Items.AddRange(tt);

            cboxEnterFunc3.Items.AddRange(tt);
            cboxExitFunc3.Items.AddRange(tt);

            cboxEnterFunc4.Items.AddRange(tt);
            cboxExitFunc4.Items.AddRange(tt);

            cboxEnterFunc5.Items.AddRange(tt);
            cboxExitFunc5.Items.AddRange(tt);

            cboxEnterFunc6.Items.AddRange(tt);
            cboxExitFunc6.Items.AddRange(tt);

            cboxEnterFunc7.Items.AddRange(tt);
            cboxExitFunc7.Items.AddRange(tt);
        }

        private void LoadEditFunctionNames()
        {
            tboxPos1.Text = mf.seq.pos1;
            tboxPos2.Text = mf.seq.pos2;
            tboxPos3.Text = mf.seq.pos3;
            tboxPos4.Text = mf.seq.pos4;
            tboxPos5.Text = mf.seq.pos5;
            tboxPos6.Text = mf.seq.pos6;
            tboxPos7.Text = mf.seq.pos7;
            tboxPos8.Text = mf.seq.pos8;
        }

        #endregion Procedures

        #region YouTurn

        // YouTurn Tab

        private void BtnYouTurnKeyHole_Click(object sender, EventArgs e)
        {
            mf.yt.LoadYouTurnShapeFromData(Settings.Default.KeyHole);
            Settings.Default.setAS_youTurnShape = "KeyHole";
            Settings.Default.Save();
            btnYouTurnKeyHole.BackColor = Color.LimeGreen;
            btnYouTurnSemiCircle.BackColor = Color.Silver;
            btnYouTurnCustom.BackColor = Color.Silver;
            btnYouTurnWideReturn.BackColor = Color.Silver;
        }

        private void BtnYouTurnSemiCircle_Click(object sender, EventArgs e)
        {
            mf.yt.LoadYouTurnShapeFromData(Settings.Default.SemiCircle);
            Settings.Default.setAS_youTurnShape = "SemiCircle";
            Settings.Default.Save();
            btnYouTurnKeyHole.BackColor = Color.Silver;
            btnYouTurnSemiCircle.BackColor = Color.LimeGreen;
            btnYouTurnCustom.BackColor = Color.Silver;
            btnYouTurnWideReturn.BackColor = Color.Silver;
        }

        private void BtnYouTurnWideReturn_Click(object sender, EventArgs e)
        {
            mf.yt.LoadYouTurnShapeFromData(Settings.Default.WideReturn);
            Settings.Default.setAS_youTurnShape = "WideReturn";
            Settings.Default.Save();
            btnYouTurnKeyHole.BackColor = Color.Silver;
            btnYouTurnSemiCircle.BackColor = Color.Silver;
            btnYouTurnCustom.BackColor = Color.Silver;
            btnYouTurnWideReturn.BackColor = Color.LimeGreen;
        }

        private void BtnYouTurnCustom_Click(object sender, EventArgs e)
        {
            mf.yt.LoadYouTurnShapeFromData(Settings.Default.Custom);
            Settings.Default.setAS_youTurnShape = "Custom";
            Settings.Default.Save();
            btnYouTurnKeyHole.BackColor = Color.Silver;
            btnYouTurnSemiCircle.BackColor = Color.Silver;
            btnYouTurnCustom.BackColor = Color.LimeGreen;
            btnYouTurnWideReturn.BackColor = Color.Silver;
        }

        private void BtnYouTurnRecord_Click(object sender, EventArgs e)
        {
            if (mf.ABLines.BtnABLineOn)
            {
                Form form = new FormYouTurnRecord(mf);

                form.Show(Owner);

                Close();
            }
            else { mf.TimedMessageBox(3000, "No AB Lines", "Start AB Line Guidance"); }
        }

        private void BtnIsUsingDubins_Click(object sender, EventArgs e)
        {
            if (++mf.yt.YouTurnType > 2) mf.yt.YouTurnType = 0;

            if (mf.yt.YouTurnType == 0)
            {
                btnIsUsingDubins.Text = String.Get("gsPattern");
                btnIsUsingDubins.BackColor = Color.Salmon;
                btnYouTurnCustom.Enabled = true;
                btnYouTurnKeyHole.Enabled = true;
                btnYouTurnRecord.Enabled = true;
                btnYouTurnWideReturn.Enabled = true;
                btnYouTurnSemiCircle.Enabled = true;
            }
            else
            {
                btnIsUsingDubins.Text = String.Get("gsDubins") + (mf.yt.YouTurnType > 1 ? " Curve" : "");
                btnIsUsingDubins.BackColor = Color.LightGreen;
                btnYouTurnCustom.Enabled = false;
                btnYouTurnKeyHole.Enabled = false;
                btnYouTurnRecord.Enabled = false;
                btnYouTurnWideReturn.Enabled = false;
                btnYouTurnSemiCircle.Enabled = false;
            }
        }

        private void CboxRowWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            mf.yt.rowSkipsWidth = cboxRowWidth.SelectedIndex + 1;
        }

        #endregion YouTurn

        #region distance

        private void BtnTriggerDistanceDn_MouseDown(object sender, MouseEventArgs e)
        {
            if (--mf.yt.triggerDistanceOffset < 0) mf.yt.triggerDistanceOffset = 0;
            lblTriggerDistance.Text = mf.yt.triggerDistanceOffset.ToString() + "m";
        }

        private void BtnTriggerDistanceUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (++mf.yt.triggerDistanceOffset > 50) mf.yt.triggerDistanceOffset = 50;
            lblTriggerDistance.Text = mf.yt.triggerDistanceOffset.ToString() + "m";
        }

        private void BtnGeoFenceDistanceDn_MouseDown(object sender, MouseEventArgs e)
        {
            if (--mf.yt.geoFenceDistance < 0) mf.yt.geoFenceDistance = 0;
            lblGeoFenceDistance.Text = mf.yt.geoFenceDistance.ToString() + "m";
        }

        private void BtnGeoFenceDistanceUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (++mf.yt.geoFenceDistance > 50) mf.yt.geoFenceDistance = 50;
            lblGeoFenceDistance.Text = mf.yt.geoFenceDistance.ToString() + "m";
        }

        private void BtnDistanceDn_MouseDown(object sender, MouseEventArgs e)
        {
            if (--mf.yt.youTurnStartOffset < 0) mf.yt.youTurnStartOffset = 0;
            lblDistance.Text = Math.Abs(mf.yt.youTurnStartOffset).ToString() + " m";
        }

        private void BtnDistanceUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (++mf.yt.youTurnStartOffset > 50) mf.yt.youTurnStartOffset = 50;
            lblDistance.Text = Math.Abs(mf.yt.youTurnStartOffset).ToString() + " m";
        }

        #endregion distance

        #region Sequence select

        private void Cbox_IndexChanged(object sender, EventArgs e)
        {
            ComboBox ComboBox = sender as ComboBox;

            if (ComboBox != null && ComboBox.SelectedIndex == 0)
            {
                ComboBox.SelectedIndex = -1;
                bool Exit = ComboBox.Name.Substring(0, ComboBox.Name.Length - 5).EndsWith("Exit");
                Control[] Controls = this.Controls.Find("cbox" + (Exit ? "Exit" : "Enter") + "Action" + ComboBox.Name[ComboBox.Name.Length - 1], true);
                if (Controls.Length > 0)
                {
                    ComboBox = Controls[0] as ComboBox;
                    if (ComboBox != null)
                    {
                        ComboBox.SelectedIndex = -1;

                    }
                }

                Controls = this.Controls.Find("nud" + (Exit ? "Exit" : "Enter") + ComboBox.Name[ComboBox.Name.Length - 1], true);
                if (Controls.Length > 0)
                {
                    NumericUpDown NumBox = Controls[0] as NumericUpDown;
                    if (NumBox != null)
                    {
                        NumBox.Value = 0;
                    }
                }

            }
        }

        #endregion Sequence select

        #region Edit names

        private void BtnSaveNames_Click(object sender, EventArgs e)
        {
            //pos1 = tboxPos1.Text; pos2 = tboxPos2.Text; auto manual buttons are read only
            mf.seq.pos3 = tboxPos3.Text;
            mf.seq.pos4 = tboxPos4.Text;
            mf.seq.pos5 = tboxPos5.Text;
            mf.seq.pos6 = tboxPos6.Text;
            mf.seq.pos7 = tboxPos7.Text;
            mf.seq.pos8 = tboxPos8.Text;

            //clear everything out
            cboxEnterFunc0.Items.Clear();
            cboxEnterFunc1.Items.Clear();
            cboxEnterFunc2.Items.Clear();
            cboxEnterFunc3.Items.Clear();
            cboxEnterFunc4.Items.Clear();
            cboxEnterFunc5.Items.Clear();
            cboxEnterFunc6.Items.Clear();
            cboxEnterFunc7.Items.Clear();
            cboxExitFunc0.Items.Clear();
            cboxExitFunc1.Items.Clear();
            cboxExitFunc2.Items.Clear();
            cboxExitFunc3.Items.Clear();
            cboxExitFunc4.Items.Clear();
            cboxExitFunc5.Items.Clear();
            cboxExitFunc6.Items.Clear();
            cboxExitFunc7.Items.Clear();

            //add the dash, item 0
            cboxEnterFunc0.Items.Add(" ");
            cboxEnterFunc1.Items.Add(" ");
            cboxEnterFunc2.Items.Add(" ");
            cboxEnterFunc3.Items.Add(" ");
            cboxEnterFunc4.Items.Add(" ");
            cboxEnterFunc5.Items.Add(" ");
            cboxEnterFunc6.Items.Add(" ");
            cboxEnterFunc7.Items.Add(" ");
            cboxExitFunc0.Items.Add(" ");
            cboxExitFunc1.Items.Add(" ");
            cboxExitFunc2.Items.Add(" ");
            cboxExitFunc3.Items.Add(" ");
            cboxExitFunc4.Items.Add(" ");
            cboxExitFunc5.Items.Add(" ");
            cboxExitFunc6.Items.Add(" ");
            cboxExitFunc7.Items.Add(" ");

            //reload the comboboxes with updated strings
            LoadComboStrings();

            //populate boxes again with updated names
            PopulateSequencePages();

            //save in settings
            Vehicle.Default.seq_FunctionList = mf.seq.pos3 + "," + mf.seq.pos4 + "," + mf.seq.pos5 + "," + mf.seq.pos6 + "," + mf.seq.pos7 + "," + mf.seq.pos8;
            Vehicle.Default.Save();

            //reload buttons text
            btnToggle3.Text = mf.seq.pos3;
            btnToggle4.Text = mf.seq.pos4;
            btnToggle5.Text = mf.seq.pos5;
            btnToggle6.Text = mf.seq.pos6;
            btnToggle7.Text = mf.seq.pos7;
            btnToggle8.Text = mf.seq.pos8;

            //select entry tab page 1
            tabControl1.SelectTab(1);

            //flash that they have been saved
            mf.TimedMessageBox(1500, "Function Names", "Saved to Settings.....");
        }

        private void BtnEditCancel_Click(object sender, EventArgs e)
        {
            //select entry tab page 1
            tabControl1.SelectTab(1);
        }

        private void TabEdit_Enter(object sender, EventArgs e)
        {
            btnOK.Enabled = false;
            btnCancel.Enabled = false;

            //Fill in the strings for comboboxes - editable
            string line = Vehicle.Default.seq_FunctionList;
            string[] words = line.Split(',');

            mf.seq.pos3 = words[0];
            mf.seq.pos4 = words[1];
            mf.seq.pos5 = words[2];
            mf.seq.pos6 = words[3];
            mf.seq.pos7 = words[4];
            mf.seq.pos8 = words[5];

            //the edit page of text boxes
            LoadEditFunctionNames();
        }

        private void TabEdit_Leave(object sender, EventArgs e)
        {
            btnOK.Enabled = true;
            btnCancel.Enabled = true;
        }

        #endregion Edit names

        //private void btnResetAll_Click(object sender, EventArgs e)
        //{
        //    mf.seq.ResetAllSequences();
        //    PopulateSequencePages();
        //    mf.mc.machineControlData[mf.mc.cnYouTurnByte] = 0;
        //}

        private void BtnOK_Click(object sender, EventArgs e)
        {
            //save all the sequences and events
            SaveSequences();

            //Vehicle.Default.set_youSkipHeight = mf.yt.rowSkipsHeight;
            Vehicle.Default.set_youSkipWidth = mf.yt.rowSkipsWidth;
            Vehicle.Default.Youturn_Type = mf.yt.YouTurnType;

            Vehicle.Default.set_youTurnDistance = mf.yt.youTurnStartOffset;
            //mf.hl.boxLength = 3.0 * mf.yt.triggerDistanceOffset;

            StringBuilder sbEntry = new StringBuilder();
            StringBuilder sbExit = new StringBuilder();

            //Sequence functions 0,0,0,0,0
            for (int i = 0; i < FormGPS.MAXFUNCTIONS - 1; i++)
            {
                sbEntry.Append(mf.seq.seqEnter[i].function.ToString());
                sbEntry.Append(",");
                sbExit.Append(mf.seq.seqExit[i].function.ToString());
                sbExit.Append(",");
            }
            sbEntry.Append(mf.seq.seqEnter[FormGPS.MAXFUNCTIONS - 1].function.ToString());
            sbExit.Append(mf.seq.seqExit[FormGPS.MAXFUNCTIONS - 1].function.ToString());

            Vehicle.Default.seq_FunctionEnter = sbEntry.ToString();
            Vehicle.Default.seq_FunctionExit = sbExit.ToString();
            sbEntry.Clear(); sbExit.Clear();

            //Sequence actions
            for (int i = 0; i < FormGPS.MAXFUNCTIONS - 1; i++)
            {
                sbEntry.Append(mf.seq.seqEnter[i].action.ToString());
                sbEntry.Append(",");
                sbExit.Append(mf.seq.seqExit[i].action.ToString());
                sbExit.Append(",");
            }
            sbEntry.Append(mf.seq.seqEnter[FormGPS.MAXFUNCTIONS - 1].action.ToString());
            sbExit.Append(mf.seq.seqExit[FormGPS.MAXFUNCTIONS - 1].action.ToString());

            Vehicle.Default.seq_ActionEnter = sbEntry.ToString();
            Vehicle.Default.seq_ActionExit = sbExit.ToString();
            sbEntry.Clear(); sbExit.Clear();

            //Sequence Distances
            for (int i = 0; i < FormGPS.MAXFUNCTIONS - 1; i++)
            {
                sbEntry.Append(mf.seq.seqEnter[i].distance.ToString());
                sbEntry.Append(",");
                sbExit.Append(mf.seq.seqExit[i].distance.ToString());
                sbExit.Append(",");
            }
            sbEntry.Append(mf.seq.seqEnter[FormGPS.MAXFUNCTIONS - 1].distance.ToString());
            sbExit.Append(mf.seq.seqExit[FormGPS.MAXFUNCTIONS - 1].distance.ToString());

            Vehicle.Default.seq_DistanceEnter = sbEntry.ToString();
            Vehicle.Default.seq_DistanceExit = sbExit.ToString();

            if (Vehicle.Default.set_youTriggerDistance != mf.yt.triggerDistanceOffset)
            {
                Vehicle.Default.set_youTriggerDistance = mf.yt.triggerDistanceOffset;

                for (int i = 0; i < mf.bnd.bndArr.Count; i++)
                {
                    mf.StartTasks(mf.bnd.bndArr[i], i, TaskName.TurnLine);
                }
            }

            if (Vehicle.Default.set_geoFenceDistance != mf.yt.geoFenceDistance)
            {
                Vehicle.Default.set_geoFenceDistance = mf.yt.geoFenceDistance;

                for (int i = 0; i < mf.bnd.bndArr.Count; i++)
                {
                    mf.StartTasks(mf.bnd.bndArr[i], i, TaskName.GeoFence);
                }
            }
            mf.yt.ResetCreatedYouTurn();

            //save it all
            Vehicle.Default.Save();
            Close();
        }

        private void BtnTurnAllOff_Click(object sender, EventArgs e)
        {
            mf.mc.Send_Uturn[3] = 0;
            FunctionButtonsOnOff();
        }

        private void BtnToggle3_Click(object sender, EventArgs e)
        {
            mf.mc.Send_Uturn[3] ^= 0x01;

            FunctionButtonsOnOff();
        }

        private void BtnToggle4_Click(object sender, EventArgs e)
        {
            mf.mc.Send_Uturn[3] ^= 0x02;

            FunctionButtonsOnOff();
        }

        private void BtnToggle5_Click(object sender, EventArgs e)
        {
            mf.mc.Send_Uturn[3] ^= 0x04;

            FunctionButtonsOnOff();
        }

        private void BtnToggle6_Click(object sender, EventArgs e)
        {
            mf.mc.Send_Uturn[3] ^= 0x08;
            FunctionButtonsOnOff();
        }

        private void BtnToggle7_Click(object sender, EventArgs e)
        {
            mf.mc.Send_Uturn[3] ^= 0x10;
            FunctionButtonsOnOff();
        }

        private void BtnToggle8_Click(object sender, EventArgs e)
        {
            mf.mc.Send_Uturn[3] ^= 0x20;
            FunctionButtonsOnOff();
        }

        private void FunctionButtonsOnOff()
        {
            if ((mf.mc.Send_Uturn[3] & 0x01) == 0x01) btnToggle3.BackColor = Color.LightGreen;
            else btnToggle3.BackColor = Color.LightSalmon;

            if ((mf.mc.Send_Uturn[3] & 0x02) == 0x02) btnToggle4.BackColor = Color.LightGreen;
            else btnToggle4.BackColor = Color.LightSalmon;

            if ((mf.mc.Send_Uturn[3] & 0x04) == 0x04) btnToggle5.BackColor = Color.LightGreen;
            else btnToggle5.BackColor = Color.LightSalmon;

            if ((mf.mc.Send_Uturn[3] & 0x08) == 0x08) btnToggle6.BackColor = Color.LightGreen;
            else btnToggle6.BackColor = Color.LightSalmon;

            if ((mf.mc.Send_Uturn[3] & 0x10) == 0x10) btnToggle7.BackColor = Color.LightGreen;
            else btnToggle7.BackColor = Color.LightSalmon;

            if ((mf.mc.Send_Uturn[3] & 0x20) == 0x20) btnToggle8.BackColor = Color.LightGreen;
            else btnToggle8.BackColor = Color.LightSalmon;

            mf.DataSend[8] = "Uturn: " + Convert.ToString(mf.mc.Send_Uturn[3], 2).PadLeft(6, '0');
            mf.SendData(mf.mc.Send_Uturn, false);
        }
    }
}
