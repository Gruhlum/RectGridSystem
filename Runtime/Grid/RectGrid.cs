using HexTecGames.Basics;
using HexTecGames.GridBaseSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames.GridRectSystem
{
    public class RectGrid : BaseGrid
    {
        public override int MaximumRotation
        {
            get
            {
                return TileCoord.MaximumRotation;
            }
        }

        public override float TileWidth
        {
            get
            {
                return tileWidth;
            }
        }
        [SerializeField] private float tileWidth = default;

        public override float TileHeight
        {
            get
            {
                return tileHeight;
            }
        }
        [SerializeField] private float tileHeight = default;

        public override float TotalVerticalSpacing
        {
            get
            {
                return TileHeight + VerticalSpacing;
            }
        }

        public override float TotalHorizontalSpacing
        {
            get
            {
                return TileWidth + HorizontalSpacing;
            }
        }

        public override Vector3 CoordToWorldPosition(Coord coord)
        {
            return new Vector2(
                coord.x * TotalHorizontalSpacing + transform.position.x,
                coord.y * TotalVerticalSpacing + transform.position.y);
            //return new Vector2(coord.x + transform.position.x, coord.y + transform.position.y);
        }
        public override Coord WorldPositionToCoord(Vector3 position)
        {
            return new Coord(Mathf.RoundToInt((position.x - transform.position.x) / TotalHorizontalSpacing),
                Mathf.RoundToInt((position.y - transform.position.y) / TotalVerticalSpacing));
        }

        //public List<TileCoord> FindClosestEmptyTiles(TileCoord center, int maxRange = 20)
        //{
        //    List<TileCoord> emptyTiles = new List<TileCoord>();
        //    bool foundTile = false;
        //    for (int i = 0; i < maxRange; i++)
        //    {
        //        var results = center.GetTilesByDistance(i);
        //        foreach (var result in results)
        //        {
        //            if (IsTileEmpty(result))
        //            {
        //                emptyTiles.Add(result);
        //                foundTile = true;
        //            }
        //        }
        //        if (foundTile)
        //        {
        //            return emptyTiles;
        //        }
        //    }
        //    return null;
        //}

        public override Coord GetDirectionCoord(int direction)
        {
            return TileCoord.GetAdjacent(direction);
        }
        public override int GetDirection(Coord center, Coord coord)
        {
            return TileCoord.GetDirection(center, coord);
        }
        public override Coord GetDirectionFromInput(Vector2 input)
        {
            input = input.normalized;

            float angle = Vector2.SignedAngle(input, Vector2.right) + 90;
            int direction = Mathf.RoundToInt(angle / 90f);
            Debug.Log("input: " + input.ToString() + " angle: " + angle + " direction: " + direction);
            return TileCoord.adjacents[direction];
        }

        public override Coord GetRotatedCoord(Coord center, Coord coord, int rotation)
        {
            return TileCoord.Rotate(center, coord, rotation);
        }
        public override List<Coord> GetArea(Coord center, int radius)
        {
            return TileCoord.GetArea(center, radius);
        }
        public override List<Coord> GetRing(Coord center, int radius)
        {
            return TileCoord.GetRing(center, radius);
        }
        public override List<Coord> GetAdjacents(Coord center)
        {
            return TileCoord.GetAdjacents(center, 1);
        }
        public override List<Coord> GetNeighbourCoords(Coord center)
        {
            return TileCoord.GetNeighbours(center);
        }

        public override int GetDistance(Coord coord1, Coord coord2)
        {
            return TileCoord.GetDistance(coord1, coord2);
        }

        public override Coord GetClosestCoordInLine(Coord start, Coord target, int direction)
        {
            return TileCoord.GetClosestCoordInLine(start, target, direction);
        }

        public override List<Coord> GetLine(Coord start, Coord target)
        {
            return TileCoord.GetLine(start, target);
        }

        public override List<Coord> GetCoordsInBox(Vector2 start, Vector2 end)
        {
            return TileCoord.GetCoordsInBox(start, end);
        }

        public override List<Coord> GetCorner(int direction, int radius, int thickness)
        {
            throw new System.NotImplementedException();
        }

        public override bool IsInLine(Coord coord1, Coord coord2)
        {
            return TileCoord.IsInLine(coord1, coord2);
        }
    }
}