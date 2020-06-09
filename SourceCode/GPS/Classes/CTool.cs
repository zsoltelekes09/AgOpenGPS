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

        public double ToolWidth;
        public double ToolOverlap;
        public double WidthMinusOverlap;

        public List<CSection> section = new List<CSection>();

        public vec2 hitchPos;
        public vec3 toolPos;
        public vec3 tankPos;

        public double hitchLength;

        public double ToolFarLeftSpeed = 0;
        public double ToolFarRightSpeed = 0;

        public double toolTrailingHitchLength, toolTankTrailingHitchLength;
        public double ToolOffset;

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


        //read pixel values
        public int rpXPosition;
        public int rpWidth;

        //Constructor called by FormGPS
        public CTool(FormGPS _f, double num)
        {
            mf = _f;

            numOfSections = Properties.Vehicle.Default.setVehicle_numSections;


            for (int j = 0; j <= numOfSections; j++)
            {
                //section[j] = new CSection(_f);
                section.Add(new CSection(mf));

                _f.Controls.Add(section[j].SectionButton);

                section[j].SectionButton.BringToFront();
                section[j].SectionButton.Text = (j + 1).ToString();
                section[j].SectionButton.Name = (num + "," + j).ToString();
                section[j].SectionButton.Click += new System.EventHandler(mf.btnSectionMan_Click);

                section[j].SectionButton.Enabled = false;
                section[j].SectionButton.FlatAppearance.BorderColor = SystemColors.ActiveCaptionText;
                section[j].SectionButton.FlatStyle = FlatStyle.Flat;
                section[j].SectionButton.TextAlign = ContentAlignment.MiddleCenter;
                section[j].SectionButton.UseVisualStyleBackColor = false;
                section[j].SectionButton.BackColor = Color.Silver;
            }

            toolPos = new vec3(0, 0, 0);
            tankPos = new vec3(0, 0, 0);
            hitchPos = new vec2(0, 0);

            //from settings grab the vehicle specifics
            ToolWidth = Properties.Vehicle.Default.setVehicle_toolWidth;
            ToolOverlap = Properties.Vehicle.Default.setVehicle_toolOverlap;
            ToolOffset = Properties.Vehicle.Default.setVehicle_toolOffset;
            WidthMinusOverlap = ToolWidth - ToolOverlap;
            
            
            toolTrailingHitchLength = Properties.Vehicle.Default.setTool_toolTrailingHitchLength;
            toolTankTrailingHitchLength = Properties.Vehicle.Default.setVehicle_tankTrailingHitchLength;
            hitchLength = Properties.Vehicle.Default.setVehicle_hitchLength;

            isToolBehindPivot = Properties.Vehicle.Default.setTool_isToolBehindPivot;
            isToolTrailing = Properties.Vehicle.Default.setTool_isToolTrailing;
            isToolTBT = Properties.Vehicle.Default.setTool_isToolTBT;

            LookAheadOnSetting = Properties.Vehicle.Default.setVehicle_toolLookAheadOn;
            LookAheadOffSetting = Properties.Vehicle.Default.setVehicle_toolLookAheadOff;
            TurnOffDelay = Properties.Vehicle.Default.setVehicle_toolOffDelay;

            MappingOnDelay = Properties.Vehicle.Default.setVehicle_MappingOnDelay;
            MappingOffDelay = Properties.Vehicle.Default.setVehicle_MappingOffDelay;

            toolMinUnappliedPixels = Properties.Vehicle.Default.setVehicle_minApplied;

        }

        public void DrawTool()
        {
            //translate down to the hitch pin
            GL.Translate(0.0, hitchLength, 0.0);

            //there is a trailing tow between hitch
            if (isToolTrailing)
            {
                if (isToolTBT)
                {
                    //rotate to tank heading
                    GL.Rotate(Glm.ToDegrees(mf.fixHeading - tankPos.heading), 0.0, 0.0, 1.0);

                    //draw the tank hitch
                    GL.LineWidth(2f);
                    GL.Color3(0.7f, 0.7f, 0.97f);
                    GL.Begin(PrimitiveType.Lines);
                    GL.Vertex3(0.0, toolTankTrailingHitchLength, 0.0);
                    GL.Vertex3(0, 0, 0);
                    GL.End();

                    //pivot markers
                    GL.Color3(0.95f, 0.95f, 0f);
                    GL.PointSize(6f);
                    GL.Begin(PrimitiveType.Points);
                    GL.Vertex3(0.0, toolTankTrailingHitchLength, 0.0);
                    GL.End();

                    //move down the tank hitch, unwind, rotate to section heading
                    GL.Translate(0.0, toolTankTrailingHitchLength, 0.0);
                    //GL.Rotate(Glm.ToDegrees(tankPos.heading), 0.0, 0.0, 1.0);
                }


                GL.Rotate(Glm.ToDegrees(tankPos.heading - toolPos.heading), 0.0, 0.0, 1.0);

                //draw the hitch
                GL.LineWidth(2);
                GL.Color3(0.7f, 0.7f, 0.97f);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(0.0, toolTrailingHitchLength, 0.0);
                GL.Vertex3(0, 0, 0);
                GL.End();

                GL.Translate(0.0, toolTrailingHitchLength, 0.0);


            }

            
            //look ahead lines
            GL.LineWidth(1);
            GL.Begin(PrimitiveType.Lines);
            
            //lookahead section on
            GL.Color3(0.20f, 0.7f, 0.2f);//-5.25  and 5.35
            GL.Vertex3(section[0].positionLeft, (lookAheadDistanceOnPixelsLeft) * 0.1, 0);
            GL.Vertex3(section[numOfSections - 1].positionRight, (lookAheadDistanceOnPixelsRight) * 0.1, 0);

            //lookahead section off
            GL.Color3(0.70f, 0.2f, 0.2f);
            GL.Vertex3(section[0].positionLeft, (lookAheadDistanceOffPixelsLeft) * 0.1, 0);
            GL.Vertex3(section[numOfSections - 1].positionRight, (lookAheadDistanceOffPixelsRight) * 0.1, 0);

            if (mf.vehicle.isHydLiftOn)
            {
                GL.Color3(0.70f, 0.2f, 0.72f);
                GL.Vertex3(section[0].positionLeft, (mf.vehicle.hydLiftLookAheadDistanceLeft * 0.1), 0);
                GL.Vertex3(section[numOfSections - 1].positionRight, (mf.vehicle.hydLiftLookAheadDistanceRight * 0.1), 0);
            }

            GL.End();

            //draw the sections
            GL.LineWidth(2);

            double hite = mf.camera.camSetDistance / -100;
            if (hite > 1.3) hite = 1.0;
            if (hite < 0.5) hite = 0.5;


            for (int j = 0; j < numOfSections; j++)
            {
                //if section is on, green, if off, red color
                if (section[j].IsSectionOn || section[numOfSections].IsSectionOn)
                {
                    if (section[j].BtnSectionState == FormGPS.btnStates.Auto)
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

                double mid = (((section[j].positionRight + 100) - (section[j].positionLeft + 100))) / 2 + section[j].positionLeft;

                GL.Begin(PrimitiveType.TriangleFan);
                {
                    GL.Vertex3(section[j].positionLeft, 0, 0);
                    GL.Vertex3(section[j].positionLeft, -hite, 0);

                    GL.Vertex3(mid, -hite * 1.5, 0);

                    GL.Vertex3(section[j].positionRight, -hite, 0);
                    GL.Vertex3(section[j].positionRight, 0, 0);
                }
                GL.End();

                GL.Begin(PrimitiveType.LineLoop);
                {
                    GL.Color3(0.0, 0.0, 0.0);
                    GL.Vertex3(section[j].positionLeft, 0, 0);
                    GL.Vertex3(section[j].positionLeft, -hite, 0);

                    GL.Vertex3(mid, -hite * 1.5, 0);

                    GL.Vertex3(section[j].positionRight, -hite, 0);
                    GL.Vertex3(section[j].positionRight, 0, 0);
                }
                GL.End();
            }

            //GL.End();

            //draw section markers if close enough
            if (mf.camera.camSetDistance > -250)
            {
                GL.Color3(0.0f, 0.0f, 0.0f);
                //section markers
                GL.PointSize(3.0f);
                GL.Begin(PrimitiveType.Points);
                for (int j = 0; j < numOfSections - 1; j++)
                    GL.Vertex3(section[j].positionRight, 0, 0);
                GL.End();
            }


            if (isToolTrailing)
            {
                GL.Translate(0.0, -toolTrailingHitchLength, 0.0);
                GL.Rotate(Glm.ToDegrees(tankPos.heading - toolPos.heading), 0.0, 0.0, -1.0);

                if (isToolTBT)
                {
                    GL.Translate(0.0, -toolTankTrailingHitchLength, 0.0);
                    GL.Rotate(Glm.ToDegrees(mf.fixHeading - tankPos.heading), 0.0, 0.0, -1.0);
                }

            }
            GL.Translate(0.0, -hitchLength, 0.0);



        }
    }
}
