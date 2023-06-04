using PawnsAndGuns.Game.Pawns;
using UnityEngine;

namespace PawnsAndGuns.Game.Cells
{
    public class TriggerCell : Cell
    {
        public delegate void OnPawnMove(Pawn pawn);

        public OnPawnMove pawnMoveInside, pawnMoveOutside;
        public Color ReactsOn = default;
        protected override void SetPawn(Pawn pawn)
        {
            base.SetPawn(pawn);

            if (ReactsOn != default && pawn != null && !ReactsOn.Equals(pawn.Team)) return;

            if (pawn != null)
            {
                pawnMoveInside?.Invoke(pawn);
            } else
            {
                pawnMoveOutside?.Invoke(pawn);
            }
        }
    }
}