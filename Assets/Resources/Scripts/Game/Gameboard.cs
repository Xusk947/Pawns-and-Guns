using PawnsAndGuns.Pawns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PawnsAndGuns.Game
{
    public class Gameboard : MonoBehaviour
    {
        public static Vector2Int GAME_BOARD_SIZE = new Vector2Int(128, 5);
        public static Gameboard Instance;

        public Sprite TileSprite;
        public Color WhiteTileColor, BlackTileColor;

        public Color PlayerTeam;
        public Color EnemyTeam;
        [SerializeField]
        public List<Vector2Int> _cellsToSpawn = new List<Vector2Int>();

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
                    _cells[x, y] = SpawnCell(x, y);
                }
            }

            Pawn pawn = Instantiate(Content.King);
            pawn.Team = PlayerTeam;
            _cells[2, 2].Pawn = pawn;
            FollowCamera.Instance.transform.position = pawn.transform.position - new Vector3(0, 0, 10);
        }

        private Cell SpawnCell(int x, int y)
        {
            GameObject gameObject = new GameObject($"Cell({x}:{y})");
            gameObject.transform.SetParent(transform);
            gameObject.transform.position = new Vector2(x, y);

            Cell cell = gameObject.AddComponent<Cell>();
            cell.SpriteRenderer.sprite = TileSprite;
            bool isWhite = (x + y) % 2 == 0;
            cell.SpriteRenderer.material.color = isWhite ? WhiteTileColor : BlackTileColor;
            cell.SpriteRenderer.material.color -= new Color(0, 0, 0, .2f);
            cell.x = x;
            cell.y = y;

            return cell;
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
            SpawnBoard();
            gameObject.AddComponent<GameboardController>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Vector2Int mousePosition = MouseTile;
            Gizmos.DrawWireCube(new Vector3(mousePosition.x, mousePosition.y), new Vector3(1, 1, 1));

            for (int i = 0; i < _cellsToSpawn.Count; i++)
            {
                Vector2Int pos = _cellsToSpawn[i];
                bool isWhite = (pos.x + pos.y) % 2 == 0;
                Gizmos.color = isWhite ? Color.white : Color.black;
                Gizmos.DrawWireCube(new Vector3(pos.x, pos.y), new Vector3(1, 1, 1));
            }
        }
    }
}
