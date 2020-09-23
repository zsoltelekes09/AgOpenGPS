using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CHead
    {
        public bool BtnHeadLand = false;
        public bool isToolUp = true;

        /// <summary>
        /// array of turns
        /// </summary>
        public List<CHeadLines> headArr = new List<CHeadLines>();
    }

    public class CHeadLines
    {
        //list of coordinates of boundary line
        public List<List<Vec3>> HeadLine = new List<List<Vec3>>();
        
        public Vec3[] HeadVertices;
        public int[] HeadIndexer;

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
            int ptCount = HeadIndexer.Length;
            if (ptCount < 3) return;
            GL.Begin(PrimitiveType.Triangles);
            for (int h = 0; h < ptCount; h++)
            {
                GL.Vertex3(HeadVertices[HeadIndexer[h]].Easting, HeadVertices[HeadIndexer[h]].Northing, 0);
            }
            GL.End();
        }

        public void PreCalcHeadArea()
        {
            Tess _tess = new Tess();
            for (int i = 0; i < HeadLine.Count; i++)
            {
                _tess.AddContour(HeadLine[i], ContourOrientation.CounterClockwise);
            }
            _tess.Tessellate(WindingRule.Positive, ElementType.Polygons, 3);

            //Tess tess = new Tess(HeadLine);
            HeadIndexer = _tess.Elements;
            HeadVertices = _tess.Vertices;
        }
    }
}