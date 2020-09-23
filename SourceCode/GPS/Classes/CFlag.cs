using System;

namespace AgOpenGPS
{
    public class CFlag
    {
        //WGS84 Lat Long
        public double Latitude = 0;

        public double Longitude = 0;

        //UTM coordinates
        public double Northing = 0;

        public double Easting = 0, Heading = 0;

        //color of the flag - 0 is red, 1 is green, 2 is purple
        public int color = 0;

        public int ID = 0;

        public string notes = "";

        //constructor
        public CFlag(double _lati, double _longi, double _easting, double _northing, double _heading, int _color, int _ID, string _notes = "Notes")
        {
            Latitude = Math.Round(_lati, 7);
            Longitude = Math.Round(_longi, 7);
            Easting = Math.Round(_easting, 7);
            Northing = Math.Round(_northing, 7);
            Heading = Math.Round(_heading, 7);
            color = _color;
            ID = _ID;
            notes = _notes;
        }
    }
}