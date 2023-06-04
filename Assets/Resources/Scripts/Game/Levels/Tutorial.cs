using DG.Tweening;
using PawnsAndGuns.Controllers;
using PawnsAndGuns.Game.Cells;
using PawnsAndGuns.Game.Pawns;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace PawnsAndGuns.Game.Levels
{
    public class Tutorial : MonoBehaviour 
    {
        private void Start()
        {
            new PlayerController(Gameboard.Instance.PlayerTeam);
            SpawnStartArea();
            SpawnText(Content.TextTutorial,0, 0 - 3);
        }
        private TextMeshProUGUI SpawnText(TextMeshProUGUI Instance, int x, int y, float time = 5.0f, float floatSpeed = 2.0f)
        {
            TextMeshProUGUI text = Instantiate(Instance);
            text.transform.position = new Vector3(x, y, 0);
            text.transform.SetParent(Gameboard.Instance.Canvas.transform);

            return text;
        }

        private void SpawnStartArea()
        {
            List<Cell> cells = new List<Cell>()
            {
                Gameboard.Instance.SetCell<CheckPointCell>(0, 0),
                Gameboard.Instance.SetCell<Cell>(0, 1),
                Gameboard.Instance.SetCell<Cell>(0, 2),
                Gameboard.Instance.SetCell<Cell>(0, 3),
                Gameboard.Instance.SetCell<Cell>(0, 4),
            };

            TriggerCell cell = Gameboard.Instance.SetCell<TriggerCell>(0, 5);

            TextMeshProUGUI tutorialText = SpawnText(Content.TextWalls, cell.globalX, cell.globalY + 2);

            HideText(tutorialText);
            cell.pawnMoveInside = (Pawn pawn) =>
            {
                RemoveCells(cells);
                SpawnWallsTutorialArea(cell);
                ShowText(tutorialText);
            };

            cell.pawnMoveOutside = (Pawn pawn) =>
            {
                HideText(tutorialText);
                cell.transform.DOScale(Vector3.zero, .25f).OnComplete(() => { 
                    Destroy(cell.gameObject);
                });
            };

        }

        private void SpawnWallsTutorialArea(Cell lastTriggerCell)
        {
            List<Cell> cells = new List<Cell>(); 

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    Cell spawnedCell;
                    if (x == 1 && y > 0 && y < 4) spawnedCell = Gameboard.Instance.SetCell<WallCell>(lastTriggerCell.globalX - 1 + x, lastTriggerCell.globalY + 1 + y);
                    else spawnedCell = Gameboard.Instance.SetCell<Cell>(lastTriggerCell.globalX - 1 + x, lastTriggerCell.globalY + 1 + y);
                    cells.Add(spawnedCell);
                }
            }

            cells.AddRange(new Cell[] {
                Gameboard.Instance.SetCell<Cell>(lastTriggerCell.globalX, lastTriggerCell.globalY + 6),
                Gameboard.Instance.SetCell<Cell>(lastTriggerCell.globalX, lastTriggerCell.globalY + 7),
                Gameboard.Instance.SetCell<Cell>(lastTriggerCell.globalX, lastTriggerCell.globalY + 8),
                Gameboard.Instance.SetCell<Cell>(lastTriggerCell.globalX, lastTriggerCell.globalY + 9) });

            TriggerCell cell = Gameboard.Instance.SetCell<TriggerCell>(lastTriggerCell.globalX, lastTriggerCell.globalY + 10);

            TextMeshProUGUI text = SpawnText(Content.TextSpaceMove, cell.globalX, cell.globalY + 3);

            HideText(text);

            cell.pawnMoveInside = (Pawn pawn) =>
            {
                RemoveCells(cells);
                SpawnEmptyBlockJumpArea(cell.globalX, cell.globalY);
                ShowText(text);
            };
            cell.pawnMoveOutside = (Pawn pawn) =>
            {
                HideText(text);
                Destroy(cell);
            };
        }



        private void SpawnEmptyBlockJumpArea(int x, int y)
        {
            List<Cell> cells = new List<Cell>()
            {
                Gameboard.Instance.SetCell<Cell>(x, y + 1),
                Gameboard.Instance.SetCell<Cell>(x - 1, y + 3),
                Gameboard.Instance.SetCell<Cell>(x, y + 3),
                Gameboard.Instance.SetCell<Cell>(x + 1, y + 3),
                Gameboard.Instance.SetCell<Cell>(x - 1, y + 4),
                Gameboard.Instance.SetCell<Cell>(x, y + 4),
                Gameboard.Instance.SetCell<Cell>(x + 1, y + 4),
                Gameboard.Instance.SetCell<Cell>(x - 1, y + 6),
                Gameboard.Instance.SetCell<Cell>(x - 1, y + 7),
                Gameboard.Instance.SetCell<Cell>(x, y + 7),
                Gameboard.Instance.SetCell<Cell>(x + 1, y + 7)
            };
            TriggerCell cell = Gameboard.Instance.SetCell<TriggerCell>(x, y + 8);

            TextMeshProUGUI text = SpawnText(Content.TextPawnKill, cell.globalX, cell.globalY + 1);
            HideText(text);

            cell.pawnMoveInside = (Pawn pawn) => 
            {
                RemoveCells(cells);
                ShowText(text);
                SpawnPawnKill(cell.globalX, cell.globalY);
            };
            cell.pawnMoveOutside = (Pawn pawn) => 
            {
                HideText(text);
                Destroy(cell);
            };
        }
        private async void SpawnPawnKill(int x, int y)
        {
            List<Cell> cells = new List<Cell>()
            {
                Gameboard.Instance.SetCell<Cell>(x - 1, y + 1),
                Gameboard.Instance.SetCell<Cell>(x, y + 1),
                Gameboard.Instance.SetCell<Cell>(x + 1, y + 1),
                Gameboard.Instance.SetCell<Cell>(x - 1, y + 2),
                Gameboard.Instance.SetCell<Cell>(x, y + 2),
                Gameboard.Instance.SetCell<Cell>(x + 1, y + 2),
                Gameboard.Instance.SetCell<Cell>(x - 1, y + 3),
                Gameboard.Instance.SetCell<Cell>(x, y + 3),
                Gameboard.Instance.SetCell<Cell>(x + 1, y + 3),
            };

            await Task.Delay(TimeSpan.FromSeconds(.5));
            cells.Add(Gameboard.Instance.SetCell<Cell>(x, y + 4));

            TriggerCell cell = Gameboard.Instance.SetCell<TriggerCell>(x, y + 5);

            TextMeshProUGUI text = SpawnText(Content.TutorialFinished, cell.globalX, cell.globalY + 2);
            HideText(text);

            cell.pawnMoveInside = (Pawn pawn) => 
            {
                RemoveCells(cells);
                ShowText(text);
            };
            cell.pawnMoveOutside = (Pawn pawn) => 
            {
                HideText(text);
            };

            Gameboard.Instance.SetPawn(x, y + 2, Gameboard.Instance.EnemyTeam, Content.Pawn);


        }

        private void RemoveCells(List<Cell> cells)
        {
            foreach (Cell cell in cells)
            {
                Destroy(cell.gameObject);
            }
        }

        private void ShowText(TextMeshProUGUI text)
        {
            text.gameObject.SetActive(true);
            text.DOFade(.6f, .5f).OnComplete(() =>
            {
                var sequence = DOTween.Sequence();

                sequence.Append(text.transform.DOShakePosition(0.5f, strength: .1f, vibrato: 0)).SetEase(Ease.InSine).SetLoops(-1, LoopType.Yoyo);
            });
        }

        private void HideText(TextMeshProUGUI text)
        {
            text.DOKill(true);
            text.DOFade(0f, .5f).OnComplete(() => { text.gameObject.SetActive(false); });
        }

        private class TutorialStage
        {
            public Vector2Int Position;
            public TextMeshProUGUI TextInstance;
            public bool Used;
            public TutorialStage(Vector2Int position, TextMeshProUGUI textInstance)
            {
                Position = position;
                TextInstance = textInstance;
            }
        }
    }
}