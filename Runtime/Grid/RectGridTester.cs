using HexTecGames.Basics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.RectGridSystem
{
    public class RectGridTester : MonoBehaviour
    {
        [SerializeField] private Spawner<TileHighlighter> highlightSpawner = default;

        [SerializeField] private RectGrid grid = default;
        [SerializeField] private float speed = 1;
        [SerializeField] private bool oneByOne = default;

        [SerializeField] private float range = 10;

        private void Awake()
        {
            StartTest();
        }

        [ContextMenu("Start Test")]
        public void StartTest()
        {
            StartCoroutine(AnimateTest());
        }
        private IEnumerator AnimateTest()
        {
            yield return GetTilesByDistance();
            yield return new WaitForSeconds(1f / speed);
            yield return GetSquare();
        }
        private IEnumerator GetTilesByDistance()
        {
            for (int i = 0; i < range; i++)
            {
                var results = grid.Center.GetTilesByDistance(i);

                yield return DisplayResults(results);
            }
        }
        private IEnumerator GetSquare()
        {
            for (int i = 0; i < range; i++)
            {
                var results = grid.Center.GetSquare(i);
                yield return new WaitForSeconds(1.4f / speed);
                yield return DisplayResults(results);
            }
        }
        private IEnumerator DisplayResults(List<TileCoord> results)
        {
            foreach (var result in results)
            {
                var highlight = highlightSpawner.Spawn();
                StartCoroutine(highlight.ShowHighlight(grid.TileCoordToWorldPosition(result),0, 0.8f / speed, 0.3f / speed, 0.3f / speed));
                if (oneByOne)
                {
                    yield return new WaitForSeconds(0.1f / speed);
                }               
            }
            yield return new WaitForSeconds(0.4f / speed);
        }
    }
}