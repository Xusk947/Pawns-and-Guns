using PawnsAndGuns.Game;
using PawnsAndGuns.Game.Cells;
using PawnsAndGuns.Game.Pawns;
using UnityEngine;

namespace PawnsAndGuns.Controllers
{
    public class PlayerController : Controller
    {
        public Color Team { get => _team; set => _team = value; }
        public bool CanMove { get => _canMove; set => _canMove = value; }

        private Color _team;

        private Cell _selected;
        private bool _canMove = true;

        public PlayerController(Color team)
        {
            Team = team;
            Pawn.PawnMoveEvent += UpdateKingState;
            Controller.SetController(this);
        }

        public void UpdateMovement()
        {
            if (!_canMove) return;
            if (Input.GetMouseButtonDown(0)) // Check for left mouse button click
            {
                Vector2Int mouseTile = Gameboard.MouseTile;
                bool canSelectNewTile = true;

                if (_selected != null)
                {
                    _selected.Deselect();
                    if (_selected.Pawn != null && _selected.Pawn.CanMoveAt(mouseTile.x, mouseTile.y))
                    {
                        _selected.Pawn.MoveTo(mouseTile.x, mouseTile.y);
                        canSelectNewTile = false;
                    }
                    _selected = null;
                }

                Cell cell = Gameboard.Instance.GetCell(mouseTile.x, mouseTile.y);

                if (canSelectNewTile && cell != null)
                {
                    if (cell.Pawn == null) return;
                    if (cell.Pawn.Team != _team) return;
                    cell.Select();
                    _selected = cell;
                }
            }
        }

        private void UpdateKingState(Pawn pawn)
        {
            if (pawn != Gameboard.Instance.King) return;
        }
    }
}