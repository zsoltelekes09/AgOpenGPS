using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CHead
    {
        public bool isOn;
        public bool isToolUp;





        //!speed
        public bool isToolInHeadland, isToolOuterPointsInHeadland;
        private readonly FormGPS mf;
        public vec2 downL = new vec2(9000, 9000), downR = new vec2(9000, 9002);
        public bool isToolLeftIn = false;
        public bool isToolRightIn = false;
        public bool isLookRightIn = false;
        public bool isLookLeftIn = false;












        /// <summary>
        /// array of turns
        /// </summary>
        public List<CHeadLines> headArr = new List<CHeadLines>();

        //constructor
        public CHead(FormGPS _f)
        {
            mf = _f;
            isOn = false;
            isToolUp = true;
        }


        //!speed
        public void WhereAreToolLookOnPoints()
        {
            if (headArr[0].HeadLine.Count == 0)
            {
                return;
            }
            else
            {
                vec3 toolFix = mf.toolPos;
                double sinAB = Math.Sin(toolFix.heading);
                double cosAB = Math.Cos(toolFix.heading);

                double pos = mf.section[0].rpSectionWidth;
                double mOn = (mf.tool.lookAheadDistanceOnPixelsRight - mf.tool.lookAheadDistanceOnPixelsLeft) / mf.tool.rpWidth;
                double endHeight = (mf.tool.lookAheadDistanceOnPixelsLeft + (mOn * pos)) * 0.1;

                for (int j = 0; j < mf.tool.numOfSections; j++)
                {
                    if (j == 0)
                    {
                        downL.easting = mf.section[j].leftPoint.easting + (sinAB * mf.tool.lookAheadDistanceOnPixelsLeft * 0.1);
                        downL.northing = mf.section[j].leftPoint.northing + (cosAB * mf.tool.lookAheadDistanceOnPixelsLeft * 0.1);

                        downR.easting = mf.section[j].rightPoint.easting + (sinAB * endHeight);
                        downR.northing = mf.section[j].rightPoint.northing + (cosAB * endHeight);

                        isLookLeftIn = IsPointInsideHeadLine(downL);
                        isLookRightIn = IsPointInsideHeadLine(downR);

                        mf.section[j].isLookOnInHeadland = !isLookLeftIn && !isLookRightIn;
                        isLookLeftIn = isLookRightIn;
                    }
                    else
                    {
                        pos += mf.section[j].rpSectionWidth;
                        endHeight = (mf.tool.lookAheadDistanceOnPixelsLeft + (mOn * pos)) * 0.1;

                        downR.easting = mf.section[j].rightPoint.easting + (sinAB * endHeight);
                        downR.northing = mf.section[j].rightPoint.northing + (cosAB * endHeight);

                        isLookRightIn = IsPointInsideHeadLine(downR);
                        mf.section[j].isLookOnInHeadland = !isLookLeftIn && !isLookRightIn;
                        isLookLeftIn = isLookRightIn;
                    }
                }
            }
        }

        public void SetHydPosition()
        {
            if (mf.vehicle.isHydLiftOn && mf.pn.speed > 0.2)
            {
            }
        }


        public bool IsPointInsideHeadLine(vec2 pt)
        {
            //if inside outer boundary, then potentially add
            if (headArr.Count > 0 && headArr[0].IsPointInHeadArea(pt))
            {
                //for (int b = 1; b < mf.bnd.bndArr.Count; b++)
                //{
                //    if (mf.bnd.bndArr[b].isSet)
                //    {
                //        if (headArr[b].IsPointInHeadArea(pt))
                //        {
                //            //point is in an inner turn area but inside outer
                //            return false;
                //        }
                //    }
                //}
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}