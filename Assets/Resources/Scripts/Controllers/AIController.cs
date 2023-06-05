using PawnsAndGuns.Game;
using PawnsAndGuns.Game.Cells;
using PawnsAndGuns.Game.Pawns;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using XCore.Extensions;

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
        private Pawn _currentPawn;
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

            Pawn pawn = _pawns[0];
            if (pawn == null || pawn.IsDestroyed() || pawn.Killed || pawn.cell == null)
            {
                UpdateControllablePawns();
                _pawns.Remove(pawn);
                return;
            };

            _currentPawn = pawn;

            UpdateCurrentPawn();
        }

        private void UpdateCurrentPawn()
        {
            Cell cell;
            if (CanHitPawn(_currentPawn, out cell))
            {
                if (cell is TriggerCell)
                {
                    if ((cell as TriggerCell).ReactsOn != _currentPawn.Team) return;
                };
                if (cell is CheckPointCell) return;
                _canMove = false;
                _currentPawn.MoveTo(cell.globalX, cell.globalY);
                _pawns.Remove(_currentPawn);
            }
            else if (CanMoveCloseToEnemyPawn(_currentPawn, out cell))
            {
                if (cell is TriggerCell)
                {
                    if ((cell as TriggerCell).ReactsOn != _currentPawn.Team) return;
                };
                if (cell is CheckPointCell) return;
                if (cell == _currentPawn.cell) return;
                _canMove = false;
                _currentPawn.MoveTo(cell.globalX, cell.globalY);
                _pawns.Remove(_currentPawn);
            }
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
                for(int k = 1; k < moveWay.Length; k++)
                {
                    int moveX = pawn.cell.globalX + moveWay.Way.x * k;
                    int moveY = pawn.cell.globalY + moveWay.Way.y * k;
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
            }
            cell = null;
            return false;
        }

        private bool CanMoveCloseToEnemyPawn(Pawn pawn, out Cell outCell)
        {
            Pawn king = Gameboard.Instance.King;
            outCell = null;
            if (king == null) return false;
            if (king.cell == null) return false;

            outCell = NearestToKing(pawn);

            return outCell != null;
        }

        private Cell NearestToKing(Pawn pawn)
        {
            float minDistance = 999999f;
            Vector2Int kingPosition = Gameboard.Instance.King.cell.Position;

            Cell outCell = null;

            for (int i = 0; i < pawn.MoveWays.Count; i++)
            {
                MoveWay moveWay = pawn.MoveWays[i];
                for (int k = 1; k < moveWay.Length + 1; k++)
                {
                    int moveX = pawn.cell.globalX + moveWay.Way.x * k;
                    int moveY = pawn.cell.globalY + moveWay.Way.y * k;

                    Cell cell = Gameboard.Instance.GetCell(moveX, moveY);
                    if (cell == null) continue;
                    if (!pawn.CanMoveAt(moveX, moveY)) continue;
                    if (outCell == null)
                    {
                        outCell = cell;
                    }
                    else
                    {
                        float distance = cell.Position.SquareDistance(kingPosition);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            outCell = cell;
                        }
                    }
                }
            }

            return outCell;
        }

        private Cell NearestToKing(List<MoveWay> moveWays, Cell based)
        {
            Vector2Int kingPosition = Gameboard.Instance.King.cell.Position;

            Cell outCell = based;
            float minDistance = based.Position.SquareDistance(kingPosition);

            for (int i = 0; i < moveWays.Count; i++)
            {
                MoveWay moveWay = moveWays[i];
                for (int k = 1; k < moveWay.Length + 1; k++)
                {
                    int moveX = based.globalX + moveWay.Way.x * k;
                    int moveY = based.globalY + moveWay.Way.y * k;

                    Cell cell = Gameboard.Instance.GetCell(moveX, moveY);
                    if (cell == null) continue;
                    if (outCell == null)
                    {
                        outCell = cell;
                    }
                    else
                    {
                        float distance = cell.Position.SquareDistance(kingPosition);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            outCell = cell;
                        }
                    }
                }
            }

            return outCell;
        }

        private void MoveRandomly(Pawn pawn)
        {
            MoveWay moveWay = pawn.MoveWays[Random.Range(0, pawn.MoveWays.Count)];

            int length = Random.Range(0, moveWay.Length);

            if (pawn.CanMoveAt(moveWay.Way.x * length + pawn.cell.globalX, moveWay.Way.y * length + pawn.cell.globalY))
            {
                pawn.MoveTo(moveWay.Way.x * length + pawn.cell.globalX, moveWay.Way.y * length + pawn.cell.globalY);
            }
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Team;

            if (_currentPawn != null)
            {
                Gizmos.DrawWireCube(_currentPawn.transform.position, new Vector3(1, 1));

                Cell cell;

                if (CanHitPawn(_currentPawn, out cell))
                {
                    Gizmos.color = cell.Pawn.Team;
                    Gizmos.DrawWireCube(cell.transform.position, new Vector3(1, 1));
                }
                else if (CanMoveCloseToEnemyPawn(_currentPawn, out cell))
                {
                    Gizmos.color = Team - new Color(0, 0, 0, .5f);
                    Gizmos.DrawCube(cell.transform.position, new Vector3(1, 1, 1));
                }
            }
        }
    }
}