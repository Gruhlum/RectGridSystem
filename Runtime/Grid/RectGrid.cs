using HexTecGames.Basics;
using HexTecGames.GridBaseSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames.RectGridSystem
{
    public class RectGrid : BaseGrid
    {
        public override Vector3 CoordToWorldPoint(Coord coord)
        {
            return new Vector2(coord.x * TotalHorizontalSpacing + transform.position.x, coord.y * TotalVerticalSpacing + transform.position.y);
            //return new Vector2(coord.x + transform.position.x, coord.y + transform.position.y);
        }
        public override Coord WorldPositionToCoord(Vector3 position)
        {
            return new Coord(Mathf.RoundToInt((position.x - transform.position.x) / TotalHorizontalSpacing),
                Mathf.RoundToInt((position.y - transform.position.y) / TotalVerticalSpacing));
        }

        ////public List<TileCoord> FindClosestEmptyTiles(TileCoord center, int maxRange = 20)
        ////{
        ////    List<TileCoord> emptyTiles = new List<TileCoord>();
        ////    bool foundTile = false;
        ////    for (int i = 0; i < maxRange; i++)
        ////    {
        ////        var results = center.GetTilesByDistance(i);
        ////        foreach (var result in results)
        ////        {
        ////            if (IsTileEmpty(result))
        ////            {
        ////                emptyTiles.Add(result);
        ////                foundTile = true;
        ////            }
        ////        }
        ////        if (foundTile)
        ////        {
        ////            return emptyTiles;
        ////        }
        ////    }
        ////    return null;
        ////}

        public override List<Coord> GetArea(Coord center, int radius)
        {
            return TileCoord.GetArea(center, radius);
        }

        public override List<Coord> GetRing(Coord center, int radius)
        {
            return TileCoord.GetRing(center, radius);
        }

        public virtual List<Coord> GetAdjacent(Coord center)
        {
            return GetRing(center, 1);
        }

        public override List<Coord> GetNeighbourCoords(Coord center)
        {
            return TileCoord.GetAdjacents(center);
        }
    }
}