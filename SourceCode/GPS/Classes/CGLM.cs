using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public static class TBoxCheckSettings
    {
        public static bool CheckValue(this TextBox Tbox, ref double value, double Minimum, double Maximum)
        {
            if (value < Minimum)
            {
                value = Minimum;
                Tbox.BackColor = System.Drawing.Color.OrangeRed;

                MessageBox.Show("Serious Settings Problem with - " + Tbox.Name
                    + " \n\rMinimum has been exceeded\n\rDouble check ALL your Settings and \n\rFix it and Resave Vehicle File",
                "Critical Settings Warning",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return true;
            }
            else if (value > Maximum)
            {
                value = Maximum;
                Tbox.BackColor = System.Drawing.Color.OrangeRed;
                MessageBox.Show("Serious Settings Problem with - " + Tbox.Name
                    + " \n\rMaximum has been exceeded\n\rDouble check ALL your Settings and \n\rFix it and Resave Vehicle File",
                "Critical Settings Warning",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return true;
            }

            //value is ok
            return false;
        }

        public static bool CheckValue(this TextBox Tbox, ref int value, int Minimum, int Maximum)
        {
            if (value < Minimum)
            {
                value = Minimum;
                Tbox.BackColor = System.Drawing.Color.OrangeRed;

                MessageBox.Show("Serious Settings Problem with - " + Tbox.Name
                    + " \n\rMinimum has been exceeded\n\rDouble check ALL your Settings and \n\rFix it and Resave Vehicle File",
                "Critical Settings Warning",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return true;
            }
            else if (value > Maximum)
            {
                value = Maximum;
                Tbox.BackColor = System.Drawing.Color.OrangeRed;
                MessageBox.Show("Serious Settings Problem with - " + Tbox.Name
                    + " \n\rMaximum has been exceeded\n\rDouble check ALL your Settings and \n\rFix it and Resave Vehicle File",
                "Critical Settings Warning",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return true;
            }

            //value is ok
            return false;
        }
    }

    public static class ListCalc
    {
        public static void LimitToRange(this double value, double Minimum, double Maximum)
        {
            if (value < Minimum) value = Minimum;
            else if (value > Maximum) value = Maximum;
        }

        public static void CalculateHeadings(this List<Vec3> Points, bool Loop)
        {
            int cnt = Points.Count;
            if (cnt > 3)
            {
                Vec3 point;
                for (int i = 0; i + 1 < cnt; i++)
                {
                    point = Points[i];
                    point.Heading = Math.Atan2(Points[i + 1].Easting - Points[i].Easting, Points[i + 1].Northing - Points[i].Northing);
                    if (point.Heading < 0) point.Heading += Glm.twoPI;
                    Points[i] = point;
                }
                if (Loop)//loop headings
                {
                    point = Points[cnt - 1];
                    point.Heading = Math.Atan2(Points[0].Easting - Points[cnt - 1].Easting, Points[0].Northing - Points[cnt - 1].Northing);
                    if (point.Heading < 0) point.Heading += Glm.twoPI;
                    Points[cnt - 1] = point;
                }
                else
                {
                    point = Points[cnt - 1];
                    point.Heading = Points[cnt - 2].Heading;
                    Points[cnt - 1] = point;
                }
            }
        }

        public static void CalculateRoundedCorner(this List<Vec3> Points, double Radius, bool Loop, double MaxAngle, bool tram = false, bool Experimental = false, bool Left = false, double halfWheelTrack = 0)
        {
            int A, C;
            double radius = Radius;

            for (int B = 0; B < Points.Count; B++)
            {
                if (!Loop && (B == 0 || B + 1 == Points.Count)) continue;
                A = (B == 0) ? Points.Count - 1 : B - 1;
                C = (B + 1 == Points.Count) ? 0 : B + 1;
                bool stop = false;
                double dx1, dy1, dx2, dy2, angle, tan, segment = 0, length1 = 0, length2 = 0;

                while (true)
                {
                    dx1 = Points[B].Northing - Points[A].Northing;
                    dy1 = Points[B].Easting - Points[A].Easting;
                    dx2 = Points[B].Northing - Points[C].Northing;
                    dy2 = Points[B].Easting - Points[C].Easting;

                    angle = (Math.Atan2(dy1, dx1) - Math.Atan2(dy2, dx2));

                    if (angle < 0) angle += Glm.twoPI;
                    if (angle > Glm.twoPI) angle -= Glm.twoPI;
                    angle /= 2;

                    if (Math.Abs(angle) > Glm.PIBy2 - MaxAngle && Math.Abs(angle) < Glm.PIBy2 + MaxAngle) //(170 / 2 = 85) > 85 degrees almost flat && < 95 degrees
                    {
                        if ((C - A > 2) || (Experimental && C - A > 0))
                        {
                            while (C - 1 > A)//Fix for some weird spikes
                            {
                                C = C == 0 ? Points.Count - 1 : C - 1;
                                Points.RemoveAt(C);
                            }
                        }
                        stop = true;
                        break;
                    }
                    if (tram)
                    {
                        if (Left)
                        {
                            if (Math.Abs(angle) > Glm.PIBy2) radius = Radius - halfWheelTrack;
                            else radius = Radius + halfWheelTrack;
                        }
                        else
                        {
                            if (Math.Abs(angle) < Glm.PIBy2) radius = Radius - halfWheelTrack;
                            else radius = Radius + halfWheelTrack;
                        }
                    }

                    tan = Math.Abs(Math.Tan(angle));

                    segment = radius / tan;

                    length1 = GetLength(dx1, dy1);
                    length2 = GetLength(dx2, dy2);
                    if (segment > length1)
                    {
                        A = (A == 0) ? Points.Count - 1 : A - 1;
                        if (A == C)
                        {
                            stop = true;
                            break;
                        }
                    }
                    if (segment > length2)
                    {
                        C = (C + 1 == Points.Count) ? 0 : C + 1;
                        if (C == A)
                        {
                            stop = true;
                            break;
                        }
                    }
                    else if (segment < length1) break;
                }
                if (stop) continue;

                // Points of intersection are calculated by the proportion between 
                // the coordinates of the vector, length of vector and the length of the segment.
                var p1Cross = GetProportionPoint(Points[B], segment, length1, dx1, dy1);
                var p2Cross = GetProportionPoint(Points[B], segment, length2, dx2, dy2);

                bool reverse = false;
                if (Math.Abs(angle) > Glm.PIBy2)
                {
                    Vec2 test = p1Cross;
                    p1Cross = p2Cross;
                    p2Cross = test;
                    reverse = true;
                }

                // Calculation of the coordinates of the circle 
                // center by the addition of angular vectors.
                double dx = Points[B].Northing * 2 - p1Cross.Northing - p2Cross.Northing;
                double dy = Points[B].Easting * 2 - p1Cross.Easting - p2Cross.Easting;


                if (dx1 == 0 && dy1 == 0 || dx2 == 0 && dy2 == 0 || dx == 0 && dy == 0) continue;

                Vec2 circlePoint;

                double L = GetLength(dx, dy);
                double d = GetLength(segment, radius);

                circlePoint = GetProportionPoint(Points[B], d, L, dx, dy);

                //StartAngle and EndAngle of arc
                var startAngle = Math.Atan2(p1Cross.Easting - circlePoint.Easting, p1Cross.Northing - circlePoint.Northing);
                var endAngle = Math.Atan2(p2Cross.Easting - circlePoint.Easting, p2Cross.Northing - circlePoint.Northing);

                if (startAngle < 0) startAngle += Glm.twoPI;
                if (endAngle < 0) endAngle += Glm.twoPI;

                bool Looping = (A > C);
                while (C - 1 > A || Looping)
                {
                    if (C == 0)
                    {
                        if (A == Points.Count - 1) break;
                        Looping = false;
                    }

                    C = C == 0 ? Points.Count - 1 : C - 1;

                    if (A > C) A--;

                    Points.RemoveAt(C);
                }

                B = A > B ? -1 : A;

                double sweepAngle;

                if (((Glm.twoPI - endAngle + startAngle) % Glm.twoPI) < ((Glm.twoPI - startAngle + endAngle) % Glm.twoPI))
                {
                    sweepAngle = (Glm.twoPI - endAngle + startAngle) % Glm.twoPI;
                }
                else
                {
                    sweepAngle = (Glm.twoPI - startAngle + endAngle) % Glm.twoPI;
                }

                int sign = Math.Sign(sweepAngle);

                if (reverse)
                {
                    sign = -sign;
                    startAngle = endAngle;
                }

                int pointsCount = (int)Math.Round(Math.Abs(sweepAngle / 0.0872665));

                double degreeFactor = sweepAngle / pointsCount;

                Vec3[] points = new Vec3[pointsCount];

                for (int j = 0; j < pointsCount; ++j)
                {
                    var pointX = circlePoint.Northing + Math.Cos(startAngle + sign * (j + 1) * degreeFactor) * radius;
                    var pointY = circlePoint.Easting + Math.Sin(startAngle + sign * (j + 1) * degreeFactor) * radius;
                    points[j] = new Vec3(pointX, pointY, 0);
                }
                Points.InsertRange(B + 1, points);

                B += points.Length;
            }

            Points.CalculateHeadings(Loop);
        }

        public static void FindCrossingPoints(this List<Vec4> Crossings, ref List<Vec2> Tram, double Northing, double Easting, double Northing2, double Easting2, int Index)
        {
            if (Tram.Count > 2)
            {
                int k = Tram.Count - 2;
                for (int j = -2; j < Tram.Count - 2; k = j)
                {
                    j += 2;
                    if (DoLinesIntersect(Northing, Easting, Northing2, Easting2, Tram[j].Northing, Tram[j].Easting, Tram[k].Northing - Tram[j].Northing, Tram[k].Easting - Tram[j].Easting, out double t))
                    {
                        Crossings.Add(new Vec4(Northing + (t * Northing2), Easting + (t * Easting2), t, Index));
                    }
                }
            }
        }

        public static void FindCrossingPoints(this List<Vec4> Crossings, ref List<Vec3> Bound, double Northing, double Easting, double Northing2, double Easting2, int Index)
        {
            if (Bound.Count > 2)
            {
                int k = Bound.Count - 2;
                for (int j = -2; j < Bound.Count - 2; k = j)
                {
                    j += 2;
                    if (DoLinesIntersect(Northing, Easting, Northing2, Easting2, Bound[j].Northing, Bound[j].Easting, Bound[k].Northing - Bound[j].Northing, Bound[k].Easting - Bound[j].Easting, out double t))
                    {
                        Crossings.Add(new Vec4(Northing + (t * Northing2), Easting + (t * Easting2), t, Index));
                    }
                }
            }
        }

        public static bool DoLinesIntersect(double Northing, double Easting, double Northing2, double Easting2, double Northing3, double Easting3, double Northing4, double Easting4, out double t)
        {
            t = -1;
            double s = (-Easting2 * (Northing - Northing3) + Northing2 * (Easting - Easting3)) / (-Northing4 * Easting2 + Northing2 * Easting4);
            if (s >= 0 && s <= 1)
            {
                t = (Northing4 * (Easting - Easting3) - Easting4 * (Northing - Northing3)) / (-Northing4 * Easting2 + Northing2 * Easting4);
                if (t >= 0 && t <= 1)
                {
                    return true;
                }
                else return false;
            }
            else return false;
        }

        public static double GetLength(double dx, double dy)
        {
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private static Vec2 GetProportionPoint(Vec3 point, double segment, double length, double dx, double dy)
        {
            double factor = segment / length;
            return new Vec2((point.Northing - dx * factor), (point.Easting - dy * factor));
        }
    }

    public static class Glm
    {

        //Regex file expression
        public static string fileRegex = "(^(PRN|AUX|NUL|CON|COM[1-9]|LPT[1-9]|(\\.+)$)(\\..*)?$)|(([\\x00-\\x1f\\\\?*:\";‌​|/<>])+)|([\\.]+)";

        //inches to meters
        public static double in2m = 0.02539999999997;

        //meters to inches
        public static double m2in = 39.37007874019995;

        //meters to feet
        public static double m2ft = 3.28084;

        //Hectare to Acres
        public static double ha2ac = 2.47105;

        //Acres to Hectare
        public static double ac2ha = 0.404686;

        //Meters to Acres
        public static double m2ac = 0.000247105;

        //Meters to Hectare
        public static double m2ha = 0.0001;

        // liters per hectare to us gal per acre
        public static double galAc2Lha = 9.35396;

        //us gal per acre to liters per hectare
        public static double LHa2galAc = 0.106907;

        //Liters to Gallons
        public static double L2Gal = 0.264172;

        //Gallons to Liters
        public static double Gal2L = 3.785412534258;

        //the pi's
        public static double twoPI = 6.28318530717958647692;

        public static double PIBy2 = 1.57079632679489661923;

        //Degrees Radians Conversions
        public static double ToDegrees(double radians)
        {
            return radians * 57.295779513082325225835265587528;
        }

        public static double ToRadians(double degrees)
        {
            return degrees * 0.01745329251994329576923690768489;
        }

        //Distance calcs of all kinds
        public static double Distance(double east1, double north1, double east2, double north2)
        {
            return Math.Sqrt(Math.Pow(east1 - east2, 2) + Math.Pow(north1 - north2, 2));
        }

        public static double Distance(Vec2 first, Vec2 second)
        {
            return Math.Sqrt(
                Math.Pow(first.Easting - second.Easting, 2)
                + Math.Pow(first.Northing - second.Northing, 2));
        }

        public static double Distance(Vec2 first, Vec3 second)
        {
            return Math.Sqrt(
                Math.Pow(first.Easting - second.Easting, 2)
                + Math.Pow(first.Northing - second.Northing, 2));
        }

        public static double Distance(Vec3 first, Vec2 second)
        {
            return Math.Sqrt(
                Math.Pow(first.Easting - second.Easting, 2)
                + Math.Pow(first.Northing - second.Northing, 2));
        }

        public static double Distance(Vec3 first, Vec3 second)
        {
            return Math.Sqrt(
                Math.Pow(first.Easting - second.Easting, 2)
                + Math.Pow(first.Northing - second.Northing, 2));
        }

        public static double Distance(Vec3 first, double east, double north)
        {
            return Math.Sqrt(
                Math.Pow(first.Easting - east, 2)
                + Math.Pow(first.Northing - north, 2));
        }

        //not normalized distance, no square root
        public static double DistanceSquared(double northing1, double easting1, double northing2, double easting2)
        {
            return Math.Pow(easting1 - easting2, 2) + Math.Pow(northing1 - northing2, 2);
        }
    }
}