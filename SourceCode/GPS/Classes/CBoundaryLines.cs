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
        public List<vec3> bndLine = new List<vec3>();
        public List<vec3> bndArea = new List<vec3>();

        //the list of constants and multiples of the boundary
        public List<vec2> calcList = new List<vec2>();

        public double Northingmin, Northingmax, Eastingmin, Eastingmax;

        //area variable
        public double area;

        //boundary variables
        public bool isDriveAround, isDriveThru;

        public void CalculateBoundaryHeadings()
        {
            //to calc heading based on next and previous points to give an average heading.
            int cnt = bndLine.Count;
            vec3[] arr = new vec3[cnt];
            cnt--;
            bndLine.CopyTo(arr);
            bndLine.Clear();

            //first point needs last, first, second points
            vec3 pt3 = arr[0];
            pt3.heading = Math.Atan2(arr[1].easting - arr[cnt].easting, arr[1].northing - arr[cnt].northing);
            if (pt3.heading < 0) pt3.heading += glm.twoPI;
            bndLine.Add(pt3);

            //middle points
            for (int i = 1; i < cnt; i++)
            {
                pt3 = arr[i];
                pt3.heading = Math.Atan2(arr[i + 1].easting - arr[i - 1].easting, arr[i + 1].northing - arr[i - 1].northing);
                if (pt3.heading < 0) pt3.heading += glm.twoPI;
                bndLine.Add(pt3);
            }

            //last and first point
            pt3 = arr[cnt];
            pt3.heading = Math.Atan2(arr[0].easting - arr[cnt - 1].easting, arr[0].northing - arr[cnt - 1].northing);
            if (pt3.heading < 0) pt3.heading += glm.twoPI;
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
                distance = glm.Distance(bndLine[i], bndLine[i + 1]);
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
                distance = glm.Distance(bndLine[i], bndLine[j]);
                if (distance > spacing)
                {
                    vec3 pointB = new vec3((bndLine[i].easting + bndLine[j].easting) / 2.0,
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
                distance = glm.Distance(bndLine[i], bndLine[j]);
                if (distance > spacing)
                {
                    vec3 pointB = new vec3((bndLine[i].easting + bndLine[j].easting) / 2.0,
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
            vec3[] arr = new vec3[cnt];
            cnt--;
            bndLine.CopyTo(arr);
            bndLine.Clear();
            for (int i = cnt; i >= 0; i--)
            {
                arr[i].heading -= Math.PI;
                if (arr[i].heading < 0) arr[i].heading += glm.twoPI;
                bndLine.Add(arr[i]);
            }
        }

        public void PreCalcBoundaryLines()
        {
            int j = bndLine.Count - 1;
            //clear the list, constant is easting, multiple is northing
            calcList.Clear();
            vec2 constantMultiple = new vec2(0, 0);

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

        public bool IsPointInsideBoundary(vec3 TestPoint)
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

        public bool IsPointInsideBoundary(vec2 TestPoint)
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
            for (int h = 0; h < ptCount - 2; h += 3)
            {
                GL.Vertex3(bndArea[h].easting, bndArea[h].northing, 0);
                GL.Vertex3(bndArea[h + 1].easting, bndArea[h + 1].northing, 0);
                GL.Vertex3(bndArea[h + 2].easting, bndArea[h + 2].northing, 0);
            }
            GL.End();
        }

        public void CalculateBoundaryWinding()
        {
            int ptCount = bndLine.Count;
            if (ptCount < 3) return;

            double area2 = 0;         // Accumulates area in the loop
            vec3 lastpoint = new vec3(bndLine[ptCount - 1].easting + (-Math.Sin(glm.PIBy2 + bndLine[ptCount - 1].heading) * 5f), bndLine[ptCount - 1].northing + (-Math.Cos(glm.PIBy2 + bndLine[ptCount - 1].heading) * 5f), 0);

            for (int i = 0; i < ptCount; i++)
            {
                vec3 point = new vec3(bndLine[i].easting + (-Math.Sin(glm.PIBy2 + bndLine[i].heading) * 5f), bndLine[i].northing + (-Math.Cos(glm.PIBy2 + bndLine[i].heading) * 5f), 0);
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

            var v = new ContourVertex[ptCount];
            for (int i = 0; i < ptCount; i++)
            {
                v[i].Position = new Vec3(bndLine[i].easting, bndLine[i].northing, 0);
            }

            Tess _tess = new Tess();
            _tess.AddContour(v, ContourOrientation.Clockwise);
            _tess.Tessellate(WindingRule.Positive, ElementType.Polygons, 3, null);

            bndArea.Clear();

            //var output = new List<Polygon>();
            for (int i = 0; i < _tess.ElementCount; i++)
            {
                for (int k = 0; k < 3; k++)
                {
                    int index = _tess.Elements[i * 3 + k];
                    if (index == -1) continue;
                    bndArea.Add(new vec3(_tess.Vertices[index].Position.X, _tess.Vertices[index].Position.Y, 0));
                }
            }
        }
    }
}