using HexTecGames.Basics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.RectGridSystem
{
    public class BuildController : MonoBehaviour
    {
        [SerializeField] private RectGrid grid = default;
        [SerializeField] private GhostObject ghost = default;

        [Header("Settings")]
        [SerializeField] private bool dropSelectedAfterBuild = default;
        [SerializeField][DrawIf("dropSelectedAfterBuild", false)] private bool allowBuildHoldingDown = default;
        [SerializeField] private bool allowRemoving = default;
        [SerializeField][DrawIf("allowRemoving", true)] private bool allowRemoveHoldingDown = default;
        [SerializeField][DrawIf("allowRemoving", true)] private bool allowOverwriting = default;

        [SerializeField] private MultiSpawner tileObjectSpawner = default;
        [Header("Placement")]
        [SerializeField] private TileHighlightSpawner highlightSpawner = default;
        [SerializeField] private Color invalidLocationCol = Color.red;
        [SerializeField] private Color validLocationCol = Color.green;
        public TileObjectData SelectedObject
        {
            get
            {
                return selectedObject;
            }
            private set
            {
                selectedObject = value;
            }
        }
        private TileObjectData selectedObject;

        public TileCoord HoverCoord
        {
            get
            {
                return hoverCoord;
            }
            private set
            {
                hoverCoord = value;
            }
        }
        private TileCoord hoverCoord;

        private bool isValidPosition;
        private List<TileObject> tileObjects = new List<TileObject>();
        private bool clearedSelection;

        private void Update()
        {
            TileCoord coord = grid.MousePositionToTileCoord();
            if (HoverCoord != coord)
            {
                if (SelectedObject != null)
                {
                    UpdateSelection(coord);
                    if (allowBuildHoldingDown && Input.GetMouseButton(0))
                    {
                        Build();
                    }
                }
                else HoverCoord = coord;
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (SelectedObject != null)
                {
                    Build();
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                clearedSelection = false;
            }
            if (SelectedObject != null && Input.GetMouseButtonDown(1))
            {
                ClearSelectedObject();
                clearedSelection = true;
            }
            else if (allowRemoving)
            {
                if (clearedSelection)
                {
                    return;
                }
                if (allowRemoveHoldingDown && Input.GetMouseButton(1))
                {
                    RemoveTileObject();
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    RemoveTileObject();
                }
            }
        }

        private void RemoveTileObject()
        {
            RemoveTileObject(HoverCoord);
        }
        private void RemoveTileObject(TileCoord coord)
        {
            TileObject tileObject = grid.FindTileObject(coord);
            if (tileObject != null)
            {
                tileObject.Deactivate();
            }
        }

        private void CheckValidTiles()
        {
            var results = SelectedObject.GetTiles();
            highlightSpawner.DeactivateAll();
            bool allowed = true;
            foreach (var result in results)
            {
                TileCoord normalizedCoord = result + HoverCoord;
                Vector2 position = grid.TileCoordToWorldPosition(normalizedCoord);
                Color col;
                if (!grid.IsTileValid(normalizedCoord))
                {
                    col = invalidLocationCol;
                    allowed = false;
                }
                else col = validLocationCol;
                highlightSpawner.Spawn().Activate(position);
            }
            isValidPosition = allowed;
        }
        public void Build()
        {
            Build(SelectedObject, HoverCoord);
            if (dropSelectedAfterBuild)
            {
                ClearSelectedObject();
            }
            else CheckValidTiles();
        }
        public void Build(TileObjectData data, TileCoord coord, bool ignoreRestriction = false)
        {
            if (!ignoreRestriction && !isValidPosition)
            {
                if (allowOverwriting && allowRemoving)
                {
                    if (!grid.TileExists(coord))
                    {
                        Debug.Log("Invalid Position");
                        return;
                    }
                    else
                    {
                        var result = grid.FindTileObject(coord);
                        result.Deactivate();                      
                    }
                }
                else
                {
                    Debug.Log("Invalid Position");
                    return;
                }
                
            }
            TileObject clone = tileObjectSpawner.Spawn(data.prefab);
            clone.Setup(grid, data, coord);
            //var results = data.GetNormalizedTiles(coord);
            //grid.AddTileObject(results, clone);
        }
        public void ClearSelectedObject()
        {
            SetSelectedObject(null);
        }
        public void SetSelectedObject(TileObjectData tileObjectData)
        {
            SelectedObject = tileObjectData;
            if (tileObjectData == null)
            {
                ghost.Deactivate();
                highlightSpawner.DeactivateAll();
            }
            else
            {
                TileCoord coord = grid.MousePositionToTileCoord();
                ghost.Activate(grid.TileCoordToWorldPosition(coord), tileObjectData.Sprite, tileObjectData.Color);
                UpdateSelection(coord);
            }
        }
        public void UpdateSelection(TileCoord coord)
        {
            HoverCoord = coord;
            ghost.SetPosition(grid.TileCoordToWorldPosition(coord) + SelectedObject.GetOffsetVector());
            CheckValidTiles();
        }
    }
}