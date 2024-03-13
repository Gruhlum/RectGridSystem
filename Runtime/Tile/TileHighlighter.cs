using HexTecGames.Basics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.RectGridSystem
{
    public class TileHighlighter : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sr = default;

        [SerializeField] private float startDelay = default;
        [SerializeField] private bool hasDuration = default;
        [DrawIf(nameof(hasDuration), true)] private float duration = 1;
        [SerializeField] private float fadeIn = 1;
        [SerializeField] private float fadeOut = 1;


        private void Reset()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        public void Activate(Vector3 position)
        {
            StartCoroutine(ShowHighlight(position));
        }
        public void Deactivate()
        {
            StartCoroutine(FadeOut(sr.color, fadeOut));
        }
        public IEnumerator ShowHighlight(Vector2 position)
        {
            yield return ShowHighlight(position, startDelay, duration, fadeIn, fadeOut);
        }
        public IEnumerator ShowHighlight(Vector2 position, float startDelay, float duration, float fadeIn, float fadeOut)
        {
            sr.enabled = false;
            Color startCol = sr.color;
            transform.position = position;
            yield return new WaitForSeconds(startDelay);
            sr.enabled = true;
            yield return FadeIn(startCol, fadeOut);

            if (hasDuration == false)
            {
                yield break;
            }
            yield return new WaitForSeconds(duration);

            yield return FadeOut(startCol, fadeIn);
            sr.color = startCol;
            gameObject.SetActive(false);
        }

        private IEnumerator FadeColor(Color startCol, Color endCol, float duration)
        {
            float timer = 0;
            while (timer < duration)
            {
                sr.color = Color.Lerp(startCol, endCol, timer / duration);
                yield return null;
                timer += Time.deltaTime;
            }
            sr.color = endCol;
        }
        private IEnumerator FadeOut(Color startCol, float duration)
        {
            Color endColor = startCol;
            endColor.a = 0;
            yield return FadeColor(startCol, endColor, duration);
        }
        private IEnumerator FadeIn(Color startCol, float duration)
        {
            Color endColor = startCol;
            endColor.a = 0;
            yield return FadeColor(endColor, startCol, duration);
        }
    }
}