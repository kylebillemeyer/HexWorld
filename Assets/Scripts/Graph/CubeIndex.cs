using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using HexWorld.Models;

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

        public static List<CubeIndex> GetSpiral(CubeIndex center, int radius)
        {
            var results = new List<CubeIndex>() { center };
            foreach (var i in Enumerable.Range(1, radius - 1))
            {
                results.AddRange(GetRing(center, i));
            }
            return results;
        }
    }
}