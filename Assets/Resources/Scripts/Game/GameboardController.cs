using PawnsAndGuns.Controllers;
using PawnsAndGuns.Game.Cells;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XCore;

namespace PawnsAndGuns.Game
{
    public class GameboardController : MonoBehaviour
    {
        public static GameboardController Instance;
        private void Awake()
        {
            Instance = this;
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
                GameSettings.Save();
            }
        }

        private void OnDrawGizmos()
        {
            List<Controller> controllers = Controller.GetAllControllers();
            for (int i = 0; i < controllers.Count; i++)
            {
                if (controllers[i] is AIController)
                {
                    (controllers[i] as AIController).OnDrawGizmos();
                }
            }
        }
    }
}