using UnityEngine;

public static class Extensions
{
    public static bool IsPositionValid(this Transform transform)
    {
        Vector3 min = BulletBoundary.Instance.Min;
        Vector3 max = BulletBoundary.Instance.Max;
        bool isNotValid = transform.position.x < min.x || transform.position.y < min.y ||
            transform.position.x > max.x || transform.position.y > max.y;
        return !isNotValid;
    }
}
