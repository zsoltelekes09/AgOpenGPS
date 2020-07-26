//Please, if you use this, share the improvements

using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    /// <summary>
    /// Represents a three dimensional vector.
    /// </summary>
    public struct Vec3
    {
        public readonly static Vec3 Zero = new Vec3();
        public double easting, northing, heading;

        public Vec3(double easting, double northing, double heading)
        {
            this.easting = easting;
            this.northing = northing;
            this.heading = heading;
        }

        public Vec3(Vec3 v)
        {
            easting = v.easting;
            northing = v.northing;
            heading = v.heading;
        }

        public double HeadingXZ()
        {
            return Math.Atan2(easting, northing);
        }

        public void Normalize()
        {
            double length = GetLength();
            if (Math.Abs(length) < 0.0000000000001)
            {
                throw new DivideByZeroException("Trying to normalize a vector with length of zero.");
            }

            easting /= length;
            northing /= length;
            heading /= length;
        }

        //Returns the length of the vector
        public double GetLength()
        {
            return Math.Sqrt((easting * easting) + (heading * heading) + (northing * northing));
        }

        // Calculates the squared length of the vector.
        public double GetLengthSquared()
        {
            return (easting * easting) + (heading * heading) + (northing * northing);
        }

        public static Vec3 operator -(Vec3 lhs, Vec3 rhs)
        {
            return new Vec3(lhs.easting - rhs.easting, lhs.northing - rhs.northing, lhs.heading - rhs.heading);
        }

        public static void Neg(ref Vec3 v)
        {
            v.easting = -v.easting;
            v.northing = -v.northing;
            v.heading = -v.heading;
        }

        public static void Dot(ref Vec3 u, ref Vec3 v, out double dot)
        {
            dot = u.easting * v.easting + u.northing * v.northing + u.heading * v.heading;
        }

        public static int LongAxis(ref Vec3 v)
        {
            int i = 0;
            if (Math.Abs(v.northing) > Math.Abs(v.easting)) i = 1;
            if (Math.Abs(v.heading) > Math.Abs(i == 0 ? v.easting : v.northing)) i = 2;
            return i;
        }

        public double this[int index]
        {
            get
            {
                if (index == 0) return easting;
                if (index == 1) return northing;
                if (index == 2) return heading;
                throw new IndexOutOfRangeException();
            }
            set
            {
                if (index == 0) easting = value;
                else if (index == 1) northing = value;
                else if (index == 2) heading = value;
                else throw new IndexOutOfRangeException();
            }
        }
    }

    //

    /// <summary>
    /// easting, northing, heading, boundary#
    /// </summary>
    public struct Vec4
    {
        public double easting; //easting
        public double heading; //heading etc
        public double northing; //northing
        public int index;    //altitude

        public Vec4(double _easting, double _northing, double _heading, int _index)
        {
            easting = _easting;
            heading = _heading;
            northing = _northing;
            index = _index;
        }
    }

    public class ToolSettings
    {
        public double LookAheadOn = 1, LookAheadOff = 0.8, TurnOffDelay = 0, MappingOnDelay = 0.95, MappingOffDelay = 0.85;
        public double TrailingHitchLength = -6, TankTrailingHitchLength = -1.5, HitchLength = -0.5, ToolOffset = 0, SlowSpeedCutoff = 0;
        public bool BehindPivot = true, Trailing = true, TBT = false;
        public int MinApplied = 0;
        public List<double[]> Sections = new List<double[]> {};

    }

    public struct Vec2
    {
        public double easting; //easting
        public double northing; //northing

        public Vec2(double easting, double northing)
        {
            this.easting = easting;
            this.northing = northing;
        }

        public static Vec2 operator -(Vec2 lhs, Vec2 rhs)
        {
            return new Vec2(lhs.easting - rhs.easting, lhs.northing - rhs.northing);
        }

        public Vec2 Normalize()
        {
            double length = GetLength();
            if (Math.Abs(length) < 0.000000000001)
            {
                throw new DivideByZeroException("Trying to normalize a vector with length of zero.");
            }

            return new Vec2(easting /= length, northing /= length);
        }

        public double GetLength()
        {
            return Math.Sqrt((easting * easting) + (northing * northing));
        }

        public double GetLengthSquared()
        {
            return (easting * easting) + (northing * northing);
        }

        public static Vec2 operator *(Vec2 self, double s)
        {
            return new Vec2(self.easting * s, self.northing * s);
        }

        public static Vec2 operator +(Vec2 lhs, Vec2 rhs)
        {
            return new Vec2(lhs.easting + rhs.easting, lhs.northing + rhs.northing);
        }
    }

    //structure for contour guidance
    public struct CVec
    {
        public double x;
        public double z;
        public double h;
        public int strip;
        public int pt;

        //specialized contour vector
        public CVec(double x, double z, double h, int s, int p)
        {
            this.x = x;
            this.z = z;
            this.h = h;
            strip = s;
            pt = p;
        }
    }
}