using PawnsAndGuns.Game.Pawns;
using UnityEngine;

namespace PawnsAndGuns.Game.Cells
{
    public class WallCell : Cell
    {

        protected override void Awake()
        {
            SpriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            SpriteRenderer.sprite = Content.WallSprite;
        }
        public override bool CanAcceptPawn(Pawn pawn)
        {
            return false;
        }
    }
}