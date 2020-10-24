using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CMazeGrid
    {
        private readonly FormGPS mf;

        public bool[] mazeArr;
        public double mazeScale = 1;
        public int mazeRowYDim;
        public int mazeColXDim;
        //public List<vec3> mazePathList = new List<vec3>();

        public CMazeGrid(FormGPS _f)
        {
            mf = _f;
            mazeArr = new bool[0];
        }

        public void BuildMazeGridArray()
        {
            //mf.CalculateMinMax();
            double mazeY = (mf.maxFieldY - mf.minFieldY);
            double mazeX = (mf.maxFieldX - mf.minFieldX);

            mazeScale = (mazeY > mazeX) ? (int)(mazeY / 150) : (int)(mazeX / 150);

            if (mazeScale < 4) mazeScale = 4;

            mazeRowYDim = (int)Math.Ceiling(mazeY / mazeScale);
            mazeColXDim = (int)Math.Ceiling(mazeX / mazeScale);
            mazeArr = new bool[mazeRowYDim * mazeColXDim];

            //row is Y, col is X   int[Y,X] [i,j] [row,col]
            Vec3 Pos = new Vec3();

            for (int i = 0; i < mazeRowYDim; i++)
            {
                for (int j = 0; j < mazeColXDim; j++)
                {
                    Pos.Easting = (j * mazeScale) + (int)mf.minFieldX;
                    Pos.Northing = (i * mazeScale) + (int)mf.minFieldY;
                    if (!mf.gf.IsPointInsideGeoFences(Pos))
                    {
                        mazeArr[(i * mazeColXDim) + j] = true;
                    }
                    else
                    {
                        mazeArr[(i * mazeColXDim) + j] = false;
                    }
                }
            }
        }

        public List<Vec3> SearchForPath(Vec3 start, Vec3 stop)
        {
            CMazePath maze = new CMazePath(mazeRowYDim, mazeColXDim, mazeArr);

            List<Vec3> mazeList = maze.Search((int)((start.Northing - mf.minFieldY) / mf.mazeGrid.mazeScale),
                                                (int)((start.Easting - mf.minFieldX) / mf.mazeGrid.mazeScale),
                                          (int)((stop.Northing - mf.minFieldY) / mf.mazeGrid.mazeScale),
                                          (int)((stop.Easting - mf.minFieldX) / mf.mazeGrid.mazeScale));

            if (mazeList == null) return mazeList;

            //we find our way back, we want to go forward, so reverse the list
            mazeList.Reverse();

            int cnt = mazeList.Count;

            if (cnt < 3)
            {
                mazeList.Clear();
                return mazeList;
            }

            //the temp array
            Vec3[] arr2 = new Vec3[cnt];

            mazeList.CopyTo(arr2);
            mazeList.Clear();

            for (int h = 0; h < cnt; h++)
            {
                arr2[h].Easting = (arr2[h].Easting * mazeScale) + mf.minFieldX;
                arr2[h].Northing = (arr2[h].Northing * mazeScale) + mf.minFieldY;
                mazeList.Add(arr2[h]);
            }

            //fill in the gaps
            for (int i = 0; i < cnt; i++)
            {
                int j = i + 1;
                if (j == cnt)
                    j = i;
                double distance = Glm.Distance(mazeList[i], mazeList[j]);
                if (distance > 2)
                {
                    Vec3 pointB = new Vec3((mazeList[i].Northing + mazeList[j].Northing) / 2.0,
                        (mazeList[i].Easting + mazeList[j].Easting) / 2.0, 0);

                    mazeList.Insert(j, pointB);
                    cnt = mazeList.Count;
                    i--; //go back to original point again
                }
            }

            cnt = mazeList.Count;

            //the temp array
            Vec3[] arr = new Vec3[cnt];

            //how many samples
            int smPts = (int)mazeScale;

            //read the points before and after the setpoint
            for (int s = 0; s < smPts; s++)
            {
                arr[s].Easting = mazeList[s].Easting;
                arr[s].Northing = mazeList[s].Northing;
                arr[s].Heading = mazeList[s].Heading;
            }

            for (int s = cnt - smPts; s < cnt; s++)
            {
                arr[s].Easting = mazeList[s].Easting;
                arr[s].Northing = mazeList[s].Northing;
                arr[s].Heading = mazeList[s].Heading;
            }

            //average them - center weighted average
            for (int i = smPts; i < cnt - smPts; i++)
            {
                for (int j = -smPts; j < smPts; j++)
                {
                    arr[i].Easting += mazeList[j + i].Easting;
                    arr[i].Northing += mazeList[j + i].Northing;
                }
                arr[i].Easting /= (smPts * 2);
                arr[i].Northing /= (smPts * 2);
                arr[i].Heading = mazeList[i].Heading;
            }

            //clear the list and reload with calc headings - first and last droppped
            mazeList.Clear();

            for (int i = (int)mazeScale; i < cnt - mazeScale; i++)
            {
                Vec3 pt3 = arr[i];
                pt3.Heading = Math.Atan2(arr[i + 1].Easting - arr[i].Easting, arr[i + 1].Northing - arr[i].Northing);
                if (pt3.Heading < 0) pt3.Heading += Glm.twoPI;
                mazeList.Add(pt3);
            }

            return mazeList;
        }

        public void DrawArr()
        {
            GL.PointSize(2.0f);
            GL.Begin(PrimitiveType.Points);

            for (int h = 0; h < mazeArr.Length; h++)
            {
                if (mazeArr[h]) GL.Color3(0.0095f, 0.007520f, 0.97530f);
                else GL.Color3(0.95f, 0.7520f, 0.07530f);

                int Y = h / mazeColXDim; //Y
                int X = h - (h / mazeColXDim * mazeColXDim); //X
                GL.Vertex3((X * mazeScale) + (int)mf.minFieldX, (Y * mazeScale) + (int)mf.minFieldY, 0);
            }
            GL.End();
        }
    }
}