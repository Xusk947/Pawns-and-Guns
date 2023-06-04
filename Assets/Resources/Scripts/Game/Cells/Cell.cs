using DG.Tweening;
using PawnsAndGuns.Controllers;
using PawnsAndGuns.Game.Pawns;
using UnityEngine;

namespace PawnsAndGuns.Game.Cells
{
    public class Cell : MonoBehaviour
    {
        public bool Selected;
        public Chunk Chunk;
        public bool Spawned;
        public Vector2Int Position { get { return new Vector2Int(x, y); } }

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

            transform.localScale = Vector3.zero;
            transform.DOScale(new Vector3(1, 1, 1), .25f).OnComplete(() => { Spawned = true; });
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