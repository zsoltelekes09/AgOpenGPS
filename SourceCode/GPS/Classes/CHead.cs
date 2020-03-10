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

        public bool isToolInHeadland, isToolUp;

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
    }
}