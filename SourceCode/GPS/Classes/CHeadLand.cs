using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Threading;

namespace AgOpenGPS
{
    public partial class CBoundaryLines
    {
        //list of coordinates of boundary line
        public List<List<Vec3>> HeadLine = new List<List<Vec3>>();
        public List<List<int>> HeadLineIndexer = new List<List<int>>();

        //list of coordinates of boundary line
        public List<List<Vec3>> Template = new List<List<Vec3>>();

        public void DrawHeadLand(int linewidth)
        {
            if (HeadLine.Count == 0) return;

            GL.LineWidth(linewidth);
            GL.Color3(0.960f, 0.96232f, 0.30f);
            for (int i = 0; i < HeadLine.Count; i++)
            {
                GL.Begin(PrimitiveType.LineLoop);
                for (int j = 0; j < HeadLine[i].Count; j++)
                {
                    GL.Vertex3(HeadLine[i][j].Easting, HeadLine[i][j].Northing, 0);
                }
                GL.End();
            }
        }

        public void DrawHeadLandBackBuffer()
        {
            GL.Begin(PrimitiveType.Triangles);
            for (int i = 0; i < HeadLineIndexer.Count; i++)
            {
                for (int j = 0; j < HeadLineIndexer[i].Count; j++)
                {
                    if (HeadLine.Count > i && HeadLine[i].Count > HeadLineIndexer[i][j])
                        GL.Vertex3(HeadLine[i][HeadLineIndexer[i][j]].Easting, HeadLine[i][HeadLineIndexer[i][j]].Northing, 0);
                }
            }
            GL.End();
        }
    }
}
