using UnityEngine;

namespace PawnsAndGuns.Controllers
{
    public interface Controller
    {

        public delegate void HandleController(Controller controller);
        public static event HandleController OnControllerDone;

        public Color Team { get; set; }
        public bool CanMove { get; set; }
        public void SelectedToMove(int movePoints);

        public void UpdateMovement();
        public void FinishMove();

        public static void FinishController(Controller controller)
        {
            OnControllerDone?.Invoke(controller);
        }
    }
}