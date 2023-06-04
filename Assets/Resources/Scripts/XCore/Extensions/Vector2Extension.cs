using UnityEngine;

namespace XCore.Extensions
{
    public static class Vector2Extensions
    {
        public static Vector2 SetX(this ref Vector2 vector, float x) {  return new Vector2(x, vector.x); }

        public static Vector2 SetY(this ref Vector2 vector, float y) {  return new Vector2(y, vector.y); }


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