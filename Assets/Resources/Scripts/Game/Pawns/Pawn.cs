using DG.Tweening;
using PawnsAndGuns.Game.Cells;
using System.Collections.Generic;
using UnityEngine;

namespace PawnsAndGuns.Game.Pawns
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class Pawn : MonoBehaviour
    {
        public delegate void OnPawnMove(Pawn pawn);
        public static event OnPawnMove PawnMove;

        public static void MovePawn(Pawn pawn)
        {
            PawnMove?.Invoke(pawn);
        }

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
        private Color _team = default;
        private AudioSource _audioSource;

        private void Awake()
        {
            _highlighters = new List<GameObject>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = _type.Sprite;
            _spriteRenderer.sortingLayerName = "Pawns";
            _moveWays = _type.Moves;
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.spatialBlend = 0f;
        }

        public virtual void OnSelect()
        {
            FollowCamera.Instance.target = gameObject;
            Highlight();
        }

        public bool CanMoveAt(int x, int y)
        {
            Cell targetCell = Gameboard.Instance.GetCell(x, y);

            foreach(MoveWay moveWay in _moveWays)
            {
                for (int i = 1; i < moveWay.Length + 1; i++)
                {
                    int moveX = cell.globalX + moveWay.Way.x * i;
                    int moveY = cell.globalY + moveWay.Way.y * i;
                    Cell cellToCheck = Gameboard.Instance.GetCell(moveX, moveY);

                    if (targetCell == null) break;
                    // Check if cell has a Pawn, then broke the cell walker
                    if (x != moveX || y != moveY)
                    {
                        if (cellToCheck != null && cellToCheck.Pawn != null)
                        {
                            break;
                        }
                        continue;
                    }
                    // If target cell doesn't have a Pawn and pawn can only eat on this direction continue
                    if (targetCell.Pawn == null && moveWay.CanEatOnly) return false;
                    if (IsAvailableCell(targetCell)) return true;
                }
            }
            return false;
        }

        public void MoveTo(int x, int y)
        {
            GameboardController.Instance.CurrentController.CanMove = false;
            this.cell.Pawn = null;
            Cell cell = Gameboard.Instance.GetCell(x, y);

            var sequence = DOTween.Sequence();


            _audioSource.clip = cell.Pawn == null ? Content.AudioClipMove : Content.AudioClipKill;
            _audioSource.PlayDelayed(.1f);

            sequence
                .Join(transform.DOMove(cell.transform.position, .5f).SetEase(Ease.InBack))
                .Join(transform.DOScale(new Vector3(1, 1, 1), .5f).SetEase(Ease.InFlash))
                .OnComplete(() => {
                    if (cell.Pawn != null)
                    {
                        cell.DestroyPawn(this);
                    } else
                    {
                        GameboardController.Instance.CurrentController.CanMove = true;
                        cell.Pawn = this;
                    }
                    MovePawn(this);
                    OnDeselect();
                });
        }

        public virtual void OnDeselect()
        {
            _spriteRenderer.material.DOColor(new Color(_spriteRenderer.material.color.r, _spriteRenderer.material.color.g, _spriteRenderer.material.color.b, 1f), .25f);
            ClearHighlights();
        }
        private void Highlight()
        {
            _spriteRenderer.material.DOColor(new Color(_spriteRenderer.material.color.r, _spriteRenderer.material.color.g, _spriteRenderer.material.color.b, .5f), .25f);

            for (int i = 0; i < _moveWays.Count; i++) { 
                MoveWay moveWay = _moveWays[i];
                for(int k = 1; k < moveWay.Length + 1; k++)
                {
                    Cell moveCell = Gameboard.Instance.GetCell(cell.globalX + moveWay.Way.x * k, cell.globalY + moveWay.Way.y * k);
                    // check if cell is available 
                    if (IsAvailableCell(moveCell))
                    {
                        if (moveCell.Pawn == null && moveWay.CanEatOnly) continue;
                        if (moveCell.Pawn != null)
                        {
                            if (moveCell.Pawn.Team != Team)
                            {
                                SpawnHighlightGameObject(moveCell.globalX, moveCell.globalY, moveCell.Pawn.Team - new Color(0, 0, 0, .5f));
                            }
                            break;
                        } else
                        {
                            SpawnHighlightGameObject(moveCell.globalX, moveCell.globalY, Team - new Color(0, 0, 0, .5f));
                        }
                    }
                }
            }
        }

        private bool IsAvailableCell(Cell cell)
        {
            if (cell == null) return false;
            return cell.CanAcceptPawn(this);
        }

        private GameObject SpawnHighlightGameObject(float x, float y, Color color)
        {
            GameObject highlight = new GameObject("highlight");
            highlight.transform.SetParent(transform);
            highlight.transform.position = new Vector3(x, y, transform.position.z);
            highlight.transform.localScale = Vector3.zero;
            highlight.transform.DOScale(new Vector3(.5f, .5f, .5f), .25f);

            SpriteRenderer spriteRenderer = highlight.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = Content.Highlight;
            spriteRenderer.material.color = new Color(color.r, color.g, color.b, .2f);
            spriteRenderer.sortingLayerName = "FX";

            _highlighters.Add(highlight);
            return highlight;
        }

        private void ClearHighlights()
        {
            foreach (GameObject highlight in _highlighters)
            {
                highlight.transform.DOScale(Vector3.zero, 0.25f).OnComplete(() => { 
                    Destroy(highlight);
                });
            }
            _highlighters.Clear();
        }
        
        private void SetTeam(Color team)
        {
            if (Pawns.ContainsKey(_team))
            {
                Pawns[_team].Remove(this);
            }

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