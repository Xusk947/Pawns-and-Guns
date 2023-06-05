using DG.Tweening;
using TMPro;
using UnityEngine;

namespace PawnsAndGuns.Game
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class Score : MonoBehaviour
    {
        public static Score Instance { get; private set; }
        private TextMeshProUGUI _text;
        private Sequence _sequence;
        private void Awake()
        {
            Instance = this;
            _text = GetComponent<TextMeshProUGUI>();
        }

        public void SetScore(int score)
        {
            _text.text = score.ToString();
            float randomScale = Random.Range(.5f, 1.5f);
            _text.transform.localScale = new Vector3(1 + randomScale, 1 + randomScale, 1);
            _text.color = Gameboard.Instance.PlayerTeam;

            if (_sequence != null) _sequence.Kill();
            _sequence = DOTween.Sequence();

            float duration = .25f;
            
            _sequence.Join(_text.transform.DOScale(new Vector3(1, 1, 1), duration))
                .Join(_text.transform.DOShakeRotation(duration))
                .Join(_text.DOColor(Color.white, duration)).SetEase(Ease.InSine);

            _sequence.Play();
        }
    }
}