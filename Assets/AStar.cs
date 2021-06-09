using System.Collections.Generic;
using UnityEngine;

public static class AStar {
    private struct Node {
        public readonly float distance;
        public readonly float moved;
        public readonly float sum;
        public readonly Vector2Int position;
        public readonly Node? before;
        public Node(Vector2Int position, Vector2Int dest, Node before = default, float moved = 0) {
            this.position = position;
            distance = (position - dest).magnitude;
            this.moved = moved;
            sum = distance + moved;
            this.before = before;
        }
    }


    public static Queue<Vector2Int> Find(Vector2Int from, Vector2Int to) {
        var start = new Node(from, to);
        var open = new SortedSet<Node>(Comparer<Node>.Create((node1, node2) => (int)(node1.sum - node2.sum))) { start };
        var closed = new HashSet<Vector2Int>();
        var ret = new Queue<Vector2Int>();
        var dx = new[] { -1, 0, 1, -1, 0, 1, -1, 0, 1 };
        var dy = new[] { -1, -1, -1, 0, 0, 0, 1, 1, 1 };
        while (open.Count > 0) {
            var current = open.Min;
            open.Remove(current);
            if(current.position == to) {
                break;
            }
            for(var i = 0; i < dx.Length; ++i) {
                var next = new Vector2Int(dx[i], dy[i]) + current.position;
                if (closed.Contains(next)) {
                    continue;
                }
            }
        }
        return ret;
    }
}
