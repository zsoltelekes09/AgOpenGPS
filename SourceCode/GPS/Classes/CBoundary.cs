using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AgOpenGPS
{
    public partial class CBoundary
    {
        //copy of the mainform address
        private readonly FormGPS mf;

        public List<CBoundaryLines> bndArr = new List<CBoundaryLines>();

        public List<Vec3> bndBeingMadePts = new List<Vec3>();

        public double createBndOffset;
        public bool isBndBeingMade;

        public bool isDrawRightSide = true, isOkToAddPoints = false;

        public int boundarySelected = -1, closestBoundaryNum;

        public bool BtnHeadLand = false;
        public bool isToolUp = true;

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
    }

    public partial class CBoundaryLines
    {
        //list of coordinates of boundary line
        public List<Vec3> bndLine = new List<Vec3>();
        public List<int> Indexer = new List<int>();

        public double Northingmin, Northingmax, Eastingmin, Eastingmax;

        //area variable
        public double Area = 0;

        //boundary variables
        public bool isDriveAround, isDriveThru;

        public void FixBoundaryLine(CancellationToken ct)
        {
            double area = Math.Abs(bndLine.PolygonArea(ct, true) / 2.0);

            double Multiplier = Math.Max(1, Math.Min((area / 10000) / 10000, 10));

            if (!ct.IsCancellationRequested) Area = area;

            double MinDist = 2 * Multiplier, MaxDist = 4 * Multiplier;

            double distance;
            for (int i = 0; i < bndLine.Count; i++)
            {
                int j = (i == bndLine.Count - 1) ? 0 : i + 1;

                //make sure distance isn't too small between points on turnLine
                distance = Glm.Distance(bndLine[i], bndLine[j]);
                if (distance < MinDist)
                {
                    bndLine.RemoveAt(j);
                    i--;
                }
                else if (distance > MaxDist)//make sure distance isn't too big between points on turnLine
                {
                    double northing = bndLine[i].Northing / 2 + bndLine[j].Northing / 2;
                    double easting = bndLine[i].Easting / 2 + bndLine[j].Easting / 2;
                    double heading = bndLine[i].Heading / 2 + bndLine[j].Heading / 2;

                    if (j == 0) bndLine.Add(new Vec3(northing, easting, heading));
                    else bndLine.Insert(j, new Vec3(northing, easting, heading));
                    i--;
                }
            }
        }

        public void DrawBoundaryLine()
        {
            ////draw the perimeter line so far
            if (bndLine.Count < 1) return;
            GL.LineWidth(2);

            GL.Begin(PrimitiveType.LineLoop);
            for (int h = 0; h < bndLine.Count; h++) GL.Vertex3(bndLine[h].Easting, bndLine[h].Northing, 0);
            GL.End();
        }

        public void DrawBoundaryBackBuffer()
        {
            GL.Begin(PrimitiveType.Triangles);
            for (int j = 0; j < Indexer.Count; j++)
            {
                GL.Vertex3(bndLine[Indexer[j]].Easting, bndLine[Indexer[j]].Northing, 0);
            }
            GL.End();
        }

        public void BoundaryMinMax(CancellationToken ct)
        {
            if (bndLine.Count > 0)
            {
                Northingmin = Northingmax = bndLine[0].Northing;
                Eastingmin = Eastingmax = bndLine[0].Easting;

                for (int i = 0; i < bndLine.Count; i++)
                {
                    if (ct.IsCancellationRequested) break;
                    if (Northingmin > bndLine[i].Northing) Northingmin = bndLine[i].Northing;
                    if (Northingmax < bndLine[i].Northing) Northingmax = bndLine[i].Northing;
                    if (Eastingmin > bndLine[i].Easting) Eastingmin = bndLine[i].Easting;
                    if (Eastingmax < bndLine[i].Easting) Eastingmax = bndLine[i].Easting;
                }
            }
        }
    }
}