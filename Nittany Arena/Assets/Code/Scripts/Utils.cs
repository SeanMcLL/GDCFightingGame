using UnityEngine;

public static class Utils
{
    public static T GetOrAddComponent<T>(this GameObject self) where T : Component
    {
        T component = self.GetComponent<T>();
        return component != null ? component : self.AddComponent<T>() as T;
    }

    public static Vector2 ClosestCardinalDirection(this Vector2 self)
    {
        Vector2[] cardinalDirections =
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };

        Vector2 normalized = self.normalized;
        Vector2 cardinalDirection = Vector2.zero;
        float closestDot = Mathf.NegativeInfinity;

        for (int i = 0; i < cardinalDirections.Length; i++)
        {
            float dot = Vector2.Dot(normalized, cardinalDirections[i]);

            if (dot == 1.0f)
                return cardinalDirections[i];

            if (closestDot < dot)
            {
                closestDot = dot;
                cardinalDirection = cardinalDirections[i];
            }
        }

        return cardinalDirection;
    }
}
