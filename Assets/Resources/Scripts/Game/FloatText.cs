using DG.Tweening;
using TMPro;
using UnityEngine;

namespace PawnsAndGuns.Game
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class FloatText : MonoBehaviour
    {
        private static int _index = 0;
        public TextMeshProUGUI Text { get; private set; }

        private float _timer;
        private bool spawned = false;

        public static FloatText Spawn(float destroyTime = 1.5f)
        {
            FloatText floatText = Instantiate(Content.FloatText);
            floatText._timer = destroyTime;
            _index++;
            return floatText;
        }

        private void Awake()
        {
            Text = GetComponent<TextMeshProUGUI>();
            transform.localScale = Vector3.zero;
            transform.SetParent(Gameboard.Instance.Canvas.transform, false);
            gameObject.SetActive(false);
        }

        public void Activate()
        {
            gameObject.SetActive(true);
            var sequence = DOTween.Sequence();

            float duration = .25f;

            sequence.Join(transform.DOScale(new Vector3(1, 1, 1), duration))
                .Join(transform.DOShakeRotation(duration)).SetEase(Ease.InCubic).OnComplete(() => { spawned = true; });

            sequence.Play();
        }

        private void Update()
        {
            if (!spawned) return;
            _timer -= Time.deltaTime;
            if (_timer <= 0) {
                var sequence = DOTween.Sequence();

                float duration = .25f;

                sequence.Join(transform.DOScale(Vector3.zero, duration))
                    .Join(transform.DOShakeRotation(duration)).SetEase(Ease.OutCubic).OnComplete(() => { Destroy(gameObject); });

                sequence.Play();
                _timer = 9999f;
            }
        }

        /**
         * <summary>
         * Set the font Size
         * value from 0 to 1
         * <paramref name="fontSize"/> set the text size
         * </summary>
         */
        public void SetFontSize(float fontSize)
        {
            Text.fontSize = fontSize;
        }
    }
}