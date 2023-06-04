using UnityEngine;

namespace XCore.Extensions
{
	public static class Vector3Extensions
	{
		public static Vector3 SetX(this ref Vector3 vector, float x) { return new Vector3(x, vector.y, vector.z); }

		public static Vector3 SetY(this ref Vector3 vector, float y) { return new Vector3(vector.x, y, vector.z); }

		public static Vector3 SetZ(this ref Vector3 vector, float z) { return new Vector3(vector.x, vector.y, z); }

        public static float Distance(this Vector3 vector, Vector3 other)
        {
            return Vector3.Distance(vector, other);
        }

        public static float SquareDistance(this Vector3 vector, Vector3 other)
        {
            Vector3 difference = vector - other;
            return difference.sqrMagnitude;
        }
    }
}