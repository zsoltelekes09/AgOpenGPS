using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;


namespace AgOpenGPS
{
    public class CTool
    {
        //create a new section and set left and right positions
        //created whether used or not, saves restarting program

        private readonly FormGPS mf;

        public List<CSection> Sections = new List<CSection>();

        public Vec2 HitchPos;
        public Vec3 ToolWheelPos;
        public Vec2 ToolHitchPos;
        public Vec3 TankWheelPos;
        public Vec2 TankHitchPos;

        public double HitchLength;
        public int ToolNum;

        public double ToolFarLeftSpeed = 0;
        public double ToolFarRightSpeed = 0;

        public double ToolWheelLength, TankWheelLength, ToolHitchLength, TankHitchLength;
        public double ToolOffset, SlowSpeedCutoff;

        public double LookAheadOffSetting, LookAheadOnSetting;
        public double TurnOffDelay, MappingOnDelay, MappingOffDelay;

        public double lookAheadDistanceOnPixelsLeft, lookAheadDistanceOnPixelsRight;
        public double lookAheadDistanceOffPixelsLeft, lookAheadDistanceOffPixelsRight;

        public bool isToolTrailing, isToolTBT;
        public bool isToolBehindPivot;
        public string toolsAttachType;

        //storage for the cos and sin of heading
        public double cosSectionHeading = 1.0, sinSectionHeading = 0.0;

        //used for super section off on
        public int toolMinUnappliedPixels;

        //how many individual sections
        public int numOfSections;
        public bool SuperSection;


        //read pixel values
        public int rpXPosition;
        public int rpWidth;

        //Constructor called by FormGPS
        public CTool(FormGPS _f, int num)
        {
            mf = _f;
            ToolNum = num;

            SetToolSettings(num);
        }

        public void SetToolSettings(int num)
        {
            ToolNum = num;
            numOfSections = Properties.Vehicle.Default.ToolSettings[ToolNum].Sections.Count;

            SetSections();

            ToolWheelPos = new Vec3(0, 0, 0);
            ToolHitchPos = new Vec2(0, 0);
            TankWheelPos = new Vec3(0, 0, 0);
            TankHitchPos = new Vec2(0, 0);
            HitchPos = new Vec2(0, 0);

            ToolWheelLength = Properties.Vehicle.Default.ToolSettings[ToolNum].ToolWheelLength;
            TankWheelLength = Properties.Vehicle.Default.ToolSettings[ToolNum].TankWheelLength;
            ToolHitchLength = Properties.Vehicle.Default.ToolSettings[ToolNum].ToolHitchLength;
            TankHitchLength = Properties.Vehicle.Default.ToolSettings[ToolNum].TankHitchLength;
            HitchLength = Properties.Vehicle.Default.ToolSettings[ToolNum].HitchLength;

            isToolBehindPivot = Properties.Vehicle.Default.ToolSettings[ToolNum].BehindPivot;
            isToolTrailing = Properties.Vehicle.Default.ToolSettings[ToolNum].Trailing;
            isToolTBT = Properties.Vehicle.Default.ToolSettings[ToolNum].TBT;

            LookAheadOnSetting = Properties.Vehicle.Default.ToolSettings[ToolNum].LookAheadOn;
            LookAheadOffSetting = Properties.Vehicle.Default.ToolSettings[ToolNum].LookAheadOff;

            TurnOffDelay = Properties.Vehicle.Default.ToolSettings[ToolNum].TurnOffDelay;

            MappingOnDelay = Properties.Vehicle.Default.ToolSettings[ToolNum].MappingOnDelay;
            MappingOffDelay = Properties.Vehicle.Default.ToolSettings[ToolNum].MappingOffDelay;

            toolMinUnappliedPixels = Properties.Vehicle.Default.ToolSettings[ToolNum].MinApplied;
            SlowSpeedCutoff = Properties.Vehicle.Default.ToolSettings[ToolNum].SlowSpeedCutoff;
        }

        public void RemoveSections()
        {
            for (int j = Sections.Count - 1; j >= 0; j--)
            {
                if (Sections[j].IsMappingOn)
                {
                    Sections[j].TurnMappingOff();
                }

                mf.Controls.Remove(Sections[j].SectionButton);
                Sections[j].SectionButton.Dispose();
                Sections.RemoveAt(j);
            }
        }

        public void SetSections()
        {
            for (int j = Sections.Count-1; j >= 0; j--)
            {
                if (Sections[j].IsMappingOn)
                {
                    Sections[j].TurnMappingOff();
                }

                if (j > numOfSections)
                {
                    mf.Controls.Remove(Sections[j].SectionButton);
                    Sections[j].SectionButton.Dispose();
                    Sections.RemoveAt(j);
                    mf.TotalSections--;
                }
                else if (j == numOfSections)
                {
                    mf.Controls.Remove(Sections[j].SectionButton);
                }
            }

            if (numOfSections > 0)
            {
                double MostLeft = 35, MostRight = -35;
                for (int j = 0; j <= numOfSections; j++)
                {

                    if (Sections.Count <= j) Sections.Add(new CSection(mf));
                    if (j < numOfSections)
                    {

                        Sections[j].positionLeft = Properties.Vehicle.Default.ToolSettings[ToolNum].Sections[j][0];
                        MostLeft = Math.Min(MostLeft, Sections[j].positionLeft);
                        Sections[j].positionRight = Properties.Vehicle.Default.ToolSettings[ToolNum].Sections[j][1];
                        MostRight = Math.Max(MostRight, Sections[j].positionRight);


                        Sections[j].positionForward = Properties.Vehicle.Default.ToolSettings[ToolNum].Sections[j][2];
                        Sections[j].rpSectionPosition = 375 + (int)Math.Round(Sections[j].positionLeft * 10, 0, MidpointRounding.AwayFromZero);
                        Sections[j].rpSectionWidth = (int)Math.Round((Sections[j].positionRight - Sections[j].positionLeft) * 10, 0, MidpointRounding.AwayFromZero);


                        if (!mf.Controls.Contains(Sections[j].SectionButton))
                        {
                            mf.TotalSections++;
                            Sections[j].BtnSectionState = mf.autoBtnState;

                            mf.Controls.Add(Sections[j].SectionButton);

                            Sections[j].SectionButton.BringToFront();
                            Sections[j].SectionButton.Text = (j + 1).ToString();
                            Sections[j].SectionButton.Name = (ToolNum + "," + j).ToString();
                            Sections[j].SectionButton.Click += new EventHandler(mf.btnSectionMan_Click);

                            Sections[j].SectionButton.Enabled = (mf.autoBtnState != 0);
                            Sections[j].SectionButton.FlatAppearance.BorderColor = SystemColors.ActiveCaptionText;
                            Sections[j].SectionButton.FlatStyle = FlatStyle.Flat;
                            Sections[j].SectionButton.TextAlign = ContentAlignment.MiddleCenter;
                            Sections[j].SectionButton.UseVisualStyleBackColor = false;

                            SectionButtonColor(j);
                        }
                        else
                        {
                            Sections[j].BtnSectionState = mf.autoBtnState;
                            Sections[j].SectionButton.Enabled = (mf.autoBtnState != 0);
                            SectionButtonColor(j);
                        }
                    }
                }

                //now do the full width section
                Sections[numOfSections].positionLeft = MostLeft;
                Sections[numOfSections].positionRight = MostRight;

                rpXPosition = 375 + (int)(Math.Round(MostLeft * 10, 0, MidpointRounding.AwayFromZero));
                rpWidth = (int)(Math.Round((MostRight - MostLeft) * 10, 0, MidpointRounding.AwayFromZero));
            }
        }

        public void SectionButtonColor(int j)
        {
            if (!mf.isJobStarted)
            {
                Sections[j].SectionButton.BackColor = mf.isDay ? Color.Silver : Color.Silver;
            }
            else if (Sections[j].BtnSectionState == FormGPS.btnStates.On)
                Sections[j].SectionButton.BackColor = mf.isDay ? Color.Yellow : Color.DarkGoldenrod;
            else if (Sections[j].BtnSectionState == FormGPS.btnStates.Auto)
                Sections[j].SectionButton.BackColor = mf.isDay ? Color.Lime : Color.ForestGreen;
            else
                Sections[j].SectionButton.BackColor = mf.isDay ? Color.Red : Color.Crimson;

            Sections[j].SectionButton.ForeColor = mf.isDay ? Color.Black : Color.White;
        }

        public void DrawTool()
        {
            //translate down to the hitch pin
            GL.Translate(0.0, HitchLength, 0.0);

            if (numOfSections > 0)
            {
                //there is a trailing tow between hitch
                if (isToolTrailing)
                {
                    if (isToolTBT)
                    {
                        //rotate to tank heading
                        GL.Rotate(Glm.ToDegrees(mf.fixHeading - TankWheelPos.Heading), 0.0, 0.0, 1.0);

                        //draw the tank
                        GL.LineWidth(2f);
                        GL.Color3(0.7f, 0.7f, 0.97f);
                        GL.Begin(PrimitiveType.Lines);
                        GL.Vertex3(0, 0, 0);
                        GL.Vertex3(0, TankWheelLength, 0);
                        GL.End();

                        //Hitch
                        GL.LineWidth(1f);
                        GL.Color3(0.37f, 0.37f, 0.97f);
                        GL.Begin(PrimitiveType.Lines);
                        GL.Vertex3(0, TankWheelLength, 0);
                        GL.Vertex3(0, TankWheelLength + TankHitchLength, 0);
                        GL.End();

                        //pivot markers
                        GL.Color3(0.95f, 0.95f, 0f);
                        GL.PointSize(6f);
                        GL.Begin(PrimitiveType.Points);
                        GL.Vertex3(0.0, TankWheelLength, 0.0);
                        if (TankHitchLength != 0)
                        {
                            GL.Color3(0.95f, 0.0f, 0.0f);
                            GL.Vertex3(0.0, TankWheelLength + TankHitchLength, 0.0);
                        }
                        GL.End();

                        //move down the tank hitch, unwind, rotate to section heading
                        GL.Translate(0.0, TankWheelLength + TankHitchLength, 0.0);
                    }

                    GL.Rotate(Glm.ToDegrees(TankWheelPos.Heading - ToolWheelPos.Heading), 0.0, 0.0, 1.0);

                    //draw the hitch
                    GL.LineWidth(2);
                    GL.Color3(0.7f, 0.7f, 0.97f);
                    GL.Begin(PrimitiveType.Lines);
                    GL.Vertex3(0.0, ToolWheelLength, 0.0);
                    GL.Vertex3(0, 0, 0);
                    GL.End();

                    //Hitch
                    GL.LineWidth(1f);
                    GL.Color3(0.37f, 0.37f, 0.97f);
                    GL.Begin(PrimitiveType.Lines);
                    GL.Vertex3(0, ToolWheelLength, 0);
                    GL.Vertex3(0, ToolWheelLength + ToolHitchLength, 0);
                    GL.End();



                    if (ToolHitchLength != 0)
                    {
                        //pivot marker
                        GL.Color3(0.95f, 0.95f, 0f);
                        GL.PointSize(6f);
                        GL.Begin(PrimitiveType.Points);
                        GL.Vertex3(0.0, ToolWheelLength, 0.0);
                        GL.End();
                    }

                    GL.Translate(0.0, ToolWheelLength + ToolHitchLength, 0.0);
                }
                //look ahead lines
                GL.LineWidth(1);
                GL.Begin(PrimitiveType.Lines);

                //lookahead section on
                GL.Color3(0.20f, 0.7f, 0.2f);
                GL.Vertex3(Sections[0].positionLeft, Sections[0].positionForward + (lookAheadDistanceOnPixelsLeft) * 0.1, 0);
                for (int j = 0; j < numOfSections; j++)
                {
                    if (j > 0 && (Sections[j - 1].positionRight != Sections[j].positionLeft || Sections[j - 1].positionForward != Sections[j].positionForward))
                    {
                        GL.Vertex3(Sections[j - 1].positionRight, Sections[j - 1].positionForward + (lookAheadDistanceOnPixelsLeft + (((lookAheadDistanceOnPixelsRight - lookAheadDistanceOnPixelsLeft) / (rpWidth * 0.1)) * (Sections[j - 1].positionRight - Sections[0].positionLeft))) * 0.1, 0);
                        GL.Vertex3(Sections[j].positionLeft, Sections[j].positionForward + (lookAheadDistanceOnPixelsLeft + (((lookAheadDistanceOnPixelsRight - lookAheadDistanceOnPixelsLeft) / (rpWidth * 0.1)) * (Sections[j].positionLeft - Sections[0].positionLeft))) * 0.1, 0);
                    }
                }
                GL.Vertex3(Sections[numOfSections - 1].positionRight, Sections[numOfSections - 1].positionForward + (lookAheadDistanceOnPixelsRight) * 0.1, 0);


                //lookahead section off
                GL.Color3(0.70f, 0.2f, 0.2f);
                GL.Vertex3(Sections[0].positionLeft, Sections[0].positionForward + (lookAheadDistanceOffPixelsLeft) * 0.1, 0);
                for (int j = 0; j < numOfSections; j++)
                {
                    if (j > 0 && (Sections[j - 1].positionRight != Sections[j].positionLeft || Sections[j - 1].positionForward != Sections[j].positionForward))
                    {
                        GL.Vertex3(Sections[j - 1].positionRight, Sections[j -1].positionForward + (lookAheadDistanceOffPixelsLeft + (((lookAheadDistanceOffPixelsRight - lookAheadDistanceOffPixelsLeft) / (rpWidth * 0.1)) * (Sections[j - 1].positionRight - Sections[0].positionLeft))) * 0.1, 0);
                        GL.Vertex3(Sections[j].positionLeft, Sections[j].positionForward + (lookAheadDistanceOffPixelsLeft + (((lookAheadDistanceOffPixelsRight - lookAheadDistanceOffPixelsLeft) / (rpWidth * 0.1)) * (Sections[j].positionLeft - Sections[0].positionLeft))) * 0.1, 0);
                    }
                }
                GL.Vertex3(Sections[numOfSections - 1].positionRight, Sections[numOfSections - 1].positionForward + (lookAheadDistanceOffPixelsRight) * 0.1, 0);

                if (mf.vehicle.BtnHydLiftOn)
                {
                    GL.Color3(0.70f, 0.2f, 0.72f);
                    GL.Vertex3(Sections[0].positionLeft, (mf.vehicle.hydLiftLookAheadDistanceLeft * 0.1), 0);
                    GL.Vertex3(Sections[numOfSections - 1].positionRight, (mf.vehicle.hydLiftLookAheadDistanceRight * 0.1), 0);
                }

                GL.End();
            }

            //draw the sections
            GL.LineWidth(2);

            double hite = mf.camera.camSetDistance / -100;
            if (hite > 1.3) hite = 1.3;
            if (hite < 0.5) hite = 0.5;

            for (int j = 0; j < numOfSections; j++)
            {
                //if section is on, green, if off, red color
                if (Sections[j].IsSectionOn || Sections[numOfSections].IsSectionOn)
                {
                    if (Sections[j].BtnSectionState == FormGPS.btnStates.Auto)
                    {
                        GL.Color3(0.0f, 0.9f, 0.0f);
                        //if (mf.section[j].isMappingOn) GL.Color3(0.0f, 0.7f, 0.0f);
                        //else GL.Color3(0.70f, 0.0f, 0.90f);
                    }
                    else GL.Color3(0.97, 0.97, 0);
                }
                else
                {
                    //if (!mf.section[j].isMappingOn) GL.Color3(0.70f, 0.2f, 0.2f);
                    //else GL.Color3(0.00f, 0.250f, 0.90f);
                    GL.Color3(0.7f, 0.2f, 0.2f);
                }

                double mid = (Sections[j].positionLeft + Sections[j].positionRight) /2;

                GL.Begin(PrimitiveType.TriangleFan);
                {
                    GL.Vertex3(Sections[j].positionLeft, Sections[j].positionForward, 0);
                    GL.Vertex3(Sections[j].positionLeft, Sections[j].positionForward - hite, 0);

                    GL.Vertex3(mid, Sections[j].positionForward - hite * 1.5, 0);

                    GL.Vertex3(Sections[j].positionRight, Sections[j].positionForward - hite, 0);
                    GL.Vertex3(Sections[j].positionRight, Sections[j].positionForward, 0);
                }
                GL.End();

                GL.Begin(PrimitiveType.LineLoop);
                {
                    GL.Color3(0.0, 0.0, 0.0);
                    GL.Vertex3(Sections[j].positionLeft, Sections[j].positionForward, 0);
                    GL.Vertex3(Sections[j].positionLeft, Sections[j].positionForward - hite, 0);

                    GL.Vertex3(mid, Sections[j].positionForward - hite * 1.5, 0);

                    GL.Vertex3(Sections[j].positionRight, Sections[j].positionForward - hite, 0);
                    GL.Vertex3(Sections[j].positionRight, Sections[j].positionForward, 0);
                }
                GL.End();
            }

            //GL.End();
            
            //draw section markers if close enough
            if (mf.camera.camSetDistance > 250)
            {
                GL.Color3(0.0f, 0.0f, 0.0f);
                //section markers
                GL.PointSize(3.0f);
                GL.Begin(PrimitiveType.Points);
                for (int j = 0; j < numOfSections - 1; j++)
                    GL.Vertex3(Sections[j].positionRight, 0, 0);
                GL.End();
            }

            if (isToolTrailing)
            {
                GL.Translate(0.0, -(ToolWheelLength + ToolHitchLength), 0.0);
                GL.Rotate(Glm.ToDegrees(TankWheelPos.Heading - ToolWheelPos.Heading), 0.0, 0.0, -1.0);

                if (isToolTBT)
                {
                    GL.Translate(0.0, -(TankWheelLength + TankHitchLength), 0.0);
                    GL.Rotate(Glm.ToDegrees(mf.fixHeading - TankWheelPos.Heading), 0.0, 0.0, -1.0);
                }
            }
            GL.Translate(0.0, -HitchLength, 0.0);
        }
    }
}
