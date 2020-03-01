using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CHead
    {
        //copy of the mainform address
        private readonly FormGPS mf;

        public double singleSpaceHeadlandDistance;
        public bool isOn;
        public double leftToolDistance;
        public double rightToolDistance;

        //generated box for finding closest point
        public vec2 downL = new vec2(9000, 9000), downR = new vec2(9000, 9002);

        public bool isToolInHeadland, isToolUp, isToolOuterPointsInHeadland;

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
            singleSpaceHeadlandDistance = 18;
            isOn = false;
            headArr.Add(new CHeadLines());
            isToolUp = true;
        }

        public void SetHydPosition()
        {
            if (mf.vehicle.isHydLiftOn && mf.pn.speed > 0.2)
            {
                if (isToolInHeadland)
                {
                    mf.mc.machineData[mf.mc.mdHydLift] = 2;
                    isToolUp = true;
                }
                else
                {
                    mf.mc.machineData[mf.mc.mdHydLift] = 1;
                    isToolUp = false;
                }
            }
        }

        public void WhereAreToolCorners()
        {
            if (mf.bnd.LastBoundary > -1 && headArr.Count > mf.bnd.LastBoundary)
            {
                bool isLeftInWk, isRightInWk = false;

                for (int j = 0; j < mf.tool.numOfSections; j++)
                {
                    isLeftInWk = isRightInWk;//grab the right of previous section, its the left of this section
                    isRightInWk = false;
                    for (int i = ((mf.bnd.LastBoundary >= mf.bnd.bndArr.Count) || mf.bnd.LastBoundary == -1) ? 0 : mf.bnd.LastBoundary; i < mf.bnd.bndArr.Count; i++)
                    {
                        if (mf.bnd.bndArr[i].isOwnField)
                        {
                            if (j == 0) isLeftInWk |= mf.hd.headArr[mf.bnd.LastBoundary].IsPointInHeadArea(mf.section[j].leftPoint);
                            isRightInWk |= mf.hd.headArr[mf.bnd.LastBoundary].IsPointInHeadArea(mf.section[j].rightPoint);
                        }
                        if (mf.bnd.LastBoundary > -1) break;
                    }

                    if (isLeftInWk || isRightInWk)//no point in checking inside Boundaries when both outside!
                    {
                        for (int i = 0; i < mf.bnd.bndArr.Count; i++)
                        {
                            if (!mf.bnd.bndArr[i].isOwnField && (mf.bnd.LastBoundary == -1 || mf.bnd.bndArr[i].OuterField == mf.bnd.LastBoundary || mf.bnd.bndArr[i].OuterField == -1))
                            {
                                if (j == 0) isLeftInWk &= !mf.hd.headArr[mf.bnd.LastBoundary].IsPointInHeadArea(mf.section[j].leftPoint);
                                isRightInWk &= !mf.hd.headArr[mf.bnd.LastBoundary].IsPointInHeadArea(mf.section[j].rightPoint);
                            }
                        }
                    }

                    //save left side
                    if (j == 0) mf.tool.isLeftSideInHeadland = isLeftInWk;
                    //merge the two sides into in or out
                    mf.section[j].isInHeadlandArea = !(isLeftInWk || isRightInWk);

                }

                //save right side
                mf.tool.isRightSideInHeadland = isRightInWk;

                //is the tool in or out based on endpoints
                isToolOuterPointsInHeadland = !(mf.tool.isLeftSideInHeadland && mf.tool.isRightSideInHeadland);

            }
        }

        public void WhereAreToolLookOnPoints()
        {
            if (mf.bnd.LastBoundary > -1 && headArr.Count > mf.bnd.LastBoundary)
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

        public void DrawHeadLinesBack()
        {
            for (int i = 0; i < mf.bnd.bndArr.Count; i++)
            {
                if (mf.bnd.LastBoundary == -1 || i == mf.bnd.LastBoundary || (!mf.bnd.bndArr[i].isOwnField && mf.bnd.bndArr[i].OuterField == -1) || (!mf.bnd.bndArr[i].isOwnField && mf.bnd.bndArr[i].OuterField == mf.bnd.LastBoundary))
                {
                    if (headArr[i].hdLine.Count > 0) headArr[i].DrawHeadLineBackBuffer();
                }
            }
        }

        public void DrawHeadLines()
        {
            for (int i = 0; i < mf.bnd.bndArr.Count; i++)
            {
                if (mf.bnd.LastBoundary >= 0 || (i == mf.bnd.LastBoundary || (!mf.bnd.bndArr[i].isOwnField && mf.bnd.bndArr[i].OuterField == -1) || mf.bnd.bndArr[i].OuterField == mf.bnd.LastBoundary))
                {
                    if (headArr[i].hdLine.Count > 0) headArr[i].DrawHeadLine(mf.ABLine.lineWidth);
                }
            }
        }

        public bool IsPointInsideHeadLine(vec2 pt)
        {
            //if inside outer boundary, then potentially add
            if (mf.bnd.LastBoundary > -1 && headArr.Count > mf.bnd.LastBoundary && headArr[mf.bnd.LastBoundary].IsPointInHeadArea(pt))
            {
                for (int b = 0; b < mf.bnd.bndArr.Count; b++)
                {
                    if (!mf.bnd.bndArr[b].isOwnField)
                    {
                        if (headArr[b].IsPointInHeadArea(pt))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}