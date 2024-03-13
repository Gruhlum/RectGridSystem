using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.RectGridSystem
{
    [System.Serializable]
    public struct TileCoord
    {
        public int x;
        public int y;

        public static TileCoord Zero
        {
            get
            {
                return zero;
            }
        }
        private static readonly TileCoord zero = new TileCoord(0, 0);
        public static TileCoord Up
        {
            get
            {
                return up;
            }
        }
        private static readonly TileCoord up = new TileCoord(0, 1);

        public static TileCoord Down
        {
            get
            {
                return down;
            }
        }
        private static readonly TileCoord down = new TileCoord(0, -1);

        public static TileCoord Left
        {
            get
            {
                return left;
            }
        }
        private static readonly TileCoord left = new TileCoord(-1, 0);

        public static TileCoord Right
        {
            get
            {
                return right;
            }
        }
        private static readonly TileCoord right = new TileCoord(1, 0);
        public static TileCoord UpRight
        {
            get
            {
                return upRight;
            }
        }
        private static readonly TileCoord upRight = new TileCoord(1, 1);
        public static TileCoord DownRight
        {
            get
            {
                return downRight;
            }
        }
        private static readonly TileCoord downRight = new TileCoord(1, -1);
        public static TileCoord DownLeft
        {
            get
            {
                return downLeft;
            }
        }
        private static readonly TileCoord downLeft = new TileCoord(-1, -1);
        public static TileCoord UpLeft
        {
            get
            {
                return upLeft;
            }
        }
        private static readonly TileCoord upLeft = new TileCoord(-1, 1);


        private static readonly TileCoord[] neighbours = new TileCoord[] { Up, Right, Down, Left };
        private static readonly TileCoord[] diagonals = new TileCoord[] { UpRight, DownRight, DownLeft, UpLeft };

        public TileCoord(Vector2 pos)
        {
            x = Mathf.RoundToInt(pos.x);
            y = Mathf.RoundToInt(pos.y);

        }
        public TileCoord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"Tile({x}, {y})";
        }

        public override bool Equals(object obj)
        {
            return obj is TileCoord coord &&
                   this.x == coord.x &&
                   this.y == coord.y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.x, this.y);
        }

        public static int GetDistance(TileCoord coord1, TileCoord coord2)
        {
            int diffX = Mathf.Max(coord1.x, coord2.x) - Mathf.Min(coord1.x, coord2.x);
            int diffY = Mathf.Max(coord1.y, coord2.y) - Mathf.Min(coord1.y, coord2.y);

            return diffX + diffY;
        }
        public List<TileCoord> GetSquare(int radius)
        {
            List<TileCoord> results = new List<TileCoord>();
            for (int i = 1; i < radius; i++)
            {
                results.AddRange(GetRing(i));
            }
            return results;
        }
        public List<TileCoord> GetRing(int radius)
        {
            if (radius < 0)
            {
                return null;
            }
            if (radius == 0)
            {
                return new List<TileCoord>() { this };
            }
            List<TileCoord> results = new List<TileCoord>();
            TileCoord startCoord = new TileCoord(x - radius, y - radius);
            for (int i = 0; i < 4; i++)
            {
                for (int x = 0; x < radius * 2; x++)
                {
                    startCoord = startCoord.GetNeighbour(i);
                    results.Add(startCoord);
                }
            }
            return results;
        }
        public List<TileCoord> GetTilesByDistance(int distance = 1)
        {
            if (distance < 0)
            {
                return null;
            }
            if (distance == 0)
            {
                return new() { this };
            }

            List<TileCoord> results = new List<TileCoord>();
            TileCoord lastResult;
            for (int dir = 0; dir < 4; dir++)
            {
                lastResult = GetNeighbour(dir, distance);
                results.Add(lastResult);
                for (int i = 1; i < distance; i++)
                {
                    lastResult = lastResult.GetDiagonal(dir + 1);
                    results.Add(lastResult);
                }
            }

            return results;
        }
        public List<TileCoord> GetFullNeighbours(int distance = 1)
        {
            List<TileCoord> results = GetNeighbours(distance);
            results.AddRange(GetDiagonals(distance));
            return results;
        }
        public List<TileCoord> GetNeighbours(int distance = 1)
        {
            if (distance < 0)
            {
                return null;
            }
            if (distance == 0)
            {
                return new() { this };
            }

            var results = new List<TileCoord>();
            if (distance == 1)
            {
                foreach (var neighbour in neighbours)
                {
                    results.Add(this + neighbour);
                }
                return results;
            }
            for (int i = 0; i < 4; i++)
            {
                results.Add(GetNeighbour(i, distance));
            }
            return results;
        }
        public List<TileCoord> GetDiagonals(int distance = 1)
        {
            if (distance < 0)
            {
                return null;
            }
            if (distance == 0)
            {
                return new() { this };
            }

            var results = new List<TileCoord>();
            if (distance == 1)
            {
                foreach (var neighbour in neighbours)
                {
                    results.Add(this + neighbour);
                }
                return results;
            }
            for (int i = 0; i < 4; i++)
            {
                results.Add(GetDiagonal(i, distance));
            }
            return results;
        }
        public TileCoord GetNeighbour(int direction, int distance = 1)
        {
            if (distance <= 1)
            {
                return this + neighbours[direction % 4];
            }
            else return this + neighbours[direction % 4] * distance;
        }
        public TileCoord GetDiagonal(int direction, int distance = 1)
        {
            if (distance <= 1)
            {
                return this + diagonals[direction % 4];
            }
            return this + diagonals[direction % 4] * distance;
        }
        public bool IsNeighbour(TileCoord coord, int distance = 1)
        {
            int currentDistance = GetDistance(this, coord);
            return currentDistance == distance;
        }
        public bool IsDiagonalNeighbour(TileCoord coord, int distance = 1)
        {
            int diffX = Mathf.Max(x, coord.x) - Mathf.Min(x, coord.x);
            int diffY = Mathf.Max(y, coord.y) - Mathf.Min(y, coord.y);

            return diffX == distance && diffY == distance;
        }
        public bool IsFullNeighbour(TileCoord coord)
        {
            if (IsNeighbour(coord) || IsDiagonalNeighbour(coord))
            {
                return true;
            }
            return false;
        }
        public static TileCoord operator *(TileCoord coord, float number)
        {
            return new TileCoord(Mathf.RoundToInt(coord.x * number), (Mathf.RoundToInt(coord.y * number)));
        }
        public static TileCoord operator +(TileCoord coord1, TileCoord coord2)
        {
            coord1.x += coord2.x;
            coord1.y += coord2.y;
            return coord1;
        }
        public static TileCoord operator -(TileCoord coord1, TileCoord coord2)
        {
            coord1.x -= coord2.x;
            coord1.y -= coord2.y;
            return coord1;
        }
        public static bool operator ==(TileCoord coord1, TileCoord coord2)
        {
            if (coord1.x != coord2.x)
            {
                return false;
            }
            if (coord1.y != coord2.y)
            {
                return false;
            }
            return true;
        }
        public static bool operator !=(TileCoord coord1, TileCoord coord2)
        {
            return !(coord1 == coord2);
        }

    }
}