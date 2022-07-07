using System.Collections.Generic;
using UnityEngine;

public class PathFinder
{
    private Queue<GridPositionHandle> currentPath = new Queue<GridPositionHandle>();

    public bool TryGetNextTile(out Vector2Int ret)
    {
        if(IsRemainPath)
        {
            ret = currentPath.Dequeue().WorldPosition;
            return true;
        }

        ret = Vector2Int.zero;
        return false;
    }

    public void OnDrawGizmos(Vector3 pos, float z)
    {
        if (currentPath.Count == 0)
        {
            return;
        }

        Gizmos.color = Color.white;
        pos.z = z;
        foreach (var next in currentPath)
        {
            var nextV3 = (Vector3)GridField.Convert(next);
            nextV3.z = z;
            Gizmos.DrawLine(pos, nextV3);
            pos = nextV3;
        }
    }

    public void ResetPath(Vector2Int current, Vector2Int next)
    {
        currentPath.Clear();

        var newPath = AStar.Find(current, next);
        while (newPath.Count > 0)
        {
            currentPath.Enqueue(new GridPositionHandle(newPath.Pop()));
        }
    }

    public bool IsRemainPath => currentPath.Count > 0;
}
