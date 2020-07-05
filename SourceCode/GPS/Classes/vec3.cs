//Please, if you use this, share the improvements

using System;

namespace AgOpenGPS
{
    /// <summary>
    /// Represents a three dimensional vector.
    /// </summary>
    public struct Vec3
    {
        public double easting;
        public double northing;
        public double heading;

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