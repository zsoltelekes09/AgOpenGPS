using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormBoundary : Form
    {
        private readonly FormGPS mf;

        private bool Selectedreset = true;
        private int Position = 0;
        private int oldposition = 0;
        private bool scroll = false;
        private double viewableRatio = 0;
        private double contentHeight = 0;
        private int oldY = 0;
        private double thumbHeight = 0;
        private readonly int scrollmaxheight = 0;
        private readonly int startscrollY = 0;
        private readonly int startscrollX = 0;
        private readonly int items = 0;
        private readonly int rowheight = 0;

        private double easting, northing, latK, lonK;


        public FormBoundary(Form callingForm)
        {
            Owner = mf = callingForm as FormGPS;

            //winform initialization
            InitializeComponent();

            nudBndOffset.Controls[0].Enabled = false;

            scrollmaxheight = button4.Size.Height;
            startscrollY = button4.Location.Y;
            startscrollX = button4.Location.X;
            rowheight = (int)tableLayoutPanel1.RowStyles[0].Height;
            items = (int)(tableLayoutPanel1.Height / rowheight + 0.5);

            UpdateScroll(-1);

            this.Text = gStr.gsStartDeleteABoundary;

            //Column Header
            Boundary.Text = gStr.gsBounds;
            Thru.Text = gStr.gsDriveThru;
            Area.Text = gStr.gsArea;
            Around.Text = gStr.gsAround;

            //Bouton
            //btnDelete.Text = gStr.gsDelete;
            //btnSerialCancel.Text = gStr.gsSaveAndReturn;
            //btnDeleteAll.Text = gStr.gsDeleteAll;
            btnGo.Text = gStr.gsGo;
            lblOffset.Text = gStr.gsOffset;

            btnLoadBoundaryFromGE.Visible = false;
            btnLoadMultiBoundaryFromGE.Visible = false;
        }

        void UpdateScroll(double pos)
        {
            contentHeight = (mf.bnd.bndArr.Count) * rowheight;
            viewableRatio = tableLayoutPanel1.Size.Height / contentHeight;
            if (viewableRatio >= 1)
            {
                button4.Size = new Size(rowheight, scrollmaxheight);
                button4.Location = new Point(startscrollX, startscrollY);
            }
            else
            {
                thumbHeight = (scrollmaxheight * viewableRatio < rowheight * 2) ? rowheight * 2 : (scrollmaxheight * viewableRatio);
                button4.Size = new Size(rowheight, (int)(thumbHeight + 0.5));
                if (pos < 0)
                {
                    button4.Location = new Point(startscrollX, (int)(startscrollY + Position * ((scrollmaxheight - thumbHeight) / (mf.bnd.bndArr.Count - items)) + 0.5));
                }
                else
                {
                    button4.Location = new Point(startscrollX, (int)(startscrollY + pos));
                }
            }
        }

        private void UpdateChart()
        {
            int field = 1;
            int inner = 1;

            for (int i = 0; i <= mf.bnd.bndArr.Count && i <= Position + items; i++)
            {
                if (i < Position && i < mf.bnd.bndArr.Count) inner += 1;
                else
                {
                    Control aa = tableLayoutPanel1.GetControlFromPosition(0, i - Position);
                    if (aa == null)
                    {
                        var a = new Button
                        {
                            Margin = new Padding(0),
                            Size = new Size(280, 50),
                            Name = string.Format("{0}", i - Position),
                            TextAlign = ContentAlignment.MiddleCenter
                        };
                        a.Click += B_Click;
                        a.FlatStyle = FlatStyle.Flat;
                        a.FlatAppearance.BorderColor = BackColor;
                        a.FlatAppearance.MouseOverBackColor = BackColor;
                        a.FlatAppearance.MouseDownBackColor = BackColor;

                        aa = a;

                        var c = new Button
                        {
                            Margin = new Padding(0),
                            Size = new System.Drawing.Size(80, 50),
                            Name = string.Format("{0}", i - Position),
                            TextAlign = ContentAlignment.MiddleCenter
                        };
                        c.Click += DriveThru_Click;
                        var d = new Button
                        {
                            Margin = new Padding(0),
                            Size = new System.Drawing.Size(80, 50),
                            Name = string.Format("{0}", i - Position),
                            TextAlign = ContentAlignment.MiddleCenter
                        };
                        d.Click += DriveAround_Click;

                        tableLayoutPanel1.Controls.Add(a, 0, i - Position);
                        tableLayoutPanel1.Controls.Add(c, 2, i - Position);
                        tableLayoutPanel1.Controls.Add(d, 3, i - Position);
                    }

                    if (i < mf.bnd.bndArr.Count && i < Position + items)
                    {
                        tableLayoutPanel1.SetColumnSpan(aa, 1);

                        Control bb = tableLayoutPanel1.GetControlFromPosition(1, i - Position);
                        if (bb == null)
                        {
                            var b = new Button
                            {
                                Margin = new Padding(0),
                                Size = new System.Drawing.Size(150, 50),
                                TextAlign = ContentAlignment.MiddleCenter,
                                Name = string.Format("{0}", i - Position),
                            };
                            b.Click += B_Click;
                            b.FlatStyle = FlatStyle.Flat;
                            b.FlatAppearance.BorderColor = BackColor;
                            b.FlatAppearance.MouseOverBackColor = BackColor;
                            b.FlatAppearance.MouseDownBackColor = BackColor;
                            tableLayoutPanel1.Controls.Add(b, 1, i - Position);
                            bb = b;
                        }
                        Control cc = tableLayoutPanel1.GetControlFromPosition(2, i - Position);
                        cc.Visible = true;
                        Control dd = tableLayoutPanel1.GetControlFromPosition(3, i - Position);
                        dd.Visible = true;

                        Font backupfont = new Font(aa.Font.FontFamily, 18F, FontStyle.Bold);

                        if (i == 0)
                        {
                            aa.Text = string.Format(gStr.gsOuter + " {0}", field);
                            cc.Enabled = false;
                            dd.Enabled = false;

                            mf.bnd.bndArr[i].isDriveThru = false;
                            mf.bnd.bndArr[i].isDriveAround = false;
                            cc.Text = gStr.gsNo;
                            dd.Text = gStr.gsNo;

                        }
                        else
                        {
                            aa.Text = string.Format(gStr.gsInner + " {0}", inner);
                            inner += 1;
                            cc.Enabled = true;
                            dd.Enabled = true;
                            cc.Text = mf.bnd.bndArr[i].isDriveThru ? gStr.gsYes : gStr.gsNo;
                            dd.Text = mf.bnd.bndArr[i].isDriveAround ? gStr.gsYes : gStr.gsNo;
                        }

                        aa.Font = backupfont;
                        cc.BackColor = Color.WhiteSmoke;
                        dd.BackColor = Color.WhiteSmoke;
                        cc.Anchor = AnchorStyles.None;
                        dd.Anchor = AnchorStyles.None;

                        if (mf.isMetric)
                        {
                            bb.Text = Math.Round(mf.bnd.bndArr[i].area * 0.0001, 2) + " Ha";
                        }
                        else
                        {
                            bb.Text = Math.Round(mf.bnd.bndArr[i].area * 0.000247105, 2) + " Ac";
                        }

                        if (Selectedreset == false && i == mf.bnd.boundarySelected)
                        {
                            aa.ForeColor = Color.Red;
                            bb.ForeColor = Color.Red;
                        }
                        else
                        {
                            aa.ForeColor = default;
                            bb.ForeColor = default;
                        }
                    }
                    else
                    {
                        Control bb = tableLayoutPanel1.GetControlFromPosition(0, i - Position);
                        //delete rest of buttons
                        while (true)
                        {
                            Control ff = tableLayoutPanel1.GetNextControl(bb, true);
                            if (ff == null) break;
                            else ff.Dispose();
                        }
                        bb.Dispose();
                        break;
                    }
                }
            }
        }

        private void FormBoundary_Load(object sender, EventArgs e)
        {
            btnLeftRight.Image = mf.bnd.isDrawRightSide ? Properties.Resources.BoundaryRight : Properties.Resources.BoundaryLeft;
            btnDelete.Enabled = false;

            //update the list view with real data
            UpdateScroll(-1);
            UpdateChart();
            nudBndOffset.Value = (decimal)(mf.Guidance.GuidanceWidth * 0.5);
        }

        void DriveThru_Click(object sender, EventArgs e)
        {
            if (sender is Button b)
            {
                int pos = Convert.ToInt32(b.Name) + Position;
                mf.bnd.bndArr[pos].isDriveThru = !mf.bnd.bndArr[pos].isDriveThru;
                UpdateChart();
                mf.FileSaveBoundary();
            }
        }

        void DriveAround_Click(object sender, EventArgs e)
        {
            if (sender is Button b)
            {
                int pos = Convert.ToInt32(b.Name) + Position;
                mf.bnd.bndArr[pos].isDriveAround = !mf.bnd.bndArr[pos].isDriveAround;
                UpdateChart();
                mf.FileSaveBoundary();
            }
        }

        void B_Click(object sender, EventArgs e)
        {
            if (sender is Button b)
            {

                mf.bnd.boundarySelected = Convert.ToInt32(b.Name) + Position;

                Selectedreset = false;

                if (mf.bnd.bndArr.Count > mf.bnd.boundarySelected) btnDelete.Enabled = true;
                else btnDelete.Enabled = false;
            }
            UpdateChart();
        }

        private void BtnSerialCancel_Click(object sender, EventArgs e)
        {
            mf.bnd.boundarySelected = -1;
            mf.bnd.isOkToAddPoints = false;
            Close();
        }

        private void BtnLeftRight_Click(object sender, EventArgs e)
        {
            mf.bnd.isDrawRightSide = !mf.bnd.isDrawRightSide;
            btnLeftRight.Image = mf.bnd.isDrawRightSide ? Properties.Resources.BoundaryRight : Properties.Resources.BoundaryLeft;
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result3 = MessageBox.Show(gStr.gsCompletelyDeleteBoundary,
                gStr.gsDeleteForSure,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result3 == DialogResult.Yes)
            {

                btnLeftRight.Enabled = false;
                btnGo.Enabled = false;
                btnDelete.Enabled = false;
                nudBndOffset.Enabled = false;

            if (mf.bnd.bndArr.Count > mf.bnd.boundarySelected)
            {
                mf.bnd.bndArr.RemoveAt(mf.bnd.boundarySelected);
                mf.turn.turnArr.RemoveAt(mf.bnd.boundarySelected);
                mf.gf.geoFenceArr.RemoveAt(mf.bnd.boundarySelected);
                mf.hd.headArr.RemoveAt(mf.bnd.boundarySelected);
            }

            mf.FileSaveBoundary();
            mf.FileSaveHeadland();

            if (mf.bnd.bndArr.Count == 0) mf.hd.isOn = false;

            mf.bnd.boundarySelected = -1;

            if (Position + items >= mf.bnd.bndArr.Count) Position--;
            if (Position < 0) Position = 0;

            Selectedreset = true;
            mf.fd.UpdateFieldBoundaryGUIAreas();
            mf.mazeGrid.BuildMazeGridArray();

                UpdateChart();
                UpdateScroll(-1);
            }
            else
            {
                mf.TimedMessageBox(1500, gStr.gsNothingDeleted, gStr.gsActionHasBeenCancelled);
            }
        }

        private void ResetAllBoundary()
        {
            Position = 0;

            mf.bnd.bndArr.Clear();
            mf.turn.turnArr.Clear();
            mf.gf.geoFenceArr.Clear();
            mf.hd.headArr.Clear();

            mf.FileSaveBoundary();
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowStyles.Clear();

            UpdateChart();
            UpdateScroll(-1);

            btnDelete.Enabled = false;
        }

        private void BtnOpenGoogleEarth_Click(object sender, EventArgs e)
        {
            //save new copy of kml with selected flag and view in GoogleEarth
          
            mf.FileMakeKMLFromCurrentPosition(mf.pn.latitude, mf.pn.longitude);
            System.Diagnostics.Process.Start(mf.fieldsDirectory + mf.currentFieldDirectory + "\\CurrentPosition.KML");
            Close();

        }

        private void NudBndOffset_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender, this);
            //btnCancel.Focus();
        }

        private void BtnGo_Click(object sender, EventArgs e)
        {
            mf.bnd.createBndOffset = (double)nudBndOffset.Value;
            mf.bnd.isBndBeingMade = true;

            Form form2 = new FormBoundaryPlayer(mf, this);
            form2.Show(this);


            Hide();
        }

        private void BtnDeleteAll_Click(object sender, EventArgs e)
        {
            DialogResult result3 = MessageBox.Show(gStr.gsCompletelyDeleteBoundary,
                gStr.gsDeleteForSure,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result3 == DialogResult.Yes)
            {

                ResetAllBoundary();

                mf.bnd.boundarySelected = -1;
                Selectedreset = true;

                mf.bnd.isOkToAddPoints = false;
                mf.FileSaveHeadland();

                mf.hd.isOn = false;
                mf.fd.UpdateFieldBoundaryGUIAreas();
                mf.mazeGrid.BuildMazeGridArray();
            }
        }

        private void Up_Scroll_Click(object sender, EventArgs e)
        {
            if (Position > 0) Position--;
            UpdateChart();
            UpdateScroll(-1);
        }

        private void Down_Scroll_Click(object sender, EventArgs e)
        {
            if (Position + items < mf.bnd.bndArr.Count) Position++;
            UpdateChart();
            UpdateScroll(-1);
        }

        void MouseWheel_Scroll(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                if (Position > 0) Position -= (e.Delta / 120);
            }
            else
            {
                if (Position + items < mf.bnd.bndArr.Count) Position -= (e.Delta / 120);
            }
            UpdateChart();
            UpdateScroll(-1);
        }

        void Mouse_Down(object sender, MouseEventArgs e)
        {
            oldY = MousePosition.Y;
            scroll = true;
        }

        void Mouse_Up(object sender, MouseEventArgs e)
        {
            scroll = false;
        }

        void Mouse_Leave(object sender, EventArgs e)
        {
            scroll = false;
        }
        
        void Mouse_Move(object sender, MouseEventArgs e)
        {
            if (scroll == true && viewableRatio < 1)
            {
                if (!(oldY == MousePosition.Y))
                {
                    if (button4.Location.Y + (MousePosition.Y - oldY) > startscrollY)
                    {
                        if ((button4.Location.Y + (MousePosition.Y - oldY)) < (startscrollY + scrollmaxheight - thumbHeight))
                        {
                            Position = (int)(((button4.Location.Y + MousePosition.Y - oldY) - startscrollY) / ((scrollmaxheight - thumbHeight) / (mf.bnd.bndArr.Count - items)) + 0.5);
                            UpdateScroll((button4.Location.Y + (MousePosition.Y - oldY) - startscrollY));
                        }
                        else
                        {
                            Position = mf.bnd.bndArr.Count - items;
                            UpdateScroll(scrollmaxheight - thumbHeight);
                        }
                    }
                    else
                    {
                        Position = 0;
                        UpdateScroll(0);
                    }
                    if (Position != oldposition)
                    {
                        oldposition = Position;
                        UpdateChart();
                    }
                    oldY = MousePosition.Y;
                }
            }
        }

        private void BtnLoadBoundaryFromGE_Click(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                Selectedreset = true;
                btnDelete.Enabled = false;

                string fileAndDirectory;
                {
                    //create the dialog instance
                    using (OpenFileDialog ofd = new OpenFileDialog
                    {
                        //set the filter to text KML only
                        Filter = "KML files (*.KML)|*.KML",

                        //the initial directory, fields, for the open dialog
                        InitialDirectory = mf.fieldsDirectory + mf.currentFieldDirectory
                    })
                    {
                        //was a file selected
                        if (ofd.ShowDialog(this) == DialogResult.Cancel) return;
                        else fileAndDirectory = ofd.FileName;
                    }
                }

                //start to read the file
                string line;
                string coordinates = null;
                int startIndex;
                int i = 0;

                using (System.IO.StreamReader reader = new System.IO.StreamReader(fileAndDirectory))
                {

                    if (button.Name == "btnLoadMultiBoundaryFromGE") ResetAllBoundary();
                    else i = mf.bnd.bndArr.Count;

                    try
                    {
                        while (!reader.EndOfStream)
                        {
                            line = reader.ReadLine();

                            startIndex = line.IndexOf("<coordinates>");

                            if (startIndex != -1)
                            {
                                while (true)
                                {
                                    int endIndex = line.IndexOf("</coordinates>");

                                    if (endIndex == -1)
                                    {
                                        //just add the line
                                        if (startIndex == -1) coordinates += line.Substring(0);
                                        else coordinates += line.Substring(startIndex + 13);
                                    }
                                    else
                                    {
                                        if (startIndex == -1) coordinates += line.Substring(0, endIndex);
                                        else coordinates += line.Substring(startIndex + 13, endIndex - (startIndex + 13));
                                        break;
                                    }
                                    line = reader.ReadLine();
                                    line = line.Trim();
                                    startIndex = -1;
                                }

                                line = coordinates;
                                char[] delimiterChars = { ' ', '\t', '\r', '\n' };
                                string[] numberSets = line.Split(delimiterChars);

                                //at least 3 points
                                if (numberSets.Length > 2)
                                {
                                    mf.bnd.bndArr.Add(new CBoundaryLines());
                                    mf.turn.turnArr.Add(new CTurnLines());
                                    mf.gf.geoFenceArr.Add(new CGeoFenceLines());
                                    mf.hd.headArr.Add(new CHeadLines());

                                    foreach (var item in numberSets)
                                    {
                                        string[] fix = item.Split(',');
                                        double.TryParse(fix[0], NumberStyles.Float, CultureInfo.InvariantCulture, out lonK);
                                        double.TryParse(fix[1], NumberStyles.Float, CultureInfo.InvariantCulture, out latK);
                                        double[] xy = mf.pn.DecDeg2UTM(latK, lonK);

                                        //match new fix to current position
                                        easting = xy[0] - mf.pn.utmEast;
                                        northing = xy[1] - mf.pn.utmNorth;

                                        double east = easting;
                                        double nort = northing;

                                        //fix the azimuth error
                                        easting = (Math.Cos(-mf.pn.convergenceAngle) * east) - (Math.Sin(-mf.pn.convergenceAngle) * nort);
                                        northing = (Math.Sin(-mf.pn.convergenceAngle) * east) + (Math.Cos(-mf.pn.convergenceAngle) * nort);

                                        //add the point to boundary
                                        Vec3 bndPt = new Vec3(easting, northing, 0);
                                        mf.bnd.bndArr[i].bndLine.Add(bndPt);
                                    }

                                    if (mf.bnd.bndArr[i].bndLine.Count > 0)
                                    {
                                        //fix the points if there are gaps bigger then
                                        mf.bnd.bndArr[i].FixBoundaryLine(i, mf.Guidance.GuidanceWidth);
                                        mf.bnd.bndArr[i].PreCalcBoundaryLines();
                                        mf.bnd.bndArr[i].CalculateBoundaryArea();
                                        mf.bnd.bndArr[i].CalculateBoundaryWinding();


                                        mf.turn.BuildTurnLines(i);
                                        mf.gf.BuildGeoFenceLines(i);

                                        coordinates = "";
                                        i++;
                                    }
                                    else
                                    {
                                        mf.bnd.bndArr.RemoveAt(mf.bnd.bndArr.Count - 1);
                                        mf.turn.turnArr.RemoveAt(mf.bnd.bndArr.Count - 1);
                                        mf.gf.geoFenceArr.RemoveAt(mf.bnd.bndArr.Count - 1);
                                        mf.hd.headArr.RemoveAt(mf.bnd.bndArr.Count - 1);
                                    }
                                }
                                else
                                {
                                    mf.TimedMessageBox(2000, gStr.gsErrorreadingKML, gStr.gsChooseBuildDifferentone);
                                }
                                if (button.Name == "btnLoadBoundaryFromGE")
                                {
                                    break;
                                }
                            }
                        }
                        mf.fd.UpdateFieldBoundaryGUIAreas();
                        mf.mazeGrid.BuildMazeGridArray();

                        mf.FileSaveBoundary();
                        UpdateChart();
                        UpdateScroll(-1);
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
            }
        }

        private void BtnDriveOrExt_Click(object sender, EventArgs e)
        {
            if (btnLoadBoundaryFromGE.Visible == true)
            {
                btnLoadBoundaryFromGE.Visible = false;
                btnLoadMultiBoundaryFromGE.Visible = false;
                btnGo.Visible = true;
                btnLeftRight.Visible = true;
                nudBndOffset.Visible = true;
                label1.Visible = true;
                lblOffset.Visible = true;

            }
            else
            {
                btnLoadBoundaryFromGE.Visible = true;
                btnLoadMultiBoundaryFromGE.Visible = true;
                btnGo.Visible = false;
                btnLeftRight.Visible = false;
                nudBndOffset.Visible = false;
                label1.Visible = false;
                lblOffset.Visible = false;
            }
        }
    }
}
