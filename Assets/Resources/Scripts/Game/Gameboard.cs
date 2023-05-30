using PawnsAndGuns.Pawns;
using System.Collections;
using UnityEngine;

namespace PawnsAndGuns.Game
{
    public class Gameboard : MonoBehaviour
    {
        public static Vector2Int GAME_BOARD_SIZE = new Vector2Int(11, 17);
        public static Gameboard Instance;

        public Sprite WhiteSprite, BlackSprite;

        public Color PlayerTeam;
        public Color EnemyTeam;

        public Pawn Pawn;

        public Cell[,] Cells
        {
            get { return Cells; }
        }

        private Cell[,] _cells;

        private void SpawnBoard()
        {
            for (int x = 0; x < GAME_BOARD_SIZE.x; x++)
            {
                for (int y = 0; y < GAME_BOARD_SIZE.y; y++)
                {
                    GameObject gameObject = new GameObject($"Cell({x}:{y})");
                    gameObject.transform.SetParent(transform);
                    gameObject.transform.position = new Vector2(x, y);

                    Cell cell = gameObject.AddComponent<Cell>();
                    bool isWhite = (x + y) % 2 == 0;

                    cell.SpriteRenderer.sprite = isWhite ? WhiteSprite : BlackSprite;
                    cell.x = x;
                    cell.y = y;
                    _cells[x, y] = cell;
                }
            }
        }

        public static Vector2Int MouseTile
        {
            get
            {
                Vector3 mousePosition = Input.mousePosition;
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, -Camera.main.transform.position.z));

                // Convert the world position to a tile position
                int tileX = Mathf.FloorToInt(worldPosition.x + .5f);
                int tileY = Mathf.FloorToInt(worldPosition.y + .5f);

                return new Vector2Int(tileX, tileY);
            }
        }
        public static Vector2Int WorldToTilePosition(float x, float y)
        {
            return new Vector2Int(Mathf.FloorToInt(x + .5f), Mathf.FloorToInt(y + .5f));
        }
        public Cell GetCell(Vector2Int position)
        {
            return GetCell(position.x, position.y);
        }
        public Cell GetCell(int x, int y)
        {
            if (x < 0 || y < 0 || x >= GAME_BOARD_SIZE.x || y >= GAME_BOARD_SIZE.y) return null;
            return _cells[x, y];
        }
        private void Awake()
        {
            Instance = this;
            _cells = new Cell[GAME_BOARD_SIZE.x, GAME_BOARD_SIZE.y];
            Content.Load();
        }

        private void Start()
        {
            Camera.main.transform.position = new Vector3(GAME_BOARD_SIZE.x / 2, GAME_BOARD_SIZE.y / 2, -10);
            SpawnBoard();
            gameObject.AddComponent<GameboardController>();

            Pawn pawn = Instantiate(Pawn);
            pawn.Team = PlayerTeam;
            GetCell(1, 8).Pawn = pawn;

            Pawn pawn1 = Instantiate(Pawn);
            pawn1.Team = PlayerTeam;
            GetCell(1, 9).Pawn = pawn1;
            for (int i =0; i < 10; i++)
            {
                Pawn pawn2 = Instantiate(Pawn);
                pawn2.Team = EnemyTeam;
                GetCell(1 + i, 10).Pawn = pawn2;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Vector2Int mousePosition = MouseTile;
            Gizmos.DrawWireCube(new Vector3(mousePosition.x, mousePosition.y), new Vector3(1, 1, 1));
        }
    }
}
