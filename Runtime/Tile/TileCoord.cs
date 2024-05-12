using HexTecGames.GridBaseSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.RectGridSystem
{
    public static class TileCoord
    {
        public static Coord Zero
        {
            get
            {
                return zero;
            }
        }
        private static readonly Coord zero = new Coord(0, 0);

        public static Coord Up
        {
            get
            {
                return up;
            }
        }
        private static readonly Coord up = new Coord(0, 1);
        public static Coord Down
        {
            get
            {
                return down;
            }
        }
        private static readonly Coord down = new Coord(0, -1);
        public static Coord Left
        {
            get
            {
                return left;
            }
        }
        private static readonly Coord left = new Coord(-1, 0);
        public static Coord Right
        {
            get
            {
                return right;
            }
        }
        private static readonly Coord right = new Coord(1, 0);

        public static Coord UpRight
        {
            get
            {
                return upRight;
            }
        }
        private static readonly Coord upRight = new Coord(1, 1);
        public static Coord DownRight
        {
            get
            {
                return downRight;
            }
        }
        private static readonly Coord downRight = new Coord(1, -1);
        public static Coord DownLeft
        {
            get
            {
                return downLeft;
            }
        }
        private static readonly Coord downLeft = new Coord(-1, -1);
        public static Coord UpLeft
        {
            get
            {
                return upLeft;
            }
        }
        private static readonly Coord upLeft = new Coord(-1, 1);

        private static readonly Coord[] adjacents = new Coord[] { Up, Right, Down, Left };
        private static readonly Coord[] diagonals = new Coord[] { UpRight, DownRight, DownLeft, UpLeft };

        public static int GetDistance(Coord coord1, Coord coord2)
        {
            int diffX = Mathf.Max(coord1.x, coord2.x) - Mathf.Min(coord1.x, coord2.x);
            int diffY = Mathf.Max(coord1.y, coord2.y) - Mathf.Min(coord1.y, coord2.y);

            return diffX + diffY;
        }
        public static List<Coord> GetArea(Coord center, int radius)
        {
            if (radius < 0)
            {
                return null;
            }
            List<Coord> results = new List<Coord>();
            for (int i = 0; i < radius; i++)
            {
                results.AddRange(GetRing(center, i));
            }
            return results;
        }
        public static List<Coord> GetRing(Coord center, int radius)
        {
            if (radius < 0)
            {
                return null;
            }
            if (radius == 0)
            {
                return new List<Coord>() { center };
            }
            List<Coord> results = new List<Coord>();
            Coord startCoord = new Coord(center.x - radius, center.y - radius);
            for (int i = 0; i < 4; i++)
            {
                for (int x = 0; x < radius * 2; x++)
                {
                    startCoord = GetAdjacent(startCoord, i);
                    results.Add(startCoord);
                }
            }
            return results;
        }
       
        public static Coord GetAdjacent(Coord center, int direction, int distance = 1)
        {
            if (distance <= 1)
            {
                return center + adjacents[direction % 4];
            }
            else return center + adjacents[direction % 4] * distance;
        }
        public static List<Coord> GetAdjacents(Coord center, int distance = 1)
        {
            List<Coord> results = new List<Coord>();
            for (int i = 0; i < 4; i++)
            {
                results.Add(GetAdjacent(center, i, distance));
            }
            return results;
        }
        public static Coord GetDiagonal(Coord center, int direction, int distance = 1)
        {
            if (distance <= 1)
            {
                return center + diagonals[direction % 4];
            }
            return center + diagonals[direction % 4] * distance;
        }
        public static List<Coord> GetDiagonals(Coord center, int distance = 1)
        {
            if (distance < 0)
            {
                return null;
            }
            if (distance == 0)
            {
                return new() { center };
            }

            var results = new List<Coord>();
            if (distance == 1)
            {
                foreach (var neighbour in adjacents)
                {
                    results.Add(center + neighbour);
                }
                return results;
            }
            for (int i = 0; i < 4; i++)
            {
                results.Add(GetDiagonal(center, i, distance));
            }
            return results;
        }
        public static List<Coord> GetNeighbours(Coord center, int distance = 1)
        {
            List<Coord> results = new List<Coord>();
            results.AddRange(GetAdjacents(center, distance));
            results.AddRange(GetNeighbours(center, distance));

            return results;
        }
        
        public static bool IsAdjacent(Coord coord1, Coord coord2, int distance = 1)
        {
            int currentDistance = GetDistance(coord1, coord2);
            return currentDistance == distance;
        }
        public static bool IsDiagonal(Coord coord1, Coord coord2, int distance = 1)
        {
            int diffX = Mathf.Max(coord1.x, coord2.x) - Mathf.Min(coord1.x, coord2.x);
            int diffY = Mathf.Max(coord1.y, coord2.y) - Mathf.Min(coord1.y, coord2.y);

            return diffX == distance && diffY == distance;
        }
        public static bool IsNeighbour(Coord coord1, Coord coord2)
        {
            if (IsNeighbour(coord1, coord2) || IsDiagonal(coord1, coord2))
            {
                return true;
            }
            return false;
        }
    }
}