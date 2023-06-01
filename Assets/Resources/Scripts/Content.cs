using PawnsAndGuns.Game.Pawns;
using TMPro;
using UnityEngine;

namespace PawnsAndGuns
{
    public static class Content
    {
        public static ParticleSystem PawnDeath { get; private set; }
        public static Sprite Highlight { get; private set; }
        public static Sprite WallSprite { get; private set; }
        public static Sprite TileSprite { get; private set; }

        public static TextMeshProUGUI TextTutorial { get; private set; }
        public static TextMeshProUGUI TextMoveAlongTheBoard { get; private set; }
        public static TextMeshProUGUI TextPawnAssign { get; private set; }
        public static TextMeshProUGUI TextCheckPoint { get; private set; }
        public static TextMeshProUGUI TextPawnKill { get; private set; }

        public static AudioClip AudioClipMove { get; private set; }
        public static AudioClip AudioClipKill { get; private set; }
        public static AudioClip TextAppear { get; private set; }


        public static Pawn Pawn, Rook, Knight, Bishop, Queen, King;
        public static void Load()
        {
            PawnDeath = Resources.Load<ParticleSystem>("Content/FX/PawnDeath");
            Highlight = Resources.Load<Sprite>("Sprites/chess/highlight");

            Pawn = Resources.Load<Pawn>("Content/Pawn");
            Rook = Resources.Load<Pawn>("Content/Rook");
            Knight = Resources.Load<Pawn>("Content/Knight");
            Bishop = Resources.Load<Pawn>("Content/Bishop");
            Queen = Resources.Load<Pawn>("Content/Queen");
            King = Resources.Load<Pawn>("Content/King");

            Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/chess/chess-board");
            TileSprite = sprites[0];
            WallSprite = sprites[1];

            TextTutorial = Resources.Load<TextMeshProUGUI>("Content/Texts/Controls");
            TextMoveAlongTheBoard = Resources.Load<TextMeshProUGUI>("Content/Texts/MoveAlongTheBoard");
            TextPawnAssign = Resources.Load<TextMeshProUGUI>("Content/Texts/HowToAssignPawns");
            TextCheckPoint = Resources.Load<TextMeshProUGUI>("Content/Texts/CheckPoint");
            TextPawnKill = Resources.Load<TextMeshProUGUI>("Content/Texts/PawnKill");

            AudioClipKill = Resources.Load<AudioClip>("Content/Sounds/explosion");
            AudioClipMove = Resources.Load<AudioClip>("Content/Sounds/move");
            TextAppear = Resources.Load<AudioClip>("Content/Sounds/text-appear");
        }
    }
}