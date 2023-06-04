using PawnsAndGuns.Game.Cells;
using PawnsAndGuns.Game.Pawns;
using System;
using UnityEngine;
using XCore;

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

        public void SetPawn(int x, int y, Color team, Pawn Instance, string name = "Pawn")
        {
            Cell cell = GetCell(x, y);
            if (cell != null)
            {
                Pawn pawn = Instantiate(Instance);
                pawn.Team = team;

                cell.Pawn = pawn;
            }
        }

        public T SetCell<T>(int x, int y) where T : Cell
        {
            Vector2Int chunkPos = new Vector2Int(Mathf.FloorToInt(x / (float)Chunk.CHUNK_SIZE.x), Mathf.FloorToInt(y / (float)Chunk.CHUNK_SIZE.y));
            Chunk chunk = Chunk.GetChunk(chunkPos);
            Vector2Int tilePos = new Vector2Int((x % Chunk.CHUNK_SIZE.x + Chunk.CHUNK_SIZE.x) % Chunk.CHUNK_SIZE.x, (y % Chunk.CHUNK_SIZE.y + Chunk.CHUNK_SIZE.y) % Chunk.CHUNK_SIZE.y);

            GameObject gameObject = new GameObject($"Cell({tilePos.x}:{tilePos.y})");
            gameObject.isStatic = true;

            T cell = gameObject.AddComponent<T>();


            bool isWhite = (x + y) % 2 == 0;
            cell.SpriteRenderer.material.color = isWhite ? WhiteTileColor : BlackTileColor;

            cell.SpriteRenderer.material.color -= new Color(0, 0, 0, .5f);

            chunk.SetCell(tilePos.x, tilePos.y, cell);

            return cell;
        }

        public void RemoveCell(int x, int y)
        {
            // Convert x, y  to tile and chunk positions
            Vector2Int chunkPos = new Vector2Int(Mathf.FloorToInt(x / (float)Chunk.CHUNK_SIZE.x), Mathf.FloorToInt(y / (float)Chunk.CHUNK_SIZE.y));
            Chunk chunk = Chunk.GetChunk(chunkPos);
            Vector2Int tilePos = new Vector2Int((x % Chunk.CHUNK_SIZE.x + Chunk.CHUNK_SIZE.x) % Chunk.CHUNK_SIZE.x, (y % Chunk.CHUNK_SIZE.y + Chunk.CHUNK_SIZE.y) % Chunk.CHUNK_SIZE.y);

            Cell cell = chunk.GetCell(tilePos.x, tilePos.y);
            // Check if chunk have a cell on x, y coordinates
            if (cell == null) return;
            // Destroy Pawn on a Cell tile
            if (cell.Pawn != null) cell.Pawn.Kill();
            // Set null value to cell at x, y
            Destroy(cell.gameObject);
        }

        public Cell GetCell(int x, int y)
        {
            Vector2Int chunkPos = new Vector2Int(Mathf.FloorToInt(x / (float)Chunk.CHUNK_SIZE.x), Mathf.FloorToInt(y / (float)Chunk.CHUNK_SIZE.y));
            Chunk chunk = Chunk.GetChunk(chunkPos);
            Vector2Int tilePos = new Vector2Int((x % Chunk.CHUNK_SIZE.x + Chunk.CHUNK_SIZE.x) % Chunk.CHUNK_SIZE.x, (y % Chunk.CHUNK_SIZE.y + Chunk.CHUNK_SIZE.y) % Chunk.CHUNK_SIZE.y);
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
