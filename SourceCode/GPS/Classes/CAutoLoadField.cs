using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CAutoLoadField
    {
        public double Northingmin, Northingmax, Eastingmin, Eastingmax;
        public List<Vec2> Boundary = new List<Vec2>();
        public string Dir = "";
    }
}
