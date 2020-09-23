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
        public List<Vec3> bndBeingMadePts = new List<Vec3>();

        public double createBndOffset;
        public bool isBndBeingMade;

        public bool isDrawRightSide = true, isOkToAddPoints = false;

        public int boundarySelected = -1, closestBoundaryNum;

        //point at the farthest boundary segment from pivotAxle
        public Vec3 closestBoundaryPt = new Vec3(-10000, -10000, 9);

        //constructor
        public CBoundary(FormGPS _f)
        {
            mf = _f;
        }

        public void DrawBoundaryLines()
        {
            for (int i = 0; i < bndArr.Count; i++)
            {
                if (boundarySelected == i) GL.Color3(1.0f, 0.0f, 0.0f);
                else GL.Color3(0.95f, 0.5f, 0.250f);
                if (bndArr[i].Northingmin > mf.worldGrid.NorthingMax || bndArr[i].Northingmax < mf.worldGrid.NorthingMin) continue;
                if (bndArr[i].Eastingmin > mf.worldGrid.EastingMax || bndArr[i].Eastingmax < mf.worldGrid.EastingMin) continue;
                bndArr[i].DrawBoundaryLine();
            }

            if (bndBeingMadePts.Count > 0)
            {
                //the boundary so far
                Vec3 pivot = mf.pivotAxlePos;
                GL.LineWidth(1);
                GL.Color3(0.825f, 0.22f, 0.90f);
                GL.Begin(PrimitiveType.LineStrip);
                for (int h = 0; h < bndBeingMadePts.Count; h++) GL.Vertex3(bndBeingMadePts[h].Easting, bndBeingMadePts[h].Northing, 0);
                GL.End();
                GL.Color3(0.295f, 0.972f, 0.290f);

                //line from last point to pivot marker
                GL.Color3(0.825f, 0.842f, 0.0f);
                GL.Enable(EnableCap.LineStipple);
                GL.LineStipple(1, 0x0700);
                GL.Begin(PrimitiveType.LineStrip);
                if (mf.bnd.isDrawRightSide)
                {
                    GL.Vertex3(bndBeingMadePts[0].Easting, bndBeingMadePts[0].Northing, 0);
                    GL.Vertex3(pivot.Easting + Math.Cos(pivot.Heading) * mf.bnd.createBndOffset, pivot.Northing + Math.Sin(pivot.Heading) * -mf.bnd.createBndOffset, 0);
                    GL.Vertex3(bndBeingMadePts[bndBeingMadePts.Count - 1].Easting, bndBeingMadePts[bndBeingMadePts.Count - 1].Northing, 0);
                }
                else
                {
                    GL.Vertex3(bndBeingMadePts[0].Easting, bndBeingMadePts[0].Northing, 0);
                    GL.Vertex3(pivot.Easting + (Math.Cos(pivot.Heading) * -mf.bnd.createBndOffset), pivot.Northing + (Math.Sin(pivot.Heading) * mf.bnd.createBndOffset), 0);
                    GL.Vertex3(bndBeingMadePts[bndBeingMadePts.Count - 1].Easting, bndBeingMadePts[bndBeingMadePts.Count - 1].Northing, 0);
                }
                GL.End();
                GL.Disable(EnableCap.LineStipple);

                //boundary points
                GL.Color3(0.0f, 0.95f, 0.95f);
                GL.PointSize(6.0f);
                GL.Begin(PrimitiveType.Points);
                for (int h = 0; h < bndBeingMadePts.Count; h++) GL.Vertex3(bndBeingMadePts[h].Easting, bndBeingMadePts[h].Northing, 0);
                GL.End();
            }
        }

        //draws the derived closest point
        public void DrawClosestPoint()
        {
            GL.PointSize(4.0f);
            GL.Color3(0.919f, 0.932f, 0.070f);
            GL.Begin(PrimitiveType.Points);
            GL.Vertex3(closestBoundaryPt.Easting, closestBoundaryPt.Northing, 0);
            GL.End();

        }
    }
}