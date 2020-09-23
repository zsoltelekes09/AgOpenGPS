//Please, if you use this, share the improvements

using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace AgOpenGPS
{
    public class CWorldGrid
    {
        private readonly FormGPS mf;

        //Z
        public double NorthingMin, NorthingMax;

        public double EastingMin, EastingMax;

        public CWorldGrid(FormGPS _f)
        {
            mf = _f;
        }

        public void DrawFieldSurface()
        {
            Color field = mf.isDay ? mf.fieldColorDay : mf.fieldColorNight;

            GL.Enable(EnableCap.Texture2D);
            GL.Color3(field.R, field.G, field.B);
            GL.BindTexture(TextureTarget.Texture2D, mf.texture[1]);
            GL.Begin(PrimitiveType.TriangleStrip);
            GL.TexCoord2(0, 0);
            GL.Vertex3(EastingMin, NorthingMax, 0.0);
            GL.TexCoord2(20, 0.0);
            GL.Vertex3(EastingMax, NorthingMax, 0.0);
            GL.TexCoord2(0.0, 20);
            GL.Vertex3(EastingMin, NorthingMin, 0.0);
            GL.TexCoord2(20, 20);
            GL.Vertex3(EastingMax, NorthingMin, 0.0);

            GL.End();
            GL.Disable(EnableCap.Texture2D);
        }

        public void DrawWorldGrid(double _gridZoom)
        {
            GL.Color3(0, 0, 0);
            GL.LineWidth(1);
            GL.Begin(PrimitiveType.Lines);
            for (double num = Math.Floor(EastingMin / _gridZoom) * _gridZoom; num < EastingMax; num += _gridZoom)
            {
                if (num < EastingMin) continue;

                GL.Vertex3(num, NorthingMax, 0.1);
                GL.Vertex3(num, NorthingMin, 0.1);
            }

            for (double num2 = Math.Floor(NorthingMin / _gridZoom) * _gridZoom; num2 < NorthingMax; num2 += _gridZoom)
            {
                if (num2 < NorthingMin) continue;

                GL.Vertex3(EastingMax, num2, 0.1);
                GL.Vertex3(EastingMin, num2, 0.1);
            }
            GL.End();
        }

        public void CheckWorldGrid(double northing, double easting)
        {
            double n = Math.Floor(northing / 200.0 + 0.5) * 200.0;
            double e = Math.Floor(easting / 200.0 + 0.5) * 200.0;

            NorthingMax = n + 20000.0;
            NorthingMin = n - 20000.0;
            EastingMax = e + 20000.0;
            EastingMin = e - 20000.0;
        }
    }
}