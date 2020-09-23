
namespace AgOpenGPS
{
    public class CGuidance
    {
        public double GuidanceWidth, GuidanceOverlap, GuidanceOffset, WidthMinusOverlap;

        public CGuidance()
        {
            GuidanceWidth = Properties.Vehicle.Default.GuidanceWidth;
            GuidanceOverlap = Properties.Vehicle.Default.GuidanceOverlap;
            WidthMinusOverlap = GuidanceWidth - GuidanceOverlap;
            GuidanceOffset = Properties.Vehicle.Default.GuidanceOffset;
        }
    }
}
