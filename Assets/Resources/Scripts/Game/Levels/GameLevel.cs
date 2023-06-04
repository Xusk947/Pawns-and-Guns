using PawnsAndGuns.Controllers;
using PawnsAndGuns.Game.Cells;
using PawnsAndGuns.Game.Pawns;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using XCore.Extensions;

namespace PawnsAndGuns.Game.Levels
{
    public class GameLevel : MonoBehaviour
    {
        private void Start()
        {
            new PlayerController(Gameboard.Instance.PlayerTeam);
            new AIController(Gameboard.Instance.EnemyTeam);
            CreateSpawn();
        }

        private void CreateSpawn()
        {
            Gameboard.Instance.SetCell<Cell>(-1, -1);
            Gameboard.Instance.SetCell<Cell>(-1, 0);
            Gameboard.Instance.SetCell<Cell>(-1, 1);
            Gameboard.Instance.SetCell<Cell>(0, 1);
            Gameboard.Instance.SetCell<Cell>(0, -1);
            Gameboard.Instance.SetCell<Cell>(+1, +1);
            Gameboard.Instance.SetCell<Cell>(+1, 0);
            Gameboard.Instance.SetCell<Cell>(+1, -1);
            Gameboard.Instance.SetCell<CheckPointCell>(0, 0);
            TriggerCell cell = Gameboard.Instance.SetCell<TriggerCell>(0, 2);
            cell.ReactsOn = Gameboard.Instance.PlayerTeam;
            cell.pawnMoveInside = (Pawn pawn) => {

                List<Pawn> pawnsToSpawn = new List<Pawn>();

                for(int i = 0; i < UnityEngine.Random.Range(1, 3); i++)
                {
                    Pawn pawnToSpawn = Instantiate(Content.playablePawns.Random());
                    pawnsToSpawn.Add(pawnToSpawn);
                }
                int size = UnityEngine.Random.Range(5, 10);
                SpawnRect(cell.globalX + size / 2, cell.globalY + size / 2 + 1, size, pawnsToSpawn);
            };

            cell.pawnMoveOutside = (Pawn pawn) => { Destroy(cell.gameObject); };
        }

        private async void SpawnRect(int x, int y, int size, List<Pawn> pawnToSpawn = default)
        {
            for(int i = -size / 2; i < size / 2; i++)
            {
                for(int k = -size / 2; k < size / 2; k++)
                {
                    Gameboard.Instance.SetCell<Cell>(x + i, y + k);
                }
            }
            await Task.Delay(TimeSpan.FromSeconds(.25f));
            if (pawnToSpawn != null)
            {
                for(int i = 0; i < pawnToSpawn.Count; i++)
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
            
        }
    }
}