using System.Collections.Generic;
using UnityEngine;

public static class AStar {
    private struct Node {
        public readonly float distance;
        public readonly float moved;
        public readonly float sum;
        public readonly Vector2Int position;
        public Node(Vector2Int position, Vector2Int dest, float moved = 0) {
            this.position = position;
            distance = (position-dest).magnitude;
            this.moved = moved;
            sum = distance + moved;
        }
    }


    public static Queue<Vector2Int> Find(Vector2Int from, Vector2Int to) {
        var start = new Node(from, to);
        var open = new SortedSet<Node>(Comparer<Node>.Create((node1, node2) => (int)(node1.sum- node2.sum))) { start };
        var closed = new HashSet<Node>();
        var ret = new Queue<Vector2Int>();
        while (open.Count > 0) {
            var current = open.Min;
            open.Remove(current);
        }
        return ret;
    }
}
