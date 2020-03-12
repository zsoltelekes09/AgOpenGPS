using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CHead
    {
        public bool isOn;
        public bool isToolUp;

        /// <summary>
        /// array of turns
        /// </summary>
        public List<CHeadLines> headArr = new List<CHeadLines>();

        //constructor
        public CHead(FormGPS _f)
        {
            isOn = false;
            isToolUp = true;
        }
    }
}