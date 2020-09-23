using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CBoundaryLines
    {

        //list of coordinates of boundary line
        public List<Vec3> bndLine = new List<Vec3>();
        public Vec3[] bndVertices;
        public int[] bndIndexer = new int[0];

        public double Northingmin, Northingmax, Eastingmin, Eastingmax;

        //area variable
        public double area = 0;

        //boundary variables
        public bool isDriveAround, isDriveThru;

        public void FixBoundaryLine()
        {
            double distance;
            for (int i = 0; i < bndLine.Count; i++)
            {
                int j = (i == bndLine.Count - 1) ? 0 : i + 1;

                //make sure distance isn't too small between points on turnLine
                distance = Glm.Distance(bndLine[i], bndLine[j]);
                if (distance < 2)
                {
                    bndLine.RemoveAt(j);
                    i--;
                }
                else if (distance > 4)//make sure distance isn't too big between points on turnLine
                {
                    double northing = bndLine[i].Northing / 2 + bndLine[j].Northing / 2;
                    double easting = bndLine[i].Easting / 2 + bndLine[j].Easting / 2;
                    double heading = bndLine[i].Heading / 2 + bndLine[j].Heading / 2;


                    if (j == 0) bndLine.Add(new Vec3(northing, easting, heading));
                    else bndLine.Insert(j, new Vec3(northing, easting, heading));
                    i--;
                }
            }
            bndLine.CalculateHeadings(true);
        }

        public void ReverseWinding()
        {
            bndLine.Reverse();
            bndLine.CalculateHeadings(true);
        }

        public void PreCalcBoundaryLines()
        {
            Northingmin = Northingmax = bndLine[0].Northing;
            Eastingmin = Eastingmax = bndLine[0].Easting;

            for (int i = 0; i < bndLine.Count; i++)
            {
                if (Northingmin > bndLine[i].Northing) Northingmin = bndLine[i].Northing;
                if (Northingmax < bndLine[i].Northing) Northingmax = bndLine[i].Northing;
                if (Eastingmin > bndLine[i].Easting) Eastingmin = bndLine[i].Easting;
                if (Eastingmax < bndLine[i].Easting) Eastingmax = bndLine[i].Easting;
            }
        }

        public void DrawBoundaryLine()
        {
            ////draw the perimeter line so far
            if (bndLine.Count < 1) return;
            GL.LineWidth(2);
            int ptCount = bndLine.Count;

            GL.Begin(PrimitiveType.Lines);
            for (int h = 0; h < ptCount; h++) GL.Vertex3(bndLine[h].Easting, bndLine[h].Northing, 0);
            GL.End();
        }

        public void DrawBoundaryBackBuffer()
        {
            int ptCount = bndIndexer.Length;
            if (ptCount < 3) return;

            GL.Begin(PrimitiveType.Triangles);
            for (int h = 0; h < ptCount; h++)
            {
                GL.Vertex3(bndVertices[bndIndexer[h]].Easting, bndVertices[bndIndexer[h]].Northing, 0);
            }
            GL.End();
        }

        public void CalculateBoundaryWinding()
        {
            int ptCount = bndLine.Count;
            if (ptCount < 3) return;

            double area2 = 0;         // Accumulates area in the loop
            Vec3 lastpoint = new Vec3(bndLine[ptCount - 1].Northing + Math.Sin(bndLine[ptCount - 1].Heading) * -5f, bndLine[ptCount - 1].Easting + Math.Cos(bndLine[ptCount - 1].Heading) * 5f, 0);

            for (int i = 0; i < ptCount; i++)
            {
                Vec3 point = new Vec3(bndLine[i].Northing + Math.Sin(bndLine[i].Heading) * -5f, bndLine[i].Easting + Math.Cos(bndLine[ptCount - 1].Heading) * 5f, 0);
                area2 += (lastpoint.Easting + point.Easting) * (lastpoint.Northing - point.Northing);
                lastpoint = point;
            }
            area2 = Math.Abs(area2 / 2);
            if (area2 > area)
            {
                ReverseWinding();
            }
        }

        public void CalculateBoundaryArea()
        {
            int ptCount = bndLine.Count;

            area = 0;
            if (ptCount < 3) return;
            int j = ptCount - 1;

            for (int i = 0; i < ptCount; j = i++)
            {
                area += (bndLine[j].Easting + bndLine[i].Easting) * (bndLine[j].Northing - bndLine[i].Northing);
            }
            area = Math.Abs(area / 2);

            if (area > 0.01)
            {
                Tess _tess = new Tess();
                _tess.AddContour(bndLine, ContourOrientation.CounterClockwise);
                _tess.Tessellate(WindingRule.Positive, ElementType.Polygons, 3);
                bndIndexer = _tess.Elements;
                bndVertices = _tess.Vertices;
            }
        }
    }
}