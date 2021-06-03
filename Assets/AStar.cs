using System.Collections.Generic;
using UnityEngine;

public static class AStar {
    private struct Node {
        public readonly float distance;
        public readonly float moved;
        public readonly float sum;
        public Node(float distance, float moved = 0) {
            this.distance = distance;
            this.moved = moved;
            sum = distance + moved;
        }
    }


    public static Queue<Vector2Int> Find(Vector2Int from, Vector2Int to) {
        var start = new Node((to - from).magnitude);
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
