using DG.Tweening;
using PawnsAndGuns.Game.Pawns;
using UnityEngine;

namespace PawnsAndGuns.Game.Cells
{
    public class Cell : MonoBehaviour
    {
        public bool Selected;
        public Chunk Chunk;
        public int x;
        public int y;

        public int globalX { get { return x + Chunk.x * Chunk.CHUNK_SIZE.x; } }
        public int globalY { get { return y + Chunk.y * Chunk.CHUNK_SIZE.y; } }

        public SpriteRenderer SpriteRenderer { get; protected set; }
        public Pawn Pawn
        {
            get { return _pawn; }
            set
            {
                SetPawn(value);
            }
        }
        protected Pawn _pawn;
        protected virtual void Awake()
        {
            SpriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            SpriteRenderer.sortingLayerName = "Board";
            SpriteRenderer.sprite = Content.TileSprite;
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

        public virtual bool CanAcceptPawn(Pawn pawn)
        {
            if (Pawn != null && Pawn.Team == pawn.Team) return false;
            return true;
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

        protected virtual void SetPawn(Pawn pawn)
        {
            _pawn = pawn;
            if (pawn == null) return;
            pawn.transform.SetParent(transform);
            pawn.cell = this;
            pawn.transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        }
    }
}