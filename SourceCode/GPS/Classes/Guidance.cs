
namespace AgOpenGPS
{
    public class Guidance
    {
        public double GuidanceWidth, GuidanceOverlap, GuidanceOffset, WidthMinusOverlap;

        public Guidance()
        {
            GuidanceWidth = Properties.Vehicle.Default.GuidanceWidth;
            GuidanceOverlap = Properties.Vehicle.Default.GuidanceOverlap;
            WidthMinusOverlap = GuidanceWidth - GuidanceOverlap;
            GuidanceOffset = Properties.Vehicle.Default.GuidanceOffset;
        }
    }
}
