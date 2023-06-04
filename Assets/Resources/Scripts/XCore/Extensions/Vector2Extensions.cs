using UnityEngine;

namespace XCore.Extensions
{
    public static class Vector2Extensions
    {
        public static Vector2 SetX(this ref Vector2 vector, float x) {  return new Vector2(x, vector.x); }

        public static Vector2 SetY(this ref Vector2 vector, float y) {  return new Vector2(y, vector.y); }

        public static float Distance(this Vector2 vector, Vector2 other)
        {
            return Vector2.Distance(vector, other);
        }

        public static float SquareDistance(this Vector2 vector, Vector2 other)
        {
            Vector2 difference = vector - other;
            return difference.sqrMagnitude;
        }

        public static float Distance(this Vector2Int vector, Vector2Int other)
        {
            return Vector2Int.Distance(vector, other);
        }

        public static float SquareDistance(this Vector2Int vector, Vector2Int other)
        {
            Vector2Int difference = vector - other;
            return difference.sqrMagnitude;
        }

        public static float SquareDistance(this Vector2Int vector, int x, int y)
        {
            Vector2Int difference = vector - new Vector2Int(x, y);
            return difference.sqrMagnitude;
        }

        public static Vector2 Random(float scale = 1f)
        {
            return new Vector2(UnityEngine.Random.Range(-scale, scale), UnityEngine.Random.Range(-scale, scale));
        }

        public static Vector2 GetVector(this float angle)
        {
            float angleRad = angle * Mathf.PI / 180;
            return new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }
    }
}