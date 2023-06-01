using PawnsAndGuns.Game.Cells;
using PawnsAndGuns.Game.Pawns;
using System;
using UnityEngine;

namespace PawnsAndGuns.Game
{
    public class Gameboard : MonoBehaviour
    {
        public static Gameboard Instance;

        public Color WhiteTileColor, BlackTileColor;

        public Color PlayerTeam;
        public Color EnemyTeam;
        public Color NeutralTeam;

        public Pawn King;
        public Canvas Canvas;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                print(GetCell(MouseTile.x, MouseTile.y));
            }
        }

        private void SpawnCell<T>(int x, int y) where T : Cell
        {
            Vector2Int chunkPos = new Vector2Int(Mathf.FloorToInt(x / (float)Chunk.CHUNK_SIZE.x), Mathf.FloorToInt(y / (float)Chunk.CHUNK_SIZE.y));
            Chunk chunk = Chunk.GetChunk(chunkPos);
            Vector2Int tilePos = new Vector2Int((x % Chunk.CHUNK_SIZE.x + Chunk.CHUNK_SIZE.x) % Chunk.CHUNK_SIZE.x, (y % Chunk.CHUNK_SIZE.y + Chunk.CHUNK_SIZE.y) % Chunk.CHUNK_SIZE.y);

            GameObject gameObject = new GameObject($"Cell({tilePos.x}:{tilePos.y})");
            gameObject.isStatic = true;

            T cell = gameObject.AddComponent<T>();

            bool isWhite = (x + y) % 2 == 0;
            cell.SpriteRenderer.material.color = isWhite ? WhiteTileColor : BlackTileColor;
            cell.SpriteRenderer.material.color -= new Color(0, 0, 0, .2f);

            chunk.SetCell(tilePos.x, tilePos.y, cell);
        }

        public Cell GetCell(int x, int y)
        {
            Vector2Int chunkPos = new Vector2Int(Mathf.FloorToInt(x / (float)Chunk.CHUNK_SIZE.x), Mathf.FloorToInt(y / (float)Chunk.CHUNK_SIZE.y));
            Chunk chunk = Chunk.GetChunk(chunkPos);
            Vector2Int tilePos = new Vector2Int((x % Chunk.CHUNK_SIZE.x + Chunk.CHUNK_SIZE.x) % Chunk.CHUNK_SIZE.x, (y % Chunk.CHUNK_SIZE.y + Chunk.CHUNK_SIZE.y) % Chunk.CHUNK_SIZE.y);
            print(tilePos.x + " : " + tilePos.y);
            return chunk.GetCell(tilePos.x, tilePos.y);
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

        public static Vector2Int WorldToChunkPosition(float x, float y)
        {
            int chunkPosX = Mathf.FloorToInt(x / Chunk.CHUNK_SIZE.x);
            int chunkPosY = Mathf.FloorToInt(y / Chunk.CHUNK_SIZE.y);
            return new Vector2Int(chunkPosX, chunkPosY);
        }

        private void Awake()
        {
            Instance = this;

            Content.Load();

            int width = 16;
            int height = 16;

            for (int x = -width; x < width; x++)
            {
                for (int y = -height; y < height; y++)
                {
                    SpawnCell<Cell>(x, y);
                }
            }
            SpawnCell<CheckPointCell>(0, 0);
        }

        private void Start()
        {
            gameObject.AddComponent<GameboardController>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Vector2Int mousePosition = MouseTile;
            Gizmos.DrawWireCube(new Vector3(mousePosition.x, mousePosition.y), new Vector3(1, 1, 1));
            Vector2Int chunkPosition = WorldToChunkPosition(mousePosition.x, mousePosition.y);
            Gizmos.DrawWireCube(new Vector3(Chunk.CHUNK_SIZE.x * chunkPosition.x + Chunk.CHUNK_SIZE.x / 2f - .5f, Chunk.CHUNK_SIZE.x * chunkPosition.y + Chunk.CHUNK_SIZE.y / 2f - .5f, 0), new Vector3(Chunk.CHUNK_SIZE.x, Chunk.CHUNK_SIZE.y));
        }
    }
}
