using UnityEngine;

public class GridPositionHandle
{
    public Vector2Int WorldPosition => LocalPosition + Delta;
    public Vector2Int LocalPosition;
    public Vector2Int Delta;

    public GridPositionHandle(Vector2Int localPosition = default, Vector2Int delta = default)
    {
        LocalPosition = localPosition;
        Delta = delta;
    }
}
