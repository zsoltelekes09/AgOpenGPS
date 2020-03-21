using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CBoundary
    {
        //copy of the mainform address
        private readonly FormGPS mf;

        /// <summary>
        /// array of boundaries
        /// </summary>
        ///
        public List<CBoundaryLines> bndArr = new List<CBoundaryLines>();
        public List<vec3> bndBeingMadePts = new List<vec3>();

        private readonly double scanWidth, boxLength;

        public double createBndOffset;
        public bool isBndBeingMade;

        public bool isDrawRightSide = true, isOkToAddPoints = false;
        //constructor
        public CBoundary(FormGPS _f)
        {
            mf = _f;
            boundarySelected = -1;
            scanWidth = 1.0;
            boxLength = 2000;
            LastBoundary = -1;
            //boundaries array
        }

        // the list of possible bounds points
        public List<vec4> bndClosestList = new List<vec4>();

        public int boundarySelected, LastBoundary, closestBoundaryNum;

        //generated box for finding closest point
        public vec2 boxA = new vec2(9000, 9000), boxB = new vec2(9000, 9002);

        public vec2 boxC = new vec2(9001, 9001), boxD = new vec2(9002, 9003);

        //point at the farthest boundary segment from pivotAxle
        public vec3 closestBoundaryPt = new vec3(-10000, -10000, 9);

        public void DrawBoundaryLines()
        {
            for (int i = 0; i < bndArr.Count; i++)
            {
                if (boundarySelected == i) GL.Color3(1.0f, 0.0f, 0.0f);
                else GL.Color3(0.95f, 0.5f, 0.250f);
                bndArr[i].DrawBoundaryLine();
            }


            if (bndBeingMadePts.Count > 0)
            {
                //the boundary so far
                vec3 pivot = mf.pivotAxlePos;
                GL.LineWidth(1);
                GL.Color3(0.825f, 0.22f, 0.90f);
                GL.Begin(PrimitiveType.LineLoop);
                for (int h = 0; h < bndBeingMadePts.Count; h++) GL.Vertex3(bndBeingMadePts[h].easting, bndBeingMadePts[h].northing, 0);
                GL.End();
                GL.Color3(0.295f, 0.972f, 0.290f);

                //line from last point to pivot marker
                GL.Color3(0.825f, 0.842f, 0.0f);
                GL.Enable(EnableCap.LineStipple);
                GL.LineStipple(1, 0x0700);
                GL.Begin(PrimitiveType.LineStrip);
                if (mf.bnd.isDrawRightSide)
                {
                    GL.Vertex3(bndBeingMadePts[0].easting, bndBeingMadePts[0].northing, 0);

                    GL.Vertex3(pivot.easting + (Math.Sin(pivot.heading - glm.PIBy2) * -mf.bnd.createBndOffset),
                            pivot.northing + (Math.Cos(pivot.heading - glm.PIBy2) * -mf.bnd.createBndOffset), 0);
                    GL.Vertex3(bndBeingMadePts[bndBeingMadePts.Count - 1].easting, bndBeingMadePts[bndBeingMadePts.Count - 1].northing, 0);
                }
                else
                {
                    GL.Vertex3(bndBeingMadePts[0].easting, bndBeingMadePts[0].northing, 0);

                    GL.Vertex3(pivot.easting + (Math.Sin(pivot.heading - glm.PIBy2) * mf.bnd.createBndOffset),
                            pivot.northing + (Math.Cos(pivot.heading - glm.PIBy2) * mf.bnd.createBndOffset), 0);
                    GL.Vertex3(bndBeingMadePts[bndBeingMadePts.Count - 1].easting, bndBeingMadePts[bndBeingMadePts.Count - 1].northing, 0);
                }
                GL.End();
                GL.Disable(EnableCap.LineStipple);

                //boundary points
                GL.Color3(0.0f, 0.95f, 0.95f);
                GL.PointSize(6.0f);
                GL.Begin(PrimitiveType.Points);
                for (int h = 0; h < bndBeingMadePts.Count; h++) GL.Vertex3(bndBeingMadePts[h].easting, bndBeingMadePts[h].northing, 0);
                GL.End();
            }
        }

        //draws the derived closest point
        public void DrawClosestPoint()
        {
            GL.PointSize(4.0f);
            GL.Color3(0.919f, 0.932f, 0.070f);
            GL.Begin(PrimitiveType.Points);
            GL.Vertex3(closestBoundaryPt.easting, closestBoundaryPt.northing, 0);
            GL.End();

            GL.LineWidth(1);
            GL.Color3(0.92f, 0.62f, 0.42f);
            GL.Begin(PrimitiveType.LineStrip);
            GL.Vertex3(boxD.easting, boxD.northing, 0);
            GL.Vertex3(boxA.easting, boxA.northing, 0);
            GL.Vertex3(boxB.easting, boxB.northing, 0);
            GL.Vertex3(boxC.easting, boxC.northing, 0);
            GL.End();
        }
    }
}