//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace HexTecGames.RectGridSystem
//{
//	[CreateAssetMenu(menuName = "HexTecGames/MultiTileObjectData")]
//	public class MultiTileObjectData : TileObjectData
//	{
//		public List<TileCoord> size = default;
 
//        public override bool HasOffsetX
//        {
//            get
//            {
//                return offsetX;
//            }
//        }
//        [SerializeField] private bool offsetX = default;

//        public override bool HasOffsetY
//        {
//            get
//            {
//                return offsetY;
//            }
//        }
//        [SerializeField] private bool offsetY = default;
//        public override List<TileCoord> GetTiles()
//        {
//            return size;
//        }
//    }
//}