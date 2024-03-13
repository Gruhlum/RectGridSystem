using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.RectGridSystem
{
    [System.Serializable]
    public class Tile
    {
        public TileCoord TileCoord
        {
            get
            {
                return tileCoord;
            }
            set
            {
                tileCoord = value;
            }
        }
        private TileCoord tileCoord;

        public RectGrid Grid
        {
            get
            {
                return grid;
            }
            private set
            {
                grid = value;
            }
        }
        private RectGrid grid;

        public Tile(TileCoord tileCoord, RectGrid grid)
        {
            this.TileCoord = tileCoord;
            this.Grid = grid;
        }

        public Vector2 GetWorldPosition()
        {
            return Grid.TileCoordToWorldPosition(TileCoord);
        }
    }
}