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

        [SerializeField]
        private PawnSO _type;
        public Color Team
        {
            get { return _team; }
            set { SetTeam(value); }
        }

        public Cell cell;

        private List<GameObject> _highlighters;
        private SpriteRenderer _spriteRenderer;

        private List<MoveWay> _moveWays;
        private Color _team;

        private void Awake()
        {
            _highlighters = new List<GameObject>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = _type.Sprite;
            _moveWays = _type.Moves;
        }

        public virtual void OnSelect()
        {
            FollowCamera.Instance.target = gameObject;
            Highlight();
        }

        public bool CanMoveAt(int x, int y)
        {
            foreach(MoveWay moveWay in _moveWays)
            {
                for (int i = 1; i < moveWay.Length + 1; i++)
                {
                    int moveX = cell.x + moveWay.Way.x * i;
                    int moveY = cell.y + moveWay.Way.y * i;


                    Cell targetCell = Gameboard.Instance.GetCell(x, y);
                    Cell checkedCell = Gameboard.Instance.GetCell(moveX, moveY);

                    if (targetCell == null) break;
                    if (x != moveX || y != moveY)
                    {
                        if (checkedCell != null && checkedCell.Pawn != null)
                        {
                            break;
                        }
                        continue;
                    }

                    if (!IsAvailableCell(targetCell)) continue;
                    if (targetCell.Pawn == null && moveWay.CanEatOnly) continue;
                    return true;
                }
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
        private void Highlight()
        {
            _spriteRenderer.material.color = new Color(_spriteRenderer.material.color.r, _spriteRenderer.material.color.g, _spriteRenderer.material.color.b, .5f);

            for (int i = 0; i < _moveWays.Count; i++) { 
                MoveWay moveWay = _moveWays[i];
                for(int k = 1; k < moveWay.Length + 1; k++)
                {
                    Cell moveCell = Gameboard.Instance.GetCell(cell.x + moveWay.Way.x * k, cell.y + moveWay.Way.y * k);

                    if (!IsAvailableCell(moveCell)) continue;
                    if (moveCell.Pawn == null && moveWay.CanEatOnly) continue;
                    if (moveCell.Pawn != null)
                    {
                        if (moveCell.Pawn.Team != Team)
                        {
                            SpawnHighlightGameObject(moveCell.x, moveCell.y, moveCell.Pawn.Team - new Color(0, 0, 0, .5f));
                        }
                        break;
                    } else
                    {
                        SpawnHighlightGameObject(moveCell.x, moveCell.y, Team - new Color(0, 0, 0, .5f));
                    }
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
            spriteRenderer.sprite = Content.Highlight;
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