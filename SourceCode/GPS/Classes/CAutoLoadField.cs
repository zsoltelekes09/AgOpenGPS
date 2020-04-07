using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgOpenGPS
{
    public class CAutoLoadField
    {
        public List<vec2> Boundary = new List<vec2>();
        public string Dir = "";
        public CAutoLoadField()
        {
            Dir = "";
            Boundary = new List<vec2>();
        }
    }
}
