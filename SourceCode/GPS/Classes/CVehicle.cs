//Please, if you use this, share the improvements

using OpenTK.Graphics.OpenGL;
using System;

namespace AgOpenGPS
{
    public class CVehicle
    {
        private readonly FormGPS mf;

        public bool isSteerAxleAhead;
        public bool isPivotBehindAntenna;

        public double antennaHeight;
        public double antennaPivot;
        public double wheelbase;
        public double minTurningRadius;
        public double antennaOffset;
        public int vehicleType;

        //min vehicle speed allowed before turning shit off
        public double slowSpeedCutoff = 0;

        //autosteer values
        public double goalPointLookAheadSeconds, goalPointLookAheadMinimumDistance, goalPointDistanceMultiplier, goalPointLookAheadUturnMult;

        public double stanleyGain, stanleyHeadingErrorGain;
        public double minLookAheadDistance = 2.0;
        public double maxSteerAngle;
        public double maxAngularVelocity;
        public double treeSpacing;
        public double hydLiftLookAheadTime;
        
        public double hydLiftLookAheadDistanceLeft, hydLiftLookAheadDistanceRight;

        public bool isHydLiftOn;

        public CVehicle(FormGPS _f)
        {
            //constructor
            mf = _f;

            isPivotBehindAntenna = Properties.Vehicle.Default.setVehicle_isPivotBehindAntenna;
            antennaHeight = Properties.Vehicle.Default.setVehicle_antennaHeight;
            antennaPivot = Properties.Vehicle.Default.setVehicle_antennaPivot;
            antennaOffset = Properties.Vehicle.Default.setVehicle_antennaOffset;

            wheelbase = Properties.Vehicle.Default.setVehicle_wheelbase;
            minTurningRadius = Properties.Vehicle.Default.setVehicle_minTurningRadius;
            isSteerAxleAhead = Properties.Vehicle.Default.setVehicle_isSteerAxleAhead;

            slowSpeedCutoff = Properties.Vehicle.Default.setVehicle_slowSpeedCutoff;

            goalPointLookAheadSeconds = Properties.Vehicle.Default.setVehicle_goalPointLookAhead;
            goalPointLookAheadMinimumDistance = Properties.Vehicle.Default.setVehicle_lookAheadMinimum;
            goalPointDistanceMultiplier = Properties.Vehicle.Default.setVehicle_lookAheadDistanceFromLine;
            goalPointLookAheadUturnMult = Properties.Vehicle.Default.setVehicle_goalPointLookAheadUturnMult;

            stanleyGain = Properties.Vehicle.Default.setVehicle_stanleyGain;
            stanleyHeadingErrorGain = Properties.Vehicle.Default.setVehicle_stanleyHeadingErrorGain;

            maxAngularVelocity = Properties.Vehicle.Default.setVehicle_maxAngularVelocity;
            maxSteerAngle = Properties.Vehicle.Default.setVehicle_maxSteerAngle;

            isHydLiftOn = false;

            //treeSpacing = Properties.Settings.Default.setDistance_TreeSpacing;
            treeSpacing = 0;

            vehicleType = Properties.Vehicle.Default.setVehicle_vehicleType;

            hydLiftLookAheadTime = Properties.Vehicle.Default.setVehicle_hydraulicLiftLookAhead;
        }

        public double UpdateGoalPointDistance(double distanceFromCurrentLine)
        {
            //how far should goal point be away  - speed * seconds * kmph -> m/s then limit min value
            double goalPointDistance = mf.pn.speed * goalPointLookAheadSeconds * 0.27777777;

            if (distanceFromCurrentLine < 1.0)
                goalPointDistance += distanceFromCurrentLine * goalPointDistance * mf.vehicle.goalPointDistanceMultiplier;
            else
                goalPointDistance += goalPointDistance * mf.vehicle.goalPointDistanceMultiplier;

            if (goalPointDistance < mf.vehicle.goalPointLookAheadMinimumDistance) goalPointDistance = mf.vehicle.goalPointLookAheadMinimumDistance;

            mf.lookaheadActual = goalPointDistance;

            return goalPointDistance;
        }

        public void DrawVehicle()
        {
            //draw vehicle
            GL.Translate(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing, 0);
            GL.Rotate(Glm.ToDegrees(-mf.fixHeading), 0.0, 0.0, 1.0);
            GL.PushMatrix();


            GL.PointSize(6.0f);

            //antenna
            GL.Color3(0.0f, 0.95f, 0.95f);

            for (int i = 0; i < mf.Tools.Count; i++)
            {
                if (mf.Tools[i].isToolBehindPivot)
                {
                    GL.Begin(PrimitiveType.Points);
                    //hitch pin
                    GL.Vertex3(0, antennaPivot, 0);
                    GL.Color3(0.95f, 0.0f, 0.0f);
                    GL.Vertex3(0, mf.Tools[i].hitchLength, 0);
                    GL.End();
                }
            }

            if (!mf.vehicle.isHydLiftOn)
            {
                GL.Color3(0.9, 0.90, 0.0);
                GL.Begin(PrimitiveType.TriangleFan);
                GL.Vertex3(0, antennaPivot, -0.0);
                GL.Vertex3(1.0, -0, 0.0);
                GL.Color3(0.0, 0.90, 0.92);
                GL.Vertex3(0, wheelbase, 0.0);
                GL.Color3(0.920, 0.0, 0.9);
                GL.Vertex3(-1.0, -0, 0.0);
                GL.Vertex3(1.0, -0, 0.0);
                GL.End();
            }
            else
            {
                if (mf.hd.isToolUp)
                {
                    GL.Color3(0.0, 0.950, 0.0);
                    GL.Begin(PrimitiveType.TriangleFan);
                    GL.Vertex3(0, antennaPivot, -0.0);
                    GL.Vertex3(1.0, -0, 0.0);
                    GL.Vertex3(0, wheelbase, 0.0);
                    GL.Vertex3(-1.0, -0, 0.0);
                    GL.Vertex3(1.0, -0, 0.0);
                    GL.End();
                }
                else
                {
                    GL.Color3(0.950, 0.0, 0.0);
                    GL.Begin(PrimitiveType.TriangleFan);
                    GL.Vertex3(0, antennaPivot, -0.0);
                    GL.Vertex3(1.0, -0, 0.0);
                    GL.Vertex3(0, wheelbase, 0.0);
                    GL.Vertex3(-1.0, -0, 0.0);
                    GL.Vertex3(1.0, -0, 0.0);
                    GL.End();
                }
            }


            GL.LineWidth(3);
            GL.Color3(0.0, 0.0, 0.0);
            GL.Begin(PrimitiveType.LineLoop);
            {
                GL.Vertex3(-1.0, 0, 0);
                GL.Vertex3(1.0, 0, 0);
                GL.Vertex3(0, wheelbase, 0);
            }
            GL.End();




            # region Front Arrow

            GL.Begin(PrimitiveType.TriangleFan);
            {
                GL.Color3(0.950, 0.95, 0.32);
                GL.Vertex3(0, 5.5, -0.0);
                GL.Vertex3(0.35, 4.85, 0.0);
                GL.Vertex3(0, 5.2, 0.0);
                GL.Vertex3(-0.35, 4.85, 0.0);
                GL.Vertex3(0, 5.5, 0.0);
            }
            GL.End();

            GL.LineWidth(1);
            GL.Begin(PrimitiveType.LineLoop);
            {
                GL.Color3(0.0, 0.0, 0.0);
                GL.Vertex3(0, 5.5, -0.0);
                GL.Vertex3(0.35, 4.85, 0.0);
                GL.Vertex3(0, 5.2, 0.0);
                GL.Vertex3(-0.35, 4.85, 0.0);
                GL.Vertex3(0, 5.5, 0.0);
            }
            GL.End();

            #endregion

            if (mf.bnd.isBndBeingMade)
            {
                if (mf.bnd.isDrawRightSide)
                {
                    GL.LineWidth(2);
                    GL.Color3(0.0, 0.970, 0.0);
                    GL.Begin(PrimitiveType.LineStrip);
                    {
                        GL.Vertex3(0.0, 0, 0);
                        GL.Color3(0.970, 0.920, 0.20);
                        GL.Vertex3(mf.bnd.createBndOffset, 0, 0);
                        GL.Vertex3(mf.bnd.createBndOffset*0.75, 0.25, 0);
                    }
                    GL.End();

                }

                //draw on left side
                else
                {
                    GL.LineWidth(2);
                    GL.Color3(0.0, 0.970, 0.0);
                    GL.Begin(PrimitiveType.LineStrip);
                    {
                        GL.Vertex3(0.0, 0, 0);
                        GL.Color3(0.970, 0.920, 0.20);
                        GL.Vertex3(-mf.bnd.createBndOffset, 0, 0);
                        GL.Vertex3(-mf.bnd.createBndOffset*0.75, 0.25, 0);
                    }
                    GL.End();
                }
            }

            //if (mf.camera.camSetDistance > -150)
            //{
            //    GL.LineWidth(2);
            //    //Svenn Arrow
            //    GL.Color3(0.9, 0.95, 0.10);
            //    GL.Begin(PrimitiveType.LineStrip);
            //    {
            //        GL.Vertex3(0.6, wheelbase + 6, 0.0);
            //        GL.Vertex3(0, wheelbase + 8, 0.0);
            //        GL.Vertex3(-0.6, wheelbase + 6, 0.0);
            //    }
            //    GL.End();
            //}

            if (mf.curve.isBtnCurveOn && !mf.ct.isContourBtnOn)
            {
                GL.Color4(0.969, 0.95, 0.9510, 0.87);
                if (mf.curve.curveNumber == 0) mf.font.DrawTextVehicle(0, wheelbase, "0", 1.5);
                else  if (mf.curve.curveNumber < 0) mf.font.DrawTextVehicle(0, wheelbase, (-mf.curve.curveNumber) + "R", 1.5);
                else mf.font.DrawTextVehicle(0, wheelbase, mf.curve.curveNumber.ToString() + "L", 1.5);
            }
            else if (mf.ABLine.isBtnABLineOn && !mf.ct.isContourBtnOn)
            {
                GL.Color4(0.96, 0.95, 0.9510, 0.87);

                if (mf.ABLine.passNumber == 0) mf.font.DrawTextVehicle(0, wheelbase, "0", 1.5);
                else if (mf.ABLine.passNumber < 0) mf.font.DrawTextVehicle(0, wheelbase, -mf.ABLine.passNumber + "R", 1.5);
                else mf.font.DrawTextVehicle(0, wheelbase, mf.ABLine.passNumber.ToString() + "L", 1.5);
            }

            //draw the rigid hitch
            GL.Color3(0.37f, 0.37f, 0.97f);

            for (int i = 0; i < mf.Tools.Count; i++)
            {
                if (mf.Tools[i].isToolBehindPivot)
                {
                    GL.Begin(PrimitiveType.Lines);
                    GL.Vertex3(0, mf.Tools[i].hitchLength, 0);
                    GL.Vertex3(0, 0, 0);
                    GL.End();
                }
            }



            GL.LineWidth(1);

            if (mf.camera.camSetDistance < -1000)
            {
                GL.Color4(0.5f, 0.5f, 0.9f, 0.20);
                double theta = Glm.twoPI / 20;
                double c = Math.Cos(theta);//precalculate the sine and cosine
                double s = Math.Sin(theta);

                double x = mf.camera.camSetDistance * -.015;//we start at angle = 0
                double y = 0;
                GL.LineWidth(1);
                GL.Begin(PrimitiveType.TriangleFan);
                GL.Vertex3(x, y, 0.0);
                for (int ii = 0; ii < 20; ii++)
                {
                    //output vertex
                    GL.Vertex3(x, y, 0.0);

                    //apply the rotation matrix
                    double t = x;
                    x = (c * x) - (s * y);
                    y = (s * t) + (c * y);
                    // GL.Vertex3(x, y, 0.0);
                }
                GL.End();
                GL.Color3(0.5f, 0.9f, 0.2f);
                GL.LineWidth(1);
                GL.Begin(PrimitiveType.LineLoop);

                for (int ii = 0; ii < 20; ii++)
                {
                    //output vertex
                    GL.Vertex3(x, y, 0.0);

                    //apply the rotation matrix
                    double t = x;
                    x = (c * x) - (s * y);
                    y = (s * t) + (c * y);
                    // GL.Vertex3(x, y, 0.0);
                }
                GL.End();
            }
        }
    }
}