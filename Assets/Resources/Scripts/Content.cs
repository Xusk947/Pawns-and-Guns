using PawnsAndGuns.Game.Pawns;
using System.Collections.Generic;
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
        public static TextMeshProUGUI TextWalls { get; private set; }
        public static TextMeshProUGUI TextSpaceMove { get; private set; }
        public static TextMeshProUGUI TextCheckPoint { get; private set; }
        public static TextMeshProUGUI TextPawnKill { get; private set; }
        public static TextMeshProUGUI TutorialFinished { get; private set; }

        public static AudioClip AudioClipMove { get; private set; }
        public static AudioClip AudioClipKill { get; private set; }
        public static AudioClip TextAppear { get; private set; }


        public static Pawn Pawn, Rook, Knight, Bishop, Queen, King;
        public static List<Pawn> playablePawns;
        public static List<Pawn> easyPawns;
        public static List<Pawn> normalPawns;
        public static List<Pawn> hardPawns;
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

            easyPawns = new List<Pawn>() { Bishop, Knight };
            normalPawns = new List<Pawn>() { Rook, Bishop };
            hardPawns = new List<Pawn>() { Queen };

            playablePawns = new List<Pawn>() { Bishop, Knight, Rook, Bishop, Queen };

            Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/chess/chess-board");
            TileSprite = sprites[0];
            WallSprite = sprites[1];

            TextTutorial = Resources.Load<TextMeshProUGUI>("Content/Texts/Controls");
            TextWalls = Resources.Load<TextMeshProUGUI>("Content/Texts/TextWalls");
            TextSpaceMove = Resources.Load<TextMeshProUGUI>("Content/Texts/SpaceMove");
            TextCheckPoint = Resources.Load<TextMeshProUGUI>("Content/Texts/CheckPoint");
            TextPawnKill = Resources.Load<TextMeshProUGUI>("Content/Texts/PawnKill");
            TutorialFinished = Resources.Load<TextMeshProUGUI>("Content/Texts/TutorialFinished");

            AudioClipKill = Resources.Load<AudioClip>("Content/Sounds/explosion");
            AudioClipMove = Resources.Load<AudioClip>("Content/Sounds/move");
            TextAppear = Resources.Load<AudioClip>("Content/Sounds/text-appear");
        }
    }
}