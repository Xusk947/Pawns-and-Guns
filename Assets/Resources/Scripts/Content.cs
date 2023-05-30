using PawnsAndGuns.Pawns;
using UnityEngine;

namespace PawnsAndGuns
{
    public static class Content
    {
        public static ParticleSystem PawnDeath { get; private set; }
        public static Sprite Highlight { get; private set; }

        public static Pawn Bishop, King;
        public static void Load()
        {
            PawnDeath = Resources.Load<ParticleSystem>("Content/FX/PawnDeath");
            Highlight = Resources.Load<Sprite>("Sprites/highlight");

            Bishop = Resources.Load<Pawn>("Content/Pawn");
            King = Resources.Load<Pawn>("Content/KingPawn");
        }
    }
}