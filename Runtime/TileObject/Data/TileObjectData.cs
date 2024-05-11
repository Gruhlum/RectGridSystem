//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace HexTecGames.RectGridSystem
//{
//    public abstract class TileObjectData : ScriptableObject
//    {
//        public Sprite Sprite
//        {
//            get
//            {
//                return sprite;
//            }
//            private set
//            {
//                sprite = value;
//            }
//        }
//        [SerializeField] private Sprite sprite;

//        public Color Color
//        {
//            get
//            {
//                return color;
//            }
//            private set
//            {
//                color = value;
//            }
//        }
//        [SerializeField] private Color color = Color.white;
//        public TileObject prefab = default;

//        public virtual bool HasOffsetX
//        {
//            get
//            {
//                return false;
//            }
//        }
//        public virtual bool HasOffsetY
//        {
//            get
//            {
//                return false;
//            }
//        }
//        public virtual List<TileCoord> GetTiles()
//        {
//            return new() { TileCoord.Zero };
//        }
//        public List<TileCoord> GetNormalizedTiles(TileCoord center)
//        {
//            var results = GetTiles();
//            List<TileCoord> coords = new List<TileCoord>();
//            foreach (var result in results)
//            {
//                coords.Add(result + center);
//            }
//            return coords;
//        }
//        public Vector2 GetOffsetVector()
//        {
//            Vector2 offset = Vector2.zero;
//            if (HasOffsetX)
//            {
//                offset.x = 0.5f;
//            }
//            if (HasOffsetY)
//            {
//                offset.y = 0.5f;
//            }
//            return offset;
//        }
//    }
//}