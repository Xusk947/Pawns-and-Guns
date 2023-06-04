using PawnsAndGuns.Controllers;
using PawnsAndGuns.Game.Cells;
using PawnsAndGuns.Game.Pawns;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using XCore.Extensions;

namespace PawnsAndGuns.Game.Levels
{
    public class GameLevel : MonoBehaviour
    {
        private List<Cell> _lastSpawnedCells;
        private Vector2Int _nextTriggerCellPosition;
        private bool CanSpawn = true;

        private void Start()
        {
            new PlayerController(Gameboard.Instance.PlayerTeam);
            new AIController(Gameboard.Instance.EnemyTeam);
            CreateSpawn();
        }

        private void CreateSpawn()
        {
            _lastSpawnedCells = new List<Cell>()
            {
                Gameboard.Instance.SetCell<Cell>(-1, -1),
                Gameboard.Instance.SetCell<Cell>(-1, 0),
                Gameboard.Instance.SetCell<Cell>(-1, 1),
                Gameboard.Instance.SetCell<Cell>(0, 1),
                Gameboard.Instance.SetCell<Cell>(0, -1),
                Gameboard.Instance.SetCell<Cell>(+1, +1),
                Gameboard.Instance.SetCell<Cell>(+1, 0),
                Gameboard.Instance.SetCell<Cell>(+1, -1),
                Gameboard.Instance.SetCell<CheckPointCell>(0, 0),
            };

            _nextTriggerCellPosition = new Vector2Int(0, 2);
        }

        private void SpawnBaseRoom(List<Cell> cells)
        {
            CanSpawn = false;
            TriggerCell cell = Gameboard.Instance.SetCell<TriggerCell>(_nextTriggerCellPosition.x, _nextTriggerCellPosition.y);
            cell.ReactsOn = Gameboard.Instance.PlayerTeam;
            cell.SpriteRenderer.color = cell.ReactsOn;
            cell.pawnMoveInside = (Pawn pawn) => {
                RemoveCells(cells);
                List<Pawn> pawnsToSpawn = new List<Pawn>();

                for (int i = 0; i < UnityEngine.Random.Range(1, 3); i++)
                {
                    Pawn pawnToSpawn = Instantiate(Content.playablePawns.Random());
                    pawnsToSpawn.Add(pawnToSpawn);
                }
                int size = UnityEngine.Random.Range(5, 10);
                _lastSpawnedCells = SpawnRect(cell.globalX + size / 2, cell.globalY + size / 2 + 1, size, pawnsToSpawn);

                Vector2Int nextTriggerCellPos = new Vector2Int(cell.globalX + UnityEngine.Random.Range(0, size), cell.globalY + size);

                if (Gameboard.Instance.GetCell(nextTriggerCellPos.x, nextTriggerCellPos.y) != null)
                {
                    nextTriggerCellPos = new Vector2Int(cell.globalX + UnityEngine.Random.Range(0, size), cell.globalY + size + 1);
                }
                _nextTriggerCellPosition = nextTriggerCellPos;
            };

            cell.pawnMoveOutside = (Pawn pawn) => 
            { 
                Destroy(cell.gameObject);
                CanSpawn = true;
            };
        }

        private void RemoveCells(List<Cell> cells)
        {
            foreach (Cell cell in cells)
            {
                Destroy(cell.gameObject);
            }
        }
        private List<Cell> SpawnRect(int x, int y, int size, List<Pawn> pawnToSpawn = default)
        {
            List<Cell> cells = new List<Cell>();
            for(int i = -size / 2; i < size / 2; i++)
            {
                for(int k = -size / 2; k < size / 2; k++)
                {
                    cells.Add(Gameboard.Instance.SetCell<Cell>(x + i, y + k));
                }
            }

            SpawnPawns(x, y, size, pawnToSpawn);

            return cells;
        }

        private async void SpawnPawns(int x, int y, int size, List<Pawn> pawnToSpawn)
        {
            await Task.Delay(TimeSpan.FromSeconds(.25f));
            if (pawnToSpawn != null)
            {
                for (int i = 0; i < pawnToSpawn.Count; i++)
                {
                    await Task.Delay(TimeSpan.FromSeconds(.1f));
                    Pawn pawn = pawnToSpawn[i];
                    Cell spawnCell = Gameboard.Instance.GetCell(x + UnityEngine.Random.Range(-size / 2, size / 2), y + UnityEngine.Random.Range(-size / 2, size / 2));
                    spawnCell.Pawn = pawn;
                    pawn.Team = Gameboard.Instance.EnemyTeam;
                }
            }
        }
        private void Update()
        {
            if (CanSpawn)
            {
                if (!Pawn.Pawns.ContainsKey(Gameboard.Instance.EnemyTeam) || Pawn.Pawns[Gameboard.Instance.EnemyTeam].Count <= 0)
                {
                    SpawnBaseRoom(_lastSpawnedCells);
                }
            }
        }
    }
}