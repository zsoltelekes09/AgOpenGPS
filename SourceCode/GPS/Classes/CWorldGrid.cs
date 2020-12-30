//Please, if you use this, share the improvements

using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace AgOpenGPS
{
    public class CWorldGrid
    {
        private readonly FormGPS mf;

        public double NorthingMin, NorthingMax;

        public double EastingMin, EastingMax;

        public double GridSize = 20000;
        public double Count = 100;
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
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 0);
            GL.Vertex3(EastingMin, NorthingMin, 0.0);
            GL.TexCoord2(Count, 0.0);
            GL.Vertex3(EastingMax, NorthingMin, 0.0);
            GL.TexCoord2(Count, Count);
            GL.Vertex3(EastingMax, NorthingMax, 0.0);
            GL.TexCoord2(0.0, Count);//topright
            GL.Vertex3(EastingMin, NorthingMax, 0.0);

            GL.End();



            /*
            
            string fileAndDirectory = mf.baseDirectory + "VR.PNG";

            if (mf.texture[12] != 0)
            {
                GL.Enable(EnableCap.Texture2D);
                GL.Color3(0.96f, .96f, 0.96f);
                GL.BindTexture(TextureTarget.Texture2D, mf.texture[12]);
                GL.Begin(PrimitiveType.TriangleStrip);
                GL.TexCoord2(0, 0);
                GL.Vertex3(mf.minFieldX, mf.maxFieldY, 0.0);
                GL.TexCoord2(1.0, 0.0);
                GL.Vertex3(mf.maxFieldX, mf.maxFieldY, 0.0);
                GL.TexCoord2(0.0, 1.0);
                GL.Vertex3(mf.minFieldX, mf.minFieldY, 0.0);
                GL.TexCoord2(1.0, 1.0);
                GL.Vertex3(mf.maxFieldX, mf.minFieldY, 0.0);
                GL.End();
            }
            else if (File.Exists(fileAndDirectory))
            {
                using (Bitmap bitmap2 = new Bitmap(fileAndDirectory))
                {
                    GL.GenTextures(1, out mf.texture[12]);
                    GL.BindTexture(TextureTarget.Texture2D, mf.texture[12]);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, 9729);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
                    BitmapData bitmapData2 = bitmap2.LockBits(new Rectangle(0, 0, bitmap2.Width, bitmap2.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData2.Width, bitmapData2.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData2.Scan0);
                    bitmap2.UnlockBits(bitmapData2);
                }
            }
            */

            GL.Disable(EnableCap.Texture2D);
        }

        public void DrawWorldGrid(double _gridZoom)
        {
            GL.Color3(0, 0, 0);
            GL.LineWidth(1);
            GL.Begin(PrimitiveType.Lines);
            for (double num = Math.Round(EastingMin / _gridZoom, MidpointRounding.AwayFromZero) * _gridZoom; num < EastingMax; num += _gridZoom)
            {
                if (num < EastingMin) continue;

                GL.Vertex3(num, NorthingMax, 0.1);
                GL.Vertex3(num, NorthingMin, 0.1);
            }

            for (double num2 = Math.Round(NorthingMin / _gridZoom, MidpointRounding.AwayFromZero) * _gridZoom; num2 < NorthingMax; num2 += _gridZoom)
            {
                if (num2 < NorthingMin) continue;

                GL.Vertex3(EastingMax, num2, 0.1);
                GL.Vertex3(EastingMin, num2, 0.1);
            }
            GL.End();
        }

        public void CheckWorldGrid(double northing, double easting)
        {
            double n = Math.Round(northing / (GridSize / Count * 2) , MidpointRounding.AwayFromZero) * (GridSize / Count * 2);
            double e = Math.Round(easting / (GridSize / Count * 2), MidpointRounding.AwayFromZero) * (GridSize / Count * 2);

            NorthingMax = n + GridSize;
            NorthingMin = n - GridSize;
            EastingMax = e + GridSize;
            EastingMin = e - GridSize;
        }
    }
}