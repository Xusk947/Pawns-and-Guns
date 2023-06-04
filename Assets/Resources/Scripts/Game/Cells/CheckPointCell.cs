using PawnsAndGuns.Game.Pawns;

namespace PawnsAndGuns.Game.Cells
{
    public class CheckPointCell : Cell
    {
        public static CheckPointCell LastCheckPoint;

        protected override void Awake()
        {
            base.Awake();
            LastCheckPoint = this;
        }
        public void SpawnKing()
        {
            if (!Spawned) return;
            Pawn king = Instantiate(Content.King);
            king.Team = Gameboard.Instance.PlayerTeam;
            king.name = "King";
            Gameboard.Instance.King = king;
            Pawn = king;

            FollowCamera.Instance.target = king.gameObject;
        }

        public override bool CanAcceptPawn(Pawn pawn)
        {
            return pawn == Gameboard.Instance.King;
        }

        protected override void SetPawn(Pawn pawn)
        {
            base.SetPawn(pawn);
            if (pawn == null) return;
            if (pawn == Gameboard.Instance.King) LastCheckPoint = this;
        }
    }
}