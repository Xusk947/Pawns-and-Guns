using System.Collections.Generic;
using UnityEngine;

namespace PawnsAndGuns.Game.Cells
{ 
    public class Chunk : MonoBehaviour
    {
        public static Dictionary<Vector2Int, Chunk> Chunks = new Dictionary<Vector2Int, Chunk>();
        public static Vector2Int CHUNK_SIZE = new Vector2Int(16, 16);

        /**
         * Cell tiles 
         */
        public Cell[,] Cells { get; private set; }
        private Vector2Int _position;

        public int x { get { return _position.x; } }
        public int y { get { return _position.y; } }

        private void Awake()
        {
            Cells = new Cell[CHUNK_SIZE.x, CHUNK_SIZE.y];
        }
        
        public Cell GetCell(int x, int y)
        {
            if (!InBounds(x, y)) return null;
            return Cells[x, y];
        }

        public void SetCell(int cellX, int cellY, Cell cell)
        {
            if (!InBounds(cellX, cellY)) return;
            if (Cells[cellX, cellY] != null)
            {
                Destroy(Cells[cellX, cellY].gameObject);
                Cells[cellX, cellY] = null;
            }
            // Destroy cell if cell value is null else place a new cell
            if (cell == null) return;
            cell.transform.SetParent(transform);
            cell.transform.position = transform.position + new Vector3(cellX, cellY);
            cell.x = cellX;
            cell.y = cellY;
            cell.Chunk = this;
            Cells[cellX, cellY] = cell;
        }
        public void SetCell<T>(int cellX, int cellY) where T : Cell
        {
            if (!InBounds(cellX, cellY)) return;
            if (Cells[cellX, cellY] != null)
            {
                Destroy(Cells[cellX, cellY]);
            }

            T cell = new GameObject($"Cell({cellX}:{cellY})").AddComponent<T>();
            cell.transform.SetParent(transform);
            cell.transform.position = transform.position + new Vector3(cellX, cellY);
            cell.x = cellX;
            cell.y = cellY;
            cell.Chunk = this;
            Cells[cellX, cellY] = cell;
        }

        public static Chunk GetChunk(int chunkX, int chunkY)
        {
            return GetChunk(new Vector2Int(chunkX, chunkY));
        }

        public static Chunk GetChunk(Vector2Int position)
        {
            if (Chunks.ContainsKey(position))
            {
                return Chunks[position];
            }
            else
            {
                Chunk chunk = new GameObject($"Chunk({position.x}:{position.y})").AddComponent<Chunk>();
                chunk._position = new Vector2Int(position.x, position.y);
                chunk.transform.SetParent(Gameboard.Instance.transform);
                chunk.transform.position = new Vector3(position.x * CHUNK_SIZE.x, position.y * CHUNK_SIZE.y);
                Chunks[position] = chunk;
                return chunk;
            }
        }
        private bool InBounds(int x, int y)
        {
            if (x < 0 || y < 0 || x >= CHUNK_SIZE.x || y >= CHUNK_SIZE.y) return false;
            return true;
        }

        private void OnDestroy()
        {
            Chunks.Remove(_position);
        }

    }
}