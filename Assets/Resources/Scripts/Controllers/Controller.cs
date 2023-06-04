using System.Collections.Generic;
using UnityEngine;

namespace PawnsAndGuns.Controllers
{
    public interface Controller
    {
        private static Dictionary<Color, Controller> Controllers = new Dictionary<Color, Controller>();
        private static List<Controller> AllControllers = new List<Controller>();
        public Color Team { get; set; }
        public bool CanMove { get; set; }
        public void UpdateMovement();

        public static void SetController(Controller controller)
        {
            Controllers[controller.Team] = controller;
            AllControllers.Add(controller);
        }

        public static void RemoveController(Controller controller)
        {
            Controllers.Remove(controller.Team);
            AllControllers.Remove(controller);
        }

        public static Controller GetController(Color team)
        {
            if (!Controllers.ContainsKey(team)) return null;
            return Controllers[team];
        }

        public static List<Controller> GetAllControllers()
        {
            return AllControllers;
        }
    }
}