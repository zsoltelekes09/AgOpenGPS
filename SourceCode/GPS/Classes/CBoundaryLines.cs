using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CBoundaryLines
    {
        //constructor
        public CBoundaryLines()
        {
            area = 0;
            isDriveAround = false;
            isDriveThru = false;
        }

        //list of coordinates of boundary line
        public List<Vec3> bndLine = new List<Vec3>();
        public List<Vec3> bndArea = new List<Vec3>();

        //the list of constants and multiples of the boundary
        public List<Vec2> calcList = new List<Vec2>();

        public double Northingmin, Northingmax, Eastingmin, Eastingmax;

        //area variable
        public double area;

        //boundary variables
        public bool isDriveAround, isDriveThru;

        public void CalculateBoundaryHeadings()
        {
            //to calc heading based on next and previous points to give an average heading.
            int cnt = bndLine.Count;
            Vec3[] arr = new Vec3[cnt];
            cnt--;
            bndLine.CopyTo(arr);
            bndLine.Clear();

            //first point needs last, first, second points
            Vec3 pt3 = arr[0];
            pt3.heading = Math.Atan2(arr[1].easting - arr[cnt].easting, arr[1].northing - arr[cnt].northing);
            if (pt3.heading < 0) pt3.heading += Glm.twoPI;
            bndLine.Add(pt3);

            //middle points
            for (int i = 1; i < cnt; i++)
            {
                pt3 = arr[i];
                pt3.heading = Math.Atan2(arr[i + 1].easting - arr[i - 1].easting, arr[i + 1].northing - arr[i - 1].northing);
                if (pt3.heading < 0) pt3.heading += Glm.twoPI;
                bndLine.Add(pt3);
            }

            //last and first point
            pt3 = arr[cnt];
            pt3.heading = Math.Atan2(arr[0].easting - arr[cnt - 1].easting, arr[0].northing - arr[cnt - 1].northing);
            if (pt3.heading < 0) pt3.heading += Glm.twoPI;
            bndLine.Add(pt3);
        }

        public void FixBoundaryLine(int bndNum, double spacing)
        {
            //boundary point spacing based on eq width
            spacing *= 0.25;

            if (spacing < 1) spacing = 1;
            if (spacing > 3) spacing = 3;

            //make sure boundaries are wound correctly
            if (bndNum != 0) spacing *= 0.66;

            //make sure distance isn't too small between points on headland
            int bndCount = bndLine.Count;
            double distance;

            for (int i = 0; i < bndCount - 1; i++)
            {
                distance = Glm.Distance(bndLine[i], bndLine[i + 1]);
                if (distance < spacing)
                {
                    bndLine.RemoveAt(i + 1);
                    bndCount = bndLine.Count;
                    i--;
                }
            }

            //make sure distance isn't too big between points on boundary
            bndCount = bndLine.Count;
            spacing *= 1.33;

            for (int i = 0; i < bndCount; i++)
            {
                int j = i + 1;

                if (j == bndCount) j = 0;
                distance = Glm.Distance(bndLine[i], bndLine[j]);
                if (distance > spacing)
                {
                    Vec3 pointB = new Vec3((bndLine[i].easting + bndLine[j].easting) / 2.0,
                        (bndLine[i].northing + bndLine[j].northing) / 2.0, bndLine[i].heading);

                    bndLine.Insert(j, pointB);
                    bndCount = bndLine.Count;
                    i--;
                }
            }
            
            //make sure distance isn't too big between points on boundary
            bndCount = bndLine.Count;
            spacing *= 1.33;

            for (int i = 0; i < bndCount; i++)
            {
                int j = i + 1;

                if (j == bndCount) j = 0;
                distance = Glm.Distance(bndLine[i], bndLine[j]);
                if (distance > spacing)
                {
                    Vec3 pointB = new Vec3((bndLine[i].easting + bndLine[j].easting) / 2.0,
                        (bndLine[i].northing + bndLine[j].northing) / 2.0, bndLine[i].heading);

                    bndLine.Insert(j, pointB);
                    bndCount = bndLine.Count;
                    i--;
                }
            }

            //make sure headings are correct for calculated points
            CalculateBoundaryHeadings();
        }

        public void ReverseWinding()
        {
            //reverse the boundary
            int cnt = bndLine.Count;
            Vec3[] arr = new Vec3[cnt];
            cnt--;
            bndLine.CopyTo(arr);
            bndLine.Clear();
            for (int i = cnt; i >= 0; i--)
            {
                arr[i].heading -= Math.PI;
                if (arr[i].heading < 0) arr[i].heading += Glm.twoPI;
                bndLine.Add(arr[i]);
            }
        }

        public void PreCalcBoundaryLines()
        {
            int j = bndLine.Count - 1;
            //clear the list, constant is easting, multiple is northing
            calcList.Clear();
            Vec2 constantMultiple = new Vec2(0, 0);

            Northingmin = Northingmax = bndLine[0].northing;
            Eastingmin = Eastingmax = bndLine[0].easting;

            for (int i = 0; i < bndLine.Count; j = i++)
            {
                if (Northingmin > bndLine[i].northing) Northingmin = bndLine[i].northing;
                if (Northingmax < bndLine[i].northing) Northingmax = bndLine[i].northing;
                if (Eastingmin > bndLine[i].easting) Eastingmin = bndLine[i].easting;
                if (Eastingmax < bndLine[i].easting) Eastingmax = bndLine[i].easting;

                //check for divide by zero
                if (Math.Abs(bndLine[i].northing - bndLine[j].northing) < 0.00000000001)
                {
                    constantMultiple.easting = bndLine[i].easting;
                    constantMultiple.northing = 0;
                    calcList.Add(constantMultiple);
                }
                else
                {
                    //determine constant and multiple and add to list
                    constantMultiple.easting = bndLine[i].easting - ((bndLine[i].northing * bndLine[j].easting)
                                    / (bndLine[j].northing - bndLine[i].northing)) + ((bndLine[i].northing * bndLine[i].easting)
                                        / (bndLine[j].northing - bndLine[i].northing));
                    constantMultiple.northing = (bndLine[j].easting - bndLine[i].easting) / (bndLine[j].northing - bndLine[i].northing);
                    calcList.Add(constantMultiple);
                }
            }
        }

        public bool IsPointInsideBoundary(Vec3 TestPoint)
        {
            if (calcList.Count < 3) return false;
            int j = bndLine.Count - 1;
            bool oddNodes = false;

            if (TestPoint.northing > Northingmin || TestPoint.northing < Northingmax || TestPoint.easting > Eastingmin || TestPoint.easting < Eastingmax)
            {
                //test against the constant and multiples list the test point
                for (int i = 0; i < bndLine.Count; j = i++)
                {
                    if ((bndLine[i].northing < TestPoint.northing && bndLine[j].northing >= TestPoint.northing)
                    || (bndLine[j].northing < TestPoint.northing && bndLine[i].northing >= TestPoint.northing))
                    {
                        oddNodes ^= ((TestPoint.northing * calcList[i].northing) + calcList[i].easting < TestPoint.easting);
                    }
                }
            }
            return oddNodes; //true means inside.
        }

        public bool IsPointInsideBoundary(Vec2 TestPoint)
        {
            if (calcList.Count < 3) return false;
            int j = bndLine.Count - 1;
            bool oddNodes = false;

            if (TestPoint.northing > Northingmin || TestPoint.northing < Northingmax || TestPoint.easting > Eastingmin || TestPoint.easting < Eastingmax)
            {
                //test against the constant and multiples list the test point
                for (int i = 0; i < bndLine.Count; j = i++)
                {
                    if ((bndLine[i].northing < TestPoint.northing && bndLine[j].northing >= TestPoint.northing)
                    || (bndLine[j].northing < TestPoint.northing && bndLine[i].northing >= TestPoint.northing))
                    {
                        oddNodes ^= ((TestPoint.northing * calcList[i].northing) + calcList[i].easting < TestPoint.easting);
                    }
                }
            }
            return oddNodes; //true means inside.
        }

        public void DrawBoundaryLine()
        {
            ////draw the perimeter line so far
            if (bndLine.Count < 1) return;
            GL.LineWidth(2);
            int ptCount = bndLine.Count;

            GL.Begin(PrimitiveType.LineLoop);
            for (int h = 0; h < ptCount; h++) GL.Vertex3(bndLine[h].easting, bndLine[h].northing, 0);
            GL.End();
        }

        public void DrawBoundaryBackBuffer()
        {
            int ptCount = bndArea.Count;
            if (ptCount < 3) return;

            GL.Begin(PrimitiveType.Triangles);
            for (int h = 0; h < ptCount - 2; h++)
            {
                GL.Vertex3(bndArea[h].easting, bndArea[h].northing, 0);
            }
            GL.End();
        }

        public void CalculateBoundaryWinding()
        {
            int ptCount = bndLine.Count;
            if (ptCount < 3) return;

            double area2 = 0;         // Accumulates area in the loop
            Vec3 lastpoint = new Vec3(bndLine[ptCount - 1].easting + (-Math.Sin(Glm.PIBy2 + bndLine[ptCount - 1].heading) * 5f), bndLine[ptCount - 1].northing + (-Math.Cos(Glm.PIBy2 + bndLine[ptCount - 1].heading) * 5f), 0);

            for (int i = 0; i < ptCount; i++)
            {
                Vec3 point = new Vec3(bndLine[i].easting + (-Math.Sin(Glm.PIBy2 + bndLine[i].heading) * 5f), bndLine[i].northing + (-Math.Cos(Glm.PIBy2 + bndLine[i].heading) * 5f), 0);
                area2 += (lastpoint.easting + point.easting) * (lastpoint.northing - point.northing);
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
            if (ptCount < 1) return;

            area = 0;         // Accumulates area in the loop
            int j = ptCount - 1;  // The last vertex is the 'previous' one to the first

            for (int i = 0; i < ptCount; j = i++)
            {
                area += (bndLine[j].easting + bndLine[i].easting) * (bndLine[j].northing - bndLine[i].northing);
            }
            area = Math.Abs(area / 2);


            Tess tess = new Tess(bndLine);
            bndArea.Clear();
            for (int i = 0; i < tess.ElementCount; i++)
            {
                for (int k = 0; k < 3; k++)
                {
                    int index = tess.Elements[i * 3 + k];
                    bndArea.Add(new Vec3(tess.Vertices[index].easting, tess.Vertices[index].northing, 0));
                }
            }
        }
    }
}