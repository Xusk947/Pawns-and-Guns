using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace PawnsAndGuns.Game.Pawns
{
    [CreateAssetMenu(fileName = "Pawn", menuName = "PawnAndGuns/PawnSO", order = 1)]
    public class PawnSO : ScriptableObject
    {
        public List<MoveWay> Moves = new List<MoveWay>();
        public Sprite Sprite;
    }

    [Serializable]
    public class MoveWay
    {
        public Vector2Int Way;
        public bool CanEatOnly;
        [Description("Leave -1 for inf")]
        public int Length;
    }
}