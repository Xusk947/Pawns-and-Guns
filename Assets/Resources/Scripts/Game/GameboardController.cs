using PawnsAndGuns.Controllers;
using System.Collections.Generic;
using UnityEngine;

namespace PawnsAndGuns.Game
{
    public class GameboardController : MonoBehaviour
    {
        public static GameboardController Instance;
        
        public Controller CurrentController { get => _currentController; }

        private List<Controller> _controllers;
        private Controller _currentController;

        private void Awake()
        {
            Instance = this;

            _controllers = new List<Controller>()
            {
                new PlayerController(Gameboard.Instance.PlayerTeam)
            };

            _currentController = _controllers[0];
            
            Controller.OnControllerDone += ControllerFinished;
        }

        
        private void ControllerFinished(Controller controller)
        {
            int index = _controllers.IndexOf(controller);
            if (index + 1 >= _controllers.Count)
            {
                _currentController = _controllers[0];
            } else
            {
                _currentController = _controllers[index + 1];
            }
        }

        private void Update()
        {
            if (_currentController != null) _currentController.UpdateMovement();
        }

        private void OnDestroy()
        {
            Controller.OnControllerDone -= ControllerFinished;
        }
    }
}