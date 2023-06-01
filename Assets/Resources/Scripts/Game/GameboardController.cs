using PawnsAndGuns.Controllers;
using PawnsAndGuns.Game.Cells;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PawnsAndGuns.Game
{
    public class GameboardController : MonoBehaviour
    {
        public static GameboardController Instance;
        private void Awake()
        {
            Instance = this;

            new PlayerController(Gameboard.Instance.PlayerTeam);
            new AIController(Gameboard.Instance.EnemyTeam);
        }
        private void Update()
        {
            List<Controller> controllers = Controller.GetAllControllers();
            for (int i = 0; i < controllers.Count; i++)
            {
                controllers[i].UpdateMovement();
            }

            if (CheckPointCell.LastCheckPoint != null && Gameboard.Instance.King == null)
            {
                CheckPointCell.LastCheckPoint.SpawnKing();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}