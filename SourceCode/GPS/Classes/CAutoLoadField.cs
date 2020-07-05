using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CAutoLoadField
    {
        public List<Vec2> Boundary = new List<Vec2>();
        public string Dir = "";
        public CAutoLoadField()
        {
            Dir = "";
            Boundary = new List<Vec2>();
        }
    }
}
