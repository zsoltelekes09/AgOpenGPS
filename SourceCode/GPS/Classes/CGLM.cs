using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public static class DanielP
    {
        public static bool GetLineIntersection(Vec3 PointAA, Vec3 PointAB, Vec3 PointBA, Vec3 PointBB, out Vec3 Crossing, out double TimeA)
        {
            TimeA = -1;
            Crossing = new Vec3();
            double Denominator = (PointAB.Northing - PointAA.Northing) * (PointBB.Easting - PointBA.Easting) - (PointBB.Northing - PointBA.Northing) * (PointAB.Easting - PointAA.Easting);

            if (Denominator != 0.0)
            {
                TimeA = ((PointBB.Northing - PointBA.Northing) * (PointAA.Easting - PointBA.Easting) - (PointAA.Northing - PointBA.Northing) * (PointBB.Easting - PointBA.Easting)) / Denominator;

                if (TimeA > 0.0 && TimeA < 1.0)
                {
                    double TimeB = ((PointAB.Northing - PointAA.Northing) * (PointAA.Easting - PointBA.Easting) - (PointAA.Northing - PointBA.Northing) * (PointAB.Easting - PointAA.Easting)) / Denominator;
                    if (TimeB > 0.0 && TimeB < 1.0)
                    {
                        Crossing = PointAA + (PointAB - PointAA) * TimeA;
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }

        public static bool GetLineIntersection(Vec2 PointAA, Vec2 PointAB, Vec2 PointBA, Vec2 PointBB, out Vec2 Crossing, out double TimeA)
        {
            TimeA = -1;
            Crossing = new Vec2();
            double denominator = (PointAB.Northing - PointAA.Northing) * (PointBB.Easting - PointBA.Easting) - (PointBB.Northing - PointBA.Northing) * (PointAB.Easting - PointAA.Easting);

            if (denominator != 0.0)
            {
                TimeA = ((PointBB.Northing - PointBA.Northing) * (PointAA.Easting - PointBA.Easting) - (PointAA.Northing - PointBA.Northing) * (PointBB.Easting - PointBA.Easting)) / denominator;

                if (TimeA > 0.0 && TimeA < 1.0)
                {
                    double TimeB = ((PointAB.Northing - PointAA.Northing) * (PointAA.Easting - PointBA.Easting) - (PointAA.Northing - PointBA.Northing) * (PointAB.Easting - PointAA.Easting)) / denominator;
                    if (TimeB > 0.0 && TimeB < 1.0)
                    {
                        Crossing = PointAA + (PointAB - PointAA) * TimeA;
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }

        public static double PolygonArea(this List<Vec3> tt, bool ForceCW = false)
        {
            double Area = 0;
            int j = tt.Count - 1;
            for (int i = 0; i < tt.Count; j = i++)
                Area += (tt[i].Northing - tt[j].Northing) * (tt[i].Easting + tt[j].Easting);

            if (ForceCW && Area > 0)
            {
                tt.Reverse();//force Clockwise rotation
            }
            return Area;
        }

        public class VertexPoint
        {
            public int Idx;
            public Vec3 Coords;
            public VertexPoint Next;
            public VertexPoint Prev;
            public VertexPoint Crossing;
            //ClockWise or Crossing;
            public bool Data = false;

            public VertexPoint(Vec3 coords, bool intersection = false, int idx = 0)
            {
                Coords = coords;
                Data = intersection;
                Idx = idx;
            }
        }

        public static List<List<Vec3>> ClipPolyLine(this List<Vec3> Points, List<Vec3> clipPoints, bool BoundaryMode, double Offset)
        {
            List<List<Vec3>> FinalPolyLine = new List<List<Vec3>>();
            List<VertexPoint> PolyLine = PolyLineStructure(Points);
            List<VertexPoint> Crossings = new List<VertexPoint>();
            List<VertexPoint> Polygons = new List<VertexPoint>();

            VertexPoint CurrentVertex = PolyLine[0];
            VertexPoint StopVertex;
            if (BoundaryMode) StopVertex = CurrentVertex;
            else StopVertex = CurrentVertex.Prev;

            int IntersectionCount = 0;
            int safety = 0;
            bool start = true;
            while (true)
            {
                if (!start && CurrentVertex == StopVertex) break;
                start = false;

                VertexPoint SecondVertex = CurrentVertex.Next;

                int sectcnt = 0;
                int safety2 = 0;
                bool start2 = true;
                while (true)
                {
                    if (!start2 && SecondVertex == StopVertex) break;
                    start2 = false;

                    if (GetLineIntersection(CurrentVertex.Coords, CurrentVertex.Next.Coords, SecondVertex.Coords, SecondVertex.Next.Coords, out Vec3 intersectionPoint2D, out _))
                    {
                        sectcnt++;
                        IntersectionCount++;

                        VertexPoint AA = InsertCrossing(intersectionPoint2D, CurrentVertex);
                        VertexPoint BB = InsertCrossing(intersectionPoint2D, SecondVertex);

                        AA.Crossing = BB;
                        BB.Crossing = AA;
                    }
                    SecondVertex = SecondVertex.Next;

                    if (safety2++ > 100000) break;
                }
                for (int i = 0; i <= sectcnt; i++) CurrentVertex = CurrentVertex.Next;

                if (safety++ > 100000) break;
            }

            if (IntersectionCount > 0)
            {
                CurrentVertex = PolyLine[0];
                StopVertex = CurrentVertex;

                bool Searching = true;
                start = true;
                safety = 0;

                while (Crossings.Count > 0 || Searching)
                {
                    if (Crossings.Count > 0)
                    {
                        start = true;
                        CurrentVertex = Crossings[0];
                        StopVertex = CurrentVertex;
                        Crossings.RemoveAt(0);
                    }

                    while (true)
                    {
                        if (!start && CurrentVertex == StopVertex)
                        {
                            Polygons.Add(CurrentVertex);
                            Searching = false;
                            break;
                        }

                        start = false;
                        if (CurrentVertex.Data)
                        {
                            if (BoundaryMode) Crossings.Add(CurrentVertex.Next);

                            VertexPoint CC = CurrentVertex.Crossing.Next;
                            CurrentVertex.Crossing.Next = CurrentVertex.Next;
                            CurrentVertex.Next.Prev = CurrentVertex.Crossing;
                            CurrentVertex.Crossing.Data = false;
                            CurrentVertex.Crossing.Crossing = null;
                            CurrentVertex.Next = CC;
                            CurrentVertex.Next.Prev = CurrentVertex;
                            CurrentVertex.Data = false;
                            CurrentVertex.Crossing = null;
                        }
                        CurrentVertex = CurrentVertex.Next;
                        if (safety++ > 100000) break;
                    }
                }
            }
            else Polygons.Add(PolyLine[0]);

            if (BoundaryMode)
            {
                for (int i = 0; i < Polygons.Count; i++)
                {
                    start = true;
                    CurrentVertex = Polygons[i];
                    StopVertex = CurrentVertex;
                    double ccw = 0;

                    while (true)
                    {
                        if (!start && CurrentVertex == StopVertex)
                        {
                            if (ccw > 0)
                            {
                                Polygons.RemoveAt(i);
                                i--;
                            }
                            break;
                        }
                        start = false;

                        ccw += (CurrentVertex.Next.Coords.Northing - CurrentVertex.Coords.Northing) * (CurrentVertex.Next.Coords.Easting + CurrentVertex.Coords.Easting);

                        CurrentVertex = CurrentVertex.Next;
                    }
                }

                if (clipPoints != null)//hard on cpu!
                {
                    for (int i = 0; i < Polygons.Count; i++)
                    {
                        start = true;
                        CurrentVertex = Polygons[i];
                        StopVertex = CurrentVertex;

                        bool stop = false;
                        while (true)
                        {
                            if (stop || !start && CurrentVertex == StopVertex)
                            {
                                break;
                            }
                            start = false;

                            for (int j = 0; j < clipPoints.Count; j++)
                            {
                                Vec3 c = clipPoints[j];

                                double dist = ((c.Easting - CurrentVertex.Coords.Easting) * (c.Easting - CurrentVertex.Coords.Easting)) + ((c.Northing - CurrentVertex.Coords.Northing) * (c.Northing - CurrentVertex.Coords.Northing));

                                if (dist < Offset * Offset - 0.01)
                                {
                                    Polygons.RemoveAt(i);
                                    i--;
                                    stop = true;
                                    break;
                                }
                            }
                            CurrentVertex = CurrentVertex.Next;
                        }
                    }
                }
            }
            else if (clipPoints != null)
            {
                List<VertexPoint> ClipPolyLine = PolyLineStructure(clipPoints);

                for (int i = 0; i < Polygons.Count; i++)
                {
                    CurrentVertex = Polygons[i];

                    StopVertex = CurrentVertex.Prev;

                    bool isInside = PointInPolygon(clipPoints, CurrentVertex.Coords);

                    if (isInside) FinalPolyLine.Add(new List<Vec3>());

                    safety = 0;
                    start = true;
                    while (true)
                    {
                        if (!start && CurrentVertex == StopVertex) break;
                        start = false;

                        VertexPoint SecondVertex = ClipPolyLine[0];
                        VertexPoint StopVertex2 = ClipPolyLine[0];

                        int sectcnt = 0;
                        int safety2 = 0;
                        bool start2 = true;
                        while (true)
                        {
                            if (!start2 && SecondVertex == StopVertex2) break;
                            start2 = false;

                            if (GetLineIntersection(CurrentVertex.Coords, CurrentVertex.Next.Coords, SecondVertex.Coords, SecondVertex.Next.Coords, out Vec3 Crossing, out _))
                            {
                                sectcnt++;
                                if (isInside) FinalPolyLine[FinalPolyLine.Count - 1].Add(Crossing);
                                if (isInside = !isInside)
                                {
                                    FinalPolyLine.Add(new List<Vec3>());
                                    FinalPolyLine[FinalPolyLine.Count - 1].Add(Crossing);
                                }
                            }
                            SecondVertex = SecondVertex.Next;

                            if (safety2++ > 100000) break;
                        }

                        if (sectcnt == 0 && isInside)
                        {
                            FinalPolyLine[FinalPolyLine.Count - 1].Add(CurrentVertex.Coords);
                        }

                        CurrentVertex = CurrentVertex.Next;


                        if (safety++ > 100000) break;
                    }
                }
            }
            if (FinalPolyLine.Count == 0)
            {
                for (int i = 0; i < Polygons.Count; i++)
                {
                    FinalPolyLine.Add(new List<Vec3>());

                    start = true;
                    CurrentVertex = Polygons[i];

                    if (BoundaryMode) StopVertex = CurrentVertex;
                    else StopVertex = CurrentVertex.Prev;

                    while (true)
                    {
                        if (!start && CurrentVertex == StopVertex)
                            break;
                        start = false;

                        FinalPolyLine[i].Add(CurrentVertex.Coords);

                        CurrentVertex = CurrentVertex.Next;
                    }
                }
            }
            return FinalPolyLine;
        }

        public static List<VertexPoint> PolyLineStructure(List<Vec3> polyLine)
        {
            List<VertexPoint> PolyLine = new List<VertexPoint>();

            for (int i = 0; i < polyLine.Count; i++)
            {
                PolyLine.Add(new VertexPoint(polyLine[i], false, i));
            }

            for (int i = 0; i < PolyLine.Count; i++)
            {
                int Next = (i + 1).Clamp(PolyLine.Count);
                int Prev = (i - 1).Clamp(PolyLine.Count);

                PolyLine[i].Next = PolyLine[Next];
                PolyLine[i].Prev = PolyLine[Prev];
            }

            return PolyLine;
        }

        public static VertexPoint InsertCrossing(Vec3 intersectionPoint, VertexPoint currentVertex)
        {
            VertexPoint IntersectionCrossing = new VertexPoint(intersectionPoint, true)
            {
                Next = currentVertex.Next,
                Prev = currentVertex
            };
            currentVertex.Next.Prev = IntersectionCrossing;
            currentVertex.Next = IntersectionCrossing;
            return IntersectionCrossing;
        }

        public static bool PointInPolygon(List<Vec3> Polygon, Vec3 pointAA)
        {
            Vec3 PointAB = new Vec3(20000.0, 0.0, 0.0);

            int NumCrossings = 0;

            for (int i = 0; i < Polygon.Count; i++)
            {
                Vec3 PointBA = Polygon[i];
                Vec3 PointBB = Polygon[(i + 1).Clamp(Polygon.Count)];

                if (GetLineIntersection(pointAA, PointAB, PointBA, PointBB, out _, out _))
                    NumCrossings += 1;
            }
            return NumCrossings % 2 == 1;
        }

        public static double LimitToRange(this double Value, double Min, double Max)
        {
            if (Value < Min) Value = Min;
            else if (Value > Max) Value = Max;
            return Value;
        }

        public static int Clamp(this int Idx, int Size)
        {
            return (Size + Idx) % Size;
        }

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

        public static void CalculateHeading(this List<Vec3> Points, bool Loop)
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
                point = Points[cnt - 1];
                if (Loop)//loop headings
                {
                    point.Heading = Math.Atan2(Points[0].Easting - Points[cnt - 1].Easting, Points[0].Northing - Points[cnt - 1].Northing);
                    if (point.Heading < 0) point.Heading += Glm.twoPI;
                }
                else
                {
                    point.Heading = Points[cnt - 2].Heading;
                }
                Points[cnt - 1] = point;
            }
        }

        public static void CalculateRoundedCorner(this List<Vec3> Points, double Radius, bool Loop, double MaxAngle, double Offset, bool tram = false, bool Experimental = false, bool Left = false, double halfWheelTrack = 0)
        {
            MaxAngle = Math.Min(Math.Sinh(2.0 / Offset), MaxAngle);

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

                    if (Math.Abs(angle) > Glm.PIBy2 - MaxAngle && Math.Abs(angle) < Glm.PIBy2 + MaxAngle)
                    {
                        if ((C - A > 2) || (Experimental && C - A > 0))
                        {
                            while (C - 1 > A)
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
                        if ((!Loop && A == Points.Count - 1) || A == C)
                        {
                            stop = true;
                            break;
                        }
                    }
                    if (segment > length2)
                    {
                        C = (C + 1 == Points.Count) ? 0 : C + 1;
                        if ((!Loop && C == 0) || C == A)
                        {
                            stop = true;
                            break;
                        }
                    }
                    else if (segment < length1) break;
                }
                if (stop) continue;

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

                double dx = Points[B].Northing * 2 - p1Cross.Northing - p2Cross.Northing;
                double dy = Points[B].Easting * 2 - p1Cross.Easting - p2Cross.Easting;


                if (dx1 == 0 && dy1 == 0 || dx2 == 0 && dy2 == 0 || dx == 0 && dy == 0) continue;

                Vec2 circlePoint;

                double L = GetLength(dx, dy);
                double d = GetLength(segment, radius);

                circlePoint = GetProportionPoint(Points[B], d, L, dx, dy);

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
                    sweepAngle = (Glm.twoPI - endAngle + startAngle) % Glm.twoPI;
                else
                    sweepAngle = (Glm.twoPI - startAngle + endAngle) % Glm.twoPI;

                int sign = Math.Sign(sweepAngle);

                if (reverse)
                {
                    sign = -sign;
                    startAngle = endAngle;
                }

                int pointsCount = (int)Math.Round(Math.Abs(sweepAngle / MaxAngle));

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

            Points.CalculateHeading(Loop);
        }

        public static void FindCrossingPoints(this List<Vec4> Crossings, ref List<Vec2> Tram, Vec2 Point1, Vec2 Point2, int Index)
        {
            if (Tram.Count > 2)
            {
                int k = Tram.Count - 2;
                for (int j = -2; j < Tram.Count - 2; k = j)
                {
                    j += 2;
                    if (GetLineIntersection(Point1, Point2, Tram[j], Tram[k], out Vec2 Crossing, out double Time))
                        Crossings.Add(new Vec4(Crossing.Northing, Crossing.Easting, Time, Index));
                }
            }
        }

        public static void FindCrossingPoints(this List<Vec4> Crossings, ref List<Vec3> Bound, Vec3 Point1, Vec3 Point2, int Index)
        {
            if (Bound.Count > 2)
            {
                int k = Bound.Count - 2;
                for (int j = -2; j < Bound.Count - 2; k = j)
                {
                    j += 2;
                    if (GetLineIntersection(Point1, Point2, Bound[j], Bound[k], out Vec3 Crossing, out double Time))
                        Crossings.Add(new Vec4(Crossing.Northing, Crossing.Easting, Time, Index));
                }
            }
        }

        public static double GetLength(double dx, double dy)
        {
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static Vec2 GetProportionPoint(Vec3 point, double segment, double length, double dx, double dy)
        {
            double factor = segment / length;
            return new Vec2((point.Northing - dx * factor), (point.Easting - dy * factor));
        }

        public static List<int> TriangulatePolygon(this List<Vec3> Points)
        {
            List<int> Indexer = new List<int>();

            if (Points.Count < 3) return Indexer;

            List<VertexPoint> Vertices = PolyLineStructure(Points);

            for (int i = 0; i < Vertices.Count; i++)
            {
                CheckClockwise(Vertices[i]);
            }

            for (int i = 0; i < Vertices.Count; i++)
            {
                if (!Vertices[i].Data) IsEar(Vertices[i], Vertices);
            }

            int Idx = 0;

            while (true)
            {
                if (Vertices.Count < 3) break;

                VertexPoint CurrentVertex = Vertices[Idx];
                if (CurrentVertex.Crossing != null)
                {
                    Indexer.Add(!CurrentVertex.Data ? CurrentVertex.Prev.Idx : CurrentVertex.Idx);
                    Indexer.Add(!CurrentVertex.Data ? CurrentVertex.Idx : CurrentVertex.Prev.Idx);
                    Indexer.Add(CurrentVertex.Next.Idx);

                    CurrentVertex.Prev.Next = CurrentVertex.Next;
                    CurrentVertex.Next.Prev = CurrentVertex.Prev;

                    CheckClockwise(CurrentVertex.Prev);
                    CheckClockwise(CurrentVertex.Next);

                    Vertices.Remove(CurrentVertex);
                    CurrentVertex.Prev.Crossing = null;
                    CurrentVertex.Next.Crossing = null;
                    if (!CurrentVertex.Prev.Data) IsEar(CurrentVertex.Prev, Vertices);
                    if (!CurrentVertex.Next.Data) IsEar(CurrentVertex.Next, Vertices);
                }
                Idx++;
                Idx %= Vertices.Count;
            }
            return Indexer;
        }

        public static void CheckClockwise(VertexPoint v)
        {
            if (IsTriangleOrientedClockwise(v.Prev.Coords, v.Coords, v.Next.Coords))
                v.Data = true;
            else
                v.Data = false;
        }

        public static bool IsTriangleOrientedClockwise(Vec3 p1, Vec3 p2, Vec3 p3)
        {
            double determinant = p1.Northing * p2.Easting + p3.Northing * p1.Easting + p2.Northing * p3.Easting - p1.Northing * p3.Easting - p3.Northing * p2.Easting - p2.Northing * p1.Easting;

            if (determinant > 0.0)
                return false;
            else
                return true;
        }

        public static void IsEar(VertexPoint Point, List<VertexPoint> vertices)
        {
            bool hasPointInside = false;

            for (int i = 0; i < vertices.Count; i++)
            {
                if (vertices[i].Data)
                {
                    if (IsPointInTriangle(Point.Prev.Coords, Point.Coords, Point.Next.Coords, vertices[i].Coords))
                    {
                        hasPointInside = true;
                        break;
                    }
                }
            }

            if (!hasPointInside)
            {
                Point.Crossing = Point;
            }
        }

        public static bool IsPointInTriangle(Vec3 p1, Vec3 p2, Vec3 p3, Vec3 p)
        {
            double Denominator = ((p2.Easting - p3.Easting) * (p1.Northing - p3.Northing) + (p3.Northing - p2.Northing) * (p1.Easting - p3.Easting));
            double a = ((p2.Easting - p3.Easting) * (p.Northing - p3.Northing) + (p3.Northing - p2.Northing) * (p.Easting - p3.Easting)) / Denominator;

            if (a > 0.0 && a < 1.0)
            {
                double b = ((p3.Easting - p1.Easting) * (p.Northing - p3.Northing) + (p1.Northing - p3.Northing) * (p.Easting - p3.Easting)) / Denominator;
                if (b > 0.0 && b < 1.0)
                {
                    double c = 1 - a - b;
                    if (c > 0.0 && c < 1.0)
                        return true;
                }
            }
            return false;
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