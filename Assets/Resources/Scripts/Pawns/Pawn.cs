using DG.Tweening;
using PawnsAndGuns.Game;
using System.Collections.Generic;
using UnityEngine;

namespace PawnsAndGuns.Pawns
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class Pawn : MonoBehaviour
    {
        public static Dictionary<Color, List<Pawn>> Pawns = new Dictionary<Color, List<Pawn>>();

        public Sprite highlightSprite;
        public ParticleSystem PawnDeath;

        public Color Team
        {
            get { return _team; }
            set { SetTeam(value); }
        }

        public Cell cell;

        private List<GameObject> _highlighters;
        private SpriteRenderer _spriteRenderer;

        private List<Vector2Int> _moves;
        private Color _team;

        private void Awake()
        {
            _highlighters = new List<GameObject>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _moves = GetMoves();
        }

        public virtual void OnSelect()
        {
            Highlight();
        }

        public bool CanMoveAt(int x, int y)
        {
            foreach(Vector2Int move in _moves)
            {
                int moveX = cell.x + move.x;
                int moveY = cell.y + move.y;

                if (x != moveX || y != moveY) continue;
                Cell targetCell = Gameboard.Instance.GetCell(x, y);
                if (IsAvailableCell(targetCell)) return true;
            }
            return false;
        }

        public void MoveTo(int x, int y)
        {
            GameboardController.Instance.CurrentController.CanMove = false;
            this.cell.Pawn = null;
            Cell cell = Gameboard.Instance.GetCell(x, y);

            transform.DOMove(cell.transform.position, .5f).SetEase(Ease.InBack).OnComplete(() => {
                if (cell.Pawn != null)
                {
                    cell.DestroyPawn(this);
                } else
                {
                    GameboardController.Instance.CurrentController.CanMove = true;
                    cell.Pawn = this;
                }
                OnDeselect();
            });
        }

        public virtual void OnDeselect()
        {
            _spriteRenderer.material.color = new Color(_spriteRenderer.material.color.r, _spriteRenderer.material.color.g, _spriteRenderer.material.color.b, 1f);
            ClearHighlights();
        }

        protected virtual List<Vector2Int> GetMoves()
        {
            return new List<Vector2Int> { 
                new Vector2Int(1, 0),
                new Vector2Int(0, 1),
                new Vector2Int(-1, 0),
                new Vector2Int(0, -1),
                new Vector2Int(1, 3),
            };
        }

        private void Highlight()
        {
            _spriteRenderer.material.color = new Color(_spriteRenderer.material.color.r, _spriteRenderer.material.color.g, _spriteRenderer.material.color.b, .5f);

            for (int i = 0; i < _moves.Count; i++) { 
                Vector2Int move = _moves[i];

                Cell moveCell = Gameboard.Instance.GetCell(cell.x + move.x, cell.y + move.y);

                if (!IsAvailableCell(moveCell)) continue;
                if (moveCell.Pawn != null && moveCell.Pawn.Team != Team)
                {
                    SpawnHighlightGameObject(moveCell.x, moveCell.y, moveCell.Pawn.Team - new Color(0, 0, 0, .5f));
                } else
                {
                    SpawnHighlightGameObject(moveCell.x, moveCell.y, Team - new Color(0, 0, 0, .5f));
                }
            }
        }

        private bool IsAvailableCell(Cell cell)
        {
            if (cell == null) return false;
            if (cell.Pawn != null && cell.Pawn.Team == Team) return false;

            return true;
        }

        private GameObject SpawnHighlightGameObject(float x, float y, Color color)
        {
            GameObject highlight = new GameObject("highlight");
            highlight.transform.SetParent(transform);
            highlight.transform.position = new Vector3(x, y, transform.position.z);

            SpriteRenderer spriteRenderer = highlight.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = highlightSprite;
            spriteRenderer.material.color = color;

            _highlighters.Add(highlight);
            return highlight;
        }

        private void ClearHighlights()
        {
            foreach (GameObject highlight in _highlighters)
            {
                Destroy(highlight);
            }
            _highlighters.Clear();
        }
        
        private void SetTeam(Color team)
        {
            _team = team; 
            _spriteRenderer.material.color = team;
            if (Pawns.ContainsKey(team))
            {
                Pawns[team].Add(this);
            } else
            {
                Pawns[team] = new List<Pawn>() { this };
            }
        }

        private void OnDestroy()
        {
            Pawns[Team].Remove(this);
        }
    }

}