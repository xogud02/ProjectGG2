using System.Collections.Generic;
using UnityEngine;

public class PathFinder
{
    private Queue<Vector2Int> currentPath = new Queue<Vector2Int>();

    public bool TryGetNextTile(out Vector2Int ret)
    {
        if(IsRemainPath)
        {
            ret = currentPath.Dequeue();
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

    public void SetPath(Vector2Int current, Vector2Int next)
    {
        var newPath = AStar.Find(current, next);
        while (newPath.Count > 0)
        {
            currentPath.Enqueue(newPath.Pop());
        }
    }

    public void EmptyPath(bool remainFirst)
    {
        if(IsRemainPath == false)
        {
            return;
        }

        var first = currentPath.Peek();
        currentPath.Clear();

        if(remainFirst == false)
        {
            currentPath.Enqueue(first);
        }
    }


    public bool IsRemainPath => currentPath.Count > 0;
}
