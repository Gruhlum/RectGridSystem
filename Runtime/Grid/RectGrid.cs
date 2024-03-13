using HexTecGames.Basics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames.RectGridSystem
{
    public class RectGrid : MonoBehaviour
    {
        private Dictionary<TileCoord, Tile> tiles = new Dictionary<TileCoord, Tile>();
        private Dictionary<TileCoord, TileObject> tileObjects = new Dictionary<TileCoord, TileObject>();


        public TileCoord Center
        {
            get
            {
                return center;
            }
            private set
            {
                center = value;
            }
        }
        private TileCoord center;
        public int Width
        {
            get
            {
                return this.width;
            }
            private set
            {
                this.width = value;
            }
        }
        [SerializeField] private int width = 10;
        public int Height
        {
            get
            {
                return this.height;
            }
            private set
            {
                this.height = value;
            }
        }
        [SerializeField] private int height = 10;

        [SerializeField] private Spawner<Transform> tileDisplaySpawner = default;

        public float OffsetX
        {
            get
            {
                return offsetX;
            }
            set
            {
                offsetX = value;
            }
        }
        private float offsetX = default;
        public float OffsetY
        {
            get
            {
                return offsetY;
            }
            set
            {
                offsetY = value;
            }
        }
        private float offsetY = default;
        public float SpacingX
        {
            get
            {
                return spacingX;
            }
            private set
            {
                spacingX = value;
            }
        }
        [SerializeField] private float spacingX;
        public float SpacingY
        {
            get
            {
                return spacingY;
            }
            private set
            {
                spacingY = value;
            }
        }
        [SerializeField] private float spacingY;


        private void Awake()
        {
            if (Width > 0 && Height > 0)
            {
                GenerateGrid(Width, Height);
                SpawnTileDisplays();
            }
        }
        private void SpawnTileDisplays()
        {
            if (tileDisplaySpawner.Prefab == null)
            {
                return;
            }
            foreach (var key in tiles.Keys)
            {
                var display = tileDisplaySpawner.Spawn();
                display.transform.position = TileCoordToWorldPosition(key);
            }
        }

        private void CalculateOffset()
        {
            OffsetX = -width / 2f;
            OffsetY = -height / 2f;
        }
        public void DeactivateAll()
        {
            tiles.Clear();
            tileObjects.Clear();
            tileDisplaySpawner.DeactivateAll();
        }
        public void GenerateGrid(int width, int height)
        {
            Width = width;
            Height = height;
            Center = new TileCoord(Mathf.RoundToInt((Width - 0.5f) / 2f), Mathf.RoundToInt((Height - 0.5f) / 2f));
            CalculateOffset();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    TileCoord coord = new TileCoord(x, y);
                    tiles.Add(coord, new Tile(coord, this));
                }
            }
        }

        public void AddTileObject(TileCoord coord, TileObject tileObject)
        {
            tileObjects.Add(coord, tileObject);
        }
        public void AddTileObject(List<TileCoord> coords, TileObject tileObject)
        {
            foreach (var coord in coords)
            {
                AddTileObject(coord, tileObject);
            }
        }
        public void RemoveTileObject(TileCoord coord)
        {
            if (tileObjects.TryGetValue(coord, out TileObject tileObject))
            {
                foreach (var tileCoord in tileObject.Data.GetNormalizedTiles(coord))
                {
                    tileObjects.Remove(tileCoord);
                }
            }
            else Debug.Log("No building found for " + coord.ToString());
        }
        public void RemoveTileObject(List<TileCoord> coords)
        {
            foreach (var coord in coords)
            {
                RemoveTileObject(coord);
            }
        }
        public List<T> GetTileObjects<T>() where T : TileObject
        {
            List<T> results = new List<T>();
            foreach (var item in tileObjects.Values)
            {
                if (item is T result)
                {
                    results.Add(result);
                }
            }
            return results;
        }
        public List<TileObject> GetTileObjects()
        {
            return tileObjects.Values.ToList();
        }
        public TileObject FindTileObject(TileCoord coord)
        {
            tileObjects.TryGetValue(coord, out TileObject tileObject);
            return tileObject;
        }
        public List<TileObject> FindTileObjects(List<TileCoord> coords)
        {
            List<TileObject> results = new();
            foreach (var coord in coords)
            {
                tileObjects.TryGetValue(coord, out TileObject tileObject);
                results.Add(tileObject);
            }
            return results;
        }
        public T FindTileObject<T>(TileCoord coord) where T : TileObject
        {
            List<TileObject> results = new();
            tileObjects.TryGetValue(coord, out TileObject tileObject);
            if (tileObject is T t)
            {
                return t;
            }
            return null;
        }
        public List<T> FindTileObjects<T>(List<TileCoord> coords) where T : TileObject
        {
            List<T> results = new();
            foreach (var coord in coords)
            {
                T result = FindTileObject<T>(coord);
                if (result != null)
                {
                    results.Add(result);
                }
            }
            return results;
        }
        public bool IsTileEmpty(TileCoord coord)
        {
            //foreach (var key in tileObjects.Keys)
            //{
            //    Debug.Log(key);
            //}
            return !tileObjects.ContainsKey(coord);
        }
        public bool IsTileEmpty(List<TileCoord> coords)
        {
            foreach (var coord in coords)
            {
                if (!IsTileEmpty(coord))
                {
                    return false;
                }
            }
            return true;
        }
        public bool IsTileValid(TileCoord coord)
        {
            if (!TileExists(coord))
            {
                return false;
            }
            return IsTileEmpty(coord);
        }
        public bool IsTileValid(List<TileCoord> coords)
        {
            foreach (var coord in coords)
            {
                if (!IsTileValid(coord))
                {
                    return false;
                }
            }
            return true;
        }
        public bool TileExists(TileCoord coord)
        {
            if (tiles.ContainsKey(coord))
            {
                return true;
            }
            return false;
        }
        public Tile FindTile(TileCoord coord)
        {
            tiles.TryGetValue(coord, out Tile result);
            return result;
        }
        public Tile FindTile(int x, int y)
        {
            return FindTile(new TileCoord(x, y));
        }
        public Tile FindTile(Vector2 pos)
        {
            return FindTile(WorldPositionToTileCoord(pos));
        }
        public void MoveTileObject(TileCoord currentPos, TileCoord targetPos)
        {
            if (tileObjects.TryGetValue(currentPos, out TileObject tileObj))
            {
                tileObjects.Remove(currentPos);
                tileObjects.Add(targetPos, tileObj);
            }
        }
        public List<TileCoord> FindClosestEmptyTiles(TileCoord center, int maxRange = 20)
        {
            List<TileCoord> emptyTiles = new List<TileCoord>();
            bool foundTile = false;
            for (int i = 0; i < maxRange; i++)
            {
                var results = center.GetTilesByDistance(i);
                foreach (var result in results)
                {
                    if (IsTileEmpty(result))
                    {
                        emptyTiles.Add(result);
                        foundTile = true;
                    }
                }
                if (foundTile)
                {
                    return emptyTiles;
                }
            }
            return null;
        }
        public List<TileCoord> GetEmptyTiles(List<TileCoord> coords)
        {
            for (int i = coords.Count - 1; i >= 0; i--)
            {
                if (!IsTileEmpty(coords[i]))
                {
                    coords.RemoveAt(i);
                }
            }
            return coords;
        }
        public List<TileCoord> FindClosestEmptyRing(TileCoord center)
        {
            for (int i = 0; i < 10; i++)
            {
                List<TileCoord> results = GetEmptyTiles(center.GetRing(i));
                if (results.Count > 0)
                {
                    return results;
                }
            }
            return null;
        }
        public List<TileCoord> GetEmptyNeighbours(TileCoord coord, int distance = 1)
        {
            return GetEmptyTiles(coord.GetNeighbours(distance));
        }
        public List<T> GetNeighbours<T>(TileCoord coord, int distance = 1) where T : TileObject
        {
            var results = coord.GetNeighbours(distance);
            List<T> neighbours = new List<T>();
            foreach (var result in results)
            {
                T neighbour = FindTileObject<T>(result);
                if (neighbour != null)
                {
                    neighbours.Add(neighbour);
                }
            }
            return neighbours;
        }
        public List<TileCoord> GetEmptyDiagonals(TileCoord coord, int distance = 1)
        {
            return GetEmptyTiles(coord.GetDiagonals(distance));
        }
        public bool IsValidPosition(Vector2 pos)
        {
            TileCoord coord = WorldPositionToTileCoord(pos);
            return tiles.ContainsKey(coord);
        }
        public TileCoord WorldPositionToTileCoord(Vector2 pos)
        {
            pos = AlignVector(pos);
            return new TileCoord(pos);
        }
        public TileCoord MousePositionToTileCoord()
        {
            return WorldPositionToTileCoord(Camera.main.GetMousePosition());
        }
        private bool IsHalfOffset()
        {
            if (offsetX * 10f % 10 != 0)
            {
                return true;
            }
            return false;
        }
        private Vector2 AlignVector(Vector2 pos)
        {
            if (IsHalfOffset())
            {
                //float upper = pos.x + 0.5f;
                //float lower = pos.x - 0.5f;
                //float diffUpper = Mathf.CeilToInt(upper);
                //float diffLower = pos.x - lower;
                //if (upper)
                //{

                //}
                //pos.x -= 0.5f;
                //pos.y -= 0.5f;
            }
            pos.x -= OffsetX + 0.5f;
            pos.y -= offsetY + 0.5f;
            return pos;
        }
        public Vector2 WorldPositionToGrid(Vector2 pos)
        {
            pos = pos.Round();
            return pos;
        }
        public Vector2 TileCoordToWorldPosition(TileCoord tileCoord)
        {
            if (IsHalfOffset())
            {
                return new Vector2(tileCoord.x + offsetX + 0.5f + spacingX * tileCoord.x, tileCoord.y + offsetY + 0.5f + spacingY * tileCoord.y);

            }
            else
            {
                return new Vector2(tileCoord.x + offsetX + spacingX * tileCoord.x, tileCoord.y + offsetY + spacingY * tileCoord.y);
            }
        }
    }
}