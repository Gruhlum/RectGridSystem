using HexTecGames.Basics.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.RectGridSystem
{
	public class TileObjectDisplayController : DisplayController<TileObjectData>
	{
        [SerializeField] private BuildController buildController = default;


        public override void DisplayClicked(Display<TileObjectData> display)
        {
            base.DisplayClicked(display);
            buildController.SetSelectedObject(display.Item);
        }
    }
}