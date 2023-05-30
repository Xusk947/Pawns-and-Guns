using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using PawnsAndGuns.Pawns;
using UnityEngine;

namespace PawnsAndGuns.Game
{
    public class Cell : MonoBehaviour
    {
        public bool Selected;
        public int x;
        public int y;
        public SpriteRenderer SpriteRenderer { get; private set; }
        public Pawn Pawn
        {
            get { return _pawn; }
            set
            {
                SetPawn(value);
            }
        }
        private Pawn _pawn;
        private void Awake()
        {
            SpriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }

        public void Select()
        {
            Selected = true;
            if (Pawn == null) return;
            Pawn.OnSelect();
        }

        public void Deselect()
        {
            Selected = false;
            if (Pawn == null) return;
            Pawn.OnDeselect();
        }

        public void DestroyPawn(Pawn pawn)
        {
            ParticleSystem particleSystem = Instantiate(Content.PawnDeath);
            ParticleSystem.MainModule settings = particleSystem.main;

            settings.startColor = new ParticleSystem.MinMaxGradient(_pawn.Team);
            particleSystem.transform.position = _pawn.transform.position - new Vector3(0, .5f, 0);

            var sequence = DOTween.Sequence();
            float xPath = transform.position.x + 10 * Random.Range(-1, 1);

            sequence
                .Join(_pawn.transform.DOShakeRotation(1.5f).SetEase(Ease.InOutBack))
                .Join(_pawn.transform.DOShakePosition(1.5f))
                .Join(_pawn.transform.DOScale(Vector3.zero, 1.5f).SetEase(Ease.OutBack))
            .OnComplete(() =>
            {
                    if (pawn != null)
                    {


                        Destroy(_pawn.gameObject);
                        Pawn = pawn;
                    }
                    GameboardController.Instance.CurrentController.CanMove = true;
                });
            sequence.Play();
        }

        private void SetPawn(Pawn pawn)
        {
            _pawn = pawn;
            if (pawn == null) return;
            pawn.transform.SetParent(transform);
            pawn.cell = this;
            pawn.transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        }
    }
}