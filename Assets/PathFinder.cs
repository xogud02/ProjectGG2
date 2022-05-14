using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder
{
    private Queue<Vector2Int> currentPath = new Queue<Vector2Int>();

    public bool TryGetNextTile(out Vector2Int ret)
    {
        ret = Vector2Int.zero;
        return false;
    }

    public void OnDrawGizmos(Vector3 current, float z)
    {
        if (currentPath.Count == 0)
        {
            return;
        }

        Gizmos.color = Color.white;
        current.z = z;
        foreach (var next in currentPath)
        {
            var nextV3 = (Vector3)GridField.Convert(next);
            nextV3.z = z;
            Gizmos.DrawLine(current, nextV3);
            current = nextV3;
        }
    }
}
