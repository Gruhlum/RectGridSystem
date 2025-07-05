using System.Collections.Generic;
using HexTecGames.Basics;
using HexTecGames.GridBaseSystem.Shapes;
using UnityEngine;

namespace HexTecGames.GridRectSystem.Shapes
{
    [System.Serializable]
    public class SquareShape : Shape
    {
        [SerializeField] private int height = 10;
        [SerializeField] private int width = 10;


        public override List<Coord> GetCoords(Coord center)
        {
            List<Coord> coords = new List<Coord>();
            int startX = -(width / 2) + center.x;
            int startY = -(height / 2) + center.y;
            int endX = (width / 2) + center.x;
            int endY = (height / 2) + center.y;

            if (width % 2 != 0)
            {
                endX++;
            }
            if (height % 2 != 0)
            {
                endY++;
            }

            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    coords.Add(new Coord(x, y));
                }
            }
            return coords;
        }
    }
}