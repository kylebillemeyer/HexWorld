using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using HexWorld.Models;
using HexWord.Graph;

namespace HexWorld.Graph
{
    public struct CubeIndex
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public CubeIndex(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public CubeIndex(float x, float y, float z)
        {
            var rx = Math.Round(x);
            var ry = Math.Round(y);
            var rz = Math.Round(z);

            var x_diff = Math.Abs(rx - x);
            var y_diff = Math.Abs(ry - y);
            var z_diff = Math.Abs(rz - z);

            if (x_diff > y_diff && x_diff > z_diff)
                rx = -ry - rz;
            else if (y_diff > z_diff)
                ry = -rx - rz;
            else
                rz = -rx - ry;

            X = (int)rx;
            Y = (int)ry;
            Z = (int)rz;
        }

        public override string ToString()
        {
            return String.Format("x: {0}, y: {1}, z: {2}", X, Y, Z);
        }

        public override int GetHashCode()
        {
            return new Tuple<int, int, int>(X, Y, Z).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is CubeIndex other && other.X == X && other.Y == Y && other.Z == Z;
        }

        public AxialIndex ToAxial()
        {
            return new AxialIndex(X, Z);
        }

        public Qub ToQub()
        {
            return new Qub() { x = X, y = Y, z = Z };
        }

        public Vector3 Position()
        {
            return ToAxial().Position();
        }

        public static CubeIndex FromPosition(Vector3 pos)
        {
            return AxialIndex.FromPosition(pos).ToCube();
        }

        public static CubeIndex FromQub(Qub qub)
        {
            return new CubeIndex(qub.x, qub.y, qub.z);
        }

        public CubeIndex GetNeighbor(CubeIndex dir)
        {
            return this + dir;
        }

        public CubeIndex GetNeighbor(HexDir dir)
        {
            return this + dir.AsInd();
        }

        public static CubeIndex operator +(CubeIndex a, CubeIndex b)
        {
            return new CubeIndex(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static CubeIndex operator -(CubeIndex a, CubeIndex b)
        {
            return new CubeIndex(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static CubeIndex operator *(CubeIndex a, int i)
        {
            return new CubeIndex(a.X * i, a.Y * i, a.Z * i);
        }

        public static List<CubeIndex> cubeDirections = new List<CubeIndex>() {
            new CubeIndex(+1, -1, 0),
            new CubeIndex(+1, 0, -1),
            new CubeIndex(0, +1, -1),
            new CubeIndex(-1, +1, 0),
            new CubeIndex(-1, 0, +1),
            new CubeIndex(0, -1, +1)
        };

        public static CubeIndex GetDir(int dir)
        {
            return cubeDirections[dir];
        }

        public static CubeIndex nearestIntersectingNeighbor(CubeIndex start, CubeIndex end)
        {
            var n = cube_distance(start, end);
            return cube_lerp(start, end, 1.0f / n);
        }

        private static int cube_distance(CubeIndex a, CubeIndex b)
        {
            return (Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z)) / 2;
        }

        public static CubeIndex cube_lerp(CubeIndex a, CubeIndex b, float t)
        {
            return new CubeIndex(
                lerp(a.X, b.X, t),
                lerp(a.Y, b.Y, t),
                lerp(a.Z, b.Z, t));
        }

        public static float lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        public static List<CubeIndex> GetRadialLine(CubeIndex start, int dir, int length)
        {
            var tiles = new List<CubeIndex>();
            while (length > 0)
            {
                start += cubeDirections[dir];
                tiles.Add(start);
                length--;
            }
            return tiles;
        }

        public static List<CubeIndex> GetRing(CubeIndex center, int radius)
        {
            var cube = center + (GetDir(4) * radius);

            var results = new List<CubeIndex>();
            foreach (var i in Enumerable.Range(0, 6))
            {
                foreach (var j in Enumerable.Range(0, radius))
                {
                    results.Add(cube);
                    cube = cube.GetNeighbor(GetDir(i));
                }
            }

            return results;
        }

        public static List<CubeIndex> GetSpiral(CubeIndex center, int radius, bool includeCenter = true)
        {
            var results = new List<CubeIndex>();
            if (includeCenter)
            {
                results.Add(center);
            }

            foreach (var i in Enumerable.Range(1, radius - 1))
            {
                results.AddRange(GetRing(center, i));
            }
            return results;
        }
    }
}