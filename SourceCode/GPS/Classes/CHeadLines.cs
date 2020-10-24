using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CHead
    {
        public bool BtnHeadLand = false;
        public bool isToolUp = true;
        public List<CHeadLines> headArr = new List<CHeadLines>();
    }

    public class CHeadLines
    {
        //list of coordinates of boundary line
        public List<List<Vec3>> HeadLine = new List<List<Vec3>>();
        public List<List<int>> Indexer = new List<List<int>>();

        public void DrawHeadLine(int linewidth)
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

        public void DrawHeadBackBuffer()
        {
            GL.Begin(PrimitiveType.Triangles);
            for (int i = 0; i < Indexer.Count; i++)
            {
                for (int j = Indexer[i].Count-1; j > -1; j--)
                {
                    GL.Vertex3(HeadLine[i][Indexer[i][j]].Easting, HeadLine[i][Indexer[i][j]].Northing, 0);
                }
            }
            GL.End();
        }

        public void PreCalcHeadArea()
        {
            Indexer.Clear();
            for (int i = 0; i < HeadLine.Count; i++)
            {
                HeadLine[i].PolygonArea(true);
                Indexer.Add(HeadLine[i].TriangulatePolygon());
            }
        }
    }
}