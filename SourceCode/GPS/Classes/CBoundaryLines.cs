using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CBoundaryLines
    {

        //list of coordinates of boundary line
        public List<Vec3> bndLine = new List<Vec3>();
        readonly List<int> Indexer = new List<int>();

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
        }

        public void DrawBoundaryLine()
        {
            ////draw the perimeter line so far
            if (bndLine.Count < 1) return;
            GL.LineWidth(2);
            int ptCount = bndLine.Count;

            GL.Begin(PrimitiveType.LineLoop);
            for (int h = 0; h < ptCount; h++) GL.Vertex3(bndLine[h].Easting, bndLine[h].Northing, 0);
            GL.End();
        }

        public void DrawBoundaryBackBuffer()
        {
            GL.Begin(PrimitiveType.Triangles);
            for (int j = Indexer.Count - 1; j > -1; j--)
            {
                GL.Vertex3(bndLine[Indexer[j]].Easting, bndLine[Indexer[j]].Northing, 0);
            }
            GL.End();
        }

        public void CalculateBoundaryArea()
        {
            if (bndLine.Count > 2)
            {
                area = Math.Abs(bndLine.PolygonArea(true) / 2.0);

                Indexer.Clear();
                if (area > 99)
                {
                    bndLine.CalculateRoundedCorner(0.5, true, 0.0436332, 5);

                    Indexer.AddRange(bndLine.TriangulatePolygon());

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
            }
        }
    }
}