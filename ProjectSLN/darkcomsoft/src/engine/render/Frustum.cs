﻿using OpenTK.Mathematics;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ProjectSLN.darkcomsoft.src.misc;

namespace ProjectSLN.darkcomsoft.src.engine.render
{
    //I DONT KNOW BUT THIS FUNCTIONS FRUSTUM NEED SOME OPTIMIZATION, I DON'T KNOW DO A LOOK
    public static class Frustum
    {
        private static double[] _clipMatrix = new double[16];
        private static double[,] _frustum = new double[6, 4];

        public const int A = 0;
        public const int B = 1;
        public const int C = 2;
        public const int D = 3;

        public enum ClippingPlane : int
        {
            Right = 0,
            Left = 1,
            Bottom = 2,
            Top = 3,
            Back = 4,
            Front = 5
        }

        private static void NormalizePlane(double[,] frustum, int side)
        {
            double magnitude = (double)Math.Sqrt((frustum[side, 0] * frustum[side, 0]) + (frustum[side, 1] * frustum[side, 1]) + (frustum[side, 2] * frustum[side, 2]));
            frustum[side, 0] /= magnitude;
            frustum[side, 1] /= magnitude;
            frustum[side, 2] /= magnitude;
            frustum[side, 3] /= magnitude;
        }

        public static bool PointVsFrustum(double x, double y, double z)
        {
            for (int i = 0; i < 6; i++)
            {
                if (_frustum[i, 0] * x + _frustum[i, 1] * y + _frustum[i, 2] * z + _frustum[i, 3] <= 0.0f)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool PointVsFrustum(Vector3d location)
        {
            for (int i = 0; i < 6; i++)
            {
                if (_frustum[i, 0] * location.X + _frustum[i, 1] * location.Y + _frustum[i, 2] * location.Z + _frustum[i, 3] <= 0.0f)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool SphereVsFrustum(double x, double y, double z, double radius)
        {
            for (int p = 0; p < 6; p++)
            {
                double d = _frustum[p, 0] * x + _frustum[p, 1] * y + _frustum[p, 2] * z + _frustum[p, 3];
                if (d <= -radius)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool SphereVsFrustum(Vector3d location, double radius)
        {
            for (int p = 0; p < 6; p++)
            {
                double d = _frustum[p, 0] * location.X + _frustum[p, 1] * location.Y + _frustum[p, 2] * location.Z + _frustum[p, 3];
                if (d <= -radius)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool VolumeVsFrustum(double x, double y, double z, double width, double height, double length)
        {
            for (int i = 0; i < 6; i++)
            {
                if (_frustum[i, A] * (x - width) + _frustum[i, B] * (y - height) + _frustum[i, C] * (z - length) + _frustum[i, D] > 0)
                    continue;
                if (_frustum[i, A] * (x + width) + _frustum[i, B] * (y - height) + _frustum[i, C] * (z - length) + _frustum[i, D] > 0)
                    continue;
                if (_frustum[i, A] * (x - width) + _frustum[i, B] * (y + height) + _frustum[i, C] * (z - length) + _frustum[i, D] > 0)
                    continue;
                if (_frustum[i, A] * (x + width) + _frustum[i, B] * (y + height) + _frustum[i, C] * (z - length) + _frustum[i, D] > 0)
                    continue;
                if (_frustum[i, A] * (x - width) + _frustum[i, B] * (y - height) + _frustum[i, C] * (z + length) + _frustum[i, D] > 0)
                    continue;
                if (_frustum[i, A] * (x + width) + _frustum[i, B] * (y - height) + _frustum[i, C] * (z + length) + _frustum[i, D] > 0)
                    continue;
                if (_frustum[i, A] * (x - width) + _frustum[i, B] * (y + height) + _frustum[i, C] * (z + length) + _frustum[i, D] > 0)
                    continue;
                if (_frustum[i, A] * (x + width) + _frustum[i, B] * (y + height) + _frustum[i, C] * (z + length) + _frustum[i, D] > 0)
                    continue;
                return false;
            }
            return true;
        }

        public static bool VolumeVsFrustum(Vector3d pos, Vector3d volumeSize)
        {
            for (int i = 0; i < 6; i++)
            {
                if (_frustum[i, A] * (pos.X - volumeSize.X) + _frustum[i, B] * (pos.Y - volumeSize.Y) + _frustum[i, C] * (pos.Z - volumeSize.Z) + _frustum[i, D] > 0)
                {
                    continue;
                }
                else if(_frustum[i, A] * (pos.X + volumeSize.X) + _frustum[i, B] * (pos.Y - volumeSize.Y) + _frustum[i, C] * (pos.Z - volumeSize.Z) + _frustum[i, D] > 0)
                {
                    continue;
                }
                else if(_frustum[i, A] * (pos.X - volumeSize.X) + _frustum[i, B] * (pos.Y + volumeSize.Y) + _frustum[i, C] * (pos.Z - volumeSize.Z) + _frustum[i, D] > 0)
                {
                    continue;
                }
                else if(_frustum[i, A] * (pos.X + volumeSize.X) + _frustum[i, B] * (pos.Y + volumeSize.Y) + _frustum[i, C] * (pos.Z - volumeSize.Z) + _frustum[i, D] > 0)
                {
                    continue;
                }
                else if(_frustum[i, A] * (pos.X - volumeSize.X) + _frustum[i, B] * (pos.Y - volumeSize.Y) + _frustum[i, C] * (pos.Z + volumeSize.Z) + _frustum[i, D] > 0)
                {
                    continue;
                }
                else if(_frustum[i, A] * (pos.X + volumeSize.X) + _frustum[i, B] * (pos.Y - volumeSize.Y) + _frustum[i, C] * (pos.Z + volumeSize.Z) + _frustum[i, D] > 0)
                {
                    continue;
                }
                else if (_frustum[i, A] * (pos.X - volumeSize.X) + _frustum[i, B] * (pos.Y + volumeSize.Y) + _frustum[i, C] * (pos.Z + volumeSize.Z) + _frustum[i, D] > 0)
                {
                    continue;
                }
                else if(_frustum[i, A] * (pos.X + volumeSize.X) + _frustum[i, B] * (pos.Y + volumeSize.Y) + _frustum[i, C] * (pos.Z + volumeSize.Z) + _frustum[i, D] > 0)
                {
                    continue;
                }
                return false;
            }
            return true;
        }

        public static bool VolumeVsFrustum(BoundingVolume volume)
        {
            for (int i = 0; i < 6; i++)
            {
                if (_frustum[i, A] * (volume.X - volume.Width) + _frustum[i, B] * (volume.Y - volume.Height) + _frustum[i, C] * (volume.Z - volume.Length) + _frustum[i, D] > 0)
                    continue;
                if (_frustum[i, A] * (volume.X + volume.Width) + _frustum[i, B] * (volume.Y - volume.Height) + _frustum[i, C] * (volume.Z - volume.Length) + _frustum[i, D] > 0)
                    continue;
                if (_frustum[i, A] * (volume.X - volume.Width) + _frustum[i, B] * (volume.Y + volume.Height) + _frustum[i, C] * (volume.Z - volume.Length) + _frustum[i, D] > 0)
                    continue;
                if (_frustum[i, A] * (volume.X + volume.Width) + _frustum[i, B] * (volume.Y + volume.Height) + _frustum[i, C] * (volume.Z - volume.Length) + _frustum[i, D] > 0)
                    continue;
                if (_frustum[i, A] * (volume.X - volume.Width) + _frustum[i, B] * (volume.Y - volume.Height) + _frustum[i, C] * (volume.Z + volume.Length) + _frustum[i, D] > 0)
                    continue;
                if (_frustum[i, A] * (volume.X + volume.Width) + _frustum[i, B] * (volume.Y - volume.Height) + _frustum[i, C] * (volume.Z + volume.Length) + _frustum[i, D] > 0)
                    continue;
                if (_frustum[i, A] * (volume.X - volume.Width) + _frustum[i, B] * (volume.Y + volume.Height) + _frustum[i, C] * (volume.Z + volume.Length) + _frustum[i, D] > 0)
                    continue;
                if (_frustum[i, A] * (volume.X + volume.Width) + _frustum[i, B] * (volume.Y + volume.Height) + _frustum[i, C] * (volume.Z + volume.Length) + _frustum[i, D] > 0)
                    continue;
                return false;
            }
            return true;
        }

        public static bool VolumeVsFrustum(Vector3d location, double width, double height, double length)
        {
            for (int i = 0; i < 6; i++)
            {
                if (_frustum[i, A] * (location.X - width) + _frustum[i, B] * (location.Y - height) + _frustum[i, C] * (location.Z - length) + _frustum[i, D] > 0)
                    continue;
                if (_frustum[i, A] * (location.X + width) + _frustum[i, B] * (location.Y - height) + _frustum[i, C] * (location.Z - length) + _frustum[i, D] > 0)
                    continue;
                if (_frustum[i, A] * (location.X - width) + _frustum[i, B] * (location.Y + height) + _frustum[i, C] * (location.Z - length) + _frustum[i, D] > 0)
                    continue;
                if (_frustum[i, A] * (location.X + width) + _frustum[i, B] * (location.Y + height) + _frustum[i, C] * (location.Z - length) + _frustum[i, D] > 0)
                    continue;
                if (_frustum[i, A] * (location.X - width) + _frustum[i, B] * (location.Y - height) + _frustum[i, C] * (location.Z + length) + _frustum[i, D] > 0)
                    continue;
                if (_frustum[i, A] * (location.X + width) + _frustum[i, B] * (location.Y - height) + _frustum[i, C] * (location.Z + length) + _frustum[i, D] > 0)
                    continue;
                if (_frustum[i, A] * (location.X - width) + _frustum[i, B] * (location.Y + height) + _frustum[i, C] * (location.Z + length) + _frustum[i, D] > 0)
                    continue;
                if (_frustum[i, A] * (location.X + width) + _frustum[i, B] * (location.Y + height) + _frustum[i, C] * (location.Z + length) + _frustum[i, D] > 0)
                    continue;
                return false;
            }
            return true;
        }

        public static bool CubeVsFrustum(double x, double y, double z, double size)
        {
            for (int i = 0; i < 6; i++)
            {
                if (_frustum[i, A] * (x - size) + _frustum[i, B] * (y - size) + _frustum[i, C] * (z - size) + _frustum[i, D] > 0)
                    continue;
                if (_frustum[i, A] * (x + size) + _frustum[i, B] * (y - size) + _frustum[i, C] * (z - size) + _frustum[i, D] > 0)
                    continue;
                if (_frustum[i, A] * (x - size) + _frustum[i, B] * (y + size) + _frustum[i, C] * (z - size) + _frustum[i, D] > 0)
                    continue;
                if (_frustum[i, A] * (x + size) + _frustum[i, B] * (y + size) + _frustum[i, C] * (z - size) + _frustum[i, D] > 0)
                    continue;
                if (_frustum[i, A] * (x - size) + _frustum[i, B] * (y - size) + _frustum[i, C] * (z + size) + _frustum[i, D] > 0)
                    continue;
                if (_frustum[i, A] * (x + size) + _frustum[i, B] * (y - size) + _frustum[i, C] * (z + size) + _frustum[i, D] > 0)
                    continue;
                if (_frustum[i, A] * (x - size) + _frustum[i, B] * (y + size) + _frustum[i, C] * (z + size) + _frustum[i, D] > 0)
                    continue;
                if (_frustum[i, A] * (x + size) + _frustum[i, B] * (y + size) + _frustum[i, C] * (z + size) + _frustum[i, D] > 0)
                    continue;
                return false;
            }
            return true;
        }

        public static void CalculateFrustum(Matrix4 projectionMatrix, Matrix4 modelViewMatrix)
        {
            _clipMatrix[0] = (modelViewMatrix.M11 * projectionMatrix.M11) + (modelViewMatrix.M12 * projectionMatrix.M21) + (modelViewMatrix.M13 * projectionMatrix.M31) + (modelViewMatrix.M14 * projectionMatrix.M41);
            _clipMatrix[1] = (modelViewMatrix.M11 * projectionMatrix.M12) + (modelViewMatrix.M12 * projectionMatrix.M22) + (modelViewMatrix.M13 * projectionMatrix.M32) + (modelViewMatrix.M14 * projectionMatrix.M42);
            _clipMatrix[2] = (modelViewMatrix.M11 * projectionMatrix.M13) + (modelViewMatrix.M12 * projectionMatrix.M23) + (modelViewMatrix.M13 * projectionMatrix.M33) + (modelViewMatrix.M14 * projectionMatrix.M43);
            _clipMatrix[3] = (modelViewMatrix.M11 * projectionMatrix.M14) + (modelViewMatrix.M12 * projectionMatrix.M24) + (modelViewMatrix.M13 * projectionMatrix.M34) + (modelViewMatrix.M14 * projectionMatrix.M44);

            _clipMatrix[4] = (modelViewMatrix.M21 * projectionMatrix.M11) + (modelViewMatrix.M22 * projectionMatrix.M21) + (modelViewMatrix.M23 * projectionMatrix.M31) + (modelViewMatrix.M24 * projectionMatrix.M41);
            _clipMatrix[5] = (modelViewMatrix.M21 * projectionMatrix.M12) + (modelViewMatrix.M22 * projectionMatrix.M22) + (modelViewMatrix.M23 * projectionMatrix.M32) + (modelViewMatrix.M24 * projectionMatrix.M42);
            _clipMatrix[6] = (modelViewMatrix.M21 * projectionMatrix.M13) + (modelViewMatrix.M22 * projectionMatrix.M23) + (modelViewMatrix.M23 * projectionMatrix.M33) + (modelViewMatrix.M24 * projectionMatrix.M43);
            _clipMatrix[7] = (modelViewMatrix.M21 * projectionMatrix.M14) + (modelViewMatrix.M22 * projectionMatrix.M24) + (modelViewMatrix.M23 * projectionMatrix.M34) + (modelViewMatrix.M24 * projectionMatrix.M44);

            _clipMatrix[8] = (modelViewMatrix.M31 * projectionMatrix.M11) + (modelViewMatrix.M32 * projectionMatrix.M21) + (modelViewMatrix.M33 * projectionMatrix.M31) + (modelViewMatrix.M34 * projectionMatrix.M41);
            _clipMatrix[9] = (modelViewMatrix.M31 * projectionMatrix.M12) + (modelViewMatrix.M32 * projectionMatrix.M22) + (modelViewMatrix.M33 * projectionMatrix.M32) + (modelViewMatrix.M34 * projectionMatrix.M42);
            _clipMatrix[10] = (modelViewMatrix.M31 * projectionMatrix.M13) + (modelViewMatrix.M32 * projectionMatrix.M23) + (modelViewMatrix.M33 * projectionMatrix.M33) + (modelViewMatrix.M34 * projectionMatrix.M43);
            _clipMatrix[11] = (modelViewMatrix.M31 * projectionMatrix.M14) + (modelViewMatrix.M32 * projectionMatrix.M24) + (modelViewMatrix.M33 * projectionMatrix.M34) + (modelViewMatrix.M34 * projectionMatrix.M44);

            _clipMatrix[12] = (modelViewMatrix.M41 * projectionMatrix.M11) + (modelViewMatrix.M42 * projectionMatrix.M21) + (modelViewMatrix.M43 * projectionMatrix.M31) + (modelViewMatrix.M44 * projectionMatrix.M41);
            _clipMatrix[13] = (modelViewMatrix.M41 * projectionMatrix.M12) + (modelViewMatrix.M42 * projectionMatrix.M22) + (modelViewMatrix.M43 * projectionMatrix.M32) + (modelViewMatrix.M44 * projectionMatrix.M42);
            _clipMatrix[14] = (modelViewMatrix.M41 * projectionMatrix.M13) + (modelViewMatrix.M42 * projectionMatrix.M23) + (modelViewMatrix.M43 * projectionMatrix.M33) + (modelViewMatrix.M44 * projectionMatrix.M43);
            _clipMatrix[15] = (modelViewMatrix.M41 * projectionMatrix.M14) + (modelViewMatrix.M42 * projectionMatrix.M24) + (modelViewMatrix.M43 * projectionMatrix.M34) + (modelViewMatrix.M44 * projectionMatrix.M44);

            _frustum[(int)ClippingPlane.Right, 0] = _clipMatrix[3] - _clipMatrix[0];
            _frustum[(int)ClippingPlane.Right, 1] = _clipMatrix[7] - _clipMatrix[4];
            _frustum[(int)ClippingPlane.Right, 2] = _clipMatrix[11] - _clipMatrix[8];
            _frustum[(int)ClippingPlane.Right, 3] = _clipMatrix[15] - _clipMatrix[12];
            NormalizePlane(_frustum, (int)ClippingPlane.Right);

            _frustum[(int)ClippingPlane.Left, 0] = _clipMatrix[3] + _clipMatrix[0];
            _frustum[(int)ClippingPlane.Left, 1] = _clipMatrix[7] + _clipMatrix[4];
            _frustum[(int)ClippingPlane.Left, 2] = _clipMatrix[11] + _clipMatrix[8];
            _frustum[(int)ClippingPlane.Left, 3] = _clipMatrix[15] + _clipMatrix[12];
            NormalizePlane(_frustum, (int)ClippingPlane.Left);

            _frustum[(int)ClippingPlane.Bottom, 0] = _clipMatrix[3] + _clipMatrix[1];
            _frustum[(int)ClippingPlane.Bottom, 1] = _clipMatrix[7] + _clipMatrix[5];
            _frustum[(int)ClippingPlane.Bottom, 2] = _clipMatrix[11] + _clipMatrix[9];
            _frustum[(int)ClippingPlane.Bottom, 3] = _clipMatrix[15] + _clipMatrix[13];
            NormalizePlane(_frustum, (int)ClippingPlane.Bottom);

            _frustum[(int)ClippingPlane.Top, 0] = _clipMatrix[3] - _clipMatrix[1];
            _frustum[(int)ClippingPlane.Top, 1] = _clipMatrix[7] - _clipMatrix[5];
            _frustum[(int)ClippingPlane.Top, 2] = _clipMatrix[11] - _clipMatrix[9];
            _frustum[(int)ClippingPlane.Top, 3] = _clipMatrix[15] - _clipMatrix[13];
            NormalizePlane(_frustum, (int)ClippingPlane.Top);

            _frustum[(int)ClippingPlane.Back, 0] = _clipMatrix[3] - _clipMatrix[2];
            _frustum[(int)ClippingPlane.Back, 1] = _clipMatrix[7] - _clipMatrix[6];
            _frustum[(int)ClippingPlane.Back, 2] = _clipMatrix[11] - _clipMatrix[10];
            _frustum[(int)ClippingPlane.Back, 3] = _clipMatrix[15] - _clipMatrix[14];
            NormalizePlane(_frustum, (int)ClippingPlane.Back);

            _frustum[(int)ClippingPlane.Front, 0] = _clipMatrix[3] + _clipMatrix[2];
            _frustum[(int)ClippingPlane.Front, 1] = _clipMatrix[7] + _clipMatrix[6];
            _frustum[(int)ClippingPlane.Front, 2] = _clipMatrix[11] + _clipMatrix[10];
            _frustum[(int)ClippingPlane.Front, 3] = _clipMatrix[15] + _clipMatrix[14];
            NormalizePlane(_frustum, (int)ClippingPlane.Front);
        }

        public static void CalculateFrustum(Matrix4 projectionMatrix)
        {
            _clipMatrix[0] = (projectionMatrix.M11) + (projectionMatrix.M21) + (projectionMatrix.M31) + (projectionMatrix.M41);
            _clipMatrix[1] = (projectionMatrix.M12) + (projectionMatrix.M22) + (projectionMatrix.M32) + (projectionMatrix.M42);
            _clipMatrix[2] = (projectionMatrix.M13) + (projectionMatrix.M23) + (projectionMatrix.M33) + (projectionMatrix.M43);
            _clipMatrix[3] = (projectionMatrix.M14) + (projectionMatrix.M24) + (projectionMatrix.M34) + (projectionMatrix.M44);

            _clipMatrix[4] = (projectionMatrix.M11) + (projectionMatrix.M21) + (projectionMatrix.M31) + (projectionMatrix.M41);
            _clipMatrix[5] = ( projectionMatrix.M12) + (projectionMatrix.M22) + ( projectionMatrix.M32) + (projectionMatrix.M42);
            _clipMatrix[6] = (projectionMatrix.M13) + (projectionMatrix.M23) + (projectionMatrix.M33) + (projectionMatrix.M43);
            _clipMatrix[7] = (projectionMatrix.M14) + (projectionMatrix.M24) + (projectionMatrix.M34) + (projectionMatrix.M44);

            _clipMatrix[8] = (projectionMatrix.M11) + (projectionMatrix.M21) + (projectionMatrix.M31) + ( projectionMatrix.M41);
            _clipMatrix[9] = (projectionMatrix.M12) + (projectionMatrix.M22) + (projectionMatrix.M32) + (projectionMatrix.M42);
            _clipMatrix[10] = (projectionMatrix.M13) + (projectionMatrix.M23) + (projectionMatrix.M33) + (projectionMatrix.M43);
            _clipMatrix[11] = (projectionMatrix.M14) + (projectionMatrix.M24) + (projectionMatrix.M34) + (projectionMatrix.M44);

            _clipMatrix[12] = (projectionMatrix.M11) + (projectionMatrix.M21) + (projectionMatrix.M31) + (projectionMatrix.M41);
            _clipMatrix[13] = (projectionMatrix.M12) + (projectionMatrix.M22) + (projectionMatrix.M32) + (projectionMatrix.M42);
            _clipMatrix[14] = (projectionMatrix.M13) + (projectionMatrix.M23) + (projectionMatrix.M33) + (projectionMatrix.M43);
            _clipMatrix[15] = (projectionMatrix.M14) + (projectionMatrix.M24) + (projectionMatrix.M34) + (projectionMatrix.M44);

            _frustum[(int)ClippingPlane.Right, 0] = _clipMatrix[3] - _clipMatrix[0];
            _frustum[(int)ClippingPlane.Right, 1] = _clipMatrix[7] - _clipMatrix[4];
            _frustum[(int)ClippingPlane.Right, 2] = _clipMatrix[11] - _clipMatrix[8];
            _frustum[(int)ClippingPlane.Right, 3] = _clipMatrix[15] - _clipMatrix[12];
            NormalizePlane(_frustum, (int)ClippingPlane.Right);

            _frustum[(int)ClippingPlane.Left, 0] = _clipMatrix[3] + _clipMatrix[0];
            _frustum[(int)ClippingPlane.Left, 1] = _clipMatrix[7] + _clipMatrix[4];
            _frustum[(int)ClippingPlane.Left, 2] = _clipMatrix[11] + _clipMatrix[8];
            _frustum[(int)ClippingPlane.Left, 3] = _clipMatrix[15] + _clipMatrix[12];
            NormalizePlane(_frustum, (int)ClippingPlane.Left);

            _frustum[(int)ClippingPlane.Bottom, 0] = _clipMatrix[3] + _clipMatrix[1];
            _frustum[(int)ClippingPlane.Bottom, 1] = _clipMatrix[7] + _clipMatrix[5];
            _frustum[(int)ClippingPlane.Bottom, 2] = _clipMatrix[11] + _clipMatrix[9];
            _frustum[(int)ClippingPlane.Bottom, 3] = _clipMatrix[15] + _clipMatrix[13];
            NormalizePlane(_frustum, (int)ClippingPlane.Bottom);

            _frustum[(int)ClippingPlane.Top, 0] = _clipMatrix[3] - _clipMatrix[1];
            _frustum[(int)ClippingPlane.Top, 1] = _clipMatrix[7] - _clipMatrix[5];
            _frustum[(int)ClippingPlane.Top, 2] = _clipMatrix[11] - _clipMatrix[9];
            _frustum[(int)ClippingPlane.Top, 3] = _clipMatrix[15] - _clipMatrix[13];
            NormalizePlane(_frustum, (int)ClippingPlane.Top);

            _frustum[(int)ClippingPlane.Back, 0] = _clipMatrix[3] - _clipMatrix[2];
            _frustum[(int)ClippingPlane.Back, 1] = _clipMatrix[7] - _clipMatrix[6];
            _frustum[(int)ClippingPlane.Back, 2] = _clipMatrix[11] - _clipMatrix[10];
            _frustum[(int)ClippingPlane.Back, 3] = _clipMatrix[15] - _clipMatrix[14];
            NormalizePlane(_frustum, (int)ClippingPlane.Back);

            _frustum[(int)ClippingPlane.Front, 0] = _clipMatrix[3] + _clipMatrix[2];
            _frustum[(int)ClippingPlane.Front, 1] = _clipMatrix[7] + _clipMatrix[6];
            _frustum[(int)ClippingPlane.Front, 2] = _clipMatrix[11] + _clipMatrix[10];
            _frustum[(int)ClippingPlane.Front, 3] = _clipMatrix[15] + _clipMatrix[14];
            NormalizePlane(_frustum, (int)ClippingPlane.Front);
        }
    }

    public class FrustumTeste
    {
        public static FrustumTeste FromViewProjection(Matrix4 m)
        {
            var p = new[]
            {
                new Plane(m.Column3 + m.Column0), //Left
                new Plane(m.Column3 - m.Column0), //Right

                new Plane(m.Column3 + m.Column1), //Bottom
                new Plane(m.Column3 - m.Column1), //Top

                new Plane(m.Column3 + m.Column2), //Near
                new Plane(m.Column3 - m.Column2) //Far
            };

            foreach (var plane in p)
                plane.Normalize();

            return new FrustumTeste(p);
        }

        public readonly Plane[] Planes;

        public FrustumTeste(Plane[] planes)
        {
            Planes = planes;
        }

        public bool SpehereIntersection(Vector3 position, float radius) => Planes.All(plane => !(Vector3.Dot(position, plane.Normal) + plane.D + radius <= 0));
    }

    public struct BoundingVolume
    {
        public double X;
        public double Y;
        public double Z;

        public int Width;
        public int Height;
        public int Length;
    }
}