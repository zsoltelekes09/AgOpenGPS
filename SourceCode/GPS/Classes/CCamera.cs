using OpenTK.Graphics.OpenGL;
using System;

namespace AgOpenGPS
{
    public class CCamera
    {
        public double camPitch;
        public double camSetDistance = -75;

        public double gridZoom;

        public double zoomValue = 15;

        public bool camFollowing;

        //private double camDelta = 0;

        public CCamera()
        {
            //get the pitch of camera from settings
            camPitch = Properties.Settings.Default.setDisplay_camPitch;
            camFollowing = true;
        }

        public void SetWorldCam(double _fixPosX, double _fixPosY, double _fixHeading)
        {
            //back the camera up
            GL.Translate(0.0, 0.0, camSetDistance * 0.5);
            //rotate the camera down to look at fix
            GL.Rotate(camPitch, 1.0, 0.0, 0.0);

            //following game style or N fixed cam
            if (camFollowing)
                GL.Rotate(_fixHeading, 0.0, 0.0, 1.0);
            GL.Translate(-_fixPosX, -_fixPosY, 0.0);
        }
    }
}