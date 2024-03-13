using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.RectGridSystem
{
    public class MultiTileObject : TileObject
    {      
        public override void Setup(RectGrid grid, TileObjectData data, TileCoord center)
        {
            base.Setup(grid, data, center);
        }
        protected override void SetPosition(TileCoord center, bool animate = true)
        {
            base.SetPosition(center, animate);
            //transform.position =  Grid.TileCoordToWorldPosition(Tile.TileCoord - center.TileCoord);
        }
    }
}