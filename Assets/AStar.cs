using System.Collections.Generic;
using UnityEngine;

public static class AStar {
    private class Node {
        public readonly float distance;
        public readonly float moved;
        public readonly float sum;
        public readonly Vector2Int position;
        public readonly Node before;
        public Node(Vector2Int position, Vector2Int dest, Node before = default, float moved = 0) {
            this.position = position;
            distance = (position - dest).magnitude;
            this.moved = moved;
            sum = distance + moved;
            this.before = before;
        }
    }

    private class NodeComparer : IComparer<Node> {
        int IComparer<Node>.Compare(Node x, Node y) {
            return x.sum.CompareTo(y.sum);//TODO implement awesome compare logic
        }
    }

    public static readonly int[] dx = new[] { -1, 0, 1, -1, 1, -1, 0, 1 };
    public static readonly int[] dy = new[] { -1, -1, -1, 0, 0, 1, 1, 1 };

    public static Stack<Vector2Int> Find(Vector2Int from, Vector2Int to) {
        var start = new Node(from, to);
        var open = new SortedSet<Node>(new NodeComparer()) { start };
        var closed = new HashSet<Vector2Int>();
        var ret = new Stack<Vector2Int>();
        var reverse = new Queue<Vector2Int>();
        var diagonal = Mathf.Sqrt(2);
        while (open.Count > 0) {
            var current = open.Min;
            if (current.position == to) {
                reverse.Enqueue(current.position);
                var before = current.before;
                while (before != null) {
                    reverse.Enqueue(before.position);
                    before = before.before;
                }
                break;
            }
            open.Remove(current);
            closed.Add(current.position);

            for (var i = 0; i < dx.Length; ++i) {
                var nextPosition = new Vector2Int(dx[i], dy[i]) + current.position;
                if (GridField.IsMovable(nextPosition, true) == false || closed.Contains(nextPosition)) {//TODO IsMovable Ignore Occupy
                    continue;
                }
                var next = new Node(nextPosition, to, current, current.moved + (dx[i] * dy[i] == 0 ? 1 : diagonal));
                open.Add(next);
            }
        }
        while (reverse.Count > 0) {
            ret.Push(reverse.Dequeue());
        }
        if(ret.Count > 0) {
            ret.Pop();// remove 1st(current)
        }
        return ret;
    }
}
