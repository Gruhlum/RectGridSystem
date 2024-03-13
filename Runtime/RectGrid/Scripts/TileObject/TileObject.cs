using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace HexTecGames.RectGridSystem
{
    public abstract class TileObject : MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer sr = default;

        public TileObjectData Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }
        private TileObjectData data = default;

        public TileCoord TileCoord
        {
            get
            {
                return tile;
            }
            protected set
            {
                tile = value;
            }
        }
        private TileCoord tile;

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
        public virtual void Setup(RectGrid grid, TileObjectData data, TileCoord coord)
        {
            if (coord == null)
            {
                Debug.LogError("Tile is null!");
            }
            grid.AddTileObject(coord, this);
            this.Grid = grid;
            this.Data = data;          
            SetPosition(coord, false);
            if (sr != null)
            {
                //sr.color = data.Color;
                sr.sprite = data.Sprite;
            }
            name = data.name;
        }
        public virtual void Deactivate()
        {
            grid.RemoveTileObject(TileCoord);
            gameObject.SetActive(false);
        }
        public Sprite GetSprite()
        {
            return sr.sprite;
        }
        protected virtual void SetPosition(TileCoord center, bool animate = true)
        {
            Vector2 result = Grid.TileCoordToWorldPosition(center) + Data.GetOffsetVector();
            transform.position = result;
            TileCoord = center;
        }       
    }
}