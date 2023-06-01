using PawnsAndGuns.Game;
using PawnsAndGuns.Game.Cells;
using PawnsAndGuns.Game.Pawns;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace PawnsAndGuns.Controllers
{
    public class AIController : Controller
    {
        public Color Team { get => _team; set => _team = value; }
        public bool CanMove { get => _canMove; set => _canMove = value; }

        private Color _team;
        private bool _canMove = true;

        private List<Pawn> _pawns;
        private float MoveTime;
        private float MoveTimer = 0;
        public AIController(Color team, float moveTime = 1.5f)
        {
            _team = team;
            _pawns = new List<Pawn>();

            MoveTime = moveTime;
            MoveTimer = MoveTime;

            UpdateControllablePawns();
            Controller.SetController(this);
        }

        public void UpdateMovement()
        {
            MoveTimer -= Time.deltaTime;
            if (MoveTimer > 0) return;
            if (!_canMove) return;

            MoveTimer = MoveTime;

            if (_pawns.Count <= 0) UpdateControllablePawns();
            if (_pawns.Count == 0) return;
            _canMove = false;

            Pawn pawn = _pawns[0];

            if (pawn == null || pawn.IsDestroyed() || pawn.Killed)
            {
                UpdateControllablePawns();
                _pawns.Remove(pawn);
                return;
            };

            Cell outCell;
            // Try to hit any aviable Pawn
            if (CanHitPawn(pawn, out outCell))
            {
                pawn.MoveTo(outCell.globalX, outCell.globalY);
            } else if (CanMoveCloseToEnemyPawn(pawn, out outCell)) 
            {
                pawn.MoveTo(outCell.globalX, outCell.globalY);
            } else
            {
                MoveRandomly(pawn);
            }

            _pawns.Remove(pawn);
        }

        private void UpdateControllablePawns()
        {
            if (Pawn.Pawns.ContainsKey(Team))
            {
                _pawns = new List<Pawn>(Pawn.Pawns[Team]);
            }
        }
        private bool CanHitPawn(Pawn pawn, out Cell cell)
        {
            for(int i = 0; i < pawn.MoveWays.Count; i++)
            {
                MoveWay moveWay = pawn.MoveWays[i];
                int moveX = pawn.cell.globalX + moveWay.Way.x;
                int moveY = pawn.cell.globalY + moveWay.Way.y;

                if (pawn.CanMoveAt(moveX, moveY))
                {
                    Cell moveCell = Gameboard.Instance.GetCell(moveX, moveY);
                    if (moveCell.Pawn != null && moveCell.Pawn.Team != pawn.Team)
                    {
                        cell = moveCell;
                        return true;
                    }
                }
            }
            cell = null;
            return false;
        }

        private bool CanMoveCloseToEnemyPawn(Pawn pawn, out Cell outCell)
        {
            int maxX = 0;
            int maxY = 0;

            for(int i = 0; i < pawn.MoveWays.Count; i++)
            {
                MoveWay moveWay = pawn.MoveWays[i];
                maxX = moveWay.Way.x > maxX ? moveWay.Way.x : maxX;
                maxY = moveWay.Way.y > maxY ? moveWay.Way.y : maxY;
            }

            Cell closestCell = null;

            for(int x = -maxX * 2; x < maxX * 2; x++)
            {
                for (int y = -maxY * 2; y < maxY * 2; y++)
                {
                    Cell cell = Gameboard.Instance.GetCell(pawn.cell.globalX, pawn.cell.globalY);
                    if (cell.Pawn != null && cell.Pawn.Team != pawn.Team)
                    {
                        if (closestCell == null) closestCell = cell;
                        else if (Vector2Int.Distance(pawn.cell.Position, cell.Position) < Vector2Int.Distance(pawn.cell.Position, closestCell.Position))
                        {
                            closestCell = cell;
                        }
                    }
                }
            }

            if (closestCell != null)
            {
                outCell = closestCell;
                return true;
            }

            outCell = null;
            return false;
        }

        private void MoveRandomly(Pawn pawn)
        {
            MoveWay moveWay = pawn.MoveWays[Random.Range(0, pawn.MoveWays.Count)];

            pawn.MoveTo(moveWay.Way.x + pawn.cell.globalX, moveWay.Way.y + pawn.cell.globalY);
        }
    }
}