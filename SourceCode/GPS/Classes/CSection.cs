//Please, if you use this, share the improvements
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AgOpenGPS
{
    //each section is composed of a triangle list
    //the triangle list makes up the individual triangles that make the block or patch of applied (green spot)

    public class CSection
    {
        //copy of the mainform address
        private readonly FormGPS mf;

        //list of patch data individual triangles
        public List<Vec3> triangleList = new List<Vec3>();

        //is this section on or off
        public bool IsSectionOn = false;
        public bool IsSectionRequiredOn = false;
        public bool SectionOnRequest = false;
        public int SectionOverlapTimer = 0;
        public Button SectionButton = new Button();

        //mapping
        public bool IsMappingOn = false;
        public bool MappingOnRequest = false;
        public int MappingOnTimer = 0;
        public int MappingOffTimer = 0;

        public double speedPixels = 0;


        //the left side is always negative, right side is positive
        //so a section on the left side only would be -8, -4
        //in the center -4,4  on the right side only 4,8
        //reads from left to right
        //   ------========---------||----------========---------
        //        -8      -4      -1  1         4      8
        // in (meters)

        public double positionLeft = -4;
        public double positionRight = 4;
        public double positionForward = 0;


        //used by readpixel to determine color in pixel array
        public int rpSectionWidth = 0;
        public int rpSectionPosition = 0;

        //points in world space that start and end of section are in
        public Vec2 leftPoint;
        public Vec2 rightPoint;

        //whether or not this section is in boundary, headland
        public int numTriangles = 0;

        //used to determine state of Manual section button - Off Auto On
        public FormGPS.btnStates BtnSectionState = FormGPS.btnStates.Off;

        //simple constructor, position is set in GPSWinForm_Load in FormGPS when creating new object
        public CSection(FormGPS _f)
        {
            //constructor
            mf = _f;
        }

        public void TurnMappingOn()
        {
            numTriangles = 0;

            //do not tally square meters on inital point, that would be silly
            if (!IsMappingOn)
            {
               //set the section bool to on
                IsMappingOn = true;

                //starting a new patch chunk so create a new triangle list
                triangleList = new List<Vec3>();
                Color t1 = mf.sectionColorDay;
                Vec3 colur = new Vec3(t1.R, t1.G, t1.B);
                triangleList.Add(colur);

                //left side of triangle
                Vec3 point = new Vec3(leftPoint.Northing, leftPoint.Easting, 0);
                triangleList.Add(point);

                //Right side of triangle
                point = new Vec3(rightPoint.Northing, rightPoint.Easting, 0);
                triangleList.Add(point);
            }
        }

        public void TurnMappingOff()
        {
            AddMappingPoint();

            IsMappingOn = false;
            numTriangles = 0;

            if (triangleList.Count > 4)
            {
                //save the triangle list in a patch list to add to saving file
                mf.PatchSaveList.Add(triangleList);
                mf.PatchDrawList.Add(triangleList);
            }
            else
            {
                triangleList.Clear();
            }
        }

        //every time a new fix, a new patch point from last point to this point
        //only need prev point on the first points of triangle strip that makes a box (2 triangles)

        public void AddMappingPoint()
        {
            //add two triangles for next step.
            //left side
            Vec3 point = new Vec3(leftPoint.Northing, leftPoint.Easting, 0);

            //add the point to List
            triangleList.Add(point);

            //Right side
            Vec3 point2 = new Vec3(rightPoint.Northing, rightPoint.Easting, 0);

            //add the point to the list
            triangleList.Add(point2);

            //count the triangle pairs
            numTriangles++;

            //quick count
            int c = triangleList.Count - 1;

            //when closing a job the triangle patches all are emptied but the section delay keeps going.
            //Prevented by quick check. 4 points plus colour
            if (c >= 5)
            {
                //calculate area of these 2 new triangles - AbsoluteValue of (Ax(By-Cy) + Bx(Cy-Ay) + Cx(Ay-By)/2)
                {
                    double temp = (triangleList[c].Easting * (triangleList[c - 1].Northing - triangleList[c - 2].Northing))
                              + (triangleList[c - 1].Easting * (triangleList[c - 2].Northing - triangleList[c].Northing))
                                  + (triangleList[c - 2].Easting * (triangleList[c].Northing - triangleList[c - 1].Northing));

                    temp = Math.Abs(temp / 2.0);
                    mf.fd.workedAreaTotal += temp;
                    mf.fd.workedAreaTotalUser += temp;

                    //temp = 0;
                    temp = (triangleList[c - 1].Easting * (triangleList[c - 2].Northing - triangleList[c - 3].Northing))
                              + (triangleList[c - 2].Easting * (triangleList[c - 3].Northing - triangleList[c - 1].Northing))
                                  + (triangleList[c - 3].Easting * (triangleList[c - 1].Northing - triangleList[c - 2].Northing));

                    temp = Math.Abs(temp / 2.0);
                    mf.fd.workedAreaTotal += temp;
                    mf.fd.workedAreaTotalUser += temp;
                }
            }

            if (numTriangles > 36)
            {
                numTriangles = 0;

                //save the cutoff patch to be saved later
                mf.PatchSaveList.Add(triangleList);

                mf.PatchDrawList.Add(triangleList);

                triangleList = new List<Vec3>();

                //Add Patch colour
                Color t1 = mf.sectionColorDay;
                Vec3 colur = new Vec3(t1.R, t1.G, t1.B);
                triangleList.Add(colur);

                //add the points to List, yes its more points, but breaks up patches for culling
                triangleList.Add(point);
                triangleList.Add(point2);
            }
        }
    }
}