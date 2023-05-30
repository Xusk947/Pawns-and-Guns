using UnityEngine;

namespace PawnsAndGuns
{
    public static class Content
    {
        public static ParticleSystem PawnDeath { get; private set; }
        public static void Load()
        {
            PawnDeath = Resources.Load<ParticleSystem>("Content/FX/PawnDeath");
        }
    }
}