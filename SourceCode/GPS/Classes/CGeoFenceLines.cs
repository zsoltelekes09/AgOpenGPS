using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AgOpenGPS
{
    public partial class CBoundaryLines
    {
        //list of coordinates of boundary line
        public List<Vec3> geoFenceLine = new List<Vec3>();

        //the list of constants and multiples of the boundary
        public List<Vec2> GeocalcList = new List<Vec2>();

        public bool IsPointInGeoFenceArea(Vec3 TestPoint)
        {
            if (GeocalcList.Count < 3 || GeocalcList.Count < geoFenceLine.Count) return false;
            int j = geoFenceLine.Count - 1;
            bool oddNodes = false;

            if (TestPoint.Northing > Northingmin || TestPoint.Northing < Northingmax || TestPoint.Easting > Eastingmin || TestPoint.Easting < Eastingmax)
            {
                //test against the constant and multiples list the test point
                for (int i = 0; i < geoFenceLine.Count; j = i++)
                {
                    if ((geoFenceLine[i].Northing < TestPoint.Northing && geoFenceLine[j].Northing >= TestPoint.Northing)
                    || (geoFenceLine[j].Northing < TestPoint.Northing && geoFenceLine[i].Northing >= TestPoint.Northing))
                    {
                        oddNodes ^= ((TestPoint.Northing * GeocalcList[i].Northing) + GeocalcList[i].Easting < TestPoint.Easting);
                    }
                }
            }
            return oddNodes; //true means inside.
        }

        public bool IsPointInGeoFenceArea(Vec2 TestPoint)
        {
            if (GeocalcList.Count < 3) return false;
            int j = geoFenceLine.Count - 1;
            bool oddNodes = false;

            if (TestPoint.Northing > Northingmin || TestPoint.Northing < Northingmax || TestPoint.Easting > Eastingmin || TestPoint.Easting < Eastingmax)
            {
                //test against the constant and multiples list the test point
                for (int i = 0; i < geoFenceLine.Count; j = i++)
                {
                    if ((geoFenceLine[i].Northing < TestPoint.Northing && geoFenceLine[j].Northing >= TestPoint.Northing)
                    || (geoFenceLine[j].Northing < TestPoint.Northing && geoFenceLine[i].Northing >= TestPoint.Northing))
                    {
                        oddNodes ^= ((TestPoint.Northing * GeocalcList[i].Northing) + GeocalcList[i].Easting < TestPoint.Easting);
                    }
                }
            }
            return oddNodes; //true means inside.
        }

        public void DrawGeoFence()
        {
            ////draw the turn line oject
            if (geoFenceLine.Count < 1) return;
            GL.LineWidth(3);
            GL.Color3(0.96555f, 0.1232f, 0.50f);
            //GL.PointSize(4);

            GL.Begin(PrimitiveType.LineLoop);
            for (int h = 0; h < geoFenceLine.Count; h++) GL.Vertex3(geoFenceLine[h].Easting, geoFenceLine[h].Northing, 0);
            GL.End();
        }

        public void BuildGeoFenceLine(List<Vec3> bndLine, double geoFenceDistance, CancellationToken ct, out List<Vec3> geoFenceLine2, out List<Vec2> GeocalcList2)
        {
            geoFenceLine2 = new List<Vec3>();
            GeocalcList2 = new List<Vec2>();

            Vec3 point = new Vec3();

            for (int i = 0; i < bndLine.Count; i++)
            {
                if (ct.IsCancellationRequested) break;
                point.Northing = bndLine[i].Northing + Math.Sin(bndLine[i].Heading) * -geoFenceDistance;
                point.Easting = bndLine[i].Easting + Math.Cos(bndLine[i].Heading) * geoFenceDistance;
                geoFenceLine2.Add(point);
            }

            FixGeoFence(ref geoFenceLine2, Math.Abs(geoFenceDistance), bndLine, ct);

            PreCalcGeoFence(ref geoFenceLine2, ref GeocalcList2, ct);
        }

        public void FixGeoFence(ref List<Vec3> geoFenceLine2, double totalHeadWidth, in List<Vec3> curBnd, CancellationToken ct)
        {
            geoFenceLine2.PolygonArea(ct, true);

            List<List<Vec3>> tt = geoFenceLine2.ClipPolyLine(curBnd, true, totalHeadWidth, ct);
            if (tt.Count > 0) geoFenceLine2 = tt[0];

            double distance;
            for (int i = 0; i < geoFenceLine2.Count; i++)
            {
                if (ct.IsCancellationRequested) break;
                if (i > 0)
                {
                    int j = (i == geoFenceLine2.Count - 1) ? 0 : i + 1;
                    //make sure distance isn't too small between points on turnLine
                    distance = Glm.Distance(geoFenceLine2[i], geoFenceLine2[j]);
                    if (distance < 2)
                    {
                        geoFenceLine2.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        public void PreCalcGeoFence(ref List<Vec3> geoFenceLine2, ref List<Vec2> GeocalcList2, CancellationToken ct)
        {
            if (geoFenceLine2.Count > 3)
            {
                int j = geoFenceLine2.Count - 1;
                //clear the list, constant is easting, multiple is northing
                GeocalcList2.Clear();
                Vec2 constantMultiple = new Vec2(0, 0);

                for (int i = 0; i < geoFenceLine2.Count; j = i++)
                {
                    if (ct.IsCancellationRequested) break;

                    //check for divide by zero
                    if (Math.Abs(geoFenceLine2[i].Northing - geoFenceLine2[j].Northing) < double.Epsilon)
                    {
                        constantMultiple.Easting = geoFenceLine2[i].Easting;
                        constantMultiple.Northing = 0;
                        GeocalcList2.Add(constantMultiple);
                    }
                    else
                    {
                        //determine constant and multiple and add to list
                        constantMultiple.Easting = geoFenceLine2[i].Easting - ((geoFenceLine2[i].Northing * geoFenceLine2[j].Easting)
                                        / (geoFenceLine2[j].Northing - geoFenceLine2[i].Northing)) + ((geoFenceLine2[i].Northing * geoFenceLine2[i].Easting)
                                            / (geoFenceLine2[j].Northing - geoFenceLine2[i].Northing));
                        constantMultiple.Northing = (geoFenceLine2[j].Easting - geoFenceLine2[i].Easting) / (geoFenceLine2[j].Northing - geoFenceLine2[i].Northing);
                        GeocalcList2.Add(constantMultiple);
                    }
                }
            }
        }
    }
}